using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;
using MvvmDialogs.Contrib.ComShellDialogs;

namespace ComShellFolderBrowserDialog
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService(frameworkDialogFactory: new ComShellDialogFactory()));
        }
    }
}
