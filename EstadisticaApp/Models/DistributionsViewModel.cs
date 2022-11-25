using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MathNet.Numerics.Distributions;

namespace EstadisticaApp.Models
{
    public class DistributionsViewModel : INotifyPropertyChanged
    {
        private double _media;

        public double Media
        {
            get { return _media; }
            set { if (_media == value) return; _media = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Media))); }
        }

        private double _desv;

        public double DesviacionStandard
        {
            get { return _desv; }
            set { if (_desv == value) return;  _desv = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DesviacionStandard))); }
        }

        private double _valor;

        public double Valor
        {
            get { return _valor; }
            set { if (_valor == value) return; _valor = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Valor))); }
        }

        private double _pvalor;

        public double Pvalor
        {
            get { return _pvalor; }
            set { if (_pvalor == value) return; _pvalor = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pvalor))); }
        }


        private double _gradosLibertad;

        public double GradosLibertad
        {
            get { return _gradosLibertad; }
            set { if (_gradosLibertad == value) return;  _gradosLibertad = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GradosLibertad))); }
        }

        private int _n;

        public int n
        {
            get { return _n; }
            set { if (_n == value) return; _n = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(n))); }
        }

        private double _probabilidad;

        public double Probabilidad
        {
            get { return _probabilidad; }
            set { _probabilidad = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Probabilidad))); }
        }

        private double _densidad;

        public double Densidad
        {
            get { return _densidad; }
            set { _densidad = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Densidad))); }
        }


        public ICommand NormalDistCommand { get; set; }
        public DistributionsViewModel() {
            //MathNet.Numerics.Distributions.Normal normal = new Normal(0, 1);

            //MathNet.Numerics.Distributions.ChiSquared chiSquared = new ChiSquared(1);

            //MathNet.Numerics.Distributions.Binomial binomial = new Binomial(1, 10);
            //MathNet.Numerics.Distributions.Poisson poisson = new Poisson(1);

            NormalDistCommand = new Command(() =>
            {
                Normal normal = new(Media, DesviacionStandard);

                Probabilidad = normal.CumulativeDistribution(Valor);

                Densidad = normal.Density(Valor);
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
