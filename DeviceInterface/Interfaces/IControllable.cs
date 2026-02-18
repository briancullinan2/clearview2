using EPIC.DataLayer.Customization;
using EPIC.DeviceInterface.Utilities;

namespace EPIC.DeviceInterface.Interfaces
{
    /// <summary>
    /// Controls connections to the ClearView device through a common interface.
    /// </summary>
    // Token: 0x02000005 RID: 5
    public interface IControllable : IDisposable
    {
        /// <summary>
        /// Occurs when data is received from the device.
        /// </summary>
        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000016 RID: 22
        // (remove) Token: 0x06000017 RID: 23
        event DataReceivedHandler DataReceived;

        /// <summary>
        /// Open the device for communication.
        /// </summary>
        /// <returns></returns>
        // Token: 0x06000018 RID: 24
        void Open();

        /// <summary>
        /// Set the duration of the pulse.
        /// </summary>
        /// <param name="pulseDuration"></param>
        /// <returns></returns>
        // Token: 0x06000019 RID: 25
        void SetPulseDuration(PulseDuration pulseDuration);

        /// <summary>
        /// Set the voltage.
        /// </summary>
        /// <param name="voltage"></param>
        /// <returns></returns>
        // Token: 0x0600001A RID: 26
        void SetExposureVoltage(Voltage voltage);

        /// <summary>
        /// Set the frequency.
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        // Token: 0x0600001B RID: 27
        void SetFrequency(PWM0Frequency frequency);

        /// <summary>
        /// Start the exposure.
        /// </summary>
        /// <returns></returns>
        // Token: 0x0600001C RID: 28
        void StartExposure();

        /// <summary>
        /// Force the exposure to stop prematurely.
        /// </summary>
        /// <returns></returns>
        // Token: 0x0600001D RID: 29
        void StopExposure();

        /// <summary>
        /// Reset the device programatically.
        /// </summary>
        /// <returns></returns>
        // Token: 0x0600001E RID: 30
        void Reset();

        /// <summary>
        /// Close the device.
        /// </summary>
        /// <returns></returns>
        // Token: 0x0600001F RID: 31
        void Close();

        /// <summary>
        /// A unique object that described the location of the hardware device to connect to.
        /// </summary>
        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000020 RID: 32
        // (set) Token: 0x06000021 RID: 33
        object UniqueIdentifier { get; set; }
    }
}
