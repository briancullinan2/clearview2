using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.DataLayer.Customization
{
    public enum Gender
    {
        Male,
        Female,
        Other,
        Unspecified
    }

    public enum Voltage
    {
        V900,
        V1000,
        V1100,
        V1200,
        V1300,
        V1400
    }

    public enum PWM0Frequency
    {
        P1100,
        P1120,
        P1140,
        P1160,
        P1180,
        P1200,
        P1220,
        P1240,
        P1260,
        P1280,
        P1300,
        P1320,
        P1340,
        P1360,
        P1380,
        P1400,
        P1420,
        P1440,
        P1460,
        P1480,
        P1500,
        P1520,
        P1540,
        P1560,
        P1580
    }

    public enum PulseWidth
    {
        W100,
        W200,
        W300,
        W400,
        W500,
        W600,
        W700,
        W800,
        W900,
        W1000,
    }

    public enum PulseDuration
    {
        MS100,
        MS200,
        MS300,
        MS400,
        MS500,
        MS600,
        MS700,
        MS800,
        MS900,
        MS1000,
        MS1100,
        MS1200,
        MS1300,
        MS1400,
        MS1500,
        MS1600,
        MS1700,
        MS1800,
        MS1900,
        MS2000,

    }

    public enum FirmwareVersion
    {
        HRFirmware,
        STFirmware
    }

    public enum CameraInterface
    {
        EmguGeneric,
        SanTech,
        Philips
    }

    public static class PatientFields
    {
        public enum Gender
        {
            Male,
            Female,
            Other,
            Unspecified
        }

    }
}
