using System;

namespace DirectShowLib
{
    public enum FilterCategory
    {
        VideoInputDevice
    }

    public class DsDevice
    {
        public string Name { get; set; }
        public string DevicePath { get; set; }

        public static DsDevice[] GetDevicesOfCat(FilterCategory cat)
        {
            // Return empty list as a safe default for compilation/runtime when DirectShow not available
            return new DsDevice[0];
        }
    }
}
