using EPIC.DataLayer.Customization;
using EPIC.DeviceInterface.Utilities;
using FTD2XX_NET;
using System.Diagnostics;
using System.Text;

namespace EPIC.DeviceInterface.Interfaces
{
    /// <summary>
    /// This is the HR firmware, when this is replaced, copy and rename, and continue to add functionality to the LatestFirmware class.
    /// Functionality on the settings and testing screens is determined based on which classes override the latest functions.
    /// Stubs can be created if functions are the same.
    /// </summary>
    // Token: 0x02000008 RID: 8
    public class HRFirmware : IControllable, IDisposable, ILightable, IBoostable
    {
        /// <summary>
        /// Occurs when data is received from the device.
        /// </summary>
        // Token: 0x14000003 RID: 3
        // (add) Token: 0x06000024 RID: 36 RVA: 0x000025B4 File Offset: 0x000007B4
        // (remove) Token: 0x06000025 RID: 37 RVA: 0x000025EC File Offset: 0x000007EC
        public event DataReceivedHandler DataReceived;

        // Token: 0x06000026 RID: 38 RVA: 0x00002621 File Offset: 0x00000821
        internal HRFirmware()
        {
            this.FTDIDevice = new FTDI();
            this.ReceivedDataEvent = new AutoResetEvent(true);
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002640 File Offset: 0x00000840
        protected void ReadData()
        {
            uint nrOfBytesAvailable = 0U;
            while (!this._closing)
            {
                this.ReceivedDataEvent.WaitOne(100);
                FTDI.FT_STATUS status = this.FTDIDevice.GetRxBytesAvailable(ref nrOfBytesAvailable);
                if (status != null)
                {
                    return;
                }
                if (nrOfBytesAvailable > 0U)
                {
                    byte[] readData = new byte[nrOfBytesAvailable];
                    uint numBytesRead = 0;

                    // 2. Call the Read overload that accepts a byte array
                    // Note: In FTD2XX_NET, this signature doesn't use 'ref' for the array
                    status = FTDIDevice.Read(readData, (uint)readData.Length, ref numBytesRead);
                    if (status != null)
                    {
                        throw new Exception(string.Format("Failed to read data (error {0})", status));
                    }
                    string newOutputLine = string.Empty;
                    foreach (char characterVar in readData)
                    {
                        if (HRFirmware.IsPrintableCharacter(characterVar))
                        {
                            newOutputLine += characterVar;
                        }
                        else
                        {
                            newOutputLine += string.Format("(0x{0:X})", (int)characterVar);
                        }
                    }
                    Log.Debug(string.Format("Data received: [{0}]", newOutputLine));
                    if (this.DataReceived != null)
                    {
                        // Convert the raw bytes from the FTDI buffer into a readable string
                        //string receivedText = Encoding.ASCII.GetString(readData, 0, (int)numBytesRead);

                        // Pass that string to your method
                        this.DataReceived(readData, newOutputLine);
                    }
                }
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002748 File Offset: 0x00000948
        protected virtual void SetFlowControl(ushort flowControl, byte xOn, byte xOff)
        {
            Log.Debug("Setting flow control");
            FTDI.FT_STATUS status = this.FTDIDevice.SetFlowControl(flowControl, xOn, xOff);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to set flow control: {0}", status));
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x0000278C File Offset: 0x0000098C
        protected virtual void SetCharacteristics(byte dataBits, byte stopBits, byte parityBits)
        {
            Log.Debug("Setting data characteristics");
            FTDI.FT_STATUS status = this.FTDIDevice.SetDataCharacteristics(dataBits, stopBits, parityBits);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to set data characteristics: {0}", status));
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000027D0 File Offset: 0x000009D0
        public virtual void Open()
        {
            try
            {
                this._closing = false;
                if (this.FTDIDevice.IsOpen)
                {
                    this.Close();
                }
                Log.Debug("Opening USB communication channel");
                lock (this.FTDIDevice)
                {
                    FTDI.FT_STATUS status = this.FTDIDevice.OpenByLocation((uint)this.UniqueIdentifier);
                    if (status != null)
                    {
                        throw new Exception(string.Format("Unable to open the specified device: {0}", status));
                    }
                    this.SetBaudRate(9600U);
                    this.SetCharacteristics(8, 0, 0);
                    this.SetFlowControl(0, 13, 10);
                    this.SetTimeouts(750U, 750U);
                    status = this.FTDIDevice.SetEventNotification(1U, this.ReceivedDataEvent);
                    if (status != null)
                    {
                        throw new Exception(string.Format("Unable to set event handler: {0}", status));
                    }
                    Task.Run(new Action(this.ReadData));
                }
            }
            catch (Exception ex)
            {
                Log.Error("There was an error while opening the device.", ex);
                throw;
            }
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000028F8 File Offset: 0x00000AF8
        protected virtual void SetBaudRate(uint baudRate)
        {
            Log.Debug(string.Format("Setting baud rate: {0}", baudRate));
            FTDI.FT_STATUS status = this.FTDIDevice.SetBaudRate(baudRate);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to set appropriate baud rate: {0}", status));
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002948 File Offset: 0x00000B48
        protected virtual void SetTimeouts(uint readTimeout, uint writeTimeout)
        {
            Log.Debug("Setting component timeouts");
            FTDI.FT_STATUS status = this.FTDIDevice.SetTimeouts(readTimeout, writeTimeout);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to set timeout values: {0}", status));
            }
        }

        /// <summary>
        /// This method is called to clear out the buffer when an error 
        /// event has been detected. The attempts to get the communication
        /// process back in sync.
        /// </summary>
        // Token: 0x0600002D RID: 45 RVA: 0x0000298C File Offset: 0x00000B8C
        protected virtual void ClearBuffer()
        {
            uint numBytesAvailable = 0U;
            FTDI.FT_STATUS status = this.FTDIDevice.GetRxBytesAvailable(ref numBytesAvailable);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to receive response (ClearBuffer): {0}", status));
            }
            if (numBytesAvailable > 0U)
            {
                uint numBytesRead = 0U;
                byte[] readData = new byte[numBytesAvailable];

                // 2. Call the Read overload that accepts a byte array
                // Note: In FTD2XX_NET, this signature doesn't use 'ref' for the array
                status = FTDIDevice.Read(readData, (uint)readData.Length, ref numBytesRead);
                if (status != null)
                {
                    throw new Exception(string.Format("Failed to read data (error {0})", status));
                }
                string newOutputLine = string.Empty;
                foreach (char characterVar in readData)
                {
                    if (HRFirmware.IsPrintableCharacter(characterVar))
                    {
                        newOutputLine += characterVar;
                    }
                    else
                    {
                        newOutputLine += string.Format("(0x{0:X})", (int)characterVar);
                    }
                }
                Log.Debug(string.Concat(new object[]
                {
                    "Cleared buffer, removed ",
                    numBytesAvailable,
                    " characters [",
                    newOutputLine,
                    "]"
                }));
            }
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00002A9C File Offset: 0x00000C9C
        public virtual void SetPulseDuration(PulseDuration pulseDuration)
        {
            string valueToTransmit = "e=";
            if ((int)pulseDuration <= 1000)
            {
                if ((int)pulseDuration == 500)
                {
                    valueToTransmit += "00.5";
                    goto IL_77;
                }
                if ((int)pulseDuration == 1000)
                {
                    valueToTransmit += "01.0";
                    goto IL_77;
                }
            }
            else
            {
                if ((int)pulseDuration == 2000)
                {
                    valueToTransmit += "02.0";
                    goto IL_77;
                }
                if ((int)pulseDuration == 30000)
                {
                    valueToTransmit += "30.0";
                    goto IL_77;
                }
            }
            throw new Exception("Invalid pulse duration value");
        IL_77:
            this.SendCommand(valueToTransmit);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x00002B28 File Offset: 0x00000D28
        public virtual void SetExposureVoltage(Voltage voltage)
        {
            string valueToTransmit = "V=";
            if ((int)voltage <= 60)
            {
                if ((int)voltage == 50)
                {
                    valueToTransmit += "FF";
                    goto IL_286;
                }
                if ((int)voltage == 60)
                {
                    valueToTransmit += "A0";
                    goto IL_286;
                }
            }
            else
            {
                if ((int)voltage == 70)
                {
                    valueToTransmit += "72";
                    goto IL_286;
                }
                switch ((int)voltage)
                {
                    case 80:
                        valueToTransmit += "52";
                        goto IL_286;
                    case 90:
                        valueToTransmit += "38";
                        goto IL_286;
                    case 100:
                        valueToTransmit += "24";
                        goto IL_286;
                    case 101:
                        valueToTransmit += "21";
                        goto IL_286;
                    case 102:
                        valueToTransmit += "1F";
                        goto IL_286;
                    case 103:
                        valueToTransmit += "1D";
                        goto IL_286;
                    case 104:
                        valueToTransmit += "1B";
                        goto IL_286;
                    case 105:
                        valueToTransmit += "1A";
                        goto IL_286;
                    case 106:
                        valueToTransmit += "18";
                        goto IL_286;
                    case 107:
                        valueToTransmit += "16";
                        goto IL_286;
                    case 108:
                        valueToTransmit += "15";
                        goto IL_286;
                    case 109:
                        valueToTransmit += "13";
                        goto IL_286;
                    case 110:
                        valueToTransmit += "12";
                        goto IL_286;
                    case 111:
                        valueToTransmit += "11";
                        goto IL_286;
                    case 112:
                        valueToTransmit += "0F";
                        goto IL_286;
                    case 113:
                        valueToTransmit += "0D";
                        goto IL_286;
                    case 114:
                        valueToTransmit += "0C";
                        goto IL_286;
                    case 115:
                        valueToTransmit += "0A";
                        goto IL_286;
                    case 116:
                        valueToTransmit += "08";
                        goto IL_286;
                    case 117:
                        valueToTransmit += "07";
                        goto IL_286;
                    case 118:
                        valueToTransmit += "06";
                        goto IL_286;
                    case 119:
                        valueToTransmit += "05";
                        goto IL_286;
                    case 120:
                        valueToTransmit += "04";
                        goto IL_286;
                }
            }
            throw new Exception("Invalid voltage value");
        IL_286:
            this.SendCommand(valueToTransmit);
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00002DC4 File Offset: 0x00000FC4
        public virtual void SetFrequency(PWM0Frequency frequency)
        {
            string valueToTransmit = "f=";
            if ((int)frequency <= 950)
            {
                if ((int)frequency <= 800)
                {
                    if ((int)frequency == 750)
                    {
                        valueToTransmit += "0750";
                        goto IL_136;
                    }
                    if ((int)frequency == 800)
                    {
                        valueToTransmit += "0800";
                        goto IL_136;
                    }
                }
                else
                {
                    if ((int)frequency == 850)
                    {
                        valueToTransmit += "0850";
                        goto IL_136;
                    }
                    if ((int)frequency == 900)
                    {
                        valueToTransmit += "0900";
                        goto IL_136;
                    }
                    if ((int)frequency == 950)
                    {
                        valueToTransmit += "0950";
                        goto IL_136;
                    }
                }
            }
            else if ((int)frequency <= 1050)
            {
                if ((int)frequency == 1000)
                {
                    valueToTransmit += "1000";
                    goto IL_136;
                }
                if ((int)frequency == 1050)
                {
                    valueToTransmit += "1050";
                    goto IL_136;
                }
            }
            else
            {
                if ((int)frequency == 1100)
                {
                    valueToTransmit += "1100";
                    goto IL_136;
                }
                if ((int)frequency == 1150)
                {
                    valueToTransmit += "1150";
                    goto IL_136;
                }
                if ((int)frequency == 1200)
                {
                    valueToTransmit += "1200";
                    goto IL_136;
                }
            }
            throw new Exception("Invalid PWM0 frequency specified");
        IL_136:
            this.SendCommand(valueToTransmit);
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002F68 File Offset: 0x00001168
        protected virtual void SendCommand(byte[] commandToSend)
        {
            string cmdString = Encoding.ASCII.GetString(commandToSend);
            this.ClearBuffer();
            bool ack = false;
            string response = string.Empty;
            DataReceivedHandler handler = delegate (byte[] data, string formatted)
            {
                response += Encoding.ASCII.GetString(data);
                if (response == ">,00," + cmdString)
                {
                    ack = true;
                }
            };
            uint numBytesWritten = 0U;
            this.DataReceived += handler;
            Log.Debug(string.Format("Sending: {0}", cmdString));
            FTDI.FT_STATUS status = this.FTDIDevice.Write(commandToSend, commandToSend.Length, ref numBytesWritten);
            if (status != null)
            {
                throw new Exception(string.Format("Unable to write to the FTDI buffer: {0}", status));
            }
            Stopwatch watchdog = new Stopwatch();
            watchdog.Start();
            while (!ack && watchdog.ElapsedMilliseconds < 2000L)
            {
                Thread.Sleep(10);
            }
            watchdog.Stop();
            this.DataReceived -= handler;
            if (!ack)
            {
                throw new Exception(string.Format("Command failed to acknowledge: {0}", cmdString));
            }
        }

        // Token: 0x06000032 RID: 50 RVA: 0x0000305E File Offset: 0x0000125E
        protected virtual void SendCommand(string commandToSend)
        {
            commandToSend += Environment.NewLine;
            this.SendCommand(Encoding.ASCII.GetBytes(commandToSend));
        }

        // Token: 0x06000033 RID: 51 RVA: 0x0000307E File Offset: 0x0000127E
        public virtual void SetBoostVoltage(bool on)
        {
            this.SendCommand(on ? "P=1" : "P=0");
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003095 File Offset: 0x00001295
        public virtual void SetDiagnosticsMode(bool on)
        {
            this.SendCommand(on ? "d=1" : "d=0");
        }

        // Token: 0x06000035 RID: 53 RVA: 0x000030AC File Offset: 0x000012AC
        public virtual void StartExposure()
        {
            this.SendCommand("g");
        }

        // Token: 0x06000036 RID: 54 RVA: 0x000030B9 File Offset: 0x000012B9
        public virtual void StopExposure()
        {
            this.SendCommand("a");
        }

        // Token: 0x06000037 RID: 55 RVA: 0x000030C6 File Offset: 0x000012C6
        public virtual void GetStatusInfo()
        {
            this.SendCommand("i");
        }

        // Token: 0x06000038 RID: 56 RVA: 0x000030D3 File Offset: 0x000012D3
        public virtual void Reset()
        {
            this.SendCommand("R=1");
        }

        // Token: 0x06000039 RID: 57 RVA: 0x000030E0 File Offset: 0x000012E0
        public virtual void SetExposureVoltage(int voltage)
        {
            this.SendCommand("V=");
        }

        // Token: 0x0600003A RID: 58 RVA: 0x000030ED File Offset: 0x000012ED
        public virtual void SetLED(bool on)
        {
            this.SendCommand(on ? "l=1" : "l=0");
        }

        /// <summary>
        /// Simple utility method to determine if a character is a printable
        /// character.
        /// </summary>
        /// <param name="candidate">Character to check</param>
        /// <returns>True or false based on if the character is printable</returns>
        // Token: 0x0600003B RID: 59 RVA: 0x00003104 File Offset: 0x00001304
        private static bool IsPrintableCharacter(char candidate)
        {
            return candidate >= ' ' && candidate <= '\u007f';
        }

        /// <summary>
        /// Closes the current communication connection to the scanner
        /// </summary>
        /// <returns>A boolean to indicate either success or failure</returns>
        // Token: 0x0600003C RID: 60 RVA: 0x00003118 File Offset: 0x00001318
        public virtual void Close()
        {
            this._closing = true;
            Log.Debug("Closing communications with device.");
            lock (this.FTDIDevice)
            {
                if (this.FTDIDevice.IsOpen)
                {
                    FTDI.FT_STATUS status = this.FTDIDevice.Close();
                    if (status != null)
                    {
                        throw new Exception(string.Format("There was an error closing the device: {0}", status));
                    }
                }
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600003D RID: 61 RVA: 0x0000319C File Offset: 0x0000139C
        // (set) Token: 0x0600003E RID: 62 RVA: 0x000031A4 File Offset: 0x000013A4
        public object UniqueIdentifier { get; set; }

        // Token: 0x0600003F RID: 63 RVA: 0x000031AD File Offset: 0x000013AD
        public void Dispose()
        {
            this.Close();
        }

        // Token: 0x04000008 RID: 8
        protected const uint WATCHDOG = 2000U;

        // Token: 0x04000009 RID: 9
        protected const uint COMMUNICATION_RATE = 9600U;

        // Token: 0x0400000A RID: 10
        protected const uint WRITE_TIMEOUT = 750U;

        // Token: 0x0400000B RID: 11
        protected const uint READ_TIMEOUT = 750U;

        // Token: 0x0400000C RID: 12
        protected const string GO_COMMAND = "g";

        // Token: 0x0400000D RID: 13
        protected const string ABORT_COMMAND = "a";

        // Token: 0x0400000E RID: 14
        protected const string INFORMATION_COMMAND = "i";

        // Token: 0x0400000F RID: 15
        protected const string PULSE_FREQUENCY_COMMAND = "f=";

        // Token: 0x04000010 RID: 16
        protected const string LED_OFF_COMMAND = "l=0";

        // Token: 0x04000011 RID: 17
        protected const string LED_ON_COMMAND = "l=1";

        // Token: 0x04000012 RID: 18
        protected const string DIAGNOSTICS_MODE_ON_COMMAND = "d=1";

        // Token: 0x04000013 RID: 19
        protected const string DIAGNOSTICS_MODE_OFF_COMMAND = "d=0";

        // Token: 0x04000014 RID: 20
        protected const string PULSE_DURATION_COMMAND = "e=";

        // Token: 0x04000015 RID: 21
        protected const string SPECIFIC_VOLTAGE_COMMAND = "V=";

        // Token: 0x04000016 RID: 22
        protected const string ERROR_TOKEN = "?";

        // Token: 0x04000017 RID: 23
        protected const string TRACE_PARSING_TOKEN = "|";

        // Token: 0x04000018 RID: 24
        protected const string BOOST_VOLTAGE_ON = "P=1";

        // Token: 0x04000019 RID: 25
        protected const string BOOST_VOLTAGE_OFF = "P=0";

        // Token: 0x0400001A RID: 26
        protected const string FORCE_RESET = "R=1";

        // Token: 0x0400001C RID: 28
        protected readonly FTDI FTDIDevice;

        // Token: 0x0400001D RID: 29
        protected readonly AutoResetEvent ReceivedDataEvent;

        // Token: 0x0400001E RID: 30
        private bool _closing;
    }
}
