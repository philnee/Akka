namespace AkkaTest.ActorModel.Messages
{
    class RemoveChartSeriesMessage
    {
        public string StockSymbol { get; set; }

        public RemoveChartSeriesMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}