using System;
using Microsoft.Win32;

namespace Artwork_Stack.Tools
{
    internal static class WinRegistry
    {
        private const string RegPath = @"Software\ArtworkStack";

        internal static T GetValue<T>(string key)
        {
            RegistryKey HKCU = Registry.CurrentUser;
            Object value;
            try
            {
                value = HKCU.OpenSubKey(RegPath).GetValue(key);
            }
            catch
            {
                value = null;
            }
            HKCU.Close();

            if(typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }
            else
            {
                return (T)value;
            }
        }

        internal static void SetValue(string key, object value)
        {
            RegistryKey HKCU = Registry.CurrentUser;
            RegistryKey Artwrk = null;
            try
            {
                Artwrk = HKCU.OpenSubKey(RegPath, true);
            }
            finally
            {
                if (Artwrk == null) Artwrk = HKCU.CreateSubKey(RegPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (Artwrk != null)
            {
                Artwrk.SetValue(key, value);
                Artwrk.Close();
            }
            HKCU.Close();
        }
        
        internal static class Keys
        {
            public const string DefaultPath       = "DefaultPath";
            public const string RecurseTraversing = "RecurseTraversing";
            public const string GroupByAlbum      = "GroupByAlbum";
            public const string SkipExisting      = "SkipExisting";
            public const string ShowJobs          = "ShowJobs";
            public const string Crop              = "Crop";
            public const string Resize            = "Resize";
            public const string ResizeTo          = "ResizeTo";
        }
    }
}
