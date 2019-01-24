namespace PumpkinTrade
{
    public interface IPumpkinTradeService
    {
        bool Buy(string userName, decimal price);
        bool Sell(string userName, decimal price);
        string GetProcessedTrades();
    }
}