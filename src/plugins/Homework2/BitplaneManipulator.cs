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


	[Filter("Manipulate Bitplanes")]
		public class BitplaneManipulator : Filter
	{
		public override string InputForm
		{
			get
			{
				return "form new \"Bitplane Manipulation\" \"Name\" imbue label new \"Plane 0\" \"Text\" imbue 13 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 1\" \"Text\" imbue 13 32 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 2\" \"Text\" imbue 13 52 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 3\" \"Text\" imbue 13 72 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 4\" \"Text\" imbue 113 12 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 5\" \"Text\" imbue 113 32 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 6\" \"Text\" imbue 113 52 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue label new \"Plane 7\" \"Text\" imbue 113 72 point \"Location\" imbue 63 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane0\" \"Name\" imbue 80 12 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane1\" \"Name\" imbue 80 32 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane2\" \"Name\" imbue 80 52 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane3\" \"Name\" imbue 80 72 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane4\" \"Name\" imbue 187 12 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane5\" \"Name\" imbue 187 32 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane6\" \"Name\" imbue 187 52 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue checkbox new \"plane7\" \"Name\" imbue 187 72 point \"Location\" imbue 13 13 size \"Size\" imbue \"Controls.Add\" imbue return";
			}
		}
		public BitplaneManipulator(string name) : base(name) { }
		public override Hashtable TranslateData(Hashtable input)
		{
			byte total = 0;
			if((bool)input["plane0"])
				total += 1;
			if((bool)input["plane1"])
				total += 2;
			if((bool)input["plane2"])
				total += 4;
			if((bool)input["plane3"])
				total += 8;
			if((bool)input["plane4"])
				total += 16;
			if((bool)input["plane5"])
				total += 32;
			if((bool)input["plane6"])
				total += 64;
			if((bool)input["plane7"])
				total += 128;
			//precompute the values 
			byte[] table = new byte[256];
			for(int i = 0; i < 256; i++) 
			{
				table[i] = (byte)(i & total);
			}
			input["translation-table"] = table;
			return input;
		}
		public override byte[][] Transform(Hashtable input)
		{
			byte[][] image = (byte[][])input["image"];
			int iWidth = image.Length;
			int iHeight = image[0].Length;
			byte[][] clone = new byte[iWidth][];
			byte[] table = (byte[])input["translation-table"];
			for(int i = 0; i < iWidth; i++)
			{
				byte[] q = new byte[iHeight];
				byte[] iX = image[i];
				for(int j = 0; j < iHeight; j++)
				{
					q[j] = table[iX[j]];
				}
				clone[i] = q;
			}	
			return clone;
		}
	}	
}
