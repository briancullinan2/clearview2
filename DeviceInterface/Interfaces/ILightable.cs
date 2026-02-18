namespace EPIC.DeviceInterface.Interfaces
{
    /// <summary>
    /// Some devices do not include and LED, this interface is used to indicate that.
    /// </summary>
    // Token: 0x02000006 RID: 6
    public interface ILightable
    {
        /// <summary>
        /// Turns the LED on or off.
        /// </summary>
        /// <param name="on"></param>
        /// <returns></returns>
        // Token: 0x06000022 RID: 34
        void SetLED(bool on);
    }
}
