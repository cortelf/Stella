using Microsoft.Extensions.Hosting;

namespace Stella.Polling;

public class StellaPollingHostedService(IStellaPollingApp stellaPollingApp): IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _workerTask = Task.Run(async () => await stellaPollingApp.RunPollingAsync(_cancellationTokenSource.Token));
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _cancellationTokenSource.CancelAsync();
        await _workerTask!;
    }
}