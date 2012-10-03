namespace ScoutsOffline.Http
{
    using System.Collections.Generic;
    using HtmlAgilityPack;

    public class HtmlForm
    {
        public readonly FormValueCollection Values = new FormValueCollection();

        public static HtmlForm FromNode(HtmlNode htmlNode)
        {
            var form = new HtmlForm();
            var inputs = htmlNode.SelectNodes("//input");
            foreach (var input in inputs)
            {
                var name = input.GetAttributeValue("name", null);
                var value = input.GetAttributeValue("value", null);
                if (name != null)
                {
                    form.Values.Add(name, value);
                }
            }
            return form;
        }
    }
}
