using System;

namespace EPIC.CameraInterface
{
	/// <summary>
	/// Binning method that implements an algorithm that determines the average values of the
	/// component parts of the input pixels and creates an output pixel of the averages.
	/// </summary>
	// Token: 0x02000005 RID: 5
	internal class AverageMethod : BinningMethod
	{
		// Token: 0x0600000B RID: 11 RVA: 0x00002594 File Offset: 0x00000794
		public override void Bin(byte[] pixels, byte[] output, int scale, int stride, int pixelSize)
		{
			int byteCount = pixels.Length;
			int reductionCount = scale * scale;
			int verticalOffset = scale * stride;
			int horizontalOffset = scale * pixelSize;
			int[] totals = new int[pixelSize];
			int outIndex = 0;
			for (int y = 0; y < byteCount; y += verticalOffset)
			{
				for (int x = 0; x < stride; x += horizontalOffset)
				{
					for (int i = 0; i < pixelSize; i++)
					{
						totals[i] = 0;
					}
					for (int z = 0; z < verticalOffset; z += stride)
					{
						for (int w = 0; w < horizontalOffset; w += pixelSize)
						{
							int pixelByte = y + x + z + w;
							for (int j = 0; j < pixelSize; j++)
							{
								totals[j] += (int)pixels[pixelByte++];
							}
						}
					}
					for (int k = 0; k < pixelSize; k++)
					{
						output[outIndex++] = (byte)(totals[k] / reductionCount);
					}
				}
			}
		}
	}
}
