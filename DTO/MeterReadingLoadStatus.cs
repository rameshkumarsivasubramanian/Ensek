using System.Collections.Generic;

namespace Ensek.DTO
{
    public class MeterReadingLoadStatus
    {
        public int TotalRecords { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public IEnumerable<string> ValidationResults { get; set; }
    }
}
