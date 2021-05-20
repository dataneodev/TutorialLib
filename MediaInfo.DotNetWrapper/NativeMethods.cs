using System;
using System.IO;
using System.Runtime.InteropServices;


namespace MediaInfo.DotNetWrapper
{
    internal static class NativeMethods
    {
        static NativeMethods()
        {
            var myFolder = AppDomain.CurrentDomain.BaseDirectory;
            var is64 = IntPtr.Size == 8;
            var subfolder = is64 ? "MediaInfox64" : "MediaInfox86";

            LoadLibrary(Path.Combine(myFolder, subfolder, "MediaInfo.dll"));
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        // Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfo_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr handle, long fileSize, long fileOffset);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open(IntPtr handle, long fileSize, long fileOffset);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr handle, IntPtr buffer, IntPtr bufferSize);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open_Buffer_Continue(
          IntPtr handle,
          long fileSize,
          byte[] buffer,
          IntPtr bufferSize);

        [DllImport("MediaInfo.dll")]
        internal static extern long MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern long MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfo_Close(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_GetI(
          IntPtr handle,
          IntPtr streamKind,
          IntPtr streamNumber,
          IntPtr parameter,
          IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_GetI(
          IntPtr handle,
          IntPtr streamKind,
          IntPtr streamNumber,
          IntPtr parameter,
          IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Get(
          IntPtr handle,
          IntPtr streamKind,
          IntPtr streamNumber,
          [MarshalAs(UnmanagedType.LPWStr)] string parameter,
          IntPtr kindOfInfo,
          IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Get(
          IntPtr handle,
          IntPtr streamKind,
          IntPtr streamNumber,
          IntPtr parameter,
          IntPtr kindOfInfo,
          IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Option(
          IntPtr handle,
          [MarshalAs(UnmanagedType.LPWStr)] string option,
          [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Option(IntPtr handle, IntPtr option, IntPtr value);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_State_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Count_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_New();

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfoList_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_Open(
          IntPtr handle,
          [MarshalAs(UnmanagedType.LPWStr)] string fileName,
          IntPtr options);

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfoList_Close(IntPtr handle, IntPtr filePos);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_Inform(IntPtr handle, IntPtr filePos, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_GetI(
          IntPtr handle,
          IntPtr filePos,
          IntPtr streamKind,
          IntPtr streamNumber,
          IntPtr parameter,
          IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_Get(
          IntPtr handle,
          IntPtr filePos,
          IntPtr streamKind,
          IntPtr streamNumber,
          [MarshalAs(UnmanagedType.LPWStr)] string parameter,
          IntPtr kindOfInfo,
          IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_Option(
          IntPtr handle,
          [MarshalAs(UnmanagedType.LPWStr)] string option,
          [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_State_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoList_Count_Get(
          IntPtr handle,
          IntPtr filePos,
          IntPtr streamKind,
          IntPtr streamNumber);
    }
}
