using Avalonia.Controls;
using Material.Dialog;
using Material.Dialog.Enums;
using Material.Dialog.Icons;
using Material.Dialog.Interfaces;
using System;
using System.Threading.Tasks;
using Weiya2025.Assets;

namespace Weiya2025.Utils
{
    internal static class DialogsHelper
    {
        /// <summary>
        /// 成功視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="autoClose"></param>
        /// <returns></returns>
        public static async Task SuccessAsync(Window parent, string title, string message, int autoClose = 3)
        {
            await DialogPopOnly(parent, title, message, DialogIconKind.Success, autoClose);
        }

        /// <summary>
        /// 錯誤視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="autoClose"></param>
        /// <returns></returns>
        public static async Task ErrorAsync(Window parent, string title, string message, int autoClose = 3)
        {
            await DialogPopOnly(parent, title, message, DialogIconKind.Error, autoClose);
        }

        /// <summary>
        /// 訊息視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="autoClose"></param>
        /// <returns></returns>
        public static async Task InfoAsync(Window parent, string title, string message, int autoClose = 3)
        {
            await DialogPopOnly(parent, title, message, DialogIconKind.Info, autoClose);
        }

        /// <summary>
        /// 警告視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="autoClose"></param>
        /// <returns></returns>
        public static async Task WarningAsync(Window parent, string title, string message, int autoClose = 3)
        {
            await DialogPopOnly(parent, title, message, DialogIconKind.Warning, autoClose);
        }

        /// <summary>
        /// 等待完成作業視窗
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T> WaitAsync<T>(Window parent, string title, string message, Func<Task<T>> task)
        {
            parent.IsEnabled = false;
            var dialog = CreateDialog(title, message, DialogIconKind.Stop);
            _ = dialog.ShowDialog(parent);

            //執行 長等待
            var result = await task();

            dialog.GetWindow().Close();
            parent.IsEnabled = true;
            return result;
        }
        /// <summary>
        /// 等待完成作業視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task WaitAsync(Window parent, string title, string message, Func<Task> task)
        {
            parent.IsEnabled = false;
            var dialog = CreateDialog(title, message, DialogIconKind.Stop);
            _ = dialog.ShowDialog(parent);

            //執行 長等待
            await task();

            dialog.GetWindow().Close();
            parent.IsEnabled = true;
        }

        /// <summary>
        /// 詢問後執行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<T?> AskAsync<T>(Window parent, string title, string message, Func<Task<T>> task)
        {
            parent.IsEnabled = false;
            var dialog = CreateDialog(title, message, DialogIconKind.Help, DialogButtonsEnum.OkCancel);
            var confirm = await dialog.ShowDialog(parent);

            if (!(confirm?.GetResult?.Equals(DialogHelper.DIALOG_RESULT_OK) ?? false))
            {
                parent.IsEnabled = true;
                return default(T);
            }

            var result = await WaitAsync(parent, i18n.wait_processing, i18n.wait_waiting, task);
            parent.IsEnabled = true;
            return result;
        }
        /// <summary>
        /// 詢問後執行
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task AskAsync(Window parent, string title, string message, Func<Task> task)
        {
            parent.IsEnabled = false;
            var dialog = CreateDialog(title, message, DialogIconKind.Help, DialogButtonsEnum.OkCancel);
            var confirm = await dialog.ShowDialog(parent);

            if (!(confirm?.GetResult?.Equals(DialogHelper.DIALOG_RESULT_OK) ?? false))
            {
                parent.IsEnabled = true;
                return;
            }

            await WaitAsync(parent, i18n.wait_processing, i18n.wait_waiting, task);
            parent.IsEnabled = true;
        }

        #region private void
        /// <summary>
        /// 彈出視窗
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="icon"></param>
        /// <param name="autoClose"></param>
        /// <returns></returns>
        private static async Task DialogPopOnly(Window parent, string title, string message, DialogIconKind icon = DialogIconKind.Info, int autoClose = 3)
        {
            parent.IsEnabled = false;
            var dialog = CreateDialog(title, message, icon);
            _ = dialog.ShowDialog(parent);

            await Task.Delay((autoClose < 3 ? 3 : autoClose) * 1000);

            dialog.GetWindow().Close();
            parent.IsEnabled = true;
        }

        /// <summary>
        /// 等待視窗範本
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static IDialogWindow<DialogResult> CreateDialog(string title, string message, DialogIconKind icon = DialogIconKind.Info, DialogButtonsEnum? buttons = null)
        {
            //var dialog = DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams
            //{
            //    ContentHeader = title,
            //    SupportingText = message,
            //    StartupLocation = WindowStartupLocation.CenterOwner,
            //    DialogHeaderIcon = icon,
            //    Borderless = true,
            //    DialogButtons = buttons is null ? [] : DialogHelper.CreateSimpleDialogButtons(buttons.Value)
            //});

            var background = icon switch
            {
                DialogIconKind.Success => "#4BB543",
                DialogIconKind.Error => "#D75455",
                DialogIconKind.Warning => "#E98B2A",
                DialogIconKind.Info => "#3399FF",
                DialogIconKind.Help => "#9966FF",
                DialogIconKind.Stop => "#666666",
                _ => "#3399FF",
            };

            var dialog = DialogHelper.CreateCustomDialog(new CustomDialogBuilderParams
            {
                Width = icon == DialogIconKind.Stop ? 1200 : null,
                Content = new Label
                {
                    Padding = Avalonia.Thickness.Parse("20"),
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Content = title,
                    FontSize = 110,
                    FontFamily = Avalonia.Media.FontFamily.Parse("Microsoft YaHei"),
                    Background = Avalonia.Media.Brush.Parse(background),
                    Foreground = Avalonia.Media.Brush.Parse("#FFFFFF")
                },
                SupportingText = message,
                StartupLocation = WindowStartupLocation.CenterOwner,
                Borderless = true,
                DialogButtons = buttons is null ? [] : DialogHelper.CreateSimpleDialogButtons(buttons.Value)
            });

            dialog.GetWindow().ShowInTaskbar = false;
            return dialog;
        }
        #endregion private void

    }
}
