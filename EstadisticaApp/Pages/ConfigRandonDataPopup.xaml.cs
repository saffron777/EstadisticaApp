using CommunityToolkit.Maui.Views;
using EstadisticaApp.Models;
namespace EstadisticaApp.Pages;

public partial class ConfigRandonDataPopup : Popup
{
	public ConfigRandonDataPopup()
	{
		InitializeComponent();
		//BindingContext = new DataGridPageViewModel();
		
		////this.SetDynamicResource(Button.CommandParameterProperty, )
		//var multiBinding = new MultiBinding();
		//multiBinding.Bindings.Add(new Binding { Path = "Text", Source = Muestra });
		//multiBinding.Bindings.Add(new Binding { Path = "Text", Source = MinVal });
		//multiBinding.Bindings.Add(new Binding { Path = "Text", Source = MaxVal});
		//multiBinding.Converter = new RndDataConverter();

  //      btnOk.Command = ((DataGridPageViewModel)BindingContext).RndCommand;
		
		//btnOk.CommandParameter = multiBinding;
		


  //      Muestra.BindingContext = BindingContext;
		//Muestra.SetBinding(Entry.TextProperty, "Muestra");

		//MinVal.BindingContext= BindingContext;
		//MinVal.SetBinding(Entry.TextProperty, "Min");

  //      MaxVal.BindingContext = BindingContext;
  //      MaxVal.SetBinding(Entry.TextProperty, "Max");
    }


    void OnCancelButtonClicked(object? sender, EventArgs e) => Close(false);
    void OnOKButtonClicked(object? sender, EventArgs e) => Close(false);


}