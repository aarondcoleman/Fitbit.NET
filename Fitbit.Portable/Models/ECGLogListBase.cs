using System.Collections.Generic;

namespace Fitbit.Models
{
    public class ECGLogListBase
    {
        public Pagination Pagination { get; set; }
        //Shortcut property to allow user to check for additional paged data
        public bool HasMorePages => !string.IsNullOrEmpty(Pagination?.Next);
        public List<ECGReadings> ECGReadings { get; set; }
    }
}