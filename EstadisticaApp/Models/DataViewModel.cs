using CommunityToolkit.Maui.Views;
using EstadisticaApp.Pages;
using MathNet.Numerics.Random;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using static Java.Util.Jar.Attributes;

namespace EstadisticaApp.Models
{

    public class DataGridPageViewModel : BindableObject
    {
        private ObservableCollection<DataViewModel> items = new ObservableCollection<DataViewModel>();
        public ObservableCollection<DataViewModel> Items { get => items; protected set { items = value; OnPropertyChanged(); } }

        public ObservableCollection<DataViewModel> SelectedItems { get; set; } = new ObservableCollection<DataViewModel>();

        public ICommand RemoveSelectedCommand { get; set; }

        public ICommand AddNewCommand { get; set; }

        public ICommand RndCommand { get; set; }

        public ICommand PopupCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public DataViewModel Item { get; set; } = new DataViewModel();               

        public int Row { get; set; }

        private int _muestra;
        public int Muestra { 
            get => _muestra; 
            set 
            { 
                _muestra = value;
                OnPropertyChanged(nameof(Muestra));
            } 
        }

        private double _min;
        public double Min
        {
            get => _min;
            set
            {
                _min = value;
                OnPropertyChanged(nameof(Min));
            }
        }

        private double _max;
        public double Max
        {
            get => _max;
            set
            {
                _max = value;
                OnPropertyChanged(nameof(Max));
            }
        }

        protected void Initialize()
        {
            var datos = App.EstatisdicaRepository.GetDatos();

            try
            {
                items.Clear();
                datos.ForEach(x =>
                {
                    Items.Add(new DataViewModel { Id = x.Id, X = x.X, Y = x.Y });
                });
            }
            catch (Exception ex)
            {

                
            }
            
        }

        

        public DataGridPageViewModel()
        {

            Initialize();

            ClearCommand = new Command(() =>
            {
                Items.Clear();
                App.EstatisdicaRepository.ClearData();
            });

            RndCommand = new Command((n) =>
            {
                                
                if (n is RndDataModel obj)
                {
                    Items.Clear();
                    App.EstatisdicaRepository.ClearData();

                    for (int i = 1; i <= obj.Muestra; i++)
                    {
                        var random = new Random();
                        var rndX = random.NextDecimal() * (decimal)(obj.Max - obj.Min) + (decimal)obj.Min;
                        var rndY = random.NextDecimal() * (decimal)(obj.Max - obj.Min) + (decimal)obj.Min;

                        App.EstatisdicaRepository.AddNewData(Math.Round(rndX,2), Math.Round(rndY,2));

                        int maxId = 0;
                        var registros = App.EstatisdicaRepository.GetDatos();
                        if (registros.Count > 0)
                            maxId = registros.Max(x => x.Id);

                        Items.Insert(Row, new DataViewModel
                        {
                            Id = maxId,
                            X = Math.Round(rndX,2),
                            Y = Math.Round(rndY,2),
                        });
                    }
                   
                }
            });

            PopupCommand = new Command(async (item) =>
            {
                ConfigRandonDataPopup popup = new ConfigRandonDataPopup();

                var result = await Application.Current.MainPage.ShowPopupAsync<ConfigRandonDataPopup>(popup);

                if(result  is Boolean bResult)
                {
                    Initialize();
                }
            });

            LoadCommand = new Command(() =>
            {
                Initialize();
            });

            AddNewCommand = new Command((item) =>
            {
                if (item is DataViewModel data)
                {
                    App.EstatisdicaRepository.AddNewData(data.X.Value, data.Y.Value);

                    int maxId = 0;
                    var registros = App.EstatisdicaRepository.GetDatos();
                    if (registros.Count > 0)
                    maxId = registros.Max(x => x.Id);

                    Items.Insert(Row, new DataViewModel
                    {
                        Id = maxId,
                        X = data.X,
                        Y = data.Y,
                    });

                    Item.X = null;
                    Item.Y= null;


                }                    
            });

            UpdateCommand = new Command((item) =>
            {
                if (item is DataViewModel data)
                {
                    var reg = items.FirstOrDefault(x => x.Id == data.Id);
                    
                    if (reg != null)
                    {
                        reg.X = data.X;
                        reg.Y = data.Y; 
                        App.EstatisdicaRepository.UpdateData(data.Id, data.X.Value, data.Y.Value);
                    }

                    
                }
            });

            RemoveSelectedCommand = new Command((item) =>
            {
                if (item is DataViewModel data)
                {
                    items.Remove(data);
                    App.EstatisdicaRepository.DeleteData(data.Id);
                }
            });
        }
    }

    public class DataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null && values[1] != null && values.Length == 2)
            {
                decimal x = decimal.Parse(values[0].ToString());
                decimal y = decimal.Parse(values[1].ToString());
                return new DataViewModel {  Id = 0, X = x, Y = y };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RndDataConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (values[0] != null && values[1] != null && values[2] != null && (!string.IsNullOrWhiteSpace(values[0].ToString()) && !string.IsNullOrWhiteSpace(values[1].ToString()) && !string.IsNullOrWhiteSpace(values[2].ToString())) && values.Length == 3)
            {
                int muestra ;
                double min ;
                double max ;

                int.TryParse(values[0].ToString(), out muestra);
                double.TryParse(values[1].ToString(), out min);
                double.TryParse(values[2].ToString(), out max);


                return new RndDataModel { Muestra = muestra, Min = min, Max = max };
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class RndDataModel
    {
        private int _muestra;
        public int Muestra
        {
            get => _muestra;
            set
            {
                _muestra = value;
               
            }
        }

        private double _min;
        public double Min
        {
            get => _min;
            set
            {
                _min = value;
               
            }
        }

        private double _max;
        public double Max
        {
            get => _max;
            set
            {
                _max = value;
               
            }
        }
    }

    public class DataViewModel:INotifyPropertyChanged
    {
        private int _id;
        public int Id { 
            get => _id; 
            set
            {
                if (_id == value)
                    return;

                _id = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            } 
        }

        private decimal? _x = null;
        public decimal? X 
        { 
            get => _x;
            set {
                if (_x == value)
                    return;

                _x = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(X)));
            } 
        }


        private decimal? _y = null;

        public decimal? Y 
        { 
            get => _y;
            set 
            {
                if (_y == value) return;

                _y = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Y)));
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
