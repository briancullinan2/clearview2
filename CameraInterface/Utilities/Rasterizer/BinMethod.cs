namespace EPIC.CameraInterface.Utilities.Rasterizer
{
    /// <summary>
    /// The method to use for binning.
    /// This determines the arithmetic algorithm used to reduces multiple pixels to one pixel.
    /// </summary>
    // Token: 0x02000002 RID: 2
    public enum BinMethod
    {
        /// <summary>
        /// Output the maximum value of each component part.
        /// </summary>
        // Token: 0x04000002 RID: 2
        Maximum,
        /// <summary>
        /// Output the average of the values of each component part.
        /// </summary>
        // Token: 0x04000003 RID: 3
        Average
    }
}
