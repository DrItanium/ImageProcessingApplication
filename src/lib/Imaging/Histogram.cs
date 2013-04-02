using System;
using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace Libraries.Imaging 
{
  public class ColorHistogram 
  {
    //take a blow up in space to have verified code
    private int width, height;
    private Histogram red, green, blue;
    public Histogram Red { get { return red; } } 
    public Histogram Green { get { return green; } } 
    public Histogram Blue { get { return blue; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public ColorHistogram(int[][] image) 
    {
      width = image.Length;
      height = image[0].Length;
      byte[][] r = new byte[width][];
      byte[][] g = new byte[width][];
      byte[][] b = new byte[width][];
      for(int i = 0; i < width; i++)
      {
        byte[] rLine = new byte[height];
        byte[] gLine = new byte[height];
        byte[] bLine = new byte[height];
        int[] line = image[i];
        for(int j = 0; j < height; j++)
        {
          Color c = Color.FromArgb(line[j]);
          rLine[j] = (byte)c.R;
          gLine[j] = (byte)c.G;
          bLine[j] = (byte)c.B;
        }
        r[i] = rLine;
        g[i] = gLine;
        b[i] = bLine;
      }
      red = new Histogram(r);
      green = new Histogram(g);
      blue = new Histogram(b);
    }
    public ColorHistogram(int width, int height)
    {
      this.width = width;
      this.height = height;
      red = new Histogram(width, height);
      blue = new Histogram(width, height);
      green = new Histogram(width, height);
    }
    public void Repurpose(IEnumerable<int> elements) 
    {
      var decomp = Decompose(elements);
      red.Repurpose(decomp.Item1);
      green.Repurpose(decomp.Item2);
      blue.Repurpose(decomp.Item3);
    }
    private static Tuple<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>> Decompose(IEnumerable<int> elements) 
    {
      List<byte> r = new List<byte>();
      List<byte> g = new List<byte>();
      List<byte> b = new List<byte>();
      foreach(var v in elements) 
      {
        Color c = Color.FromArgb(v);
        r.Add((byte)c.R);
        g.Add((byte)c.G);
        b.Add((byte)c.B);
      }
      return new Tuple<IEnumerable<byte>, IEnumerable<byte>, IEnumerable<byte>>(r, g, b);
    }
  }
  public class Histogram 
  {
    //8-bit histogram
    public const int NUM_VALUES = 256;
    private long[] contents, totals;
    private byte[] globalEqualizedIntensity; 
    private double[] pk;
    private int width, height;
    private long totalPixelCount;
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public long PixelCount { get { return totalPixelCount; } }
    public byte[] GlobalEqualizedIntensity { get { return globalEqualizedIntensity; } }
    public double[] PK { get { return pk; } }
    public long this[byte intensity] { get { return contents[(int)intensity]; } }
    public long this[int intensity] { get { return contents[intensity]; } }
    public long this[int from, int to]
    {
      get
      {
        if(from == 0)
          return totals[to - 1];
        else if((to - from) == 1) //only one item
          return totals[from];
        else
        {
          long total = 0L;
          for(int i = from; i < to; i++)
            total += (long)this[i];
          return total;
        }
      }
    }
    private Histogram()
    {
      contents = new long[NUM_VALUES];
      totals = new long[NUM_VALUES];
      pk = new double[NUM_VALUES];
      globalEqualizedIntensity = new byte[NUM_VALUES];

    }
    public Histogram(int width, int height) 
      : this()
    {
      this.width = width;
      this.height = height;
      totalPixelCount = width * height;
    }
    public Histogram(IEnumerable<byte> data)
      : this()
    {
      foreach(var v in data)
      {
        contents[(int)v]++;
        totalPixelCount++;
      }
      SetupExtraneousData();
    }
    public Histogram(Bitmap bitmap)
      : this(bitmap.Width, bitmap.Height)
    {
      PerformActionAcrossTheImageAndSetup((i,j) => contents[bitmap.GetPixel(i,j).R]++);
    }
    private void PerformActionAcrossTheImageAndSetup(Action<int,int> body)
    {
      for(int i = 0; i < width; i++)
        for(int j = 0; j < height; j++)
          body(i,j);
      SetupExtraneousData();

    }
    public Histogram(byte[][] value)
      : this(value.Length, value[0].Length)
    {
      PerformActionAcrossTheImageAndSetup((x,y) => contents[(int)value[x][y]]++);
    }
    public Histogram(int width, int height, byte[,] value)
      : this(width, height)
    {
      PerformActionAcrossTheImageAndSetup((i,j) => contents[(int)value[i,j]]++);
    }	
    public void Repurpose(IEnumerable<byte> elements)
    {
      for(int i = 0; i < 256; i++)
        contents[i] = 0;
      totalPixelCount = 0;
      foreach(byte b in elements)
      {
        contents[(int)b]++;
        totalPixelCount++;
      }
      SetupExtraneousData();
    }
    private void SetupExtraneousData()
    {
      double pixelCount = (double)totalPixelCount;
      byte previousIntensity = (byte)0;
      for(int i = 0; i < NUM_VALUES; i++)
      {
        //do this in a single pass
        double amount = (double)SetupTotalsIteration(i);
        SetupPkIteration(i, pixelCount);
        SetupEqualizedIntensityIteration(i, pixelCount, amount,
            ref previousIntensity);
      }
    }
    private void SetupPkIteration(int i, double pixelCount)
    {
      pk[i] = contents[i] / pixelCount;
    }
    private void SetupEqualizedIntensityIteration(int i, 
        double count, double amount, 
        ref byte previousIntensity)
    {
      if(contents[i] == 0)
        globalEqualizedIntensity[i] = previousIntensity;
      else
      {
        double result = (255.0 * (amount / count));
        globalEqualizedIntensity[i] = (byte)result;
        previousIntensity = (byte)result;
      }
    }
    private long SetupTotalsIteration(int i)
    {
      if(i == 0)
        totals[i] = contents[i]; //just copy over the number of pixels
      else
        totals[i] = totals[i - 1] + contents[i];
      return totals[i];
    }
  }
}
