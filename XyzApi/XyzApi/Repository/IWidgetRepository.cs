using System.Collections.Generic;
using System.Threading.Tasks;
using XyzApi.Models;

namespace XyzApi.Repository
{
    public interface IWidgetRepository
    {
        Task<IEnumerable<Widget>> GetWidgets();
        Task AddWidget(string widgetName);
    }
}