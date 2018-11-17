using System.ComponentModel.DataAnnotations;

namespace XyzApi.Models
{
    public class Widget
    {
        [Key] public int WidgetId { get; set; }
        public string WidgetName { get; set; }
    }
}