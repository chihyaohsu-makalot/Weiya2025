using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weiya2025.Factories;
using Weiya2025.Options;
using Weiya2025.Utils;
using Weiya2025.ViewModels;

namespace Weiya2025.Views.Pages
{
    internal partial class MainWindow : Window
    {
#if DEBUG
        public MainWindow()
        {
            InitializeComponent();
        }
#endif

        protected readonly ILogger _logger;
        protected readonly IServiceProvider _provider;
        //全部的人
        protected readonly WeiyaOption _waiya;

        public MainWindow(ILogger<MainWindow> logger, IServiceProvider provider, MainWindowViewModel model, IOptions<WeiyaOption> waiya)
        {
            _logger = logger;
            _provider = provider;
            _waiya = waiya.Value;

            //view model
            this.DataContext = model;

            //UI initial
            InitializeComponent();
        }

        #region Binding
        /// <summary>
        /// View Model
        /// </summary>
        private MainWindowViewModel _model => (MainWindowViewModel)DataContext!;
        #endregion Binding

        #region inject
        /// <summary>
        /// 得獎者紀錄器
        /// </summary>
        private RewardFactory _reward => _provider.GetRequiredService<RewardFactory>();
        #endregion inject

        #region events
        /// <summary>
        /// 畫面載入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowLoaded(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
#if DEBUG
            //為了 by pass avalonia IDE
            if (Design.IsDesignMode) return;
#endif

            //檢查身分 如果不需要可以註解
            SetInitialUiValue();
        }

        /// <summary>
        /// 按鈕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnButtonClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
#if DEBUG
            //為了 by pass avalonia IDE
            if (Design.IsDesignMode) return;
#endif
            try
            {
                switch (sender)
                {
                    //500 元 32 人
                    case Button btn when btn.Name!.Equals("btnTake1"):
                        foreach (var user in SelectUser(32, _model.Taken1Users.Count))
                        {
                            await PrintAndSaveUsers(user, u =>
                            {
                                _model.Taken1Users.Add(u);
                                _reward.SetRewardUser("500_1", $"{u.Division}@{u.Department}@{++_model.TotalUserCount}", u.Name);
                            });
                        }
                        break;
                    //500 元 32 人
                    case Button btn when btn.Name!.Equals("btnTake2"):
                        if (!_model.Taken1Users.Any() || _model.Taken1Users.Count() < 32)
                        {
                            await DialogsHelper.WarningAsync(this, "請先抽出第一輪 '獎金 500 元 32 人'", string.Empty);
                            return;
                        }

                        foreach (var user in SelectUser(32, _model.Taken2Users.Count))
                        {
                            await PrintAndSaveUsers(user, u =>
                            {
                                _model.Taken2Users.Add(u);
                                _reward.SetRewardUser("500_2", $"{u.Division}@{u.Department}@{++_model.TotalUserCount}", u.Name);
                            });
                        }
                        break;
                    //1000 元 10 人
                    case Button btn when btn.Name!.Equals("btnTake3"):
                        if (!_model.Taken2Users.Any() || _model.Taken2Users.Count() < 32)
                        {
                            await DialogsHelper.WarningAsync(this, "請先抽出第二輪 '獎金 500 元 32 人'", string.Empty);
                            return;
                        }

                        foreach (var user in SelectUser(10, _model.Taken3Users.Count))
                        {
                            await PrintAndSaveUsers(user, u =>
                            {
                                _model.Taken3Users.Add(u);
                                _reward.SetRewardUser("1000_1", $"{u.Division}@{u.Department}@{++_model.TotalUserCount}", u.Name);
                            });
                        }
                        break;
                    //1000 元 10 人
                    case Button btn when btn.Name!.Equals("btnTake4"):
                        if (!_model.Taken3Users.Any() || _model.Taken2Users.Count() < 10)
                        {
                            await DialogsHelper.WarningAsync(this, "請先抽出第三輪 '獎金 1000 元 10 人'", string.Empty);
                            return;
                        }

                        foreach (var user in SelectUser(10, _model.Taken4Users.Count))
                        {
                            await PrintAndSaveUsers(user, u =>
                            {
                                _model.Taken4Users.Add(u);
                                _reward.SetRewardUser("1000_2", $"{u.Division}@{u.Department}@{++_model.TotalUserCount}", u.Name);
                            });
                        }
                        break;
                }
            }
            catch (Exception err)
            {
                await DialogsHelper.ErrorAsync(this, err.GetBaseException().Message, string.Empty);
            }
        }

        #endregion events

        #region private void
        /// <summary>
        /// 初始化畫面資料
        /// </summary>
        private async void SetInitialUiValue()
        {
            if (_model is null)
                return;

            _model.MyName = "";
            _model.Time = DateTimeOffset.Now;

            //載入已經抽過的人
            _reward.ReloadRewardedUser("500_1", _model.Taken1Users);
            _reward.ReloadRewardedUser("500_2", _model.Taken2Users);
            _reward.ReloadRewardedUser("1000_1", _model.Taken3Users);
            _reward.ReloadRewardedUser("1000_2", _model.Taken4Users);
            _model.TotalUserCount = (ushort)(_model.Taken1Users.Count + _model.Taken2Users.Count + _model.Taken3Users.Count + _model.Taken4Users.Count);
        }

        /// <summary>
        /// 取得抽到的人
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<UserOption> SelectUser(int count, int checkUserCollectionCount)
        {
            var take = count - checkUserCollectionCount;
            if (take <= 0)
            {
                throw new Exception("本輪已全部抽出");
            }

            var nextCollection = _waiya.Users.Where(w => !_model.Taken1Users.Any(t => t.Division == w.Division && t.Department == w.Department && t.Name == w.Name))
                .Where(w => !_model.Taken2Users.Any(t => t.Division == w.Division && t.Department == w.Department && t.Name == w.Name))
                .Where(w => !_model.Taken3Users.Any(t => t.Division == w.Division && t.Department == w.Department && t.Name == w.Name))
                .Where(w => !_model.Taken4Users.Any(t => t.Division == w.Division && t.Department == w.Department && t.Name == w.Name));

            var currentRound = new List<UserOption>();

            foreach (var next in Enumerable.Range(0, take))
            {
                var users = nextCollection
                    //過濾這輪已經抽過的人
                    .Where(w => !currentRound.Any(t => t.Division == w.Division && t.Department == w.Department && t.Name == w.Name))
                    //打亂
                    .OrderBy(o => Guid.NewGuid());

                //用亂數取得一個人
                var user = users.Skip(new Random().Next(users.Count())).First();

                //加入本輪清單
                currentRound.Add(user);

                yield return user;
            }
        }
        /// <summary>
        /// 唱名並且存檔
        /// </summary>
        /// <param name="user">唱名</param>
        /// <param name="save">存檔</param>
        /// <returns></returns>
        private async Task PrintAndSaveUsers(UserOption user, Action<UserOption> save)
        {
            await DialogsHelper.WaitAsync(this, user.Division, string.Empty, async () =>
            {
                await Task.Delay(2 * 1000);

                await DialogsHelper.WaitAsync(this, user.Department, string.Empty, async () =>
                {
                    await Task.Delay(2 * 1000);

                    await DialogsHelper.WaitAsync(this, user.Name, string.Empty, async () =>
                    {
                        await Task.Delay(2 * 1000);
                        save(user);
                    });
                });
            });
        }
        #endregion private void

    }
}