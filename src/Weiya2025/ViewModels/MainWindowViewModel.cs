using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Weiya2025.Options;

namespace Weiya2025.ViewModels
{
    internal partial class MainWindowViewModel : ObservableObject
    {
#if DEBUG
        public MainWindowViewModel() { }
#endif


        #region property defind
        /// <summary>
        /// 定義變數 Avlonia 會在建置後自動生成 public 對應變數 MyName
        /// <para>任何類別 型別 都可</para>
        /// </summary>
        [ObservableProperty]
        private string _myName;

        /// <summary>
        /// 時間定義
        /// <para>(Avalonia 時間物件要使用 <seealso cref="DateTimeOffset"/>)</para>
        /// </summary>
        [ObservableProperty]
        private DateTimeOffset _time;

        /**
        * 到時候需要載入已經抽過的人 避免全部被洗掉
        */

        /// <summary>
        /// 第一輪 已選擇使用者
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserOption> _taken1Users = new ObservableCollection<UserOption>();
        /// <summary>
        /// 第二輪 已選擇使用者
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserOption> _taken2Users = new ObservableCollection<UserOption>();
        /// <summary>
        /// 第三輪 已選擇使用者
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserOption> _taken3Users = new ObservableCollection<UserOption>();
        /// <summary>
        /// 第四輪 已選擇使用者
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<UserOption> _taken4Users = new ObservableCollection<UserOption>();
        /// <summary>
        /// 已抽出總人數
        /// </summary>
        [ObservableProperty]
        private ushort _totalUserCount;
        #endregion property defind
    }
}
