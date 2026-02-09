using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SensorTechnology
{
	// Token: 0x02000010 RID: 16
	public class StCam
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00003F4D File Offset: 0x0000214D
		private StCam()
		{
		}

		// Token: 0x06000086 RID: 134
		[DllImport("StCamD.dll", EntryPoint = "StCam_Open")]
		public static extern IntPtr Open(uint dwInstance);

		// Token: 0x06000087 RID: 135
		[DllImport("StCamD.dll", EntryPoint = "StCam_Close")]
		public static extern void Close(IntPtr hCamera);

		// Token: 0x06000088 RID: 136
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetLastError")]
		public static extern uint GetLastError(IntPtr hCamera);

		// Token: 0x06000089 RID: 137
		[DllImport("StCamD.dll", EntryPoint = "StCam_CameraCount")]
		public static extern uint CameraCount();

		// Token: 0x0600008A RID: 138
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetReceiveMsgWindow")]
		public static extern bool SetReceiveMsgWindow(IntPtr hCamera, IntPtr hWnd);

		// Token: 0x0600008B RID: 139
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetUSBSpeed")]
		public static extern bool GetUSBSpeed(IntPtr hCamera, out byte pbyteUSBSpeed);

		// Token: 0x0600008C RID: 140
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetUSBMaxSpeed")]
		public static extern bool GetUSBMaxSpeed(IntPtr hCamera, out byte pbyteUSBSpeed);

		// Token: 0x0600008D RID: 141
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetColorArray")]
		public static extern bool GetColorArray(IntPtr hCamera, out ushort pwColorArray);

		// Token: 0x0600008E RID: 142
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnableTransferBitsPerPixel")]
		public static extern bool GetEnableTransferBitsPerPixel(IntPtr hCamera, out uint pdwEnableTransferBitsPerPixel);

		// Token: 0x0600008F RID: 143
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetTransferBitsPerPixel")]
		public static extern bool SetTransferBitsPerPixel(IntPtr hCamera, uint dwTransferBitsPerPixel);

		// Token: 0x06000090 RID: 144
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetTransferBitsPerPixel")]
		public static extern bool GetTransferBitsPerPixel(IntPtr hCamera, out uint pdwTransferBitsPerPixel);

		// Token: 0x06000091 RID: 145
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMaximumImageSize")]
		public static extern bool GetMaximumImageSize(IntPtr hCamera, out uint pdwMaxWidth, out uint pdwMaxHeight);

		// Token: 0x06000092 RID: 146
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnableImageSize")]
		public static extern bool GetEnableImageSize(IntPtr hCamera, out uint pdwReserved, out ushort pwEnableScanMode);

		// Token: 0x06000093 RID: 147
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetImageSize")]
		public static extern bool SetImageSize(IntPtr hCamera, uint dwReserved, ushort wScanMode, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

		// Token: 0x06000094 RID: 148
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetImageSize")]
		public static extern bool GetImageSize(IntPtr hCamera, out uint pdwReserved, out ushort pwScanMode, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

		// Token: 0x06000095 RID: 149
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetSkippingAndBinning")]
		public static extern bool GetSkippingAndBinning(IntPtr hCamera, out byte pbyteHSkipping, out byte pbyteVSkipping, out byte pbyteHBinning, out byte pbyteVBinning);

		// Token: 0x06000096 RID: 150
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetSkippingAndBinning")]
		public static extern bool SetSkippingAndBinning(IntPtr hCamera, byte byteHSkipping, byte byteVSkipping, byte byteHBinning, byte byteVBinning);

		// Token: 0x06000097 RID: 151
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetBinningSumMode")]
		public static extern bool GetBinningSumMode(IntPtr hCamera, out ushort pwMode);

		// Token: 0x06000098 RID: 152
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetBinningSumMode")]
		public static extern bool SetBinningSumMode(IntPtr hCamera, ushort wMode);

		// Token: 0x06000099 RID: 153
		[DllImport("StCamD.dll", EntryPoint = "StCam_StartTransfer")]
		public static extern bool StartTransfer(IntPtr hCamera);

		// Token: 0x0600009A RID: 154
		[DllImport("StCamD.dll", EntryPoint = "StCam_StopTransfer")]
		public static extern bool StopTransfer(IntPtr hCamera);

		// Token: 0x0600009B RID: 155
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetPreviewPixelFormat")]
		public static extern bool SetPreviewPixelFormat(IntPtr hCamera, uint dwPreviewPixelFormat);

		// Token: 0x0600009C RID: 156
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewPixelFormat")]
		public static extern bool GetPreviewPixelFormat(IntPtr hCamera, out uint pdwPreviewPixelFormat);

		// Token: 0x0600009D RID: 157
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnablePreviewPixelFormat")]
		public static extern bool GetEnablePreviewPixelFormat(IntPtr hCamera, out uint pdwEnablePreviewPixelFormat);

		// Token: 0x0600009E RID: 158
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetColorInterpolationMethod")]
		public static extern bool SetColorInterpolationMethod(IntPtr hCamera, byte byteColorInterpolationMethod);

		// Token: 0x0600009F RID: 159
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetColorInterpolationMethod")]
		public static extern bool GetColorInterpolationMethod(IntPtr hCamera, out byte pbyteColorInterpolationMethod);

		// Token: 0x060000A0 RID: 160
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_CreatePreviewWindowA")]
		public static extern bool CreatePreviewWindowA(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, bool bCloseEnable);

		// Token: 0x060000A1 RID: 161
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_CreatePreviewWindowW")]
		public static extern bool CreatePreviewWindowW(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, bool bCloseEnable);

		// Token: 0x060000A2 RID: 162
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_CreatePreviewWindow")]
		public static extern bool CreatePreviewWindow(IntPtr hCamera, string pszWindowName, uint dwStyle, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight, IntPtr hWndParent, IntPtr hMenu, bool bCloseEnable);

		// Token: 0x060000A3 RID: 163
		[DllImport("StCamD.dll", EntryPoint = "StCam_DestroyPreviewWindow")]
		public static extern bool DestroyPreviewWindow(IntPtr hCamera);

		// Token: 0x060000A4 RID: 164
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_SetPreviewWindowNameA")]
		public static extern bool SetPreviewWindowNameA(IntPtr hCamera, string pszWindowName);

		// Token: 0x060000A5 RID: 165
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_GetPreviewWindowNameA")]
		public static extern bool GetPreviewWindowNameA(IntPtr hCamera, StringBuilder pszWindowName, int lngMaxCount);

		// Token: 0x060000A6 RID: 166
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_SetPreviewWindowNameW")]
		public static extern bool SetPreviewWindowNameW(IntPtr hCamera, string pszWindowName);

		// Token: 0x060000A7 RID: 167
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_SetPreviewWindowName")]
		public static extern bool SetPreviewWindowName(IntPtr hCamera, string pszWindowName);

		// Token: 0x060000A8 RID: 168
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_GetPreviewWindowNameW")]
		public static extern bool GetPreviewWindowNameW(IntPtr hCamera, StringBuilder pszWindowName, int lngMaxCount);

		// Token: 0x060000A9 RID: 169
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_GetPreviewWindowName")]
		public static extern bool GetPreviewWindowName(IntPtr hCamera, StringBuilder pszWindowName, int lngMaxCount);

		// Token: 0x060000AA RID: 170
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetPreviewMaskSize")]
		public static extern bool SetPreviewMaskSize(IntPtr hCamera, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

		// Token: 0x060000AB RID: 171
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewMaskSize")]
		public static extern bool GetPreviewMaskSize(IntPtr hCamera, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

		// Token: 0x060000AC RID: 172
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowSize")]
		public static extern bool SetPreviewWindowSize(IntPtr hCamera, int lngPositionX, int lngPositionY, uint dwWidth, uint dwHeight);

		// Token: 0x060000AD RID: 173
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowSize")]
		public static extern bool GetPreviewWindowSize(IntPtr hCamera, out int plngPositionX, out int plngPositionY, out uint pdwWidth, out uint pdwHeight);

		// Token: 0x060000AE RID: 174
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetPreviewWindowStyle")]
		public static extern bool SetPreviewWindowStyle(IntPtr hCamera, uint dwStyle);

		// Token: 0x060000AF RID: 175
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWindowStyle")]
		public static extern bool GetPreviewWindowStyle(IntPtr hCamera, out uint pdwStyle);

		// Token: 0x060000B0 RID: 176
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAspectMode")]
		public static extern bool SetAspectMode(IntPtr hCamera, byte byteAspectMode);

		// Token: 0x060000B1 RID: 177
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAspectMode")]
		public static extern bool GetAspectMode(IntPtr hCamera, out byte pbyteAspectMode);

		// Token: 0x060000B2 RID: 178
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetPreviewDestSize")]
		public static extern bool SetPreviewDestSize(IntPtr hCamera, uint dwOffsetX, uint dwOffsetY, uint dwWidth, uint dwHeight);

		// Token: 0x060000B3 RID: 179
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewDestSize")]
		public static extern bool GetPreviewDestSize(IntPtr hCamera, out uint pdwOffsetX, out uint pdwOffsetY, out uint pdwWidth, out uint pdwHeight);

		// Token: 0x060000B4 RID: 180
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetMagnificationMode")]
		public static extern bool SetMagnificationMode(IntPtr hCamera, byte byteMagnificationMode);

		// Token: 0x060000B5 RID: 181
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMagnificationMode")]
		public static extern bool GetMagnificationMode(IntPtr hCamera, out byte pbyteMagnificationMode);

		// Token: 0x060000B6 RID: 182
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDC")]
		public static extern bool GetDC(IntPtr hCamera, out IntPtr phDC);

		// Token: 0x060000B7 RID: 183
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDCWithReset")]
		public static extern bool GetDCWithReset(IntPtr hCamera, out IntPtr phDC);

		// Token: 0x060000B8 RID: 184
		[DllImport("StCamD.dll", EntryPoint = "StCam_ReleaseDC")]
		public static extern bool ReleaseDC(IntPtr hCamera, IntPtr hDC);

		// Token: 0x060000B9 RID: 185
		[DllImport("StCamD.dll", EntryPoint = "StCam_ResetOverlay")]
		public static extern bool ResetOverlay(IntPtr hCamera);

		// Token: 0x060000BA RID: 186
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetColorKey")]
		public static extern bool SetColorKey(IntPtr hCamera, uint dwColorKey);

		// Token: 0x060000BB RID: 187
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetColorKey")]
		public static extern bool GetColorKey(IntPtr hCamera, out uint pdwColorKey);

		// Token: 0x060000BC RID: 188
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetDisplayMode")]
		public static extern bool SetDisplayMode(IntPtr hCamera, byte byteDisplayMode);

		// Token: 0x060000BD RID: 189
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDisplayMode")]
		public static extern bool GetDisplayMode(IntPtr hCamera, out byte pbyteDisplayMode);

		// Token: 0x060000BE RID: 190
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAlphaChannel")]
		public static extern bool GetAlphaChannel(IntPtr hCamera, out byte pbyteAlphaChannel);

		// Token: 0x060000BF RID: 191
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAlphaChannel")]
		public static extern bool SetAlphaChannel(IntPtr hCamera, byte byteAlphaChannel);

		// Token: 0x060000C0 RID: 192
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewImagePixelValue")]
		public static extern bool GetPreviewImagePixelValue(IntPtr hCamera, uint nX, uint nY, out uint pdwColor);

		// Token: 0x060000C1 RID: 193
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewWnd")]
		public static extern bool GetPreviewWnd(IntPtr hCamera, out IntPtr hWnd);

		// Token: 0x060000C2 RID: 194
		[DllImport("StCamD.dll", EntryPoint = "StCam_TakeRawSnapShot")]
		public static extern bool TakeRawSnapShot(IntPtr hCamera, IntPtr pbyteBuffer, uint dwBufferSize, out uint pdwNumberOfByteTrans, uint[] pdwFrameNo, uint dwMilliseconds);

		// Token: 0x060000C3 RID: 195
		[DllImport("StCamD.dll", EntryPoint = "StCam_TakePreviewSnapShot")]
		public static extern bool TakePreviewSnapShot(IntPtr hCamera, IntPtr pbyteBuffer, uint dwBufferSize, out uint pdwNumberOfByteTrans, uint[] pdwFrameNo, uint dwMilliseconds);

		// Token: 0x060000C4 RID: 196
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_SaveImageA")]
		public static extern bool SaveImageA(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

		// Token: 0x060000C5 RID: 197
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_SaveImageW")]
		public static extern bool SaveImageW(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

		// Token: 0x060000C6 RID: 198
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_SaveImage")]
		public static extern bool SaveImage(IntPtr hCamera, uint dwWidth, uint dwHeight, uint dwPreviewPixelFormat, IntPtr pbyteData, string pszFileName, uint dwParam);

		// Token: 0x060000C7 RID: 199
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetALCMode")]
		public static extern bool GetALCMode(IntPtr hCamera, out byte pbyteAlcMode);

		// Token: 0x060000C8 RID: 200
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetALCMode")]
		public static extern bool SetALCMode(IntPtr hCamera, byte byteAlcMode);

		// Token: 0x060000C9 RID: 201
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetExposureClock")]
		public static extern bool GetExposureClock(IntPtr hCamera, out uint pdwExposureClock);

		// Token: 0x060000CA RID: 202
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetExposureClock")]
		public static extern bool SetExposureClock(IntPtr hCamera, uint dwExposureClock);

		// Token: 0x060000CB RID: 203
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMaxShortExposureClock")]
		public static extern bool GetMaxShortExposureClock(IntPtr hCamera, out uint pdwMaximumExpClock);

		// Token: 0x060000CC RID: 204
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMaxLongExposureClock")]
		public static extern bool GetMaxLongExposureClock(IntPtr hCamera, out uint pdwMaximumExpClock);

		// Token: 0x060000CD RID: 205
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetExposureTimeFromClock")]
		public static extern bool GetExposureTimeFromClock(IntPtr hCamera, uint dwExposureClock, out float pfExpTime);

		// Token: 0x060000CE RID: 206
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetExposureClockFromTime")]
		public static extern bool GetExposureClockFromTime(IntPtr hCamera, float fExpTime, out uint pdwExposureClock);

		// Token: 0x060000CF RID: 207
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetGain")]
		public static extern bool GetGain(IntPtr hCamera, out ushort pwGain);

		// Token: 0x060000D0 RID: 208
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetGain")]
		public static extern bool SetGain(IntPtr hCamera, ushort wGain);

		// Token: 0x060000D1 RID: 209
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMaxGain")]
		public static extern bool GetMaxGain(IntPtr hCamera, out ushort pwMaxGain);

		// Token: 0x060000D2 RID: 210
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetGainDBFromSettingValue")]
		public static extern bool GetGainDBFromSettingValue(IntPtr hCamera, ushort wGain, out float pfGainDB);

		// Token: 0x060000D3 RID: 211
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetTargetBrightness")]
		public static extern bool GetTargetBrightness(IntPtr hCamera, out byte pbyteTargetBrightness, out byte pbyteTolerance, out byte pbyteThreshold);

		// Token: 0x060000D4 RID: 212
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetTargetBrightness")]
		public static extern bool SetTargetBrightness(IntPtr hCamera, byte byteTargetBrightness, byte byteTolerance, byte byteThreshold);

		// Token: 0x060000D5 RID: 213
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetALCWeight")]
		public static extern bool GetALCWeight(IntPtr hCamera, byte[] pbyteALCWeight);

		// Token: 0x060000D6 RID: 214
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetALCWeight")]
		public static extern bool SetALCWeight(IntPtr hCamera, byte[] pbyteALCWeight);

		// Token: 0x060000D7 RID: 215
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAEMinExposureClock")]
		public static extern bool SetAEMinExposureClock(IntPtr hCamera, uint dwMinExposureClock);

		// Token: 0x060000D8 RID: 216
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAEMinExposureClock")]
		public static extern bool GetAEMinExposureClock(IntPtr hCamera, out uint pdwMinExposureClock);

		// Token: 0x060000D9 RID: 217
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAEMaxExposureClock")]
		public static extern bool GetAEMaxExposureClock(IntPtr hCamera, out uint pdwMaxExposureClock);

		// Token: 0x060000DA RID: 218
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAEMaxExposureClock")]
		public static extern bool SetAEMaxExposureClock(IntPtr hCamera, uint dwMaxExposureClock);

		// Token: 0x060000DB RID: 219
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetALCControlSpeed")]
		public static extern bool SetALCControlSpeed(IntPtr hCamera, byte byteShutterCtrlSpeedLimit, byte byteGainCtrlSpeedLimit, byte byteSkipFrameCount, byte byteAverageFrameCount);

		// Token: 0x060000DC RID: 220
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetALCControlSpeed")]
		public static extern bool GetALCControlSpeed(IntPtr hCamera, out byte pbyteShutterCtrlSpeedLimit, out byte pbyteGainCtrlSpeedLimit, out byte pbyteSkipFrameCount, out byte pbyteAverageFrameCount);

		// Token: 0x060000DD RID: 221
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetGainControlRange")]
		public static extern bool GetGainControlRange(IntPtr hCamera, out ushort pwMinGain, out ushort pwMaxGain);

		// Token: 0x060000DE RID: 222
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetGainControlRange")]
		public static extern bool SetGainControlRange(IntPtr hCamera, ushort wMinGain, ushort wMaxGain);

		// Token: 0x060000DF RID: 223
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDigitalGain")]
		public static extern bool GetDigitalGain(IntPtr hCamera, out ushort pwDigitalGain);

		// Token: 0x060000E0 RID: 224
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetDigitalGain")]
		public static extern bool SetDigitalGain(IntPtr hCamera, ushort wDigitalGain);

		// Token: 0x060000E1 RID: 225
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetShutterSpeed")]
		public static extern bool GetShutterSpeed(IntPtr hCamera, out ushort pwShutterLine, out ushort pwShutterClock);

		// Token: 0x060000E2 RID: 226
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetShutterSpeed")]
		public static extern bool SetShutterSpeed(IntPtr hCamera, ushort wShutterLine, ushort wShutterClock);

		// Token: 0x060000E3 RID: 227
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetShutterControlRange")]
		public static extern bool GetShutterControlRange(IntPtr hCamera, out ushort pwMinShutterLine, out ushort pwMinShutterClock, out ushort pwMaxShutterLine, out ushort pwMaxShutterClock);

		// Token: 0x060000E4 RID: 228
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetShutterControlRange")]
		public static extern bool SetShutterControlRange(IntPtr hCamera, ushort wMinShutterLine, ushort wMinShutterClock, ushort wMaxShutterLine, ushort wMaxShutterClock);

		// Token: 0x060000E5 RID: 229
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceMode")]
		public static extern bool SetWhiteBalanceMode(IntPtr hCamera, byte byteWBMode);

		// Token: 0x060000E6 RID: 230
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceMode")]
		public static extern bool GetWhiteBalanceMode(IntPtr hCamera, out byte pbyteWBMode);

		// Token: 0x060000E7 RID: 231
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceGain")]
		public static extern bool SetWhiteBalanceGain(IntPtr hCamera, ushort wWBGainR, ushort wWBGainGr, ushort wWBGainGb, ushort wWBGainB);

		// Token: 0x060000E8 RID: 232
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceGain")]
		public static extern bool GetWhiteBalanceGain(IntPtr hCamera, out ushort pwWBGainR, out ushort pwWBGainGr, out ushort pwWBGainGb, out ushort pwWBGainB);

		// Token: 0x060000E9 RID: 233
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceTarget")]
		public static extern bool SetWhiteBalanceTarget(IntPtr hCamera, ushort wAWBTargetR, ushort wAWBTargetB);

		// Token: 0x060000EA RID: 234
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceTarget")]
		public static extern bool GetWhiteBalanceTarget(IntPtr hCamera, out ushort pwAWBTargetR, out ushort pwAWBTargetB);

		// Token: 0x060000EB RID: 235
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetWhiteBalanceToleranceThreshold")]
		public static extern bool SetWhiteBalanceToleranceThreshold(IntPtr hCamera, ushort wAWBTolerance, ushort wAWBThreshold);

		// Token: 0x060000EC RID: 236
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetWhiteBalanceToleranceThreshold")]
		public static extern bool GetWhiteBalanceToleranceThreshold(IntPtr hCamera, out ushort pwAWBTolerance, out ushort pwAWBThreshold);

		// Token: 0x060000ED RID: 237
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAWBWeight")]
		public static extern bool SetAWBWeight(IntPtr hCamera, byte[] pbyteAWBWeight);

		// Token: 0x060000EE RID: 238
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAWBWeight")]
		public static extern bool GetAWBWeight(IntPtr hCamera, byte[] pbyteAWBWeight);

		// Token: 0x060000EF RID: 239
		[DllImport("StCamD.dll", EntryPoint = "StCam_RawWhiteBalance")]
		public static extern bool RawWhiteBalance(IntPtr hCamera, uint dwWidth, uint dwHeight, ushort wColorArray, IntPtr pbyteRaw);

		// Token: 0x060000F0 RID: 240
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetGammaMode")]
		public static extern bool SetGammaMode(IntPtr hCamera, byte byteGammaTarget, byte byteGammaMode, ushort wGamma, byte[] pbyteGammaTable);

		// Token: 0x060000F1 RID: 241
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetGammaMode")]
		public static extern bool GetGammaMode(IntPtr hCamera, byte byteGammaTarget, out byte pbyteGammaMode, out ushort pwGamma, byte[] pbyteGammaTable);

		// Token: 0x060000F2 RID: 242
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetGammaModeEx")]
		public static extern bool SetGammaModeEx(IntPtr hCamera, byte byteGammaTarget, byte byteGammaMode, ushort wGamma, short shtBrightness, byte byteContrast, byte[] pbyteGammaTable);

		// Token: 0x060000F3 RID: 243
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetGammaModeEx")]
		public static extern bool GetGammaModeEx(IntPtr hCamera, byte byteGammaTarget, out byte pbyteGammaMode, out ushort pwGamma, out short pshtBrightness, out byte pbyteContrast, byte[] pbyteGammaTable);

		// Token: 0x060000F4 RID: 244
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetCameraGammaValue")]
		public static extern bool GetCameraGammaValue(IntPtr hCamera, out ushort pwValue);

		// Token: 0x060000F5 RID: 245
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetCameraGammaValue")]
		public static extern bool SetCameraGammaValue(IntPtr hCamera, ushort wValue);

		// Token: 0x060000F6 RID: 246
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetSharpnessMode")]
		public static extern bool SetSharpnessMode(IntPtr hCamera, byte byteSharpnessMode, ushort wSharpnessGain, byte byteSharpnessCoring);

		// Token: 0x060000F7 RID: 247
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetSharpnessMode")]
		public static extern bool GetSharpnessMode(IntPtr hCamera, out byte pbyteSharpnessMode, out ushort pwSharpnessGain, out byte pbyteSharpnessCoring);

		// Token: 0x060000F8 RID: 248
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetHueSaturationMode")]
		public static extern bool SetHueSaturationMode(IntPtr hCamera, byte byteHueSaturationMode, short shtHue, ushort wSaturation);

		// Token: 0x060000F9 RID: 249
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetHueSaturationMode")]
		public static extern bool GetHueSaturationMode(IntPtr hCamera, out byte pbyteHueSaturationMode, out short pshtHue, out ushort pwSaturation);

		// Token: 0x060000FA RID: 250
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetColorMatrix")]
		public static extern bool SetColorMatrix(IntPtr hCamera, byte byteColorMatrixMode, short[] pshtColorMatrix);

		// Token: 0x060000FB RID: 251
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetColorMatrix")]
		public static extern bool GetColorMatrix(IntPtr hCamera, out byte pbyteColorMatrixMode, short[] pshtColorMatrix);

		// Token: 0x060000FC RID: 252
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnableMirrorMode")]
		public static extern bool GetEnableMirrorMode(IntPtr hCamera, out byte pbyteMirrorMode);

		// Token: 0x060000FD RID: 253
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetMirrorMode")]
		public static extern bool SetMirrorMode(IntPtr hCamera, byte byteMirrorMode);

		// Token: 0x060000FE RID: 254
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetMirrorMode")]
		public static extern bool GetMirrorMode(IntPtr hCamera, out byte pbyteMirrorMode);

		// Token: 0x060000FF RID: 255
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetRotationMode")]
		public static extern bool SetRotationMode(IntPtr hCamera, byte byteRotationMode);

		// Token: 0x06000100 RID: 256
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetRotationMode")]
		public static extern bool GetRotationMode(IntPtr hCamera, out byte pbyteRotationMode);

		// Token: 0x06000101 RID: 257
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_SaveAVIA")]
		public static extern bool SaveAVIA(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

		// Token: 0x06000102 RID: 258
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_SaveAVIW")]
		public static extern bool SaveAVIW(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

		// Token: 0x06000103 RID: 259
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_SaveAVI")]
		public static extern bool SaveAVI(IntPtr hCamera, string pszFileName, uint dwCompressor, uint dwLength, IntPtr lpReserved);

		// Token: 0x06000104 RID: 260
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAVIStatus")]
		public static extern bool SetAVIStatus(IntPtr hCamera, byte byteAVIStatus);

		// Token: 0x06000105 RID: 261
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAVIStatus")]
		public static extern bool GetAVIStatus(IntPtr hCamera, out byte pbyteAVIStatus, out uint pdwTotalFrameCounts, out uint pdwCurrentFrameCounts);

		// Token: 0x06000106 RID: 262
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetAVIQuality")]
		public static extern bool SetAVIQuality(IntPtr hCamera, uint dwQuality);

		// Token: 0x06000107 RID: 263
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetAVIQuality")]
		public static extern bool GetAVIQuality(IntPtr hCamera, out uint pdwQuality);

		// Token: 0x06000108 RID: 264
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnableClock")]
		public static extern bool GetEnableClock(IntPtr hCamera, out uint pdwEnableClockMode, out uint pdwStandardClock, out uint pdwMinimumClock, out uint pdwMaximumClock);

		// Token: 0x06000109 RID: 265
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetClock")]
		public static extern bool SetClock(IntPtr hCamera, uint dwClockMode, uint dwClock);

		// Token: 0x0600010A RID: 266
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetClock")]
		public static extern bool GetClock(IntPtr hCamera, out uint pdwClockMode, out uint pdwClock);

		// Token: 0x0600010B RID: 267
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetFrameClock")]
		public static extern bool GetFrameClock(IntPtr hCamera, out ushort pwCurrentLinePerFrame, out ushort pwCurrentClockPerLine);

		// Token: 0x0600010C RID: 268
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetOutputFPS")]
		public static extern bool GetOutputFPS(IntPtr hCamera, out float pfFPS);

		// Token: 0x0600010D RID: 269
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetVBlankForFPS")]
		public static extern bool SetVBlankForFPS(IntPtr hCamera, uint dwVLines);

		// Token: 0x0600010E RID: 270
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetVBlankForFPS")]
		public static extern bool GetVBlankForFPS(IntPtr hCamera, out uint pdwVLines);

		// Token: 0x0600010F RID: 271
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetEnableDefectPixelCorrectionCount")]
		public static extern bool GetEnableDefectPixelCorrectionCount(IntPtr hCamera, out ushort pwCount);

		// Token: 0x06000110 RID: 272
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDefectPixelCorrectionMode")]
		public static extern bool GetDefectPixelCorrectionMode(IntPtr hCamera, out ushort pwMode);

		// Token: 0x06000111 RID: 273
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetDefectPixelCorrectionMode")]
		public static extern bool SetDefectPixelCorrectionMode(IntPtr hCamera, ushort wMode);

		// Token: 0x06000112 RID: 274
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDefectPixelCorrectionPosition")]
		public static extern bool GetDefectPixelCorrectionPosition(IntPtr hCamera, ushort wIndex, out uint pdwX, out uint pdwY);

		// Token: 0x06000113 RID: 275
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetDefectPixelCorrectionPosition")]
		public static extern bool SetDefectPixelCorrectionPosition(IntPtr hCamera, ushort wIndex, uint dwX, uint dwY);

		// Token: 0x06000114 RID: 276
		[DllImport("StCamD.dll", EntryPoint = "StCam_DetectDefectPixel")]
		public static extern bool DetectDefectPixel(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteRaw, ushort wThreshold);

		// Token: 0x06000115 RID: 277
		[DllImport("StCamD.dll", EntryPoint = "StCam_AddPreviewBitmapCallback")]
		public static extern bool AddPreviewBitmapCallback(IntPtr hCamera, StCam.fStCamPreviewBitmapCallbackFunc pPreviewBitmapCallbackFunc, IntPtr pContext, out uint pdwPreviewBitmapCallbackNo);

		// Token: 0x06000116 RID: 278
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemovePreviewBitmapCallback")]
		public static extern bool RemovePreviewBitmapCallback(IntPtr hCamera, uint dwPreviewBitmapCallbackNo);

		// Token: 0x06000117 RID: 279
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemoveAllPreviewBitmapCallback")]
		public static extern bool RemoveAllPreviewBitmapCallback(IntPtr hCamera);

		// Token: 0x06000118 RID: 280
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewBitmapCallbackCount")]
		public static extern bool GetPreviewBitmapCallbackCount(IntPtr hCamera, out uint pdwListCount);

		// Token: 0x06000119 RID: 281
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewBitmapCallback")]
		public static extern bool GetPreviewBitmapCallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamPreviewBitmapCallbackFunc ppPreviewBitmapCallbackFunc, out uint pdwCallbackFunctionNo);

		// Token: 0x0600011A RID: 282
		[DllImport("StCamD.dll", EntryPoint = "StCam_AddPreviewGDICallback")]
		public static extern bool AddPreviewGDICallback(IntPtr hCamera, StCam.fStCamPreviewGDICallbackFunc pPreviewGDICallbackFunc, IntPtr pContext, out uint pdwPreviewGDICallbackNo);

		// Token: 0x0600011B RID: 283
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemovePreviewGDICallback")]
		public static extern bool RemovePreviewGDICallback(IntPtr hCamera, uint dwPreviewGDICallbackNo);

		// Token: 0x0600011C RID: 284
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemoveAllPreviewGDICallback")]
		public static extern bool RemoveAllPreviewGDICallback(IntPtr hCamera);

		// Token: 0x0600011D RID: 285
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewGDICallbackCount")]
		public static extern bool GetPreviewGDICallbackCount(IntPtr hCamera, out uint pdwListCount);

		// Token: 0x0600011E RID: 286
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetPreviewGDICallback")]
		public static extern bool GetPreviewGDICallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamPreviewGDICallbackFunc ppPreviewGDICallbackFunc, out uint pdwCallbackFunctionNo);

		// Token: 0x0600011F RID: 287
		[DllImport("StCamD.dll", EntryPoint = "StCam_AddRawCallback")]
		public static extern bool AddRawCallback(IntPtr hCamera, StCam.fStCamRawCallbackFunc pRawCallbackFunc, IntPtr pContext, out uint pdwRawCallbackNo);

		// Token: 0x06000120 RID: 288
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemoveRawCallback")]
		public static extern bool RemoveRawCallback(IntPtr hCamera, uint dwRawCallbackNo);

		// Token: 0x06000121 RID: 289
		[DllImport("StCamD.dll", EntryPoint = "StCam_RemoveAllRawCallback")]
		public static extern bool RemoveAllRawCallback(IntPtr hCamera);

		// Token: 0x06000122 RID: 290
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetRawCallbackCount")]
		public static extern bool GetRawCallbackCount(IntPtr hCamera, out uint pdwListCount);

		// Token: 0x06000123 RID: 291
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetRawCallback")]
		public static extern bool GetRawCallback(IntPtr hCamera, uint dwCallbackIndex, out StCam.fStCamRawCallbackFunc ppRawCallbackFunc, out uint pdwCallbackFunctionNo);

		// Token: 0x06000124 RID: 292
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_SaveSettingFileA")]
		public static extern bool SaveSettingFileA(IntPtr hCamera, string pszFileName);

		// Token: 0x06000125 RID: 293
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_SaveSettingFileW")]
		public static extern bool SaveSettingFileW(IntPtr hCamera, string pszFileName);

		// Token: 0x06000126 RID: 294
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_SaveSettingFile")]
		public static extern bool SaveSettingFile(IntPtr hCamera, string pszFileName);

		// Token: 0x06000127 RID: 295
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_LoadSettingFileA")]
		public static extern bool LoadSettingFileA(IntPtr hCamera, string pszFileName);

		// Token: 0x06000128 RID: 296
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_LoadSettingFileW")]
		public static extern bool LoadSettingFileW(IntPtr hCamera, string pszFileName);

		// Token: 0x06000129 RID: 297
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_LoadSettingFile")]
		public static extern bool LoadSettingFile(IntPtr hCamera, string pszFileName);

		// Token: 0x0600012A RID: 298
		[DllImport("StCamD.dll", EntryPoint = "StCam_ResetSetting")]
		public static extern bool ResetSetting(IntPtr hCamera);

		// Token: 0x0600012B RID: 299
		[DllImport("StCamD.dll", EntryPoint = "StCam_CameraSetting")]
		public static extern bool CameraSetting(IntPtr hCamera, ushort wMode);

		// Token: 0x0600012C RID: 300
		[DllImport("StCamD.dll", EntryPoint = "StCam_ReadUserMemory")]
		public static extern bool ReadUserMemory(IntPtr hCamera, byte[] pbyteBuffer, ushort wOffset, ushort wLength);

		// Token: 0x0600012D RID: 301
		[DllImport("StCamD.dll", EntryPoint = "StCam_WriteUserMemory")]
		public static extern bool WriteUserMemory(IntPtr hCamera, byte[] pbyteBuffer, ushort wOffset, ushort wLength);

		// Token: 0x0600012E RID: 302
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_ReadCameraUserIDA")]
		public static extern bool ReadCameraUserIDA(IntPtr hCamera, out uint pdwCameraID, StringBuilder pszBuffer, uint dwBufferSize);

		// Token: 0x0600012F RID: 303
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_ReadCameraUserIDW")]
		public static extern bool ReadCameraUserIDW(IntPtr hCamera, out uint pdwCameraID, StringBuilder pszBuffer, uint dwBufferSize);

		// Token: 0x06000130 RID: 304
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_ReadCameraUserID")]
		public static extern bool ReadCameraUserID(IntPtr hCamera, out uint pdwCameraID, StringBuilder pszBuffer, uint dwBufferSize);

		// Token: 0x06000131 RID: 305
		[DllImport("StCamD.dll", CharSet = CharSet.Ansi, EntryPoint = "StCam_WriteCameraUserIDA")]
		public static extern bool WriteCameraUserIDA(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

		// Token: 0x06000132 RID: 306
		[DllImport("StCamD.dll", CharSet = CharSet.Unicode, EntryPoint = "StCam_WriteCameraUserIDW")]
		public static extern bool WriteCameraUserIDW(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

		// Token: 0x06000133 RID: 307
		[DllImport("StCamD.dll", CharSet = CharSet.Auto, EntryPoint = "StCam_WriteCameraUserID")]
		public static extern bool WriteCameraUserID(IntPtr hCamera, uint dwCameraID, string pszBuffer, uint dwBufferSize);

		// Token: 0x06000134 RID: 308
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetCameraVersion")]
		public static extern bool GetCameraVersion(IntPtr hCamera, out ushort pwUSBVendorID, out ushort pwUSBProductID, out ushort pwFPGAVersion, out ushort pwFirmVersion);

		// Token: 0x06000135 RID: 309
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDriverVersion")]
		public static extern bool GetDriverVersion(IntPtr hCamera, out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

		// Token: 0x06000136 RID: 310
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetUSBDllVersion")]
		public static extern bool GetUSBDllVersion(out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

		// Token: 0x06000137 RID: 311
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetCAMDllVersion")]
		public static extern bool GetCAMDllVersion(out uint pdwFileVersionMS, out uint pdwFileVersionLS, out uint pdwProductVersionMS, out uint pdwProductVersionLS);

		// Token: 0x06000138 RID: 312
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetUSBFunctionAddress")]
		public static extern bool GetUSBFunctionAddress(IntPtr hCamera, out byte pbyteUSBFunctionAddress);

		// Token: 0x06000139 RID: 313
		[DllImport("StCamD.dll", EntryPoint = "StCam_ConvertBitmapBGR24ToRGB24")]
		public static extern bool ConvertBitmapBGR24ToRGB24(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteBitmap);

		// Token: 0x0600013A RID: 314
		[DllImport("StCamD.dll", EntryPoint = "StCam_ConvertRawToBGR")]
		public static extern bool ConvertRawToBGR(IntPtr hCamera, uint dwWidth, uint dwHeight, IntPtr pbyteSrcRaw, IntPtr pbyteDstBGR, byte byteColorInterpolationMethod, uint dwPreviewPixelFormat);

		// Token: 0x0600013B RID: 315
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetControlArea")]
		public static extern bool SetControlArea(IntPtr hCamera, ushort[] pwSepalateX, ushort[] pwSepalateY);

		// Token: 0x0600013C RID: 316
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetControlArea")]
		public static extern bool GetControlArea(IntPtr hCamera, ushort[] pwSepalateX, ushort[] pwSepalateY);

		// Token: 0x0600013D RID: 317
		[DllImport("StCamD.dll", EntryPoint = "StCam_GetDigitalClamp")]
		public static extern bool GetDigitalClamp(IntPtr hCamera, out ushort pwValue);

		// Token: 0x0600013E RID: 318
		[DllImport("StCamD.dll", EntryPoint = "StCam_SetDigitalClamp")]
		public static extern bool SetDigitalClamp(IntPtr hCamera, ushort wValue);

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
