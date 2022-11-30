using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using MathNet.Numerics.Statistics;
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

        public ISeries[] SeriesHist { get; set; }  = Array.Empty<ISeries>();
        
        public ISeries[] SeriesCandle { get; set; } = Array.Empty<ISeries>();
        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Labels = new string[] { "X", "Y" },
                LabelsRotation = 15
            }
        };


        public void Refresh()
        {
            Init();
        }

        private void Init()
        {
            isBusy = true;
            var datos = App.EstatisdicaRepository.GetDatos();
            

            try
            {
                DiagramaDispersion(datos);

                Histograma(datos);

                DiagramaDeCaja(datos);

            }
            catch (Exception ex)
            {


            }

            isBusy = false;
        }

        private void DiagramaDeCaja(List<Dato> datos)
        {
            LiveCharts.Configure(config =>
                                config
                                    .HasMap<Box>((box, point) =>
                                    {
                                        // in this lambda function we take an instance of the City class (see city parameter)
                                        // and the point in the chart for that instance (see point parameter)
                                        // LiveCharts will call this method for every instance of our City class array,
                                        // now we need to populate the point coordinates from our City instance to our point

                                        point.SecondaryValue = box.Rango; // use the date for the X axis (secondary)

                                        // now LiveCharts uses Primary (high), Tertiary (open)
                                        // Quaternary (close) and Quinary (low) planes to represent
                                        // a financial point:

                                        point.PrimaryValue = (float)box.Maximo;
                                        point.TertiaryValue = (float)box.Quartil1;
                                        point.QuaternaryValue = (float)box.Quartil3;
                                        point.QuinaryValue = (float)box.Minimo;
                                    })
                                );





            var m_x = datos.Select(x => (double)x.X).AsEnumerable();
            var m_y = datos.Select(x => (double)x.Y).AsEnumerable();

            var maxx = (double)datos.Max(x => x.X);
            var maxy = (double)datos.Max(x => x.Y);


            var minx = (double)datos.Min(x => x.X);
            var miny = (double)datos.Min(x => x.Y);

            var px25 = m_x.Percentile(25);
            var py25 = m_y.Percentile(25);

            var px50 = m_x.Percentile(50);
            var py50 = m_y.Percentile(50);

            var px75 = m_x.Percentile(75);
            var py75 = m_y.Percentile(75);


            var BoxData = new[]
            {
                    new Box
                    {
                        Rango = 1,
                        Minimo= minx,
                         Quartil1 = px25,
                          Quartil3= px75,
                    },
                    new Box
                    {
                        Rango = 2,
                        Minimo= miny,
                         Quartil1 = py25,
                          Quartil3= py75,
                    }
                };


            SeriesCandle = new[]
            {
                    new CandlesticksSeries<Box>
                    {
                        Mapping = (box, point) =>
                        {
                            point.SecondaryValue = box.Rango;
                            point.PrimaryValue = (float)box.Maximo;
                            point.TertiaryValue = (float)box.Quartil1;
                            point.QuaternaryValue = (float)box.Quartil3;
                            point.QuinaryValue = (float)box.Minimo;
                        },
                        Values = BoxData
                    }
                };
        }

        private void Histograma(List<Dato> datos)
        {
            var serie = new ScatterSeries<ObservablePoint>();
            var values = new ObservableCollection<ObservablePoint>();

            var columnX = new ColumnSeries<double>();
            columnX.Name = "x";
            columnX.Values = datos.Select(x => (double)x.X).ToArray();


            var columnY = new ColumnSeries<double>();
            columnY.Name = "y";
            columnY.Values = datos.Select(x => (double)x.Y).ToArray();


            SeriesHist = new ISeries[] { columnX, columnY };
        }

        private void DiagramaDispersion(List<Dato> datos)
        {
            var serie = new ScatterSeries<ObservablePoint>();
            var values = new ObservableCollection<ObservablePoint>();
            List<ISeries> _series = new List<ISeries>();

            foreach (var item in datos)
            {
                values.Add(new((double)item.X, (double)item.Y));
            }

            serie.Values = values;

            _series.Add(serie);
            Series = _series.ToArray();
        }

        public SerieViewModel()
        {
            Init();

            RefreshCommand = new Command(Refresh);
        }

    }

    public class Box
    {
        public double Rango { get; set; }
        public double Maximo { get; set; } 
        public double Minimo { get; set; }
        public double Quartil1 { get; set; }
        public double Quartil2 { get; set; }
        public double Quartil3 { get; set; }
    }
}
