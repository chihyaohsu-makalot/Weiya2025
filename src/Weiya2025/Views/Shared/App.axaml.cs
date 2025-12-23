using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using Weiya2025.Assets;
using Weiya2025.Factories;
using Weiya2025.Utils;
using Weiya2025.Views.Pages;

namespace Weiya2025.Views.Shared
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            //註冊與注入
            var provider = DiServiceBuilder.Build();
            //取得語系
            if (Design.IsDesignMode)
            {
                i18n.Culture = new CultureInfo("zh-Hant");
            }
            else
            {
                var langFactory = provider!.GetRequiredService<LanguageFactory>();
                i18n.Culture = new CultureInfo(langFactory.GetCurrentLanguage());
            }

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = provider?.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}