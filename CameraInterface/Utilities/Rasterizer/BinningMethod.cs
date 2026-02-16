using System;

namespace EPIC.CameraInterface.Utilities.Rasterizer
{
	/// <summary>
	/// Base class for binning method implementations.
	/// Provides a factory method for getting the proper subclass that implements the specified method.
	/// </summary>
	// Token: 0x02000004 RID: 4
	internal abstract class BinningMethod
	{
		/// <summary>
		/// Bin the input in the pixels array into the output array.
		/// </summary>
		/// <param name="pixels">byte array containing all the bytes that make up the input image</param>
		/// <param name="output">byte array into which to output the bytes of the binned image</param>
		/// <param name="scale">the scaling factor</param>
		/// <param name="stride">number of bytes in a single row of pixels</param>
		/// <param name="pixelSize">number of byte in a single pixel</param>
		// Token: 0x06000008 RID: 8
		public abstract void Bin(byte[] pixels, byte[] output, int scale, int stride, int pixelSize);

		/// <summary>
		/// Get an instance that implements the specified method
		/// </summary>
		/// <param name="method">the bin method to use</param>
		/// <returns>subclass instance that implements the method</returns>
		// Token: 0x06000009 RID: 9 RVA: 0x0000256D File Offset: 0x0000076D
		public static BinningMethod GetInstance(BinMethod method)
		{
			if (method == BinMethod.Maximum)
			{
				return new MaxMethod();
			}
			if (method == BinMethod.Average)
			{
				return new AverageMethod();
			}
			throw new ArgumentException("Bin method not supported.");
		}
	}
}
