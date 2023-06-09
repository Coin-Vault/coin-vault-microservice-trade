namespace TradingService.AsyncDataServices
{
    public interface IMessageBusEncryption
    {
        string EncryptMessage(string key, string message);
    }
}
