using EPIC.DataLayer.Customization;
using EPIC.DeviceInterface.Utilities;
using FTD2XX_NET;

namespace EPIC.DeviceInterface.Interfaces
{
    /// <summary>
    /// For older version of the device.
    /// </summary>
    // Token: 0x0200000A RID: 10
    public sealed class STFirmware : HRFirmware
    {
        // Token: 0x06000045 RID: 69 RVA: 0x000031CC File Offset: 0x000013CC
        public override void Open()
        {
            try
            {
                Log.Debug("Opening USB communication channel");
                FTDI.FT_STATUS status = this.FTDIDevice.OpenByLocation((uint)base.UniqueIdentifier);
                if (status != null)
                {
                    throw new Exception(string.Format("Unable to open the specified device: {0}", status));
                }
                this.SetBaudRate(9600U);
                this.SetTimeouts(750U, 750U);
                this.SendCommand(new byte[]
                {
                    192,
                    0,
                    0,
                    192
                });
                this.FTDIDevice.SetEventNotification(1U, this.ReceivedDataEvent);
                if (status != null)
                {
                    throw new Exception(string.Format("Unable to set event handler: {0}", status));
                }
                Task.Run(new Action(base.ReadData));
            }
            catch (Exception ex)
            {
                Log.Error("There was an error while opening the device.", ex);
                throw;
            }
        }

        // Token: 0x06000046 RID: 70 RVA: 0x000032DC File Offset: 0x000014DC
        public override void SetPulseDuration(PulseDuration pulseDuration)
        {
            byte[] buffer;
            if ((int)pulseDuration <= 1000)
            {
                if ((int)pulseDuration == 500)
                {
                    buffer = new byte[]
                    {
                        200,
                        1,
                        1,
                        200
                    };
                    goto IL_89;
                }
                if ((int)pulseDuration == 1000)
                {
                    buffer = new byte[]
                    {
                        200,
                        2,
                        2,
                        200
                    };
                    goto IL_89;
                }
            }
            else
            {
                if ((int)pulseDuration == 2000)
                {
                    buffer = new byte[]
                    {
                        200,
                        4,
                        3,
                        207
                    };
                    goto IL_89;
                }
                if ((int)pulseDuration == 30000)
                {
                    buffer = new byte[]
                    {
                        200,
                        64,
                        4,
                        140
                    };
                    goto IL_89;
                }
            }
            throw new Exception("Invalid pulse duration value");
        IL_89:
            this.SendCommand(buffer);
        }

        // Token: 0x06000047 RID: 71 RVA: 0x0000339C File Offset: 0x0000159C
        public override void SetExposureVoltage(Voltage voltage)
        {
            byte[] buffer = new byte[0];
            if ((int)voltage <= 100)
            {
                if ((int)voltage != 50)
                {
                    if ((int)voltage == 100)
                    {
                        buffer = new byte[]
                        {
                            201,
                            1,
                            1,
                            201
                        };
                    }
                }
                else
                {
                    buffer = new byte[]
                    {
                        201,
                        1,
                        2,
                        202
                    };
                }
            }
            else if ((int)voltage != 110)
            {
                if ((int)voltage == 120)
                {
                    buffer = new byte[]
                    {
                        201,
                        1,
                        4,
                        204
                    };
                }
            }
            else
            {
                buffer = new byte[]
                {
                    201,
                    1,
                    3,
                    203
                };
            }
            this.SendCommand(buffer);
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00003444 File Offset: 0x00001644
        public override void SetFrequency(PWM0Frequency frequency)
        {
            byte[] buffer = new byte[0];
            if ((int)frequency <= 900)
            {
                if ((int)frequency != 800)
                {
                    if ((int)frequency == 900)
                    {
                        buffer = new byte[]
                        {
                            202,
                            132,
                            0,
                            78
                        };
                    }
                }
                else
                {
                    buffer = new byte[]
                    {
                        202,
                        116,
                        0,
                        190
                    };
                }
            }
            else if ((int)frequency != 1000)
            {
                if ((int)frequency == 1100)
                {
                    buffer = new byte[]
                    {
                        202,
                        155,
                        0,
                        81
                    };
                }
            }
            else
            {
                buffer = new byte[]
                {
                    202,
                    144,
                    0,
                    90
                };
            }
            this.SendCommand(buffer);
        }
    }
}
