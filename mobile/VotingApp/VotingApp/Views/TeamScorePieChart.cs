using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Syncfusion.SfChart.XForms;

using Xamarin.Forms;

namespace VotingApp
{
    class TeamScorePieChart : SfChart
    {
        #region Constant Fields
        public static readonly BindableProperty ItemSourceProperty =
                                        BindableProperty.CreateAttached(nameof(PieSeries.ItemsSource),
                                            typeof(List<TeamScore>),
                                            typeof(TeamScorePieChart),
                                            null,
                                            propertyChanged: OnItemSourceChanged);

        readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        #endregion

        #region Constructors
        public TeamScorePieChart()
        {
            TeamScorePieSeries = new PieSeries
            {
                EnableSmartLabels = true,
                EnableAnimation = true,
                XBindingPath = nameof(TeamScore.Color),
                YBindingPath = nameof(TeamScore.Points),
                ItemsSource = new List<TeamScore>()
            };

            Series.Add(TeamScorePieSeries);
        }
        #endregion

        #region Properties
        public PieSeries TeamScorePieSeries { get; private set; }
        #endregion

        #region Methods

        public static List<TeamScore> GetItemSource(BindableObject view) => (List<TeamScore>)view.GetValue(ItemSourceProperty);
        public static void SetItemSource(BindableObject view, ReturnType value) => view.SetValue(ItemSourceProperty, value);

        public async Task UpdatePieChart(List<TeamScore> newItemSource, List<TeamScore> oldItemSource)
        {
            if (oldItemSource is null || newItemSource is null)
                return;

            await _lock.WaitAsync().ConfigureAwait(false);

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (oldItemSource.Count > 0 && newItemSource.Count > 0)
                        TeamScorePieSeries.EnableAnimation = false;

                    TeamScorePieSeries.ItemsSource = newItemSource;

                    var colorModel = new ChartColorModel
                    {
                        Palette = ChartColorPalette.Custom,
                        CustomBrushes = new List<Color>()
                    };

                    foreach (TeamScore teamScore in TeamScorePieSeries.ItemsSource)
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

                    TeamScorePieSeries.ColorModel = colorModel;
                    TeamScorePieSeries.DataMarker = new ChartDataMarker { ShowLabel = true };
                }
                finally
                {
                    _lock.Release();
                }
            });
        }

        static async void OnItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var teamScorePieChart = bindable as TeamScorePieChart;
            var currentItemSource = teamScorePieChart?.TeamScorePieSeries?.ItemsSource as List<TeamScore>;
            var oldItemSource = oldValue as List<TeamScore>;
            var newItemSource = newValue as List<TeamScore>;


            if (teamScorePieChart is null || currentItemSource is null || oldItemSource is null || newItemSource is null)
                return;

            await teamScorePieChart.UpdatePieChart(newItemSource, oldItemSource).ConfigureAwait(false);
        }
        #endregion
    }
}
