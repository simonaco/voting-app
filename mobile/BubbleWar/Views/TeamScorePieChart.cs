using System;
using System.Linq;
using System.Collections.Generic;

using Syncfusion.SfChart.XForms;

using Xamarin.Forms;

namespace BubbleWar
{
    public class TeamScorePieChart : SfChart
    {
        #region Constant Fields
        public static readonly BindableProperty ItemSourceProperty =
                                        BindableProperty.CreateAttached(nameof(PieSeries.ItemsSource),
                                            typeof(List<TeamScore>),
                                            typeof(TeamScorePieChart),
                                            null,
                                            propertyChanged: OnItemSourceChanged);

        #endregion

        #region Constructors
        public TeamScorePieChart()
        {
            TeamScorePieSeries = new PieSeries
            {
                EnableSmartLabels = true,
                EnableAnimation = true,
                XBindingPath = nameof(TeamScore.Color),
                YBindingPath = nameof(TeamScore.Points)
            };

            Series.Add(TeamScorePieSeries);
        }
        #endregion

        #region Properties
        public static List<TeamScore> GetItemSource(BindableObject view) => (List<TeamScore>)view.GetValue(ItemSourceProperty);
        public static void SetItemSource(BindableObject view, ReturnType value) => view.SetValue(ItemSourceProperty, value);

        public PieSeries TeamScorePieSeries { get; private set; }
        #endregion

        #region Methods
        static void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var teamScorePieChart = (TeamScorePieChart)bindable;
            var currentItemSource = (List<TeamScore>)teamScorePieChart.TeamScorePieSeries.ItemsSource;
            var oldItemSource = (List<TeamScore>)oldValue;
            var newItemSource = (List<TeamScore>)newValue;

            if (oldItemSource?.Count > 0 && newItemSource?.Count > 0)
                teamScorePieChart.TeamScorePieSeries.EnableAnimation = false;

            teamScorePieChart.TeamScorePieSeries.ItemsSource = newItemSource;

            var colorModel = new ChartColorModel
            {
                Palette = ChartColorPalette.Custom,
                CustomBrushes = new List<Color>()
            };

            foreach (TeamScore teamScore in teamScorePieChart.TeamScorePieSeries.ItemsSource)
            {
                switch (teamScore.Color)
                {
                    case TeamColor.Green:
                        colorModel.CustomBrushes.Add(Color.Green);
                        break;

                    case TeamColor.Red:
                        colorModel.CustomBrushes.Add(Color.Red);
                        break;

                    default:
                        throw new NotSupportedException($"{teamScore.Color.GetType()} Not Supported");
                }
            }

            teamScorePieChart.TeamScorePieSeries.ColorModel = colorModel;
            teamScorePieChart.TeamScorePieSeries.DataMarker = new ChartDataMarker { ShowLabel = true };
        }
        #endregion
    }
}
