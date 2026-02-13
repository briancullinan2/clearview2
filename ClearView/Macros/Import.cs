using System.IO;
using System.Text.RegularExpressions;

namespace EPIC.ClearView.Macros
{
    // Token: 0x0200001C RID: 28
    public static class Import
    {
        // Token: 0x0600018C RID: 396 RVA: 0x0000D114 File Offset: 0x0000B314
        public static IEnumerable<Match> RawData21(string dataFile)
        {
            List<string> list = new List<string>();
            using (FileStream fileStream = new FileStream(dataFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string item;
                    while ((item = streamReader.ReadLine()) != null)
                    {
                        list.Add(item);
                    }
                }
            }
            Regex regex21 = new Regex("^(?<analysisResultsId>[^,]*),(?<patientTreatmentId>[^,]*),(?<dateAnalysed>[^,]*),(?<filtered>[^,]*),(?<fingerDesc>[^,]*),(?<fingerType>[^,]*),(?<sectorNumber>[^,]*),(?<startAngle>[^,]*),(?<endAngle>[^,]*),(?<sectorArea>[^,]*),(?<integralArea>[^,]*),(?<normalizedArea>[^,]*),(?<averageIntensity>[^,]*),(?<entropy>[^,]*),(?<formCoefficient>[^,]*),(?<fractalCoefficient>[^,]*),(?<NS>[^,]*),(?<centerX>[^,]*),(?<centerY>[^,]*),(?<radiusMin>[^,]*),(?<radiusMax>[^,]*),(?<angle>[^,]*),(?<form2>[^,]*),(?<noiseLevel>[^,]*),(?<breakCoefficient>[^,]*),(?<softwareVersion>[^,]*),(?<AI1>[^,]*),(?<AI2>[^,]*),(?<AI3>[^,]*),(?<AI4>[^,]*),(?<form1_1>[^,]*),(?<form1_2>[^,]*),(?<form1_3>[^,]*),(?<form1_4>[^,]*),(?<ringThickness>[^,]*),(?<ringIntensity>[^,]*),(?<form2Prime>[^,]*),(?<username>[^,]*),?$");
            return from x in list
                   select regex21.Match(x);
        }
    }
}
