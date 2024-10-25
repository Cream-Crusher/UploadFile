using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LimetimePhotoUploadUI.Models
{
    public static class HtmlHelperExtender
    {
        public static IHtmlContent Json(this IHtmlHelper html, object data)
        {
            return html.Raw(JsonConvert.SerializeObject(data,
                    new JsonSerializerSettings()
                    {
                            Formatting = Newtonsoft.Json.Formatting.Indented,
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            StringEscapeHandling = StringEscapeHandling.EscapeHtml
                    }));
        }
    }
}