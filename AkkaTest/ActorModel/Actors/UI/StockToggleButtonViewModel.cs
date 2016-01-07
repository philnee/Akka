using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Akka.Actor;
using AkkaTest.ActorModel;
using AkkaTest.ActorModel.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace AkkaTest.ViewModel
{
    public class StockToggleButtonViewModel : ViewModelBase
    {
        private string _buttonText;
        public string StockSymbol { get; set; }
        public ICommand ToggleCommand { get; set; }
        public IActorRef StockToggleButtonActorRef { get; private set; }

        public string ButtonText
        {
            get { return _buttonText; }
            set { Set(() => ButtonText, ref _buttonText, value); }
        }

        public StockToggleButtonViewModel(IActorRef stocksCoordinatorRef, string stockSymbol)
        {
            StockSymbol = stockSymbol;

            StockToggleButtonActorRef = ActorSystemReference.ActorSystem
                .ActorOf(
                Props.Create(() =>
                new StockToggleButtonActor(stocksCoordinatorRef, this, stockSymbol)));

            ToggleCommand = new RelayCommand(
                () => StockToggleButtonActorRef.Tell(new FlipToggleMessage()));

            UpdateButtonTextToOff();
        }

        public void UpdateButtonTextToOff()
        {
            ButtonText = ConstructButtonText(false);
        }

        public void UpdateButtonTextToOn()
        {
            ButtonText = ConstructButtonText(true);
        }

        private string ConstructButtonText(bool isToggledOn)
        {
            return string.Format("{0} {1}",
                StockSymbol,
                isToggledOn ? "(on)" : "off");
        }
    }

    class StockToggleButtonActor : ReceiveActor
    {
        private readonly IActorRef _stocksCoordinatorRef;
        private readonly StockToggleButtonViewModel _stockToggleButtonViewModel;
        private readonly string _stockSymbol;

        public StockToggleButtonActor(IActorRef stocksCoordinatorRef, StockToggleButtonViewModel stockToggleButtonViewModel, string stockSymbol)
        {
            _stocksCoordinatorRef = stocksCoordinatorRef;
            _stockToggleButtonViewModel = stockToggleButtonViewModel;
            _stockSymbol = stockSymbol;

            ToggledOff();
        }

        private void ToggledOff()
        {
            Receive<FlipToggleMessage>(
                message =>
                {
                    _stocksCoordinatorRef.Tell(new WatchStockMessage(_stockSymbol));

                    _stockToggleButtonViewModel.UpdateButtonTextToOn();

                    Become(ToggledOn);
                });
        }

        private void ToggledOn()
        {
            Receive<FlipToggleMessage>(
                message =>
                {
                    _stocksCoordinatorRef.Tell(new UnWatchStockMessage(_stockSymbol));

                    _stockToggleButtonViewModel.UpdateButtonTextToOff();

                    Become(ToggledOff);
                });
        }
    }
}
