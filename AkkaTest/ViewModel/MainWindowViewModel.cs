using System.Collections.Generic;
using Akka.Actor;
using AkkaTest.ActorModel;
using AkkaTest.ActorModel.Actors;
using AkkaTest.ActorModel.Actors.UI;
using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;

namespace AkkaTest.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IActorRef _chartingActorRef;
        private IActorRef _stocksCoordinatorActorRef;
        private PlotModel _plotModel;

        public Dictionary<string, StockToggleButtonViewModel> StockToggleButtonViewModels; 

        public MainWindowViewModel()
        {
            SetUpChartModel();

            InitializeActors();

            CreateStockButtonViewModels();
        }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { Set(() => PlotModel, ref _plotModel, value); }
        }

        private void CreateStockButtonViewModels()
        {
            StockToggleButtonViewModels = new Dictionary<string, StockToggleButtonViewModel>();

            CreateStockButtonViewModel("AAA");
            CreateStockButtonViewModel("BBB");
            CreateStockButtonViewModel("CCC");
        }

        private void CreateStockButtonViewModel(string stockSymbol)
        {
            var newViewModel = new StockToggleButtonViewModel(_stocksCoordinatorActorRef, stockSymbol);

            StockToggleButtonViewModels.Add(stockSymbol, newViewModel);
        }

        private void InitializeActors()
        {
            _chartingActorRef =
                ActorSystemReference.ActorSystem.ActorOf(Props.Create(() => new LineChartingActor(PlotModel)));

            _stocksCoordinatorActorRef =
                ActorSystemReference.ActorSystem.ActorOf(
                    Props.Create(() => new StocksCoordinatorActor(_chartingActorRef)), "StocksCoordinator");
        }

        private void SetUpChartModel()
        {
            _plotModel = new PlotModel
            {
                   
            };

            var stockDateTimeAxes = new DateTimeAxis();

            _plotModel.Axes.Add(stockDateTimeAxes);
            var linearAxes = new LinearAxis();

            _plotModel.Axes.Add(linearAxes);
        }
    }
}