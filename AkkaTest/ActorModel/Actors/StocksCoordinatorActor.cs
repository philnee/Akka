﻿using System.Collections.Generic;
using Akka.Actor;
using AkkaTest.ActorModel.Messages;

namespace AkkaTest.ActorModel.Actors
{
    class StocksCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef _chartingActor;
        private readonly Dictionary<string, IActorRef> _stockActors;

        public StocksCoordinatorActor(IActorRef chartingActor)
        {
            _chartingActor = chartingActor;

            _stockActors = new Dictionary<string, IActorRef>();

            Receive<WatchStockMessage>(message => WatchStock(message));
            Receive<UnWatchStockMessage>(message => UnWatchStock(message));
        }

        private void UnWatchStock(UnWatchStockMessage message)
        {
            bool childActorNeedsCreating = !_stockActors.ContainsKey(message.StockSymbol);

            if (childActorNeedsCreating)
            {
                IActorRef newChildActor =
                    Context.ActorOf(
                        Props.Create(() => new StockActor(message.StockSymbol)),
                        "StockActor_" + message.StockSymbol);
            }

            _chartingActor.Tell(new AddChartSeriesMessage(message.StockSymbol));

            _stockActors[message.StockSymbol].Tell(new SubscribeToNewStockPricesMessage(_chartingActor));
        }

        private void WatchStock(WatchStockMessage message)
        {
            if (!_stockActors.ContainsKey(message.StockSymbol))
            {
                return;
            }

            _chartingActor.Tell(new RemoveChartSeriesMessage(message.StockSymbol));

            _stockActors[message.StockSymbol].Tell(new UnSubscribeFromNewStockPricesMessage(_chartingActor));
        }
    }
}
