using System.Xml.Linq;

namespace AlloyTicketRequestApi.Services
{
    public class FormDataMapperService
    {
        public static (string? Text, string? Url) GetTextAndUrl(string? elementDefinition)
        {
            if (string.IsNullOrWhiteSpace(elementDefinition))
                return (null, null);
            try
            {
                var doc = XDocument.Parse(elementDefinition);
                var textItem = doc.Descendants("ITEM").FirstOrDefault(e => (string?)e.Attribute("Name") == "Text");
                var urlItem = doc.Descendants("ITEM").FirstOrDefault(e => (string?)e.Attribute("Name") == "URL");
                var text = textItem?.Attribute("Value")?.Value;
                var url = urlItem?.Attribute("Value")?.Value;
                return (text, url);
            }
            catch
            {
                return (null, null);
            }
        }
    }
}
