using EstadisticaApp.Data;

namespace EstadisticaApp;

public partial class App : Application
{
    public static EstatisdicaRepository EstatisdicaRepository { get; private set; }
    public App(EstatisdicaRepository estatisdicaRepository)
	{
		InitializeComponent();

		MainPage = new AppShell();

        EstatisdicaRepository = estatisdicaRepository;
        
    }
}
