using System.IO;

namespace Fitbit.Portable.Tests.Helpers
{
    public class SampleData
    {
        private static string SampleDataPath = Path.Combine(Path.GetFullPath(@"..\..\"), "SampleData");

        public static string PathFor(string sampleFile)
        {
            return Path.Combine(SampleDataPath, sampleFile);
        }
    }
}