using Syncfusion.Maui.AIAssistView;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SyncfusionMAUIApp1
{
    public class SmartOrderInfoRepository : INotifyPropertyChanged
    {
        private ObservableCollection<OrderInfo> orderInfo;
        public ObservableCollection<OrderInfo> OrderInfoCollection
        {
            get { return orderInfo; }
            set { this.orderInfo = value; }
        }

        private ObservableCollection<ISuggestion> _suggestions;

        public ObservableCollection<ISuggestion> Suggestions
        {
            get { return this._suggestions; }
            set
            {
                this._suggestions = value;
                RaisePropertyChanged("Suggestions");
            }
        }

        public SmartOrderInfoRepository()
        {
            orderInfo = new ObservableCollection<OrderInfo>();
            var assistSuggestions = new AssistItemSuggestion();

            this._suggestions = new ObservableCollection<ISuggestion>()
            {
                new AssistSuggestion() {Text = "Sort the OrderID by Descending"},
                new AssistSuggestion() {Text = "Filter Where CustomerID is Maria Anders "},
                new AssistSuggestion() { Text = "Group the Customer Columns"},
            };

            this.GenerateOrders();
        }

        public void GenerateOrders()
        {
            orderInfo.Add(new OrderInfo("1001", "Maria Anders", "Germany", "ALFKI", "Berlin"));
            orderInfo.Add(new OrderInfo("1002", "Ana Trujillo", "Mexico", "ANATR", "Mexico D.F."));
            orderInfo.Add(new OrderInfo("1003", "Ant Fuller", "Mexico", "ANTON", "Mexico D.F."));
            orderInfo.Add(new OrderInfo("1004", "Thomas Hardy", "UK", "AROUT", "London"));
            orderInfo.Add(new OrderInfo("1005", "Tim Adams", "Sweden", "BERGS", "London"));
            orderInfo.Add(new OrderInfo("1006", "Hanna Moos", "Germany", "BLAUS", "Mannheim"));
            orderInfo.Add(new OrderInfo("1007", "Andrew Fuller", "France", "BLONP", "Strasbourg"));
            orderInfo.Add(new OrderInfo("1008", "Martin King", "Spain", "BOLID", "Madrid"));
            orderInfo.Add(new OrderInfo("1009", "Lenny Lin", "France", "BONAP", "Marsiella"));
            orderInfo.Add(new OrderInfo("1010", "John Carter", "Canada", "BOTTM", "Lenny Lin"));
            orderInfo.Add(new OrderInfo("1011", "Laura King", "UK", "AROUT", "London"));
            orderInfo.Add(new OrderInfo("1012", "Anne Wilson", "Germany", "BLAUS", "Mannheim"));
            orderInfo.Add(new OrderInfo("1013", "Martin King", "France", "BLONP", "Strasbourg"));
            orderInfo.Add(new OrderInfo("1014", "Gina Irene", "UK", "AROUT", "London"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
