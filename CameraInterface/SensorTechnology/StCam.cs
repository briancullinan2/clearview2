using System.Runtime.InteropServices;

namespace SensorTechnology
{
    // Token: 0x02000010 RID: 16
    public partial class StCam
    {
        // Token: 0x06000085 RID: 133 RVA: 0x00003F4D File Offset: 0x0000214D
        private StCam()
        {
        }

        // Token: 0x06000086 RID: 134
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_Open")]
        public static partial IntPtr Open(uint dwInstance);

        // Token: 0x06000087 RID: 135
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_Close")]
        public static partial void Close(IntPtr hCamera);

        // Token: 0x06000088 RID: 136
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetLastError")]
        public static partial uint GetLastError(IntPtr hCamera);

        // Token: 0x06000089 RID: 137
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_CameraCount")]
        public static partial uint CameraCount();

        // Token: 0x0600008A RID: 138
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetReceiveMsgWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetReceiveMsgWindow(IntPtr hCamera, IntPtr hWnd);

        // Token: 0x0600008B RID: 139
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetUSBSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetUSBSpeed(IntPtr hCamera, out byte pbyteUSBSpeed);

        // Token: 0x0600008C RID: 140
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetUSBMaxSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetUSBMaxSpeed(IntPtr hCamera, out byte pbyteUSBSpeed);

        // Token: 0x0600008D RID: 141
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetColorArray")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetColorArray(IntPtr hCamera, out ushort pwColorArray);

        // Token: 0x0600008E RID: 142
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnableTransferBitsPerPixel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnableTransferBitsPerPixel(IntPtr hCamera, out uint pdwEnableTransferBitsPerPixel);

        // Token: 0x0600008F RID: 143
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetTransferBitsPerPixel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetTransferBitsPerPixel(IntPtr hCamera, uint dwTransferBitsPerPixel);

        // Token: 0x06000090 RID: 144
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetTransferBitsPerPixel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetTransferBitsPerPixel(IntPtr hCamera, out uint pdwTransferBitsPerPixel);

        // Token: 0x06000091 RID: 145
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMaximumImageSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMaximumImageSize(IntPtr hCamera, out uint pdwMaxWidth, out uint pdwMaxHeight);

        // Token: 0x06000092 RID: 146
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnableImageSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnableImageSize(IntPtr hCamera, out uint pdwReserved, out ushort pwEnableScanMode);

        // Token: 0x06000093 RID: 147
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetImageSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetImageSize(IntPtr hCamera, uint dwReserved, ushort wScanMode, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

        // Token: 0x06000094 RID: 148
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetImageSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetImageSize(IntPtr hCamera, out uint pdwReserved, out ushort pwScanMode, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

        // Token: 0x06000095 RID: 149
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetSkippingAndBinning")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetSkippingAndBinning(IntPtr hCamera, out byte pbyteHSkipping, out byte pbyteVSkipping, out byte pbyteHBinning, out byte pbyteVBinning);

        // Token: 0x06000096 RID: 150
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetSkippingAndBinning")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetSkippingAndBinning(IntPtr hCamera, byte byteHSkipping, byte byteVSkipping, byte byteHBinning, byte byteVBinning);

        // Token: 0x06000097 RID: 151
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetBinningSumMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetBinningSumMode(IntPtr hCamera, out ushort pwMode);

        // Token: 0x06000098 RID: 152
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetBinningSumMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetBinningSumMode(IntPtr hCamera, ushort wMode);

        // Token: 0x06000099 RID: 153
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_StartTransfer")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool StartTransfer(IntPtr hCamera);

        // Token: 0x0600009A RID: 154
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_StopTransfer")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool StopTransfer(IntPtr hCamera);

        // Token: 0x0600009B RID: 155
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewPixelFormat")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewPixelFormat(IntPtr hCamera, uint dwPreviewPixelFormat);

        // Token: 0x0600009C RID: 156
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewPixelFormat")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewPixelFormat(IntPtr hCamera, out uint pdwPreviewPixelFormat);

        // Token: 0x0600009D RID: 157
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnablePreviewPixelFormat")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnablePreviewPixelFormat(IntPtr hCamera, out uint pdwEnablePreviewPixelFormat);

        // Token: 0x0600009E RID: 158
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetColorInterpolationMethod")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetColorInterpolationMethod(IntPtr hCamera, byte byteColorInterpolationMethod);

        // Token: 0x0600009F RID: 159
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetColorInterpolationMethod")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetColorInterpolationMethod(IntPtr hCamera, out byte pbyteColorInterpolationMethod);

        // Token: 0x060000A0 RID: 160
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_CreatePreviewWindowA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CreatePreviewWindowA(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, int bCloseEnable);

        // Token: 0x060000A1 RID: 161
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_CreatePreviewWindowW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CreatePreviewWindowW(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, int bCloseEnable);

        // Token: 0x060000A2 RID: 162
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_CreatePreviewWindow", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CreatePreviewWindow(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, int bCloseEnable);

        // Token: 0x060000A3 RID: 163
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_DestroyPreviewWindow")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DestroyPreviewWindow(IntPtr hCamera);

        // Token: 0x060000A4 RID: 164
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowNameA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewWindowNameA(IntPtr hCamera, string pszWindowName);

        // Token: 0x060000A5 RID: 165
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowNameA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWindowNameA(IntPtr hCamera, out string pszWindowName, int lngMaxCount);

        // Token: 0x060000A6 RID: 166
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowNameW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewWindowNameW(IntPtr hCamera, string pszWindowName);

        // Token: 0x060000A7 RID: 167
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowName", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewWindowName(IntPtr hCamera, string pszWindowName);

        // Token: 0x060000A8 RID: 168
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowNameW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWindowNameW(IntPtr hCamera, out string pszWindowName, int lngMaxCount);

        // Token: 0x060000A9 RID: 169
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowName", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWindowName(IntPtr hCamera, out string pszWindowName, int lngMaxCount);

        // Token: 0x060000AA RID: 170
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewMaskSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewMaskSize(IntPtr hCamera, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

        // Token: 0x060000AB RID: 171
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewMaskSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewMaskSize(IntPtr hCamera, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

        // Token: 0x060000AC RID: 172
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewWindowSize(IntPtr hCamera, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight);

        // Token: 0x060000AD RID: 173
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWindowSize(IntPtr hCamera, out int plngPositionX, out int plngPositionY, out uint pdwWidth, out uint pdwHeight);

        // Token: 0x060000AE RID: 174
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowStyle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewWindowStyle(IntPtr hCamera, uint dwStyle);

        // Token: 0x060000AF RID: 175
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowStyle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWindowStyle(IntPtr hCamera, out uint pdwStyle);

        // Token: 0x060000B0 RID: 176
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAspectMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAspectMode(IntPtr hCamera, byte byteAspectMode);

        // Token: 0x060000B1 RID: 177
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAspectMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAspectMode(IntPtr hCamera, out byte pbyteAspectMode);

        // Token: 0x060000B2 RID: 178
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetPreviewDestSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetPreviewDestSize(IntPtr hCamera, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

        // Token: 0x060000B3 RID: 179
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewDestSize")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewDestSize(IntPtr hCamera, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

        // Token: 0x060000B4 RID: 180
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetMagnificationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetMagnificationMode(IntPtr hCamera, byte byteMagnificationMode);

        // Token: 0x060000B5 RID: 181
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMagnificationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMagnificationMode(IntPtr hCamera, out byte pbyteMagnificationMode);

        // Token: 0x060000B6 RID: 182
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDC")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDC(IntPtr hCamera, out IntPtr phDC);

        // Token: 0x060000B7 RID: 183
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDCWithReset")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDCWithReset(IntPtr hCamera, out IntPtr phDC);

        // Token: 0x060000B8 RID: 184
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ReleaseDC")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReleaseDC(IntPtr hCamera, IntPtr hDC);

        // Token: 0x060000B9 RID: 185
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ResetOverlay")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ResetOverlay(IntPtr hCamera);

        // Token: 0x060000BA RID: 186
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetColorKey")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetColorKey(IntPtr hCamera, uint dwColorKey);

        // Token: 0x060000BB RID: 187
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetColorKey")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetColorKey(IntPtr hCamera, out uint pdwColorKey);

        // Token: 0x060000BC RID: 188
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetDisplayMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDisplayMode(IntPtr hCamera, byte byteDisplayMode);

        // Token: 0x060000BD RID: 189
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDisplayMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDisplayMode(IntPtr hCamera, out byte pbyteDisplayMode);

        // Token: 0x060000BE RID: 190
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAlphaChannel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAlphaChannel(IntPtr hCamera, out byte pbyteAlphaChannel);

        // Token: 0x060000BF RID: 191
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAlphaChannel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAlphaChannel(IntPtr hCamera, byte byteAlphaChannel);

        // Token: 0x060000C0 RID: 192
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewImagePixelValue")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewImagePixelValue(IntPtr hCamera, uint nX, uint nY, out uint pdwColor);

        // Token: 0x060000C1 RID: 193
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWnd")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewWnd(IntPtr hCamera, out IntPtr hWnd);

        // Token: 0x060000C2 RID: 194
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_TakeRawSnapShot")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool TakeRawSnapShot(IntPtr hCamera, IntPtr pbyteBuffer, uint dwBufferSize, out uint pdwNumberOfByteTrans, uint[] pdwFrameNo, uint dwMilliseconds);

        // Token: 0x060000C3 RID: 195
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_TakePreviewSnapShot")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool TakePreviewSnapShot(IntPtr hCamera, IntPtr pbyteBuffer, uint dwBufferSize, out uint pdwNumberOfByteTrans, uint[] pdwFrameNo, uint dwMilliseconds);

        // Token: 0x060000C4 RID: 196
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveImageA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveImageA(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

        // Token: 0x060000C5 RID: 197
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveImageW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveImageW(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

        // Token: 0x060000C6 RID: 198
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveImage", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveImage(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

        // Token: 0x060000C7 RID: 199
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetALCMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetALCMode(IntPtr hCamera, out byte pbyteAlcMode);

        // Token: 0x060000C8 RID: 200
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetALCMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetALCMode(IntPtr hCamera, byte byteAlcMode);

        // Token: 0x060000C9 RID: 201
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetExposureClock(IntPtr hCamera, out uint pdwExposureClock);

        // Token: 0x060000CA RID: 202
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetExposureClock(IntPtr hCamera, uint dwExposureClock);

        // Token: 0x060000CB RID: 203
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMaxShortExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMaxShortExposureClock(IntPtr hCamera, out uint pdwMaximumExpClock);

        // Token: 0x060000CC RID: 204
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMaxLongExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMaxLongExposureClock(IntPtr hCamera, out uint pdwMaximumExpClock);

        // Token: 0x060000CD RID: 205
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetExposureTimeFromClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetExposureTimeFromClock(IntPtr hCamera, uint dwExposureClock, out float pfExpTime);

        // Token: 0x060000CE RID: 206
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetExposureClockFromTime")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetExposureClockFromTime(IntPtr hCamera, float fExpTime, out uint pdwExposureClock);

        // Token: 0x060000CF RID: 207
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetGain(IntPtr hCamera, out ushort pwGain);

        // Token: 0x060000D0 RID: 208
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetGain(IntPtr hCamera, ushort wGain);

        // Token: 0x060000D1 RID: 209
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMaxGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMaxGain(IntPtr hCamera, out ushort pwMaxGain);

        // Token: 0x060000D2 RID: 210
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetGainDBFromSettingValue")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetGainDBFromSettingValue(IntPtr hCamera, ushort wGain, out float pfGainDB);

        // Token: 0x060000D3 RID: 211
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetTargetBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetTargetBrightness(IntPtr hCamera, out byte pbyteTargetBrightness, out byte pbyteTolerance, out byte pbyteThreshold);

        // Token: 0x060000D4 RID: 212
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetTargetBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetTargetBrightness(IntPtr hCamera, byte byteTargetBrightness, byte byteTolerance, byte byteThreshold);

        // Token: 0x060000D5 RID: 213
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetALCWeight")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetALCWeight(IntPtr hCamera, byte[] pbyteALCWeight);

        // Token: 0x060000D6 RID: 214
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetALCWeight")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetALCWeight(IntPtr hCamera, byte[] pbyteALCWeight);

        // Token: 0x060000D7 RID: 215
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAEMinExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAEMinExposureClock(IntPtr hCamera, uint dwMinExposureClock);

        // Token: 0x060000D8 RID: 216
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAEMinExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAEMinExposureClock(IntPtr hCamera, out uint pdwMinExposureClock);

        // Token: 0x060000D9 RID: 217
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAEMaxExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAEMaxExposureClock(IntPtr hCamera, out uint pdwMaxExposureClock);

        // Token: 0x060000DA RID: 218
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAEMaxExposureClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAEMaxExposureClock(IntPtr hCamera, uint dwMaxExposureClock);

        // Token: 0x060000DB RID: 219
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetALCControlSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetALCControlSpeed(IntPtr hCamera, byte byteShutterCtrlSpeedLimit, byte byteGainCtrlSpeedLimit, byte byteSkipFrameCount, byte byteAverageFrameCount);

        // Token: 0x060000DC RID: 220
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetALCControlSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetALCControlSpeed(IntPtr hCamera, out byte pbyteShutterCtrlSpeedLimit, out byte pbyteGainCtrlSpeedLimit, out byte pbyteSkipFrameCount, out byte pbyteAverageFrameCount);

        // Token: 0x060000DD RID: 221
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetGainControlRange")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetGainControlRange(IntPtr hCamera, out ushort pwMinGain, out ushort pwMaxGain);

        // Token: 0x060000DE RID: 222
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetGainControlRange")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetGainControlRange(IntPtr hCamera, ushort wMinGain, ushort wMaxGain);

        // Token: 0x060000DF RID: 223
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDigitalGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDigitalGain(IntPtr hCamera, out ushort pwDigitalGain);

        // Token: 0x060000E0 RID: 224
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetDigitalGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDigitalGain(IntPtr hCamera, ushort wDigitalGain);

        // Token: 0x060000E1 RID: 225
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetShutterSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetShutterSpeed(IntPtr hCamera, out ushort pwShutterLine, out ushort pwShutterClock);

        // Token: 0x060000E2 RID: 226
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetShutterSpeed")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetShutterSpeed(IntPtr hCamera, ushort wShutterLine, ushort wShutterClock);

        // Token: 0x060000E3 RID: 227
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetShutterControlRange")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetShutterControlRange(IntPtr hCamera, out ushort pwMinShutterLine, out ushort pwMinShutterClock, out ushort pwMaxShutterLine, out ushort pwMaxShutterClock);

        // Token: 0x060000E4 RID: 228
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetShutterControlRange")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetShutterControlRange(IntPtr hCamera, ushort wMinShutterLine, ushort wMinShutterClock, ushort wMaxShutterLine, ushort wMaxShutterClock);

        // Token: 0x060000E5 RID: 229
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWhiteBalanceMode(IntPtr hCamera, byte byteWBMode);

        // Token: 0x060000E6 RID: 230
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWhiteBalanceMode(IntPtr hCamera, out byte pbyteWBMode);

        // Token: 0x060000E7 RID: 231
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWhiteBalanceGain(IntPtr hCamera, ushort wWBGainR, ushort wWBGainGr, ushort wWBGainGb, ushort wWBGainB);

        // Token: 0x060000E8 RID: 232
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceGain")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWhiteBalanceGain(IntPtr hCamera, out ushort pwWBGainR, out ushort pwWBGainGr, out ushort pwWBGainGb, out ushort pwWBGainB);

        // Token: 0x060000E9 RID: 233
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceTarget")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWhiteBalanceTarget(IntPtr hCamera, ushort wAWBTargetR, ushort wAWBTargetB);

        // Token: 0x060000EA RID: 234
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceTarget")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWhiteBalanceTarget(IntPtr hCamera, out ushort pwAWBTargetR, out ushort pwAWBTargetB);

        // Token: 0x060000EB RID: 235
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceToleranceThreshold")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetWhiteBalanceToleranceThreshold(IntPtr hCamera, ushort wAWBTolerance, ushort wAWBThreshold);

        // Token: 0x060000EC RID: 236
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceToleranceThreshold")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetWhiteBalanceToleranceThreshold(IntPtr hCamera, out ushort pwAWBTolerance, out ushort pwAWBThreshold);

        // Token: 0x060000ED RID: 237
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAWBWeight")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAWBWeight(IntPtr hCamera, byte[] pbyteAWBWeight);

        // Token: 0x060000EE RID: 238
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAWBWeight")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAWBWeight(IntPtr hCamera, byte[] pbyteAWBWeight);

        // Token: 0x060000EF RID: 239
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RawWhiteBalance")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RawWhiteBalance(IntPtr hCamera, uint dwWidth, uint dwHeight, ushort wColorArray, IntPtr pbyteRaw);

        // Token: 0x060000F0 RID: 240
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetGammaMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetGammaMode(IntPtr hCamera, byte byteGammaTarget, byte byteGammaMode, ushort wGamma, byte[] pbyteGammaTable);

        // Token: 0x060000F1 RID: 241
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetGammaMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetGammaMode(IntPtr hCamera, byte byteGammaTarget, out byte pbyteGammaMode, out ushort pwGamma, byte[] pbyteGammaTable);

        // Token: 0x060000F2 RID: 242
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetGammaModeEx")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetGammaModeEx(IntPtr hCamera, byte byteGammaTarget, byte byteGammaMode, ushort wGamma, short shtBrightness, byte byteContrast, byte[] pbyteGammaTable);

        // Token: 0x060000F3 RID: 243
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetGammaModeEx")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetGammaModeEx(IntPtr hCamera, byte byteGammaTarget, out byte pbyteGammaMode, out ushort pwGamma, out short pshtBrightness, out byte pbyteContrast, byte[] pbyteGammaTable);

        // Token: 0x060000F4 RID: 244
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetCameraGammaValue")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetCameraGammaValue(IntPtr hCamera, out ushort pwValue);

        // Token: 0x060000F5 RID: 245
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetCameraGammaValue")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetCameraGammaValue(IntPtr hCamera, ushort wValue);

        // Token: 0x060000F6 RID: 246
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetSharpnessMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetSharpnessMode(IntPtr hCamera, byte byteSharpnessMode, ushort wSharpnessGain, byte byteSharpnessCoring);

        // Token: 0x060000F7 RID: 247
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetSharpnessMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetSharpnessMode(IntPtr hCamera, out byte pbyteSharpnessMode, out ushort pwSharpnessGain, out byte pbyteSharpnessCoring);

        // Token: 0x060000F8 RID: 248
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetHueSaturationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetHueSaturationMode(IntPtr hCamera, byte byteHueSaturationMode, short shtHue, ushort wSaturation);

        // Token: 0x060000F9 RID: 249
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetHueSaturationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetHueSaturationMode(IntPtr hCamera, out byte pbyteHueSaturationMode, out short pshtHue, out ushort pwSaturation);

        // Token: 0x060000FA RID: 250
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetColorMatrix")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetColorMatrix(IntPtr hCamera, byte byteColorMatrixMode, short[] pshtColorMatrix);

        // Token: 0x060000FB RID: 251
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetColorMatrix")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetColorMatrix(IntPtr hCamera, out byte pbyteColorMatrixMode, short[] pshtColorMatrix);

        // Token: 0x060000FC RID: 252
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnableMirrorMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnableMirrorMode(IntPtr hCamera, out byte pbyteMirrorMode);

        // Token: 0x060000FD RID: 253
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetMirrorMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetMirrorMode(IntPtr hCamera, byte byteMirrorMode);

        // Token: 0x060000FE RID: 254
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetMirrorMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetMirrorMode(IntPtr hCamera, out byte pbyteMirrorMode);

        // Token: 0x060000FF RID: 255
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetRotationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetRotationMode(IntPtr hCamera, byte byteRotationMode);

        // Token: 0x06000100 RID: 256
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetRotationMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetRotationMode(IntPtr hCamera, out byte pbyteRotationMode);

        // Token: 0x06000101 RID: 257
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveAVIA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveAVIA(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

        // Token: 0x06000102 RID: 258
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveAVIW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveAVIW(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

        // Token: 0x06000103 RID: 259
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveAVI", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveAVI(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

        // Token: 0x06000104 RID: 260
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAVIStatus")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAVIStatus(IntPtr hCamera, byte byteAVIStatus);

        // Token: 0x06000105 RID: 261
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAVIStatus")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAVIStatus(IntPtr hCamera, out byte pbyteAVIStatus, out uint pdwTotalFrameCounts, out uint pdwCurrentFrameCounts);

        // Token: 0x06000106 RID: 262
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetAVIQuality")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetAVIQuality(IntPtr hCamera, uint dwQuality);

        // Token: 0x06000107 RID: 263
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetAVIQuality")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetAVIQuality(IntPtr hCamera, out uint pdwQuality);

        // Token: 0x06000108 RID: 264
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnableClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnableClock(IntPtr hCamera, out uint pdwEnableClockMode, out uint pdwStandardClock, out uint pdwMinimumClock, out uint pdwMaximumClock);

        // Token: 0x06000109 RID: 265
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetClock(IntPtr hCamera, uint dwClockMode, uint dwClock);

        // Token: 0x0600010A RID: 266
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetClock(IntPtr hCamera, out uint pdwClockMode, out uint pdwClock);

        // Token: 0x0600010B RID: 267
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetFrameClock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetFrameClock(IntPtr hCamera, out ushort pwCurrentLinePerFrame, out ushort pwCurrentClockPerLine);

        // Token: 0x0600010C RID: 268
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetOutputFPS")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetOutputFPS(IntPtr hCamera, out float pfFPS);

        // Token: 0x0600010D RID: 269
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetVBlankForFPS")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetVBlankForFPS(IntPtr hCamera, uint dwVLines);

        // Token: 0x0600010E RID: 270
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetVBlankForFPS")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetVBlankForFPS(IntPtr hCamera, out uint pdwVLines);

        // Token: 0x0600010F RID: 271
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetEnableDefectPixelCorrectionCount")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetEnableDefectPixelCorrectionCount(IntPtr hCamera, out ushort pwCount);

        // Token: 0x06000110 RID: 272
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDefectPixelCorrectionMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDefectPixelCorrectionMode(IntPtr hCamera, out ushort pwMode);

        // Token: 0x06000111 RID: 273
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetDefectPixelCorrectionMode")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDefectPixelCorrectionMode(IntPtr hCamera, ushort wMode);

        // Token: 0x06000112 RID: 274
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDefectPixelCorrectionPosition")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDefectPixelCorrectionPosition(IntPtr hCamera, ushort wIndex, out uint pdwX, out uint pdwY);

        // Token: 0x06000113 RID: 275
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetDefectPixelCorrectionPosition")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDefectPixelCorrectionPosition(IntPtr hCamera, ushort wIndex, uint dwX, uint dwY);

        // Token: 0x06000114 RID: 276
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_DetectDefectPixel")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool DetectDefectPixel(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteRaw, ushort wThreshold);

        // Token: 0x06000115 RID: 277
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_AddPreviewBitmapCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AddPreviewBitmapCallback(IntPtr hCamera, StCam.fStCamPreviewBitmapCallbackFunc pPreviewBitmapCallbackFunc, IntPtr pContext, out uint pdwPreviewBitmapCallbackNo);

        // Token: 0x06000116 RID: 278
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemovePreviewBitmapCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemovePreviewBitmapCallback(IntPtr hCamera, uint dwPreviewBitmapCallbackNo);

        // Token: 0x06000117 RID: 279
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemoveAllPreviewBitmapCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemoveAllPreviewBitmapCallback(IntPtr hCamera);

        // Token: 0x06000118 RID: 280
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewBitmapCallbackCount")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewBitmapCallbackCount(IntPtr hCamera, out uint pdwListCount);

        // Token: 0x06000119 RID: 281
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewBitmapCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewBitmapCallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamPreviewBitmapCallbackFunc ppPreviewBitmapCallbackFunc, out uint pdwCallbackFunctionNo);

        // Token: 0x0600011A RID: 282
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_AddPreviewGDICallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AddPreviewGDICallback(IntPtr hCamera, StCam.fStCamPreviewGDICallbackFunc pPreviewGDICallbackFunc, IntPtr pContext, out uint pdwPreviewGDICallbackNo);

        // Token: 0x0600011B RID: 283
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemovePreviewGDICallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemovePreviewGDICallback(IntPtr hCamera, uint dwPreviewGDICallbackNo);

        // Token: 0x0600011C RID: 284
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemoveAllPreviewGDICallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemoveAllPreviewGDICallback(IntPtr hCamera);

        // Token: 0x0600011D RID: 285
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewGDICallbackCount")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewGDICallbackCount(IntPtr hCamera, out uint pdwListCount);

        // Token: 0x0600011E RID: 286
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetPreviewGDICallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetPreviewGDICallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamPreviewGDICallbackFunc ppPreviewGDICallbackFunc, out uint pdwCallbackFunctionNo);

        // Token: 0x0600011F RID: 287
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_AddRawCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AddRawCallback(IntPtr hCamera, StCam.fStCamRawCallbackFunc pRawCallbackFunc, IntPtr pContext, out uint pdwRawCallbackNo);

        // Token: 0x06000120 RID: 288
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemoveRawCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemoveRawCallback(IntPtr hCamera, uint dwRawCallbackNo);

        // Token: 0x06000121 RID: 289
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_RemoveAllRawCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool RemoveAllRawCallback(IntPtr hCamera);

        // Token: 0x06000122 RID: 290
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetRawCallbackCount")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetRawCallbackCount(IntPtr hCamera, out uint pdwListCount);

        // Token: 0x06000123 RID: 291
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetRawCallback")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetRawCallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamRawCallbackFunc ppRawCallbackFunc, out uint pdwCallbackFunctionNo);

        // Token: 0x06000124 RID: 292
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveSettingFileA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveSettingFileA(IntPtr hCamera, string pszFileName);

        // Token: 0x06000125 RID: 293
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveSettingFileW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveSettingFileW(IntPtr hCamera, string pszFileName);

        // Token: 0x06000126 RID: 294
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SaveSettingFile", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SaveSettingFile(IntPtr hCamera, string pszFileName);

        // Token: 0x06000127 RID: 295
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_LoadSettingFileA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool LoadSettingFileA(IntPtr hCamera, string pszFileName);

        // Token: 0x06000128 RID: 296
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_LoadSettingFileW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool LoadSettingFileW(IntPtr hCamera, string pszFileName);

        // Token: 0x06000129 RID: 297
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_LoadSettingFile", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool LoadSettingFile(IntPtr hCamera, string pszFileName);

        // Token: 0x0600012A RID: 298
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ResetSetting")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ResetSetting(IntPtr hCamera);

        // Token: 0x0600012B RID: 299
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_CameraSetting")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CameraSetting(IntPtr hCamera, ushort wMode);

        // Token: 0x0600012C RID: 300
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ReadUserMemory")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReadUserMemory(IntPtr hCamera, byte[] pbyteBuffer, ushort wOffset, ushort wLength);

        // Token: 0x0600012D RID: 301
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_WriteUserMemory")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool WriteUserMemory(IntPtr hCamera, byte[] pbyteBuffer, ushort wOffset, ushort wLength);

        // Token: 0x0600012E RID: 302
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ReadCameraUserIDA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReadCameraUserIDA(IntPtr hCamera, out uint pdwCameraID, out string pszBuffer, uint dwBufferSize);

        // Token: 0x0600012F RID: 303
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ReadCameraUserIDW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReadCameraUserIDW(IntPtr hCamera, out uint pdwCameraID, out string pszBuffer, uint dwBufferSize);

        // Token: 0x06000130 RID: 304
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ReadCameraUserID", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ReadCameraUserID(IntPtr hCamera, out uint pdwCameraID, out string pszBuffer, uint dwBufferSize);

        // Token: 0x06000131 RID: 305
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_WriteCameraUserIDA", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool WriteCameraUserIDA(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

        // Token: 0x06000132 RID: 306
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_WriteCameraUserIDW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool WriteCameraUserIDW(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

        // Token: 0x06000133 RID: 307
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_WriteCameraUserID", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool WriteCameraUserID(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

        // Token: 0x06000134 RID: 308
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetCameraVersion")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetCameraVersion(IntPtr hCamera, out ushort pwUSBVendorID, out ushort pwUSBProductID, out ushort pwFPGAVersion, out ushort pwFirmVersion);

        // Token: 0x06000135 RID: 309
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDriverVersion")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDriverVersion(IntPtr hCamera, out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

        // Token: 0x06000136 RID: 310
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetUSBDllVersion")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetUSBDllVersion(out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

        // Token: 0x06000137 RID: 311
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetCAMDllVersion")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetCAMDllVersion(out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

        // Token: 0x06000138 RID: 312
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetUSBFunctionAddress")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetUSBFunctionAddress(IntPtr hCamera, out byte pbyteUSBFunctionAddress);

        // Token: 0x06000139 RID: 313
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ConvertBitmapBGR24ToRGB24")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ConvertBitmapBGR24ToRGB24(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteBitmap);

        // Token: 0x0600013A RID: 314
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_ConvertRawToBGR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool ConvertRawToBGR(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteSrcRaw, IntPtr pbyteDstBGR, byte byteColorInterpolationMethod, uint dwPreviewPixelFormat);

        // Token: 0x0600013B RID: 315
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetControlArea")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetControlArea(IntPtr hCamera, ushort[] pwSepalateX, ushort[] pwSepalateY);

        // Token: 0x0600013C RID: 316
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetControlArea")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetControlArea(IntPtr hCamera, ushort[] pwSepalateX, ushort[] pwSepalateY);

        // Token: 0x0600013D RID: 317
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_GetDigitalClamp")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetDigitalClamp(IntPtr hCamera, out ushort pwValue);

        // Token: 0x0600013E RID: 318
        [LibraryImport("StCamD.dll", EntryPoint = "StCam_SetDigitalClamp")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool SetDigitalClamp(IntPtr hCamera, ushort wValue);

        // Token: 0x0400002F RID: 47
        public const uint ERRST_NOT_FOUND_CAMERA = 3758096385U;

        // Token: 0x04000030 RID: 48
        public const uint ERRST_ALL_CAMARA_OPENED = 3758096386U;

        // Token: 0x04000031 RID: 49
        public const uint ERRST_INVALID_CAMERA_HANDLE = 3758096387U;

        // Token: 0x04000032 RID: 50
        public const uint ERRST_INVALID_FUNCTION_RECEIVING = 3758096388U;

        // Token: 0x04000033 RID: 51
        public const uint ERRST_USB_COMMAND_TRANSFER = 3758096389U;

        // Token: 0x04000034 RID: 52
        public const uint ERRST_WINDOW_ALREADY_EXISTS = 3758096390U;

        // Token: 0x04000035 RID: 53
        public const uint ERRST_WINDOW_DOES_NOT_EXISTS = 3758096391U;

        // Token: 0x04000036 RID: 54
        public const uint ERRST_INVALID_FUNCTION_RECORDING = 3758096392U;

        // Token: 0x04000037 RID: 55
        public const uint ERRST_AVI_STREAM = 3758096393U;

        // Token: 0x04000038 RID: 56
        public const uint ERRST_AVI_NOCOMPRESSOR = 3758096394U;

        // Token: 0x04000039 RID: 57
        public const uint ERRST_AVI_UNSUPPORTED = 3758096395U;

        // Token: 0x0400003A RID: 58
        public const uint ERRST_AVI_DISK = 3758096396U;

        // Token: 0x0400003B RID: 59
        public const uint ERRST_AVI_CANCELED = 3758096397U;

        // Token: 0x0400003C RID: 60
        public const uint ERRST_AVI_WRITE = 3758096398U;

        // Token: 0x0400003D RID: 61
        public const uint ERRST_INVALID_FILE_NAME = 3758096399U;

        // Token: 0x0400003E RID: 62
        public const uint ERRST_FILE_OPEN = 3758096400U;

        // Token: 0x0400003F RID: 63
        public const uint ERRST_FILE_WRITE = 3758096401U;

        // Token: 0x04000040 RID: 64
        public const uint ERRST_NOT_SUPPORTED_FUNCTION = 3758096417U;

        // Token: 0x04000041 RID: 65
        public const int WM_STCAM_TRANSFER_START = 45057;

        // Token: 0x04000042 RID: 66
        public const int WM_STCAM_TRANSFER_FINISH = 45058;

        // Token: 0x04000043 RID: 67
        public const int WM_STCAM_PREVIEW_WINDOW_CREATE = 45059;

        // Token: 0x04000044 RID: 68
        public const int WM_STCAM_PREVIEW_WINDOW_CLOSE = 45060;

        // Token: 0x04000045 RID: 69
        public const int WM_STCAM_PREVIEW_WINDOW_RESIZE = 45061;

        // Token: 0x04000046 RID: 70
        public const int WM_STCAM_PREVIEW_MASK_RESIZE = 45062;

        // Token: 0x04000047 RID: 71
        public const int WM_STCAM_PREVIEW_DEST_RESIZE = 45063;

        // Token: 0x04000048 RID: 72
        public const int WM_STCAM_AVI_FILE_START = 45064;

        // Token: 0x04000049 RID: 73
        public const int WM_STCAM_AVI_FILE_FINISH = 45065;

        // Token: 0x0400004A RID: 74
        public const int WM_STCAM_PREVIEW_MENU_COMMAND = 45066;

        // Token: 0x0400004B RID: 75
        public const int WM_STCAM_UPDATED_PREVIEW_IMAGE = 45067;

        // Token: 0x0400004C RID: 76
        public const ushort STCAM_COLOR_ARRAY_MONO = 1;

        // Token: 0x0400004D RID: 77
        public const ushort STCAM_COLOR_ARRAY_RGGB = 2;

        // Token: 0x0400004E RID: 78
        public const ushort STCAM_COLOR_ARRAY_GRBG = 3;

        // Token: 0x0400004F RID: 79
        public const ushort STCAM_COLOR_ARRAY_GBRG = 4;

        // Token: 0x04000050 RID: 80
        public const ushort STCAM_COLOR_ARRAY_BGGR = 5;

        // Token: 0x04000051 RID: 81
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_RAW_08 = 1U;

        // Token: 0x04000052 RID: 82
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_RAW_10 = 2U;

        // Token: 0x04000053 RID: 83
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_RAW_12 = 4U;

        // Token: 0x04000054 RID: 84
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_RAW_14 = 8U;

        // Token: 0x04000055 RID: 85
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_RAW_16 = 16U;

        // Token: 0x04000056 RID: 86
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_MONO_08 = 32U;

        // Token: 0x04000057 RID: 87
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_MONO_10 = 64U;

        // Token: 0x04000058 RID: 88
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_MONO_12 = 128U;

        // Token: 0x04000059 RID: 89
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_MONO_14 = 256U;

        // Token: 0x0400005A RID: 90
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_MONO_16 = 512U;

        // Token: 0x0400005B RID: 91
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_BGR_08 = 1024U;

        // Token: 0x0400005C RID: 92
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_BGR_10 = 2048U;

        // Token: 0x0400005D RID: 93
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_08 = 1U;

        // Token: 0x0400005E RID: 94
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_10 = 2U;

        // Token: 0x0400005F RID: 95
        public const uint STCAM_TRANSFER_BITS_PER_PIXEL_12 = 4U;

        // Token: 0x04000060 RID: 96
        public const uint STCAM_TRANSER_BITS_PER_PIXEL_08 = 1U;

        // Token: 0x04000061 RID: 97
        public const uint STCAM_TRANSER_BITS_PER_PIXEL_16 = 2U;

        // Token: 0x04000062 RID: 98
        public const uint STCAM_IMAGE_SIZE_MODE_CUSTOM = 1U;

        // Token: 0x04000063 RID: 99
        public const uint STCAM_IMAGE_SIZE_MODE_VGA = 8U;

        // Token: 0x04000064 RID: 100
        public const uint STCAM_IMAGE_SIZE_MODE_XGA = 32U;

        // Token: 0x04000065 RID: 101
        public const uint STCAM_IMAGE_SIZE_MODE_QUAD_VGA = 128U;

        // Token: 0x04000066 RID: 102
        public const uint STCAM_IMAGE_SIZE_MODE_SXGA = 256U;

        // Token: 0x04000067 RID: 103
        public const uint STCAM_IMAGE_SIZE_MODE_UXGA = 1024U;

        // Token: 0x04000068 RID: 104
        public const ushort STCAM_SCAN_MODE_NORMAL = 0;

        // Token: 0x04000069 RID: 105
        public const ushort STCAM_SCAN_MODE_PARTIAL_2 = 1;

        // Token: 0x0400006A RID: 106
        public const ushort STCAM_SCAN_MODE_PARTIAL_4 = 2;

        // Token: 0x0400006B RID: 107
        public const ushort STCAM_SCAN_MODE_PARTIAL_1 = 4;

        // Token: 0x0400006C RID: 108
        public const ushort STCAM_SCAN_MODE_VARIABLE_PARTIAL = 8;

        // Token: 0x0400006D RID: 109
        public const ushort STCAM_SCAN_MODE_BINNING = 16;

        // Token: 0x0400006E RID: 110
        public const ushort STCAM_SCAN_MODE_BINNING_PARTIAL_1 = 32;

        // Token: 0x0400006F RID: 111
        public const ushort STCAM_SCAN_MODE_BINNING_PARTIAL_2 = 64;

        // Token: 0x04000070 RID: 112
        public const ushort STCAM_SCAN_MODE_BINNING_PARTIAL_4 = 128;

        // Token: 0x04000071 RID: 113
        public const ushort STCAM_SCAN_MODE_BINNING_VARIABLE_PARTIAL = 256;

        // Token: 0x04000072 RID: 114
        public const ushort STCAM_SCAN_MODE_AOI = 32768;

        // Token: 0x04000073 RID: 115
        public const uint STCAM_PIXEL_FORMAT_08_MONO_OR_RAW = 1U;

        // Token: 0x04000074 RID: 116
        public const uint STCAM_PIXEL_FORMAT_24_BGR = 4U;

        // Token: 0x04000075 RID: 117
        public const uint STCAM_PIXEL_FORMAT_32_BGR = 8U;

        // Token: 0x04000076 RID: 118
        public const byte STCAM_COLOR_INTERPOLATION_NONE_MONO = 0;

        // Token: 0x04000077 RID: 119
        public const byte STCAM_COLOR_INTERPOLATION_NONE_COLOR = 1;

        // Token: 0x04000078 RID: 120
        public const byte STCAM_COLOR_INTERPOLATION_NEAREST_NEIGHBOR = 2;

        // Token: 0x04000079 RID: 121
        public const byte STCAM_COLOR_INTERPOLATION_BILINEAR = 3;

        // Token: 0x0400007A RID: 122
        public const byte STCAM_COLOR_INTERPOLATION_BILINEAR_FALSE_COLOR_REDUCTION = 5;

        // Token: 0x0400007B RID: 123
        public const byte STCAM_COLOR_INTERPOLATION_BICUBIC = 4;

        // Token: 0x0400007C RID: 124
        public const byte STCAM_ASPECT_MODE_FIXED = 0;

        // Token: 0x0400007D RID: 125
        public const byte STCAM_ASPECT_MODE_KEEP_ASPECT = 1;

        // Token: 0x0400007E RID: 126
        public const byte STCAM_ASPECT_MODE_ADJUST_WINDOW = 2;

        // Token: 0x0400007F RID: 127
        public const byte STCAM_ASPECT_MODE_MASK_SIZE = 3;

        // Token: 0x04000080 RID: 128
        public const byte STCAM_ASPECT_MODE_CUSTOM_OFFSET = 254;

        // Token: 0x04000081 RID: 129
        public const byte STCAM_ASPECT_MODE_CUSTOM_CENTER = 255;

        // Token: 0x04000082 RID: 130
        public const byte STCAM_ASPECT_MODE_CUSTOM = 255;

        // Token: 0x04000083 RID: 131
        public const byte STCAM_MAGNIFICATION_MODE_OFF = 0;

        // Token: 0x04000084 RID: 132
        public const byte STCAM_MAGNIFICATION_MODE_ON = 1;

        // Token: 0x04000085 RID: 133
        public const byte STCAM_ALCMODE_FIXED_SHUTTER_AGC_OFF = 0;

        // Token: 0x04000086 RID: 134
        public const byte STCAM_ALCMODE_AUTO_SHUTTER_ON_AGC_ON = 1;

        // Token: 0x04000087 RID: 135
        public const byte STCAM_ALCMODE_AUTO_SHUTTER_ON_AGC_OFF = 2;

        // Token: 0x04000088 RID: 136
        public const byte STCAM_ALCMODE_FIXED_SHUTTER_AGC_ON = 3;

        // Token: 0x04000089 RID: 137
        public const byte STCAM_ALCMODE_AUTO_SHUTTER_AGC_ONESHOT = 4;

        // Token: 0x0400008A RID: 138
        public const byte STCAM_ALCMODE_AUTO_SHUTTER_ONESHOT_AGC_OFF = 5;

        // Token: 0x0400008B RID: 139
        public const byte STCAM_ALCMODE_FIXED_SHUTTER_AGC_ONESHOT = 6;

        // Token: 0x0400008C RID: 140
        public const byte STCAM_ALCMODE_CAMERA_AE_ON = 16;

        // Token: 0x0400008D RID: 141
        public const byte STCAM_ALCMODE_CAMERA_AGC_ON = 32;

        // Token: 0x0400008E RID: 142
        public const byte STCAM_ALCMODE_CAMERA_AE_AGC_ON = 48;

        // Token: 0x0400008F RID: 143
        public const byte STCAM_ALCMODE_ALC_FIXED_AGC_OFF = 0;

        // Token: 0x04000090 RID: 144
        public const byte STCAM_ALCMODE_ALC_FULLAUTO_AGC_ON = 1;

        // Token: 0x04000091 RID: 145
        public const byte STCAM_ALCMODE_ALC_FULLAUTO_AGC_OFF = 2;

        // Token: 0x04000092 RID: 146
        public const byte STCAM_ALCMODE_ALC_FIXED_AGC_ON = 3;

        // Token: 0x04000093 RID: 147
        public const byte STCAM_ALCMODE_ALCAGC_ONESHOT = 4;

        // Token: 0x04000094 RID: 148
        public const byte STCAM_ALCMODE_ALC_ONESHOT_AGC_OFF = 5;

        // Token: 0x04000095 RID: 149
        public const byte STCAM_ALCMODE_ALC_FIXED_AGC_ONESHOT = 6;

        // Token: 0x04000096 RID: 150
        public const byte STCAM_WB_OFF = 0;

        // Token: 0x04000097 RID: 151
        public const byte STCAM_WB_MANUAL = 1;

        // Token: 0x04000098 RID: 152
        public const byte STCAM_WB_FULLAUTO = 2;

        // Token: 0x04000099 RID: 153
        public const byte STCAM_WB_ONESHOT = 3;

        // Token: 0x0400009A RID: 154
        public const byte STCAM_GAMMA_OFF = 0;

        // Token: 0x0400009B RID: 155
        public const byte STCAM_GAMMA_ON = 1;

        // Token: 0x0400009C RID: 156
        public const byte STCAM_GAMMA_REVERSE = 2;

        // Token: 0x0400009D RID: 157
        public const byte STCAM_GAMMA_TABLE = 255;

        // Token: 0x0400009E RID: 158
        public const byte STCAM_GAMMA_TARGET_Y = 0;

        // Token: 0x0400009F RID: 159
        public const byte STCAM_GAMMA_TARGET_R = 1;

        // Token: 0x040000A0 RID: 160
        public const byte STCAM_GAMMA_TARGET_GR = 2;

        // Token: 0x040000A1 RID: 161
        public const byte STCAM_GAMMA_TARGET_GB = 3;

        // Token: 0x040000A2 RID: 162
        public const byte STCAM_GAMMA_TARGET_B = 4;

        // Token: 0x040000A3 RID: 163
        public const byte STCAM_SHARPNESS_OFF = 0;

        // Token: 0x040000A4 RID: 164
        public const byte STCAM_SHARPNESS_ON = 1;

        // Token: 0x040000A5 RID: 165
        public const byte STCAM_HUE_SATURATION_OFF = 0;

        // Token: 0x040000A6 RID: 166
        public const byte STCAM_HUE_SATURATION_ON = 1;

        // Token: 0x040000A7 RID: 167
        public const byte STCAM_COLOR_MATRIX_OFF = 0;

        // Token: 0x040000A8 RID: 168
        public const byte STCAM_COLOR_MATRIX_CUSTOM = 255;

        // Token: 0x040000A9 RID: 169
        public const byte STCAM_MIRROR_OFF = 0;

        // Token: 0x040000AA RID: 170
        public const byte STCAM_MIRROR_HORIZONTAL = 1;

        // Token: 0x040000AB RID: 171
        public const byte STCAM_MIRROR_VERTICAL = 2;

        // Token: 0x040000AC RID: 172
        public const byte STCAM_MIRROR_HORIZONTAL_VERTICAL = 3;

        // Token: 0x040000AD RID: 173
        public const byte STCAM_MIRROR_HORIZONTAL_CAMERA = 16;

        // Token: 0x040000AE RID: 174
        public const byte STCAM_MIRROR_VERTICAL_CAMERA = 32;

        // Token: 0x040000AF RID: 175
        public const byte STCAM_ROTATION_OFF = 0;

        // Token: 0x040000B0 RID: 176
        public const byte STCAM_ROTATION_CLOCKWISE_90 = 1;

        // Token: 0x040000B1 RID: 177
        public const byte STCAM_ROTATION_COUNTERCLOCKWISE_90 = 2;

        // Token: 0x040000B2 RID: 178
        public const uint STCAM_AVI_COMPRESSOR_UNCOMPRESSED = 0U;

        // Token: 0x040000B3 RID: 179
        public const uint STCAM_AVI_COMPRESSOR_MJPG = 1196444237U;

        // Token: 0x040000B4 RID: 180
        public const uint STCAM_AVI_COMPRESSOR_MP42 = 842297453U;

        // Token: 0x040000B5 RID: 181
        public const uint STCAM_AVI_COMPRESSOR_MPV4 = 879194221U;

        // Token: 0x040000B6 RID: 182
        public const uint STCAM_AVI_COMPRESSOR_DIALOG_BOX = 4294967295U;

        // Token: 0x040000B7 RID: 183
        public const uint STCAM_CLOCK_MODE_NORMAL = 0U;

        // Token: 0x040000B8 RID: 184
        public const uint STCAM_CLOCK_MODE_DIV_2 = 1U;

        // Token: 0x040000B9 RID: 185
        public const uint STCAM_CLOCK_MODE_DIV_4 = 2U;

        // Token: 0x040000BA RID: 186
        public const uint STCAM_CLOCK_MODE_VGA_90FPS = 256U;

        // Token: 0x040000BB RID: 187
        public const uint STCAM_CLOCK_MODE_CUSTOM = 2147483648U;

        // Token: 0x040000BC RID: 188
        public const ushort STCAM_USBPID_STC_C33USB = 773;

        // Token: 0x040000BD RID: 189
        public const ushort STCAM_USBPID_STC_B33USB = 1797;

        // Token: 0x040000BE RID: 190
        public const ushort STCAM_USBPID_STC_B83USB = 2053;

        // Token: 0x040000BF RID: 191
        public const ushort STCAM_USBPID_STC_C83USB = 1541;

        // Token: 0x040000C0 RID: 192
        public const ushort STCAM_USBPID_STC_TB33USB = 2310;

        // Token: 0x040000C1 RID: 193
        public const ushort STCAM_USBPID_STC_TC33USB = 4102;

        // Token: 0x040000C2 RID: 194
        public const ushort STCAM_USBPID_STC_TB83USB = 4358;

        // Token: 0x040000C3 RID: 195
        public const ushort STCAM_USBPID_STC_TC83USB = 4614;

        // Token: 0x040000C4 RID: 196
        public const ushort STCAM_USBPID_STC_TB133USB = 265;

        // Token: 0x040000C5 RID: 197
        public const ushort STCAM_USBPID_STC_TC133USB = 521;

        // Token: 0x040000C6 RID: 198
        public const ushort STCAM_USBPID_STC_TB152USB = 4870;

        // Token: 0x040000C7 RID: 199
        public const ushort STCAM_USBPID_STC_TC152USB = 5126;

        // Token: 0x040000C8 RID: 200
        public const ushort STCAM_USBPID_STC_TB202USB = 5382;

        // Token: 0x040000C9 RID: 201
        public const ushort STCAM_USBPID_STC_TC202USB = 5638;

        // Token: 0x040000CA RID: 202
        public const ushort STCAM_USBPID_STC_MB33USB = 272;

        // Token: 0x040000CB RID: 203
        public const ushort STCAM_USBPID_STC_MC33USB = 528;

        // Token: 0x040000CC RID: 204
        public const ushort STCAM_USBPID_STC_MB83USB = 784;

        // Token: 0x040000CD RID: 205
        public const ushort STCAM_USBPID_STC_MC83USB = 1040;

        // Token: 0x040000CE RID: 206
        public const ushort STCAM_USBPID_STC_MB133USB = 1296;

        // Token: 0x040000CF RID: 207
        public const ushort STCAM_USBPID_STC_MC133USB = 1552;

        // Token: 0x040000D0 RID: 208
        public const ushort STCAM_USBPID_STC_MB152USB = 1808;

        // Token: 0x040000D1 RID: 209
        public const ushort STCAM_USBPID_STC_MC152USB = 2064;

        // Token: 0x040000D2 RID: 210
        public const ushort STCAM_USBPID_STC_MB202USB = 2320;

        // Token: 0x040000D3 RID: 211
        public const ushort STCAM_USBPID_STC_MC202USB = 4112;

        // Token: 0x040000D4 RID: 212
        public const ushort STCAM_USBPID_STC_MBA5MUSB3 = 273;

        // Token: 0x040000D5 RID: 213
        public const ushort STCAM_USBPID_STC_MCA5MUSB3 = 529;

        // Token: 0x040000D6 RID: 214
        public const byte STCAM_AVI_STATUS_STOP = 0;

        // Token: 0x040000D7 RID: 215
        public const byte STCAM_AVI_STATUS_START = 1;

        // Token: 0x040000D8 RID: 216
        public const byte STCAM_AVI_STATUS_PAUSE = 2;

        // Token: 0x040000D9 RID: 217
        public const byte STCAM_DISPLAY_MODE_GDI = 0;

        // Token: 0x040000DA RID: 218
        public const byte STCAM_DISPLAY_MODE_GDI_HALFTONE = 8;

        // Token: 0x040000DB RID: 219
        public const byte STCAM_DISPLAY_MODE_DD_OFFSCREEN = 1;

        // Token: 0x040000DC RID: 220
        public const byte STCAM_DISPLAY_MODE_DD_OVERLAY = 2;

        // Token: 0x040000DD RID: 221
        public const byte STCAM_DISPLAY_MODE_DD_OFFSCREEN_HQ = 3;

        // Token: 0x040000DE RID: 222
        public const byte STCAM_DISPLAY_MODE_DD_OVERLAY_HQ = 4;

        // Token: 0x040000DF RID: 223
        public const byte STCAM_DISPLAY_MODE_DIRECTX = 5;

        // Token: 0x040000E0 RID: 224
        public const byte STCAM_DISPLAY_MODE_DIRECTX_VSYNC_ON = 6;

        // Token: 0x040000E1 RID: 225
        public const byte STCAM_DISPLAY_MODE_DIRECTX_VSYNC_ON2 = 7;

        // Token: 0x040000E2 RID: 226
        public const byte STCAM_DISPLAY_MODE_DIRECTX_FPU = 9;

        // Token: 0x040000E3 RID: 227
        public const byte STCAM_DISPLAY_MODE_DIRECTX_VSYNC_ON_FPU = 10;

        // Token: 0x040000E4 RID: 228
        public const byte STCAM_DISPLAY_MODE_DIRECTX_VSYNC_ON2_FPU = 11;

        // Token: 0x040000E5 RID: 229
        public const uint STCAM_TRUE = 4294967295U;

        // Token: 0x040000E6 RID: 230
        public const uint STCAM_FALSE = 0U;

        // Token: 0x040000E7 RID: 231
        public const ushort STCAM_CAMERA_SETTING_INITIALIZE = 32768;

        // Token: 0x040000E8 RID: 232
        public const ushort STCAM_CAMERA_SETTING_WRITE = 8192;

        // Token: 0x040000E9 RID: 233
        public const ushort STCAM_CAMERA_SETTING_READ = 4096;

        // Token: 0x040000EA RID: 234
        public const ushort STCAM_CAMERA_SETTING_STANDARD = 2048;

        // Token: 0x040000EB RID: 235
        public const ushort STCAM_CAMERA_SETTING_DEFECT_PIXEL_POSITION = 1024;

        // Token: 0x040000EC RID: 236
        public const ushort STCAM_DEFECT_PIXEL_CORRECTION_OFF = 0;

        // Token: 0x040000ED RID: 237
        public const ushort STCAM_DEFECT_PIXEL_CORRECTION_ON = 1;

        // Token: 0x040000EE RID: 238
        public const ushort STCAM_BINNING_SUM_MODE_OFF = 0;

        // Token: 0x040000EF RID: 239
        public const ushort STCAM_BINNING_SUM_MODE_H = 1;

        // Token: 0x040000F0 RID: 240
        public const uint WS_OVERLAPPED = 0U;

        // Token: 0x040000F1 RID: 241
        public const uint WS_POPUP = 2147483648U;

        // Token: 0x040000F2 RID: 242
        public const uint WS_CHILD = 1073741824U;

        // Token: 0x040000F3 RID: 243
        public const uint WS_MINIMIZE = 536870912U;

        // Token: 0x040000F4 RID: 244
        public const uint WS_VISIBLE = 268435456U;

        // Token: 0x040000F5 RID: 245
        public const uint WS_DISABLED = 134217728U;

        // Token: 0x040000F6 RID: 246
        public const uint WS_CLIPSIBLINGS = 67108864U;

        // Token: 0x040000F7 RID: 247
        public const uint WS_CLIPCHILDREN = 33554432U;

        // Token: 0x040000F8 RID: 248
        public const uint WS_MAXIMIZE = 16777216U;

        // Token: 0x040000F9 RID: 249
        public const uint WS_CAPTION = 12582912U;

        // Token: 0x040000FA RID: 250
        public const uint WS_BORDER = 8388608U;

        // Token: 0x040000FB RID: 251
        public const uint WS_DLGFRAME = 4194304U;

        // Token: 0x040000FC RID: 252
        public const uint WS_VSCROLL = 2097152U;

        // Token: 0x040000FD RID: 253
        public const uint WS_HSCROLL = 1048576U;

        // Token: 0x040000FE RID: 254
        public const uint WS_SYSMENU = 524288U;

        // Token: 0x040000FF RID: 255
        public const uint WS_THICKFRAME = 262144U;

        // Token: 0x04000100 RID: 256
        public const uint WS_GROUP = 131072U;

        // Token: 0x04000101 RID: 257
        public const uint WS_TABSTOP = 65536U;

        // Token: 0x04000102 RID: 258
        public const uint WS_MINIMIZEBOX = 131072U;

        // Token: 0x04000103 RID: 259
        public const uint WS_MAXIMIZEBOX = 65536U;

        // Token: 0x04000104 RID: 260
        public const uint WS_OVERLAPPEDWINDOW = 13565952U;

        // Token: 0x04000105 RID: 261
        public const uint WS_POPUPWINDOW = 2156396544U;

        // Token: 0x04000106 RID: 262
        public const uint WS_TILED = 0U;

        // Token: 0x04000107 RID: 263
        public const uint WS_ICONIC = 131072U;

        // Token: 0x04000108 RID: 264
        public const uint WS_SIZEBOX = 262144U;

        // Token: 0x04000109 RID: 265
        public const uint WS_TILEDWINDOW = 13565952U;

        // Token: 0x0400010A RID: 266
        public const uint WS_CHILDWINDOW = 1073741824U;

        // Token: 0x02000011 RID: 17
        // (Invoke) Token: 0x06000140 RID: 320
        public delegate void fStCamPreviewBitmapCallbackFunc(IntPtr pbyteBitmap, uint dwBufferSize, uint dwWidth, uint dwHeight, uint dwFrameNo, uint dwPreviewPixelFormat, IntPtr lpContext, IntPtr lpReserved);

        // Token: 0x02000012 RID: 18
        // (Invoke) Token: 0x06000144 RID: 324
        public delegate void fStCamRawCallbackFunc(IntPtr pbyteBuffer, uint dwBufferSize, uint dwWidth, uint dwHeight, uint dwFrameNo, ushort wColorArray, uint dwTransferBitsPerPixel, IntPtr lpContext, IntPtr lpReserved);

        // Token: 0x02000013 RID: 19
        // (Invoke) Token: 0x06000148 RID: 328
        public delegate void fStCamPreviewGDICallbackFunc(IntPtr hDC, uint dwWidth, uint dwHeight, uint dwFrameNo, IntPtr lpContext, IntPtr lpReserved);
    }
}
