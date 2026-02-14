using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;

namespace EPIC.DataLayer.Customization
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum Gender : int
    {
        Female = 1,
        Male = 2,
        Other = 3,
        Unspecified = 0
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum Voltage : int
    {
        V90 = 90,
        V100 = 100,
        V110 = 110,
        V120 = 120,
        V130 = 130,
        V140 = 140
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum PWM0Frequency : int
    {
        [EnumMember(Value = "P800")]
        [Description("P800")]
        [Display(Description = "P800")]
        P800 = 800,
        P900 = 900,
        P1100 = 1100,
        P1200 = 1200,
        P1300 = 1300,
        P1400 = 1400,
        P1500 = 1500
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum PulseWidth : int
    {
        W5 = 5,
        W6 = 6,
        W7 = 7,
        W8 = 8,
        W9 = 9,
        W10 = 10,
        W11 = 11,
        W12 = 12,
        W13 = 13,
        W14 = 14,
        W15 = 15,
        W16 = 16,
        W17 = 17,
        W18 = 18,
        W19 = 19,
        W20 = 20
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum PulseDuration : int
    {
        MS100 = 100,
        MS200 = 200,
        MS300 = 300,
        MS400 = 400,
        MS500 = 500,
        MS600 = 600,
        MS700 = 700,
        MS800 = 800,
        MS900 = 900,
        MS1000 = 1000,
        MS1100 = 1100,
        MS1200 = 1200,
        MS1300 = 1300,
        MS1400 = 1400,
        MS1500 = 1500,
        MS1600 = 1600,
        MS1700 = 1700,
        MS1800 = 1800,
        MS1900 = 1900,
        MS2000 = 2000,

    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum FirmwareVersion
    {
        Unknown,
        HRFirmware,
        STFirmware
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public enum CameraInterface
    {
        Unknown,
        EmguGeneric,
        SanTech,
        Philips
    }

    // ReSharper disable InconsistentNaming
    //public enum NBAnalysisGroup
    // ReSharper restore InconsistentNaming
    //{
    // The integer values of this enum are stored in the database. So they must never
    // be changed unless all existing records in the database are updated to match the
    // new enum values.
    //	Cardiovascular = 1,
    //	Respiratory = 2,
    //	Hepatic = 3,
    //	Gastrointestinal = 4,
    //	Renal = 5
    //}

    public enum OrganSystem : int
    {
        // The integer values of this enum are stored in the database. So they must never
        // be changed unless all existing records in the database are updated to match the
        // new enum values.
        Cardiovascular = 4,
        Respiratory = 5,
        Hepatic = 2,
        Gastrointestinal = 1,
        Renal = 6
    }

    /// <summary>
    /// Enumeration that defines the different types of organ compoents available in the solution.
    /// </summary>
    /// <remarks>
    /// Organs apply to different sectors on different fingers of a scan.  This enumeration
    /// is used when instanciating a sector and when calculating the final values after image
    /// processing and analysis.
    /// </remarks>
    public enum OrganComponent : int
    {
        // The integer values of this enum are stored in the database. So they must never
        // be changed unless all existing records in the database are updated to match the
        // new enum values.
        NotSet = 0,
        //0 - 9  Cardiovascular
        Heart,
        LeftCardiovascularSystem,
        LeftCoronaryVessels,
        LeftHeart,
        LeftThroatLarynxTracheaThyroid,
        RightCardiovascularSystem,
        RightCoronaryVessels,
        RightHeart,
        RightThroatLarynxTracheaThyroid,
        // 10 - 15 Respiratory 
        LeftLEarNoseMaxillarySinus,
        LeftRespiratoryMammary,
        LeftThoraxRespiratory,
        RightREarNoseMaxillarySinus,
        RightRespiratoryMammary,
        RightThoraxZoneRespiratory,
        // 16 - 21 Gastro Large
        ColonAscending,
        ColonDescending,
        ColonSigmoid,
        LeftColonTransverse,
        Rectum,
        RightColonTransverse,
        // 22 - 26 Stomach/Sm Int
        AbdominalZone,
        BlindGut,
        Duodenum,
        Ileum,
        Jejunum,
        //27 - 31 Immune
        Appendix,
        LeftImmuneSystem,
        LeftSpleen,
        RightImmuneSystem,
        RightSpleen,
        //32 - 39 Renal/Reproductive
        LeftAdrenal,
        LeftKidney,
        LeftUrogenital,
        RightAdrenal,
        RightKidney,
        RightUrogenital,
        UroKidneyLeft,
        UroKidneyRight,
        // 40 - 56 Endocrine
        Gallbladder,
        LeftCerebralCortexZone,
        LeftCerebralVessels,
        LeftHypothalamus,
        LeftLiver,
        LeftPancreas,
        LeftPinealEpiphysis,
        LeftPituitaryHypophysis,
        LeftThyroid,
        RightCerebralCortexZone,
        RightCerebralVessels,
        RightHypothalamus,
        RightLiver,
        RightPancreas,
        RightPinealEpiphysis,
        RightPituitaryHypophysis,
        RightThyroid,
        // 57 -   Skeletal
        LeftCervicalSpine,
        LeftCoccyxPelvis,
        LeftLSideEye,
        LeftLJawTeeth,
        LeftLumbarSpine,
        LeftNervousSystem,
        LeftSacrum,
        LeftThorax,
        RightCervicalSpine,
        RightCoccyxPelvis,
        RightRSideEye,
        RightRJawTeeth,
        RightLumbarSpine,
        RightNervousSystem,
        RightSacrum,
        RightThorax,
        LeftREarNoseMaxillarySinus,
        RightLEarNoseMaxillarySinus,
        RightLJawTeeth,
        LeftRJawTeeth,
        RightLSideEye,
        LeftRSideEye
    };


    //DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)]
    //public static class PatientFields
    //{
    //}
}
