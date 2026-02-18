namespace EPIC.DeviceInterface.Utilities
{
    /// <summary>
    /// Handler for when data is received from the FTDI device.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="formatted"></param>
    // Token: 0x02000009 RID: 9
    // (Invoke) Token: 0x06000042 RID: 66
    public delegate void DataReceivedHandler(byte[] data, string formatted);
}
