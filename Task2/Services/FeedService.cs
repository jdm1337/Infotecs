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
        private readonly TaskScheduleService taskScheduleService;

        public FeedService(IConfiguration configuration, TaskScheduleService taskScheduleService)
        {
            _configuration = configuration;
            RssUrl = configuration.GetSection("RssUrl").Value;
            UpdateInterval = Convert.ToInt32(configuration.GetSection("UpdateInterval").Value);
            this.taskScheduleService = taskScheduleService;
            taskScheduleService.Execute(UpdateData, UpdateInterval);
            
        }
        protected void UpdateData()
        {
            using var reader = XmlReader.Create(RssUrl);
            Feed = SyndicationFeed.Load(reader);
        }
    }
}
