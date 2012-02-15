using System;
using Microsoft.Win32;

namespace Artwork_Stack.Tools
{
    internal static class WinRegistry
    {
        private const string RegPath = @"Software\ArtworkStack";

        internal static T GetValue<T>(string key)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            Object value;
            try
            {
                value = HKLM.OpenSubKey(RegPath).GetValue(key);
            }
            catch
            {
                value = null;
            }
            HKLM.Close();

            if(typeof(T) == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value);
            }
            else
            {
                return (T)value;
            }
        }

        internal static void SetValue(string key, object value)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Artwrk = null;
            try
            {
                Artwrk = HKLM.OpenSubKey(RegPath, true);
            }
            finally
            {
                if (Artwrk == null) Artwrk = HKLM.CreateSubKey(RegPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (Artwrk != null)
            {
                Artwrk.SetValue(key, value);
                Artwrk.Close();
            }
            HKLM.Close();
        }
        
        internal static class Keys
        {
            public const string DefaultPath       = "DefaultPath";
            public const string RecurseTraversing = "RecurseTraversing";
            public const string GroupByAlbum      = "GroupByAlbum";
            public const string SkipExisting      = "SkipExisting";
            public const string ShowJobs          = "ShowJobs";
        }
    }
}
