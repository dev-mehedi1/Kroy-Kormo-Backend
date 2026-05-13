using Sob_Need_Server.Interface;

namespace Sob_Need_Server.Services.Background
{
    public class RankingBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<RankingBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<RankingBackgroundService> _logger = logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(30); // Run every 30 minutes

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Ranking Background Service is starting.");

            try
            {
                using PeriodicTimer timer = new PeriodicTimer(_period);
                while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
                {
                    try
                    {
                        _logger.LogInformation("Ranking Background Service is running score updates...");

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var rankingService = scope.ServiceProvider.GetRequiredService<IRankingService>();
                            await rankingService.UpdateAllScoresAsync();
                        }

                        _logger.LogInformation("Ranking Background Service completed score updates.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while updating ranking scores.");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Ranking Background Service is stopping due to cancellation.");
            }
        }
    }
}
