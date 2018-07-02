using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MvvmDialogs.ComShellDialogs
{
    internal static class Utility
    {
        public static String[] GetFileNames(IShellItemArray items)
        {
            UInt32 count;
            HResult hresult = items.GetCount( out count );
            if( hresult != HResult.Ok ) throw new InvalidOperationException( "IShellItemArray.GetCount failed. HResult: " + hresult );

            String[] fileNames = new String[ count ];

            for( UInt32 i = 0; i < count; i++ )
            {
                HResult result = items.GetItemAt( i, out IShellItem shellItem );
                if( result != HResult.Ok ) throw new InvalidOperationException( "IShellItemArray.GetItemAt( " + i + " ) failed. HResult: " + hresult );

                String fileName = Utility.GetFileNameFromShellItem( shellItem );

                fileNames[i] = fileName;
            }

            return fileNames;
        }

        private static readonly Guid _ishellItem2Guid = new Guid(ShellIIDGuid.IShellItem2);

        public static IShellItem2 ParseShellItem2Name( String value )
        {
            Guid ishellItem2GuidCopy = _ishellItem2Guid;

            IShellItem2 shellItem;
            HResult result = ShellNativeMethods.SHCreateItemFromParsingName( value, IntPtr.Zero, ref ishellItem2GuidCopy, out shellItem );
            if( result == HResult.Ok )
            {
                return shellItem;
            }
            else
            {
                // TODO: Ideally we'd interrogate each Win32 error to decide if this function should return null (e.g. nonsensical filename) or throw (some system error).
                // But that takes too much effort, so just always return null for now.
                return null;
            }
        }

        const UInt32 HResult_Facility_Mask    = 0x07FF_0000u;
        const UInt32 HResult_Facility_Win32   = 0x0007_0000u;
        const UInt32 HResult_Severity_Failure = 0x8000_0000u;
        const UInt32 HResult_Code_Mask        = 0x0000_FFFFu;

        public static HResult HResultFromWin32Error(UInt32 win32ErrorCode)
        {
            // HResult's can encompass a Win32 error code. See https://en.wikipedia.org/wiki/HRESULT
            if( (Int32)win32ErrorCode > 0 )
            {
                UInt32 win32Masked = win32ErrorCode & HResult_Code_Mask;

                UInt32 hresult = HResult_Severity_Failure | HResult_Facility_Win32 | win32Masked;
                return (HResult)hresult;
            }

            return (HResult)win32ErrorCode; // TODO: Is this correct?
        }

        public static UInt32 Win32ErrorFromHResult(UInt32 hresult)
        {
            Boolean isFailure = ( hresult & HResult_Severity_Failure ) == HResult_Severity_Failure;
            Boolean isWin32   = ( hresult & HResult_Facility_Win32   ) == HResult_Facility_Win32;

            if( isWin32 )
            {
                if( isFailure )
                {
                    return hresult & HResult_Code_Mask;
                }
                else
                {
                    return 0; // ERROR_SUCCESS
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException( nameof(hresult), hresult, message: "Value is not a Win32 error HRESULT." );
            }
        }

        public static String GetFileNameFromShellItem(IShellItem item)
        {
            string filename = null;
            IntPtr pszString = IntPtr.Zero;
            HResult hr = item.GetDisplayName( ShellItemDesignNameOptions.DesktopAbsoluteParsing, out pszString );
            if( hr == HResult.Ok && pszString != IntPtr.Zero )
            {
                filename = Marshal.PtrToStringAuto( pszString );
                Marshal.FreeCoTaskMem( pszString );
            }
            return filename;
        }

        public static void SetFilters(IFileDialog dialog, IReadOnlyCollection<Filter> filters, Int32 selectedFilterZeroBasedIndex)
        {
            if( filters == null || filters.Count == 0 ) return;

            FilterSpec[] specs = Utility.CreateFilterSpec( filters );
            dialog.SetFileTypes( (UInt32)specs.Length, specs );

            if( selectedFilterZeroBasedIndex > -1 && selectedFilterZeroBasedIndex < filters.Count )
            {
                dialog.SetFileTypeIndex( 1 + (UInt32)selectedFilterZeroBasedIndex ); // In the COM interface (like the other Windows OFD APIs), filter indexes are 1-based, not 0-based.
            }
        }

        public static FilterSpec[] CreateFilterSpec(IReadOnlyCollection<Filter> filters)
        {
            FilterSpec[] specs = new FilterSpec[ filters.Count ];
            Int32 i = 0;
            foreach( Filter filter in filters )
            {
                specs[i] = filter.ToFilterSpec();
                i++;
            }
            return specs;
        }
    }

    
}
