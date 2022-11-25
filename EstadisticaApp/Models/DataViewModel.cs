using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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

        public ICommand UpdateCommand { get; set; }

        public ICommand LoadCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public DataViewModel Item { get; set; }
        public int Row { get; set; }

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

        private decimal? _x;
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


        private decimal? _y;

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
