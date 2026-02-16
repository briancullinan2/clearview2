using System;

namespace EPIC.CameraInterface.Utilities.Rasterizer
{
	/// <summary>
	/// Binning method that implements an algorithm that determines the maximum values of the
	/// component parts of the input pixels and creates an output pixel of the maximums.
	/// </summary>
	// Token: 0x02000006 RID: 6
	internal class MaxMethod : BinningMethod
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000268C File Offset: 0x0000088C
		public override void Bin(byte[] pixels, byte[] output, int scale, int stride, int pixelSize)
		{
			int byteCount = pixels.Length;
			int verticalOffset = scale * stride;
			int horizontalOffset = scale * pixelSize;
			int[] maxiumums = new int[pixelSize];
			int outIndex = 0;
			for (int y = 0; y < byteCount; y += verticalOffset)
			{
				for (int x = 0; x < stride; x += horizontalOffset)
				{
					for (int i = 0; i < pixelSize; i++)
					{
						maxiumums[i] = 0;
					}
					for (int z = 0; z < verticalOffset; z += stride)
					{
						for (int w = 0; w < horizontalOffset; w += pixelSize)
						{
							int pixelByte = y + x + z + w;
							for (int j = 0; j < pixelSize; j++)
							{
								int value = (int)pixels[pixelByte++];
								if (value > maxiumums[j])
								{
									maxiumums[j] = value;
								}
							}
						}
					}
					for (int k = 0; k < pixelSize; k++)
					{
						output[outIndex++] = (byte)maxiumums[k];
					}
				}
			}
		}
	}
}
