using System.Net.Http;

namespace PingPongProject.Workers
{
    public class PingRequestTimedHostedService : BackgroundService
    {
        private readonly ILogger<PingRequestTimedHostedService> _logger;
        private int executionCount = 0;
        private Timer? _timer = null;
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

                //_timer = new Timer(DoWork, null, TimeSpan.Zero,
                //    TimeSpan.FromSeconds(10));
                DoWork(httpClient);

                await Task.Delay(10000);
            }
        }

        private async void DoWork(HttpClient httpClient)
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
