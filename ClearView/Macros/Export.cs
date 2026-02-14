using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace EPIC.ClearView.Macros
{
    // Token: 0x0200001F RID: 31
    public static class Export
    {
        // Token: 0x060001AC RID: 428 RVA: 0x00010CA8 File Offset: 0x0000EEA8
        public static void Calibration(List<DataLayer.Entities.ImageCalibration> calibrations, string calibrationDir, Action<string, int> reportProgress = null)
        {
            if (reportProgress != null)
            {
                reportProgress("Creating Export Directory", 0);
            }
            if (!Directory.Exists(calibrationDir))
            {
                FileSystem.CreateDirectory(calibrationDir);
            }
            int num = 0;
            foreach (DataLayer.Entities.ImageCalibration calibration in calibrations)
            {
                string path = string.Format("CalibrationImage#{0}.bmp", calibrations.IndexOf(calibration));

                DataLayer.Entities.ImageCapture capture = calibration.Image.Capture;
                if (capture != null)
                {
                    int brightness = capture.Capture.Brightness;
                    int gain = capture.Capture.Gain;
                    path = string.Format("B={1}G={2}_CalibrationImage#{0}.bmp", calibrations.IndexOf(calibration), brightness, gain);
                }
                string path2 = Path.Combine(calibrationDir, path);
                using (MemoryStream memoryStream = new MemoryStream(calibration.Image.ImageData))
                {
                    using (FileStream fileStream = new FileStream(path2, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, false))
                        {
                            gzipStream.CopyTo(fileStream);
                        }
                    }
                }
                num++;
                if (reportProgress != null)
                {
                    reportProgress(string.Format("Exported Image ({0} of {1})", num, calibrations.Count), (int)((double)num / (double)calibrations.Count * 100.0));
                }
            }
        }

        // Token: 0x060001AD RID: 429 RVA: 0x00010EBC File Offset: 0x0000F0BC
        public static void Colored(List<DataLayer.Entities.ImageCalibration> calibrations, string calibrationDir, Action<string, int> reportProgress = null)
        {
            if (reportProgress != null)
            {
                reportProgress("Creating Export Directory", 0);
            }
            if (!Directory.Exists(calibrationDir))
            {
                FileSystem.CreateDirectory(calibrationDir);
            }
            int num = 0;
            foreach (DataLayer.Entities.ImageCalibration calibrationEntity in calibrations)
            {
                string path = string.Format("CalibrationImage#{0}.bmp", calibrations.IndexOf(calibrationEntity));
                DataLayer.Entities.ImageCapture capture = calibrationEntity.Image.Capture;
                if (capture != null)
                {
                    int brightness = capture.Capture.Brightness;
                    int gain = capture.Capture.Gain;
                    path = string.Format("B={1}G={2}_CalibrationImage#{0}.bmp", calibrations.IndexOf(calibrationEntity), brightness, gain);
                }
                string path2 = Path.Combine(calibrationDir, path);
                using (MemoryStream memoryStream = new MemoryStream(calibrationEntity.Colorized.ImageData))
                {
                    using (FileStream fileStream = new FileStream(path2, FileMode.CreateNew, FileAccess.ReadWrite))
                    {
                        using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, false))
                        {
                            gzipStream.CopyTo(fileStream);
                        }
                    }
                }
                num++;
                if (reportProgress != null)
                {
                    reportProgress(string.Format("Exported Colored Image ({0} of {1})", num, calibrations.Count), (int)((double)num / (double)calibrations.Count * 100.0));
                }
            }
        }

        // Token: 0x060001AE RID: 430 RVA: 0x000110D0 File Offset: 0x0000F2D0
        public static void Patient(List<DataLayer.Entities.Patient> patients, string patientsPath, string append = null)
        {
            if (append == null)
            {
                append = DateTime.Now.ToString("yyMMddfff");
            }
            foreach (DataLayer.Entities.Patient patientEntity in patients)
            {
                string path = Path.Combine(patientsPath, string.Concat(new string[]
                {
                    patientEntity.FirstName,
                    patientEntity.LastName,
                    "_",
                    append,
                    ".vcf"
                }));
                FileStream fileStream = File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite);
                string s = string.Format("BEGIN:VCARD\r\nVERSION:4.0\r\nN:{0};{1};;;\r\nFN: {0}{2}{1}\r\nGENDER:{4}\r\nBDAY:{5}\r\nREV:{3}\r\nEND:VCARD", new object[]
                {
                    patientEntity.FirstName,
                    patientEntity.LastName,
                    (patientEntity.MiddleInitial.Trim() != "") ? (" " + patientEntity.MiddleInitial + " ") : " ",
                    DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"),
                    patientEntity.Gender.ToString(),
                    patientEntity.BirthDate.ToString("yyyyMMdd")
                });
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
        }
    }
}
