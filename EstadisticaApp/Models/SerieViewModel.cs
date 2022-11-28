using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EstadisticaApp.Models
{
    [ObservableObject]
    public partial class SerieViewModel
    {
        [ObservableProperty]
        public bool isBusy;

        public ICommand RefreshCommand { get; set; }

        public ISeries[] Series { get; set; } = Array.Empty<ISeries>();


        public ISeries[] Seriesh { get; set; }
        = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };

        public ISeries[] SeriesHist { get; set; } =
    {
        new ColumnSeries<double>
        {
            Name = "Mary",
            Values = new double[] { 2, 5, 4 }
        },
        new ColumnSeries<double>
        {
            Name = "Ana",
            Values = new double[] { 3, 1, 6 }
        }
    };

        public Axis[] XAxes { get; set; } =
        {
        new Axis
        {
            Labels = new string[] { "Category 1", "Category 2", "Category 3" },
            LabelsRotation = 15
        }
    };

        //    public ISeries[] Series { get; set; } =
        //{
        //        new ScatterSeries<ObservablePoint>
        //        {
        //            Values = new ObservableCollection<ObservablePoint>
        //            {
        //                new(2.2, 5.4),
        //                new(4.5, 2.5),
        //                new(4.2, 7.4),
        //                new(6.4, 9.9),
        //                new(4.2, 9.2),
        //                new(5.8, 3.5),
        //                new(7.3, 5.8),
        //                new(8.9, 3.9),
        //                new(6.1, 4.6),
        //                new(9.4, 7.7),
        //                new(8.4, 8.5),
        //                new(3.6, 9.6),
        //                new(4.4, 6.3),
        //                new(5.8, 4.8),
        //                new(6.9, 3.4),
        //                new(7.6, 1.8),
        //                new(8.3, 8.3),
        //                new(9.9, 5.2),
        //                new(8.1, 4.7),
        //                new(7.4, 3.9),
        //                new(6.8, 2.3),
        //                new(5.3, 7.1),
        //            }
        //        }
        //    };


        public void Refresh()
        {
            Init();
        }

        private void Init()
        {
            isBusy = true;
            var datos = App.EstatisdicaRepository.GetDatos();

            var serie = new ScatterSeries<ObservablePoint>();
            var values = new ObservableCollection<ObservablePoint>();

            List<ISeries> _series = new List<ISeries>();

            foreach (var item in datos)
            {
                values.Add(new((double)item.X, (double)item.Y));
            }

            serie.Values = values;

            try
            {
                _series.Add(serie);
                Series = _series.ToArray();
            }
            catch (Exception ex)
            {


            }

            isBusy = false;
        }


        public SerieViewModel()
        {
            Init();

            RefreshCommand = new Command(Refresh);
        }

    }
}
