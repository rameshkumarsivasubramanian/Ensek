namespace Ensek.Data.Abstract
{
    public interface IMeterReadingsLibrary
    {
        bool HasOlderReading(string AccountId, string MeterReadingDateTime);

        void InsertReading(string AccountId, string MeterReadingDateTime, string MeterReadValue);

        void SaveChanges();
    }
}
