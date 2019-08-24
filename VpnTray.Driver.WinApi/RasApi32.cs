using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace VpnTray.Driver.WinApi
{
    public static class RasApi32
    {
        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasEnumEntries(
            [In][Optional] IntPtr reserved, 
            [In][Optional][MarshalAs(UnmanagedType.LPStr)] string phonebook,
            [In][Out][Optional] RasEntryName[] buffer, 
            [In][Out] ref int bufferSize,
            [Out] out int count);

        public static RasEntryName[] RasEnumEntries(string phonebook = null)
        {
            int entrySize = Marshal.SizeOf<RasEntryName>();

            var buffer = new RasEntryName[1];
            buffer[0].Size = entrySize;

            int bufferSize = entrySize;

            int result = RasEnumEntries(IntPtr.Zero, phonebook, buffer, ref bufferSize, out int count);

            switch (result)
            {
                case RasErrorCodes.ERROR_SUCCESS:
                    return count == 0 ? new RasEntryName[0] : buffer;
                case RasErrorCodes.ERROR_BUFFER_TOO_SMALL:
                    buffer = new RasEntryName[count];
                    buffer[0].Size = entrySize;

                    bufferSize = count * entrySize;

                    result = RasEnumEntries(IntPtr.Zero, phonebook, buffer, ref bufferSize, out _);

                    if (result == RasErrorCodes.ERROR_SUCCESS)
                    {
                        return buffer;
                    }

                    break;
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasGetEntryProperties(
            [In][Optional][MarshalAs(UnmanagedType.LPStr)] string phonebook,
            [In][MarshalAs(UnmanagedType.LPStr)] string entryName,
            [In][Out][Optional] ref RasEntry entry,
            [In][Out] ref int bufferSize,
            [Out][Optional] IntPtr deprecated1,
            [In][Out][Optional] IntPtr deprecated2);

        public static RasEntry RasGetEntryProperties(string entryName, string phonebook = null)
        {
            int entrySize = Marshal.SizeOf<RasEntry>();
            var entry = new RasEntry
            {
                Size = entrySize
            };

            int result = RasGetEntryProperties(phonebook, entryName, ref entry, ref entrySize);

            if (result == RasErrorCodes.ERROR_SUCCESS)
            {
                return entry;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasGetEntryDialParams(
            [In][Optional][MarshalAs(UnmanagedType.LPStr)] string phonebook, 
            [In][Out] ref RasDialParams dialParams,
            [Out] out bool passwordRetrieved);

        public static RasDialParams RasGetEntryDialParams(string entryName, string phonebook = null)
        {
            var dialparams = new RasDialParams
            {
                Size = Marshal.SizeOf<RasDialParams>(),
                EntryName = entryName
            };

            int result = RasGetEntryDialParams(phonebook, ref dialparams, out _);

            if (result == RasErrorCodes.ERROR_SUCCESS)
            {
                return dialparams;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasDial(
            [In][Optional] IntPtr extensions,
            [In][Optional][MarshalAs(UnmanagedType.LPStr)] string phonebook,
            [In] ref RasDialParams dialParams,
            [In] int notifierType,
            [In][Optional] RasDialFunc callback,
            [Out] out IntPtr connection);

        public static IntPtr RasDial(RasDialParams dialParams, string phonebook = null, RasDialFunc callback = null)
        {
            int result = RasDial(IntPtr.Zero, phonebook, ref dialParams, 0, callback, out IntPtr connection);
            if (result == RasErrorCodes.ERROR_SUCCESS)
            {
                return connection;
            }
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasHangUp([In] IntPtr connection);

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasEnumConnections(
            [In][Out][Optional] RasConn[] buffer,
            [In][Out] ref int bufferSize,
            [Out] out int count);

        public static RasConn[] RasEnumConnections()
        {
            int entrySize = Marshal.SizeOf<RasConn>();

            var buffer = new RasConn[1];
            buffer[0].Size = entrySize;

            int bufferSize = entrySize;

            int result = RasEnumConnections(buffer, ref bufferSize, out int count);

            switch (result)
            {
                case RasErrorCodes.ERROR_SUCCESS:
                    return count == 0 ? new RasConn[0] : buffer;

                case RasErrorCodes.ERROR_BUFFER_TOO_SMALL:
                {
                    buffer = new RasConn[count];
                    buffer[0].Size = entrySize;
                    bufferSize = count * entrySize;

                    result = RasEnumConnections(buffer, ref bufferSize, out _);

                    if (result == RasErrorCodes.ERROR_SUCCESS)
                    {
                        return buffer;
                    }

                    break;
                }
            }

            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        [DllImport("RasApi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int RasGetConnectStatus(
            [In] IntPtr handle,
            [In][Out] ref RasConnStatus status);

        public static RasConnStatus RasGetConnectStatus(IntPtr handle)
        {
            var status = new RasConnStatus
            {
                Size = Marshal.SizeOf<RasConnStatus>()
            };

            if (RasGetConnectStatus(handle, ref status) != RasErrorCodes.ERROR_SUCCESS)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            return status;
        }
    }
}