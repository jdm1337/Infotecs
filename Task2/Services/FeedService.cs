using System.ServiceModel.Syndication;
using System.Xml;

namespace Task2.Services
{
    public class FeedService
    {
        public SyndicationFeed Feed { get; set; }
        private string RssUrl { get; set; }
        private int UpdateInterval { get; set; }
        private readonly IConfiguration _configuration;

        public FeedService(IConfiguration configuration)
        {
            _configuration = configuration;
            RssUrl = configuration.GetSection("RssPath").Value;
            UpdateInterval = Convert.ToInt32(configuration.GetSection("UpdateInterval").Value);
        }
        protected async Task UpdateDataAsync()
        {
            using var reader = XmlReader.Create(RssUrl);
            Feed = SyndicationFeed.Load(reader);
        }
    }
}
