using Ensek.Models;

namespace Ensek.Data.Abstract
{
    public interface IAccountsLibrary
    {
        bool IsValidAccountId(MeterReading mr);
    }
}
