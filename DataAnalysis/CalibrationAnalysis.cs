using MathWorks.MATLAB.NET.ComponentData;
using MathWorks.MATLAB.NET.Utility;
using System.Reflection;

namespace EPIC.DataAnalysis
{
    // Token: 0x02000002 RID: 2
    public class CalibrationAnalysis : IDisposable
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00001050
        static CalibrationAnalysis()
        {
            // Ensure the MathWorks MCR class is referenced with its full namespace so the symbol is resolved.
            if (MWMCR.MCRAppInitialized)
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string text = executingAssembly.Location;
                int num = text.LastIndexOf("\\");
                text = text.Remove(num, text.Length - num);
                string value = MCRComponentState.MCC_CalibrationAnalysis_name_data + ".ctf";
                Stream stream = null;
                string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
                foreach (string text2 in manifestResourceNames)
                {
                    if (text2.Contains(value))
                    {
                        stream = executingAssembly.GetManifestResourceStream(text2);
                        break;
                    }
                }
                CalibrationAnalysis.mcr = new MWMCR(
                    "",                // Component Name (often empty or the DLL name)
                    text,              // Component Root (the path to your extracted folder)
                    stream,            // The CTF stream you found in the manifest
                    true               // IsEmbedded (true if using the stream)
                );
                return;
            }
            throw new ApplicationException("MWArray assembly could not be initialized");
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002138 File Offset: 0x00001138
        ~CalibrationAnalysis()
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
        public object binImageNorm()
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("binImageNorm", new object[0]);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000021A4 File Offset: 0x000011A4
        public object binImageNorm(object PImage)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("binImageNorm", new object[]
            {
                PImage
            });
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000021CC File Offset: 0x000011CC
        public object binImageNorm(object PImage, object npix)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("binImageNorm", new object[]
            {
                PImage,
                npix
            });
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000011F8
        public object binImageNorm(object PImage, object npix, object normalize)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("binImageNorm", new object[]
            {
                PImage,
                npix,
                normalize
            });
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002228 File Offset: 0x00001228
        public object[] binImageNorm(int numArgsOut)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "binImageNorm", new object[0]);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002240 File Offset: 0x00001240
        public object[] binImageNorm(int numArgsOut, object PImage)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "binImageNorm", new object[]
            {
                PImage
            });
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000226C File Offset: 0x0000126C
        public object[] binImageNorm(int numArgsOut, object PImage, object npix)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "binImageNorm", new object[]
            {
                PImage,
                npix
            });
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000229C File Offset: 0x0000129C
        public object[] binImageNorm(int numArgsOut, object PImage, object npix, object normalize)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "binImageNorm", new object[]
            {
                PImage,
                npix,
                normalize
            });
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000022CE File Offset: 0x000012CE
        public object checkCalibrationv22()
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[0]);
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000022E8 File Offset: 0x000012E8
        public object checkCalibrationv22(object imageBaselineSetFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile
            });
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002310 File Offset: 0x00001310
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile
            });
        }

        // Token: 0x06000011 RID: 17 RVA: 0x0000233C File Offset: 0x0000133C
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile, object sigmaRegions)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions
            });
        }

        // Token: 0x06000012 RID: 18 RVA: 0x0000236C File Offset: 0x0000136C
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions
            });
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000023A4 File Offset: 0x000013A4
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth
            });
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000023E0 File Offset: 0x000013E0
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth, object sigmaMeans)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth,
                sigmaMeans
            });
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002420 File Offset: 0x00001420
        public object checkCalibrationv22(object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth, object sigmaMeans, object useIm2ellitCenter)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth,
                sigmaMeans,
                useIm2ellitCenter
            });
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002464 File Offset: 0x00001464
        public object[] checkCalibrationv22(int numArgsOut)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[0]);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x0000247C File Offset: 0x0000147C
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile
            });
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000024A8 File Offset: 0x000014A8
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile
            });
        }

        // Token: 0x06000019 RID: 25 RVA: 0x000024D8 File Offset: 0x000014D8
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile, object sigmaRegions)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions
            });
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000250C File Offset: 0x0000150C
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions
            });
        }

        // Token: 0x0600001B RID: 27 RVA: 0x00002544 File Offset: 0x00001544
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth
            });
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00002580 File Offset: 0x00001580
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth, object sigmaMeans)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth,
                sigmaMeans
            });
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000025C4 File Offset: 0x000015C4
        public object[] checkCalibrationv22(int numArgsOut, object imageBaselineSetFile, object IUTFile, object sigmaRegions, object threshRegions, object binDepth, object sigmaMeans, object useIm2ellitCenter)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "checkCalibrationv22", new object[]
            {
                imageBaselineSetFile,
                IUTFile,
                sigmaRegions,
                threshRegions,
                binDepth,
                sigmaMeans,
                useIm2ellitCenter
            });
        }

        // Token: 0x0600001E RID: 30 RVA: 0x0000260A File Offset: 0x0000160A
        public object loadCData()
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("loadCData", new object[0]);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002624 File Offset: 0x00001624
        public object loadCData(object imageBaselineSetFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction("loadCData", new object[]
            {
                imageBaselineSetFile
            });
        }

        // Token: 0x06000020 RID: 32 RVA: 0x0000264C File Offset: 0x0000164C
        public object[] loadCData(int numArgsOut)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "loadCData", new object[0]);
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002664 File Offset: 0x00001664
        public object[] loadCData(int numArgsOut, object imageBaselineSetFile)
        {
            return CalibrationAnalysis.mcr.EvaluateFunction(numArgsOut, "loadCData", new object[]
            {
                imageBaselineSetFile
            });
        }

        // Token: 0x06000022 RID: 34 RVA: 0x0000268D File Offset: 0x0000168D
        public void WaitForFiguresToDie()
        {
            CalibrationAnalysis.mcr.WaitForFiguresToDie();
        }

        // Token: 0x04000001 RID: 1
        private static MWMCR mcr;

        // Token: 0x04000002 RID: 2
        private bool disposed;
    }
}
