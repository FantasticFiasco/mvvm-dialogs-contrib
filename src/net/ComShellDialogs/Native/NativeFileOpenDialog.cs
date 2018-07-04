using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    [ComImport]
    [Guid( ShellIIDGuid.IFileOpenDialog )]
    [CoClass( typeof( FileOpenDialogRCW ) )]
    internal interface NativeFileOpenDialog : IFileOpenDialog
    {
    }
}
