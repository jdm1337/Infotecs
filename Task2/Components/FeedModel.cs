using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Task2.Components
{
    public class FeedModel : ComponentBase
    {
        public SyndicationFeed Feed { get; set; }
        private readonly IConfiguration _configuration;
        public FeedModel(IConfiguration configuration)
        {
            _configuration = configuration; 
        }
        protected override async Task OnInitializedAsync()
        {
            var url = "https://habr.com/ru/rss/interesting/";
            using var reader = XmlReader.Create(url);
            Feed = SyndicationFeed.Load(reader);
        }
    }
}
