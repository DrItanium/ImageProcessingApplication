using System.Reflection;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text;
using Libraries.Imaging;
using Libraries.Filter;
using System.Threading;
using System.ComponentModel;
using System.Collections;


namespace CS555.Homework2
{
  [Filter("Median Filter")]
    public class MedianFilter : SpatialFilter
  {
    private List<byte> elements;
    private object elementsLock;
    public MedianFilter(string name) : base(name) 
    {
      elements = new List<byte>();
      elementsLock = new object();
    }
    protected override byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable values)
    {
      lock(elementsLock)
      {
        int width = input.Length;
        int height = input[0].Length;
        elements.Clear();
        for(int s = -a; s < a; s++)
        {
          int wX = x + s;
          if(wX < 0 || wX >= width)
            continue;
          for(int t = -b; t < b; t++)
          {
            int wY = y + t;
            if(wY < 0 || wY >= height)
              continue;
            elements.Add(input[wX][wY]);
          }
        }
        elements.Sort();
        return elements[elements.Count >> 1];
      }
    }
  }
}
