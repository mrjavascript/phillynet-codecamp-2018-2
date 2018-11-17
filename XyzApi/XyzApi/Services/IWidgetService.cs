using System.Collections.Generic;
using System.Threading.Tasks;
using XyzApi.Models;

namespace XyzApi.Services
{
    public interface IWidgetService
    {
        Task<IEnumerable<Widget>> GetWidgets();
        Task AddWidget(string widgetName);
    }
}