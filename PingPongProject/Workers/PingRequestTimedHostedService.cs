using System.Net.Http;

namespace PingPongProject.Workers
{
    public class PingRequestTimedHostedService : BackgroundService
    {
        private readonly ILogger<PingRequestTimedHostedService> _logger;
        private int executionCount = 0;
        //private PeriodicTimer? _timer = null;
        private readonly IHttpClientFactory _httpClientFactory;

        public PingRequestTimedHostedService(ILogger<PingRequestTimedHostedService> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var httpClient = _httpClientFactory.CreateClient("PongService");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Timed Hosted Service running.");
                using(var _timer = new PeriodicTimer(TimeSpan.FromSeconds(10)))
                {
                    while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync())
                    {
                        ExecutePeriodicTask(httpClient);
                    }
                    await _timer.WaitForNextTickAsync(stoppingToken);
                    
                }
            }
        }

        private async void ExecutePeriodicTask(HttpClient httpClient)
        {
            var count = Interlocked.Increment(ref executionCount);

            var response = await httpClient.GetAsync("/pong");
            if (response.IsSuccessStatusCode) {
                _logger.LogInformation(
                "got reply: pong");
            }
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

    }
}
