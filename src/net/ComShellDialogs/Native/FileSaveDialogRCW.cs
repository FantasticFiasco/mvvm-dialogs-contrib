using System.Runtime.InteropServices;

namespace MvvmDialogs.Contrib.ComShellDialogs.Native
{
    [ComImport]
    [ClassInterface( ClassInterfaceType.None )]
    [TypeLibType( TypeLibTypeFlags.FCanCreate )]
    [Guid( ShellCLSIDGuid.FileSaveDialog )]
    internal class FileSaveDialogRCW
    {
    }
}
