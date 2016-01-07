namespace AkkaTest.ExternalServices
{
    interface IStockPriceServiceGateway
    {
        decimal GetLatestPrice(string stockSymbol);
    }
}
