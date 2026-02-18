namespace EPIC.DeviceInterface.Interfaces
{
    /// <summary>
    /// Some devices cannot change voltage, this interface is used to indicate that.
    /// </summary>
    // Token: 0x02000007 RID: 7
    public interface IBoostable
    {
        /// <summary>
        /// Boost the voltage on the device.
        /// </summary>
        /// <param name="on"></param>
        /// <returns></returns>
        // Token: 0x06000023 RID: 35
        void SetBoostVoltage(bool on);
    }
}
