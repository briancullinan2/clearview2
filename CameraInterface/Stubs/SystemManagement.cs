// Minimal stub for System.Management types used in CameraManager to allow compilation in environments
// where System.Management is not available. On Windows full framework this can be removed.
using System;
using System.Collections.Generic;

namespace System.Management
{
    public class ManagementObjectSearcher : IDisposable
    {
        private string _query;
        public ManagementObjectSearcher(string query) { _query = query; }
        public IEnumerable<object> Get() { return new object[0]; }
        public void Dispose() { }
    }

    public class ManagementBaseObject
    {
        public object GetPropertyValue(string name) { return ""; }
    }

    public class ManagementException : Exception { }
}
