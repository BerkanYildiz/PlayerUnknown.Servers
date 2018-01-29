namespace PlayerUnknown.Leaker
{
    internal class Options
    {
        public static string TargetProcess      = "TslGame";
        public static string YourProcess        = "HandleLeaker.exe";
        public static uint DesiredAccess        = 0x0010;
        public static int DelayToWait           = 10;
        public static uint ObjectTimeout        = 1000;
        public static bool UseDuplicateHandle   = false;
    }
}