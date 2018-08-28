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
            teamScorePieChart.TeamScorePieSeries.ItemsSource = newValue as List<TeamScore>;

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
        }
        #endregion
    }
}
