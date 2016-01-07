using System;

namespace AkkaTest.ActorModel.Actors
{
    internal class StockPriceMessage
    {
        public readonly string StockSymbol;
        public readonly decimal StockPrice;
        public readonly DateTime Date;

        public StockPriceMessage(string stockSymbol, decimal stockPrice, DateTime date)
        {
            StockSymbol = stockSymbol;
            StockPrice = stockPrice;
            Date = date;
        }
    }
}