using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace WebMVC.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IOptions<AppSettings> _settings;
        private HttpClient _httpClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly string _notificationUrl;
        public IMapper _mapper { get; }

        public NotificationService(HttpClient httpClient, IOptions<AppSettings> settings, ILogger<NotificationService> logger, IMapper mapper)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
            _mapper = mapper;
            _notificationUrl = $"{_settings.Value.NotifyingURL}/api/v1/notification";
        }

        //public async Task<List<Notification>> GetAllApplicationEventsAsync()
        //{
        //    var uri = API.ApplicationEvent.GetAllApplicationEventsAsync(_notificationUrl);

        //    var responseString = await _httpClient.GetStringAsync(uri);

        //    var response = JsonConvert.DeserializeObject<List<Notification>>(responseString);

        //    var applicationEventViewModel = _mapper.Map<List<Notification>>(response);

        //    return applicationEventViewModel;
        //}

        //public async Task CreateApplicationEvent(ApplicationEventViewModel applicationEventViewModel)
        //{
        //    var uri = API.ApplicationEvent.CreateApplicationEvent(_notificationUrl);

        //    var applicationEvent = _mapper.Map<ApplicationEvent>(applicationEventViewModel);

        //    var applicationEventContent = new StringContent(JsonConvert.SerializeObject(applicationEvent), System.Text.Encoding.UTF8, "application/json");

        //    var response = await _httpClient.PostAsync(uri, applicationEventContent);

        //    if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        //    {
        //        throw new Exception("Error Saving Application Event, try later.");
        //    }
        //}

        //public async Task<ApplicationEventViewModel> Edit(Guid id)
        //{
        //    return null;
        //}
    }
}