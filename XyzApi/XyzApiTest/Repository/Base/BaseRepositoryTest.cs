using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using XyzApi.Repository;
using XyzApi.Repository.Impl;
using XyzApi.Utility;
using XyzApiTest.Utility;

namespace XyzApiTest.Repository.Base
{
    public abstract class BaseRepositoryTest
    {
        private const string XyzWidgetSchema =
            "..\\XyzApiTest\\SQLite\\Schema\\xyz_widget.sql";
        
        protected readonly ConfigurationFixture Configuration;

        protected BaseRepositoryTest(ConfigurationFixture configuration)
        {
            Configuration = configuration;
        }
        
        protected async Task<IWidgetRepository> CreateWidgetRepository(
            Mock<IDatabaseConnectionFactory> connectionFactoryMock = null)
        {
            if (connectionFactoryMock == null)
            {
                var db = new InMemoryDatabase();
                db.OpenConnection();

                connectionFactoryMock = new Mock<IDatabaseConnectionFactory>();
                connectionFactoryMock.Setup(c => c.GetConnection()).Returns(db.OpenConnection());
            }

            IWidgetRepository widgetRepository = new WidgetRepository(connectionFactoryMock.Object);

            var projectRoot = AppContext.BaseDirectory.Substring(0,
                AppContext.BaseDirectory.LastIndexOf("bin", StringComparison.Ordinal));
            var script = File.ReadAllText(Path.Combine(projectRoot, XyzWidgetSchema));

            await widgetRepository.CreateSqliteSchema(script);
            return widgetRepository;
        }
    }
}