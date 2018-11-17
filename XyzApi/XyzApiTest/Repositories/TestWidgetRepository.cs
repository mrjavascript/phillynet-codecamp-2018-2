using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;
using XyzApi.Models;
using XyzApi.Repository;
using XyzApi.Repository.Impl;
using XyzApi.Utility;
using XyzApiTest.Utility;

namespace XyzApiTest.Repositories
{
    public class TestWidgetRepository : BaseRepositoryTest, IClassFixture<ConfigurationFixture>
    {
        public TestWidgetRepository(ConfigurationFixture configuration) : base(configuration)
        {
        }

        [Fact]
        public async Task test_get_widgets_ormlite()
        {
            // mock
            var table = new Widget
            {
                WidgetId = 1,
                WidgetName = "test widget"
            };
            var table2 = new Widget
            {
                WidgetId = 2,
                WidgetName = "another test widget"
            };

            var db = new InMemoryDatabase();
            db.OpenConnection();
            db.Insert(new List<Widget>{table, table2});
            
            // test
            var repository = CreateMockRepository(db);
            var result = await repository.GetWidgets();
//            Assert.Single(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task test_get_widgets_sqlite()
        {
            var repository = await CreateWidgetRepository();
            
            //
            //    Mock data
            await repository.AddWidget("philly");
            await repository.AddWidget("dot");
            await repository.AddWidget("net");
            
            //
            //    Validate
            var result = await repository.GetWidgets();
            
            Assert.Equal(3, result.Count());
        }

        private IWidgetRepository CreateMockRepository(InMemoryDatabase db)
        {
            var widgetDbMock = new Mock<IDatabaseConnectionFactory>();
            widgetDbMock.Setup(c => c.GetConnection()).Returns(db.OpenConnection());
            return new WidgetRepository(widgetDbMock.Object);
        }
    }
}