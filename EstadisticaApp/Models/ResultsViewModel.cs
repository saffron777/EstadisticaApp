using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearRegression;
using LiveChartsCore;

//using static Android.Content.ClipData;

namespace EstadisticaApp.Models
{
    public class ResultsViewModel : BindableObject
    {        
        public ICommand RefreshCommand { get; set; }
        private bool isBusy;
        public bool IsBusy { get => isBusy; set { isBusy = value; OnPropertyChanged(); } }
        public int n { get; set; }
        private List<Dato> datos { get; set; }
        public ResultsViewModel()
        {

            init();

            RefreshCommand = new Command(() =>
            {
                init();            
                
                SerieViewModel serieViewModel = new SerieViewModel();
                serieViewModel.Refresh();
            });
        }

        private void init()
        {
            datos = App.EstatisdicaRepository.GetDatos();
            n = datos.Count();          

            _resultados.Clear();
            Resultados = new ObservableCollection<Result>();
            Calcular();
        }

        private ObservableCollection<Result> _resultados = new ObservableCollection<Result>();
        public ObservableCollection<Result> Resultados { get => _resultados; set { _resultados = value; OnPropertyChanged(); } }

        private Dictionary<string, double> Media()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var mediax = (double)datos.Average(x => x.X);
            var mediay = (double)datos.Average(x => x.Y);

            result.Add("Media X =", Math.Round(mediax, 3));
            result.Add("Media Y =", Math.Round(mediay,3));

            return result;
        }

        private Dictionary<string, double> Varianza()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var vari = varianzas();

            result.Add("Var X =", vari[0]);
            result.Add("Var Y =", vari[1]);


            return result;
        }

        private Dictionary<string, double> Desviacion()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var vari = varianzas();

            result.Add("Desv X =", Math.Round(Math.Sqrt(vari[0]),3));
            result.Add("Desv Y =", Math.Round(Math.Sqrt(vari[1]),3));


            return result;
        }

        private List<double> varianzas()
        {
            var result = new List<double>();

            var mediax = (double)datos.Average(x => x.X);
            var mediay = (double)datos.Average(x => x.Y);

            double varx = 0;
            datos.ForEach(x => {
                var d = (double)x.X - (double)mediax;
                var p = Math.Pow(d, 2);
                varx += p;
            });
            varx /= n;

            result.Add(Math.Round(varx,3));

            double vary = 0;
            datos.ForEach(x => {
                var d = (double)x.Y - (double)mediay;
                var p = Math.Pow(d, 2);
                vary += p;
            });
            vary /= n;

            result.Add(Math.Round(vary,3));

            return result;
        }


        private Dictionary<string, double> Suma()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var sumax = (double)datos.Sum(x => x.X);
            var sumay = (double)datos.Sum(x => x.Y);

            result.Add("SUM X =", sumax);
            result.Add("SUM Y =", sumay);

            return result;
        }

        private Dictionary<string, double> SumaQ()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var sumax2 = (double)datos.Sum(x => Math.Pow((double)x.X, 2));
            var sumay2 = (double)datos.Sum(x => Math.Pow((double)x.Y, 2));

            result.Add("SUM X^2 =", sumax2);
            result.Add("SUM Y^2 =", sumay2);

            return result;
        }

        private Dictionary<string, double> Max()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var maxx = (double)datos.Max(x => x.X);
            var maxy = (double)datos.Max(x => x.Y);

            result.Add("Max X =", maxx);
            result.Add("Max Y =", maxy);

            return result;
        }


        private Dictionary<string, double> Min()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var maxx = (double)datos.Min(x => x.X);
            var maxy = (double)datos.Min(x => x.Y);

            result.Add("Min X =", maxx);
            result.Add("Min Y =", maxy);

            return result;
        }

        private Dictionary<string, double> Multxy()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();


            var sumxy = multi();

            result.Add("SUM XY =", sumxy);

            return result;
        }

        private double multi()
        {
            return (double)datos.Sum(x => x.X * x.Y);
        }

        private Dictionary<string, double> Covxy() { 
            Dictionary<string, double> result = new Dictionary<string, double>();

            var cov = covarianza();

            result.Add("COV XY =", cov);

            return result;
        }

        private double covarianza()
        {
            var sumxy = (double)datos.Sum(x => x.X * x.Y);
            var mediax = (double)datos.Average(x => x.X);
            var mediay = (double)datos.Average(x => x.Y);


            var cov = (sumxy / n) - mediax * mediay;

            return Math.Round(cov,4);
        }


        private double pearson()
        {
            var cov = covarianza();

            var varianzas_m = varianzas();


            var result = cov / Math.Sqrt(varianzas_m[0] * varianzas_m[1]);

            return result;
        }

        private Dictionary<string, double> Pearson()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var pear = pearson();

            result.Add("Pearson =", Math.Round(pear, 4));

            return result;
        }


        private Dictionary<string, double> Total()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            
            result.Add("N =", n);

            return result;
        }


        private Dictionary<string, double> Mediana()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var m_x = datos.Select(x=> (double)x.X).AsEnumerable();
            var m_y = datos.Select(x=> (double)x.Y).AsEnumerable();

            var mediax = m_x.Median();
            var mediay = m_y.Median();

            
            result.Add("MEDIANA X =", Math.Round(mediax, 3));
            result.Add("MEDIANA Y =", Math.Round(mediay,3));

            return result;
        }

        private Dictionary<string, double> Moda()
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var m_x = datos.Select(x => (double)x.X).AsEnumerable();

            MathNet.Numerics.Distributions.Normal normal = new Normal(0, 1);

            

            return result;
        }


        private Dictionary<string, double> Percentil(int n)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            var m_x = datos.Select(x => (double)x.X).AsEnumerable();
            var m_y = datos.Select(x => (double)x.Y).AsEnumerable();

            var px = m_x.Percentile(n);
            var py = m_y.Percentile(n);

            result.Add($"PERCENTIL({n}) X =", Math.Round(px, 3));
            result.Add($"PERCENTIL({n}) Y =", Math.Round(py,3));

            return result;
        }
        private void Calcular()
        {
           
            if (n == 0)
                return;

            IsBusy = true;

            Dictionary<string, Dictionary<string, double>> results = new Dictionary<string, Dictionary<string, double>>();

            results.Add("TOTAL", Total());
            results.Add("MIN", Min());
            results.Add("MAX", Max());
            results.Add("MEDIA", Media());
            results.Add("MEDIANA", Mediana());
            results.Add("PERCENTIL25", Percentil(25));
            results.Add("PERCENTIL75", Percentil(75));
            results.Add("VAR", Varianza());
            results.Add("DESV", Desviacion());
            results.Add("SUM", Suma());
            results.Add("SUMQ", SumaQ());
            results.Add("SUMXY", Multxy());
            results.Add("COVXY", Covxy());
            results.Add("PEARSON", Pearson());


            foreach (var dic in results)
            {

                var dic_i = dic.Value;

                if (dic.Key == "MEDIA")
                {                   
                 
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Media X =",
                        ResultadoL = dic_i["Media X ="],
                        Etiquetar = "Media Y = ",
                        ResultadoR = dic_i["Media Y ="]
                    });
                }
                else if(dic.Key=="MIN")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Min X = ",
                        ResultadoL = dic_i["Min X ="],
                        Etiquetar = "Min Y = ",
                        ResultadoR = dic_i["Min Y ="]
                    });
                }
                else if (dic.Key == "MAX")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Max X = ",
                        ResultadoL = dic_i["Max X ="],
                        Etiquetar = "Max Y = ",
                        ResultadoR = dic_i["Max Y ="]
                    });
                }
                else if (dic.Key == "PERCENTIL25")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "QUARTIL 1 X = ",
                        ResultadoL = dic_i["PERCENTIL(25) X ="],
                        Etiquetar = "QUARTIL 1 Y = ",
                        ResultadoR = dic_i["PERCENTIL(25) Y ="]
                    });
                }
                else if (dic.Key == "PERCENTIL75")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "QUARTIL 3 X = ",
                        ResultadoL = dic_i["PERCENTIL(75) X ="],
                        Etiquetar = "QUARTIL 3 Y = ",
                        ResultadoR = dic_i["PERCENTIL(75) Y ="]
                    });
                }
                else if (dic.Key == "MEDIANA")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "MEDIANA X =",
                        ResultadoL = dic_i["MEDIANA X ="],
                        Etiquetar = "MEDIANA Y =",
                        ResultadoR = dic_i["MEDIANA Y ="]
                    });
                }
                else if (dic.Key == "VAR")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Var X =",
                        ResultadoL = dic_i["Var X ="],
                        Etiquetar = "Var Y =",
                        ResultadoR = dic_i["Var Y ="]
                    });
                }
                else if (dic.Key == "DESV")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Desv X =",
                        ResultadoL = dic_i["Desv X ="],
                        Etiquetar = "Desv Y =",
                        ResultadoR = dic_i["Desv Y ="]
                    });
                }
                else if (dic.Key=="SUM")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "SUM X =",
                        ResultadoL = dic_i["SUM X ="],
                        Etiquetar = "SUM Y =",
                        ResultadoR = dic_i["SUM Y ="]
                    });
                }
                else if (dic.Key == "SUMQ")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "SUM X^2 =",
                        ResultadoL = dic_i["SUM X^2 ="],
                        Etiquetar = "SUM Y^2 =",
                        ResultadoR = dic_i["SUM Y^2 ="]
                    });
                }
                else if(dic.Key == "SUMXY")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "SUM XY =",
                        ResultadoL = dic_i["SUM XY ="],                        
                    });
                }
                else if (dic.Key == "COVXY")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "COV XY =",
                        ResultadoL = dic_i["COV XY ="],
                    });
                }
                else if (dic.Key == "PEARSON")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "Pearson =",
                        ResultadoL = dic_i["Pearson ="],
                    });
                }
                else if (dic.Key == "TOTAL")
                {
                    Resultados.Add(new Result
                    {
                        EtiquetaL = "N =",
                        ResultadoL = dic_i["N ="],
                    });
                }


            }
            IsBusy = false; 

        }

    }
    public class Result : INotifyPropertyChanged
    {

        double _ResultadoL;
        public double ResultadoL
        {
            get => _ResultadoL;
            set
            {
                if (_ResultadoL == value)
                {
                    return;
                }


                _ResultadoL = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultadoL)));
            }
        }

        double? _ResultadoR;
        public double? ResultadoR
        {
            get => _ResultadoR; 
            set
            {
                if (_ResultadoR == value)
                {
                    return;
                }


                _ResultadoR = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ResultadoR)));
            } 
        }


        public string EtiquetaL { get; set; }
        public string Etiquetar { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
