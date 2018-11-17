using System.Collections.Generic;
using System.Threading.Tasks;
using XyzApi.Models;
using XyzApi.Repository;

namespace XyzApi.Services.Impl
{
    public class WidgetService : IWidgetService
    {
        private readonly IWidgetRepository _widgetRepository;

        public WidgetService(IWidgetRepository widgetRepository)
        {
            _widgetRepository = widgetRepository;
        }

        public async Task<IEnumerable<Widget>> GetWidgets()
        {
            return await _widgetRepository.GetWidgets();
        }

        public async Task AddWidget(string widgetName)
        {
            await _widgetRepository.AddWidget(widgetName);
        }
    }
}