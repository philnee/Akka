using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaTest.ActorModel.Messages;
using AkkaTest.ExternalServices;

namespace AkkaTest.ActorModel.Actors
{
    class StockPriceLookupActor: ReceiveActor
    {
        private readonly IStockPriceServiceGateway _stockPriceServiceGateway;

        public StockPriceLookupActor(IStockPriceServiceGateway stockPriceServiceGateway)
        {
            _stockPriceServiceGateway = stockPriceServiceGateway;

            Receive<RefreshStockPriceMessage>(message => LookupStockPrice(message));


        }

        private void LookupStockPrice(RefreshStockPriceMessage message)
        {
            var latestPrice = _stockPriceServiceGateway.GetLatestPrice(message.StockSymbol);

            Sender.Tell(new UpdatedStockPriceMessage(latestPrice, DateTime.Now));
        }
    }
}
