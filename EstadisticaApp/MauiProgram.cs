using EstadisticaApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using UraniumUI;
using CommunityToolkit.Maui;

namespace EstadisticaApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).ConfigureMauiHandlers(handlers =>
        {
            handlers.AddUraniumUIHandlers();
        }).UseMauiCommunityToolkit();

        string dbPath = FileAccessHelper.GetLocalFilePath("Data.db3");
        builder.Services.AddSingleton<EstatisdicaRepository>(s => ActivatorUtilities.CreateInstance<EstatisdicaRepository>(s, dbPath));
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}