using System.IO;

namespace Fitbit.Portable.Tests
{
    public static class SampleDataHelper
    {
        private static readonly string SampleDataPath = Path.Combine(Path.GetFullPath(@"..\..\"), "SampleData");

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