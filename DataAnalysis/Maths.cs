using Emgu.CV;
using Emgu.CV.Structure;
using mlformC;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EPIC.DataAnalysis
{
    /// <summary>
    /// Commonly used math functions.
    /// </summary>
    // Token: 0x02000002 RID: 2
    public static class Maths
    {
        /// <summary>
        /// Compare a bitmap against the baseline image using the matlab runtime.
        /// </summary>
        /// <param name="bitmap">The bitmap for comparison.</param>
        /// <param name="settings">The calibration settings to check against.</param>
        /// <param name="colorizedEntity"></param>
        /// <returns>An ImageCalibrationEntity that has NOT been saved, the caller is responsible for handling the results.</returns>
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public static DataLayer.Entities.ImageCalibration CheckCalibration(Bitmap bitmap, DataLayer.Entities.DeviceCalibrationSetting settings, out DataLayer.Entities.Image colorizedEntity)
        {
            DataLayer.Entities.ImageCalibration result2;
            try
            {
                byte[,,] image = new byte[3, bitmap.Height, bitmap.Width];
                using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(bitmap))
                {
                    byte[,,] data = cvImage.Data;
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            image[0, y, x] = data[y, x, 2];
                            image[1, y, x] = data[y, x, 1];
                            image[2, y, x] = data[y, x, 0];
                        }
                    }
                }
                double[] sigmaRegions = new double[]
                {
                    settings.SigmaRegionOuter,
                    settings.SigmaRegionInner,
                    settings.SigmaRegionCorona,
                    settings.SigmaRegionHighP,
                    settings.SigmaRegionTotal,
                    settings.SigmaRegionClump
                };
                double[] thresholdRegions = new double[]
                {
                    settings.ThresholdPercentOuter,
                    settings.ThresholdPercentInner,
                    settings.ThresholdPercentCorona,
                    settings.ThresholdPercentHighP,
                    settings.ThresholdPercentTotal,
                    settings.ThresholdPercentClumps
                };
                double[] sigmaMeans = new double[]
                {
                    settings.SigmaMeansOuter,
                    settings.SigmaMeansInner,
                    settings.SigmaMeansCorona,
                    settings.SigmaMeansHighP
                };
                int[] biningDepth = new int[]
                {
                    settings.BinDepth
                };
                object[] argsOut = Maths.CalibrationAnalysis.checkCalibrationv22(3, Maths.BaseCalibration, image, sigmaRegions, thresholdRegions, biningDepth, sigmaMeans);
                bool[,] failedValue = (bool[,])argsOut[0];
                object[,,] resultArrayData = (object[,,])argsOut[1];
                object[,,] outputImages = (object[,,])argsOut[2];
                double[,] failedPixelsInRegion = (double[,])resultArrayData[0, 0, 0];
                double[,] totalPixelsInRegion = (double[,])resultArrayData[1, 0, 0];
                double[,] percentPixelFailureInRegion = (double[,])resultArrayData[2, 0, 0];
                bool[,] pixelCompareStatus = (bool[,])resultArrayData[3, 0, 0];
                bool[,] meanCompareStatus = (bool[,])resultArrayData[4, 0, 0];
                double[,] meanStatusDiff = (double[,])resultArrayData[5, 0, 0];
                double[,] centeredImage = (double[,])outputImages[0, 0, 0];
                double[,] diffImage = (double[,])outputImages[1, 0, 0];
                bool[,] outerFail = (bool[,])outputImages[2, 0, 0];
                bool[,] innerFail = (bool[,])outputImages[3, 0, 0];
                bool[,] coronaFail = (bool[,])outputImages[4, 0, 0];
                bool[,] highpFail = (bool[,])outputImages[5, 0, 0];
                double[,] outerIntensity = (double[,])outputImages[6, 0, 0];
                double[,] innerIntensity = (double[,])outputImages[7, 0, 0];
                double[,] coronaIntensity = (double[,])outputImages[8, 0, 0];
                double[,] highpIntensity = (double[,])outputImages[9, 0, 0];
                double[,] totalIntensity = (double[,])outputImages[10, 0, 0];
                Image<Bgr, byte> colorized = Maths.GetColorized(centeredImage, diffImage, outerFail, innerFail, coronaFail, highpFail);
                Bitmap colorizedBitmap = colorized.Bitmap;
                colorizedEntity = new DataLayer.Entities.Image
                {
                    ImageData = Maths.CompressImage(colorizedBitmap)
                };
                colorizedBitmap.Dispose();
                colorized.Dispose();
                int noiseLevel = Maths.GetOptimalNoiseLevel(bitmap);
                DataLayer.Entities.ImageCalibration result = new DataLayer.Entities.ImageCalibration
                {
                    NoiseLevel = (double)noiseLevel,
                    IntensityOuter = outerIntensity[0, 0],
                    IntensityInner = innerIntensity[0, 0],
                    IntensityCorona = coronaIntensity[0, 0],
                    IntensityHighP = highpIntensity[0, 0],
                    IntensityTotal = totalIntensity[0, 0],
                    Failed = failedValue[0, 0],
                    OuterFailures = failedPixelsInRegion[0, 0],
                    InnerFailures = failedPixelsInRegion[0, 1],
                    CoronaFailures = failedPixelsInRegion[0, 2],
                    HighPFailures = failedPixelsInRegion[0, 3],
                    TotalFailures = failedPixelsInRegion[0, 4],
                    ClumpsFailures = failedPixelsInRegion[0, 5],
                    OuterTotalPixels = totalPixelsInRegion[0, 0],
                    InnerTotalPixels = totalPixelsInRegion[0, 1],
                    CoronaTotalPixels = totalPixelsInRegion[0, 2],
                    HighPTotalPixels = totalPixelsInRegion[0, 3],
                    TotalTotalPixels = totalPixelsInRegion[0, 4],
                    ClumpsTotalPixels = totalPixelsInRegion[0, 5],
                    OuterFailurePercent = percentPixelFailureInRegion[0, 0],
                    InnerFailurePercent = percentPixelFailureInRegion[0, 1],
                    CoronaFailurePercent = percentPixelFailureInRegion[0, 2],
                    HighPFailurePercent = percentPixelFailureInRegion[0, 3],
                    TotalFailurePercent = percentPixelFailureInRegion[0, 4],
                    ClumpsFailurePercent = percentPixelFailureInRegion[0, 5],
                    OuterFailed = pixelCompareStatus[0, 0],
                    InnerFailed = pixelCompareStatus[0, 1],
                    CoronaFailed = pixelCompareStatus[0, 2],
                    HighPFailed = pixelCompareStatus[0, 3],
                    TotalFailed = pixelCompareStatus[0, 4],
                    ClumpsFailed = pixelCompareStatus[0, 5],
                    OuterMeanDiff = meanStatusDiff[0, 0],
                    InnerMeanDiff = meanStatusDiff[0, 1],
                    CoronaMeanDiff = meanStatusDiff[0, 2],
                    HighPMeanDiff = meanStatusDiff[0, 3],
                    OuterMeanFailed = meanCompareStatus[0, 0],
                    InnerMeanFailed = meanCompareStatus[0, 1],
                    CoronaMeanFailed = meanCompareStatus[0, 2],
                    HighPMeanFailed = meanCompareStatus[0, 3],
                    SigmaRegionOuter = settings.SigmaRegionOuter,
                    SigmaRegionInner = settings.SigmaRegionInner,
                    SigmaRegionCorona = settings.SigmaRegionCorona,
                    SigmaRegionHighP = settings.SigmaRegionHighP,
                    SigmaRegionTotal = settings.SigmaRegionTotal,
                    SigmaRegionClump = settings.SigmaRegionClump,
                    ThresholdPercentOuter = settings.ThresholdPercentOuter,
                    ThresholdPercentInner = settings.ThresholdPercentInner,
                    ThresholdPercentCorona = settings.ThresholdPercentCorona,
                    ThresholdPercentHighP = settings.ThresholdPercentHighP,
                    ThresholdPercentTotal = settings.ThresholdPercentTotal,
                    ThresholdPercentClumps = settings.ThresholdPercentClumps,
                    SigmaMeansOuter = settings.SigmaMeansOuter,
                    SigmaMeansInner = settings.SigmaMeansInner,
                    SigmaMeansCorona = settings.SigmaMeansCorona,
                    SigmaMeansHighP = settings.SigmaMeansHighP,
                    SigmaMeansTotal = settings.SigmaMeansTotal,
                    SigmaMeansClumps = settings.SigmaMeansClumps,
                    BinDepth = settings.BinDepth
                };
                result2 = result;
            }
            catch (Exception ex)
            {
                Log.Error("There was an error processing the image.", ex);
                colorizedEntity = null;
                result2 = null;
            }
            return result2;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x00002998 File Offset: 0x00000B98
        private static Image<Bgr, byte> GetColorized(double[,] centeredImage, double[,] diffImage, bool[,] outerFail, bool[,] innerFail, bool[,] coronaFail, bool[,] highpFail)
        {
            double height = (double)diffImage.GetLength(0) * 0.5;
            double width = (double)diffImage.GetLength(1) * 0.5;
            Image<Bgr, byte> colorizeImage = new Image<Bgr, byte>((int)width, (int)height);
            List<double> diffFlat = diffImage.Cast<double>().ToList<double>();
            List<bool> outerFlat = outerFail.Cast<bool>().ToList<bool>();
            List<bool> innerFlat = innerFail.Cast<bool>().ToList<bool>();
            List<bool> coronaFlat = coronaFail.Cast<bool>().ToList<bool>();
            List<bool> highpFlat = highpFail.Cast<bool>().ToList<bool>();
            List<double> outerDiffFailures = (from x in Enumerable.Range(0, diffImage.Length)
                                              where outerFlat[x]
                                              select diffFlat[x]).ToList<double>();
            List<double> outerNeg = (from x in outerDiffFailures
                                     where x < 0.0
                                     select x).ToList<double>();
            List<double> outerPos = (from x in outerDiffFailures
                                     where x > 0.0
                                     select x).ToList<double>();
            List<double> innerDiffFailures = (from x in Enumerable.Range(0, diffImage.Length)
                                              where innerFlat[x]
                                              select diffFlat[x]).ToList<double>();
            List<double> innerNeg = (from x in innerDiffFailures
                                     where x < 0.0
                                     select x).ToList<double>();
            List<double> innerPos = (from x in innerDiffFailures
                                     where x > 0.0
                                     select x).ToList<double>();
            List<double> coronaDiffFailures = (from x in Enumerable.Range(0, diffImage.Length)
                                               where coronaFlat[x]
                                               select diffFlat[x]).ToList<double>();
            List<double> coronaNeg = (from x in coronaDiffFailures
                                      where x < 0.0
                                      select x).ToList<double>();
            List<double> coronaPos = (from x in coronaDiffFailures
                                      where x > 0.0
                                      select x).ToList<double>();
            List<double> highpDiffFailures = (from x in Enumerable.Range(0, diffImage.Length)
                                              where highpFlat[x]
                                              select diffFlat[x]).ToList<double>();
            List<double> highpNeg = (from x in highpDiffFailures
                                     where x < 0.0
                                     select x).ToList<double>();
            List<double> highpPos = (from x in highpDiffFailures
                                     where x > 0.0
                                     select x).ToList<double>();
            Dictionary<Maths.FailureRegion, Tuple<double, double, double, double>> regionMinMax = new Dictionary<Maths.FailureRegion, Tuple<double, double, double, double>>
            {
                {
                    Maths.FailureRegion.Outer,
                    new Tuple<double, double, double, double>(outerNeg.Any<double>() ? outerNeg.Min() : 0.0, outerNeg.Any<double>() ? outerNeg.Max() : 0.0, outerPos.Any<double>() ? outerPos.Min() : 0.0, outerPos.Any<double>() ? outerPos.Max() : 0.0)
                },
                {
                    Maths.FailureRegion.Inner,
                    new Tuple<double, double, double, double>(innerNeg.Any<double>() ? innerNeg.Min() : 0.0, innerNeg.Any<double>() ? innerNeg.Max() : 0.0, innerPos.Any<double>() ? innerPos.Min() : 0.0, innerPos.Any<double>() ? innerPos.Max() : 0.0)
                },
                {
                    Maths.FailureRegion.Corona,
                    new Tuple<double, double, double, double>(coronaNeg.Any<double>() ? coronaNeg.Min() : 0.0, coronaNeg.Any<double>() ? coronaNeg.Max() : 0.0, coronaPos.Any<double>() ? coronaPos.Min() : 0.0, coronaPos.Any<double>() ? coronaPos.Max() : 0.0)
                },
                {
                    Maths.FailureRegion.HighP,
                    new Tuple<double, double, double, double>(highpNeg.Any<double>() ? highpNeg.Min() : 0.0, highpNeg.Any<double>() ? highpNeg.Max() : 0.0, highpPos.Any<double>() ? highpPos.Min() : 0.0, highpPos.Any<double>() ? highpPos.Max() : 0.0)
                }
            };
            int x2 = 0;
            while ((double)x2 < width)
            {
                int y = 0;
                while ((double)y < height)
                {
                    int actualX = x2 + (int)(0.5 * width);
                    int actualY = y + (int)(0.5 * height);
                    double diff = diffImage[actualY, actualX];
                    double @default = centeredImage[actualY, actualX];
                    Maths.FailureRegion failed = outerFail[actualY, actualX] ? Maths.FailureRegion.Outer : (innerFail[actualY, actualX] ? Maths.FailureRegion.Inner : (coronaFail[actualY, actualX] ? Maths.FailureRegion.Corona : (highpFail[actualY, actualX] ? Maths.FailureRegion.HighP : Maths.FailureRegion.None)));
                    if (failed != Maths.FailureRegion.None)
                    {
                        Color color = Maths.DetermineColorToUse(diff, failed, regionMinMax[failed].Item1, regionMinMax[failed].Item2, regionMinMax[failed].Item3, regionMinMax[failed].Item4);
                        colorizeImage.Data[y, x2, 0] = color.B;
                        colorizeImage.Data[y, x2, 1] = color.G;
                        colorizeImage.Data[y, x2, 2] = color.R;
                    }
                    else
                    {
                        colorizeImage.Data[y, x2, 0] = (byte)@default;
                        colorizeImage.Data[y, x2, 1] = (byte)@default;
                        colorizeImage.Data[y, x2, 2] = (byte)@default;
                    }
                    y++;
                }
                x2++;
            }
            return colorizeImage;
        }

        /// <summary>
        /// Deflate an image and return the bytes.
        /// </summary>
        /// <param name="image">The image to be deflated.</param>
        /// <returns>A byte array of the deflated image for storing.</returns>
        // Token: 0x06000003 RID: 3 RVA: 0x00003018 File Offset: 0x00001218
        private static byte[] CompressImage(Bitmap image)
        {
            byte[] result2;
            using (MemoryStream outFile = new MemoryStream())
            {
                using (MemoryStream inFile = new MemoryStream())
                {
                    image.Save(inFile, ImageFormat.Bmp);
                    inFile.Seek(0L, SeekOrigin.Begin);
                    using (GZipStream compress = new GZipStream(outFile, CompressionMode.Compress))
                    {
                        inFile.CopyTo(compress);
                    }
                    byte[] result = outFile.ToArray();
                    result2 = ((result.Length == 0) ? null : result);
                }
            }
            return result2;
        }

        /// <summary>
        /// Get the color based on region and how far it is from the baseline image.
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="failed"></param>
        /// <param name="negMin"></param>
        /// <param name="negMax"></param>
        /// <param name="posMin"></param>
        /// <param name="posMax"></param>
        /// <returns></returns>
        // Token: 0x06000004 RID: 4 RVA: 0x000030D8 File Offset: 0x000012D8
        private static Color DetermineColorToUse(double diff, Maths.FailureRegion failed, double negMin, double negMax, double posMin, double posMax)
        {
            double increment;
            if (diff < 0.0)
            {
                increment = (negMin - negMax) / 5.0;
                if (failed == Maths.FailureRegion.Outer)
                {
                    if (diff > increment)
                    {
                        return Color.FromArgb(0, 95, 0);
                    }
                    if (diff > increment * 2.0)
                    {
                        return Color.FromArgb(0, 135, 0);
                    }
                    if (diff > increment * 3.0)
                    {
                        return Color.FromArgb(0, 175, 0);
                    }
                    if (diff > increment * 4.0)
                    {
                        return Color.FromArgb(0, 215, 0);
                    }
                    return Color.FromArgb(0, 255, 0);
                }
                else if (failed == Maths.FailureRegion.Corona)
                {
                    if (diff > increment)
                    {
                        return Color.FromArgb(48, 0, 95);
                    }
                    if (diff > increment * 2.0)
                    {
                        return Color.FromArgb(68, 0, 135);
                    }
                    if (diff > increment * 3.0)
                    {
                        return Color.FromArgb(88, 0, 175);
                    }
                    if (diff > increment * 4.0)
                    {
                        return Color.FromArgb(108, 0, 215);
                    }
                    return Color.FromArgb(128, 0, 255);
                }
                else if (failed == Maths.FailureRegion.HighP)
                {
                    if (diff > increment)
                    {
                        return Color.FromArgb(0, 95, 95);
                    }
                    if (diff > increment * 2.0)
                    {
                        return Color.FromArgb(0, 135, 135);
                    }
                    if (diff > increment * 3.0)
                    {
                        return Color.FromArgb(0, 175, 175);
                    }
                    if (diff > increment * 4.0)
                    {
                        return Color.FromArgb(0, 215, 215);
                    }
                    return Color.FromArgb(0, 255, 255);
                }
                else if (failed == Maths.FailureRegion.Inner)
                {
                    if (diff > increment)
                    {
                        return Color.FromArgb(0, 0, 95);
                    }
                    if (diff > increment * 2.0)
                    {
                        return Color.FromArgb(0, 0, 135);
                    }
                    if (diff > increment * 3.0)
                    {
                        return Color.FromArgb(0, 0, 175);
                    }
                    if (diff > increment * 4.0)
                    {
                        return Color.FromArgb(0, 0, 215);
                    }
                    return Color.FromArgb(0, 0, 255);
                }
            }
            increment = (posMax - posMin) / 5.0;
            Color result;
            if (failed == Maths.FailureRegion.Outer)
            {
                if (diff < increment)
                {
                    result = Color.FromArgb(95, 95, 0);
                }
                else if (diff < increment * 2.0)
                {
                    result = Color.FromArgb(135, 135, 0);
                }
                else if (diff < increment * 3.0)
                {
                    result = Color.FromArgb(175, 175, 0);
                }
                else if (diff < increment * 4.0)
                {
                    result = Color.FromArgb(215, 215, 0);
                }
                else
                {
                    result = Color.FromArgb(255, 255, 0);
                }
            }
            else if (failed == Maths.FailureRegion.Corona)
            {
                if (diff < increment)
                {
                    result = Color.FromArgb(95, 65, 0);
                }
                else if (diff < increment * 2.0)
                {
                    result = Color.FromArgb(135, 90, 0);
                }
                else if (diff < increment * 3.0)
                {
                    result = Color.FromArgb(175, 115, 0);
                }
                else if (diff < increment * 4.0)
                {
                    result = Color.FromArgb(215, 140, 0);
                }
                else
                {
                    result = Color.FromArgb(255, 165, 0);
                }
            }
            else if (failed == Maths.FailureRegion.HighP)
            {
                if (diff < increment)
                {
                    result = Color.FromArgb(95, 45, 35);
                }
                else if (diff < increment * 2.0)
                {
                    result = Color.FromArgb(135, 60, 70);
                }
                else if (diff < increment * 3.0)
                {
                    result = Color.FromArgb(175, 75, 105);
                }
                else if (diff < increment * 4.0)
                {
                    result = Color.FromArgb(215, 90, 145);
                }
                else
                {
                    result = Color.FromArgb(255, 105, 180);
                }
            }
            else
            {
                if (failed != Maths.FailureRegion.Inner)
                {
                    throw new NotImplementedException();
                }
                if (diff < increment)
                {
                    result = Color.FromArgb(95, 0, 0);
                }
                else if (diff < increment * 2.0)
                {
                    result = Color.FromArgb(135, 0, 0);
                }
                else if (diff < increment * 3.0)
                {
                    result = Color.FromArgb(175, 0, 0);
                }
                else if (diff < increment * 4.0)
                {
                    result = Color.FromArgb(215, 0, 0);
                }
                else
                {
                    result = Color.FromArgb(255, 0, 0);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the noise level by finding the center point and counting the number of pixels brighter than 40.
        /// If the number of noisy pixels is greater than .0002 than the noise level is incremented.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        // Token: 0x06000005 RID: 5 RVA: 0x000036E0 File Offset: 0x000018E0
        private static int GetOptimalNoiseLevel(Bitmap bitmap)
        {
            int workingNoiseLevel = 20;
            Bitmap tempMappingImage = new Bitmap(320, 240, PixelFormat.Format32bppArgb);
            using (Graphics graphicsObject = Graphics.FromImage(tempMappingImage))
            {
                byte[,,] energizedImage = new byte[3, bitmap.Height, bitmap.Width];
                using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(bitmap))
                {
                    byte[,,] data = cvImage.Data;
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            energizedImage[0, y, x] = data[y, x, 2];
                            energizedImage[1, y, x] = data[y, x, 1];
                            energizedImage[2, y, x] = data[y, x, 0];
                        }
                    }
                }
                object[] argsOut = Maths.FormCalc.compute_factors(1, energizedImage, "", "basic");
                double[,] native = (double[,])argsOut[0];
                double centerX = native[0, 0];
                double centerY = native[1, 0];
                double radiusX = native[2, 0];
                double radiusY = native[3, 0];
                double gamma = native[4, 0] * 57.29577951308232;
                graphicsObject.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 0)), new Rectangle(0, 0, 320, 240));
                graphicsObject.TranslateTransform((float)centerX, (float)centerY);
                graphicsObject.RotateTransform((float)gamma);
                float ellipseX = -Convert.ToSingle(radiusX - 10.0);
                float ellipseY = -Convert.ToSingle(radiusY - 10.0);
                float widthVar = Convert.ToSingle((radiusX - 10.0) * 2.0);
                float heightVar = Convert.ToSingle((radiusY - 10.0) * 2.0);
                graphicsObject.FillEllipse(new SolidBrush(Color.FromArgb(0, 255, 0)), ellipseX, ellipseY, widthVar, heightVar);
                graphicsObject.RotateTransform(-(float)gamma);
            }
            using (Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap))
            {
                byte[,,] data = image.Data;
                using (Image<Bgr, byte> map = new Image<Bgr, byte>(tempMappingImage))
                {
                    byte[,,] mapData = map.Data;
                    bool passedCalculation;
                    do
                    {
                        int totalPixels = 0;
                        int brightPixels = 0;
                        for (int x = 0; x < map.Width; x++)
                        {
                            for (int y = 0; y < map.Height; y++)
                            {
                                if (mapData[y, x, 1] == 255)
                                {
                                    totalPixels++;
                                    if ((int)data[y, x, 0] > workingNoiseLevel)
                                    {
                                        brightPixels++;
                                    }
                                }
                            }
                        }
                        double noiseRatio = Convert.ToDouble(brightPixels) / Convert.ToDouble(totalPixels);
                        if (noiseRatio > 0.0002)
                        {
                            passedCalculation = false;
                            workingNoiseLevel++;
                        }
                        else
                        {
                            passedCalculation = true;
                        }
                    }
                    while (!passedCalculation);
                }
            }
            return workingNoiseLevel;
        }

        /// <summary>
        /// This is the ClearView 1.1.1.2 Calibration routine with these modifications:
        /// 1) Saving the calibration image to disk is removed, CalibrationImage control
        ///    is now responsible for saving.
        /// 2) Copies the image in to a matlab array.
        /// 3) Always reports to console whether the image has passed or failed.
        /// 4) Removed BypassCheck because it is just used for interaction with the interface 
        ///    which is now controlled by the Capture/Calibration.xaml page where it should be.
        /// 5) Removed FailurePointCount because every use of it referred to a const now defined at the top.
        /// 6) Logging is handled by throwing exceptions.
        ///    All other pass and fail data will be reflected in the export.
        ///    The export is always reachable by the intended users.
        /// 7) Removed logging because Excel.CreateXLSXFromImageCalibrations includes all the 
        ///    information it is reporting.
        /// 8) Removed error file logging because Excel.CreateXLSXFromImageCalibrations includes 
        ///    all the information it is reporting.
        /// 9) Uses new determine color routine.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="settings"></param>
        /// <param name="colorizedEntity"></param>
        /// <returns>True if the image passes the specified intensity range, false if too many pixels are found outside the intensity bound.</returns>
        // Token: 0x06000006 RID: 6 RVA: 0x00003A98 File Offset: 0x00001C98
        public static DataLayer.Entities.ImageCalibration DoesImagePassValidation(Bitmap bitmap, DataLayer.Entities.DeviceCalibrationSetting settings, out DataLayer.Entities.Image colorizedEntity)
        {
            DataLayer.Entities.ImageCalibration result;
            try
            {
                int errors = 0;
                using (Image<Bgr, byte> colorized = new Image<Bgr, byte>(bitmap))
                {
                    using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(bitmap))
                    {
                        byte[,,] image = new byte[3, bitmap.Height, bitmap.Width];
                        byte[,,] data = cvImage.Data;
                        byte[,,] coloredData = colorized.Data;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                image[0, y, x] = data[y, x, 2];
                                image[1, y, x] = data[y, x, 1];
                                image[2, y, x] = data[y, x, 0];
                            }
                        }
                        object[] argsOut = Maths.FormCalc.compute_factors(1, image, "", "basic");
                        double[,] native = (double[,])argsOut[0];
                        int centerX = (int)native[0, 0];
                        int centerY = (int)native[1, 0];
                        int xPixelsFromRealCenter = 159 - centerX;
                        int yPixelsFromRealCenter = 119 - centerY;
                        int negMin = 255;
                        int negMax = 0;
                        int posMin = 255;
                        int posMax = 0;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                byte intensityVar = data[y, x, 0];
                                if (x + xPixelsFromRealCenter < 0 || x + xPixelsFromRealCenter >= 320 || y + yPixelsFromRealCenter < 0 || y + yPixelsFromRealCenter >= 240)
                                {
                                    coloredData[y, x, 0] = 0;
                                    coloredData[y, x, 1] = 0;
                                    coloredData[y, x, 2] = intensityVar;
                                }
                                else if ((short)intensityVar < Maths.LimitArray[x + xPixelsFromRealCenter, y + yPixelsFromRealCenter, 0])
                                {
                                    errors++;
                                    byte diff = (byte)(Maths.LimitArray[x + xPixelsFromRealCenter, y + yPixelsFromRealCenter, 0] - (short)intensityVar);
                                    coloredData[y, x, 0] = diff;
                                    coloredData[y, x, 1] = 0;
                                    coloredData[y, x, 2] = 0;
                                    if ((int)diff < negMin)
                                    {
                                        negMin = (int)diff;
                                    }
                                    if ((int)diff > negMax)
                                    {
                                        negMax = (int)diff;
                                    }
                                }
                                else if ((short)intensityVar > Maths.LimitArray[x + xPixelsFromRealCenter, y + yPixelsFromRealCenter, 1])
                                {
                                    errors++;
                                    byte diff = (byte)((short)intensityVar - Maths.LimitArray[x + xPixelsFromRealCenter, y + yPixelsFromRealCenter, 1]);
                                    coloredData[y, x, 0] = 0;
                                    coloredData[y, x, 1] = diff;
                                    coloredData[y, x, 2] = 0;
                                    if ((int)diff < posMin)
                                    {
                                        posMin = (int)diff;
                                    }
                                    if ((int)diff > posMax)
                                    {
                                        posMax = (int)diff;
                                    }
                                }
                                else
                                {
                                    coloredData[y, x, 0] = 0;
                                    coloredData[y, x, 1] = 0;
                                    coloredData[y, x, 2] = intensityVar;
                                }
                            }
                        }
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                if (x + xPixelsFromRealCenter < 0 || x + xPixelsFromRealCenter >= 320 || y + yPixelsFromRealCenter < 0 || y + yPixelsFromRealCenter >= 240)
                                {
                                    coloredData[y, x, 0] = coloredData[y, x, 2];
                                    coloredData[y, x, 1] = coloredData[y, x, 2];
                                }
                                else if (coloredData[y, x, 0] != 0)
                                {
                                    Color color = Maths.DetermineColorToUse((double)(-(double)coloredData[y, x, 0]), Maths.FailureRegion.HighP, (double)(-(double)negMax), (double)(-(double)negMin), (double)posMin, (double)posMax);
                                    coloredData[y, x, 0] = color.B;
                                    coloredData[y, x, 1] = color.G;
                                    coloredData[y, x, 2] = color.R;
                                }
                                else if (coloredData[y, x, 1] != 0)
                                {
                                    Color color = Maths.DetermineColorToUse((double)coloredData[y, x, 0], Maths.FailureRegion.Inner, (double)(-(double)negMax), (double)(-(double)negMin), (double)posMin, (double)posMax);
                                    coloredData[y, x, 0] = color.B;
                                    coloredData[y, x, 1] = color.G;
                                    coloredData[y, x, 2] = color.R;
                                }
                                else
                                {
                                    coloredData[y, x, 0] = coloredData[y, x, 2];
                                    coloredData[y, x, 1] = coloredData[y, x, 2];
                                }
                            }
                        }
                        colorizedEntity = new DataLayer.Entities.Image
                        {
                            ImageData = Maths.CompressImage(colorized.Bitmap)
                        };
                    }
                }
                bool failed = errors >= (int)Maths.MAX_PIXELS_THAT_CAN_FAIL;
                result = new DataLayer.Entities.ImageCalibration
                {
                    Failed = failed,
                    TotalFailed = failed,
                    TotalFailures = (double)errors
                };
            }
            catch (Exception ex)
            {
                Maths.Log.Error("DoesImagePassCalibration failed.", ex);
                colorizedEntity = null;
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Splits an energized image into different sectors for each finger.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="calibration"></param>
        /// <param name="angle"></param>
        /// <param name="centerX"></param>
        /// <param name="centerY"></param>
        /// <param name="noiseLevel"></param>
        /// <param name="finger"></param>
        /// <returns></returns>
        // Token: 0x06000007 RID: 7 RVA: 0x000041A4 File Offset: 0x000023A4
        public static IEnumerable<DataLayer.Entities.ImageSector> GetImageSectors(Bitmap image, Bitmap calibration, double angle, double centerX, double centerY, double noiseLevel, string finger)
        {
            byte[,,] energizedImage = new byte[3, image.Height, image.Width];
            using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(image))
            {
                byte[,,] data = cvImage.Data;
                for (int x2 = 0; x2 < image.Width; x2++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        energizedImage[0, y, x2] = data[y, x2, 2];
                        energizedImage[1, y, x2] = data[y, x2, 1];
                        energizedImage[2, y, x2] = data[y, x2, 0];
                    }
                }
            }
            byte[,,] calibrationImage = new byte[3, calibration.Height, calibration.Width];
            using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(calibration))
            {
                byte[,,] data = cvImage.Data;
                for (int x2 = 0; x2 < calibration.Width; x2++)
                {
                    for (int y = 0; y < calibration.Height; y++)
                    {
                        calibrationImage[0, y, x2] = data[y, x2, 2];
                        calibrationImage[1, y, x2] = data[y, x2, 1];
                        calibrationImage[2, y, x2] = data[y, x2, 0];
                    }
                }
            }
            object[] argsOut = Maths.FormCalc.compute_factors(16, energizedImage, "", "full", calibrationImage, new double[]
            {
                angle
            }, new double[]
            {
                centerX,
                centerY
            }, new double[]
            {
                noiseLevel
            }, finger);
            List<DataLayer.Entities.ImageSector> sectors = new List<DataLayer.Entities.ImageSector>();
            double[,] formHolder = (double[,])argsOut[1];
            double[,] form2Holder = (double[,])argsOut[2];
            object[,] tempFormSubHolder = (object[,])argsOut[3];
            object[,] tempForm2SubHolder = (object[,])argsOut[4];
            object[,] tempBreakCoeffHolder = (object[,])argsOut[5];
            double[,] js = (double[,])argsOut[6];
            double[,] intAv = (double[,])argsOut[7];
            double[,] intAvSec = (double[,])argsOut[8];
            double[,] aTot = (double[,])argsOut[9];
            double[,] aNorm = (double[,])argsOut[10];
            double[,] fracdim = (double[,])argsOut[11];
            double[,] ent = (double[,])argsOut[12];
            object[,] tempRingWidthHolder = (object[,])argsOut[13];
            object[,] tempRingIntensityHolder = (object[,])argsOut[14];
            double[,] sectorAngles = (double[,])argsOut[15];
            int numSectors = aTot.GetLength(1);
            for (int i = 0; i < numSectors; i++)
            {
                double[,] form = (double[,])tempFormSubHolder[0, i];
                double[,] form2 = (double[,])tempForm2SubHolder[i, 0];
                double[,] breakC = (double[,])tempBreakCoeffHolder[i, 0];
                double[,] ringW = (double[,])tempRingWidthHolder[0, i];
                double[,] ringI = (double[,])tempRingIntensityHolder[0, i];
                double degree = Math.Abs(sectorAngles[i, 0] - sectorAngles[i, 1]);
                if (sectorAngles[i, 0] > sectorAngles[i, 1])
                {
                    degree = 360.0 - sectorAngles[i, 0] + sectorAngles[i, 1];
                }
                sectors.Add(new DataLayer.Entities.ImageSector
                {
                    SectorNumber = (short)i,
                    StartAngle = sectorAngles[i, 0],
                    EndAngle = sectorAngles[i, 1],
                    IntegralArea = aNorm[0, i],
                    SectorArea = aTot[0, i],
                    NormalizedArea = aTot[0, i] * 360.0 / (double)numSectors / degree,
                    Entropy = ent[0, i],
                    FractalCoefficient = fracdim[0, i],
                    JsInteger = js[i, 0],
                    AverageIntensity = intAv[0, i],
                    FormCoefficient = formHolder[i, 0],
                    Form2 = form2Holder[i, 0],
                    Form11 = Enumerable.Range(0, form.GetLength(1)).Max((int x) => (double)form.GetValue(0, x)),
                    Form12 = Enumerable.Range(0, form.GetLength(1)).Max((int x) => (double)form.GetValue(1, x)),
                    Form13 = Enumerable.Range(0, form.GetLength(1)).Max((int x) => (double)form.GetValue(2, x)),
                    Form14 = Enumerable.Range(0, form.GetLength(1)).Max((int x) => (double)form.GetValue(3, x)),
                    Form2Prime = Enumerable.Range(0, form2.GetLength(1)).Max((int x) => (double)form2.GetValue(0, x)),
                    Ai1 = intAvSec[0, i],
                    Ai2 = intAvSec[1, i],
                    Ai3 = intAvSec[2, i],
                    Ai4 = intAvSec[3, i],
                    BreakCoefficient = Enumerable.Range(0, breakC.GetLength(1)).Sum((int x) => (double)breakC.GetValue(0, x)),
                    RingThickness = Enumerable.Range(0, ringW.GetLength(1)).Average((int x) => (double)ringW.GetValue(0, x)),
                    RingIntensity = Enumerable.Range(0, ringI.GetLength(1)).Average((int x) => (double)ringI.GetValue(0, x))
                });
            }
            return sectors;
        }

        /// <summary>
        /// Calculates the orientation for the given images.
        /// </summary>
        /// <param name="energized"></param>
        /// <param name="finger"></param>
        /// <returns></returns>
        // Token: 0x06000008 RID: 8 RVA: 0x000047D0 File Offset: 0x000029D0
        public static DataLayer.Entities.ImageAlignment GetAlignment(Bitmap energized, Bitmap finger)
        {
            byte[,,] energizedImage = new byte[3, energized.Height, energized.Width];
            using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(energized))
            {
                byte[,,] data = cvImage.Data;
                for (int x = 0; x < energized.Width; x++)
                {
                    for (int y = 0; y < energized.Height; y++)
                    {
                        energizedImage[0, y, x] = data[y, x, 2];
                        energizedImage[1, y, x] = data[y, x, 1];
                        energizedImage[2, y, x] = data[y, x, 0];
                    }
                }
            }
            byte[,,] fingerImage = null;
            if (finger != null)
            {
                fingerImage = new byte[3, finger.Height, finger.Width];
                using (Image<Bgr, byte> cvImage = new Image<Bgr, byte>(finger))
                {
                    byte[,,] data = cvImage.Data;
                    for (int x = 0; x < finger.Width; x++)
                    {
                        for (int y = 0; y < finger.Height; y++)
                        {
                            fingerImage[0, y, x] = data[y, x, 2];
                            fingerImage[1, y, x] = data[y, x, 1];
                            fingerImage[2, y, x] = data[y, x, 0];
                        }
                    }
                }
            }
            object[] argsOut = Maths.FormCalc.compute_factors(1, energizedImage, fingerImage ?? "", "basic");
            double[,] native = (double[,])argsOut[0];
            double centerX = native[0, 0];
            double centerY = native[1, 0];
            double radiusX = native[2, 0];
            double radiusY = native[3, 0];
            double gamma = native[4, 0] * 57.29577951308232;
            return new DataLayer.Entities.ImageAlignment
            {
                CenterX = centerX,
                CenterY = centerY,
                RadiusX = radiusX,
                RadiusY = radiusY,
                Angle = gamma
            };
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00004A10 File Offset: 0x00002C10
        private static short[,,] GetLimitArray()
        {
            short[,,] result = new short[320, 240, 2];
            string[] lines = "\r\n0\t0\t8\t28\t0\r\n0\t1\t9\t27\t0\r\n0\t2\t5\t29\t0\r\n0\t3\t4\t30\t0\r\n0\t4\t1\t33\t0\r\n0\t5\t0\t32\t0\r\n0\t6\t1\t32\t0\r\n0\t7\t0\t32\t0\r\n0\t8\t0\t33\t0\r\n0\t9\t0\t33\t0\r\n0\t10\t0\t33\t0\r\n0\t11\t0\t33\t0\r\n0\t12\t0\t33\t0\r\n0\t13\t0\t33\t0\r\n0\t14\t0\t33\t0\r\n0\t15\t1\t33\t0\r\n0\t16\t1\t33\t0\r\n0\t17\t1\t33\t0\r\n0\t18\t0\t33\t0\r\n0\t19\t1\t33\t0\r\n0\t20\t1\t33\t0\r\n0\t21\t0\t33\t0\r\n0\t22\t1\t33\t0\r\n0\t23\t1\t33\t0\r\n0\t24\t1\t33\t0\r\n0\t25\t1\t33\t0\r\n0\t26\t0\t33\t0\r\n0\t27\t1\t33\t0\r\n0\t28\t2\t32\t0\r\n0\t29\t0\t33\t0\r\n0\t30\t1\t32\t0\r\n0\t31\t1\t32\t0\r\n0\t32\t1\t33\t0\r\n0\t33\t0\t34\t0\r\n0\t34\t1\t33\t0\r\n0\t35\t1\t32\t0\r\n0\t36\t1\t33\t0\r\n0\t37\t1\t33\t0\r\n0\t38\t1\t33\t0\r\n0\t39\t1\t33\t0\r\n0\t40\t1\t33\t0\r\n0\t41\t1\t33\t0\r\n0\t42\t1\t32\t0\r\n0\t43\t1\t32\t0\r\n0\t44\t0\t33\t0\r\n0\t45\t0\t33\t0\r\n0\t46\t1\t32\t0\r\n0\t47\t1\t32\t0\r\n0\t48\t1\t32\t0\r\n0\t49\t0\t34\t0\r\n0\t50\t0\t33\t0\r\n0\t51\t1\t32\t0\r\n0\t52\t1\t32\t0\r\n0\t53\t0\t33\t0\r\n0\t54\t1\t32\t0\r\n0\t55\t1\t32\t0\r\n0\t56\t1\t32\t0\r\n0\t57\t0\t33\t0\r\n0\t58\t0\t33\t0\r\n0\t59\t1\t32\t0\r\n0\t60\t0\t34\t0\r\n0\t61\t0\t33\t0\r\n0\t62\t1\t33\t0\r\n0\t63\t0\t33\t0\r\n0\t64\t0\t34\t0\r\n0\t65\t0\t33\t0\r\n0\t66\t1\t33\t0\r\n0\t67\t1\t32\t0\r\n0\t68\t0\t34\t0\r\n0\t69\t1\t33\t0\r\n0\t70\t1\t32\t0\r\n0\t71\t0\t32\t0\r\n0\t72\t1\t33\t0\r\n0\t73\t0\t34\t0\r\n0\t74\t0\t33\t0\r\n0\t75\t1\t32\t0\r\n0\t76\t1\t33\t0\r\n0\t77\t1\t32\t0\r\n0\t78\t1\t32\t0\r\n0\t79\t0\t33\t0\r\n0\t80\t1\t33\t0\r\n0\t81\t0\t33\t0\r\n0\t82\t0\t33\t0\r\n0\t83\t1\t32\t0\r\n0\t84\t1\t33\t0\r\n0\t85\t0\t33\t0\r\n0\t86\t1\t32\t0\r\n0\t87\t0\t33\t0\r\n0\t88\t0\t33\t0\r\n0\t89\t0\t34\t0\r\n0\t90\t1\t33\t0\r\n0\t91\t1\t32\t0\r\n0\t92\t1\t33\t0\r\n0\t93\t0\t33\t0\r\n0\t94\t1\t32\t0\r\n0\t95\t0\t33\t0\r\n0\t96\t0\t34\t0\r\n0\t97\t0\t33\t0\r\n0\t98\t0\t33\t0\r\n0\t99\t1\t33\t0\r\n0\t100\t0\t33\t0\r\n0\t101\t1\t33\t0\r\n0\t102\t0\t34\t0\r\n0\t103\t1\t33\t0\r\n0\t104\t0\t33\t0\r\n0\t105\t0\t34\t0\r\n0\t106\t1\t32\t0\r\n0\t107\t0\t33\t0\r\n0\t108\t0\t33\t0\r\n0\t109\t0\t33\t0\r\n0\t110\t0\t34\t0\r\n0\t111\t1\t32\t0\r\n0\t112\t1\t32\t0\r\n0\t113\t1\t33\t0\r\n0\t114\t1\t32\t0\r\n0\t115\t1\t33\t0\r\n0\t116\t1\t33\t0\r\n0\t117\t0\t33\t0\r\n0\t118\t0\t33\t0\r\n0\t119\t0\t33\t0\r\n0\t120\t0\t33\t0\r\n0\t121\t0\t34\t0\r\n0\t122\t1\t33\t0\r\n0\t123\t0\t33\t0\r\n0\t124\t0\t33\t0\r\n0\t125\t1\t32\t0\r\n0\t126\t1\t33\t0\r\n0\t127\t1\t32\t0\r\n0\t128\t1\t32\t0\r\n0\t129\t0\t33\t0\r\n0\t130\t1\t33\t0\r\n0\t131\t0\t33\t0\r\n0\t132\t0\t33\t0\r\n0\t133\t1\t33\t0\r\n0\t134\t1\t33\t0\r\n0\t135\t1\t32\t0\r\n0\t136\t0\t33\t0\r\n0\t137\t0\t34\t0\r\n0\t138\t0\t33\t0\r\n0\t139\t1\t32\t0\r\n0\t140\t1\t33\t0\r\n0\t141\t0\t33\t0\r\n0\t142\t0\t33\t0\r\n0\t143\t0\t33\t0\r\n0\t144\t1\t33\t0\r\n0\t145\t0\t33\t0\r\n0\t146\t0\t33\t0\r\n0\t147\t1\t33\t0\r\n0\t148\t1\t33\t0\r\n0\t149\t0\t34\t0\r\n0\t150\t0\t33\t0\r\n0\t151\t0\t33\t0\r\n0\t152\t1\t32\t0\r\n0\t153\t0\t33\t0\r\n0\t154\t1\t32\t0\r\n0\t155\t0\t33\t0\r\n0\t156\t0\t33\t0\r\n0\t157\t1\t33\t0\r\n0\t158\t1\t33\t0\r\n0\t159\t0\t33\t0\r\n0\t160\t1\t33\t0\r\n0\t161\t1\t33\t0\r\n0\t162\t0\t33\t0\r\n0\t163\t1\t33\t0\r\n0\t164\t1\t33\t0\r\n0\t165\t0\t33\t0\r\n0\t166\t1\t32\t0\r\n0\t167\t1\t32\t0\r\n0\t168\t1\t33\t0\r\n0\t169\t1\t33\t0\r\n0\t170\t0\t33\t0\r\n0\t171\t0\t33\t0\r\n0\t172\t1\t32\t0\r\n0\t173\t0\t33\t0\r\n0\t174\t0\t32\t0\r\n0\t175\t0\t33\t0\r\n0\t176\t0\t33\t0\r\n0\t177\t0\t34\t0\r\n0\t178\t1\t33\t0\r\n0\t179\t0\t33\t0\r\n0\t180\t1\t33\t0\r\n0\t181\t1\t32\t0\r\n0\t182\t0\t33\t0\r\n0\t183\t0\t33\t0\r\n0\t184\t0\t33\t0\r\n0\t185\t0\t33\t0\r\n0\t186\t0\t33\t0\r\n0\t187\t0\t33\t0\r\n0\t188\t0\t34\t0\r\n0\t189\t0\t33\t0\r\n0\t190\t0\t33\t0\r\n0\t191\t1\t33\t0\r\n0\t192\t1\t33\t0\r\n0\t193\t0\t33\t0\r\n0\t194\t0\t33\t0\r\n0\t195\t0\t33\t0\r\n0\t196\t1\t32\t0\r\n0\t197\t1\t32\t0\r\n0\t198\t1\t33\t0\r\n0\t199\t1\t32\t0\r\n0\t200\t1\t33\t0\r\n0\t201\t0\t33\t0\r\n0\t202\t2\t31\t0\r\n0\t203\t1\t33\t0\r\n0\t204\t1\t33\t0\r\n0\t205\t1\t33\t0\r\n0\t206\t0\t33\t0\r\n0\t207\t0\t33\t0\r\n0\t208\t1\t33\t0\r\n0\t209\t0\t33\t0\r\n0\t210\t0\t33\t0\r\n0\t211\t1\t33\t0\r\n0\t212\t1\t33\t0\r\n0\t213\t1\t33\t0\r\n0\t214\t0\t33\t0\r\n0\t215\t1\t33\t0\r\n0\t216\t0\t33\t0\r\n0\t217\t1\t33\t0\r\n0\t218\t1\t33\t0\r\n0\t219\t1\t32\t0\r\n0\t220\t1\t33\t0\r\n0\t221\t1\t33\t0\r\n0\t222\t1\t33\t0\r\n0\t223\t2\t32\t0\r\n0\t224\t1\t32\t0\r\n0\t225\t0\t33\t0\r\n0\t226\t0\t33\t0\r\n0\t227\t1\t32\t0\r\n0\t228\t0\t36\t0\r\n0\t229\t1\t32\t0\r\n0\t230\t1\t32\t0\r\n0\t231\t1\t32\t0\r\n0\t232\t1\t32\t0\r\n0\t233\t1\t33\t0\r\n0\t234\t3\t33\t0\r\n0\t235\t4\t33\t0\r\n0\t236\t4\t33\t0\r\n0\t237\t5\t33\t0\r\n0\t238\t5\t32\t0\r\n0\t239\t5\t33\t0\r\n1\t0\t8\t28\t0\r\n1\t1\t8\t28\t0\r\n1\t2\t5\t29\t0\r\n1\t3\t4\t30\t0\r\n1\t4\t0\t33\t0\r\n1\t5\t0\t32\t0\r\n1\t6\t1\t32\t0\r\n1\t7\t0\t32\t0\r\n1\t8\t1\t32\t0\r\n1\t9\t0\t33\t0\r\n1\t10\t1\t32\t0\r\n1\t11\t1\t32\t0\r\n1\t12\t0\t32\t0\r\n1\t13\t1\t32\t0\r\n1\t14\t0\t32\t0\r\n1\t15\t0\t33\t0\r\n1\t16\t0\t33\t0\r\n1\t17\t0\t34\t0\r\n1\t18\t0\t32\t0\r\n1\t19\t1\t33\t0\r\n1\t20\t1\t33\t0\r\n1\t21\t1\t32\t0\r\n1\t22\t1\t32\t0\r\n1\t23\t0\t32\t0\r\n1\t24\t0\t33\t0\r\n1\t25\t0\t34\t0\r\n1\t26\t0\t33\t0\r\n1\t27\t1\t33\t0\r\n1\t28\t1\t32\t0\r\n1\t29\t1\t32\t0\r\n1\t30\t1\t32\t0\r\n1\t31\t1\t32\t0\r\n1\t32\t0\t33\t0\r\n1\t33\t0\t34\t0\r\n1\t34\t0\t33\t0\r\n1\t35\t1\t32\t0\r\n1\t36\t0\t33\t0\r\n1\t37\t0\t33\t0\r\n1\t38\t1\t32\t0\r\n1\t39\t0\t33\t0\r\n1\t40\t0\t33\t0\r\n1\t41\t1\t32\t0\r\n1\t42\t0\t33\t0\r\n1\t43\t0\t33\t0\r\n1\t44\t0\t33\t0\r\n1\t45\t0\t33\t0\r\n1\t46\t1\t32\t0\r\n1\t47\t0\t32\t0\r\n1\t48\t0\t33\t0\r\n1\t49\t0\t33\t0\r\n1\t50\t0\t34\t0\r\n1\t51\t1\t33\t0\r\n1\t52\t0\t33\t0\r\n1\t53\t0\t33\t0\r\n1\t54\t0\t32\t0\r\n1\t55\t1\t32\t0\r\n1\t56\t0\t33\t0\r\n1\t57\t0\t33\t0\r\n1\t58\t0\t33\t0\r\n1\t59\t1\t32\t0\r\n1\t60\t0\t32\t0\r\n1\t61\t1\t33\t0\r\n1\t62\t0\t33\t0\r\n1\t63\t0\t32\t0\r\n1\t64\t0\t33\t0\r\n1\t65\t1\t32\t0\r\n1\t66\t0\t33\t0\r\n1\t67\t1\t33\t0\r\n1\t68\t0\t33\t0\r\n1\t69\t0\t33\t0\r\n1\t70\t0\t33\t0\r\n1\t71\t0\t32\t0\r\n1\t72\t0\t34\t0\r\n1\t73\t0\t33\t0\r\n1\t74\t1\t32\t0\r\n1\t75\t0\t33\t0\r\n1\t76\t0\t33\t0\r\n1\t77\t0\t33\t0\r\n1\t78\t0\t33\t0\r\n1\t79\t0\t33\t0\r\n1\t80\t0\t33\t0\r\n1\t81\t0\t33\t0\r\n1\t82\t0\t33\t0\r\n1\t83\t0\t32\t0\r\n1\t84\t1\t32\t0\r\n1\t85\t0\t33\t0\r\n1\t86\t0\t32\t0\r\n1\t87\t0\t32\t0\r\n1\t88\t0\t33\t0\r\n1\t89\t1\t32\t0\r\n1\t90\t1\t32\t0\r\n1\t91\t0\t33\t0\r\n1\t92\t1\t33\t0\r\n1\t93\t1\t32\t0\r\n1\t94\t1\t33\t0\r\n1\t95\t0\t33\t0\r\n1\t96\t1\t32\t0\r\n1\t97\t0\t33\t0\r\n1\t98\t1\t32\t0\r\n1\t99\t0\t33\t0\r\n1\t100\t0\t32\t0\r\n1\t101\t0\t33\t0\r\n1\t102\t0\t32\t0\r\n1\t103\t0\t32\t0\r\n1\t104\t0\t33\t0\r\n1\t105\t0\t34\t0\r\n1\t106\t0\t34\t0\r\n1\t107\t1\t32\t0\r\n1\t108\t0\t33\t0\r\n1\t109\t0\t33\t0\r\n1\t110\t0\t39\t0\r\n1\t111\t0\t32\t0\r\n1\t112\t1\t32\t0\r\n1\t113\t0\t33\t0\r\n1\t114\t0\t32\t0\r\n1\t115\t0\t33\t0\r\n1\t116\t1\t33\t0\r\n1\t117\t0\t33\t0\r\n1\t118\t0\t33\t0\r\n1\t119\t0\t33\t0\r\n1\t120\t1\t32\t0\r\n1\t121\t0\t33\t0\r\n1\t122\t1\t32\t0\r\n1\t123\t1\t32\t0\r\n1\t124\t0\t33\t0\r\n1\t125\t0\t33\t0\r\n1\t126\t0\t32\t0\r\n1\t127\t0\t33\t0\r\n1\t128\t0\t33\t0\r\n1\t129\t0\t33\t0\r\n1\t130\t0\t33\t0\r\n1\t131\t1\t33\t0\r\n1\t132\t1\t33\t0\r\n1\t133\t0\t33\t0\r\n1\t134\t1\t32\t0\r\n1\t135\t1\t32\t0\r\n1\t136\t0\t33\t0\r\n1\t137\t0\t34\t0\r\n1\t138\t0\t34\t0\r\n1\t139\t1\t32\t0\r\n1\t140\t1\t32\t0\r\n1\t141\t1\t33\t0\r\n1\t142\t0\t33\t0\r\n1\t143\t0\t32\t0\r\n1\t144\t1\t32\t0\r\n1\t145\t1\t32\t0\r\n1\t146\t1\t32\t0\r\n1\t147\t2\t31\t0\r\n1\t148\t0\t33\t0\r\n1\t149\t0\t33\t0\r\n1\t150\t0\t33\t0\r\n1\t151\t0\t33\t0\r\n1\t152\t1\t33\t0\r\n1\t153\t0\t32\t0\r\n1\t154\t1\t32\t0\r\n1\t155\t1\t32\t0\r\n1\t156\t1\t32\t0\r\n1\t157\t1\t32\t0\r\n1\t158\t0\t33\t0\r\n1\t159\t0\t33\t0\r\n1\t160\t1\t32\t0\r\n1\t161\t1\t32\t0\r\n1\t162\t0\t33\t0\r\n1\t163\t0\t33\t0\r\n1\t164\t0\t33\t0\r\n1\t165\t0\t32\t0\r\n1\t166\t0\t32\t0\r\n1\t167\t0\t32\t0\r\n1\t168\t0\t33\t0\r\n1\t169\t0\t32\t0\r\n1\t170\t0\t32\t0\r\n1\t171\t0\t33\t0\r\n1\t172\t1\t32\t0\r\n1\t173\t1\t32\t0\r\n1\t174\t0\t32\t0\r\n1\t175\t1\t32\t0\r\n1\t176\t1\t32\t0\r\n1\t177\t0\t33\t0\r\n1\t178\t0\t35\t0\r\n1\t179\t0\t32\t0\r\n1\t180\t0\t32\t0\r\n1\t181\t0\t33\t0\r\n1\t182\t1\t33\t0\r\n1\t183\t1\t32\t0\r\n1\t184\t0\t33\t0\r\n1\t185\t0\t33\t0\r\n1\t186\t0\t33\t0\r\n1\t187\t0\t32\t0\r\n1\t188\t0\t32\t0\r\n1\t189\t0\t33\t0\r\n1\t190\t0\t33\t0\r\n1\t191\t0\t33\t0\r\n1\t192\t0\t33\t0\r\n1\t193\t0\t33\t0\r\n1\t194\t1\t32\t0\r\n1\t195\t0\t33\t0\r\n1\t196\t1\t32\t0\r\n1\t197\t1\t32\t0\r\n1\t198\t0\t32\t0\r\n1\t199\t1\t32\t0\r\n1\t200\t1\t32\t0\r\n1\t201\t0\t34\t0\r\n1\t202\t1\t32\t0\r\n1\t203\t0\t33\t0\r\n1\t204\t1\t33\t0\r\n1\t205\t1\t32\t0\r\n1\t206\t1\t32\t0\r\n1\t207\t1\t32\t0\r\n1\t208\t1\t33\t0\r\n1\t209\t0\t34\t0\r\n1\t210\t0\t33\t0\r\n1\t211\t0\t32\t0\r\n1\t212\t1\t32\t0\r\n1\t213\t1\t32\t0\r\n1\t214\t0\t32\t0\r\n1\t215\t1\t32\t0\r\n1\t216\t1\t32\t0\r\n1\t217\t1\t33\t0\r\n1\t218\t1\t32\t0\r\n1\t219\t1\t33\t0\r\n1\t220\t1\t32\t0\r\n1\t221\t1\t32\t0\r\n1\t222\t1\t32\t0\r\n1\t223\t1\t32\t0\r\n1\t224\t1\t32\t0\r\n1\t225\t0\t32\t0\r\n1\t226\t0\t33\t0\r\n1\t227\t0\t32\t0\r\n1\t228\t0\t33\t0\r\n1\t229\t0\t32\t0\r\n1\t230\t0\t32\t0\r\n1\t231\t1\t32\t0\r\n1\t232\t2\t32\t0\r\n1\t233\t1\t33\t0\r\n1\t234\t3\t33\t0\r\n1\t235\t4\t32\t0\r\n1\t236\t5\t33\t0\r\n1\t237\t5\t33\t0\r\n1\t238\t5\t33\t0\r\n1\t239\t4\t33\t0\r\n2\t0\t8\t28\t0\r\n2\t1\t7\t28\t0\r\n2\t2\t4\t29\t0\r\n2\t3\t4\t30\t0\r\n2\t4\t0\t33\t0\r\n2\t5\t0\t32\t0\r\n2\t6\t1\t32\t0\r\n2\t7\t1\t32\t0\r\n2\t8\t1\t32\t0\r\n2\t9\t1\t32\t0\r\n2\t10\t0\t33\t0\r\n2\t11\t0\t32\t0\r\n2\t12\t1\t32\t0\r\n2\t13\t1\t32\t0\r\n2\t14\t0\t32\t0\r\n2\t15\t2\t31\t0\r\n2\t16\t2\t32\t0\r\n2\t17\t0\t33\t0\r\n2\t18\t0\t33\t0\r\n2\t19\t1\t32\t0\r\n2\t20\t0\t33\t0\r\n2\t21\t0\t33\t0\r\n2\t22\t0\t33\t0\r\n2\t23\t1\t31\t0\r\n2\t24\t1\t32\t0\r\n2\t25\t1\t33\t0\r\n2\t26\t1\t33\t0\r\n2\t27\t2\t31\t0\r\n2\t28\t1\t32\t0\r\n2\t29\t1\t31\t0\r\n2\t30\t1\t32\t0\r\n2\t31\t1\t32\t0\r\n2\t32\t1\t33\t0\r\n2\t33\t1\t32\t0\r\n2\t34\t0\t33\t0\r\n2\t35\t1\t33\t0\r\n2\t36\t1\t33\t0\r\n2\t37\t1\t32\t0\r\n2\t38\t1\t32\t0\r\n2\t39\t0\t33\t0\r\n2\t40\t1\t32\t0\r\n2\t41\t1\t32\t0\r\n2\t42\t1\t32\t0\r\n2\t43\t0\t32\t0\r\n2\t44\t1\t32\t0\r\n2\t45\t0\t33\t0\r\n2\t46\t0\t32\t0\r\n2\t47\t1\t32\t0\r\n2\t48\t0\t32\t0\r\n2\t49\t0\t33\t0\r\n2\t50\t0\t33\t0\r\n2\t51\t0\t33\t0\r\n2\t52\t0\t33\t0\r\n2\t53\t0\t33\t0\r\n2\t54\t0\t32\t0\r\n2\t55\t0\t32\t0\r\n2\t56\t0\t33\t0\r\n2\t57\t0\t33\t0\r\n2\t58\t0\t33\t0\r\n2\t59\t0\t33\t0\r\n2\t60\t1\t32\t0\r\n2\t61\t0\t33\t0\r\n2\t62\t1\t32\t0\r\n2\t63\t1\t32\t0\r\n2\t64\t1\t32\t0\r\n2\t65\t1\t32\t0\r\n2\t66\t0\t33\t0\r\n2\t67\t1\t32\t0\r\n2\t68\t0\t33\t0\r\n2\t69\t1\t32\t0\r\n2\t70\t1\t32\t0\r\n2\t71\t1\t32\t0\r\n2\t72\t0\t34\t0\r\n2\t73\t0\t33\t0\r\n2\t74\t0\t33\t0\r\n2\t75\t1\t32\t0\r\n2\t76\t0\t32\t0\r\n2\t77\t0\t33\t0\r\n2\t78\t0\t33\t0\r\n2\t79\t0\t33\t0\r\n2\t80\t0\t33\t0\r\n2\t81\t0\t33\t0\r\n2\t82\t0\t33\t0\r\n2\t83\t0\t33\t0\r\n2\t84\t0\t33\t0\r\n2\t85\t0\t32\t0\r\n2\t86\t1\t32\t0\r\n2\t87\t0\t32\t0\r\n2\t88\t0\t33\t0\r\n2\t89\t0\t33\t0\r\n2\t90\t1\t32\t0\r\n2\t91\t1\t32\t0\r\n2\t92\t1\t33\t0\r\n2\t93\t1\t32\t0\r\n2\t94\t1\t32\t0\r\n2\t95\t0\t32\t0\r\n2\t96\t0\t32\t0\r\n2\t97\t0\t33\t0\r\n2\t98\t1\t32\t0\r\n2\t99\t1\t32\t0\r\n2\t100\t0\t34\t0\r\n2\t101\t0\t38\t0\r\n2\t102\t0\t32\t0\r\n2\t103\t1\t32\t0\r\n2\t104\t0\t32\t0\r\n2\t105\t1\t32\t0\r\n2\t106\t0\t39\t0\r\n2\t107\t0\t32\t0\r\n2\t108\t1\t32\t0\r\n2\t109\t0\t33\t0\r\n2\t110\t1\t32\t0\r\n2\t111\t1\t32\t0\r\n2\t112\t0\t33\t0\r\n2\t113\t0\t34\t0\r\n2\t114\t0\t33\t0\r\n2\t115\t1\t32\t0\r\n2\t116\t1\t32\t0\r\n2\t117\t2\t32\t0\r\n2\t118\t1\t31\t0\r\n2\t119\t0\t33\t0\r\n2\t120\t0\t33\t0\r\n2\t121\t1\t33\t0\r\n2\t122\t0\t33\t0\r\n2\t123\t1\t32\t0\r\n2\t124\t0\t33\t0\r\n2\t125\t0\t33\t0\r\n2\t126\t1\t32\t0\r\n2\t127\t0\t33\t0\r\n2\t128\t1\t33\t0\r\n2\t129\t0\t33\t0\r\n2\t130\t0\t33\t0\r\n2\t131\t1\t32\t0\r\n2\t132\t0\t33\t0\r\n2\t133\t0\t33\t0\r\n2\t134\t0\t32\t0\r\n2\t135\t0\t33\t0\r\n2\t136\t0\t33\t0\r\n2\t137\t0\t33\t0\r\n2\t138\t1\t32\t0\r\n2\t139\t1\t32\t0\r\n2\t140\t0\t33\t0\r\n2\t141\t1\t32\t0\r\n2\t142\t0\t33\t0\r\n2\t143\t0\t32\t0\r\n2\t144\t1\t32\t0\r\n2\t145\t1\t32\t0\r\n2\t146\t1\t32\t0\r\n2\t147\t1\t32\t0\r\n2\t148\t0\t32\t0\r\n2\t149\t0\t32\t0\r\n2\t150\t0\t32\t0\r\n2\t151\t1\t32\t0\r\n2\t152\t0\t33\t0\r\n2\t153\t0\t33\t0\r\n2\t154\t1\t33\t0\r\n2\t155\t0\t34\t0\r\n2\t156\t0\t34\t0\r\n2\t157\t1\t32\t0\r\n2\t158\t0\t32\t0\r\n2\t159\t1\t32\t0\r\n2\t160\t0\t33\t0\r\n2\t161\t0\t32\t0\r\n2\t162\t0\t32\t0\r\n2\t163\t0\t33\t0\r\n2\t164\t0\t33\t0\r\n2\t165\t0\t33\t0\r\n2\t166\t0\t32\t0\r\n2\t167\t0\t32\t0\r\n2\t168\t1\t33\t0\r\n2\t169\t0\t33\t0\r\n2\t170\t0\t33\t0\r\n2\t171\t1\t32\t0\r\n2\t172\t2\t31\t0\r\n2\t173\t0\t32\t0\r\n2\t174\t0\t32\t0\r\n2\t175\t0\t32\t0\r\n2\t176\t0\t33\t0\r\n2\t177\t0\t34\t0\r\n2\t178\t0\t34\t0\r\n2\t179\t2\t31\t0\r\n2\t180\t1\t32\t0\r\n2\t181\t0\t33\t0\r\n2\t182\t0\t32\t0\r\n2\t183\t1\t32\t0\r\n2\t184\t0\t33\t0\r\n2\t185\t0\t33\t0\r\n2\t186\t0\t33\t0\r\n2\t187\t0\t33\t0\r\n2\t188\t0\t35\t0\r\n2\t189\t0\t34\t0\r\n2\t190\t0\t32\t0\r\n2\t191\t0\t33\t0\r\n2\t192\t1\t33\t0\r\n2\t193\t0\t33\t0\r\n2\t194\t1\t32\t0\r\n2\t195\t0\t33\t0\r\n2\t196\t1\t32\t0\r\n2\t197\t0\t32\t0\r\n2\t198\t0\t32\t0\r\n2\t199\t0\t33\t0\r\n2\t200\t0\t33\t0\r\n2\t201\t0\t33\t0\r\n2\t202\t0\t33\t0\r\n2\t203\t1\t32\t0\r\n2\t204\t2\t32\t0\r\n2\t205\t0\t33\t0\r\n2\t206\t1\t32\t0\r\n2\t207\t1\t32\t0\r\n2\t208\t1\t32\t0\r\n2\t209\t0\t33\t0\r\n2\t210\t1\t33\t0\r\n2\t211\t1\t32\t0\r\n2\t212\t1\t32\t0\r\n2\t213\t1\t32\t0\r\n2\t214\t1\t32\t0\r\n2\t215\t1\t32\t0\r\n2\t216\t1\t32\t0\r\n2\t217\t1\t32\t0\r\n2\t218\t0\t32\t0\r\n2\t219\t1\t32\t0\r\n2\t220\t0\t34\t0\r\n2\t221\t0\t33\t0\r\n2\t222\t1\t32\t0\r\n2\t223\t1\t32\t0\r\n2\t224\t1\t32\t0\r\n2\t225\t0\t33\t0\r\n2\t226\t0\t33\t0\r\n2\t227\t1\t32\t0\r\n2\t228\t1\t32\t0\r\n2\t229\t0\t32\t0\r\n2\t230\t0\t32\t0\r\n2\t231\t0\t33\t0\r\n2\t232\t0\t33\t0\r\n2\t233\t1\t33\t0\r\n2\t234\t3\t33\t0\r\n2\t235\t3\t33\t0\r\n2\t236\t4\t33\t0\r\n2\t237\t4\t33\t0\r\n2\t238\t5\t32\t0\r\n2\t239\t5\t33\t0\r\n3\t0\t8\t28\t0\r\n3\t1\t7\t28\t0\r\n3\t2\t5\t29\t0\r\n3\t3\t4\t30\t0\r\n3\t4\t1\t32\t0\r\n3\t5\t0\t33\t0\r\n3\t6\t1\t32\t0\r\n3\t7\t1\t31\t0\r\n3\t8\t1\t32\t0\r\n3\t9\t1\t32\t0\r\n3\t10\t0\t32\t0\r\n3\t11\t2\t31\t0\r\n3\t12\t2\t31\t0\r\n3\t13\t1\t32\t0\r\n3\t14\t1\t32\t0\r\n3\t15\t1\t32\t0\r\n3\t16\t1\t32\t0\r\n3\t17\t1\t32\t0\r\n3\t18\t1\t32\t0\r\n3\t19\t0\t33\t0\r\n3\t20\t0\t33\t0\r\n3\t21\t1\t32\t0\r\n3\t22\t1\t32\t0\r\n3\t23\t2\t31\t0\r\n3\t24\t1\t32\t0\r\n3\t25\t1\t32\t0\r\n3\t26\t1\t32\t0\r\n3\t27\t1\t32\t0\r\n3\t28\t1\t32\t0\r\n3\t29\t1\t32\t0\r\n3\t30\t1\t32\t0\r\n3\t31\t1\t32\t0\r\n3\t32\t1\t33\t0\r\n3\t33\t1\t32\t0\r\n3\t34\t0\t34\t0\r\n3\t35\t1\t33\t0\r\n3\t36\t0\t32\t0\r\n3\t37\t1\t32\t0\r\n3\t38\t1\t32\t0\r\n3\t39\t0\t32\t0\r\n3\t40\t0\t33\t0\r\n3\t41\t1\t32\t0\r\n3\t42\t0\t32\t0\r\n3\t43\t0\t32\t0\r\n3\t44\t1\t32\t0\r\n3\t45\t0\t32\t0\r\n3\t46\t0\t32\t0\r\n3\t47\t1\t32\t0\r\n3\t48\t2\t32\t0\r\n3\t49\t0\t32\t0\r\n3\t50\t0\t32\t0\r\n3\t51\t0\t33\t0\r\n3\t52\t1\t32\t0\r\n3\t53\t1\t33\t0\r\n3\t54\t1\t32\t0\r\n3\t55\t0\t32\t0\r\n3\t56\t1\t33\t0\r\n3\t57\t0\t32\t0\r\n3\t58\t0\t33\t0\r\n3\t59\t2\t32\t0\r\n3\t60\t0\t32\t0\r\n3\t61\t2\t31\t0\r\n3\t62\t1\t32\t0\r\n3\t63\t1\t32\t0\r\n3\t64\t1\t32\t0\r\n3\t65\t1\t32\t0\r\n3\t66\t1\t32\t0\r\n3\t67\t0\t33\t0\r\n3\t68\t0\t33\t0\r\n3\t69\t1\t32\t0\r\n3\t70\t1\t32\t0\r\n3\t71\t1\t32\t0\r\n3\t72\t1\t33\t0\r\n3\t73\t0\t33\t0\r\n3\t74\t1\t32\t0\r\n3\t75\t1\t32\t0\r\n3\t76\t1\t32\t0\r\n3\t77\t2\t31\t0\r\n3\t78\t2\t31\t0\r\n3\t79\t1\t32\t0\r\n3\t80\t1\t32\t0\r\n3\t81\t0\t33\t0\r\n3\t82\t0\t33\t0\r\n3\t83\t0\t33\t0\r\n3\t84\t0\t33\t0\r\n3\t85\t1\t32\t0\r\n3\t86\t1\t32\t0\r\n3\t87\t0\t32\t0\r\n3\t88\t1\t32\t0\r\n3\t89\t1\t32\t0\r\n3\t90\t1\t32\t0\r\n3\t91\t1\t32\t0\r\n3\t92\t1\t32\t0\r\n3\t93\t1\t32\t0\r\n3\t94\t1\t32\t0\r\n3\t95\t0\t33\t0\r\n3\t96\t1\t32\t0\r\n3\t97\t0\t33\t0\r\n3\t98\t1\t32\t0\r\n3\t99\t1\t32\t0\r\n3\t100\t0\t33\t0\r\n3\t101\t0\t33\t0\r\n3\t102\t1\t32\t0\r\n3\t103\t0\t32\t0\r\n3\t104\t1\t32\t0\r\n3\t105\t1\t32\t0\r\n3\t106\t1\t32\t0\r\n3\t107\t1\t32\t0\r\n3\t108\t2\t32\t0\r\n3\t109\t1\t32\t0\r\n3\t110\t1\t32\t0\r\n3\t111\t1\t33\t0\r\n3\t112\t0\t33\t0\r\n3\t113\t1\t32\t0\r\n3\t114\t1\t32\t0\r\n3\t115\t2\t31\t0\r\n3\t116\t1\t32\t0\r\n3\t117\t1\t32\t0\r\n3\t118\t1\t31\t0\r\n3\t119\t1\t32\t0\r\n3\t120\t0\t33\t0\r\n3\t121\t1\t32\t0\r\n3\t122\t1\t32\t0\r\n3\t123\t1\t32\t0\r\n3\t124\t1\t32\t0\r\n3\t125\t0\t32\t0\r\n3\t126\t0\t32\t0\r\n3\t127\t1\t32\t0\r\n3\t128\t1\t32\t0\r\n3\t129\t0\t33\t0\r\n3\t130\t1\t33\t0\r\n3\t131\t1\t32\t0\r\n3\t132\t1\t32\t0\r\n3\t133\t0\t33\t0\r\n3\t134\t1\t32\t0\r\n3\t135\t0\t32\t0\r\n3\t136\t0\t33\t0\r\n3\t137\t0\t33\t0\r\n3\t138\t0\t32\t0\r\n3\t139\t1\t33\t0\r\n3\t140\t0\t32\t0\r\n3\t141\t1\t32\t0\r\n3\t142\t1\t32\t0\r\n3\t143\t0\t32\t0\r\n3\t144\t1\t32\t0\r\n3\t145\t0\t32\t0\r\n3\t146\t1\t31\t0\r\n3\t147\t0\t32\t0\r\n3\t148\t0\t33\t0\r\n3\t149\t0\t32\t0\r\n3\t150\t1\t32\t0\r\n3\t151\t0\t33\t0\r\n3\t152\t1\t32\t0\r\n3\t153\t0\t34\t0\r\n3\t154\t1\t33\t0\r\n3\t155\t1\t32\t0\r\n3\t156\t0\t33\t0\r\n3\t157\t0\t32\t0\r\n3\t158\t0\t32\t0\r\n3\t159\t2\t31\t0\r\n3\t160\t0\t33\t0\r\n3\t161\t0\t33\t0\r\n3\t162\t1\t32\t0\r\n3\t163\t0\t33\t0\r\n3\t164\t0\t33\t0\r\n3\t165\t1\t32\t0\r\n3\t166\t1\t32\t0\r\n3\t167\t1\t31\t0\r\n3\t168\t1\t32\t0\r\n3\t169\t0\t33\t0\r\n3\t170\t1\t32\t0\r\n3\t171\t0\t33\t0\r\n3\t172\t1\t32\t0\r\n3\t173\t0\t32\t0\r\n3\t174\t0\t32\t0\r\n3\t175\t1\t32\t0\r\n3\t176\t0\t33\t0\r\n3\t177\t0\t33\t0\r\n3\t178\t0\t33\t0\r\n3\t179\t1\t32\t0\r\n3\t180\t1\t32\t0\r\n3\t181\t1\t32\t0\r\n3\t182\t0\t32\t0\r\n3\t183\t1\t32\t0\r\n3\t184\t1\t32\t0\r\n3\t185\t1\t32\t0\r\n3\t186\t0\t33\t0\r\n3\t187\t1\t32\t0\r\n3\t188\t1\t33\t0\r\n3\t189\t1\t33\t0\r\n3\t190\t0\t32\t0\r\n3\t191\t0\t33\t0\r\n3\t192\t0\t33\t0\r\n3\t193\t0\t33\t0\r\n3\t194\t0\t33\t0\r\n3\t195\t0\t32\t0\r\n3\t196\t0\t32\t0\r\n3\t197\t1\t32\t0\r\n3\t198\t1\t32\t0\r\n3\t199\t0\t32\t0\r\n3\t200\t0\t32\t0\r\n3\t201\t1\t32\t0\r\n3\t202\t0\t33\t0\r\n3\t203\t1\t32\t0\r\n3\t204\t1\t32\t0\r\n3\t205\t0\t33\t0\r\n3\t206\t0\t32\t0\r\n3\t207\t1\t32\t0\r\n3\t208\t1\t32\t0\r\n3\t209\t0\t32\t0\r\n3\t210\t1\t32\t0\r\n3\t211\t0\t33\t0\r\n3\t212\t0\t32\t0\r\n3\t213\t0\t32\t0\r\n3\t214\t1\t31\t0\r\n3\t215\t1\t31\t0\r\n3\t216\t2\t32\t0\r\n3\t217\t0\t32\t0\r\n3\t218\t0\t33\t0\r\n3\t219\t1\t32\t0\r\n3\t220\t2\t32\t0\r\n3\t221\t1\t33\t0\r\n3\t222\t1\t32\t0\r\n3\t223\t1\t31\t0\r\n3\t224\t1\t32\t0\r\n3\t225\t0\t33\t0\r\n3\t226\t1\t32\t0\r\n3\t227\t0\t32\t0\r\n3\t228\t1\t32\t0\r\n3\t229\t0\t32\t0\r\n3\t230\t1\t32\t0\r\n3\t231\t0\t32\t0\r\n3\t232\t1\t32\t0\r\n3\t233\t2\t32\t0\r\n3\t234\t3\t33\t0\r\n3\t235\t5\t32\t0\r\n3\t236\t4\t33\t0\r\n3\t237\t4\t33\t0\r\n3\t238\t5\t32\t0\r\n3\t239\t5\t32\t0\r\n4\t0\t8\t28\t0\r\n4\t1\t8\t28\t0\r\n4\t2\t5\t29\t0\r\n4\t3\t4\t30\t0\r\n4\t4\t0\t32\t0\r\n4\t5\t0\t32\t0\r\n4\t6\t0\t32\t0\r\n4\t7\t0\t32\t0\r\n4\t8\t0\t32\t0\r\n4\t9\t0\t31\t0\r\n4\t10\t0\t32\t0\r\n4\t11\t0\t33\t0\r\n4\t12\t1\t32\t0\r\n4\t13\t0\t32\t0\r\n4\t14\t1\t31\t0\r\n4\t15\t1\t32\t0\r\n4\t16\t0\t32\t0\r\n4\t17\t1\t32\t0\r\n4\t18\t0\t32\t0\r\n4\t19\t1\t32\t0\r\n4\t20\t1\t32\t0\r\n4\t21\t0\t33\t0\r\n4\t22\t1\t32\t0\r\n4\t23\t0\t33\t0\r\n4\t24\t0\t33\t0\r\n4\t25\t0\t33\t0\r\n4\t26\t0\t32\t0\r\n4\t27\t0\t32\t0\r\n4\t28\t0\t32\t0\r\n4\t29\t0\t32\t0\r\n4\t30\t0\t33\t0\r\n4\t31\t0\t32\t0\r\n4\t32\t0\t33\t0\r\n4\t33\t0\t33\t0\r\n4\t34\t0\t32\t0\r\n4\t35\t0\t33\t0\r\n4\t36\t0\t32\t0\r\n4\t37\t1\t32\t0\r\n4\t38\t0\t32\t0\r\n4\t39\t1\t32\t0\r\n4\t40\t0\t33\t0\r\n4\t41\t0\t34\t0\r\n4\t42\t0\t33\t0\r\n4\t43\t0\t32\t0\r\n4\t44\t0\t32\t0\r\n4\t45\t1\t32\t0\r\n4\t46\t0\t32\t0\r\n4\t47\t0\t32\t0\r\n4\t48\t1\t32\t0\r\n4\t49\t0\t32\t0\r\n4\t50\t0\t32\t0\r\n4\t51\t0\t32\t0\r\n4\t52\t0\t32\t0\r\n4\t53\t0\t32\t0\r\n4\t54\t0\t32\t0\r\n4\t55\t0\t31\t0\r\n4\t56\t0\t34\t0\r\n4\t57\t0\t32\t0\r\n4\t58\t0\t32\t0\r\n4\t59\t0\t31\t0\r\n4\t60\t0\t32\t0\r\n4\t61\t0\t31\t0\r\n4\t62\t1\t31\t0\r\n4\t63\t0\t32\t0\r\n4\t64\t0\t32\t0\r\n4\t65\t0\t32\t0\r\n4\t66\t0\t32\t0\r\n4\t67\t0\t32\t0\r\n4\t68\t0\t33\t0\r\n4\t69\t0\t33\t0\r\n4\t70\t0\t33\t0\r\n4\t71\t0\t32\t0\r\n4\t72\t0\t32\t0\r\n4\t73\t0\t33\t0\r\n4\t74\t0\t32\t0\r\n4\t75\t0\t31\t0\r\n4\t76\t0\t33\t0\r\n4\t77\t0\t32\t0\r\n4\t78\t0\t32\t0\r\n4\t79\t0\t31\t0\r\n4\t80\t0\t32\t0\r\n4\t81\t0\t33\t0\r\n4\t82\t0\t32\t0\r\n4\t83\t0\t31\t0\r\n4\t84\t0\t32\t0\r\n4\t85\t0\t33\t0\r\n4\t86\t1\t31\t0\r\n4\t87\t0\t32\t0\r\n4\t88\t0\t32\t0\r\n4\t89\t0\t33\t0\r\n4\t90\t0\t32\t0\r\n4\t91\t0\t33\t0\r\n4\t92\t0\t32\t0\r\n4\t93\t0\t32\t0\r\n4\t94\t0\t32\t0\r\n4\t95\t0\t32\t0\r\n4\t96\t0\t33\t0\r\n4\t97\t0\t33\t0\r\n4\t98\t0\t33\t0\r\n4\t99\t0\t32\t0\r\n4\t100\t0\t32\t0\r\n4\t101\t0\t32\t0\r\n4\t102\t1\t31\t0\r\n4\t103\t0\t32\t0\r\n4\t104\t0\t32\t0\r\n4\t105\t0\t33\t0\r\n4\t106\t0\t33\t0\r\n4\t107\t0\t32\t0\r\n4\t108\t1\t32\t0\r\n4\t109\t0\t33\t0\r\n4\t110\t0\t32\t0\r\n4\t111\t0\t32\t0\r\n4\t112\t0\t32\t0\r\n4\t113\t0\t33\t0\r\n4\t114\t0\t32\t0\r\n4\t115\t0\t33\t0\r\n4\t116\t0\t32\t0\r\n4\t117\t1\t32\t0\r\n4\t118\t0\t31\t0\r\n4\t119\t0\t32\t0\r\n4\t120\t0\t33\t0\r\n4\t121\t0\t32\t0\r\n4\t122\t0\t33\t0\r\n4\t123\t0\t32\t0\r\n4\t124\t0\t32\t0\r\n4\t125\t0\t32\t0\r\n4\t126\t0\t32\t0\r\n4\t127\t1\t32\t0\r\n4\t128\t1\t32\t0\r\n4\t129\t0\t34\t0\r\n4\t130\t0\t32\t0\r\n4\t131\t0\t32\t0\r\n4\t132\t0\t32\t0\r\n4\t133\t0\t33\t0\r\n4\t134\t0\t32\t0\r\n4\t135\t0\t32\t0\r\n4\t136\t0\t32\t0\r\n4\t137\t0\t33\t0\r\n4\t138\t1\t31\t0\r\n4\t139\t0\t32\t0\r\n4\t140\t0\t32\t0\r\n4\t141\t0\t32\t0\r\n4\t142\t0\t32\t0\r\n4\t143\t1\t31\t0\r\n4\t144\t1\t32\t0\r\n4\t145\t0\t32\t0\r\n4\t146\t0\t32\t0\r\n4\t147\t0\t32\t0\r\n4\t148\t0\t32\t0\r\n4\t149\t0\t33\t0\r\n4\t150\t0\t33\t0\r\n4\t151\t0\t32\t0\r\n4\t152\t0\t32\t0\r\n4\t153\t0\t33\t0\r\n4\t154\t0\t32\t0\r\n4\t155\t1\t32\t0\r\n4\t156\t0\t33\t0\r\n4\t157\t0\t32\t0\r\n4\t158\t0\t32\t0\r\n4\t159\t0\t32\t0\r\n4\t160\t0\t32\t0\r\n4\t161\t0\t32\t0\r\n4\t162\t0\t32\t0\r\n4\t163\t0\t32\t0\r\n4\t164\t0\t32\t0\r\n4\t165\t1\t31\t0\r\n4\t166\t1\t31\t0\r\n4\t167\t0\t32\t0\r\n4\t168\t0\t32\t0\r\n4\t169\t0\t32\t0\r\n4\t170\t0\t31\t0\r\n4\t171\t0\t32\t0\r\n4\t172\t0\t32\t0\r\n4\t173\t0\t32\t0\r\n4\t174\t0\t32\t0\r\n4\t175\t0\t32\t0\r\n4\t176\t0\t32\t0\r\n4\t177\t0\t32\t0\r\n4\t178\t0\t32\t0\r\n4\t179\t0\t31\t0\r\n4\t180\t0\t33\t0\r\n4\t181\t0\t32\t0\r\n4\t182\t0\t32\t0\r\n4\t183\t0\t32\t0\r\n4\t184\t0\t33\t0\r\n4\t185\t0\t33\t0\r\n4\t186\t0\t32\t0\r\n4\t187\t0\t32\t0\r\n4\t188\t0\t33\t0\r\n4\t189\t1\t32\t0\r\n4\t190\t0\t32\t0\r\n4\t191\t0\t33\t0\r\n4\t192\t0\t33\t0\r\n4\t193\t0\t32\t0\r\n4\t194\t0\t32\t0\r\n4\t195\t0\t32\t0\r\n4\t196\t0\t32\t0\r\n4\t197\t0\t32\t0\r\n4\t198\t1\t31\t0\r\n4\t199\t0\t32\t0\r\n4\t200\t0\t32\t0\r\n4\t201\t0\t32\t0\r\n4\t202\t0\t32\t0\r\n4\t203\t0\t32\t0\r\n4\t204\t0\t32\t0\r\n4\t205\t0\t32\t0\r\n4\t206\t0\t32\t0\r\n4\t207\t1\t32\t0\r\n4\t208\t1\t31\t0\r\n4\t209\t0\t32\t0\r\n4\t210\t1\t32\t0\r\n4\t211\t0\t33\t0\r\n4\t212\t1\t32\t0\r\n4\t213\t0\t32\t0\r\n4\t214\t2\t31\t0\r\n4\t215\t1\t32\t0\r\n4\t216\t0\t33\t0\r\n4\t217\t0\t33\t0\r\n4\t218\t0\t32\t0\r\n4\t219\t0\t32\t0\r\n4\t220\t0\t33\t0\r\n4\t221\t0\t33\t0\r\n4\t222\t0\t33\t0\r\n4\t223\t0\t31\t0\r\n4\t224\t0\t33\t0\r\n4\t225\t0\t32\t0\r\n4\t226\t0\t31\t0\r\n4\t227\t0\t33\t0\r\n4\t228\t0\t34\t0\r\n4\t229\t1\t31\t0\r\n4\t230\t0\t31\t0\r\n4\t231\t0\t32\t0\r\n4\t232\t1\t32\t0\r\n4\t233\t1\t33\t0\r\n4\t234\t2\t33\t0\r\n4\t235\t3\t33\t0\r\n4\t236\t3\t33\t0\r\n4\t237\t4\t33\t0\r\n4\t238\t3\t33\t0\r\n4\t239\t3\t33\t0\r\n5\t0\t8\t28\t0\r\n5\t1\t8\t28\t0\r\n5\t2\t4\t30\t0\r\n5\t3\t5\t29\t0\r\n5\t4\t1\t32\t0\r\n5\t5\t0\t33\t0\r\n5\t6\t0\t33\t0\r\n5\t7\t0\t32\t0\r\n5\t8\t0\t32\t0\r\n5\t9\t0\t31\t0\r\n5\t10\t0\t31\t0\r\n5\t11\t0\t32\t0\r\n5\t12\t0\t32\t0\r\n5\t13\t0\t32\t0\r\n5\t14\t0\t31\t0\r\n5\t15\t0\t31\t0\r\n5\t16\t0\t32\t0\r\n5\t17\t0\t32\t0\r\n5\t18\t0\t31\t0\r\n5\t19\t0\t31\t0\r\n5\t20\t0\t31\t0\r\n5\t21\t0\t32\t0\r\n5\t22\t0\t32\t0\r\n5\t23\t0\t32\t0\r\n5\t24\t0\t32\t0\r\n5\t25\t0\t32\t0\r\n5\t26\t0\t32\t0\r\n5\t27\t0\t32\t0\r\n5\t28\t0\t32\t0\r\n5\t29\t0\t32\t0\r\n5\t30\t0\t31\t0\r\n5\t31\t0\t31\t0\r\n5\t32\t0\t32\t0\r\n5\t33\t0\t32\t0\r\n5\t34\t0\t33\t0\r\n5\t35\t0\t31\t0\r\n5\t36\t0\t31\t0\r\n5\t37\t0\t31\t0\r\n5\t38\t0\t31\t0\r\n5\t39\t0\t31\t0\r\n5\t40\t0\t32\t0\r\n5\t41\t0\t33\t0\r\n5\t42\t0\t32\t0\r\n5\t43\t0\t31\t0\r\n5\t44\t0\t32\t0\r\n5\t45\t0\t32\t0\r\n5\t46\t0\t31\t0\r\n5\t47\t0\t31\t0\r\n5\t48\t0\t32\t0\r\n5\t49\t0\t31\t0\r\n5\t50\t0\t31\t0\r\n5\t51\t0\t31\t0\r\n5\t52\t0\t34\t0\r\n5\t53\t0\t33\t0\r\n5\t54\t0\t31\t0\r\n5\t55\t0\t31\t0\r\n5\t56\t0\t32\t0\r\n5\t57\t0\t31\t0\r\n5\t58\t0\t31\t0\r\n5\t59\t0\t30\t0\r\n5\t60\t0\t31\t0\r\n5\t61\t0\t31\t0\r\n5\t62\t0\t31\t0\r\n5\t63\t0\t30\t0\r\n5\t64\t0\t32\t0\r\n5\t65\t0\t32\t0\r\n5\t66\t0\t31\t0\r\n5\t67\t0\t31\t0\r\n5\t68\t0\t32\t0\r\n5\t69\t0\t31\t0\r\n5\t70\t0\t31\t0\r\n5\t71\t0\t31\t0\r\n5\t72\t0\t31\t0\r\n5\t73\t0\t31\t0\r\n5\t74\t0\t31\t0\r\n5\t75\t0\t31\t0\r\n5\t76\t0\t32\t0\r\n5\t77\t0\t33\t0\r\n5\t78\t0\t31\t0\r\n5\t79\t0\t30\t0\r\n5\t80\t0\t33\t0\r\n5\t81\t0\t32\t0\r\n5\t82\t0\t32\t0\r\n5\t83\t0\t31\t0\r\n5\t84\t0\t32\t0\r\n5\t85\t0\t32\t0\r\n5\t86\t0\t31\t0\r\n5\t87\t0\t32\t0\r\n5\t88\t0\t32\t0\r\n5\t89\t0\t33\t0\r\n5\t90\t0\t31\t0\r\n5\t91\t0\t32\t0\r\n5\t92\t0\t31\t0\r\n5\t93\t0\t32\t0\r\n5\t94\t0\t31\t0\r\n5\t95\t0\t31\t0\r\n5\t96\t0\t32\t0\r\n5\t97\t0\t32\t0\r\n5\t98\t0\t32\t0\r\n5\t99\t0\t31\t0\r\n5\t100\t0\t31\t0\r\n5\t101\t0\t31\t0\r\n5\t102\t0\t31\t0\r\n5\t103\t0\t31\t0\r\n5\t104\t0\t32\t0\r\n5\t105\t0\t32\t0\r\n5\t106\t0\t32\t0\r\n5\t107\t0\t31\t0\r\n5\t108\t0\t32\t0\r\n5\t109\t0\t32\t0\r\n5\t110\t0\t31\t0\r\n5\t111\t0\t31\t0\r\n5\t112\t0\t32\t0\r\n5\t113\t0\t32\t0\r\n5\t114\t0\t31\t0\r\n5\t115\t0\t31\t0\r\n5\t116\t0\t32\t0\r\n5\t117\t0\t32\t0\r\n5\t118\t0\t32\t0\r\n5\t119\t0\t31\t0\r\n5\t120\t0\t32\t0\r\n5\t121\t0\t32\t0\r\n5\t122\t0\t32\t0\r\n5\t123\t0\t31\t0\r\n5\t124\t0\t31\t0\r\n5\t125\t0\t32\t0\r\n5\t126\t0\t31\t0\r\n5\t127\t1\t30\t0\r\n5\t128\t0\t31\t0\r\n5\t129\t0\t33\t0\r\n5\t130\t0\t32\t0\r\n5\t131\t0\t31\t0\r\n5\t132\t0\t32\t0\r\n5\t133\t0\t32\t0\r\n5\t134\t0\t32\t0\r\n5\t135\t0\t31\t0\r\n5\t136\t0\t31\t0\r\n5\t137\t0\t32\t0\r\n5\t138\t0\t31\t0\r\n5\t139\t0\t32\t0\r\n5\t140\t0\t33\t0\r\n5\t141\t0\t32\t0\r\n5\t142\t0\t31\t0\r\n5\t143\t0\t31\t0\r\n5\t144\t0\t31\t0\r\n5\t145\t0\t32\t0\r\n5\t146\t0\t32\t0\r\n5\t147\t0\t31\t0\r\n5\t148\t0\t31\t0\r\n5\t149\t0\t30\t0\r\n5\t150\t0\t31\t0\r\n5\t151\t0\t31\t0\r\n5\t152\t0\t33\t0\r\n5\t153\t0\t31\t0\r\n5\t154\t0\t31\t0\r\n5\t155\t1\t30\t0\r\n5\t156\t0\t31\t0\r\n5\t157\t0\t32\t0\r\n5\t158\t0\t32\t0\r\n5\t159\t0\t31\t0\r\n5\t160\t0\t31\t0\r\n5\t161\t0\t32\t0\r\n5\t162\t0\t32\t0\r\n5\t163\t0\t31\t0\r\n5\t164\t0\t31\t0\r\n5\t165\t0\t32\t0\r\n5\t166\t0\t31\t0\r\n5\t167\t0\t32\t0\r\n5\t168\t0\t32\t0\r\n5\t169\t0\t32\t0\r\n5\t170\t0\t31\t0\r\n5\t171\t0\t31\t0\r\n5\t172\t0\t31\t0\r\n5\t173\t0\t33\t0\r\n5\t174\t0\t32\t0\r\n5\t175\t0\t31\t0\r\n5\t176\t0\t31\t0\r\n5\t177\t0\t31\t0\r\n5\t178\t0\t31\t0\r\n5\t179\t0\t32\t0\r\n5\t180\t0\t32\t0\r\n5\t181\t0\t32\t0\r\n5\t182\t0\t32\t0\r\n5\t183\t0\t31\t0\r\n5\t184\t0\t32\t0\r\n5\t185\t0\t32\t0\r\n5\t186\t0\t31\t0\r\n5\t187\t0\t31\t0\r\n5\t188\t0\t33\t0\r\n5\t189\t0\t32\t0\r\n5\t190\t0\t31\t0\r\n5\t191\t0\t31\t0\r\n5\t192\t0\t32\t0\r\n5\t193\t0\t31\t0\r\n5\t194\t0\t31\t0\r\n5\t195\t0\t33\t0\r\n5\t196\t0\t31\t0\r\n5\t197\t0\t32\t0\r\n5\t198\t0\t31\t0\r\n5\t199\t0\t31\t0\r\n5\t200\t0\t31\t0\r\n5\t201\t0\t31\t0\r\n5\t202\t0\t30\t0\r\n5\t203\t0\t30\t0\r\n5\t204\t0\t31\t0\r\n5\t205\t0\t31\t0\r\n5\t206\t0\t31\t0\r\n5\t207\t0\t31\t0\r\n5\t208\t0\t31\t0\r\n5\t209\t0\t32\t0\r\n5\t210\t0\t31\t0\r\n5\t211\t0\t31\t0\r\n5\t212\t0\t31\t0\r\n5\t213\t0\t32\t0\r\n5\t214\t0\t30\t0\r\n5\t215\t0\t31\t0\r\n5\t216\t0\t32\t0\r\n5\t217\t0\t32\t0\r\n5\t218\t0\t32\t0\r\n5\t219\t0\t31\t0\r\n5\t220\t0\t31\t0\r\n5\t221\t0\t31\t0\r\n5\t222\t0\t31\t0\r\n5\t223\t0\t31\t0\r\n5\t224\t0\t32\t0\r\n5\t225\t0\t31\t0\r\n5\t226\t0\t31\t0\r\n5\t227\t0\t31\t0\r\n5\t228\t0\t31\t0\r\n5\t229\t0\t30\t0\r\n5\t230\t0\t30\t0\r\n5\t231\t0\t31\t0\r\n5\t232\t0\t32\t0\r\n5\t233\t0\t32\t0\r\n5\t234\t0\t33\t0\r\n5\t235\t1\t33\t0\r\n5\t236\t1\t33\t0\r\n5\t237\t1\t34\t0\r\n5\t238\t1\t34\t0\r\n5\t239\t1\t33\t0\r\n6\t0\t7\t28\t0\r\n6\t1\t8\t28\t0\r\n6\t2\t4\t30\t0\r\n6\t3\t3\t31\t0\r\n6\t4\t0\t33\t0\r\n6\t5\t0\t33\t0\r\n6\t6\t0\t32\t0\r\n6\t7\t0\t32\t0\r\n6\t8\t0\t32\t0\r\n6\t9\t0\t31\t0\r\n6\t10\t0\t31\t0\r\n6\t11\t0\t32\t0\r\n6\t12\t0\t32\t0\r\n6\t13\t0\t32\t0\r\n6\t14\t0\t31\t0\r\n6\t15\t0\t31\t0\r\n6\t16\t0\t32\t0\r\n6\t17\t0\t33\t0\r\n6\t18\t0\t33\t0\r\n6\t19\t0\t32\t0\r\n6\t20\t0\t32\t0\r\n6\t21\t0\t32\t0\r\n6\t22\t0\t32\t0\r\n6\t23\t0\t33\t0\r\n6\t24\t0\t32\t0\r\n6\t25\t0\t31\t0\r\n6\t26\t0\t32\t0\r\n6\t27\t0\t32\t0\r\n6\t28\t0\t33\t0\r\n6\t29\t0\t32\t0\r\n6\t30\t0\t32\t0\r\n6\t31\t0\t32\t0\r\n6\t32\t0\t32\t0\r\n6\t33\t0\t32\t0\r\n6\t34\t0\t32\t0\r\n6\t35\t0\t32\t0\r\n6\t36\t0\t32\t0\r\n6\t37\t0\t32\t0\r\n6\t38\t0\t31\t0\r\n6[...string is too long...]".Split(new char[]
            {
                '\n'
            });
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    string[] limit = Regex.Split(line.Trim(), "\\s+");
                    short x = short.Parse(limit[0]);
                    short y = short.Parse(limit[1]);
                    short min = short.Parse(limit[2]);
                    short max = short.Parse(limit[3]);
                    result[(int)x, (int)y, 0] = min;
                    result[(int)x, (int)y, 1] = max;
                }
            }
            return result;
        }

        /// <summary>
        /// Loads values from the mat file.
        /// </summary>
        /// <param name="file"></param>
        // Token: 0x0600000A RID: 10 RVA: 0x00004AE4 File Offset: 0x00002CE4
        public static object LoadMatFile(string file)
        {
            return Maths.CalibrationAnalysis.loadCData(1, file);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00004B04 File Offset: 0x00002D04
        static Maths()
        {
            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            Maths.BaseCalibration = Path.Combine(Path.GetDirectoryName(assemblyPath) ?? "", "CData.mat");
            Maths.CalibrationAnalysis = new CalibrationAnalysis();
            Maths.FormCalc = new formCalc();
            new Image<Bgr, byte>(new byte[10, 10, 3]);
        }

        /// <summary>
        ///
        /// </summary>
        // Token: 0x0600000C RID: 12 RVA: 0x00004B88 File Offset: 0x00002D88
        public static void ForceLoad()
        {
        }

        /// <summary>
        /// Find greatest common divisor, usage (IEnumberable&lt;int&gt;)numbers.Aggregate(GCD);
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        // Token: 0x0600000D RID: 13 RVA: 0x00004B8C File Offset: 0x00002D8C
        public static int GCD(int a, int b)
        {
            return (b == 0) ? a : Maths.GCD(b, a % b);
        }

        // Token: 0x04000001 RID: 1
        private const string LIMIT_STR = "\r\n0\t0\t8\t28\t0\r\n0\t1\t9\t27\t0\r\n0\t2\t5\t29\t0\r\n0\t3\t4\t30\t0\r\n0\t4\t1\t33\t0\r\n0\t5\t0\t32\t0\r\n0\t6\t1\t32\t0\r\n0\t7\t0\t32\t0\r\n0\t8\t0\t33\t0\r\n0\t9\t0\t33\t0\r\n0\t10\t0\t33\t0\r\n0\t11\t0\t33\t0\r\n0\t12\t0\t33\t0\r\n0\t13\t0\t33\t0\r\n0\t14\t0\t33\t0\r\n0\t15\t1\t33\t0\r\n0\t16\t1\t33\t0\r\n0\t17\t1\t33\t0\r\n0\t18\t0\t33\t0\r\n0\t19\t1\t33\t0\r\n0\t20\t1\t33\t0\r\n0\t21\t0\t33\t0\r\n0\t22\t1\t33\t0\r\n0\t23\t1\t33\t0\r\n0\t24\t1\t33\t0\r\n0\t25\t1\t33\t0\r\n0\t26\t0\t33\t0\r\n0\t27\t1\t33\t0\r\n0\t28\t2\t32\t0\r\n0\t29\t0\t33\t0\r\n0\t30\t1\t32\t0\r\n0\t31\t1\t32\t0\r\n0\t32\t1\t33\t0\r\n0\t33\t0\t34\t0\r\n0\t34\t1\t33\t0\r\n0\t35\t1\t32\t0\r\n0\t36\t1\t33\t0\r\n0\t37\t1\t33\t0\r\n0\t38\t1\t33\t0\r\n0\t39\t1\t33\t0\r\n0\t40\t1\t33\t0\r\n0\t41\t1\t33\t0\r\n0\t42\t1\t32\t0\r\n0\t43\t1\t32\t0\r\n0\t44\t0\t33\t0\r\n0\t45\t0\t33\t0\r\n0\t46\t1\t32\t0\r\n0\t47\t1\t32\t0\r\n0\t48\t1\t32\t0\r\n0\t49\t0\t34\t0\r\n0\t50\t0\t33\t0\r\n0\t51\t1\t32\t0\r\n0\t52\t1\t32\t0\r\n0\t53\t0\t33\t0\r\n0\t54\t1\t32\t0\r\n0\t55\t1\t32\t0\r\n0\t56\t1\t32\t0\r\n0\t57\t0\t33\t0\r\n0\t58\t0\t33\t0\r\n0\t59\t1\t32\t0\r\n0\t60\t0\t34\t0\r\n0\t61\t0\t33\t0\r\n0\t62\t1\t33\t0\r\n0\t63\t0\t33\t0\r\n0\t64\t0\t34\t0\r\n0\t65\t0\t33\t0\r\n0\t66\t1\t33\t0\r\n0\t67\t1\t32\t0\r\n0\t68\t0\t34\t0\r\n0\t69\t1\t33\t0\r\n0\t70\t1\t32\t0\r\n0\t71\t0\t32\t0\r\n0\t72\t1\t33\t0\r\n0\t73\t0\t34\t0\r\n0\t74\t0\t33\t0\r\n0\t75\t1\t32\t0\r\n0\t76\t1\t33\t0\r\n0\t77\t1\t32\t0\r\n0\t78\t1\t32\t0\r\n0\t79\t0\t33\t0\r\n0\t80\t1\t33\t0\r\n0\t81\t0\t33\t0\r\n0\t82\t0\t33\t0\r\n0\t83\t1\t32\t0\r\n0\t84\t1\t33\t0\r\n0\t85\t0\t33\t0\r\n0\t86\t1\t32\t0\r\n0\t87\t0\t33\t0\r\n0\t88\t0\t33\t0\r\n0\t89\t0\t34\t0\r\n0\t90\t1\t33\t0\r\n0\t91\t1\t32\t0\r\n0\t92\t1\t33\t0\r\n0\t93\t0\t33\t0\r\n0\t94\t1\t32\t0\r\n0\t95\t0\t33\t0\r\n0\t96\t0\t34\t0\r\n0\t97\t0\t33\t0\r\n0\t98\t0\t33\t0\r\n0\t99\t1\t33\t0\r\n0\t100\t0\t33\t0\r\n0\t101\t1\t33\t0\r\n0\t102\t0\t34\t0\r\n0\t103\t1\t33\t0\r\n0\t104\t0\t33\t0\r\n0\t105\t0\t34\t0\r\n0\t106\t1\t32\t0\r\n0\t107\t0\t33\t0\r\n0\t108\t0\t33\t0\r\n0\t109\t0\t33\t0\r\n0\t110\t0\t34\t0\r\n0\t111\t1\t32\t0\r\n0\t112\t1\t32\t0\r\n0\t113\t1\t33\t0\r\n0\t114\t1\t32\t0\r\n0\t115\t1\t33\t0\r\n0\t116\t1\t33\t0\r\n0\t117\t0\t33\t0\r\n0\t118\t0\t33\t0\r\n0\t119\t0\t33\t0\r\n0\t120\t0\t33\t0\r\n0\t121\t0\t34\t0\r\n0\t122\t1\t33\t0\r\n0\t123\t0\t33\t0\r\n0\t124\t0\t33\t0\r\n0\t125\t1\t32\t0\r\n0\t126\t1\t33\t0\r\n0\t127\t1\t32\t0\r\n0\t128\t1\t32\t0\r\n0\t129\t0\t33\t0\r\n0\t130\t1\t33\t0\r\n0\t131\t0\t33\t0\r\n0\t132\t0\t33\t0\r\n0\t133\t1\t33\t0\r\n0\t134\t1\t33\t0\r\n0\t135\t1\t32\t0\r\n0\t136\t0\t33\t0\r\n0\t137\t0\t34\t0\r\n0\t138\t0\t33\t0\r\n0\t139\t1\t32\t0\r\n0\t140\t1\t33\t0\r\n0\t141\t0\t33\t0\r\n0\t142\t0\t33\t0\r\n0\t143\t0\t33\t0\r\n0\t144\t1\t33\t0\r\n0\t145\t0\t33\t0\r\n0\t146\t0\t33\t0\r\n0\t147\t1\t33\t0\r\n0\t148\t1\t33\t0\r\n0\t149\t0\t34\t0\r\n0\t150\t0\t33\t0\r\n0\t151\t0\t33\t0\r\n0\t152\t1\t32\t0\r\n0\t153\t0\t33\t0\r\n0\t154\t1\t32\t0\r\n0\t155\t0\t33\t0\r\n0\t156\t0\t33\t0\r\n0\t157\t1\t33\t0\r\n0\t158\t1\t33\t0\r\n0\t159\t0\t33\t0\r\n0\t160\t1\t33\t0\r\n0\t161\t1\t33\t0\r\n0\t162\t0\t33\t0\r\n0\t163\t1\t33\t0\r\n0\t164\t1\t33\t0\r\n0\t165\t0\t33\t0\r\n0\t166\t1\t32\t0\r\n0\t167\t1\t32\t0\r\n0\t168\t1\t33\t0\r\n0\t169\t1\t33\t0\r\n0\t170\t0\t33\t0\r\n0\t171\t0\t33\t0\r\n0\t172\t1\t32\t0\r\n0\t173\t0\t33\t0\r\n0\t174\t0\t32\t0\r\n0\t175\t0\t33\t0\r\n0\t176\t0\t33\t0\r\n0\t177\t0\t34\t0\r\n0\t178\t1\t33\t0\r\n0\t179\t0\t33\t0\r\n0\t180\t1\t33\t0\r\n0\t181\t1\t32\t0\r\n0\t182\t0\t33\t0\r\n0\t183\t0\t33\t0\r\n0\t184\t0\t33\t0\r\n0\t185\t0\t33\t0\r\n0\t186\t0\t33\t0\r\n0\t187\t0\t33\t0\r\n0\t188\t0\t34\t0\r\n0\t189\t0\t33\t0\r\n0\t190\t0\t33\t0\r\n0\t191\t1\t33\t0\r\n0\t192\t1\t33\t0\r\n0\t193\t0\t33\t0\r\n0\t194\t0\t33\t0\r\n0\t195\t0\t33\t0\r\n0\t196\t1\t32\t0\r\n0\t197\t1\t32\t0\r\n0\t198\t1\t33\t0\r\n0\t199\t1\t32\t0\r\n0\t200\t1\t33\t0\r\n0\t201\t0\t33\t0\r\n0\t202\t2\t31\t0\r\n0\t203\t1\t33\t0\r\n0\t204\t1\t33\t0\r\n0\t205\t1\t33\t0\r\n0\t206\t0\t33\t0\r\n0\t207\t0\t33\t0\r\n0\t208\t1\t33\t0\r\n0\t209\t0\t33\t0\r\n0\t210\t0\t33\t0\r\n0\t211\t1\t33\t0\r\n0\t212\t1\t33\t0\r\n0\t213\t1\t33\t0\r\n0\t214\t0\t33\t0\r\n0\t215\t1\t33\t0\r\n0\t216\t0\t33\t0\r\n0\t217\t1\t33\t0\r\n0\t218\t1\t33\t0\r\n0\t219\t1\t32\t0\r\n0\t220\t1\t33\t0\r\n0\t221\t1\t33\t0\r\n0\t222\t1\t33\t0\r\n0\t223\t2\t32\t0\r\n0\t224\t1\t32\t0\r\n0\t225\t0\t33\t0\r\n0\t226\t0\t33\t0\r\n0\t227\t1\t32\t0\r\n0\t228\t0\t36\t0\r\n0\t229\t1\t32\t0\r\n0\t230\t1\t32\t0\r\n0\t231\t1\t32\t0\r\n0\t232\t1\t32\t0\r\n0\t233\t1\t33\t0\r\n0\t234\t3\t33\t0\r\n0\t235\t4\t33\t0\r\n0\t236\t4\t33\t0\r\n0\t237\t5\t33\t0\r\n0\t238\t5\t32\t0\r\n0\t239\t5\t33\t0\r\n1\t0\t8\t28\t0\r\n1\t1\t8\t28\t0\r\n1\t2\t5\t29\t0\r\n1\t3\t4\t30\t0\r\n1\t4\t0\t33\t0\r\n1\t5\t0\t32\t0\r\n1\t6\t1\t32\t0\r\n1\t7\t0\t32\t0\r\n1\t8\t1\t32\t0\r\n1\t9\t0\t33\t0\r\n1\t10\t1\t32\t0\r\n1\t11\t1\t32\t0\r\n1\t12\t0\t32\t0\r\n1\t13\t1\t32\t0\r\n1\t14\t0\t32\t0\r\n1\t15\t0\t33\t0\r\n1\t16\t0\t33\t0\r\n1\t17\t0\t34\t0\r\n1\t18\t0\t32\t0\r\n1\t19\t1\t33\t0\r\n1\t20\t1\t33\t0\r\n1\t21\t1\t32\t0\r\n1\t22\t1\t32\t0\r\n1\t23\t0\t32\t0\r\n1\t24\t0\t33\t0\r\n1\t25\t0\t34\t0\r\n1\t26\t0\t33\t0\r\n1\t27\t1\t33\t0\r\n1\t28\t1\t32\t0\r\n1\t29\t1\t32\t0\r\n1\t30\t1\t32\t0\r\n1\t31\t1\t32\t0\r\n1\t32\t0\t33\t0\r\n1\t33\t0\t34\t0\r\n1\t34\t0\t33\t0\r\n1\t35\t1\t32\t0\r\n1\t36\t0\t33\t0\r\n1\t37\t0\t33\t0\r\n1\t38\t1\t32\t0\r\n1\t39\t0\t33\t0\r\n1\t40\t0\t33\t0\r\n1\t41\t1\t32\t0\r\n1\t42\t0\t33\t0\r\n1\t43\t0\t33\t0\r\n1\t44\t0\t33\t0\r\n1\t45\t0\t33\t0\r\n1\t46\t1\t32\t0\r\n1\t47\t0\t32\t0\r\n1\t48\t0\t33\t0\r\n1\t49\t0\t33\t0\r\n1\t50\t0\t34\t0\r\n1\t51\t1\t33\t0\r\n1\t52\t0\t33\t0\r\n1\t53\t0\t33\t0\r\n1\t54\t0\t32\t0\r\n1\t55\t1\t32\t0\r\n1\t56\t0\t33\t0\r\n1\t57\t0\t33\t0\r\n1\t58\t0\t33\t0\r\n1\t59\t1\t32\t0\r\n1\t60\t0\t32\t0\r\n1\t61\t1\t33\t0\r\n1\t62\t0\t33\t0\r\n1\t63\t0\t32\t0\r\n1\t64\t0\t33\t0\r\n1\t65\t1\t32\t0\r\n1\t66\t0\t33\t0\r\n1\t67\t1\t33\t0\r\n1\t68\t0\t33\t0\r\n1\t69\t0\t33\t0\r\n1\t70\t0\t33\t0\r\n1\t71\t0\t32\t0\r\n1\t72\t0\t34\t0\r\n1\t73\t0\t33\t0\r\n1\t74\t1\t32\t0\r\n1\t75\t0\t33\t0\r\n1\t76\t0\t33\t0\r\n1\t77\t0\t33\t0\r\n1\t78\t0\t33\t0\r\n1\t79\t0\t33\t0\r\n1\t80\t0\t33\t0\r\n1\t81\t0\t33\t0\r\n1\t82\t0\t33\t0\r\n1\t83\t0\t32\t0\r\n1\t84\t1\t32\t0\r\n1\t85\t0\t33\t0\r\n1\t86\t0\t32\t0\r\n1\t87\t0\t32\t0\r\n1\t88\t0\t33\t0\r\n1\t89\t1\t32\t0\r\n1\t90\t1\t32\t0\r\n1\t91\t0\t33\t0\r\n1\t92\t1\t33\t0\r\n1\t93\t1\t32\t0\r\n1\t94\t1\t33\t0\r\n1\t95\t0\t33\t0\r\n1\t96\t1\t32\t0\r\n1\t97\t0\t33\t0\r\n1\t98\t1\t32\t0\r\n1\t99\t0\t33\t0\r\n1\t100\t0\t32\t0\r\n1\t101\t0\t33\t0\r\n1\t102\t0\t32\t0\r\n1\t103\t0\t32\t0\r\n1\t104\t0\t33\t0\r\n1\t105\t0\t34\t0\r\n1\t106\t0\t34\t0\r\n1\t107\t1\t32\t0\r\n1\t108\t0\t33\t0\r\n1\t109\t0\t33\t0\r\n1\t110\t0\t39\t0\r\n1\t111\t0\t32\t0\r\n1\t112\t1\t32\t0\r\n1\t113\t0\t33\t0\r\n1\t114\t0\t32\t0\r\n1\t115\t0\t33\t0\r\n1\t116\t1\t33\t0\r\n1\t117\t0\t33\t0\r\n1\t118\t0\t33\t0\r\n1\t119\t0\t33\t0\r\n1\t120\t1\t32\t0\r\n1\t121\t0\t33\t0\r\n1\t122\t1\t32\t0\r\n1\t123\t1\t32\t0\r\n1\t124\t0\t33\t0\r\n1\t125\t0\t33\t0\r\n1\t126\t0\t32\t0\r\n1\t127\t0\t33\t0\r\n1\t128\t0\t33\t0\r\n1\t129\t0\t33\t0\r\n1\t130\t0\t33\t0\r\n1\t131\t1\t33\t0\r\n1\t132\t1\t33\t0\r\n1\t133\t0\t33\t0\r\n1\t134\t1\t32\t0\r\n1\t135\t1\t32\t0\r\n1\t136\t0\t33\t0\r\n1\t137\t0\t34\t0\r\n1\t138\t0\t34\t0\r\n1\t139\t1\t32\t0\r\n1\t140\t1\t32\t0\r\n1\t141\t1\t33\t0\r\n1\t142\t0\t33\t0\r\n1\t143\t0\t32\t0\r\n1\t144\t1\t32\t0\r\n1\t145\t1\t32\t0\r\n1\t146\t1\t32\t0\r\n1\t147\t2\t31\t0\r\n1\t148\t0\t33\t0\r\n1\t149\t0\t33\t0\r\n1\t150\t0\t33\t0\r\n1\t151\t0\t33\t0\r\n1\t152\t1\t33\t0\r\n1\t153\t0\t32\t0\r\n1\t154\t1\t32\t0\r\n1\t155\t1\t32\t0\r\n1\t156\t1\t32\t0\r\n1\t157\t1\t32\t0\r\n1\t158\t0\t33\t0\r\n1\t159\t0\t33\t0\r\n1\t160\t1\t32\t0\r\n1\t161\t1\t32\t0\r\n1\t162\t0\t33\t0\r\n1\t163\t0\t33\t0\r\n1\t164\t0\t33\t0\r\n1\t165\t0\t32\t0\r\n1\t166\t0\t32\t0\r\n1\t167\t0\t32\t0\r\n1\t168\t0\t33\t0\r\n1\t169\t0\t32\t0\r\n1\t170\t0\t32\t0\r\n1\t171\t0\t33\t0\r\n1\t172\t1\t32\t0\r\n1\t173\t1\t32\t0\r\n1\t174\t0\t32\t0\r\n1\t175\t1\t32\t0\r\n1\t176\t1\t32\t0\r\n1\t177\t0\t33\t0\r\n1\t178\t0\t35\t0\r\n1\t179\t0\t32\t0\r\n1\t180\t0\t32\t0\r\n1\t181\t0\t33\t0\r\n1\t182\t1\t33\t0\r\n1\t183\t1\t32\t0\r\n1\t184\t0\t33\t0\r\n1\t185\t0\t33\t0\r\n1\t186\t0\t33\t0\r\n1\t187\t0\t32\t0\r\n1\t188\t0\t32\t0\r\n1\t189\t0\t33\t0\r\n1\t190\t0\t33\t0\r\n1\t191\t0\t33\t0\r\n1\t192\t0\t33\t0\r\n1\t193\t0\t33\t0\r\n1\t194\t1\t32\t0\r\n1\t195\t0\t33\t0\r\n1\t196\t1\t32\t0\r\n1\t197\t1\t32\t0\r\n1\t198\t0\t32\t0\r\n1\t199\t1\t32\t0\r\n1\t200\t1\t32\t0\r\n1\t201\t0\t34\t0\r\n1\t202\t1\t32\t0\r\n1\t203\t0\t33\t0\r\n1\t204\t1\t33\t0\r\n1\t205\t1\t32\t0\r\n1\t206\t1\t32\t0\r\n1\t207\t1\t32\t0\r\n1\t208\t1\t33\t0\r\n1\t209\t0\t34\t0\r\n1\t210\t0\t33\t0\r\n1\t211\t0\t32\t0\r\n1\t212\t1\t32\t0\r\n1\t213\t1\t32\t0\r\n1\t214\t0\t32\t0\r\n1\t215\t1\t32\t0\r\n1\t216\t1\t32\t0\r\n1\t217\t1\t33\t0\r\n1\t218\t1\t32\t0\r\n1\t219\t1\t33\t0\r\n1\t220\t1\t32\t0\r\n1\t221\t1\t32\t0\r\n1\t222\t1\t32\t0\r\n1\t223\t1\t32\t0\r\n1\t224\t1\t32\t0\r\n1\t225\t0\t32\t0\r\n1\t226\t0\t33\t0\r\n1\t227\t0\t32\t0\r\n1\t228\t0\t33\t0\r\n1\t229\t0\t32\t0\r\n1\t230\t0\t32\t0\r\n1\t231\t1\t32\t0\r\n1\t232\t2\t32\t0\r\n1\t233\t1\t33\t0\r\n1\t234\t3\t33\t0\r\n1\t235\t4\t32\t0\r\n1\t236\t5\t33\t0\r\n1\t237\t5\t33\t0\r\n1\t238\t5\t33\t0\r\n1\t239\t4\t33\t0\r\n2\t0\t8\t28\t0\r\n2\t1\t7\t28\t0\r\n2\t2\t4\t29\t0\r\n2\t3\t4\t30\t0\r\n2\t4\t0\t33\t0\r\n2\t5\t0\t32\t0\r\n2\t6\t1\t32\t0\r\n2\t7\t1\t32\t0\r\n2\t8\t1\t32\t0\r\n2\t9\t1\t32\t0\r\n2\t10\t0\t33\t0\r\n2\t11\t0\t32\t0\r\n2\t12\t1\t32\t0\r\n2\t13\t1\t32\t0\r\n2\t14\t0\t32\t0\r\n2\t15\t2\t31\t0\r\n2\t16\t2\t32\t0\r\n2\t17\t0\t33\t0\r\n2\t18\t0\t33\t0\r\n2\t19\t1\t32\t0\r\n2\t20\t0\t33\t0\r\n2\t21\t0\t33\t0\r\n2\t22\t0\t33\t0\r\n2\t23\t1\t31\t0\r\n2\t24\t1\t32\t0\r\n2\t25\t1\t33\t0\r\n2\t26\t1\t33\t0\r\n2\t27\t2\t31\t0\r\n2\t28\t1\t32\t0\r\n2\t29\t1\t31\t0\r\n2\t30\t1\t32\t0\r\n2\t31\t1\t32\t0\r\n2\t32\t1\t33\t0\r\n2\t33\t1\t32\t0\r\n2\t34\t0\t33\t0\r\n2\t35\t1\t33\t0\r\n2\t36\t1\t33\t0\r\n2\t37\t1\t32\t0\r\n2\t38\t1\t32\t0\r\n2\t39\t0\t33\t0\r\n2\t40\t1\t32\t0\r\n2\t41\t1\t32\t0\r\n2\t42\t1\t32\t0\r\n2\t43\t0\t32\t0\r\n2\t44\t1\t32\t0\r\n2\t45\t0\t33\t0\r\n2\t46\t0\t32\t0\r\n2\t47\t1\t32\t0\r\n2\t48\t0\t32\t0\r\n2\t49\t0\t33\t0\r\n2\t50\t0\t33\t0\r\n2\t51\t0\t33\t0\r\n2\t52\t0\t33\t0\r\n2\t53\t0\t33\t0\r\n2\t54\t0\t32\t0\r\n2\t55\t0\t32\t0\r\n2\t56\t0\t33\t0\r\n2\t57\t0\t33\t0\r\n2\t58\t0\t33\t0\r\n2\t59\t0\t33\t0\r\n2\t60\t1\t32\t0\r\n2\t61\t0\t33\t0\r\n2\t62\t1\t32\t0\r\n2\t63\t1\t32\t0\r\n2\t64\t1\t32\t0\r\n2\t65\t1\t32\t0\r\n2\t66\t0\t33\t0\r\n2\t67\t1\t32\t0\r\n2\t68\t0\t33\t0\r\n2\t69\t1\t32\t0\r\n2\t70\t1\t32\t0\r\n2\t71\t1\t32\t0\r\n2\t72\t0\t34\t0\r\n2\t73\t0\t33\t0\r\n2\t74\t0\t33\t0\r\n2\t75\t1\t32\t0\r\n2\t76\t0\t32\t0\r\n2\t77\t0\t33\t0\r\n2\t78\t0\t33\t0\r\n2\t79\t0\t33\t0\r\n2\t80\t0\t33\t0\r\n2\t81\t0\t33\t0\r\n2\t82\t0\t33\t0\r\n2\t83\t0\t33\t0\r\n2\t84\t0\t33\t0\r\n2\t85\t0\t32\t0\r\n2\t86\t1\t32\t0\r\n2\t87\t0\t32\t0\r\n2\t88\t0\t33\t0\r\n2\t89\t0\t33\t0\r\n2\t90\t1\t32\t0\r\n2\t91\t1\t32\t0\r\n2\t92\t1\t33\t0\r\n2\t93\t1\t32\t0\r\n2\t94\t1\t32\t0\r\n2\t95\t0\t32\t0\r\n2\t96\t0\t32\t0\r\n2\t97\t0\t33\t0\r\n2\t98\t1\t32\t0\r\n2\t99\t1\t32\t0\r\n2\t100\t0\t34\t0\r\n2\t101\t0\t38\t0\r\n2\t102\t0\t32\t0\r\n2\t103\t1\t32\t0\r\n2\t104\t0\t32\t0\r\n2\t105\t1\t32\t0\r\n2\t106\t0\t39\t0\r\n2\t107\t0\t32\t0\r\n2\t108\t1\t32\t0\r\n2\t109\t0\t33\t0\r\n2\t110\t1\t32\t0\r\n2\t111\t1\t32\t0\r\n2\t112\t0\t33\t0\r\n2\t113\t0\t34\t0\r\n2\t114\t0\t33\t0\r\n2\t115\t1\t32\t0\r\n2\t116\t1\t32\t0\r\n2\t117\t2\t32\t0\r\n2\t118\t1\t31\t0\r\n2\t119\t0\t33\t0\r\n2\t120\t0\t33\t0\r\n2\t121\t1\t33\t0\r\n2\t122\t0\t33\t0\r\n2\t123\t1\t32\t0\r\n2\t124\t0\t33\t0\r\n2\t125\t0\t33\t0\r\n2\t126\t1\t32\t0\r\n2\t127\t0\t33\t0\r\n2\t128\t1\t33\t0\r\n2\t129\t0\t33\t0\r\n2\t130\t0\t33\t0\r\n2\t131\t1\t32\t0\r\n2\t132\t0\t33\t0\r\n2\t133\t0\t33\t0\r\n2\t134\t0\t32\t0\r\n2\t135\t0\t33\t0\r\n2\t136\t0\t33\t0\r\n2\t137\t0\t33\t0\r\n2\t138\t1\t32\t0\r\n2\t139\t1\t32\t0\r\n2\t140\t0\t33\t0\r\n2\t141\t1\t32\t0\r\n2\t142\t0\t33\t0\r\n2\t143\t0\t32\t0\r\n2\t144\t1\t32\t0\r\n2\t145\t1\t32\t0\r\n2\t146\t1\t32\t0\r\n2\t147\t1\t32\t0\r\n2\t148\t0\t32\t0\r\n2\t149\t0\t32\t0\r\n2\t150\t0\t32\t0\r\n2\t151\t1\t32\t0\r\n2\t152\t0\t33\t0\r\n2\t153\t0\t33\t0\r\n2\t154\t1\t33\t0\r\n2\t155\t0\t34\t0\r\n2\t156\t0\t34\t0\r\n2\t157\t1\t32\t0\r\n2\t158\t0\t32\t0\r\n2\t159\t1\t32\t0\r\n2\t160\t0\t33\t0\r\n2\t161\t0\t32\t0\r\n2\t162\t0\t32\t0\r\n2\t163\t0\t33\t0\r\n2\t164\t0\t33\t0\r\n2\t165\t0\t33\t0\r\n2\t166\t0\t32\t0\r\n2\t167\t0\t32\t0\r\n2\t168\t1\t33\t0\r\n2\t169\t0\t33\t0\r\n2\t170\t0\t33\t0\r\n2\t171\t1\t32\t0\r\n2\t172\t2\t31\t0\r\n2\t173\t0\t32\t0\r\n2\t174\t0\t32\t0\r\n2\t175\t0\t32\t0\r\n2\t176\t0\t33\t0\r\n2\t177\t0\t34\t0\r\n2\t178\t0\t34\t0\r\n2\t179\t2\t31\t0\r\n2\t180\t1\t32\t0\r\n2\t181\t0\t33\t0\r\n2\t182\t0\t32\t0\r\n2\t183\t1\t32\t0\r\n2\t184\t0\t33\t0\r\n2\t185\t0\t33\t0\r\n2\t186\t0\t33\t0\r\n2\t187\t0\t33\t0\r\n2\t188\t0\t35\t0\r\n2\t189\t0\t34\t0\r\n2\t190\t0\t32\t0\r\n2\t191\t0\t33\t0\r\n2\t192\t1\t33\t0\r\n2\t193\t0\t33\t0\r\n2\t194\t1\t32\t0\r\n2\t195\t0\t33\t0\r\n2\t196\t1\t32\t0\r\n2\t197\t0\t32\t0\r\n2\t198\t0\t32\t0\r\n2\t199\t0\t33\t0\r\n2\t200\t0\t33\t0\r\n2\t201\t0\t33\t0\r\n2\t202\t0\t33\t0\r\n2\t203\t1\t32\t0\r\n2\t204\t2\t32\t0\r\n2\t205\t0\t33\t0\r\n2\t206\t1\t32\t0\r\n2\t207\t1\t32\t0\r\n2\t208\t1\t32\t0\r\n2\t209\t0\t33\t0\r\n2\t210\t1\t33\t0\r\n2\t211\t1\t32\t0\r\n2\t212\t1\t32\t0\r\n2\t213\t1\t32\t0\r\n2\t214\t1\t32\t0\r\n2\t215\t1\t32\t0\r\n2\t216\t1\t32\t0\r\n2\t217\t1\t32\t0\r\n2\t218\t0\t32\t0\r\n2\t219\t1\t32\t0\r\n2\t220\t0\t34\t0\r\n2\t221\t0\t33\t0\r\n2\t222\t1\t32\t0\r\n2\t223\t1\t32\t0\r\n2\t224\t1\t32\t0\r\n2\t225\t0\t33\t0\r\n2\t226\t0\t33\t0\r\n2\t227\t1\t32\t0\r\n2\t228\t1\t32\t0\r\n2\t229\t0\t32\t0\r\n2\t230\t0\t32\t0\r\n2\t231\t0\t33\t0\r\n2\t232\t0\t33\t0\r\n2\t233\t1\t33\t0\r\n2\t234\t3\t33\t0\r\n2\t235\t3\t33\t0\r\n2\t236\t4\t33\t0\r\n2\t237\t4\t33\t0\r\n2\t238\t5\t32\t0\r\n2\t239\t5\t33\t0\r\n3\t0\t8\t28\t0\r\n3\t1\t7\t28\t0\r\n3\t2\t5\t29\t0\r\n3\t3\t4\t30\t0\r\n3\t4\t1\t32\t0\r\n3\t5\t0\t33\t0\r\n3\t6\t1\t32\t0\r\n3\t7\t1\t31\t0\r\n3\t8\t1\t32\t0\r\n3\t9\t1\t32\t0\r\n3\t10\t0\t32\t0\r\n3\t11\t2\t31\t0\r\n3\t12\t2\t31\t0\r\n3\t13\t1\t32\t0\r\n3\t14\t1\t32\t0\r\n3\t15\t1\t32\t0\r\n3\t16\t1\t32\t0\r\n3\t17\t1\t32\t0\r\n3\t18\t1\t32\t0\r\n3\t19\t0\t33\t0\r\n3\t20\t0\t33\t0\r\n3\t21\t1\t32\t0\r\n3\t22\t1\t32\t0\r\n3\t23\t2\t31\t0\r\n3\t24\t1\t32\t0\r\n3\t25\t1\t32\t0\r\n3\t26\t1\t32\t0\r\n3\t27\t1\t32\t0\r\n3\t28\t1\t32\t0\r\n3\t29\t1\t32\t0\r\n3\t30\t1\t32\t0\r\n3\t31\t1\t32\t0\r\n3\t32\t1\t33\t0\r\n3\t33\t1\t32\t0\r\n3\t34\t0\t34\t0\r\n3\t35\t1\t33\t0\r\n3\t36\t0\t32\t0\r\n3\t37\t1\t32\t0\r\n3\t38\t1\t32\t0\r\n3\t39\t0\t32\t0\r\n3\t40\t0\t33\t0\r\n3\t41\t1\t32\t0\r\n3\t42\t0\t32\t0\r\n3\t43\t0\t32\t0\r\n3\t44\t1\t32\t0\r\n3\t45\t0\t32\t0\r\n3\t46\t0\t32\t0\r\n3\t47\t1\t32\t0\r\n3\t48\t2\t32\t0\r\n3\t49\t0\t32\t0\r\n3\t50\t0\t32\t0\r\n3\t51\t0\t33\t0\r\n3\t52\t1\t32\t0\r\n3\t53\t1\t33\t0\r\n3\t54\t1\t32\t0\r\n3\t55\t0\t32\t0\r\n3\t56\t1\t33\t0\r\n3\t57\t0\t32\t0\r\n3\t58\t0\t33\t0\r\n3\t59\t2\t32\t0\r\n3\t60\t0\t32\t0\r\n3\t61\t2\t31\t0\r\n3\t62\t1\t32\t0\r\n3\t63\t1\t32\t0\r\n3\t64\t1\t32\t0\r\n3\t65\t1\t32\t0\r\n3\t66\t1\t32\t0\r\n3\t67\t0\t33\t0\r\n3\t68\t0\t33\t0\r\n3\t69\t1\t32\t0\r\n3\t70\t1\t32\t0\r\n3\t71\t1\t32\t0\r\n3\t72\t1\t33\t0\r\n3\t73\t0\t33\t0\r\n3\t74\t1\t32\t0\r\n3\t75\t1\t32\t0\r\n3\t76\t1\t32\t0\r\n3\t77\t2\t31\t0\r\n3\t78\t2\t31\t0\r\n3\t79\t1\t32\t0\r\n3\t80\t1\t32\t0\r\n3\t81\t0\t33\t0\r\n3\t82\t0\t33\t0\r\n3\t83\t0\t33\t0\r\n3\t84\t0\t33\t0\r\n3\t85\t1\t32\t0\r\n3\t86\t1\t32\t0\r\n3\t87\t0\t32\t0\r\n3\t88\t1\t32\t0\r\n3\t89\t1\t32\t0\r\n3\t90\t1\t32\t0\r\n3\t91\t1\t32\t0\r\n3\t92\t1\t32\t0\r\n3\t93\t1\t32\t0\r\n3\t94\t1\t32\t0\r\n3\t95\t0\t33\t0\r\n3\t96\t1\t32\t0\r\n3\t97\t0\t33\t0\r\n3\t98\t1\t32\t0\r\n3\t99\t1\t32\t0\r\n3\t100\t0\t33\t0\r\n3\t101\t0\t33\t0\r\n3\t102\t1\t32\t0\r\n3\t103\t0\t32\t0\r\n3\t104\t1\t32\t0\r\n3\t105\t1\t32\t0\r\n3\t106\t1\t32\t0\r\n3\t107\t1\t32\t0\r\n3\t108\t2\t32\t0\r\n3\t109\t1\t32\t0\r\n3\t110\t1\t32\t0\r\n3\t111\t1\t33\t0\r\n3\t112\t0\t33\t0\r\n3\t113\t1\t32\t0\r\n3\t114\t1\t32\t0\r\n3\t115\t2\t31\t0\r\n3\t116\t1\t32\t0\r\n3\t117\t1\t32\t0\r\n3\t118\t1\t31\t0\r\n3\t119\t1\t32\t0\r\n3\t120\t0\t33\t0\r\n3\t121\t1\t32\t0\r\n3\t122\t1\t32\t0\r\n3\t123\t1\t32\t0\r\n3\t124\t1\t32\t0\r\n3\t125\t0\t32\t0\r\n3\t126\t0\t32\t0\r\n3\t127\t1\t32\t0\r\n3\t128\t1\t32\t0\r\n3\t129\t0\t33\t0\r\n3\t130\t1\t33\t0\r\n3\t131\t1\t32\t0\r\n3\t132\t1\t32\t0\r\n3\t133\t0\t33\t0\r\n3\t134\t1\t32\t0\r\n3\t135\t0\t32\t0\r\n3\t136\t0\t33\t0\r\n3\t137\t0\t33\t0\r\n3\t138\t0\t32\t0\r\n3\t139\t1\t33\t0\r\n3\t140\t0\t32\t0\r\n3\t141\t1\t32\t0\r\n3\t142\t1\t32\t0\r\n3\t143\t0\t32\t0\r\n3\t144\t1\t32\t0\r\n3\t145\t0\t32\t0\r\n3\t146\t1\t31\t0\r\n3\t147\t0\t32\t0\r\n3\t148\t0\t33\t0\r\n3\t149\t0\t32\t0\r\n3\t150\t1\t32\t0\r\n3\t151\t0\t33\t0\r\n3\t152\t1\t32\t0\r\n3\t153\t0\t34\t0\r\n3\t154\t1\t33\t0\r\n3\t155\t1\t32\t0\r\n3\t156\t0\t33\t0\r\n3\t157\t0\t32\t0\r\n3\t158\t0\t32\t0\r\n3\t159\t2\t31\t0\r\n3\t160\t0\t33\t0\r\n3\t161\t0\t33\t0\r\n3\t162\t1\t32\t0\r\n3\t163\t0\t33\t0\r\n3\t164\t0\t33\t0\r\n3\t165\t1\t32\t0\r\n3\t166\t1\t32\t0\r\n3\t167\t1\t31\t0\r\n3\t168\t1\t32\t0\r\n3\t169\t0\t33\t0\r\n3\t170\t1\t32\t0\r\n3\t171\t0\t33\t0\r\n3\t172\t1\t32\t0\r\n3\t173\t0\t32\t0\r\n3\t174\t0\t32\t0\r\n3\t175\t1\t32\t0\r\n3\t176\t0\t33\t0\r\n3\t177\t0\t33\t0\r\n3\t178\t0\t33\t0\r\n3\t179\t1\t32\t0\r\n3\t180\t1\t32\t0\r\n3\t181\t1\t32\t0\r\n3\t182\t0\t32\t0\r\n3\t183\t1\t32\t0\r\n3\t184\t1\t32\t0\r\n3\t185\t1\t32\t0\r\n3\t186\t0\t33\t0\r\n3\t187\t1\t32\t0\r\n3\t188\t1\t33\t0\r\n3\t189\t1\t33\t0\r\n3\t190\t0\t32\t0\r\n3\t191\t0\t33\t0\r\n3\t192\t0\t33\t0\r\n3\t193\t0\t33\t0\r\n3\t194\t0\t33\t0\r\n3\t195\t0\t32\t0\r\n3\t196\t0\t32\t0\r\n3\t197\t1\t32\t0\r\n3\t198\t1\t32\t0\r\n3\t199\t0\t32\t0\r\n3\t200\t0\t32\t0\r\n3\t201\t1\t32\t0\r\n3\t202\t0\t33\t0\r\n3\t203\t1\t32\t0\r\n3\t204\t1\t32\t0\r\n3\t205\t0\t33\t0\r\n3\t206\t0\t32\t0\r\n3\t207\t1\t32\t0\r\n3\t208\t1\t32\t0\r\n3\t209\t0\t32\t0\r\n3\t210\t1\t32\t0\r\n3\t211\t0\t33\t0\r\n3\t212\t0\t32\t0\r\n3\t213\t0\t32\t0\r\n3\t214\t1\t31\t0\r\n3\t215\t1\t31\t0\r\n3\t216\t2\t32\t0\r\n3\t217\t0\t32\t0\r\n3\t218\t0\t33\t0\r\n3\t219\t1\t32\t0\r\n3\t220\t2\t32\t0\r\n3\t221\t1\t33\t0\r\n3\t222\t1\t32\t0\r\n3\t223\t1\t31\t0\r\n3\t224\t1\t32\t0\r\n3\t225\t0\t33\t0\r\n3\t226\t1\t32\t0\r\n3\t227\t0\t32\t0\r\n3\t228\t1\t32\t0\r\n3\t229\t0\t32\t0\r\n3\t230\t1\t32\t0\r\n3\t231\t0\t32\t0\r\n3\t232\t1\t32\t0\r\n3\t233\t2\t32\t0\r\n3\t234\t3\t33\t0\r\n3\t235\t5\t32\t0\r\n3\t236\t4\t33\t0\r\n3\t237\t4\t33\t0\r\n3\t238\t5\t32\t0\r\n3\t239\t5\t32\t0\r\n4\t0\t8\t28\t0\r\n4\t1\t8\t28\t0\r\n4\t2\t5\t29\t0\r\n4\t3\t4\t30\t0\r\n4\t4\t0\t32\t0\r\n4\t5\t0\t32\t0\r\n4\t6\t0\t32\t0\r\n4\t7\t0\t32\t0\r\n4\t8\t0\t32\t0\r\n4\t9\t0\t31\t0\r\n4\t10\t0\t32\t0\r\n4\t11\t0\t33\t0\r\n4\t12\t1\t32\t0\r\n4\t13\t0\t32\t0\r\n4\t14\t1\t31\t0\r\n4\t15\t1\t32\t0\r\n4\t16\t0\t32\t0\r\n4\t17\t1\t32\t0\r\n4\t18\t0\t32\t0\r\n4\t19\t1\t32\t0\r\n4\t20\t1\t32\t0\r\n4\t21\t0\t33\t0\r\n4\t22\t1\t32\t0\r\n4\t23\t0\t33\t0\r\n4\t24\t0\t33\t0\r\n4\t25\t0\t33\t0\r\n4\t26\t0\t32\t0\r\n4\t27\t0\t32\t0\r\n4\t28\t0\t32\t0\r\n4\t29\t0\t32\t0\r\n4\t30\t0\t33\t0\r\n4\t31\t0\t32\t0\r\n4\t32\t0\t33\t0\r\n4\t33\t0\t33\t0\r\n4\t34\t0\t32\t0\r\n4\t35\t0\t33\t0\r\n4\t36\t0\t32\t0\r\n4\t37\t1\t32\t0\r\n4\t38\t0\t32\t0\r\n4\t39\t1\t32\t0\r\n4\t40\t0\t33\t0\r\n4\t41\t0\t34\t0\r\n4\t42\t0\t33\t0\r\n4\t43\t0\t32\t0\r\n4\t44\t0\t32\t0\r\n4\t45\t1\t32\t0\r\n4\t46\t0\t32\t0\r\n4\t47\t0\t32\t0\r\n4\t48\t1\t32\t0\r\n4\t49\t0\t32\t0\r\n4\t50\t0\t32\t0\r\n4\t51\t0\t32\t0\r\n4\t52\t0\t32\t0\r\n4\t53\t0\t32\t0\r\n4\t54\t0\t32\t0\r\n4\t55\t0\t31\t0\r\n4\t56\t0\t34\t0\r\n4\t57\t0\t32\t0\r\n4\t58\t0\t32\t0\r\n4\t59\t0\t31\t0\r\n4\t60\t0\t32\t0\r\n4\t61\t0\t31\t0\r\n4\t62\t1\t31\t0\r\n4\t63\t0\t32\t0\r\n4\t64\t0\t32\t0\r\n4\t65\t0\t32\t0\r\n4\t66\t0\t32\t0\r\n4\t67\t0\t32\t0\r\n4\t68\t0\t33\t0\r\n4\t69\t0\t33\t0\r\n4\t70\t0\t33\t0\r\n4\t71\t0\t32\t0\r\n4\t72\t0\t32\t0\r\n4\t73\t0\t33\t0\r\n4\t74\t0\t32\t0\r\n4\t75\t0\t31\t0\r\n4\t76\t0\t33\t0\r\n4\t77\t0\t32\t0\r\n4\t78\t0\t32\t0\r\n4\t79\t0\t31\t0\r\n4\t80\t0\t32\t0\r\n4\t81\t0\t33\t0\r\n4\t82\t0\t32\t0\r\n4\t83\t0\t31\t0\r\n4\t84\t0\t32\t0\r\n4\t85\t0\t33\t0\r\n4\t86\t1\t31\t0\r\n4\t87\t0\t32\t0\r\n4\t88\t0\t32\t0\r\n4\t89\t0\t33\t0\r\n4\t90\t0\t32\t0\r\n4\t91\t0\t33\t0\r\n4\t92\t0\t32\t0\r\n4\t93\t0\t32\t0\r\n4\t94\t0\t32\t0\r\n4\t95\t0\t32\t0\r\n4\t96\t0\t33\t0\r\n4\t97\t0\t33\t0\r\n4\t98\t0\t33\t0\r\n4\t99\t0\t32\t0\r\n4\t100\t0\t32\t0\r\n4\t101\t0\t32\t0\r\n4\t102\t1\t31\t0\r\n4\t103\t0\t32\t0\r\n4\t104\t0\t32\t0\r\n4\t105\t0\t33\t0\r\n4\t106\t0\t33\t0\r\n4\t107\t0\t32\t0\r\n4\t108\t1\t32\t0\r\n4\t109\t0\t33\t0\r\n4\t110\t0\t32\t0\r\n4\t111\t0\t32\t0\r\n4\t112\t0\t32\t0\r\n4\t113\t0\t33\t0\r\n4\t114\t0\t32\t0\r\n4\t115\t0\t33\t0\r\n4\t116\t0\t32\t0\r\n4\t117\t1\t32\t0\r\n4\t118\t0\t31\t0\r\n4\t119\t0\t32\t0\r\n4\t120\t0\t33\t0\r\n4\t121\t0\t32\t0\r\n4\t122\t0\t33\t0\r\n4\t123\t0\t32\t0\r\n4\t124\t0\t32\t0\r\n4\t125\t0\t32\t0\r\n4\t126\t0\t32\t0\r\n4\t127\t1\t32\t0\r\n4\t128\t1\t32\t0\r\n4\t129\t0\t34\t0\r\n4\t130\t0\t32\t0\r\n4\t131\t0\t32\t0\r\n4\t132\t0\t32\t0\r\n4\t133\t0\t33\t0\r\n4\t134\t0\t32\t0\r\n4\t135\t0\t32\t0\r\n4\t136\t0\t32\t0\r\n4\t137\t0\t33\t0\r\n4\t138\t1\t31\t0\r\n4\t139\t0\t32\t0\r\n4\t140\t0\t32\t0\r\n4\t141\t0\t32\t0\r\n4\t142\t0\t32\t0\r\n4\t143\t1\t31\t0\r\n4\t144\t1\t32\t0\r\n4\t145\t0\t32\t0\r\n4\t146\t0\t32\t0\r\n4\t147\t0\t32\t0\r\n4\t148\t0\t32\t0\r\n4\t149\t0\t33\t0\r\n4\t150\t0\t33\t0\r\n4\t151\t0\t32\t0\r\n4\t152\t0\t32\t0\r\n4\t153\t0\t33\t0\r\n4\t154\t0\t32\t0\r\n4\t155\t1\t32\t0\r\n4\t156\t0\t33\t0\r\n4\t157\t0\t32\t0\r\n4\t158\t0\t32\t0\r\n4\t159\t0\t32\t0\r\n4\t160\t0\t32\t0\r\n4\t161\t0\t32\t0\r\n4\t162\t0\t32\t0\r\n4\t163\t0\t32\t0\r\n4\t164\t0\t32\t0\r\n4\t165\t1\t31\t0\r\n4\t166\t1\t31\t0\r\n4\t167\t0\t32\t0\r\n4\t168\t0\t32\t0\r\n4\t169\t0\t32\t0\r\n4\t170\t0\t31\t0\r\n4\t171\t0\t32\t0\r\n4\t172\t0\t32\t0\r\n4\t173\t0\t32\t0\r\n4\t174\t0\t32\t0\r\n4\t175\t0\t32\t0\r\n4\t176\t0\t32\t0\r\n4\t177\t0\t32\t0\r\n4\t178\t0\t32\t0\r\n4\t179\t0\t31\t0\r\n4\t180\t0\t33\t0\r\n4\t181\t0\t32\t0\r\n4\t182\t0\t32\t0\r\n4\t183\t0\t32\t0\r\n4\t184\t0\t33\t0\r\n4\t185\t0\t33\t0\r\n4\t186\t0\t32\t0\r\n4\t187\t0\t32\t0\r\n4\t188\t0\t33\t0\r\n4\t189\t1\t32\t0\r\n4\t190\t0\t32\t0\r\n4\t191\t0\t33\t0\r\n4\t192\t0\t33\t0\r\n4\t193\t0\t32\t0\r\n4\t194\t0\t32\t0\r\n4\t195\t0\t32\t0\r\n4\t196\t0\t32\t0\r\n4\t197\t0\t32\t0\r\n4\t198\t1\t31\t0\r\n4\t199\t0\t32\t0\r\n4\t200\t0\t32\t0\r\n4\t201\t0\t32\t0\r\n4\t202\t0\t32\t0\r\n4\t203\t0\t32\t0\r\n4\t204\t0\t32\t0\r\n4\t205\t0\t32\t0\r\n4\t206\t0\t32\t0\r\n4\t207\t1\t32\t0\r\n4\t208\t1\t31\t0\r\n4\t209\t0\t32\t0\r\n4\t210\t1\t32\t0\r\n4\t211\t0\t33\t0\r\n4\t212\t1\t32\t0\r\n4\t213\t0\t32\t0\r\n4\t214\t2\t31\t0\r\n4\t215\t1\t32\t0\r\n4\t216\t0\t33\t0\r\n4\t217\t0\t33\t0\r\n4\t218\t0\t32\t0\r\n4\t219\t0\t32\t0\r\n4\t220\t0\t33\t0\r\n4\t221\t0\t33\t0\r\n4\t222\t0\t33\t0\r\n4\t223\t0\t31\t0\r\n4\t224\t0\t33\t0\r\n4\t225\t0\t32\t0\r\n4\t226\t0\t31\t0\r\n4\t227\t0\t33\t0\r\n4\t228\t0\t34\t0\r\n4\t229\t1\t31\t0\r\n4\t230\t0\t31\t0\r\n4\t231\t0\t32\t0\r\n4\t232\t1\t32\t0\r\n4\t233\t1\t33\t0\r\n4\t234\t2\t33\t0\r\n4\t235\t3\t33\t0\r\n4\t236\t3\t33\t0\r\n4\t237\t4\t33\t0\r\n4\t238\t3\t33\t0\r\n4\t239\t3\t33\t0\r\n5\t0\t8\t28\t0\r\n5\t1\t8\t28\t0\r\n5\t2\t4\t30\t0\r\n5\t3\t5\t29\t0\r\n5\t4\t1\t32\t0\r\n5\t5\t0\t33\t0\r\n5\t6\t0\t33\t0\r\n5\t7\t0\t32\t0\r\n5\t8\t0\t32\t0\r\n5\t9\t0\t31\t0\r\n5\t10\t0\t31\t0\r\n5\t11\t0\t32\t0\r\n5\t12\t0\t32\t0\r\n5\t13\t0\t32\t0\r\n5\t14\t0\t31\t0\r\n5\t15\t0\t31\t0\r\n5\t16\t0\t32\t0\r\n5\t17\t0\t32\t0\r\n5\t18\t0\t31\t0\r\n5\t19\t0\t31\t0\r\n5\t20\t0\t31\t0\r\n5\t21\t0\t32\t0\r\n5\t22\t0\t32\t0\r\n5\t23\t0\t32\t0\r\n5\t24\t0\t32\t0\r\n5\t25\t0\t32\t0\r\n5\t26\t0\t32\t0\r\n5\t27\t0\t32\t0\r\n5\t28\t0\t32\t0\r\n5\t29\t0\t32\t0\r\n5\t30\t0\t31\t0\r\n5\t31\t0\t31\t0\r\n5\t32\t0\t32\t0\r\n5\t33\t0\t32\t0\r\n5\t34\t0\t33\t0\r\n5\t35\t0\t31\t0\r\n5\t36\t0\t31\t0\r\n5\t37\t0\t31\t0\r\n5\t38\t0\t31\t0\r\n5\t39\t0\t31\t0\r\n5\t40\t0\t32\t0\r\n5\t41\t0\t33\t0\r\n5\t42\t0\t32\t0\r\n5\t43\t0\t31\t0\r\n5\t44\t0\t32\t0\r\n5\t45\t0\t32\t0\r\n5\t46\t0\t31\t0\r\n5\t47\t0\t31\t0\r\n5\t48\t0\t32\t0\r\n5\t49\t0\t31\t0\r\n5\t50\t0\t31\t0\r\n5\t51\t0\t31\t0\r\n5\t52\t0\t34\t0\r\n5\t53\t0\t33\t0\r\n5\t54\t0\t31\t0\r\n5\t55\t0\t31\t0\r\n5\t56\t0\t32\t0\r\n5\t57\t0\t31\t0\r\n5\t58\t0\t31\t0\r\n5\t59\t0\t30\t0\r\n5\t60\t0\t31\t0\r\n5\t61\t0\t31\t0\r\n5\t62\t0\t31\t0\r\n5\t63\t0\t30\t0\r\n5\t64\t0\t32\t0\r\n5\t65\t0\t32\t0\r\n5\t66\t0\t31\t0\r\n5\t67\t0\t31\t0\r\n5\t68\t0\t32\t0\r\n5\t69\t0\t31\t0\r\n5\t70\t0\t31\t0\r\n5\t71\t0\t31\t0\r\n5\t72\t0\t31\t0\r\n5\t73\t0\t31\t0\r\n5\t74\t0\t31\t0\r\n5\t75\t0\t31\t0\r\n5\t76\t0\t32\t0\r\n5\t77\t0\t33\t0\r\n5\t78\t0\t31\t0\r\n5\t79\t0\t30\t0\r\n5\t80\t0\t33\t0\r\n5\t81\t0\t32\t0\r\n5\t82\t0\t32\t0\r\n5\t83\t0\t31\t0\r\n5\t84\t0\t32\t0\r\n5\t85\t0\t32\t0\r\n5\t86\t0\t31\t0\r\n5\t87\t0\t32\t0\r\n5\t88\t0\t32\t0\r\n5\t89\t0\t33\t0\r\n5\t90\t0\t31\t0\r\n5\t91\t0\t32\t0\r\n5\t92\t0\t31\t0\r\n5\t93\t0\t32\t0\r\n5\t94\t0\t31\t0\r\n5\t95\t0\t31\t0\r\n5\t96\t0\t32\t0\r\n5\t97\t0\t32\t0\r\n5\t98\t0\t32\t0\r\n5\t99\t0\t31\t0\r\n5\t100\t0\t31\t0\r\n5\t101\t0\t31\t0\r\n5\t102\t0\t31\t0\r\n5\t103\t0\t31\t0\r\n5\t104\t0\t32\t0\r\n5\t105\t0\t32\t0\r\n5\t106\t0\t32\t0\r\n5\t107\t0\t31\t0\r\n5\t108\t0\t32\t0\r\n5\t109\t0\t32\t0\r\n5\t110\t0\t31\t0\r\n5\t111\t0\t31\t0\r\n5\t112\t0\t32\t0\r\n5\t113\t0\t32\t0\r\n5\t114\t0\t31\t0\r\n5\t115\t0\t31\t0\r\n5\t116\t0\t32\t0\r\n5\t117\t0\t32\t0\r\n5\t118\t0\t32\t0\r\n5\t119\t0\t31\t0\r\n5\t120\t0\t32\t0\r\n5\t121\t0\t32\t0\r\n5\t122\t0\t32\t0\r\n5\t123\t0\t31\t0\r\n5\t124\t0\t31\t0\r\n5\t125\t0\t32\t0\r\n5\t126\t0\t31\t0\r\n5\t127\t1\t30\t0\r\n5\t128\t0\t31\t0\r\n5\t129\t0\t33\t0\r\n5\t130\t0\t32\t0\r\n5\t131\t0\t31\t0\r\n5\t132\t0\t32\t0\r\n5\t133\t0\t32\t0\r\n5\t134\t0\t32\t0\r\n5\t135\t0\t31\t0\r\n5\t136\t0\t31\t0\r\n5\t137\t0\t32\t0\r\n5\t138\t0\t31\t0\r\n5\t139\t0\t32\t0\r\n5\t140\t0\t33\t0\r\n5\t141\t0\t32\t0\r\n5\t142\t0\t31\t0\r\n5\t143\t0\t31\t0\r\n5\t144\t0\t31\t0\r\n5\t145\t0\t32\t0\r\n5\t146\t0\t32\t0\r\n5\t147\t0\t31\t0\r\n5\t148\t0\t31\t0\r\n5\t149\t0\t30\t0\r\n5\t150\t0\t31\t0\r\n5\t151\t0\t31\t0\r\n5\t152\t0\t33\t0\r\n5\t153\t0\t31\t0\r\n5\t154\t0\t31\t0\r\n5\t155\t1\t30\t0\r\n5\t156\t0\t31\t0\r\n5\t157\t0\t32\t0\r\n5\t158\t0\t32\t0\r\n5\t159\t0\t31\t0\r\n5\t160\t0\t31\t0\r\n5\t161\t0\t32\t0\r\n5\t162\t0\t32\t0\r\n5\t163\t0\t31\t0\r\n5\t164\t0\t31\t0\r\n5\t165\t0\t32\t0\r\n5\t166\t0\t31\t0\r\n5\t167\t0\t32\t0\r\n5\t168\t0\t32\t0\r\n5\t169\t0\t32\t0\r\n5\t170\t0\t31\t0\r\n5\t171\t0\t31\t0\r\n5\t172\t0\t31\t0\r\n5\t173\t0\t33\t0\r\n5\t174\t0\t32\t0\r\n5\t175\t0\t31\t0\r\n5\t176\t0\t31\t0\r\n5\t177\t0\t31\t0\r\n5\t178\t0\t31\t0\r\n5\t179\t0\t32\t0\r\n5\t180\t0\t32\t0\r\n5\t181\t0\t32\t0\r\n5\t182\t0\t32\t0\r\n5\t183\t0\t31\t0\r\n5\t184\t0\t32\t0\r\n5\t185\t0\t32\t0\r\n5\t186\t0\t31\t0\r\n5\t187\t0\t31\t0\r\n5\t188\t0\t33\t0\r\n5\t189\t0\t32\t0\r\n5\t190\t0\t31\t0\r\n5\t191\t0\t31\t0\r\n5\t192\t0\t32\t0\r\n5\t193\t0\t31\t0\r\n5\t194\t0\t31\t0\r\n5\t195\t0\t33\t0\r\n5\t196\t0\t31\t0\r\n5\t197\t0\t32\t0\r\n5\t198\t0\t31\t0\r\n5\t199\t0\t31\t0\r\n5\t200\t0\t31\t0\r\n5\t201\t0\t31\t0\r\n5\t202\t0\t30\t0\r\n5\t203\t0\t30\t0\r\n5\t204\t0\t31\t0\r\n5\t205\t0\t31\t0\r\n5\t206\t0\t31\t0\r\n5\t207\t0\t31\t0\r\n5\t208\t0\t31\t0\r\n5\t209\t0\t32\t0\r\n5\t210\t0\t31\t0\r\n5\t211\t0\t31\t0\r\n5\t212\t0\t31\t0\r\n5\t213\t0\t32\t0\r\n5\t214\t0\t30\t0\r\n5\t215\t0\t31\t0\r\n5\t216\t0\t32\t0\r\n5\t217\t0\t32\t0\r\n5\t218\t0\t32\t0\r\n5\t219\t0\t31\t0\r\n5\t220\t0\t31\t0\r\n5\t221\t0\t31\t0\r\n5\t222\t0\t31\t0\r\n5\t223\t0\t31\t0\r\n5\t224\t0\t32\t0\r\n5\t225\t0\t31\t0\r\n5\t226\t0\t31\t0\r\n5\t227\t0\t31\t0\r\n5\t228\t0\t31\t0\r\n5\t229\t0\t30\t0\r\n5\t230\t0\t30\t0\r\n5\t231\t0\t31\t0\r\n5\t232\t0\t32\t0\r\n5\t233\t0\t32\t0\r\n5\t234\t0\t33\t0\r\n5\t235\t1\t33\t0\r\n5\t236\t1\t33\t0\r\n5\t237\t1\t34\t0\r\n5\t238\t1\t34\t0\r\n5\t239\t1\t33\t0\r\n6\t0\t7\t28\t0\r\n6\t1\t8\t28\t0\r\n6\t2\t4\t30\t0\r\n6\t3\t3\t31\t0\r\n6\t4\t0\t33\t0\r\n6\t5\t0\t33\t0\r\n6\t6\t0\t32\t0\r\n6\t7\t0\t32\t0\r\n6\t8\t0\t32\t0\r\n6\t9\t0\t31\t0\r\n6\t10\t0\t31\t0\r\n6\t11\t0\t32\t0\r\n6\t12\t0\t32\t0\r\n6\t13\t0\t32\t0\r\n6\t14\t0\t31\t0\r\n6\t15\t0\t31\t0\r\n6\t16\t0\t32\t0\r\n6\t17\t0\t33\t0\r\n6\t18\t0\t33\t0\r\n6\t19\t0\t32\t0\r\n6\t20\t0\t32\t0\r\n6\t21\t0\t32\t0\r\n6\t22\t0\t32\t0\r\n6\t23\t0\t33\t0\r\n6\t24\t0\t32\t0\r\n6\t25\t0\t31\t0\r\n6\t26\t0\t32\t0\r\n6\t27\t0\t32\t0\r\n6\t28\t0\t33\t0\r\n6\t29\t0\t32\t0\r\n6\t30\t0\t32\t0\r\n6\t31\t0\t32\t0\r\n6\t32\t0\t32\t0\r\n6\t33\t0\t32\t0\r\n6\t34\t0\t32\t0\r\n6\t35\t0\t32\t0\r\n6\t36\t0\t32\t0\r\n6\t37\t0\t32\t0\r\n6\t38\t0\t31\t0\r\n6[...string is too long...]";

        // Token: 0x04000002 RID: 2
        private static short MAX_PIXELS_THAT_CAN_FAIL = 6528;

        // Token: 0x04000003 RID: 3
        private static readonly short[,,] LimitArray = Maths.GetLimitArray();

        /// <summary>
        /// The calibration matlab file.
        /// </summary>
        // Token: 0x04000005 RID: 5
        public static readonly string BaseCalibration;

        // Token: 0x04000006 RID: 6
        private static readonly CalibrationAnalysis CalibrationAnalysis;

        // Token: 0x04000007 RID: 7
        private static readonly formCalc FormCalc;

        // Token: 0x02000003 RID: 3
        private enum FailureRegion
        {
            // Token: 0x04000011 RID: 17
            None,
            // Token: 0x04000012 RID: 18
            Outer,
            // Token: 0x04000013 RID: 19
            Inner,
            // Token: 0x04000014 RID: 20
            Corona,
            // Token: 0x04000015 RID: 21
            HighP
        }
    }
}
