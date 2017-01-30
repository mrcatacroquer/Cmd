using System;

namespace Codice.CmdRunner
{
    public class PlatformIdentifier
    {
        private static bool bIsWindowsInitialized = false;
        private static bool bIsWindows = false;
        public static bool IsWindows()
        {
            if (!bIsWindowsInitialized)
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32Windows:
                    case PlatformID.Win32NT:
                        bIsWindows = true;
                        break;
                }
                bIsWindowsInitialized = true;
            }
            return bIsWindows;
        }

        private static bool bIsMacInitialized = false;
        private static bool bIsMac = false;
        public static bool IsMac()
        {
            if (!bIsMacInitialized)
            {
                if (!IsWindows())
                {
                    

                    System.Version v = Environment.Version;
                    int p = (int)Environment.OSVersion.Platform;

                    if ((v.Major >= 3 && v.Minor >= 5) ||
                        (IsRunningUnderMono() && v.Major >= 2 && v.Minor >= 2))
                    {
                        //MacOs X exist in the enumeration
                        bIsMac = p == 6;
                    }
                    else
                    {
                        if ((p == 4) || (p == 128))
                        {
                            int major = Environment.OSVersion.Version.Major;

                            // Darwin tiger is 8, darwin leopard is 9,
                            // darwin snow leopard is 10
                            // DAVE: this is not very nice, as it may conflict
                            // on other OS like solaris or aix.
                            bIsMac = (major == 8 || major == 9 || major == 10);
                        }
                    }
                }

                bIsMacInitialized = true;
            }

            return bIsMac;
        }


        private static bool IsRunningUnderMono()
        {
            Type t = Type.GetType("Mono.Runtime");

            return (t != null);
        }
    }
}
