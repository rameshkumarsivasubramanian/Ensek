using Ensek.Models;

namespace Ensek.Data.Abstract
{
    public interface IMeterReadingsLibrary
    {
        bool HasOlderReading(MeterReading mr);

        void InsertReading(MeterReading mr);

        void SaveChanges();
    }
}
