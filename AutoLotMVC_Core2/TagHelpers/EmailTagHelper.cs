using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AutoLotMVC_Core2.TagHelpers
{
    public class EmailTagHelper : TagHelper
    {
        public String EmailName { get; set; }
        public String EmailDomain { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";    // Replaces <email> with <a> tag
            var address = EmailName + "@" + EmailDomain;
            output.Attributes.SetAttribute(name: "href", value: "mailto:" + address);
            output.Content.SetContent(unencoded: address);
        }

    }
}
