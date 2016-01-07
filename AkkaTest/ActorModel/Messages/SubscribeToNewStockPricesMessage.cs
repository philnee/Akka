using Akka.Actor;

namespace AkkaTest.ActorModel.Messages
{
    class SubscribeToNewStockPricesMessage
    {
        public IActorRef Subscriber { get; private set; }

        public SubscribeToNewStockPricesMessage(IActorRef subscribingActorRef)
        {
            Subscriber = subscribingActorRef;
        }
    }
}
