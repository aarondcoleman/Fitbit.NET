using System.IO;

namespace Fitbit.Portable.Tests
{
    /// <summary>
    /// In order for this class to work with SampleData/Json files, the file must be available in bin/debug/SampleData.
    /// To do this, go to the properties of the needed file and change "Copy To Output Directory" to "Copy If Newer"
    /// If this is not done, this class will throw an error.
    /// </summary>
    public static class SampleDataHelper
    {
        private static readonly string SampleDataPath = Path.Combine(Path.GetDirectoryName(typeof(SampleDataHelper).Assembly.Location), "SampleData");

        private static string PathFor(string sampleFile)
        {
            return Path.Combine(SampleDataPath, sampleFile);
        }

        public static string GetContent(string fileName)
        {
            return File.ReadAllText(PathFor(fileName));
        }
    }
}