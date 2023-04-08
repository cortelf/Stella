namespace Stella.Polling;

public interface IPollingApp
{
    void RunPolling(CancellationToken cancellationToken = default);
}