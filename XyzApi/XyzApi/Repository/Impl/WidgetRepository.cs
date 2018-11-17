using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Builder;
using SQLitePCL;
using XyzApi.Models;
using XyzApi.Services.Impl;
using XyzApi.Utility;

namespace XyzApi.Repository.Impl
{
    public class WidgetRepository : IWidgetRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;

        public WidgetRepository(IDatabaseConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Widget>> GetWidgets()
        {
            using (var sqlConnection = _connectionFactory.GetConnection())
            {
                sqlConnection.Open();
                return await sqlConnection.QueryAsync<Widget>($"SELECT [WidgetId], [WidgetName] FROM [Widget]");
            }
        }

        public async Task AddWidget(string widgetName)
        {
            using (var sqlConnection = _connectionFactory.GetConnection())
            {
                sqlConnection.Open();
                await sqlConnection.ExecuteAsync("INSERT INTO [Widget] (WidgetName) VALUES (@WidgetName)",
                    new
                    {
                        WidgetName = widgetName
                    });
            }
        }

        public async Task CreateSqliteSchema(string script)
        {
            using (var sqlConnection = _connectionFactory.GetConnection())
            {
                sqlConnection.Open();
                await sqlConnection.ExecuteAsync(script);
            }
        }
    }
}