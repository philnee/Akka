using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace AkkaTest.ActorModel.Messages
{
    class UnSubscribeFromNewStockPricesMessage
    {
        public IActorRef Subscriber { get; private set;}

        public UnSubscribeFromNewStockPricesMessage(IActorRef subscriber)
        {
            Subscriber = subscriber;
        }
    }
}
