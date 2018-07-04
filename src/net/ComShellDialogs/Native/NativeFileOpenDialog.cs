using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    [ComImport]
    [Guid( ShellIIDGuid.IFileOpenDialog )]
    [CoClass( typeof( FileOpenDialogRCW ) )]
    internal interface NativeFileOpenDialog : IFileOpenDialog
    {
    }
}
