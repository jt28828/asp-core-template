using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DotnetCoreWebApiTemplate.Pages
{
    /// <summary>
    /// An example of a server rendered page.
    /// This page would be available once the server is running at:
    /// http://serverAddress/pages/valuespage
    /// </summary>
    public class ValuesPage : PageModel
    {
        public string DataText { get; set; }

        public List<string> DemoTextList { get; set; }

        /// <summary>
        /// This is called when a get request occurs on this page
        /// </summary>
        /// <returns></returns>
        public void OnGet()
        {
            DataText = $"This is text added to the page on load. It is currently: {DateTime.Now.ToString()}";

            DemoTextList = new List<string>();
            foreach (int i in Enumerable.Range(0, 30))
            {
                DemoTextList.Add($"This is an item in the array. It is item: {i + 1}");
            }
        }
    }
}