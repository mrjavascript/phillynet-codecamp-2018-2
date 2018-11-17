using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using XyzApi.Models;
using XyzApi.Services;

namespace XyzApi.Controllers
{
    public class WidgetController
    {
        private readonly IWidgetService _widgetService;

        public WidgetController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        [HttpGet]
        [Route("api/widget/get")]
        [SwaggerResponse(200, typeof(Task), "Successfully retrieved the widgets")]
        public async Task<IEnumerable<Widget>> GetWidgets()
        {
            return await _widgetService.GetWidgets();
        }
        
        [HttpPost]
        [Route("api/widget/add")]
        [SwaggerResponse(200, typeof(Task), "Successfully added the widget")]
        public async Task<string> AddWidget(string widgetName)
        {
            if (string.IsNullOrEmpty(widgetName))
            {
                throw new ArgumentException("name is required");
            }

            await _widgetService.AddWidget(widgetName);
            return "OK";
        }
    }
}