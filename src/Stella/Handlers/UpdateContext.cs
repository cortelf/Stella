namespace Stella.Handlers;

public class UpdateContext(int updateId)
{
    public int UpdateId { get; } = updateId;
}