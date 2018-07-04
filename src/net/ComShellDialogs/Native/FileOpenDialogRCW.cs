using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    [ComImport]
    [ClassInterface( ClassInterfaceType.None )]
    [TypeLibType( TypeLibTypeFlags.FCanCreate )]
    [Guid( ShellCLSIDGuid.FileOpenDialog )]
    internal class FileOpenDialogRCW
    {
    }
}
