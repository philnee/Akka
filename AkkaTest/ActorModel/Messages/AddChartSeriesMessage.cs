namespace AkkaTest.ActorModel.Messages
{
    class AddChartSeriesMessage
    {
        public string StockSymbol { get; set; }

        public AddChartSeriesMessage(string stockSymbol)
        {
            StockSymbol = stockSymbol;
        }
    }
}
