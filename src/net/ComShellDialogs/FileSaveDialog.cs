using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    /// <summary>Provides static methods to display a COM Shell API File Save dialog window.</summary>
    internal static class FileSaveDialog
    {
        /// <summary>Shows the file save dialog. Returns null if the dialog cancelled. Otherwise returns the file path as specified by the user.</summary>
        /// <param name="parentWindowHandle">Handle to the Win32 window that will parent the dialog. This value can be NULL (IntPtr.Zero).</param>
        /// <param name="title">Text to display in the title bar of the window. This value may be null.</param>
        /// <param name="initialDirectory">Path to the initial directory to display in the dialog. This value may be null.</param>
        /// <param name="defaultFileName">The default file name to display. The user may select a different file name. This value may be null.</param>
        /// <param name="filters">Collection of filters to display in the file type selection drop-down menu. This value may be null or empty.</param>
        /// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select. This value is optional and defaults to -1 (in which case the first filter will be selected).</param>
        /// <returns>The full path to the selected file. Returns null if the dialog was canceled.</returns>
        public static String ShowDialog(IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex = -1)
        {
            NativeFileSaveDialog nfod = new NativeFileSaveDialog();
            try
            {
                return ShowDialogInner( nfod, parentWindowHandle, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex );
            }
            finally
            {
                Marshal.ReleaseComObject( nfod );
            }
        }

        private static readonly HResult HResult_Win32_Canceled = Utility.HResultFromWin32Error( (UInt32)Win32ErrorCode.Cancelled );

        private static String ShowDialogInner(IFileSaveDialog dialog, IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex = -1)
        {
            FileOpenOptions flags =
                FileOpenOptions.NoTestFileCreate |
                FileOpenOptions.PathMustExist |
                FileOpenOptions.ForceFilesystem |
                FileOpenOptions.OverwritePrompt;

            dialog.SetOptions( flags );
            
            if( title != null )
            {
                dialog.SetTitle( title );
            }

            if( initialDirectory != null )
            {
                IShellItem2 initialDirectoryShellItem = Utility.ParseShellItem2Name( initialDirectory );
                if( initialDirectoryShellItem != null )
                {
                    dialog.SetFolder( initialDirectoryShellItem );
                }
            }

//            if( initialSaveAsItem != null )
//            {
//                IShellItem2 initialSaveAsItemShellItem = Utility.ParseShellItem2Name( initialDirectory );
//                if( initialSaveAsItemShellItem != null )
//                {
//                    dialog.SetSaveAsItem( initialSaveAsItemShellItem );
//                }
//            }

            if( defaultFileName != null )
            {
                dialog.SetFileName( defaultFileName );
            }

            Utility.SetFilters( dialog, filters, selectedFilterZeroBasedIndex );

            HResult result = dialog.Show( parentWindowHandle );
            if( result == HResult.Ok )
            {
                IShellItem selectedItem;
                dialog.GetResult( out selectedItem );

                if( selectedItem != null )
                {
                    return Utility.GetFileNameFromShellItem( selectedItem );
                }
                else
                {
                    return null;
                }
            }
            else if( result == HResult_Win32_Canceled )
            {
                // Cancelled by user.
                return null;
            }
            else
            {
                UInt32 win32ErrorCode = Utility.Win32ErrorFromHResult( (UInt32)result );
                throw new Win32Exception( error: (Int32)win32ErrorCode );
            }
        }
    }
}
