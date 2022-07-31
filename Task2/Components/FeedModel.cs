using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Task2.Services;

namespace Task2.Components
{
    public class FeedModel : ComponentBase
    {
        public SyndicationFeed Feed { get; set; }
        [Inject]
        protected FeedService FeedService { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            Feed = FeedService.Feed;
        }
    }
}
