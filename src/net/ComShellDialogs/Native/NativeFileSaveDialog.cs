using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    [ComImport]
    [Guid( ShellIIDGuid.IFileSaveDialog )]
    [CoClass( typeof( FileSaveDialogRCW ) )]
    internal interface NativeFileSaveDialog : IFileSaveDialog
    {
    }
}
