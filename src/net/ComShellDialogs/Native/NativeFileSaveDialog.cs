using System;
using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    [ComImport]
    [Guid( ShellIIDGuid.IFileSaveDialog )]
    [CoClass( typeof( FileSaveDialogRCW ) )]
    internal interface NativeFileSaveDialog : IFileSaveDialog
    {
    }
}
