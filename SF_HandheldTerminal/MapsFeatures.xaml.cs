using System.Collections.ObjectModel;
namespace SF_HandheldTerminal;
///<summary>
///MapsFeatures class
///</summary>
public partial class  MapsFeatures  : ContentPage
{
    ///<summary>
    ///MapsFeatures constructor
    ///</summary>
	public  MapsFeatures()
	{
		InitializeComponent();
	}
}
///<summary>
///MapsFeatures ViewModel class
///</summary>
public class MapsViewModel
{
    public ObservableCollection<MapModel> Data { get; set; }
	///<summary>
    ///MapsFeatures ViewModel class constructor
    ///</summary>
    public MapsViewModel()
    {
        Data = new ObservableCollection<MapModel>();
        Data.Add(new MapModel("New South Wales", "New\nSouth Wales", 1));
        Data.Add(new MapModel("Queensland", "Queensland", 2));
        Data.Add(new MapModel("Northern Territory", "Northern\nTerritory", 3));
        Data.Add(new MapModel("Victoria", "Victoria", 4));
        Data.Add(new MapModel("Tasmania", "Tasmania", 5));
        Data.Add(new MapModel("Western Australia", "Western Australia", 6));
        Data.Add(new MapModel("South Australia", "South Australia", 7));
    }
    ///<summary>
    ///MapsFeatures Model class
    ///</summary>
    public class MapModel
    {
        public string State { get; set; }
        public string StateCode { get; set; }
        public int ID { get; set; }
        ///<summary>
        ///MapsFeatures Model class constructor
        ///</summary>
        public MapModel(string state, string stateCode, int id)
        {
            State = state;
            StateCode = stateCode;
            ID = id;
        }
    }
}
