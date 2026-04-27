using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SyncfusionMAUIApp1;
///<summary>
///CartesianChartsFeatures class
///</summary>
public partial class CartesianChartsFeatures : ContentPage
{
	///<summary>
    ///CartesianChartsFeatures constructor
    ///</summary>
	public CartesianChartsFeatures()
	{
		InitializeComponent();
	}
}
///<summary>
///CartesianChartsFeatures Modelclass
///</summary>
public class PopulationGrowth
{
	public string Country { get; set; }
	public double Value { get; set; }
}
///<summary>
///CartesianChartsFeatures ViewModelclass
///</summary>
public class CartesianChartViewModel
{
		public List<PopulationGrowth> Data { get; set; }
		///<summary>
		///CartesianChartsFeatures ViewModelclass constructor
		///</summary>
		public CartesianChartViewModel()
		{
			Data = new List<PopulationGrowth>()
			{
				new PopulationGrowth { Country = "China", Value = 0.541 },
				new PopulationGrowth { Country = "Brazil", Value = 0.818 },
				new PopulationGrowth { Country = "Bolivia", Value = 1.51 },
				new PopulationGrowth { Country = "Mexico", Value = 1.302 },
				new PopulationGrowth { Country = "Egypt", Value = 2.017 },
				new PopulationGrowth { Country = "Mongolia", Value = 1.683 }
			};
		}
}
