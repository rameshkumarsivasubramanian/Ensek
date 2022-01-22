namespace Ensek.Data.Abstract
{
    public interface IMeterReadingsLibrary
    {
        bool HasOlderReading(string AccountId, string MeterReadingDateTime);
    }
}
