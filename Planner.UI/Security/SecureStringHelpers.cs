using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Planner.UI
{
    public static class SecureStringHelpers
    {
        public static string Unsecure(this SecureString secureStr)
        {
            if (secureStr == null)
                return string.Empty;

            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(secureStr);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
