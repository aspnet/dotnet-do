using Microsoft.Extensions.PlatformAbstractions;

namespace DotNetDo.Helpers
{
    public static class OS
    {
        public static readonly bool IsWindows = PlatformServices.Default.Runtime.OperatingSystemPlatform == Platform.Windows;
        public static readonly bool IsMacOSX = PlatformServices.Default.Runtime.OperatingSystemPlatform == Platform.Darwin;
        public static readonly bool IsLinux = PlatformServices.Default.Runtime.OperatingSystemPlatform == Platform.Linux;

        public static readonly string ExecutableSuffix = GetPlatformSpecificValue(win: ".exe", unix: string.Empty);
        public static readonly string DynamicLibrarySuffix = GetPlatformSpecificValue(win: ".dll", mac: ".dylib", linux: ".so");
        public static readonly string DynamicLibraryPrefix = GetPlatformSpecificValue(win: string.Empty, unix: "lib");

        public static string ExeName(string exe) => $"{exe}{ExecutableSuffix}";
        public static string LibName(string lib) => $"{DynamicLibraryPrefix}{lib}";
        public static string LibFileName(string lib) => $"{LibName(lib)}{DynamicLibrarySuffix}";

        private static string GetPlatformSpecificValue(string win, string unix, string other = null) => GetPlatformSpecificValue(win, unix, unix, other);
        private static string GetPlatformSpecificValue(string win, string mac, string linux, string other = null)
        {
            return IsWindows ? win : (IsMacOSX ? mac : (IsLinux ? linux : other));
        }
    }
}
