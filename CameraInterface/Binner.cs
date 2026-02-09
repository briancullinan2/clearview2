using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace EPIC.CameraInterface
{
	// Token: 0x02000003 RID: 3
	public class Binner
	{
		/// <summary>
		/// <para>
		/// Bin an image to reduce its size by a scale.
		/// Scale is effectively 1/scale.
		/// A scale value of 2 would reduce the image to 1/2 its original size.
		/// The default scale is 2.
		/// </para><para>
		/// Binning is performed by calculating the average of the red, green, blue and alpha values
		/// for each matrix of pixels with width and height equal to scale.
		/// The average values are then used to create a single pixel of the new image.
		/// </para><para>
		/// Both the width and height of the input image must be evenly divisible by the scale.
		/// If not, an expection is throw.
		/// </para>
		/// </summary>
		/// <param name="image">the image to bin</param>
		/// <param name="scale">the scaling factor</param>
		/// <param name="method">the method to use</param>
		/// <returns>new <code>Bitmap</code> containing the binned image</returns>
		/// <exception cref="T:System.NotSupportedException">if either width or height of input image is not
		/// 	evenly divisble by scale</exception>
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Bitmap Bin(Bitmap image, int scale = 2, BinMethod method = BinMethod.Maximum)
		{
			if (image.Width % scale != 0 || image.Height % scale != 0)
			{
				throw new NotSupportedException("Image width and height must be evenly divisble by scale.");
			}
			BinningMethod binningMethod = BinningMethod.GetInstance(method);
			int pixelSize = Binner.GetPixelSize(image);
			int reductionCount = scale * scale;
			BitmapData inputPixels = null;
			Bitmap result;
			try
			{
				inputPixels = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
				int stride = inputPixels.Stride;
				int byteCount = inputPixels.Stride * inputPixels.Height;
				byte[] pixels = new byte[byteCount];
				Marshal.Copy(inputPixels.Scan0, pixels, 0, byteCount);
				byte[] output = new byte[byteCount / reductionCount];
				binningMethod.Bin(pixels, output, scale, stride, pixelSize);
				Bitmap newImage = null;
				BitmapData outputPixels = null;
				try
				{
					newImage = new Bitmap(image.Width / scale, image.Height / scale, image.PixelFormat);
					outputPixels = newImage.LockBits(new Rectangle(0, 0, newImage.Width, newImage.Height), ImageLockMode.WriteOnly, newImage.PixelFormat);
					Marshal.Copy(output, 0, outputPixels.Scan0, output.Length);
					newImage.UnlockBits(outputPixels);
					result = newImage;
				}
				catch
				{
					try
					{
						if (outputPixels != null)
						{
							newImage.UnlockBits(outputPixels);
						}
					}
					catch
					{
					}
					if (newImage != null)
					{
						newImage.Dispose();
					}
					throw;
				}
			}
			finally
			{
				if (inputPixels != null)
				{
					image.UnlockBits(inputPixels);
				}
			}
			return result;
		}

		/// <summary>
		/// Get the number of bytes in a pixel.
		/// The number of bytes depends on the number of bits that make up a pixel.
		/// </summary>
		/// <param name="image">a <code>Bitmap</code> containing an image</param>
		/// <returns>number of bytes per pixel</returns>
		// Token: 0x06000002 RID: 2 RVA: 0x000021BC File Offset: 0x000003BC
		private static int GetPixelSize(Bitmap image)
		{
			PixelFormat pixelFormat = image.PixelFormat;
			if (pixelFormat <= PixelFormat.Format32bppRgb)
			{
				if (pixelFormat == PixelFormat.Format24bppRgb)
				{
					return 3;
				}
				if (pixelFormat != PixelFormat.Format32bppRgb)
				{
					goto IL_43;
				}
			}
			else
			{
				if (pixelFormat == PixelFormat.Format48bppRgb)
				{
					return 6;
				}
				if (pixelFormat != PixelFormat.Format32bppArgb)
				{
					if (pixelFormat != PixelFormat.Format64bppArgb)
					{
						goto IL_43;
					}
					return 8;
				}
			}
			return 4;
			IL_43:
			throw new NotSupportedException("Image format not supported");
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002218 File Offset: 0x00000418
		private static void Main(string[] args)
		{
			string filename;
			BinMethod method;
			int scale;
			int iterations;
			if (Binner.ParseArgs(args, out filename, out method, out scale, out iterations))
			{
				Bitmap image = new Bitmap(filename);
				Console.WriteLine("Width:        " + image.Width);
				Console.WriteLine("Height:       " + image.Height);
				Console.WriteLine("PixelFormat:  " + image.PixelFormat);
				Console.WriteLine(string.Format("Flags:        {0:X}", image.Flags));
				FileInfo fileInfo = new FileInfo(filename);
				string directory = fileInfo.DirectoryName;
				filename = fileInfo.Name;
				Bitmap o = Binner.Bin(image, scale, method);
				Console.WriteLine("Begin bin:    {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
				for (int i = 0; i < iterations; i++)
				{
					o.Dispose();
					o = Binner.Bin(image, scale, method);
				}
				Console.WriteLine("End bin:      {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
				o.Save(directory + "\\_" + filename, image.RawFormat);
				Console.WriteLine("Begin resize: {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
				for (int j = 0; j < iterations; j++)
				{
					o.Dispose();
					o = Binner.Resize(image, scale);
				}
				Console.WriteLine("End resize:   {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
				o.Save(directory + "\\__" + filename, image.RawFormat);
				o.Dispose();
				image.Dispose();
				return;
			}
			Binner.Usage();
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000023D4 File Offset: 0x000005D4
		private static Bitmap Resize(Bitmap image, int scale)
		{
			Bitmap newImage = new Bitmap(image, image.Width / scale, image.Height / scale);
			using (Graphics g = Graphics.FromImage(newImage))
			{
				g.InterpolationMode = InterpolationMode.NearestNeighbor;
				g.DrawImage(image, 0, 0, newImage.Width, newImage.Height);
			}
			return newImage;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002438 File Offset: 0x00000638
		private static bool ParseArgs(string[] args, out string filename, out BinMethod method, out int scale, out int iterations)
		{
			filename = string.Empty;
			method = BinMethod.Maximum;
			scale = 2;
			iterations = 1;
			if (args.Length == 0)
			{
				return false;
			}
			filename = args[0];
			if (args.Length == 1)
			{
				return true;
			}
			int i = 1;
			while (i < args.Length)
			{
				string a;
				if ((a = args[i].ToLower()) != null)
				{
					if (!(a == "/m"))
					{
						if (!(a == "/s"))
						{
							if (!(a == "/i"))
							{
								return false;
							}
							if (++i > args.Length)
							{
								return false;
							}
							if (!int.TryParse(args[i], out iterations))
							{
								return false;
							}
						}
						else
						{
							if (++i > args.Length)
							{
								return false;
							}
							if (!int.TryParse(args[i], out scale))
							{
								return false;
							}
						}
					}
					else
					{
						if (++i > args.Length)
						{
							return false;
						}
						string j = args[i].ToLower();
						if (j.StartsWith("m"))
						{
							method = BinMethod.Maximum;
						}
						else
						{
							if (!j.StartsWith("a"))
							{
								return false;
							}
							method = BinMethod.Average;
						}
					}
					i++;
					continue;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002527 File Offset: 0x00000727
		private static void Usage()
		{
			Console.WriteLine("binner imagefile [/m a|m] [/s scale] [/i count]");
			Console.WriteLine("    imagefile; name of a file containing an image");
			Console.WriteLine("    /m a|m; a = average, m = maximum; default maximum");
			Console.WriteLine("    /s scale; scale = reduction factor; default 2");
			Console.WriteLine("    /i count; count = number of times to iterate; default 1");
			Console.WriteLine("  binned image output as _imagefile; resized image output as __imagefile");
		}
	}
}
