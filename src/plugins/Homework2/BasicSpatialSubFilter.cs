using System;
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
	public abstract class SpatialFilter : Filter
	{
		public override string InputForm
		{
			get
			{
				return string.Format("form new \"{0}\" \"Text\" imbue label new \"Mask Size\" \"Text\" imbue \"maskSizeLabel\" \"Name\" imbue 13 35 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue combobox new \"mask\" \"Name\" imbue 80 35 point \"Location\" imbue 75 21 size \"Size\" imbue \"3x3\" \"Items.Add\" imbue \"5x5\" \"Items.Add\" imbue \"7x7\" \"Items.Add\" imbue \"9x9\" \"Items.Add\" imbue \"11x11\" \"Items.Add\" imbue \"13x13\" \"Items.Add\" imbue \"15x15\" \"Items.Add\" imbue \"17x17\" \"Items.Add\" imbue \"19x19\" \"Items.Add\" imbue \"21x21\" \"Items.Add\" imbue \"Controls.Add\" imbue {1} return", 
						Name, InputFormAddition);
			}
		}
		protected SpatialFilter(string name) : base(name) { }
		protected virtual string InputFormAddition { get { return string.Empty; } }
		public override Hashtable TranslateData(Hashtable input)
		{
			Hashtable ht = TranslateData_Impl(input);
			ht["image"] = input["image"];
			string maskSize = (string)input["mask"];
			switch(maskSize)
			{
				case "3x3":
					ht["mask"] = 3;
					break;
				case "5x5":
					ht["mask"] = 5;
					break;
				case "7x7":
					ht["mask"] = 7;
					break;
				case "9x9":
					ht["mask"] = 9;
					break;
				case "11x11":
					ht["mask"] = 11;
					break;
				case "13x13":
					ht["mask"] = 13;
					break;
				case "15x15":
					ht["mask"] = 15;
					break;
				case "17x17":
					ht["mask"] = 17;
					break;
				case "19x19":
					ht["mask"] = 19;
					break;
				case "21x21":
					ht["mask"] = 21;
					break;
				default:
					ht["mask"] = 3;
					break;
			}
			return ht;
		}
		public override byte[][] Transform(Hashtable input)
		{
			byte[][] image = (byte[][])input["image"];
			int iWidth = image.Length;
			int iHeight = image[0].Length;
			int m = (int)input["mask"];
			int a = (m - 1) >> 1;
			int b = a;
			byte[][] clone = new byte[iWidth][];
			for(int x = 0; x < iWidth; x++)
			{
				byte[] q = new byte[iHeight];
				for(int y = 0; y < iHeight; y++)
				{
					q[y] = Operation(a, b, x, y, image, input);
				}
				clone[x] = q;
			}
			return clone;
		}
		protected abstract byte Operation(int a, int b, int x, int y, byte[][] input, Hashtable input);
		protected virtual Hashtable TranslateData_Impl(Hashtable input) { return input; }
	}
}
