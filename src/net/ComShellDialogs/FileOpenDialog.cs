using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
	/// <summary>Provides static methods to display a COM Shell API File Open dialog window.</summary>
	public static class FileOpenDialog
	{
		/// <summary>Shows the file open dialog for multiple filename selections. Returns null if the dialog cancelled. Otherwise returns all selected paths.</summary>
		/// <param name="parentWindowHandle">Handle (hWnd) to the Win32 window that will parent the dialog. This value can be NULL (IntPtr.Zero).</param>
		/// <param name="title">Text to display in the title bar of the window. This value may be null.</param>
		/// <param name="initialDirectory">Path to the initial directory to display in the dialog. This value may be null.</param>
		/// <param name="defaultFileName">The default file name to display. The user may select a different file name. This value may be null.</param>
		/// <param name="filters">Collection of filters to display in the file type selection drop-down menu. This value may be null or empty.</param>
		/// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select. This value is optional and defaults to -1 (in which case the first filter will be selected).</param>
        /// <returns>An array of strings containing the full paths to each selected file. Returns null if the dialog was canceled.</returns>
		public static String[] ShowMultiSelectDialog(IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex)
		{
			return ShowDialog( parentWindowHandle, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.AllowMultiSelect );
		}

		/// <summary>Shows the file open dialog for a single filename selection. Returns null if the dialog cancelled. Otherwise returns the selected path.</summary>
		/// <param name="parentWindowHandle">Handle to the Win32 window that will parent the dialog. This value can be NULL (IntPtr.Zero).</param>
		/// <param name="title">Text to display in the title bar of the window. This value may be null.</param>
		/// <param name="initialDirectory">Path to the initial directory to display in the dialog. This value may be null.</param>
		/// <param name="defaultFileName">The default file name to display. The user may select a different file name. This value may be null.</param>
		/// <param name="filters">Collection of filters to display in the file type selection drop-down menu. This value may be null or empty.</param>
		/// <param name="selectedFilterZeroBasedIndex">0-based index of the filter to select.</param>
        /// <returns>The full path to the selected file. Returns null if the dialog was canceled.</returns>
		public static String ShowSingleSelectDialog(IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex)
		{
			String[] fileNames = ShowDialog( parentWindowHandle, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, FileOpenOptions.None );
			if( fileNames != null && fileNames.Length > 0 )
			{
				return fileNames[0];
			}
			else
			{
				return null;
			}
		}

		private static String[] ShowDialog(IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
		{
			NativeFileOpenDialog nfod = new NativeFileOpenDialog();
			try
			{
				return ShowDialogInner( nfod, parentWindowHandle, title, initialDirectory, defaultFileName, filters, selectedFilterZeroBasedIndex, flags );
			}
			finally
			{
				Marshal.ReleaseComObject( nfod );
			}
		}

		private static String[] ShowDialogInner(IFileOpenDialog dialog, IntPtr parentWindowHandle, String title, String initialDirectory, String defaultFileName, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex, FileOpenOptions flags)
		{
			flags = flags |
				FileOpenOptions.NoTestFileCreate |
				FileOpenOptions.PathMustExist |
				FileOpenOptions.ForceFilesystem;

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

			if( defaultFileName != null )
			{
				dialog.SetFileName( defaultFileName );
			}

			Utility.SetFilters( dialog, filters, selectedFilterZeroBasedIndex );

			HResult result = dialog.Show( parentWindowHandle );

			HResult cancelledAsHResult = Utility.HResultFromWin32( (int)HResult.Win32ErrorCanceled );
			if( result == cancelledAsHResult )
			{
				// Cancelled
				return null;
			}
			else
			{
				// OK

				IShellItemArray resultsArray;
				dialog.GetResults( out resultsArray );

				String[] fileNames = Utility.GetFileNames( resultsArray );
				return fileNames;
			}
		}
	}
}
