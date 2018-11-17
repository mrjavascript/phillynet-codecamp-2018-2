using System.Data;

namespace XyzApi.Utility
{
    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetConnection();
    }
}