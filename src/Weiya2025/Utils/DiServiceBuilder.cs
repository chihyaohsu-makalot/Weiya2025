using Avalonia.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MK.Common.Utility.LocalLogServer.Extensions;
using System;
using System.IO;
using Weiya2025.Factories;
using Weiya2025.Options;
using Weiya2025.ViewModels;
using Weiya2025.Views.Pages;

namespace Weiya2025.Utils
{
    /// <summary>
    /// DI 相依注入建立
    /// </summary>
    internal class DiServiceBuilder
    {
        /// <summary>
        /// 建立 DI 服務
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider? Build()
        {
#if DEBUG
            //為了 by pass avalonia IDE
            if (Design.IsDesignMode)
            {
                return null;
            }
#endif

            #region appsettings
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            #endregion appsettings

            #region 註冊
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<WeiyaOption>(config.GetSection("Weiya"));
            //註冊 log
            services.AddMakalog(config.GetSection("LogServer"));
            services.AddMakalogJob();
            //語系設定
            services.AddTransient<LanguageFactory>();
            services.AddTransient<RewardFactory>();

            //view models
            services.AddSingleton<MainWindowViewModel>();

            //views - 主畫面
            services.AddSingleton<MainWindow>();
            #endregion 註冊

            var provider = services.BuildServiceProvider();
            //啟用紀錄
            provider.UseMakalog();

            return provider;
        }
    }
}
