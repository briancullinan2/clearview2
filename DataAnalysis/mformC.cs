using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.ComponentData;
using MathWorks.MATLAB.NET.Utility;
using System.Reflection;

namespace mlformC
{
    // Token: 0x02000002 RID: 2
    public class formCalc : IDisposable
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00001050
        static formCalc()
        {
            if (MWMCR.MCRAppInitialized)
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string text = executingAssembly.Location;
                int num = text.LastIndexOf("\\");
                text = text.Remove(num, text.Length - num);
                string value = MCRComponentState2.MCC_mlformC_name_data + ".ctf";
                System.IO.Stream embeddedCtfStream = null;
                string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
                foreach (string text2 in manifestResourceNames)
                {
                    if (text2.Contains(value))
                    {
                        embeddedCtfStream = executingAssembly.GetManifestResourceStream(text2);
                        break;
                    }
                }
                formCalc.mcr = new MWMCR(
                    "",                // Component Name (often empty or the DLL name)
                    text,              // Component Root (the path to your extracted folder)
                    embeddedCtfStream,            // The CTF stream you found in the manifest
                    true               // IsEmbedded (true if using the stream)
                );
                return;
            }
            throw new ApplicationException("MWArray assembly could not be initialized");
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000212F File Offset: 0x0000112F
        public formCalc()
        {
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002138 File Offset: 0x00001138
        ~formCalc()
        {
            this.Dispose(false);
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002168 File Offset: 0x00001168
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002177 File Offset: 0x00001177
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x0000218A File Offset: 0x0000118A
        public MWArray calib_js()
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[0]);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000021A4 File Offset: 0x000011A4
        public MWArray calib_js(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000021CC File Offset: 0x000011CC
        public MWArray calib_js(MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000011F8
        public MWArray calib_js(MWArray gdv_image, MWArray pars, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str
            });
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002228 File Offset: 0x00001228
        public MWArray calib_js(MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_path)
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_path
            });
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002260 File Offset: 0x00001260
        public MWArray calib_js(MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_path, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction("calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_path,
                noiselevel
            });
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000229A File Offset: 0x0000129A
        public MWArray[] calib_js(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[0]);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000022B4 File Offset: 0x000012B4
        public MWArray[] calib_js(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000022E0 File Offset: 0x000012E0
        public MWArray[] calib_js(int numArgsOut, MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002310 File Offset: 0x00001310
        public MWArray[] calib_js(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str
            });
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002344 File Offset: 0x00001344
        public MWArray[] calib_js(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_path)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_path
            });
        }

        // Token: 0x06000011 RID: 17 RVA: 0x0000237C File Offset: 0x0000137C
        public MWArray[] calib_js(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_path, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_js", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_path,
                noiselevel
            });
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000023B8 File Offset: 0x000013B8
        public void calib_js(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("calib_js", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000023CC File Offset: 0x000013CC
        public void calib_run()
        {
            formCalc.mcr.EvaluateFunction(0, "calib_run", new MWArray[0]);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000023E5 File Offset: 0x000013E5
        public MWArray[] calib_run(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "calib_run", new MWArray[0]);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000023FD File Offset: 0x000013FD
        public MWArray compute_factors()
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[0]);
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002414 File Offset: 0x00001414
        public MWArray compute_factors(MWArray filename1)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1
            });
        }

        // Token: 0x06000017 RID: 23 RVA: 0x0000243C File Offset: 0x0000143C
        public MWArray compute_factors(MWArray filename1, MWArray filename2)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2
            });
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002468 File Offset: 0x00001468
        public MWArray compute_factors(MWArray filename1, MWArray filename2, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode
            });
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002498 File Offset: 0x00001498
        public MWArray compute_factors(MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path
            });
        }

        // Token: 0x0600001A RID: 26 RVA: 0x000024D0 File Offset: 0x000014D0
        public MWArray compute_factors(MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle
            });
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000250C File Offset: 0x0000150C
        public MWArray compute_factors(MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle,
                midpoint
            });
        }

        // Token: 0x0600001C RID: 28 RVA: 0x0000254C File Offset: 0x0000154C
        public MWArray compute_factors(MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle, MWArray midpoint, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction("compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle,
                midpoint,
                noiselevel
            });
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00002590 File Offset: 0x00001590
        public MWArray[] compute_factors(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[0]);
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000025A8 File Offset: 0x000015A8
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1
            });
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000025D4 File Offset: 0x000015D4
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2
            });
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002604 File Offset: 0x00001604
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode
            });
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002638 File Offset: 0x00001638
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path
            });
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00002670 File Offset: 0x00001670
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle
            });
        }

        // Token: 0x06000023 RID: 35 RVA: 0x000026AC File Offset: 0x000016AC
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle,
                midpoint
            });
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000026F0 File Offset: 0x000016F0
        public MWArray[] compute_factors(int numArgsOut, MWArray filename1, MWArray filename2, MWArray mode, MWArray calib_path, MWArray im_angle, MWArray midpoint, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "compute_factors", new MWArray[]
            {
                filename1,
                filename2,
                mode,
                calib_path,
                im_angle,
                midpoint,
                noiselevel
            });
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00002736 File Offset: 0x00001736
        public void compute_factors(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("compute_factors", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000026 RID: 38 RVA: 0x0000274A File Offset: 0x0000174A
        public MWArray convcirc()
        {
            return formCalc.mcr.EvaluateFunction("convcirc", new MWArray[0]);
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00002764 File Offset: 0x00001764
        public MWArray convcirc(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("convcirc", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000028 RID: 40 RVA: 0x0000278C File Offset: 0x0000178C
        public MWArray convcirc(MWArray x, MWArray y)
        {
            return formCalc.mcr.EvaluateFunction("convcirc", new MWArray[]
            {
                x,
                y
            });
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000027B8 File Offset: 0x000017B8
        public MWArray[] convcirc(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "convcirc", new MWArray[0]);
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000027D0 File Offset: 0x000017D0
        public MWArray[] convcirc(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "convcirc", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000027FC File Offset: 0x000017FC
        public MWArray[] convcirc(int numArgsOut, MWArray x, MWArray y)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "convcirc", new MWArray[]
            {
                x,
                y
            });
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00002829 File Offset: 0x00001829
        public void convcirc(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("convcirc", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600002D RID: 45 RVA: 0x0000283D File Offset: 0x0000183D
        public MWArray diffnorm()
        {
            return formCalc.mcr.EvaluateFunction("diffnorm", new MWArray[0]);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00002854 File Offset: 0x00001854
        public MWArray diffnorm(MWArray f)
        {
            return formCalc.mcr.EvaluateFunction("diffnorm", new MWArray[]
            {
                f
            });
        }

        // Token: 0x0600002F RID: 47 RVA: 0x0000287C File Offset: 0x0000187C
        public MWArray diffnorm(MWArray f, MWArray order)
        {
            return formCalc.mcr.EvaluateFunction("diffnorm", new MWArray[]
            {
                f,
                order
            });
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000028A8 File Offset: 0x000018A8
        public MWArray[] diffnorm(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "diffnorm", new MWArray[0]);
        }

        // Token: 0x06000031 RID: 49 RVA: 0x000028C0 File Offset: 0x000018C0
        public MWArray[] diffnorm(int numArgsOut, MWArray f)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "diffnorm", new MWArray[]
            {
                f
            });
        }

        // Token: 0x06000032 RID: 50 RVA: 0x000028EC File Offset: 0x000018EC
        public MWArray[] diffnorm(int numArgsOut, MWArray f, MWArray order)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "diffnorm", new MWArray[]
            {
                f,
                order
            });
        }

        // Token: 0x06000033 RID: 51 RVA: 0x00002919 File Offset: 0x00001919
        public void diffnorm(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("diffnorm", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000034 RID: 52 RVA: 0x0000292D File Offset: 0x0000192D
        public MWArray findbreakprofiles()
        {
            return formCalc.mcr.EvaluateFunction("findbreakprofiles", new MWArray[0]);
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00002944 File Offset: 0x00001944
        public MWArray findbreakprofiles(MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction("findbreakprofiles", new MWArray[]
            {
                profiles
            });
        }

        // Token: 0x06000036 RID: 54 RVA: 0x0000296C File Offset: 0x0000196C
        public MWArray[] findbreakprofiles(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findbreakprofiles", new MWArray[0]);
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00002984 File Offset: 0x00001984
        public MWArray[] findbreakprofiles(int numArgsOut, MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findbreakprofiles", new MWArray[]
            {
                profiles
            });
        }

        // Token: 0x06000038 RID: 56 RVA: 0x000029AD File Offset: 0x000019AD
        public void findbreakprofiles(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("findbreakprofiles", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000039 RID: 57 RVA: 0x000029C1 File Offset: 0x000019C1
        public MWArray findgap()
        {
            return formCalc.mcr.EvaluateFunction("findgap", new MWArray[0]);
        }

        // Token: 0x0600003A RID: 58 RVA: 0x000029D8 File Offset: 0x000019D8
        public MWArray findgap(MWArray ray_sec)
        {
            return formCalc.mcr.EvaluateFunction("findgap", new MWArray[]
            {
                ray_sec
            });
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002A00 File Offset: 0x00001A00
        public MWArray findgap(MWArray ray_sec, MWArray index_length)
        {
            return formCalc.mcr.EvaluateFunction("findgap", new MWArray[]
            {
                ray_sec,
                index_length
            });
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00002A2C File Offset: 0x00001A2C
        public MWArray findgap(MWArray ray_sec, MWArray index_length, MWArray min_intensity)
        {
            return formCalc.mcr.EvaluateFunction("findgap", new MWArray[]
            {
                ray_sec,
                index_length,
                min_intensity
            });
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00002A5C File Offset: 0x00001A5C
        public MWArray[] findgap(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findgap", new MWArray[0]);
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00002A74 File Offset: 0x00001A74
        public MWArray[] findgap(int numArgsOut, MWArray ray_sec)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findgap", new MWArray[]
            {
                ray_sec
            });
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002AA0 File Offset: 0x00001AA0
        public MWArray[] findgap(int numArgsOut, MWArray ray_sec, MWArray index_length)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findgap", new MWArray[]
            {
                ray_sec,
                index_length
            });
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00002AD0 File Offset: 0x00001AD0
        public MWArray[] findgap(int numArgsOut, MWArray ray_sec, MWArray index_length, MWArray min_intensity)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findgap", new MWArray[]
            {
                ray_sec,
                index_length,
                min_intensity
            });
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002B02 File Offset: 0x00001B02
        public void findgap(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("findgap", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00002B16 File Offset: 0x00001B16
        public MWArray finger2sectors()
        {
            return formCalc.mcr.EvaluateFunction("finger2sectors", new MWArray[0]);
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00002B30 File Offset: 0x00001B30
        public MWArray finger2sectors(MWArray str)
        {
            return formCalc.mcr.EvaluateFunction("finger2sectors", new MWArray[]
            {
                str
            });
        }

        // Token: 0x06000044 RID: 68 RVA: 0x00002B58 File Offset: 0x00001B58
        public MWArray[] finger2sectors(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "finger2sectors", new MWArray[0]);
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00002B70 File Offset: 0x00001B70
        public MWArray[] finger2sectors(int numArgsOut, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "finger2sectors", new MWArray[]
            {
                str
            });
        }

        // Token: 0x06000046 RID: 70 RVA: 0x00002B99 File Offset: 0x00001B99
        public void finger2sectors(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("finger2sectors", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000047 RID: 71 RVA: 0x00002BAD File Offset: 0x00001BAD
        public MWArray fit_ell_sub()
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[0]);
        }

        // Token: 0x06000048 RID: 72 RVA: 0x00002BC4 File Offset: 0x00001BC4
        public MWArray fit_ell_sub(MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[]
            {
                p
            });
        }

        // Token: 0x06000049 RID: 73 RVA: 0x00002BEC File Offset: 0x00001BEC
        public MWArray fit_ell_sub(MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x0600004A RID: 74 RVA: 0x00002C18 File Offset: 0x00001C18
        public MWArray fit_ell_sub(MWArray p, MWArray q, MWArray a0)
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0
            });
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00002C48 File Offset: 0x00001C48
        public MWArray fit_ell_sub(MWArray p, MWArray q, MWArray a0, MWArray b0)
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0,
                b0
            });
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00002C80 File Offset: 0x00001C80
        public MWArray fit_ell_sub(MWArray p, MWArray q, MWArray a0, MWArray b0, MWArray phi)
        {
            return formCalc.mcr.EvaluateFunction("fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0,
                b0,
                phi
            });
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00002CBA File Offset: 0x00001CBA
        public MWArray[] fit_ell_sub(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[0]);
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00002CD4 File Offset: 0x00001CD4
        public MWArray[] fit_ell_sub(int numArgsOut, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[]
            {
                p
            });
        }

        // Token: 0x0600004F RID: 79 RVA: 0x00002D00 File Offset: 0x00001D00
        public MWArray[] fit_ell_sub(int numArgsOut, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00002D30 File Offset: 0x00001D30
        public MWArray[] fit_ell_sub(int numArgsOut, MWArray p, MWArray q, MWArray a0)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0
            });
        }

        // Token: 0x06000051 RID: 81 RVA: 0x00002D64 File Offset: 0x00001D64
        public MWArray[] fit_ell_sub(int numArgsOut, MWArray p, MWArray q, MWArray a0, MWArray b0)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0,
                b0
            });
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00002D9C File Offset: 0x00001D9C
        public MWArray[] fit_ell_sub(int numArgsOut, MWArray p, MWArray q, MWArray a0, MWArray b0, MWArray phi)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ell_sub", new MWArray[]
            {
                p,
                q,
                a0,
                b0,
                phi
            });
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00002DD8 File Offset: 0x00001DD8
        public void fit_ell_sub(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("fit_ell_sub", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000054 RID: 84 RVA: 0x00002DEC File Offset: 0x00001DEC
        public MWArray fit_ellipse()
        {
            return formCalc.mcr.EvaluateFunction("fit_ellipse", new MWArray[0]);
        }

        // Token: 0x06000055 RID: 85 RVA: 0x00002E04 File Offset: 0x00001E04
        public MWArray fit_ellipse(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("fit_ellipse", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000056 RID: 86 RVA: 0x00002E2C File Offset: 0x00001E2C
        public MWArray fit_ellipse(MWArray x, MWArray y)
        {
            return formCalc.mcr.EvaluateFunction("fit_ellipse", new MWArray[]
            {
                x,
                y
            });
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00002E58 File Offset: 0x00001E58
        public MWArray[] fit_ellipse(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ellipse", new MWArray[0]);
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00002E70 File Offset: 0x00001E70
        public MWArray[] fit_ellipse(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ellipse", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00002E9C File Offset: 0x00001E9C
        public MWArray[] fit_ellipse(int numArgsOut, MWArray x, MWArray y)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fit_ellipse", new MWArray[]
            {
                x,
                y
            });
        }

        // Token: 0x0600005A RID: 90 RVA: 0x00002EC9 File Offset: 0x00001EC9
        public void fit_ellipse(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("fit_ellipse", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00002EDD File Offset: 0x00001EDD
        public MWArray fitell()
        {
            return formCalc.mcr.EvaluateFunction("fitell", new MWArray[0]);
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00002EF4 File Offset: 0x00001EF4
        public MWArray fitell(MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("fitell", new MWArray[]
            {
                p
            });
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00002F1C File Offset: 0x00001F1C
        public MWArray fitell(MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction("fitell", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00002F48 File Offset: 0x00001F48
        public MWArray[] fitell(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fitell", new MWArray[0]);
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00002F60 File Offset: 0x00001F60
        public MWArray[] fitell(int numArgsOut, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fitell", new MWArray[]
            {
                p
            });
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00002F8C File Offset: 0x00001F8C
        public MWArray[] fitell(int numArgsOut, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fitell", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x06000061 RID: 97 RVA: 0x00002FB9 File Offset: 0x00001FB9
        public void fitell(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("fitell", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00002FCD File Offset: 0x00001FCD
        public MWArray formc()
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[0]);
        }

        // Token: 0x06000063 RID: 99 RVA: 0x00002FE4 File Offset: 0x00001FE4
        public MWArray formc(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000064 RID: 100 RVA: 0x0000300C File Offset: 0x0000200C
        public MWArray formc(MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00003038 File Offset: 0x00002038
        public MWArray formc(MWArray gdv_image, MWArray pars, MWArray angle_start)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start
            });
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00003068 File Offset: 0x00002068
        public MWArray formc(MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end
            });
        }

        // Token: 0x06000067 RID: 103 RVA: 0x000030A0 File Offset: 0x000020A0
        public MWArray formc(MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level
            });
        }

        // Token: 0x06000068 RID: 104 RVA: 0x000030DC File Offset: 0x000020DC
        public MWArray formc(MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level, MWArray pnorm)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level,
                pnorm
            });
        }

        // Token: 0x06000069 RID: 105 RVA: 0x0000311C File Offset: 0x0000211C
        public MWArray formc(MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level, MWArray pnorm, MWArray smoothpar)
        {
            return formCalc.mcr.EvaluateFunction("formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level,
                pnorm,
                smoothpar
            });
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00003160 File Offset: 0x00002160
        public MWArray[] formc(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[0]);
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00003178 File Offset: 0x00002178
        public MWArray[] formc(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x0600006C RID: 108 RVA: 0x000031A4 File Offset: 0x000021A4
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000031D4 File Offset: 0x000021D4
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray angle_start)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start
            });
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00003208 File Offset: 0x00002208
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end
            });
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00003240 File Offset: 0x00002240
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level
            });
        }

        // Token: 0x06000070 RID: 112 RVA: 0x0000327C File Offset: 0x0000227C
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level, MWArray pnorm)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level,
                pnorm
            });
        }

        // Token: 0x06000071 RID: 113 RVA: 0x000032C0 File Offset: 0x000022C0
        public MWArray[] formc(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray angle_start, MWArray angle_end, MWArray level, MWArray pnorm, MWArray smoothpar)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formc", new MWArray[]
            {
                gdv_image,
                pars,
                angle_start,
                angle_end,
                level,
                pnorm,
                smoothpar
            });
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00003306 File Offset: 0x00002306
        public void formc(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("formc", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000073 RID: 115 RVA: 0x0000331A File Offset: 0x0000231A
        public MWArray formjs_ns()
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[0]);
        }

        // Token: 0x06000074 RID: 116 RVA: 0x00003334 File Offset: 0x00002334
        public MWArray formjs_ns(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000075 RID: 117 RVA: 0x0000335C File Offset: 0x0000235C
        public MWArray formjs_ns(MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x06000076 RID: 118 RVA: 0x00003388 File Offset: 0x00002388
        public MWArray formjs_ns(MWArray gdv_image, MWArray pars, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str
            });
        }

        // Token: 0x06000077 RID: 119 RVA: 0x000033B8 File Offset: 0x000023B8
        public MWArray formjs_ns(MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_par)
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_par
            });
        }

        // Token: 0x06000078 RID: 120 RVA: 0x000033F0 File Offset: 0x000023F0
        public MWArray formjs_ns(MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_par, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction("formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_par,
                noiselevel
            });
        }

        // Token: 0x06000079 RID: 121 RVA: 0x0000342A File Offset: 0x0000242A
        public MWArray[] formjs_ns(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[0]);
        }

        // Token: 0x0600007A RID: 122 RVA: 0x00003444 File Offset: 0x00002444
        public MWArray[] formjs_ns(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x0600007B RID: 123 RVA: 0x00003470 File Offset: 0x00002470
        public MWArray[] formjs_ns(int numArgsOut, MWArray gdv_image, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[]
            {
                gdv_image,
                pars
            });
        }

        // Token: 0x0600007C RID: 124 RVA: 0x000034A0 File Offset: 0x000024A0
        public MWArray[] formjs_ns(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str
            });
        }

        // Token: 0x0600007D RID: 125 RVA: 0x000034D4 File Offset: 0x000024D4
        public MWArray[] formjs_ns(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_par)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_par
            });
        }

        // Token: 0x0600007E RID: 126 RVA: 0x0000350C File Offset: 0x0000250C
        public MWArray[] formjs_ns(int numArgsOut, MWArray gdv_image, MWArray pars, MWArray str, MWArray calib_par, MWArray noiselevel)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "formjs_ns", new MWArray[]
            {
                gdv_image,
                pars,
                str,
                calib_par,
                noiselevel
            });
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00003548 File Offset: 0x00002548
        public void formjs_ns(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("formjs_ns", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000080 RID: 128 RVA: 0x0000355C File Offset: 0x0000255C
        public MWArray gauss()
        {
            return formCalc.mcr.EvaluateFunction("gauss", new MWArray[0]);
        }

        // Token: 0x06000081 RID: 129 RVA: 0x00003574 File Offset: 0x00002574
        public MWArray gauss(MWArray n)
        {
            return formCalc.mcr.EvaluateFunction("gauss", new MWArray[]
            {
                n
            });
        }

        // Token: 0x06000082 RID: 130 RVA: 0x0000359C File Offset: 0x0000259C
        public MWArray gauss(MWArray n, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction("gauss", new MWArray[]
            {
                n,
                alpha
            });
        }

        // Token: 0x06000083 RID: 131 RVA: 0x000035C8 File Offset: 0x000025C8
        public MWArray[] gauss(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "gauss", new MWArray[0]);
        }

        // Token: 0x06000084 RID: 132 RVA: 0x000035E0 File Offset: 0x000025E0
        public MWArray[] gauss(int numArgsOut, MWArray n)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "gauss", new MWArray[]
            {
                n
            });
        }

        // Token: 0x06000085 RID: 133 RVA: 0x0000360C File Offset: 0x0000260C
        public MWArray[] gauss(int numArgsOut, MWArray n, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "gauss", new MWArray[]
            {
                n,
                alpha
            });
        }

        // Token: 0x06000086 RID: 134 RVA: 0x00003639 File Offset: 0x00002639
        public void gauss(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("gauss", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000087 RID: 135 RVA: 0x0000364D File Offset: 0x0000264D
        public MWArray getcont5()
        {
            return formCalc.mcr.EvaluateFunction("getcont5", new MWArray[0]);
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00003664 File Offset: 0x00002664
        public MWArray getcont5(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("getcont5", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000089 RID: 137 RVA: 0x0000368C File Offset: 0x0000268C
        public MWArray getcont5(MWArray x, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("getcont5", new MWArray[]
            {
                x,
                pars
            });
        }

        // Token: 0x0600008A RID: 138 RVA: 0x000036B8 File Offset: 0x000026B8
        public MWArray[] getcont5(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcont5", new MWArray[0]);
        }

        // Token: 0x0600008B RID: 139 RVA: 0x000036D0 File Offset: 0x000026D0
        public MWArray[] getcont5(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcont5", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600008C RID: 140 RVA: 0x000036FC File Offset: 0x000026FC
        public MWArray[] getcont5(int numArgsOut, MWArray x, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcont5", new MWArray[]
            {
                x,
                pars
            });
        }

        // Token: 0x0600008D RID: 141 RVA: 0x00003729 File Offset: 0x00002729
        public void getcont5(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("getcont5", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600008E RID: 142 RVA: 0x0000373D File Offset: 0x0000273D
        public MWArray getcontours()
        {
            return formCalc.mcr.EvaluateFunction("getcontours", new MWArray[0]);
        }

        // Token: 0x0600008F RID: 143 RVA: 0x00003754 File Offset: 0x00002754
        public MWArray getcontours(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("getcontours", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000090 RID: 144 RVA: 0x0000377C File Offset: 0x0000277C
        public MWArray getcontours(MWArray x, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("getcontours", new MWArray[]
            {
                x,
                pars
            });
        }

        // Token: 0x06000091 RID: 145 RVA: 0x000037A8 File Offset: 0x000027A8
        public MWArray[] getcontours(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcontours", new MWArray[0]);
        }

        // Token: 0x06000092 RID: 146 RVA: 0x000037C0 File Offset: 0x000027C0
        public MWArray[] getcontours(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcontours", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000093 RID: 147 RVA: 0x000037EC File Offset: 0x000027EC
        public MWArray[] getcontours(int numArgsOut, MWArray x, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getcontours", new MWArray[]
            {
                x,
                pars
            });
        }

        // Token: 0x06000094 RID: 148 RVA: 0x00003819 File Offset: 0x00002819
        public void getcontours(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("getcontours", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000095 RID: 149 RVA: 0x0000382D File Offset: 0x0000282D
        public MWArray im2ellit()
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[0]);
        }

        // Token: 0x06000096 RID: 150 RVA: 0x00003844 File Offset: 0x00002844
        public MWArray im2ellit(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000097 RID: 151 RVA: 0x0000386C File Offset: 0x0000286C
        public MWArray im2ellit(MWArray x, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x,
                extract_mode
            });
        }

        // Token: 0x06000098 RID: 152 RVA: 0x00003898 File Offset: 0x00002898
        public MWArray im2ellit(MWArray x, MWArray extract_mode, MWArray smoothflag)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag
            });
        }

        // Token: 0x06000099 RID: 153 RVA: 0x000038C8 File Offset: 0x000028C8
        public MWArray im2ellit(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag
            });
        }

        // Token: 0x0600009A RID: 154 RVA: 0x00003900 File Offset: 0x00002900
        public MWArray im2ellit(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter
            });
        }

        // Token: 0x0600009B RID: 155 RVA: 0x0000393C File Offset: 0x0000293C
        public MWArray im2ellit(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level
            });
        }

        // Token: 0x0600009C RID: 156 RVA: 0x0000397B File Offset: 0x0000297B
        public MWArray[] im2ellit(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[0]);
        }

        // Token: 0x0600009D RID: 157 RVA: 0x00003994 File Offset: 0x00002994
        public MWArray[] im2ellit(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600009E RID: 158 RVA: 0x000039C0 File Offset: 0x000029C0
        public MWArray[] im2ellit(int numArgsOut, MWArray x, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x,
                extract_mode
            });
        }

        // Token: 0x0600009F RID: 159 RVA: 0x000039F0 File Offset: 0x000029F0
        public MWArray[] im2ellit(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag
            });
        }

        // Token: 0x060000A0 RID: 160 RVA: 0x00003A24 File Offset: 0x00002A24
        public MWArray[] im2ellit(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag
            });
        }

        // Token: 0x060000A1 RID: 161 RVA: 0x00003A5C File Offset: 0x00002A5C
        public MWArray[] im2ellit(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter
            });
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00003A98 File Offset: 0x00002A98
        public MWArray[] im2ellit(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellit", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level
            });
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x00003AD9 File Offset: 0x00002AD9
        public void im2ellit(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("im2ellit", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x00003AED File Offset: 0x00002AED
        public MWArray im2pix()
        {
            return formCalc.mcr.EvaluateFunction("im2pix", new MWArray[0]);
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00003B04 File Offset: 0x00002B04
        public MWArray im2pix(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("im2pix", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x00003B2C File Offset: 0x00002B2C
        public MWArray im2pix(MWArray gdv_image, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction("im2pix", new MWArray[]
            {
                gdv_image,
                extract_mode
            });
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00003B58 File Offset: 0x00002B58
        public MWArray im2pix(MWArray gdv_image, MWArray extract_mode, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("im2pix", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level
            });
        }

        // Token: 0x060000A8 RID: 168 RVA: 0x00003B88 File Offset: 0x00002B88
        public MWArray[] im2pix(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pix", new MWArray[0]);
        }

        // Token: 0x060000A9 RID: 169 RVA: 0x00003BA0 File Offset: 0x00002BA0
        public MWArray[] im2pix(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pix", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x060000AA RID: 170 RVA: 0x00003BCC File Offset: 0x00002BCC
        public MWArray[] im2pix(int numArgsOut, MWArray gdv_image, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pix", new MWArray[]
            {
                gdv_image,
                extract_mode
            });
        }

        // Token: 0x060000AB RID: 171 RVA: 0x00003BFC File Offset: 0x00002BFC
        public MWArray[] im2pix(int numArgsOut, MWArray gdv_image, MWArray extract_mode, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pix", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level
            });
        }

        // Token: 0x060000AC RID: 172 RVA: 0x00003C2E File Offset: 0x00002C2E
        public void im2pix(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("im2pix", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000AD RID: 173 RVA: 0x00003C42 File Offset: 0x00002C42
        public void ImagetoColorMap()
        {
            formCalc.mcr.EvaluateFunction(0, "ImagetoColorMap", new MWArray[0]);
        }

        // Token: 0x060000AE RID: 174 RVA: 0x00003C5C File Offset: 0x00002C5C
        public void ImagetoColorMap(MWArray epicImage)
        {
            formCalc.mcr.EvaluateFunction(0, "ImagetoColorMap", new MWArray[]
            {
                epicImage
            });
        }

        // Token: 0x060000AF RID: 175 RVA: 0x00003C88 File Offset: 0x00002C88
        public void ImagetoColorMap(MWArray epicImage, MWArray outfileName)
        {
            formCalc.mcr.EvaluateFunction(0, "ImagetoColorMap", new MWArray[]
            {
                epicImage,
                outfileName
            });
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x00003CB6 File Offset: 0x00002CB6
        public MWArray[] ImagetoColorMap(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ImagetoColorMap", new MWArray[0]);
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x00003CD0 File Offset: 0x00002CD0
        public MWArray[] ImagetoColorMap(int numArgsOut, MWArray epicImage)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ImagetoColorMap", new MWArray[]
            {
                epicImage
            });
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x00003CFC File Offset: 0x00002CFC
        public MWArray[] ImagetoColorMap(int numArgsOut, MWArray epicImage, MWArray outfileName)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ImagetoColorMap", new MWArray[]
            {
                epicImage,
                outfileName
            });
        }

        // Token: 0x060000B3 RID: 179 RVA: 0x00003D29 File Offset: 0x00002D29
        public MWArray imangle2ell()
        {
            return formCalc.mcr.EvaluateFunction("imangle2ell", new MWArray[0]);
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x00003D40 File Offset: 0x00002D40
        public MWArray imangle2ell(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("imangle2ell", new MWArray[]
            {
                x
            });
        }

        // Token: 0x060000B5 RID: 181 RVA: 0x00003D68 File Offset: 0x00002D68
        public MWArray imangle2ell(MWArray x, MWArray im_angle)
        {
            return formCalc.mcr.EvaluateFunction("imangle2ell", new MWArray[]
            {
                x,
                im_angle
            });
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x00003D94 File Offset: 0x00002D94
        public MWArray[] imangle2ell(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "imangle2ell", new MWArray[0]);
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00003DAC File Offset: 0x00002DAC
        public MWArray[] imangle2ell(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "imangle2ell", new MWArray[]
            {
                x
            });
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00003DD8 File Offset: 0x00002DD8
        public MWArray[] imangle2ell(int numArgsOut, MWArray x, MWArray im_angle)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "imangle2ell", new MWArray[]
            {
                x,
                im_angle
            });
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x00003E05 File Offset: 0x00002E05
        public void imangle2ell(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("imangle2ell", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000BA RID: 186 RVA: 0x00003E19 File Offset: 0x00002E19
        public MWArray normcols()
        {
            return formCalc.mcr.EvaluateFunction("normcols", new MWArray[0]);
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00003E30 File Offset: 0x00002E30
        public MWArray normcols(MWArray A)
        {
            return formCalc.mcr.EvaluateFunction("normcols", new MWArray[]
            {
                A
            });
        }

        // Token: 0x060000BC RID: 188 RVA: 0x00003E58 File Offset: 0x00002E58
        public MWArray normcols(MWArray A, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("normcols", new MWArray[]
            {
                A,
                p
            });
        }

        // Token: 0x060000BD RID: 189 RVA: 0x00003E84 File Offset: 0x00002E84
        public MWArray normcols(MWArray A, MWArray p, MWArray flag)
        {
            return formCalc.mcr.EvaluateFunction("normcols", new MWArray[]
            {
                A,
                p,
                flag
            });
        }

        // Token: 0x060000BE RID: 190 RVA: 0x00003EB4 File Offset: 0x00002EB4
        public MWArray[] normcols(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "normcols", new MWArray[0]);
        }

        // Token: 0x060000BF RID: 191 RVA: 0x00003ECC File Offset: 0x00002ECC
        public MWArray[] normcols(int numArgsOut, MWArray A)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "normcols", new MWArray[]
            {
                A
            });
        }

        // Token: 0x060000C0 RID: 192 RVA: 0x00003EF8 File Offset: 0x00002EF8
        public MWArray[] normcols(int numArgsOut, MWArray A, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "normcols", new MWArray[]
            {
                A,
                p
            });
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x00003F28 File Offset: 0x00002F28
        public MWArray[] normcols(int numArgsOut, MWArray A, MWArray p, MWArray flag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "normcols", new MWArray[]
            {
                A,
                p,
                flag
            });
        }

        // Token: 0x060000C2 RID: 194 RVA: 0x00003F5A File Offset: 0x00002F5A
        public void normcols(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("normcols", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000C3 RID: 195 RVA: 0x00003F6E File Offset: 0x00002F6E
        public MWArray plotray()
        {
            return formCalc.mcr.EvaluateFunction("plotray", new MWArray[0]);
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x00003F88 File Offset: 0x00002F88
        public MWArray plotray(MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction("plotray", new MWArray[]
            {
                mm
            });
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x00003FB0 File Offset: 0x00002FB0
        public MWArray plotray(MWArray mm, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction("plotray", new MWArray[]
            {
                mm,
                alpha
            });
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x00003FDC File Offset: 0x00002FDC
        public MWArray plotray(MWArray mm, MWArray alpha, MWArray str_color)
        {
            return formCalc.mcr.EvaluateFunction("plotray", new MWArray[]
            {
                mm,
                alpha,
                str_color
            });
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x0000400C File Offset: 0x0000300C
        public MWArray[] plotray(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotray", new MWArray[0]);
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x00004024 File Offset: 0x00003024
        public MWArray[] plotray(int numArgsOut, MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotray", new MWArray[]
            {
                mm
            });
        }

        // Token: 0x060000C9 RID: 201 RVA: 0x00004050 File Offset: 0x00003050
        public MWArray[] plotray(int numArgsOut, MWArray mm, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotray", new MWArray[]
            {
                mm,
                alpha
            });
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00004080 File Offset: 0x00003080
        public MWArray[] plotray(int numArgsOut, MWArray mm, MWArray alpha, MWArray str_color)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotray", new MWArray[]
            {
                mm,
                alpha,
                str_color
            });
        }

        // Token: 0x060000CB RID: 203 RVA: 0x000040B2 File Offset: 0x000030B2
        public void plotray(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("plotray", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000CC RID: 204 RVA: 0x000040C6 File Offset: 0x000030C6
        public MWArray plotsectors()
        {
            return formCalc.mcr.EvaluateFunction("plotsectors", new MWArray[0]);
        }

        // Token: 0x060000CD RID: 205 RVA: 0x000040E0 File Offset: 0x000030E0
        public MWArray plotsectors(MWArray sec_angles)
        {
            return formCalc.mcr.EvaluateFunction("plotsectors", new MWArray[]
            {
                sec_angles
            });
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00004108 File Offset: 0x00003108
        public MWArray plotsectors(MWArray sec_angles, MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction("plotsectors", new MWArray[]
            {
                sec_angles,
                mm
            });
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00004134 File Offset: 0x00003134
        public MWArray plotsectors(MWArray sec_angles, MWArray mm, MWArray textflag)
        {
            return formCalc.mcr.EvaluateFunction("plotsectors", new MWArray[]
            {
                sec_angles,
                mm,
                textflag
            });
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00004164 File Offset: 0x00003164
        public MWArray[] plotsectors(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotsectors", new MWArray[0]);
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x0000417C File Offset: 0x0000317C
        public MWArray[] plotsectors(int numArgsOut, MWArray sec_angles)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotsectors", new MWArray[]
            {
                sec_angles
            });
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x000041A8 File Offset: 0x000031A8
        public MWArray[] plotsectors(int numArgsOut, MWArray sec_angles, MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotsectors", new MWArray[]
            {
                sec_angles,
                mm
            });
        }

        // Token: 0x060000D3 RID: 211 RVA: 0x000041D8 File Offset: 0x000031D8
        public MWArray[] plotsectors(int numArgsOut, MWArray sec_angles, MWArray mm, MWArray textflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "plotsectors", new MWArray[]
            {
                sec_angles,
                mm,
                textflag
            });
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x0000420A File Offset: 0x0000320A
        public void plotsectors(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("plotsectors", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000D5 RID: 213 RVA: 0x0000421E File Offset: 0x0000321E
        public MWArray rem_out()
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[0]);
        }

        // Token: 0x060000D6 RID: 214 RVA: 0x00004238 File Offset: 0x00003238
        public MWArray rem_out(MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x00004260 File Offset: 0x00003260
        public MWArray rem_out(MWArray pars, MWArray fx)
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[]
            {
                pars,
                fx
            });
        }

        // Token: 0x060000D8 RID: 216 RVA: 0x0000428C File Offset: 0x0000328C
        public MWArray rem_out(MWArray pars, MWArray fx, MWArray fy)
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[]
            {
                pars,
                fx,
                fy
            });
        }

        // Token: 0x060000D9 RID: 217 RVA: 0x000042BC File Offset: 0x000032BC
        public MWArray rem_out(MWArray pars, MWArray fx, MWArray fy, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[]
            {
                pars,
                fx,
                fy,
                p
            });
        }

        // Token: 0x060000DA RID: 218 RVA: 0x000042F4 File Offset: 0x000032F4
        public MWArray rem_out(MWArray pars, MWArray fx, MWArray fy, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction("rem_out", new MWArray[]
            {
                pars,
                fx,
                fy,
                p,
                q
            });
        }

        // Token: 0x060000DB RID: 219 RVA: 0x0000432E File Offset: 0x0000332E
        public MWArray[] rem_out(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[0]);
        }

        // Token: 0x060000DC RID: 220 RVA: 0x00004348 File Offset: 0x00003348
        public MWArray[] rem_out(int numArgsOut, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x060000DD RID: 221 RVA: 0x00004374 File Offset: 0x00003374
        public MWArray[] rem_out(int numArgsOut, MWArray pars, MWArray fx)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[]
            {
                pars,
                fx
            });
        }

        // Token: 0x060000DE RID: 222 RVA: 0x000043A4 File Offset: 0x000033A4
        public MWArray[] rem_out(int numArgsOut, MWArray pars, MWArray fx, MWArray fy)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[]
            {
                pars,
                fx,
                fy
            });
        }

        // Token: 0x060000DF RID: 223 RVA: 0x000043D8 File Offset: 0x000033D8
        public MWArray[] rem_out(int numArgsOut, MWArray pars, MWArray fx, MWArray fy, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[]
            {
                pars,
                fx,
                fy,
                p
            });
        }

        // Token: 0x060000E0 RID: 224 RVA: 0x00004410 File Offset: 0x00003410
        public MWArray[] rem_out(int numArgsOut, MWArray pars, MWArray fx, MWArray fy, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_out", new MWArray[]
            {
                pars,
                fx,
                fy,
                p,
                q
            });
        }

        // Token: 0x060000E1 RID: 225 RVA: 0x0000444C File Offset: 0x0000344C
        public void rem_out(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("rem_out", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000E2 RID: 226 RVA: 0x00004460 File Offset: 0x00003460
        public MWArray rem_outliers()
        {
            return formCalc.mcr.EvaluateFunction("rem_outliers", new MWArray[0]);
        }

        // Token: 0x060000E3 RID: 227 RVA: 0x00004478 File Offset: 0x00003478
        public MWArray rem_outliers(MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("rem_outliers", new MWArray[]
            {
                p
            });
        }

        // Token: 0x060000E4 RID: 228 RVA: 0x000044A0 File Offset: 0x000034A0
        public MWArray rem_outliers(MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction("rem_outliers", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x060000E5 RID: 229 RVA: 0x000044CC File Offset: 0x000034CC
        public MWArray[] rem_outliers(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_outliers", new MWArray[0]);
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x000044E4 File Offset: 0x000034E4
        public MWArray[] rem_outliers(int numArgsOut, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_outliers", new MWArray[]
            {
                p
            });
        }

        // Token: 0x060000E7 RID: 231 RVA: 0x00004510 File Offset: 0x00003510
        public MWArray[] rem_outliers(int numArgsOut, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rem_outliers", new MWArray[]
            {
                p,
                q
            });
        }

        // Token: 0x060000E8 RID: 232 RVA: 0x0000453D File Offset: 0x0000353D
        public void rem_outliers(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("rem_outliers", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x00004551 File Offset: 0x00003551
        public MWArray rot()
        {
            return formCalc.mcr.EvaluateFunction("rot", new MWArray[0]);
        }

        // Token: 0x060000EA RID: 234 RVA: 0x00004568 File Offset: 0x00003568
        public MWArray rot(MWArray B)
        {
            return formCalc.mcr.EvaluateFunction("rot", new MWArray[]
            {
                B
            });
        }

        // Token: 0x060000EB RID: 235 RVA: 0x00004590 File Offset: 0x00003590
        public MWArray rot(MWArray B, MWArray a)
        {
            return formCalc.mcr.EvaluateFunction("rot", new MWArray[]
            {
                B,
                a
            });
        }

        // Token: 0x060000EC RID: 236 RVA: 0x000045BC File Offset: 0x000035BC
        public MWArray[] rot(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rot", new MWArray[0]);
        }

        // Token: 0x060000ED RID: 237 RVA: 0x000045D4 File Offset: 0x000035D4
        public MWArray[] rot(int numArgsOut, MWArray B)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rot", new MWArray[]
            {
                B
            });
        }

        // Token: 0x060000EE RID: 238 RVA: 0x00004600 File Offset: 0x00003600
        public MWArray[] rot(int numArgsOut, MWArray B, MWArray a)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rot", new MWArray[]
            {
                B,
                a
            });
        }

        // Token: 0x060000EF RID: 239 RVA: 0x0000462D File Offset: 0x0000362D
        public void rot(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("rot", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x060000F0 RID: 240 RVA: 0x00004641 File Offset: 0x00003641
        public MWArray rotell()
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[0]);
        }

        // Token: 0x060000F1 RID: 241 RVA: 0x00004658 File Offset: 0x00003658
        public MWArray rotell(MWArray a0)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0
            });
        }

        // Token: 0x060000F2 RID: 242 RVA: 0x00004680 File Offset: 0x00003680
        public MWArray rotell(MWArray a0, MWArray b0)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0
            });
        }

        // Token: 0x060000F3 RID: 243 RVA: 0x000046AC File Offset: 0x000036AC
        public MWArray rotell(MWArray a0, MWArray b0, MWArray a1)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0,
                a1
            });
        }

        // Token: 0x060000F4 RID: 244 RVA: 0x000046DC File Offset: 0x000036DC
        public MWArray rotell(MWArray a0, MWArray b0, MWArray a1, MWArray b1)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1
            });
        }

        // Token: 0x060000F5 RID: 245 RVA: 0x00004714 File Offset: 0x00003714
        public MWArray rotell(MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma
            });
        }

        // Token: 0x060000F6 RID: 246 RVA: 0x00004750 File Offset: 0x00003750
        public MWArray rotell(MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma, MWArray strcolor)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma,
                strcolor
            });
        }

        // Token: 0x060000F7 RID: 247 RVA: 0x00004790 File Offset: 0x00003790
        public MWArray rotell(MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma, MWArray strcolor, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction("rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma,
                strcolor,
                plotflag
            });
        }

        // Token: 0x060000F8 RID: 248 RVA: 0x000047D4 File Offset: 0x000037D4
        public MWArray[] rotell(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[0]);
        }

        // Token: 0x060000F9 RID: 249 RVA: 0x000047EC File Offset: 0x000037EC
        public MWArray[] rotell(int numArgsOut, MWArray a0)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0
            });
        }

        // Token: 0x060000FA RID: 250 RVA: 0x00004818 File Offset: 0x00003818
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0
            });
        }

        // Token: 0x060000FB RID: 251 RVA: 0x00004848 File Offset: 0x00003848
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0, MWArray a1)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0,
                a1
            });
        }

        // Token: 0x060000FC RID: 252 RVA: 0x0000487C File Offset: 0x0000387C
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0, MWArray a1, MWArray b1)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1
            });
        }

        // Token: 0x060000FD RID: 253 RVA: 0x000048B4 File Offset: 0x000038B4
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma
            });
        }

        // Token: 0x060000FE RID: 254 RVA: 0x000048F0 File Offset: 0x000038F0
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma, MWArray strcolor)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma,
                strcolor
            });
        }

        // Token: 0x060000FF RID: 255 RVA: 0x00004934 File Offset: 0x00003934
        public MWArray[] rotell(int numArgsOut, MWArray a0, MWArray b0, MWArray a1, MWArray b1, MWArray gamma, MWArray strcolor, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotell", new MWArray[]
            {
                a0,
                b0,
                a1,
                b1,
                gamma,
                strcolor,
                plotflag
            });
        }

        // Token: 0x06000100 RID: 256 RVA: 0x0000497A File Offset: 0x0000397A
        public void rotell(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("rotell", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000101 RID: 257 RVA: 0x0000498E File Offset: 0x0000398E
        public MWArray rotray()
        {
            return formCalc.mcr.EvaluateFunction("rotray", new MWArray[0]);
        }

        // Token: 0x06000102 RID: 258 RVA: 0x000049A8 File Offset: 0x000039A8
        public MWArray rotray(MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("rotray", new MWArray[]
            {
                p
            });
        }

        // Token: 0x06000103 RID: 259 RVA: 0x000049D0 File Offset: 0x000039D0
        public MWArray rotray(MWArray p, MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction("rotray", new MWArray[]
            {
                p,
                mm
            });
        }

        // Token: 0x06000104 RID: 260 RVA: 0x000049FC File Offset: 0x000039FC
        public MWArray rotray(MWArray p, MWArray mm, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction("rotray", new MWArray[]
            {
                p,
                mm,
                alpha
            });
        }

        // Token: 0x06000105 RID: 261 RVA: 0x00004A2C File Offset: 0x00003A2C
        public MWArray[] rotray(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotray", new MWArray[0]);
        }

        // Token: 0x06000106 RID: 262 RVA: 0x00004A44 File Offset: 0x00003A44
        public MWArray[] rotray(int numArgsOut, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotray", new MWArray[]
            {
                p
            });
        }

        // Token: 0x06000107 RID: 263 RVA: 0x00004A70 File Offset: 0x00003A70
        public MWArray[] rotray(int numArgsOut, MWArray p, MWArray mm)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotray", new MWArray[]
            {
                p,
                mm
            });
        }

        // Token: 0x06000108 RID: 264 RVA: 0x00004AA0 File Offset: 0x00003AA0
        public MWArray[] rotray(int numArgsOut, MWArray p, MWArray mm, MWArray alpha)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "rotray", new MWArray[]
            {
                p,
                mm,
                alpha
            });
        }

        // Token: 0x06000109 RID: 265 RVA: 0x00004AD2 File Offset: 0x00003AD2
        public void rotray(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("rotray", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600010A RID: 266 RVA: 0x00004AE6 File Offset: 0x00003AE6
        public MWArray shannon_entropy()
        {
            return formCalc.mcr.EvaluateFunction("shannon_entropy", new MWArray[0]);
        }

        // Token: 0x0600010B RID: 267 RVA: 0x00004B00 File Offset: 0x00003B00
        public MWArray shannon_entropy(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("shannon_entropy", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600010C RID: 268 RVA: 0x00004B28 File Offset: 0x00003B28
        public MWArray[] shannon_entropy(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "shannon_entropy", new MWArray[0]);
        }

        // Token: 0x0600010D RID: 269 RVA: 0x00004B40 File Offset: 0x00003B40
        public MWArray[] shannon_entropy(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "shannon_entropy", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600010E RID: 270 RVA: 0x00004B69 File Offset: 0x00003B69
        public void shannon_entropy(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("shannon_entropy", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600010F RID: 271 RVA: 0x00004B7D File Offset: 0x00003B7D
        public MWArray smooth()
        {
            return formCalc.mcr.EvaluateFunction("smooth", new MWArray[0]);
        }

        // Token: 0x06000110 RID: 272 RVA: 0x00004B94 File Offset: 0x00003B94
        public MWArray smooth(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("smooth", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000111 RID: 273 RVA: 0x00004BBC File Offset: 0x00003BBC
        public MWArray smooth(MWArray x, MWArray g)
        {
            return formCalc.mcr.EvaluateFunction("smooth", new MWArray[]
            {
                x,
                g
            });
        }

        // Token: 0x06000112 RID: 274 RVA: 0x00004BE8 File Offset: 0x00003BE8
        public MWArray[] smooth(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "smooth", new MWArray[0]);
        }

        // Token: 0x06000113 RID: 275 RVA: 0x00004C00 File Offset: 0x00003C00
        public MWArray[] smooth(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "smooth", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000114 RID: 276 RVA: 0x00004C2C File Offset: 0x00003C2C
        public MWArray[] smooth(int numArgsOut, MWArray x, MWArray g)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "smooth", new MWArray[]
            {
                x,
                g
            });
        }

        // Token: 0x06000115 RID: 277 RVA: 0x00004C59 File Offset: 0x00003C59
        public void smooth(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("smooth", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000116 RID: 278 RVA: 0x00004C6D File Offset: 0x00003C6D
        public MWArray unrot_ell()
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[0]);
        }

        // Token: 0x06000117 RID: 279 RVA: 0x00004C84 File Offset: 0x00003C84
        public MWArray unrot_ell(MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x06000118 RID: 280 RVA: 0x00004CAC File Offset: 0x00003CAC
        public MWArray unrot_ell(MWArray pars, MWArray fx)
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[]
            {
                pars,
                fx
            });
        }

        // Token: 0x06000119 RID: 281 RVA: 0x00004CD8 File Offset: 0x00003CD8
        public MWArray unrot_ell(MWArray pars, MWArray fx, MWArray fy)
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy
            });
        }

        // Token: 0x0600011A RID: 282 RVA: 0x00004D08 File Offset: 0x00003D08
        public MWArray unrot_ell(MWArray pars, MWArray fx, MWArray fy, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy,
                p
            });
        }

        // Token: 0x0600011B RID: 283 RVA: 0x00004D40 File Offset: 0x00003D40
        public MWArray unrot_ell(MWArray pars, MWArray fx, MWArray fy, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction("unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy,
                p,
                q
            });
        }

        // Token: 0x0600011C RID: 284 RVA: 0x00004D7A File Offset: 0x00003D7A
        public MWArray[] unrot_ell(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[0]);
        }

        // Token: 0x0600011D RID: 285 RVA: 0x00004D94 File Offset: 0x00003D94
        public MWArray[] unrot_ell(int numArgsOut, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x0600011E RID: 286 RVA: 0x00004DC0 File Offset: 0x00003DC0
        public MWArray[] unrot_ell(int numArgsOut, MWArray pars, MWArray fx)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[]
            {
                pars,
                fx
            });
        }

        // Token: 0x0600011F RID: 287 RVA: 0x00004DF0 File Offset: 0x00003DF0
        public MWArray[] unrot_ell(int numArgsOut, MWArray pars, MWArray fx, MWArray fy)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy
            });
        }

        // Token: 0x06000120 RID: 288 RVA: 0x00004E24 File Offset: 0x00003E24
        public MWArray[] unrot_ell(int numArgsOut, MWArray pars, MWArray fx, MWArray fy, MWArray p)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy,
                p
            });
        }

        // Token: 0x06000121 RID: 289 RVA: 0x00004E5C File Offset: 0x00003E5C
        public MWArray[] unrot_ell(int numArgsOut, MWArray pars, MWArray fx, MWArray fy, MWArray p, MWArray q)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "unrot_ell", new MWArray[]
            {
                pars,
                fx,
                fy,
                p,
                q
            });
        }

        // Token: 0x06000122 RID: 290 RVA: 0x00004E98 File Offset: 0x00003E98
        public void unrot_ell(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("unrot_ell", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000123 RID: 291 RVA: 0x00004EAC File Offset: 0x00003EAC
        public MWArray polsort()
        {
            return formCalc.mcr.EvaluateFunction("polsort", new MWArray[0]);
        }

        // Token: 0x06000124 RID: 292 RVA: 0x00004EC4 File Offset: 0x00003EC4
        public MWArray polsort(MWArray th)
        {
            return formCalc.mcr.EvaluateFunction("polsort", new MWArray[]
            {
                th
            });
        }

        // Token: 0x06000125 RID: 293 RVA: 0x00004EEC File Offset: 0x00003EEC
        public MWArray polsort(MWArray th, MWArray r)
        {
            return formCalc.mcr.EvaluateFunction("polsort", new MWArray[]
            {
                th,
                r
            });
        }

        // Token: 0x06000126 RID: 294 RVA: 0x00004F18 File Offset: 0x00003F18
        public MWArray[] polsort(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "polsort", new MWArray[0]);
        }

        // Token: 0x06000127 RID: 295 RVA: 0x00004F30 File Offset: 0x00003F30
        public MWArray[] polsort(int numArgsOut, MWArray th)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "polsort", new MWArray[]
            {
                th
            });
        }

        // Token: 0x06000128 RID: 296 RVA: 0x00004F5C File Offset: 0x00003F5C
        public MWArray[] polsort(int numArgsOut, MWArray th, MWArray r)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "polsort", new MWArray[]
            {
                th,
                r
            });
        }

        // Token: 0x06000129 RID: 297 RVA: 0x00004F89 File Offset: 0x00003F89
        public void polsort(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("polsort", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600012A RID: 298 RVA: 0x00004F9D File Offset: 0x00003F9D
        public MWArray im2bwthresh()
        {
            return formCalc.mcr.EvaluateFunction("im2bwthresh", new MWArray[0]);
        }

        // Token: 0x0600012B RID: 299 RVA: 0x00004FB4 File Offset: 0x00003FB4
        public MWArray im2bwthresh(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("im2bwthresh", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600012C RID: 300 RVA: 0x00004FDC File Offset: 0x00003FDC
        public MWArray im2bwthresh(MWArray x, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("im2bwthresh", new MWArray[]
            {
                x,
                level
            });
        }

        // Token: 0x0600012D RID: 301 RVA: 0x00005008 File Offset: 0x00004008
        public MWArray im2bwthresh(MWArray x, MWArray level, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction("im2bwthresh", new MWArray[]
            {
                x,
                level,
                mode
            });
        }

        // Token: 0x0600012E RID: 302 RVA: 0x00005038 File Offset: 0x00004038
        public MWArray[] im2bwthresh(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2bwthresh", new MWArray[0]);
        }

        // Token: 0x0600012F RID: 303 RVA: 0x00005050 File Offset: 0x00004050
        public MWArray[] im2bwthresh(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2bwthresh", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000130 RID: 304 RVA: 0x0000507C File Offset: 0x0000407C
        public MWArray[] im2bwthresh(int numArgsOut, MWArray x, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2bwthresh", new MWArray[]
            {
                x,
                level
            });
        }

        // Token: 0x06000131 RID: 305 RVA: 0x000050AC File Offset: 0x000040AC
        public MWArray[] im2bwthresh(int numArgsOut, MWArray x, MWArray level, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2bwthresh", new MWArray[]
            {
                x,
                level,
                mode
            });
        }

        // Token: 0x06000132 RID: 306 RVA: 0x000050DE File Offset: 0x000040DE
        public void im2bwthresh(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("im2bwthresh", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000133 RID: 307 RVA: 0x000050F2 File Offset: 0x000040F2
        public MWArray fract_dim()
        {
            return formCalc.mcr.EvaluateFunction("fract_dim", new MWArray[0]);
        }

        // Token: 0x06000134 RID: 308 RVA: 0x0000510C File Offset: 0x0000410C
        public MWArray fract_dim(MWArray coordmat)
        {
            return formCalc.mcr.EvaluateFunction("fract_dim", new MWArray[]
            {
                coordmat
            });
        }

        // Token: 0x06000135 RID: 309 RVA: 0x00005134 File Offset: 0x00004134
        public MWArray fract_dim(MWArray coordmat, MWArray maxstep)
        {
            return formCalc.mcr.EvaluateFunction("fract_dim", new MWArray[]
            {
                coordmat,
                maxstep
            });
        }

        // Token: 0x06000136 RID: 310 RVA: 0x00005160 File Offset: 0x00004160
        public MWArray[] fract_dim(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fract_dim", new MWArray[0]);
        }

        // Token: 0x06000137 RID: 311 RVA: 0x00005178 File Offset: 0x00004178
        public MWArray[] fract_dim(int numArgsOut, MWArray coordmat)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fract_dim", new MWArray[]
            {
                coordmat
            });
        }

        // Token: 0x06000138 RID: 312 RVA: 0x000051A4 File Offset: 0x000041A4
        public MWArray[] fract_dim(int numArgsOut, MWArray coordmat, MWArray maxstep)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "fract_dim", new MWArray[]
            {
                coordmat,
                maxstep
            });
        }

        // Token: 0x06000139 RID: 313 RVA: 0x000051D1 File Offset: 0x000041D1
        public void fract_dim(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("fract_dim", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600013A RID: 314 RVA: 0x000051E5 File Offset: 0x000041E5
        public MWArray ellipse_sector_area()
        {
            return formCalc.mcr.EvaluateFunction("ellipse_sector_area", new MWArray[0]);
        }

        // Token: 0x0600013B RID: 315 RVA: 0x000051FC File Offset: 0x000041FC
        public MWArray ellipse_sector_area(MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("ellipse_sector_area", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x0600013C RID: 316 RVA: 0x00005224 File Offset: 0x00004224
        public MWArray ellipse_sector_area(MWArray pars, MWArray sec_angles)
        {
            return formCalc.mcr.EvaluateFunction("ellipse_sector_area", new MWArray[]
            {
                pars,
                sec_angles
            });
        }

        // Token: 0x0600013D RID: 317 RVA: 0x00005250 File Offset: 0x00004250
        public MWArray[] ellipse_sector_area(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ellipse_sector_area", new MWArray[0]);
        }

        // Token: 0x0600013E RID: 318 RVA: 0x00005268 File Offset: 0x00004268
        public MWArray[] ellipse_sector_area(int numArgsOut, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ellipse_sector_area", new MWArray[]
            {
                pars
            });
        }

        // Token: 0x0600013F RID: 319 RVA: 0x00005294 File Offset: 0x00004294
        public MWArray[] ellipse_sector_area(int numArgsOut, MWArray pars, MWArray sec_angles)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "ellipse_sector_area", new MWArray[]
            {
                pars,
                sec_angles
            });
        }

        // Token: 0x06000140 RID: 320 RVA: 0x000052C1 File Offset: 0x000042C1
        public void ellipse_sector_area(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("ellipse_sector_area", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000141 RID: 321 RVA: 0x000052D5 File Offset: 0x000042D5
        public MWArray coeff_sectors_orig()
        {
            return formCalc.mcr.EvaluateFunction("coeff_sectors_orig", new MWArray[0]);
        }

        // Token: 0x06000142 RID: 322 RVA: 0x000052EC File Offset: 0x000042EC
        public MWArray coeff_sectors_orig(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("coeff_sectors_orig", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000143 RID: 323 RVA: 0x00005314 File Offset: 0x00004314
        public MWArray coeff_sectors_orig(MWArray gdv_image, MWArray fingertype)
        {
            return formCalc.mcr.EvaluateFunction("coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype
            });
        }

        // Token: 0x06000144 RID: 324 RVA: 0x00005340 File Offset: 0x00004340
        public MWArray coeff_sectors_orig(MWArray gdv_image, MWArray fingertype, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction("coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype,
                pars
            });
        }

        // Token: 0x06000145 RID: 325 RVA: 0x00005370 File Offset: 0x00004370
        public MWArray coeff_sectors_orig(MWArray gdv_image, MWArray fingertype, MWArray pars, MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction("coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype,
                pars,
                profiles
            });
        }

        // Token: 0x06000146 RID: 326 RVA: 0x000053A5 File Offset: 0x000043A5
        public MWArray[] coeff_sectors_orig(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "coeff_sectors_orig", new MWArray[0]);
        }

        // Token: 0x06000147 RID: 327 RVA: 0x000053C0 File Offset: 0x000043C0
        public MWArray[] coeff_sectors_orig(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "coeff_sectors_orig", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000148 RID: 328 RVA: 0x000053EC File Offset: 0x000043EC
        public MWArray[] coeff_sectors_orig(int numArgsOut, MWArray gdv_image, MWArray fingertype)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype
            });
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000541C File Offset: 0x0000441C
        public MWArray[] coeff_sectors_orig(int numArgsOut, MWArray gdv_image, MWArray fingertype, MWArray pars)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype,
                pars
            });
        }

        // Token: 0x0600014A RID: 330 RVA: 0x00005450 File Offset: 0x00004450
        public MWArray[] coeff_sectors_orig(int numArgsOut, MWArray gdv_image, MWArray fingertype, MWArray pars, MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "coeff_sectors_orig", new MWArray[]
            {
                gdv_image,
                fingertype,
                pars,
                profiles
            });
        }

        // Token: 0x0600014B RID: 331 RVA: 0x00005487 File Offset: 0x00004487
        public void coeff_sectors_orig(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("coeff_sectors_orig", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600014C RID: 332 RVA: 0x0000549B File Offset: 0x0000449B
        public MWArray im2pixfill()
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[0]);
        }

        // Token: 0x0600014D RID: 333 RVA: 0x000054B4 File Offset: 0x000044B4
        public MWArray im2pixfill(MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x0600014E RID: 334 RVA: 0x000054DC File Offset: 0x000044DC
        public MWArray im2pixfill(MWArray gdv_image, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode
            });
        }

        // Token: 0x0600014F RID: 335 RVA: 0x00005508 File Offset: 0x00004508
        public MWArray im2pixfill(MWArray gdv_image, MWArray extract_mode, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level
            });
        }

        // Token: 0x06000150 RID: 336 RVA: 0x00005538 File Offset: 0x00004538
        public MWArray im2pixfill(MWArray gdv_image, MWArray extract_mode, MWArray level, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level,
                mode
            });
        }

        // Token: 0x06000151 RID: 337 RVA: 0x00005570 File Offset: 0x00004570
        public MWArray im2pixfill(MWArray gdv_image, MWArray extract_mode, MWArray level, MWArray mode, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction("im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level,
                mode,
                midpoint
            });
        }

        // Token: 0x06000152 RID: 338 RVA: 0x000055AA File Offset: 0x000045AA
        public MWArray[] im2pixfill(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[0]);
        }

        // Token: 0x06000153 RID: 339 RVA: 0x000055C4 File Offset: 0x000045C4
        public MWArray[] im2pixfill(int numArgsOut, MWArray gdv_image)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[]
            {
                gdv_image
            });
        }

        // Token: 0x06000154 RID: 340 RVA: 0x000055F0 File Offset: 0x000045F0
        public MWArray[] im2pixfill(int numArgsOut, MWArray gdv_image, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode
            });
        }

        // Token: 0x06000155 RID: 341 RVA: 0x00005620 File Offset: 0x00004620
        public MWArray[] im2pixfill(int numArgsOut, MWArray gdv_image, MWArray extract_mode, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level
            });
        }

        // Token: 0x06000156 RID: 342 RVA: 0x00005654 File Offset: 0x00004654
        public MWArray[] im2pixfill(int numArgsOut, MWArray gdv_image, MWArray extract_mode, MWArray level, MWArray mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level,
                mode
            });
        }

        // Token: 0x06000157 RID: 343 RVA: 0x0000568C File Offset: 0x0000468C
        public MWArray[] im2pixfill(int numArgsOut, MWArray gdv_image, MWArray extract_mode, MWArray level, MWArray mode, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2pixfill", new MWArray[]
            {
                gdv_image,
                extract_mode,
                level,
                mode,
                midpoint
            });
        }

        // Token: 0x06000158 RID: 344 RVA: 0x000056C8 File Offset: 0x000046C8
        public void im2pixfill(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("im2pixfill", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000159 RID: 345 RVA: 0x000056DC File Offset: 0x000046DC
        public MWArray im2ellitfill()
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[0]);
        }

        // Token: 0x0600015A RID: 346 RVA: 0x000056F4 File Offset: 0x000046F4
        public MWArray im2ellitfill(MWArray x)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x
            });
        }

        // Token: 0x0600015B RID: 347 RVA: 0x0000571C File Offset: 0x0000471C
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode
            });
        }

        // Token: 0x0600015C RID: 348 RVA: 0x00005748 File Offset: 0x00004748
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode, MWArray smoothflag)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag
            });
        }

        // Token: 0x0600015D RID: 349 RVA: 0x00005778 File Offset: 0x00004778
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag
            });
        }

        // Token: 0x0600015E RID: 350 RVA: 0x000057B0 File Offset: 0x000047B0
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter
            });
        }

        // Token: 0x0600015F RID: 351 RVA: 0x000057EC File Offset: 0x000047EC
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level
            });
        }

        // Token: 0x06000160 RID: 352 RVA: 0x0000582C File Offset: 0x0000482C
        public MWArray im2ellitfill(MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction("im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level,
                midpoint
            });
        }

        // Token: 0x06000161 RID: 353 RVA: 0x00005870 File Offset: 0x00004870
        public MWArray[] im2ellitfill(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[0]);
        }

        // Token: 0x06000162 RID: 354 RVA: 0x00005888 File Offset: 0x00004888
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x
            });
        }

        // Token: 0x06000163 RID: 355 RVA: 0x000058B4 File Offset: 0x000048B4
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode
            });
        }

        // Token: 0x06000164 RID: 356 RVA: 0x000058E4 File Offset: 0x000048E4
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag
            });
        }

        // Token: 0x06000165 RID: 357 RVA: 0x00005918 File Offset: 0x00004918
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag
            });
        }

        // Token: 0x06000166 RID: 358 RVA: 0x00005950 File Offset: 0x00004950
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter
            });
        }

        // Token: 0x06000167 RID: 359 RVA: 0x0000598C File Offset: 0x0000498C
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level
            });
        }

        // Token: 0x06000168 RID: 360 RVA: 0x000059D0 File Offset: 0x000049D0
        public MWArray[] im2ellitfill(int numArgsOut, MWArray x, MWArray extract_mode, MWArray smoothflag, MWArray plotflag, MWArray iter, MWArray level, MWArray midpoint)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "im2ellitfill", new MWArray[]
            {
                x,
                extract_mode,
                smoothflag,
                plotflag,
                iter,
                level,
                midpoint
            });
        }

        // Token: 0x06000169 RID: 361 RVA: 0x00005A16 File Offset: 0x00004A16
        public void im2ellitfill(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("im2ellitfill", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600016A RID: 362 RVA: 0x00005A2A File Offset: 0x00004A2A
        public MWArray getfingerid()
        {
            return formCalc.mcr.EvaluateFunction("getfingerid", new MWArray[0]);
        }

        // Token: 0x0600016B RID: 363 RVA: 0x00005A44 File Offset: 0x00004A44
        public MWArray getfingerid(MWArray str)
        {
            return formCalc.mcr.EvaluateFunction("getfingerid", new MWArray[]
            {
                str
            });
        }

        // Token: 0x0600016C RID: 364 RVA: 0x00005A6C File Offset: 0x00004A6C
        public MWArray[] getfingerid(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getfingerid", new MWArray[0]);
        }

        // Token: 0x0600016D RID: 365 RVA: 0x00005A84 File Offset: 0x00004A84
        public MWArray[] getfingerid(int numArgsOut, MWArray str)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "getfingerid", new MWArray[]
            {
                str
            });
        }

        // Token: 0x0600016E RID: 366 RVA: 0x00005AAD File Offset: 0x00004AAD
        public void getfingerid(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("getfingerid", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x0600016F RID: 367 RVA: 0x00005AC1 File Offset: 0x00004AC1
        public MWArray findbreakprofiles_new()
        {
            return formCalc.mcr.EvaluateFunction("findbreakprofiles_new", new MWArray[0]);
        }

        // Token: 0x06000170 RID: 368 RVA: 0x00005AD8 File Offset: 0x00004AD8
        public MWArray findbreakprofiles_new(MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction("findbreakprofiles_new", new MWArray[]
            {
                profiles
            });
        }

        // Token: 0x06000171 RID: 369 RVA: 0x00005B00 File Offset: 0x00004B00
        public MWArray[] findbreakprofiles_new(int numArgsOut)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findbreakprofiles_new", new MWArray[0]);
        }

        // Token: 0x06000172 RID: 370 RVA: 0x00005B18 File Offset: 0x00004B18
        public MWArray[] findbreakprofiles_new(int numArgsOut, MWArray profiles)
        {
            return formCalc.mcr.EvaluateFunction(numArgsOut, "findbreakprofiles_new", new MWArray[]
            {
                profiles
            });
        }

        // Token: 0x06000173 RID: 371 RVA: 0x00005B41 File Offset: 0x00004B41
        public void findbreakprofiles_new(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
        {
            formCalc.mcr.EvaluateFunction("findbreakprofiles_new", numArgsOut, ref argsOut, argsIn);
        }

        // Token: 0x06000174 RID: 372 RVA: 0x00005B55 File Offset: 0x00004B55
        public void WaitForFiguresToDie()
        {
            formCalc.mcr.WaitForFiguresToDie();
        }

        // Token: 0x04000001 RID: 1
        private static MWMCR mcr;

        // Token: 0x04000002 RID: 2
        private bool disposed;
    }
}
