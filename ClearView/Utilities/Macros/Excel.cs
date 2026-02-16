using Emgu.CV;
using Emgu.CV.Structure;
using EPIC.ClearView.Utilities;
using EPIC.DataAnalysis;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace EPIC.ClearView.Utilities.Macros
{
    // Token: 0x0200001D RID: 29
    public static class Excel
    {
        // Token: 0x0600018D RID: 397 RVA: 0x0000D2B8 File Offset: 0x0000B4B8
        public static void CreateXLSXFromImageCalibrations(IEnumerable<DataLayer.Entities.ImageCalibration> results, string filename, Action<string, int> reportProgress = null)
        {
            if (reportProgress != null)
            {
                reportProgress("Starting Excel for OLE", 0);
            }
            Application application = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            application.Visible = false;
            Application application2 = application;
            Workbook workbook = application2.Workbooks.Add(Missing.Value);
            if (reportProgress != null)
            {
                reportProgress("Creating Workspace", 10);
            }
            try
            {
                if (Excel.o__SiteContainer1.p__Site2 == null)
                {
                    Excel.o__SiteContainer1.p__Site2 = CallSite<Func<CallSite, object, Worksheet>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Worksheet), typeof(Excel)));
                }
                Worksheet worksheet = Excel.o__SiteContainer1.p__Site2.Target(Excel.o__SiteContainer1.p__Site2, workbook.Sheets.Add(Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                worksheet.Name = "Analysis";
                workbook.Sheets.OfType<Worksheet>().Except(new Worksheet[]
                {
                    worksheet
                }).ToList<Worksheet>().ForEach(delegate (Worksheet x)
                {
                    x.Delete();
                });
                int num = 1;
                List<IGrouping<string, DataLayer.Entities.ImageCalibration>> list = (from x in results.GroupBy(delegate (DataLayer.Entities.ImageCalibration x)
                {
                    DataLayer.Entities.ImageCapture capture = x.Image.Capture;
                    if (capture == null)
                    {
                        throw new NullReferenceException("DataLayer.Entities.Capture is null for one of the images in the set.");
                    }
                    return string.Format("B{0}G{1}", capture.Capture.Brightness, capture.Capture.Gain);
                })
                                                                                     orderby x.Key
                                                                                     select x).ToList<IGrouping<string, DataLayer.Entities.ImageCalibration>>();
                foreach (Excel.SummarySections item in Excel.Properties)
                {
                    worksheet.Cells[num, "A"] = "Threshold";
                    worksheet.Cells[num, "B"] = item.Threshold;
                    num++;
                    worksheet.Cells[num, "A"] = "Section Results";
                    worksheet.Cells[num, "B"] = item.Section;
                    int num2 = num + 1;
                    int num3 = 2;
                    foreach (IGrouping<string, DataLayer.Entities.ImageCalibration> grouping in list)
                    {
                        num = num2;
                        if (Excel.o__SiteContainer1.p__Site3 == null)
                        {
                            Excel.o__SiteContainer1.p__Site3 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                        }
                        Range range = Excel.o__SiteContainer1.p__Site3.Target(Excel.o__SiteContainer1.p__Site3, worksheet.Cells[num, num3]);
                        range.set_Value(Missing.Value, grouping.Key);
                        range.Font.Bold = true;
                        range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                        Marshal.FinalReleaseComObject(range);
                        num++;
                        foreach (DataLayer.Entities.ImageCalibration imageCalibration in grouping)
                        {
                            if (num3 == 2)
                            {
                                int num4 = grouping.ToList<DataLayer.Entities.ImageCalibration>().IndexOf(imageCalibration);
                                if (Excel.o__SiteContainer1.p__Site4 == null)
                                {
                                    Excel.o__SiteContainer1.p__Site4 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                                }
                                Range range2 = Excel.o__SiteContainer1.p__Site4.Target(Excel.o__SiteContainer1.p__Site4, worksheet.Cells[num, num3 - 1]);
                                range2.set_Value(Missing.Value, "Calibration Image #" + num4);
                                range2.Columns.AutoFit();
                                if (num4 == 0)
                                {
                                    range2.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                                }
                                Marshal.FinalReleaseComObject(range2);
                            }
                            if (Excel.o__SiteContainer1.p__Site5 == null)
                            {
                                Excel.o__SiteContainer1.p__Site5 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                            }
                            Range range3 = Excel.o__SiteContainer1.p__Site5.Target(Excel.o__SiteContainer1.p__Site5, worksheet.Cells[num, num3]);
                            range3.set_Value(Missing.Value, item.Property(imageCalibration));
                            Marshal.FinalReleaseComObject(range3);
                            num++;
                        }
                        num = num2 + list.Max((IGrouping<string, DataLayer.Entities.ImageCalibration> x) => x.Count<DataLayer.Entities.ImageCalibration>()) + 1;
                        if (num3 == 2)
                        {
                            if (Excel.o__SiteContainer1.p__Site6 == null)
                            {
                                Excel.o__SiteContainer1.p__Site6 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                            }
                            Range range4 = Excel.o__SiteContainer1.p__Site6.Target(Excel.o__SiteContainer1.p__Site6, worksheet.Cells[num, num3 - 1]);
                            range4.set_Value(Missing.Value, "ave");
                            range4.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                            Marshal.FinalReleaseComObject(range4);
                        }
                        if (Excel.o__SiteContainer1.p__Site7 == null)
                        {
                            Excel.o__SiteContainer1.p__Site7 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                        }
                        Range range5 = Excel.o__SiteContainer1.p__Site7.Target(Excel.o__SiteContainer1.p__Site7, worksheet.Cells[num, num3]);
                        range5.Formula = string.Concat(new object[]
                        {
                            "=AVERAGE(",
                            Excel.IndexToColumnName(num3),
                            num2 + 1,
                            ":",
                            Excel.IndexToColumnName(num3),
                            num2 + grouping.Count<DataLayer.Entities.ImageCalibration>(),
                            ")"
                        });
                        range5.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                        if (Excel.o__SiteContainer1.p__Site8 == null)
                        {
                            Excel.o__SiteContainer1.p__Site8 = CallSite<Func<CallSite, object, double>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(double), typeof(Excel)));
                        }
                        double num5 = Excel.o__SiteContainer1.p__Site8.Target(Excel.o__SiteContainer1.p__Site8, range5.get_Value(Missing.Value));
                        if (item.Section == "Diff" && num5 >= -1.0 && num5 <= 1.0)
                        {
                            if (Excel.o__SiteContainer1.p__Site9 == null)
                            {
                                Excel.o__SiteContainer1.p__Site9 = CallSite<Func<CallSite, object, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Excel), new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                }));
                            }
                            Func<CallSite, object, object, object, object> target = Excel.o__SiteContainer1.p__Site9.Target;
                            CallSite p__Site = Excel.o__SiteContainer1.p__Site9;
                            if (Excel.o__SiteContainer1.p__Sitea == null)
                            {
                                Excel.o__SiteContainer1.p__Sitea = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof(Excel), new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                }));
                            }
                            object obj = target(p__Site, Excel.o__SiteContainer1.p__Sitea.Target(Excel.o__SiteContainer1.p__Sitea, worksheet), worksheet.Cells[num2, num3], worksheet.Cells[num2 + list.Max((IGrouping<string, DataLayer.Entities.ImageCalibration> x) => x.Count<DataLayer.Entities.ImageCalibration>()) + 2, num3]);
                            if (Excel.o__SiteContainer1.p__Siteb == null)
                            {
                                Excel.o__SiteContainer1.p__Siteb = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "Color", typeof(Excel), new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                                }));
                            }
                            Func<CallSite, object, int, object> target2 = Excel.o__SiteContainer1.p__Siteb.Target;
                            CallSite p__Siteb = Excel.o__SiteContainer1.p__Siteb;
                            if (Excel.o__SiteContainer1.p__Sitec == null)
                            {
                                Excel.o__SiteContainer1.p__Sitec = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof(Excel), new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                }));
                            }
                            target2(p__Siteb, Excel.o__SiteContainer1.p__Sitec.Target(Excel.o__SiteContainer1.p__Sitec, obj), ColorTranslator.ToOle(Color.LightGreen));
                            if (Excel.o__SiteContainer1.p__Sited == null)
                            {
                                Excel.o__SiteContainer1.p__Sited = CallSite<Action<CallSite, Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(Excel), new CSharpArgumentInfo[]
                                {
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                }));
                            }
                            Excel.o__SiteContainer1.p__Sited.Target(Excel.o__SiteContainer1.p__Sited, typeof(Marshal), obj);
                        }
                        num++;
                        Marshal.FinalReleaseComObject(range5);
                        if (num3 == 2)
                        {
                            if (Excel.o__SiteContainer1.p__Sitee == null)
                            {
                                Excel.o__SiteContainer1.p__Sitee = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                            }
                            Range range6 = Excel.o__SiteContainer1.p__Sitee.Target(Excel.o__SiteContainer1.p__Sitee, worksheet.Cells[num, num3 - 1]);
                            range6.set_Value(Missing.Value, "st dev");
                            Marshal.FinalReleaseComObject(range6);
                        }
                        if (Excel.o__SiteContainer1.p__Sitef == null)
                        {
                            Excel.o__SiteContainer1.p__Sitef = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                        }
                        Range range7 = Excel.o__SiteContainer1.p__Sitef.Target(Excel.o__SiteContainer1.p__Sitef, worksheet.Cells[num, num3]);
                        range7.Formula = string.Concat(new object[]
                        {
                            "=STDEV(",
                            Excel.IndexToColumnName(num3),
                            num2 + 1,
                            ":",
                            Excel.IndexToColumnName(num3),
                            num2 + grouping.Count<DataLayer.Entities.ImageCalibration>(),
                            ")"
                        });
                        Marshal.FinalReleaseComObject(range7);
                        num3++;
                    }
                    int num6 = num2;
                    int num7;
                    if (list.Count != 0)
                    {
                        num7 = list.Max((IGrouping<string, DataLayer.Entities.ImageCalibration> x) => x.Count<DataLayer.Entities.ImageCalibration>());
                    }
                    else
                    {
                        num7 = 0;
                    }
                    num = num6 + num7 + 4;
                    if (reportProgress != null)
                    {
                        reportProgress(string.Format("Writing Property {0} {1}: {2} of {3}", new object[]
                        {
                            item.Section,
                            item.Threshold,
                            Excel.Properties.IndexOf(item),
                            Excel.Properties.Count
                        }), (int)((double)Excel.Properties.IndexOf(item) / (double)Excel.Properties.Count * 100.0));
                    }
                }
                Marshal.FinalReleaseComObject(worksheet);
                List<DataLayer.Entities.ImageCalibration> list2 = (from x in list
                                                                   select x.First<DataLayer.Entities.ImageCalibration>()).ToList<DataLayer.Entities.ImageCalibration>();
                foreach (DataLayer.Entities.ImageCalibration calibration2 in list2)
                {
                    Excel.CreateWorksheetHistogramFromCalibration(calibration2, workbook);
                    if (reportProgress != null)
                    {
                        reportProgress(string.Format("Generating Histogram {0} of {1}", list2.IndexOf(calibration2), list2.Count), (int)((double)list2.IndexOf(calibration2) / (double)list2.Count * 100.0));
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("There was an error generating the spreadsheet.", ex);
            }
            finally
            {
                workbook.SaveAs(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close(false, Type.Missing, Type.Missing);
                Marshal.FinalReleaseComObject(workbook);
                application2.Quit();
            }
        }

        // Token: 0x0600018E RID: 398 RVA: 0x0000E000 File Offset: 0x0000C200
        private static void CreateWorksheetHistogramFromCalibration(DataLayer.Entities.ImageCalibration result, Workbook workbook)
        {
            Worksheet after = workbook.Worksheets.OfType<Worksheet>().Last<Worksheet>();
            if (Excel.o__SiteContainer1e.p__Site1f == null)
            {
                Excel.o__SiteContainer1e.p__Site1f = CallSite<Func<CallSite, object, Worksheet>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Worksheet), typeof(Excel)));
            }
            Worksheet worksheet = Excel.o__SiteContainer1e.p__Site1f.Target(Excel.o__SiteContainer1e.p__Site1f, workbook.Worksheets.Add(Type.Missing, after, Missing.Value, Missing.Value));
            DataLayer.Entities.Capture capture = result.Image.Capture.Capture;
            worksheet.Name = string.Format("B{0}G{1}", capture.Brightness, capture.Gain);
            if (Excel.o__SiteContainer1e.p__Site20 == null)
            {
                Excel.o__SiteContainer1e.p__Site20 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
            }
            Excel.o__SiteContainer1e.p__Site20.Target(Excel.o__SiteContainer1e.p__Site20, worksheet.Cells[1, "A"]).set_Value(Missing.Value, "Input Image");
            if (Excel.o__SiteContainer1e.p__Site21 == null)
            {
                Excel.o__SiteContainer1e.p__Site21 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            Range range = Excel.o__SiteContainer1e.p__Site21.Target(Excel.o__SiteContainer1e.p__Site21, worksheet.Cells[2, "A"]);
            range.set_Value(Missing.Value, "X");
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range);
            if (Excel.o__SiteContainer1e.p__Site22 == null)
            {
                Excel.o__SiteContainer1e.p__Site22 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            Range range2 = Excel.o__SiteContainer1e.p__Site22.Target(Excel.o__SiteContainer1e.p__Site22, worksheet.Cells[2, "B"]);
            range2.set_Value(Missing.Value, "Y");
            range2.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range2);
            if (Excel.o__SiteContainer1e.p__Site23 == null)
            {
                Excel.o__SiteContainer1e.p__Site23 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            Range range3 = Excel.o__SiteContainer1e.p__Site23.Target(Excel.o__SiteContainer1e.p__Site23, worksheet.Cells[2, "C"]);
            range3.set_Value(Missing.Value, "Z");
            range3.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range3);
            int[,] array;
            int j;
            object obj2;
            using (Bitmap bitmap = Compression.DecompressImage(result.Image.ImageData))
            {
                array = new int[bitmap.Width * bitmap.Height, 3];
                using (Image<Bgr, byte> image = bitmap.ToImage<Bgr, byte>())
                {
                    using (Image<Bgr, byte> image2 = Compression.DecompressImage(result.Colorized.ImageData).ToImage<Bgr, byte>())
                    {
                        for (int i = 0; i < image.Height; i++)
                        {
                            for (j = 0; j < image.Width; j++)
                            {
                                int num = i * image.Width + j;
                                array[num, 0] = j;
                                array[num, 1] = i;
                                array[num, 2] = (int)image[i, j].Red;
                                int num2 = (int)image2[i, j].Red;
                                int num3 = (int)image2[i, j].Green;
                                int num4 = (int)image2[i, j].Blue;
                                if (num2 != num3 || num3 != num4)
                                {
                                    if (Excel.o__SiteContainer1e.p__Site24 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site24 = CallSite<Func<CallSite, object, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                        }));
                                    }
                                    Func<CallSite, object, object, object, object> target = Excel.o__SiteContainer1e.p__Site24.Target;
                                    CallSite p__Site = Excel.o__SiteContainer1e.p__Site24;
                                    if (Excel.o__SiteContainer1e.p__Site25 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site25 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                        }));
                                    }
                                    object obj = target(p__Site, Excel.o__SiteContainer1e.p__Site25.Target(Excel.o__SiteContainer1e.p__Site25, worksheet), worksheet.Cells[num, "A"], worksheet.Cells[num, "C"]);
                                    Color c = Color.FromArgb((int)image2[i, j].Red, (int)image2[i, j].Green, (int)image2[i, j].Blue);
                                    if (Excel.o__SiteContainer1e.p__Site26 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site26 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "Color", typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                                        }));
                                    }
                                    Func<CallSite, object, int, object> target2 = Excel.o__SiteContainer1e.p__Site26.Target;
                                    CallSite p__Site2 = Excel.o__SiteContainer1e.p__Site26;
                                    if (Excel.o__SiteContainer1e.p__Site27 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site27 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Interior", typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                        }));
                                    }
                                    target2(p__Site2, Excel.o__SiteContainer1e.p__Site27.Target(Excel.o__SiteContainer1e.p__Site27, obj), ColorTranslator.ToOle(c));
                                    Color c2 = Color.FromArgb((int)(~c.R), (int)(~c.G), (int)(~c.B));
                                    if (Excel.o__SiteContainer1e.p__Site28 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site28 = CallSite<Func<CallSite, object, int, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "Color", typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                                        }));
                                    }
                                    Func<CallSite, object, int, object> target3 = Excel.o__SiteContainer1e.p__Site28.Target;
                                    CallSite p__Site3 = Excel.o__SiteContainer1e.p__Site28;
                                    if (Excel.o__SiteContainer1e.p__Site29 == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site29 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Font", typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                        }));
                                    }
                                    target3(p__Site3, Excel.o__SiteContainer1e.p__Site29.Target(Excel.o__SiteContainer1e.p__Site29, obj), ColorTranslator.ToOle(c2));
                                    if (Excel.o__SiteContainer1e.p__Site2a == null)
                                    {
                                        Excel.o__SiteContainer1e.p__Site2a = CallSite<Action<CallSite, Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(Excel), new CSharpArgumentInfo[]
                                        {
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                                        }));
                                    }
                                    Excel.o__SiteContainer1e.p__Site2a.Target(Excel.o__SiteContainer1e.p__Site2a, typeof(Marshal), obj);
                                }
                            }
                        }
                        if (Excel.o__SiteContainer1e.p__Site2b == null)
                        {
                            Excel.o__SiteContainer1e.p__Site2b = CallSite<Func<CallSite, object, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Excel), new CSharpArgumentInfo[]
                            {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                            }));
                        }
                        Func<CallSite, object, object, object, object> target4 = Excel.o__SiteContainer1e.p__Site2b.Target;
                        CallSite p__Site2b = Excel.o__SiteContainer1e.p__Site2b;
                        if (Excel.o__SiteContainer1e.p__Site2c == null)
                        {
                            Excel.o__SiteContainer1e.p__Site2c = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof(Excel), new CSharpArgumentInfo[]
                            {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                            }));
                        }
                        obj2 = target4(p__Site2b, Excel.o__SiteContainer1e.p__Site2c.Target(Excel.o__SiteContainer1e.p__Site2c, worksheet), worksheet.Cells[3, "A"], worksheet.Cells[array.GetLength(0) + 2, "C"]);
                        if (Excel.o__SiteContainer1e.p__Site2d == null)
                        {
                            Excel.o__SiteContainer1e.p__Site2d = CallSite<Func<CallSite, object, int[,], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "Value", typeof(Excel), new CSharpArgumentInfo[]
                            {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                            }));
                        }
                        Excel.o__SiteContainer1e.p__Site2d.Target(Excel.o__SiteContainer1e.p__Site2d, obj2, array);
                        if (Excel.o__SiteContainer1e.p__Site2e == null)
                        {
                            Excel.o__SiteContainer1e.p__Site2e = CallSite<Action<CallSite, Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(Excel), new CSharpArgumentInfo[]
                            {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                            }));
                        }
                        Excel.o__SiteContainer1e.p__Site2e.Target(Excel.o__SiteContainer1e.p__Site2e, typeof(Marshal), obj2);
                    }
                }
                if (Excel.o__SiteContainer1e.p__Site2f == null)
                {
                    Excel.o__SiteContainer1e.p__Site2f = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
                }
                Excel.o__SiteContainer1e.p__Site2f.Target(Excel.o__SiteContainer1e.p__Site2f, worksheet.Cells[1, "E"]).set_Value(Missing.Value, "Input Frequency");
                if (Excel.o__SiteContainer1e.p__Site30 == null)
                {
                    Excel.o__SiteContainer1e.p__Site30 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                range = Excel.o__SiteContainer1e.p__Site30.Target(Excel.o__SiteContainer1e.p__Site30, worksheet.Cells[2, "E"]);
                range.set_Value(Missing.Value, "Frequency");
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                Marshal.FinalReleaseComObject(range);
                if (Excel.o__SiteContainer1e.p__Site31 == null)
                {
                    Excel.o__SiteContainer1e.p__Site31 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                range2 = Excel.o__SiteContainer1e.p__Site31.Target(Excel.o__SiteContainer1e.p__Site31, worksheet.Cells[2, "F"]);
                range2.set_Value(Missing.Value, "Diff");
                range2.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                Marshal.FinalReleaseComObject(range2);
                if (Excel.o__SiteContainer1e.p__Site32 == null)
                {
                    Excel.o__SiteContainer1e.p__Site32 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                range3 = Excel.o__SiteContainer1e.p__Site32.Target(Excel.o__SiteContainer1e.p__Site32, worksheet.Cells[2, "G"]);
                range3.set_Value(Missing.Value, "Key");
                range3.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                Marshal.FinalReleaseComObject(range3);
                for (int k = 1; k <= 25; k++)
                {
                    if (Excel.o__SiteContainer1e.p__Site33 == null)
                    {
                        Excel.o__SiteContainer1e.p__Site33 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                    }
                    Range range4 = Excel.o__SiteContainer1e.p__Site33.Target(Excel.o__SiteContainer1e.p__Site33, worksheet.Cells[k + 2, "E"]);
                    range4.Formula = string.Concat(new object[]
                    {
                        "=FREQUENCY(C3:C",
                        array.GetLength(0) + 2,
                        ",G",
                        k + 2,
                        ")"
                    });
                    Marshal.FinalReleaseComObject(range4);
                    if (Excel.o__SiteContainer1e.p__Site34 == null)
                    {
                        Excel.o__SiteContainer1e.p__Site34 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                    }
                    Range range5 = Excel.o__SiteContainer1e.p__Site34.Target(Excel.o__SiteContainer1e.p__Site34, worksheet.Cells[k + 2, "F"]);
                    if (k > 1)
                    {
                        range5.Formula = string.Concat(new object[]
                        {
                            "=E",
                            k + 2,
                            "-E",
                            k + 1
                        });
                    }
                    else
                    {
                        range5.Formula = "=E" + (k + 2);
                    }
                    Marshal.FinalReleaseComObject(range5);
                    if (Excel.o__SiteContainer1e.p__Site35 == null)
                    {
                        Excel.o__SiteContainer1e.p__Site35 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                    }
                    Range range6 = Excel.o__SiteContainer1e.p__Site35.Target(Excel.o__SiteContainer1e.p__Site35, worksheet.Cells[k + 2, "G"]);
                    range6.set_Value(Missing.Value, k * 10);
                    Marshal.FinalReleaseComObject(range6);
                }
            }
            if (Excel.o__SiteContainer1e.p__Site36 == null)
            {
                Excel.o__SiteContainer1e.p__Site36 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
            }
            Excel.o__SiteContainer1e.p__Site36.Target(Excel.o__SiteContainer1e.p__Site36, worksheet.Cells[1, "I"]).set_Value(Missing.Value, "Average Image");
            if (Excel.o__SiteContainer1e.p__Site37 == null)
            {
                Excel.o__SiteContainer1e.p__Site37 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range = Excel.o__SiteContainer1e.p__Site37.Target(Excel.o__SiteContainer1e.p__Site37, worksheet.Cells[2, "I"]);
            range.set_Value(Missing.Value, "X");
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range);
            if (Excel.o__SiteContainer1e.p__Site38 == null)
            {
                Excel.o__SiteContainer1e.p__Site38 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range2 = Excel.o__SiteContainer1e.p__Site38.Target(Excel.o__SiteContainer1e.p__Site38, worksheet.Cells[2, "J"]);
            range2.set_Value(Missing.Value, "Y");
            range2.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range2);
            if (Excel.o__SiteContainer1e.p__Site39 == null)
            {
                Excel.o__SiteContainer1e.p__Site39 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range3 = Excel.o__SiteContainer1e.p__Site39.Target(Excel.o__SiteContainer1e.p__Site39, worksheet.Cells[2, "K"]);
            range3.set_Value(Missing.Value, "Z");
            range3.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range3);
            object[] array2 = (object[])Maths.LoadMatFile(Maths.BaseCalibration);
            object[,,] array3 = (object[,,])((object[,,])array2[0])[1, 0, 0];
            double[,] array4 = (double[,])array3[0, 0, 0];
            double num5 = (double)array4.GetLength(0) * 0.5;
            double num6 = (double)array4.GetLength(1) * 0.5;
            array = new int[(int)num6 * (int)num5, 3];
            j = 0;
            while ((double)j < num6)
            {
                int i = 0;
                while ((double)i < num5)
                {
                    int num7 = j + (int)(0.5 * num6);
                    int num8 = i + (int)(0.5 * num5);
                    double num9 = (double)i * num6 + (double)j;
                    array[(int)num9, 0] = j;
                    array[(int)num9, 1] = i;
                    array[(int)num9, 2] = (int)array4[num8, num7];
                    i++;
                }
                j++;
            }
            if (Excel.o__SiteContainer1e.p__Site3a == null)
            {
                Excel.o__SiteContainer1e.p__Site3a = CallSite<Func<CallSite, object, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetIndex(CSharpBinderFlags.None, typeof(Excel), new CSharpArgumentInfo[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                }));
            }
            Func<CallSite, object, object, object, object> target5 = Excel.o__SiteContainer1e.p__Site3a.Target;
            CallSite p__Site3a = Excel.o__SiteContainer1e.p__Site3a;
            if (Excel.o__SiteContainer1e.p__Site3b == null)
            {
                Excel.o__SiteContainer1e.p__Site3b = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.ResultIndexed, "Range", typeof(Excel), new CSharpArgumentInfo[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                }));
            }
            obj2 = target5(p__Site3a, Excel.o__SiteContainer1e.p__Site3b.Target(Excel.o__SiteContainer1e.p__Site3b, worksheet), worksheet.Cells[3, "I"], worksheet.Cells[array.GetLength(0) + 2, "K"]);
            if (Excel.o__SiteContainer1e.p__Site3c == null)
            {
                Excel.o__SiteContainer1e.p__Site3c = CallSite<Func<CallSite, object, int[,], object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.SetMember(CSharpBinderFlags.None, "Value", typeof(Excel), new CSharpArgumentInfo[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                }));
            }
            Excel.o__SiteContainer1e.p__Site3c.Target(Excel.o__SiteContainer1e.p__Site3c, obj2, array);
            if (Excel.o__SiteContainer1e.p__Site3d == null)
            {
                Excel.o__SiteContainer1e.p__Site3d = CallSite<Action<CallSite, Type, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "FinalReleaseComObject", null, typeof(Excel), new CSharpArgumentInfo[]
                {
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                }));
            }
            Excel.o__SiteContainer1e.p__Site3d.Target(Excel.o__SiteContainer1e.p__Site3d, typeof(Marshal), obj2);
            if (Excel.o__SiteContainer1e.p__Site3e == null)
            {
                Excel.o__SiteContainer1e.p__Site3e = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
            }
            Excel.o__SiteContainer1e.p__Site3e.Target(Excel.o__SiteContainer1e.p__Site3e, worksheet.Cells[1, "M"]).set_Value(Missing.Value, "Average Frequency");
            if (Excel.o__SiteContainer1e.p__Site3f == null)
            {
                Excel.o__SiteContainer1e.p__Site3f = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range = Excel.o__SiteContainer1e.p__Site3f.Target(Excel.o__SiteContainer1e.p__Site3f, worksheet.Cells[2, "M"]);
            range.set_Value(Missing.Value, "Frequency");
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range);
            if (Excel.o__SiteContainer1e.p__Site40 == null)
            {
                Excel.o__SiteContainer1e.p__Site40 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range2 = Excel.o__SiteContainer1e.p__Site40.Target(Excel.o__SiteContainer1e.p__Site40, worksheet.Cells[2, "N"]);
            range2.set_Value(Missing.Value, "Diff");
            range2.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range2);
            if (Excel.o__SiteContainer1e.p__Site41 == null)
            {
                Excel.o__SiteContainer1e.p__Site41 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range3 = Excel.o__SiteContainer1e.p__Site41.Target(Excel.o__SiteContainer1e.p__Site41, worksheet.Cells[2, "O"]);
            range3.set_Value(Missing.Value, "Key");
            range3.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range3);
            for (int k = 1; k <= 25; k++)
            {
                if (Excel.o__SiteContainer1e.p__Site42 == null)
                {
                    Excel.o__SiteContainer1e.p__Site42 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range4 = Excel.o__SiteContainer1e.p__Site42.Target(Excel.o__SiteContainer1e.p__Site42, worksheet.Cells[k + 2, "M"]);
                range4.Formula = string.Concat(new object[]
                {
                    "=FREQUENCY(K3:K",
                    array.GetLength(0) + 2,
                    ",O",
                    k + 2,
                    ")"
                });
                Marshal.FinalReleaseComObject(range4);
                if (Excel.o__SiteContainer1e.p__Site43 == null)
                {
                    Excel.o__SiteContainer1e.p__Site43 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range5 = Excel.o__SiteContainer1e.p__Site43.Target(Excel.o__SiteContainer1e.p__Site43, worksheet.Cells[k + 2, "N"]);
                if (k > 1)
                {
                    range5.Formula = string.Concat(new object[]
                    {
                        "=M",
                        k + 2,
                        "-M",
                        k + 1
                    });
                }
                else
                {
                    range5.Formula = "=M" + (k + 2);
                }
                Marshal.FinalReleaseComObject(range5);
                if (Excel.o__SiteContainer1e.p__Site44 == null)
                {
                    Excel.o__SiteContainer1e.p__Site44 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range6 = Excel.o__SiteContainer1e.p__Site44.Target(Excel.o__SiteContainer1e.p__Site44, worksheet.Cells[k + 2, "O"]);
                range6.set_Value(Missing.Value, k * 10);
                Marshal.FinalReleaseComObject(range6);
            }
            if (Excel.o__SiteContainer1e.p__Site45 == null)
            {
                Excel.o__SiteContainer1e.p__Site45 = CallSite<Func<CallSite, object, ChartObjects>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(ChartObjects), typeof(Excel)));
            }
            ChartObjects chartObjects = Excel.o__SiteContainer1e.p__Site45.Target(Excel.o__SiteContainer1e.p__Site45, worksheet.ChartObjects(Missing.Value));
            ChartObject chartObject = chartObjects.Add(1000.0, 80.0, 400.0, 250.0);
            chartObject.Name = "Frequency";
            Chart chart = chartObject.Chart;
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartTitle.Caption = "Frequency of Input and Baseline Images";
            chart.ChartType = XlChartType.xlLineMarkers;
            if (Excel.o__SiteContainer1e.p__Site46 == null)
            {
                Excel.o__SiteContainer1e.p__Site46 = CallSite<Func<CallSite, object, Axis>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Axis), typeof(Excel)));
            }
            Axis axis = Excel.o__SiteContainer1e.p__Site46.Target(Excel.o__SiteContainer1e.p__Site46, chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary));
            axis.HasTitle = true;
            axis.AxisTitle.Caption = "Count";
            if (Excel.o__SiteContainer1e.p__Site47 == null)
            {
                Excel.o__SiteContainer1e.p__Site47 = CallSite<Func<CallSite, object, Axis>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Axis), typeof(Excel)));
            }
            Axis axis2 = Excel.o__SiteContainer1e.p__Site47.Target(Excel.o__SiteContainer1e.p__Site47, chart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary));
            axis2.HasTitle = true;
            axis2.AxisTitle.Caption = "Intensity";
            Marshal.FinalReleaseComObject(axis2);
            Marshal.FinalReleaseComObject(axis);
            if (Excel.o__SiteContainer1e.p__Site48 == null)
            {
                Excel.o__SiteContainer1e.p__Site48 = CallSite<Func<CallSite, object, SeriesCollection>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(SeriesCollection), typeof(Excel)));
            }
            Series series = Excel.o__SiteContainer1e.p__Site48.Target(Excel.o__SiteContainer1e.p__Site48, chart.SeriesCollection(Missing.Value)).NewSeries();
            series.Formula = string.Concat(new string[]
            {
                "=SERIES(",
                worksheet.Name,
                "!$E$1,",
                worksheet.Name,
                "!$G$3:$G$27,",
                worksheet.Name,
                "!$F$3:$F$27,1)"
            });
            if (Excel.o__SiteContainer1e.p__Site49 == null)
            {
                Excel.o__SiteContainer1e.p__Site49 = CallSite<Func<CallSite, object, SeriesCollection>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(SeriesCollection), typeof(Excel)));
            }
            Series series2 = Excel.o__SiteContainer1e.p__Site49.Target(Excel.o__SiteContainer1e.p__Site49, chart.SeriesCollection(Missing.Value)).NewSeries();
            series2.Formula = string.Concat(new string[]
            {
                "=SERIES(",
                worksheet.Name,
                "!$M$1,",
                worksheet.Name,
                "!$O$3:$O$27,",
                worksheet.Name,
                "!$N$3:$N$27,1)"
            });
            Marshal.FinalReleaseComObject(series2);
            Marshal.FinalReleaseComObject(series);
            Marshal.FinalReleaseComObject(chart);
            Marshal.FinalReleaseComObject(chartObject);
            if (Excel.o__SiteContainer1e.p__Site4a == null)
            {
                Excel.o__SiteContainer1e.p__Site4a = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(Range), typeof(Excel)));
            }
            Excel.o__SiteContainer1e.p__Site4a.Target(Excel.o__SiteContainer1e.p__Site4a, worksheet.Cells[1, "Q"]).set_Value(Missing.Value, "Input/Average");
            if (Excel.o__SiteContainer1e.p__Site4b == null)
            {
                Excel.o__SiteContainer1e.p__Site4b = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range = Excel.o__SiteContainer1e.p__Site4b.Target(Excel.o__SiteContainer1e.p__Site4b, worksheet.Cells[2, "Q"]);
            range.set_Value(Missing.Value, "Input");
            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range);
            if (Excel.o__SiteContainer1e.p__Site4c == null)
            {
                Excel.o__SiteContainer1e.p__Site4c = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range2 = Excel.o__SiteContainer1e.p__Site4c.Target(Excel.o__SiteContainer1e.p__Site4c, worksheet.Cells[2, "R"]);
            range2.set_Value(Missing.Value, "Average");
            range2.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range2);
            if (Excel.o__SiteContainer1e.p__Site4d == null)
            {
                Excel.o__SiteContainer1e.p__Site4d = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
            }
            range3 = Excel.o__SiteContainer1e.p__Site4d.Target(Excel.o__SiteContainer1e.p__Site4d, worksheet.Cells[2, "S"]);
            range3.set_Value(Missing.Value, "Intensity");
            range3.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Marshal.FinalReleaseComObject(range3);
            for (int k = 1; k <= 25; k++)
            {
                if (Excel.o__SiteContainer1e.p__Site4e == null)
                {
                    Excel.o__SiteContainer1e.p__Site4e = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range7 = Excel.o__SiteContainer1e.p__Site4e.Target(Excel.o__SiteContainer1e.p__Site4e, worksheet.Cells[k + 2, "Q"]);
                range7.set_Value(Missing.Value, string.Concat(new object[]
                {
                    "=F",
                    k + 2,
                    "/N",
                    k + 2
                }));
                Marshal.FinalReleaseComObject(range7);
                if (Excel.o__SiteContainer1e.p__Site4f == null)
                {
                    Excel.o__SiteContainer1e.p__Site4f = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range8 = Excel.o__SiteContainer1e.p__Site4f.Target(Excel.o__SiteContainer1e.p__Site4f, worksheet.Cells[k + 2, "R"]);
                range8.Formula = string.Concat(new object[]
                {
                    "=N",
                    k + 2,
                    "/F",
                    k + 2
                });
                Marshal.FinalReleaseComObject(range8);
                if (Excel.o__SiteContainer1e.p__Site50 == null)
                {
                    Excel.o__SiteContainer1e.p__Site50 = CallSite<Func<CallSite, object, Range>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Range), typeof(Excel)));
                }
                Range range6 = Excel.o__SiteContainer1e.p__Site50.Target(Excel.o__SiteContainer1e.p__Site50, worksheet.Cells[k + 2, "S"]);
                range6.set_Value(Missing.Value, k * 10);
                Marshal.FinalReleaseComObject(range6);
            }
            chartObject = chartObjects.Add(1000.0, 350.0, 600.0, 250.0);
            chartObject.Name = "InputAverage";
            chart = chartObject.Chart;
            chart.HasTitle = true;
            chart.HasLegend = true;
            chart.ChartTitle.Caption = "Input Frequency Over Average Ratio";
            chart.ChartType = XlChartType.xlLineMarkers;
            if (Excel.o__SiteContainer1e.p__Site51 == null)
            {
                Excel.o__SiteContainer1e.p__Site51 = CallSite<Func<CallSite, object, Axis>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Axis), typeof(Excel)));
            }
            axis = Excel.o__SiteContainer1e.p__Site51.Target(Excel.o__SiteContainer1e.p__Site51, chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary));
            axis.HasTitle = true;
            axis.AxisTitle.Caption = "Ratio";
            if (Excel.o__SiteContainer1e.p__Site52 == null)
            {
                Excel.o__SiteContainer1e.p__Site52 = CallSite<Func<CallSite, object, Axis>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(Axis), typeof(Excel)));
            }
            axis2 = Excel.o__SiteContainer1e.p__Site52.Target(Excel.o__SiteContainer1e.p__Site52, chart.Axes(XlAxisType.xlCategory, XlAxisGroup.xlPrimary));
            axis2.HasTitle = true;
            axis2.AxisTitle.Caption = "Intensity";
            Marshal.FinalReleaseComObject(axis2);
            Marshal.FinalReleaseComObject(axis);
            if (Excel.o__SiteContainer1e.p__Site53 == null)
            {
                Excel.o__SiteContainer1e.p__Site53 = CallSite<Func<CallSite, object, SeriesCollection>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(SeriesCollection), typeof(Excel)));
            }
            Series series3 = Excel.o__SiteContainer1e.p__Site53.Target(Excel.o__SiteContainer1e.p__Site53, chart.SeriesCollection(Missing.Value)).NewSeries();
            series3.Formula = string.Concat(new string[]
            {
                "=SERIES(",
                worksheet.Name,
                "!$R$2,",
                worksheet.Name,
                "!$S$3:$S$27,",
                worksheet.Name,
                "!$R$3:$R$27,1)"
            });
            Marshal.FinalReleaseComObject(series3);
            Marshal.FinalReleaseComObject(chart);
            Marshal.FinalReleaseComObject(chartObject);
            Marshal.FinalReleaseComObject(chartObjects);
            Marshal.FinalReleaseComObject(worksheet);
        }

        // Token: 0x0600018F RID: 399 RVA: 0x00010008 File Offset: 0x0000E208
        private static string IndexToColumnName(int columnNumber)
        {
            int i = columnNumber;
            string text = string.Empty;
            while (i > 0)
            {
                int num = (i - 1) % 26;
                text = Convert.ToChar(65 + num).ToString(CultureInfo.InvariantCulture) + text;
                i = (i - num) / 26;
            }
            return text;
        }

        // Token: 0x06000190 RID: 400 RVA: 0x00010060 File Offset: 0x0000E260
        public static void CreateCSVFromImageCalibrations(List<DataLayer.Entities.ImageCalibration> results, string filename, Action<string, int> reportProgress = null)
        {
            if (reportProgress != null)
            {
                reportProgress("Writing CVS Header", 0);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Section Results,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,Mean Threshold Results,,,,,,,,,Intensity,,,,\n,,,,,Outer,,,,Inner,,,,Corona,,,,High P,,,,Total,,,,Clumps,,,,,Outer,,Inner,,Corona,,High P,,,Outer,Inner,Corona,HighP,Total\nImage Filename,Brightness,Gain,Voltage,Noise Level,Failed Pixels,Total Pixels,% Failed,Status,Failed Pixels,Total Pixels,% Failed,Status,Failed Pixels,Total Pixels,% Failed,Status,Failed Pixels,Total Pixels,% Failed,Status,Failed Pixels,Total Pixels,% Failed,Status,Failed Pixels,Total Pixels,% Failed,Status,,Diff,Status,Diff,Status,Diff,Status,Diff,Status,,,,,,");
            int num = 0;
            foreach (DataLayer.Entities.ImageCalibration imageCalibration in results)
            {
                string text = string.Format("CalibrationImage#{0}.bmp", results.IndexOf(imageCalibration));
                double num2 = double.NaN;
                double num3 = double.NaN;
                double num4 = double.NaN;
                DataLayer.Entities.ImageCapture capture = imageCalibration.Image.Capture;
                if (capture != null)
                {
                    num2 = (double)capture.Capture.Brightness;
                    num3 = (double)capture.Capture.Gain;
                    num4 = (double)capture.Capture.Voltage;
                    text = string.Format("B={1}G={2}_CalibrationImage#{0}.bmp", results.IndexOf(imageCalibration), num2, num3);
                }
                stringBuilder.AppendLine(string.Concat(new object[]
                {
                    text,
                    ",",
                    num2,
                    ",",
                    num3,
                    ",",
                    num4,
                    ",",
                    imageCalibration.NoiseLevel,
                    ",",
                    imageCalibration.OuterFailures,
                    ",",
                    imageCalibration.OuterTotalPixels,
                    ",",
                    imageCalibration.OuterFailurePercent,
                    ",",
                    imageCalibration.OuterFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.InnerFailures,
                    ",",
                    imageCalibration.InnerTotalPixels,
                    ",",
                    imageCalibration.InnerFailurePercent,
                    ",",
                    imageCalibration.InnerFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.CoronaFailures,
                    ",",
                    imageCalibration.CoronaTotalPixels,
                    ",",
                    imageCalibration.CoronaFailurePercent,
                    ",",
                    imageCalibration.CoronaFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.HighPFailures,
                    ",",
                    imageCalibration.HighPTotalPixels,
                    ",",
                    imageCalibration.HighPFailurePercent,
                    ",",
                    imageCalibration.HighPFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.TotalFailures,
                    ",",
                    imageCalibration.TotalTotalPixels,
                    ",",
                    imageCalibration.TotalFailurePercent,
                    ",",
                    imageCalibration.TotalFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.ClumpsFailures,
                    ",",
                    imageCalibration.ClumpsTotalPixels,
                    ",",
                    imageCalibration.ClumpsFailurePercent,
                    ",",
                    imageCalibration.ClumpsFailed ? "Failed" : "Passed",
                    ",,",
                    imageCalibration.OuterMeanDiff,
                    ",",
                    imageCalibration.OuterMeanFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.InnerMeanDiff,
                    ",",
                    imageCalibration.InnerMeanFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.CoronaMeanDiff,
                    ",",
                    imageCalibration.CoronaMeanFailed ? "Failed" : "Passed",
                    ",",
                    imageCalibration.HighPMeanDiff,
                    ",",
                    imageCalibration.HighPMeanFailed ? "Failed" : "Passed",
                    ",,",
                    imageCalibration.IntensityInner,
                    ",",
                    imageCalibration.IntensityOuter,
                    ",",
                    imageCalibration.IntensityCorona,
                    ",",
                    imageCalibration.IntensityHighP,
                    ",",
                    imageCalibration.IntensityTotal
                }));
                num++;
                if (reportProgress != null)
                {
                    reportProgress(string.Format("Writing Image Data ({0} of {1})", num, results.Count), (int)((double)num / (double)(results.Count + 1) * 100.0));
                }
            }
            if (reportProgress != null)
            {
                reportProgress("Saving Calibration Data to CVS", 100);
            }
            FileStream fileStream = new FileStream(filename, FileMode.CreateNew, FileAccess.ReadWrite);
            byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        // Token: 0x06000198 RID: 408 RVA: 0x000107F4 File Offset: 0x0000E9F4
        // Note: this type is marked as 'beforefieldinit'.
        static Excel()
        {
            List<Excel.SummarySections> list = new List<Excel.SummarySections>();
            List<Excel.SummarySections> list2 = list;
            Excel.SummarySections item = default(Excel.SummarySections);
            item.Property = ((DataLayer.Entities.ImageCalibration x) => x.OuterFailures);
            item.Section = "Failed Pixels";
            item.Threshold = "Outer";
            list2.Add(item);
            List<Excel.SummarySections> list3 = list;
            Excel.SummarySections item2 = default(Excel.SummarySections);
            item2.Property = ((DataLayer.Entities.ImageCalibration x) => x.InnerFailures);
            item2.Section = "Failed Pixels";
            item2.Threshold = "Inner";
            list3.Add(item2);
            List<Excel.SummarySections> list4 = list;
            Excel.SummarySections item3 = default(Excel.SummarySections);
            item3.Property = ((DataLayer.Entities.ImageCalibration x) => x.CoronaFailures);
            item3.Section = "Failed Pixels";
            item3.Threshold = "Corona";
            list4.Add(item3);
            List<Excel.SummarySections> list5 = list;
            Excel.SummarySections item4 = default(Excel.SummarySections);
            item4.Property = ((DataLayer.Entities.ImageCalibration x) => x.HighPFailures);
            item4.Section = "Failed Pixels";
            item4.Threshold = "High P";
            list5.Add(item4);
            List<Excel.SummarySections> list6 = list;
            Excel.SummarySections item5 = default(Excel.SummarySections);
            item5.Property = ((DataLayer.Entities.ImageCalibration x) => x.TotalFailures);
            item5.Section = "Failed Pixels";
            item5.Threshold = "Total";
            list6.Add(item5);
            List<Excel.SummarySections> list7 = list;
            Excel.SummarySections item6 = default(Excel.SummarySections);
            item6.Property = ((DataLayer.Entities.ImageCalibration x) => x.OuterMeanDiff);
            item6.Section = "Diff";
            item6.Threshold = "Outer";
            list7.Add(item6);
            List<Excel.SummarySections> list8 = list;
            Excel.SummarySections item7 = default(Excel.SummarySections);
            item7.Property = ((DataLayer.Entities.ImageCalibration x) => x.InnerMeanDiff);
            item7.Section = "Diff";
            item7.Threshold = "Inner";
            list8.Add(item7);
            List<Excel.SummarySections> list9 = list;
            Excel.SummarySections item8 = default(Excel.SummarySections);
            item8.Property = ((DataLayer.Entities.ImageCalibration x) => x.CoronaMeanDiff);
            item8.Section = "Diff";
            item8.Threshold = "Corona";
            list9.Add(item8);
            List<Excel.SummarySections> list10 = list;
            Excel.SummarySections item9 = default(Excel.SummarySections);
            item9.Property = ((DataLayer.Entities.ImageCalibration x) => x.HighPMeanDiff);
            item9.Section = "Diff";
            item9.Threshold = "HighP";
            list10.Add(item9);
            List<Excel.SummarySections> list11 = list;
            Excel.SummarySections item10 = default(Excel.SummarySections);
            item10.Property = ((DataLayer.Entities.ImageCalibration x) => x.IntensityOuter);
            item10.Section = "Intensity";
            item10.Threshold = "Outer";
            list11.Add(item10);
            List<Excel.SummarySections> list12 = list;
            Excel.SummarySections item11 = default(Excel.SummarySections);
            item11.Property = ((DataLayer.Entities.ImageCalibration x) => x.IntensityInner);
            item11.Section = "Intensity";
            item11.Threshold = "Inner";
            list12.Add(item11);
            List<Excel.SummarySections> list13 = list;
            Excel.SummarySections item12 = default(Excel.SummarySections);
            item12.Property = ((DataLayer.Entities.ImageCalibration x) => x.IntensityCorona);
            item12.Section = "Intensity";
            item12.Threshold = "Corona";
            list13.Add(item12);
            List<Excel.SummarySections> list14 = list;
            Excel.SummarySections item13 = default(Excel.SummarySections);
            item13.Property = ((DataLayer.Entities.ImageCalibration x) => x.IntensityHighP);
            item13.Section = "Intensity";
            item13.Threshold = "HighP";
            list14.Add(item13);
            Excel.Properties = list;
        }

        // Token: 0x040000D5 RID: 213
        private static readonly List<Excel.SummarySections> Properties;

        // Token: 0x0200001E RID: 30
        private struct SummarySections
        {
            // Token: 0x1700005F RID: 95
            // (get) Token: 0x060001A6 RID: 422 RVA: 0x00010C48 File Offset: 0x0000EE48
            // (set) Token: 0x060001A7 RID: 423 RVA: 0x00010C5F File Offset: 0x0000EE5F
            public Func<DataLayer.Entities.ImageCalibration, double> Property { get; set; }

            // Token: 0x17000060 RID: 96
            // (get) Token: 0x060001A8 RID: 424 RVA: 0x00010C68 File Offset: 0x0000EE68
            // (set) Token: 0x060001A9 RID: 425 RVA: 0x00010C7F File Offset: 0x0000EE7F
            public string Threshold { get; set; }

            // Token: 0x17000061 RID: 97
            // (get) Token: 0x060001AA RID: 426 RVA: 0x00010C88 File Offset: 0x0000EE88
            // (set) Token: 0x060001AB RID: 427 RVA: 0x00010C9F File Offset: 0x0000EE9F
            public string Section { get; set; }
        }

        // Token: 0x020000AA RID: 170
        [CompilerGenerated]
        private static class o__SiteContainer1

        {
            // Token: 0x04000241 RID: 577
            public static CallSite<Func<CallSite, object, Worksheet>> p__Site2;

            // Token: 0x04000242 RID: 578
            public static CallSite<Func<CallSite, object, Range>> p__Site3;

            // Token: 0x04000243 RID: 579
            public static CallSite<Func<CallSite, object, Range>> p__Site4;

            // Token: 0x04000244 RID: 580
            public static CallSite<Func<CallSite, object, Range>> p__Site5;

            // Token: 0x04000245 RID: 581
            public static CallSite<Func<CallSite, object, Range>> p__Site6;

            // Token: 0x04000246 RID: 582
            public static CallSite<Func<CallSite, object, Range>> p__Site7;

            // Token: 0x04000247 RID: 583
            public static CallSite<Func<CallSite, object, double>> p__Site8;

            // Token: 0x04000248 RID: 584
            public static CallSite<Func<CallSite, object, object, object, object>> p__Site9;

            // Token: 0x04000249 RID: 585
            public static CallSite<Func<CallSite, object, object>> p__Sitea;

            // Token: 0x0400024A RID: 586
            public static CallSite<Func<CallSite, object, int, object>> p__Siteb;

            // Token: 0x0400024B RID: 587
            public static CallSite<Func<CallSite, object, object>> p__Sitec;

            // Token: 0x0400024C RID: 588
            public static CallSite<Action<CallSite, Type, object>> p__Sited;

            // Token: 0x0400024D RID: 589
            public static CallSite<Func<CallSite, object, Range>> p__Sitee;

            // Token: 0x0400024E RID: 590
            public static CallSite<Func<CallSite, object, Range>> p__Sitef;
        }

        // Token: 0x020000B9 RID: 185
        [CompilerGenerated]
        private static class o__SiteContainer1e


        {
            // Token: 0x04000265 RID: 613
            public static CallSite<Func<CallSite, object, Worksheet>> p__Site1f;

            // Token: 0x04000266 RID: 614
            public static CallSite<Func<CallSite, object, Range>> p__Site20;

            // Token: 0x04000267 RID: 615
            public static CallSite<Func<CallSite, object, Range>> p__Site21;

            // Token: 0x04000268 RID: 616
            public static CallSite<Func<CallSite, object, Range>> p__Site22;

            // Token: 0x04000269 RID: 617
            public static CallSite<Func<CallSite, object, Range>> p__Site23;

            // Token: 0x0400026A RID: 618
            public static CallSite<Func<CallSite, object, object, object, object>> p__Site24;

            // Token: 0x0400026B RID: 619
            public static CallSite<Func<CallSite, object, object>> p__Site25;

            // Token: 0x0400026C RID: 620
            public static CallSite<Func<CallSite, object, int, object>> p__Site26;

            // Token: 0x0400026D RID: 621
            public static CallSite<Func<CallSite, object, object>> p__Site27;

            // Token: 0x0400026E RID: 622
            public static CallSite<Func<CallSite, object, int, object>> p__Site28;

            // Token: 0x0400026F RID: 623
            public static CallSite<Func<CallSite, object, object>> p__Site29;

            // Token: 0x04000270 RID: 624
            public static CallSite<Action<CallSite, Type, object>> p__Site2a;

            // Token: 0x04000271 RID: 625
            public static CallSite<Func<CallSite, object, object, object, object>> p__Site2b;

            // Token: 0x04000272 RID: 626
            public static CallSite<Func<CallSite, object, object>> p__Site2c;

            // Token: 0x04000273 RID: 627
            public static CallSite<Func<CallSite, object, int[,], object>> p__Site2d;

            // Token: 0x04000274 RID: 628
            public static CallSite<Action<CallSite, Type, object>> p__Site2e;

            // Token: 0x04000275 RID: 629
            public static CallSite<Func<CallSite, object, Range>> p__Site2f;

            // Token: 0x04000276 RID: 630
            public static CallSite<Func<CallSite, object, Range>> p__Site30;

            // Token: 0x04000277 RID: 631
            public static CallSite<Func<CallSite, object, Range>> p__Site31;

            // Token: 0x04000278 RID: 632
            public static CallSite<Func<CallSite, object, Range>> p__Site32;

            // Token: 0x04000279 RID: 633
            public static CallSite<Func<CallSite, object, Range>> p__Site33;

            // Token: 0x0400027A RID: 634
            public static CallSite<Func<CallSite, object, Range>> p__Site34;

            // Token: 0x0400027B RID: 635
            public static CallSite<Func<CallSite, object, Range>> p__Site35;

            // Token: 0x0400027C RID: 636
            public static CallSite<Func<CallSite, object, Range>> p__Site36;

            // Token: 0x0400027D RID: 637
            public static CallSite<Func<CallSite, object, Range>> p__Site37;

            // Token: 0x0400027E RID: 638
            public static CallSite<Func<CallSite, object, Range>> p__Site38;

            // Token: 0x0400027F RID: 639
            public static CallSite<Func<CallSite, object, Range>> p__Site39;

            // Token: 0x04000280 RID: 640
            public static CallSite<Func<CallSite, object, object, object, object>> p__Site3a;

            // Token: 0x04000281 RID: 641
            public static CallSite<Func<CallSite, object, object>> p__Site3b;

            // Token: 0x04000282 RID: 642
            public static CallSite<Func<CallSite, object, int[,], object>> p__Site3c;

            // Token: 0x04000283 RID: 643
            public static CallSite<Action<CallSite, Type, object>> p__Site3d;

            // Token: 0x04000284 RID: 644
            public static CallSite<Func<CallSite, object, Range>> p__Site3e;

            // Token: 0x04000285 RID: 645
            public static CallSite<Func<CallSite, object, Range>> p__Site3f;

            // Token: 0x04000286 RID: 646
            public static CallSite<Func<CallSite, object, Range>> p__Site40;

            // Token: 0x04000287 RID: 647
            public static CallSite<Func<CallSite, object, Range>> p__Site41;

            // Token: 0x04000288 RID: 648
            public static CallSite<Func<CallSite, object, Range>> p__Site42;

            // Token: 0x04000289 RID: 649
            public static CallSite<Func<CallSite, object, Range>> p__Site43;

            // Token: 0x0400028A RID: 650
            public static CallSite<Func<CallSite, object, Range>> p__Site44;

            // Token: 0x0400028B RID: 651
            public static CallSite<Func<CallSite, object, ChartObjects>> p__Site45;

            // Token: 0x0400028C RID: 652
            public static CallSite<Func<CallSite, object, Axis>> p__Site46;

            // Token: 0x0400028D RID: 653
            public static CallSite<Func<CallSite, object, Axis>> p__Site47;

            // Token: 0x0400028E RID: 654
            public static CallSite<Func<CallSite, object, SeriesCollection>> p__Site48;

            // Token: 0x0400028F RID: 655
            public static CallSite<Func<CallSite, object, SeriesCollection>> p__Site49;

            // Token: 0x04000290 RID: 656
            public static CallSite<Func<CallSite, object, Range>> p__Site4a;

            // Token: 0x04000291 RID: 657
            public static CallSite<Func<CallSite, object, Range>> p__Site4b;

            // Token: 0x04000292 RID: 658
            public static CallSite<Func<CallSite, object, Range>> p__Site4c;

            // Token: 0x04000293 RID: 659
            public static CallSite<Func<CallSite, object, Range>> p__Site4d;

            // Token: 0x04000294 RID: 660
            public static CallSite<Func<CallSite, object, Range>> p__Site4e;

            // Token: 0x04000295 RID: 661
            public static CallSite<Func<CallSite, object, Range>> p__Site4f;

            // Token: 0x04000296 RID: 662
            public static CallSite<Func<CallSite, object, Range>> p__Site50;

            // Token: 0x04000297 RID: 663
            public static CallSite<Func<CallSite, object, Axis>> p__Site51;

            // Token: 0x04000298 RID: 664
            public static CallSite<Func<CallSite, object, Axis>> p__Site52;

            // Token: 0x04000299 RID: 665
            public static CallSite<Func<CallSite, object, SeriesCollection>> p__Site53;
        }
    }
}
