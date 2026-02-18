using OpenCvSharp;
using System.Runtime.InteropServices;

namespace EPIC.CameraInterface.Utilities.Rasterizer
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
        public static Mat Bin(Mat image, int scale = 2, BinMethod method = BinMethod.Maximum)
        {
            if (image.Empty())
                throw new ArgumentException("Image cannot be empty");

            if (image.Width % scale != 0 || image.Height % scale != 0)
            {
                throw new NotSupportedException("Image width and height must be evenly divisible by scale.");
            }

            BinningMethod binningMethod = BinningMethod.GetInstance(method);
            int pixelSize = image.ElemSize(); // Bytes per pixel (e.g., 3 for BGR, 4 for BGRA)
            int reductionCount = scale * scale;

            // In OpenCV, Step() is the Stride (total bytes per row including padding)
            int stride = (int)image.Step();
            int byteCount = stride * image.Height;

            // 1. Copy source pixels from Mat to byte[]
            byte[] pixels = new byte[byteCount];
            Marshal.Copy(image.Data, pixels, 0, byteCount);

            // 2. Prepare output buffer
            // The output size calculation needs to be exact based on the target dimensions
            int newWidth = image.Width / scale;
            int newHeight = image.Height / scale;
            // The output array size is simply width * height * pixelSize
            // (The Bin method logic writes tightly packed bytes, no stride padding usually)
            byte[] output = new byte[newWidth * newHeight * pixelSize];

            // 3. Run the preserved Binning Algorithm
            binningMethod.Bin(pixels, output, scale, stride, pixelSize);

            // 4. Create new Mat from the output bytes
            Mat result = new Mat(newHeight, newWidth, image.Type());

            // Note: The Bin methods above write 'packed' data (no stride padding).
            // If the new Mat has padding (Step > Width * PixelSize), we must copy row-by-row 
            // or ensure the Mat is continuous. Newly created Mats are usually continuous.
            if (result.IsContinuous())
            {
                Marshal.Copy(output, 0, result.Data, output.Length);
            }
            else
            {
                // Fallback if OpenCV creates a padded Mat (rare for small/standard images)
                int resultRowWidth = newWidth * pixelSize;
                for (int i = 0; i < newHeight; i++)
                {
                    IntPtr destRow = result.Ptr(i);
                    Marshal.Copy(output, i * resultRowWidth, destRow, resultRowWidth);
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
        /*
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
        */

        // Token: 0x06000003 RID: 3 RVA: 0x00002218 File Offset: 0x00000418
        public static void Main(string[] args)
        {
            string filename;
            BinMethod method;
            int scale;
            int iterations;

            if (ParseArgs(args, out filename, out method, out scale, out iterations))
            {
                // Use OpenCvSharp to load the image
                using (Mat image = Cv2.ImRead(filename, ImreadModes.Unchanged))
                {
                    if (image.Empty())
                    {
                        Console.WriteLine("Could not load image: " + filename);
                        return;
                    }

                    Console.WriteLine("Width:        " + image.Width);
                    Console.WriteLine("Height:       " + image.Height);
                    Console.WriteLine("Type:         " + image.Type());
                    Console.WriteLine("PixelSize:    " + image.ElemSize());

                    FileInfo fileInfo = new FileInfo(filename);
                    string directory = fileInfo.DirectoryName ?? ".";
                    string name = fileInfo.Name;

                    Mat o = Binner.Bin(image, scale, method);

                    Console.WriteLine("Begin bin:    {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    for (int i = 0; i < iterations; i++)
                    {
                        o.Dispose();
                        o = Binner.Bin(image, scale, method);
                    }
                    Console.WriteLine("End bin:      {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                    string outputFile = Path.Combine(directory, "_" + name);
                    o.SaveImage(outputFile);

                    // Resize comparison
                    Console.WriteLine("Begin resize: {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    for (int j = 0; j < iterations; j++)
                    {
                        o.Dispose();
                        o = Binner.Resize(image, scale);
                    }
                    Console.WriteLine("End resize:   {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

                    string resizeFile = Path.Combine(directory, "__" + name);
                    o.SaveImage(resizeFile);

                    o.Dispose();
                }
                return;
            }
            Binner.Usage();
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000023D4 File Offset: 0x000005D4
        private static Mat Resize(Mat image, int scale)
        {
            Mat newImage = new Mat();
            // Nearest neighbor mimics the simplistic resizing of the original code
            Cv2.Resize(image, newImage, new Size(image.Width / scale, image.Height / scale), 0, 0, InterpolationFlags.Nearest);
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
