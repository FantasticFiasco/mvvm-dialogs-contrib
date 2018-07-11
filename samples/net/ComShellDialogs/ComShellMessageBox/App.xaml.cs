using System.Windows;
using GalaSoft.MvvmLight.Ioc;
using MvvmDialogs;
using MvvmDialogs.Contrib.ComShellDialogs;

namespace ComShellMessageBox
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SimpleIoc.Default.Register<IDialogService>(() => new DialogService(frameworkDialogFactory: new ComShellDialogFactory()));
        }
    }
}
