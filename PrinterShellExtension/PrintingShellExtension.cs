#region Using directives
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utility;
using System.Diagnostics;
using PrinterShellExtension.Properties;
#endregion


namespace PrinterShellExtension
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid(ConstFields.SHELL_EXT_GUID), ComVisible(true)]
    public class FileContextMenuExt : IShellExtInit, IContextMenu
    {
        private const string REGISTRY_VALUE = "OSU_Printing_App.PrinterShellExtension";
        private const int MAX_PATH_LENGTH = 260;

        // The name of the selected file.
        private string _selectedFile;

        private uint IDM_DISPLAY = 0;

        private string _menuText = "OSU Printers";
        private IntPtr _menuBmp = IntPtr.Zero;
        private string _verb = "OSU_Printers";
        private string _verbCanonicalName = "OSU_Printers";
        private string _verbHelpText = "OSU_Printers";

        private IList<Dictionary<string, string>> _printerList;

        public FileContextMenuExt()
        {
            // Load the bitmap for the menu item.
            Bitmap bmp = Resources.Logo;
            bmp.MakeTransparent(bmp.GetPixel(0, 0));
            this._menuBmp = bmp.GetHbitmap();
        }

        ~FileContextMenuExt()
        {
            if (this._menuBmp != IntPtr.Zero)
            {
                NativeMethods.DeleteObject(this._menuBmp);
                this._menuBmp = IntPtr.Zero;
            }
        }

        void ExecuteCommand(int position)
        {
            ConfigManager loader = new ConfigManager(GetConfigPath());
            loader.MovePrinterToFront(position);
            Process.Start(GetSshPath(), '"' + _selectedFile + "\" " + _printerList[position]["Name"]);
        }


        #region Shell Extension Registration

        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ShellExtReg.RegisterShellExtContextMenuHandler(t.GUID, "*",
                    REGISTRY_VALUE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ShellExtReg.UnregisterShellExtContextMenuHandler(t.GUID, "*");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        #endregion


        #region IShellExtInit Members

        /// <summary>
        /// Initialize the context menu handler.
        /// </summary>
        /// <param name="pidlFolder">
        /// A pointer to an ITEMIDLIST structure that uniquely identifies a folder.
        /// </param>
        /// <param name="pDataObj">
        /// A pointer to an IDataObject interface object that can be used to retrieve 
        /// the objects being acted upon.
        /// </param>
        /// <param name="hKeyProgID">
        /// The registry key for the file object or folder type.
        /// </param>
        public void Initialize(IntPtr pidlFolder, IntPtr pDataObj, IntPtr hKeyProgID)
        {
            if (pDataObj == IntPtr.Zero)
            {
                throw new ArgumentException("Pointer is zero.");
            }

            FORMATETC fe = new FORMATETC();
            fe.cfFormat = (short)CLIPFORMAT.CF_HDROP;
            fe.ptd = IntPtr.Zero;
            fe.dwAspect = DVASPECT.DVASPECT_CONTENT;
            fe.lindex = -1;
            fe.tymed = TYMED.TYMED_HGLOBAL;
            STGMEDIUM stm = new STGMEDIUM();

            // The pDataObj pointer contains the objects being acted upon. In this 
            // example, we get an HDROP handle for enumerating the selected files 
            // and folders.
            IDataObject dataObject = (IDataObject)Marshal.GetObjectForIUnknown(pDataObj);
            dataObject.GetData(ref fe, out stm);

            try
            {
                // Get an HDROP handle.
                IntPtr hDrop = stm.unionmember;
                if (hDrop == IntPtr.Zero)
                {
                    throw new ArgumentException("Pointer is zero.");
                }

                // Determine how many files are involved in this operation.
                uint nFiles = NativeMethods.DragQueryFile(hDrop, UInt32.MaxValue, null, 0);

                // This code sample displays the custom context menu item when only 
                // one file is selected. 
                if (nFiles == 1)
                {
                    // Get the path of the file.
                    StringBuilder fileName = new StringBuilder(MAX_PATH_LENGTH);
                    if (0 == NativeMethods.DragQueryFile(hDrop, 0, fileName,
                        fileName.Capacity))
                    {
                        Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                    }
                    this._selectedFile = fileName.ToString();
                }
                else
                {
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }
            finally
            {
                NativeMethods.ReleaseStgMedium(ref stm);
            }
        }

        #endregion



        #region IContextMenu Members

        /// <summary>
        /// Add commands to a shortcut menu.
        /// </summary>
        /// <param name="hMenu">A handle to the shortcut menu.</param>
        /// <param name="iMenu">
        /// The zero-based position at which to insert the first new menu item.
        /// </param>
        /// <param name="idCmdFirst">
        /// The minimum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="idCmdLast">
        /// The maximum value that the handler can specify for a menu item ID.
        /// </param>
        /// <param name="uFlags">
        /// Optional flags that specify how the shortcut menu can be changed.
        /// </param>
        /// <returns>
        /// If successful, returns an HRESULT value that has its severity value set 
        /// to SEVERITY_SUCCESS and its code value set to the offset of the largest 
        /// command identifier that was assigned, plus one.
        /// </returns>
        public int QueryContextMenu(
            IntPtr hMenu,
            uint iMenu,
            uint idCmdFirst,
            uint idCmdLast,
            uint uFlags)
        {
            // If uFlags include CMF_DEFAULTONLY then we should not do anything.
            if (((uint)CMF.CMF_DEFAULTONLY & uFlags) != 0)
            {
                return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0);
            }

            uint uID = idCmdFirst;

            IntPtr hSubmenu = NativeMethods.CreatePopupMenu();

            // Use InsertMenuItem to add main menu items.
            MENUITEMINFO mii = new MENUITEMINFO();
            mii.cbSize = (uint)Marshal.SizeOf(mii);

            mii.fMask = MIIM.MIIM_BITMAP | MIIM.MIIM_SUBMENU | MIIM.MIIM_FTYPE | MIIM.MIIM_ID
                | MIIM.MIIM_STATE | MIIM.MIIM_STRING;

            mii.wID = idCmdFirst + IDM_DISPLAY;
            mii.fType = MFT.MFT_STRING;
            mii.dwTypeData = this._menuText;
            mii.fState = MFS.MFS_ENABLED;
            mii.hbmpItem = this._menuBmp;
            mii.hSubMenu = hSubmenu;

            if (!NativeMethods.InsertMenuItem(hMenu, iMenu, true, ref mii))
            {
                return Marshal.GetHRForLastWin32Error();
            }

            // Get all loaded printers.
            _printerList = LoadAllPrinters();
            for (int i = 0; i < _printerList.Count; ++i)
            {
                // Use InsertMenu to add submenu items.
                if (!NativeMethods.InsertMenu(
                    hSubmenu, (uint)i,
                    0x00000400, uID++,
                    _printerList[i]["Location"]))
                {
                    return Marshal.GetHRForLastWin32Error();
                }
            }

            // Return an HRESULT value with the severity set to SEVERITY_SUCCESS. 
            // Set the code value to the offset of the largest command identifier 
            // that was not assigned
            return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0,
                uID - idCmdFirst);
        }

        /// <summary>
        /// Carry out the command associated with a shortcut menu item.
        /// </summary>
        /// <param name="pici">
        /// A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure 
        /// containing information about the command. 
        /// </param>
        public void InvokeCommand(IntPtr pici)
        {
            bool isUnicode = false;
            // Determine which structure is being passed in, CMINVOKECOMMANDINFO or 
            // CMINVOKECOMMANDINFOEX based on the cbSize member of lpcmi. Although 
            // the lpcmi parameter is declared in Shlobj.h as a CMINVOKECOMMANDINFO 
            // structure, in practice it often points to a CMINVOKECOMMANDINFOEX 
            // structure. This struct is an extended version of CMINVOKECOMMANDINFO 
            // and has additional members that allow Unicode strings to be passed.
            CMINVOKECOMMANDINFO ici = (CMINVOKECOMMANDINFO)Marshal.PtrToStructure(
                pici, typeof(CMINVOKECOMMANDINFO));

            CMINVOKECOMMANDINFOEX iciex = new CMINVOKECOMMANDINFOEX();
            if (ici.cbSize == Marshal.SizeOf(typeof(CMINVOKECOMMANDINFOEX)))
            {
                if ((ici.fMask & CMIC.CMIC_MASK_UNICODE) != 0)
                {
                    isUnicode = true;
                    iciex = (CMINVOKECOMMANDINFOEX)Marshal.PtrToStructure(pici,
                        typeof(CMINVOKECOMMANDINFOEX));
                }
            }

            // Determines whether the command is identified by its offset or verb.
            // There are two ways to identify commands:
            // 
            //   1) The command's verb string 
            //   2) The command's identifier offset
            // 
            // If the high-order word of lpcmi->lpVerb (for the ANSI case) or 
            // lpcmi->lpVerbW (for the Unicode case) is nonzero, lpVerb or lpVerbW 
            // holds a verb string. If the high-order word is zero, the command 
            // offset is in the low-order word of lpcmi->lpVerb.

            // For the ANSI case, if the high-order word is not zero, the command's 
            // verb string is in lpcmi->lpVerb. 
            if (!isUnicode && NativeMethods.HighWord(ici.verb.ToInt32()) != 0)
            {
                // Is the verb supported by this context menu extension?
                if (Marshal.PtrToStringAnsi(ici.verb) == this._verb)
                {
                    return;
                }
                else
                {
                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }

            // For the Unicode case, if the high-order word is not zero, the 
            // command's verb string is in lpcmi->lpVerbW. 
            else if (isUnicode && NativeMethods.HighWord(iciex.verbW.ToInt32()) != 0)
            {
                // Is the verb supported by this context menu extension?
                if (Marshal.PtrToStringUni(iciex.verbW) == this._verb)
                {
                    return;
                }
                else
                {
                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }

            // If the command cannot be identified through the verb string, then 
            // check the identifier offset.
            else
            {
                // Is the command identifier offset supported by this context menu 
                // extension?
                int Count = NativeMethods.LowWord(ici.verb.ToInt32());
                if (0 <= Count && Count < _printerList.Count)
                {
                    ExecuteCommand(ici.verb.ToInt32());
                }
                else
                {
                    // If the verb is not recognized by the context menu handler, it 
                    // must return E_FAIL to allow it to be passed on to the other 
                    // context menu handlers that might implement that verb.
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL);
                }
            }
        }

        /// <summary>
        /// Get information about a shortcut menu command, including the help string 
        /// and the language-independent, or canonical, name for the command.
        /// </summary>
        /// <param name="idCmd">Menu command identifier offset.</param>
        /// <param name="uFlags">
        /// Flags specifying the information to return. This parameter can have one 
        /// of the following values: GCS_HELPTEXTA, GCS_HELPTEXTW, GCS_VALIDATEA, 
        /// GCS_VALIDATEW, GCS_VERBA, GCS_VERBW.
        /// </param>
        /// <param name="pReserved">Reserved. Must be IntPtr.Zero</param>
        /// <param name="pszName">
        /// The address of the buffer to receive the null-terminated string being 
        /// retrieved.
        /// </param>
        /// <param name="cchMax">
        /// Size of the buffer, in characters, to receive the null-terminated string.
        /// </param>
        public void GetCommandString(
            UIntPtr idCmd,
            uint uFlags,
            IntPtr pReserved,
            StringBuilder pszName,
            uint cchMax)
        {
            if (idCmd.ToUInt32() == IDM_DISPLAY)
            {
                switch ((GCS)uFlags)
                {
                    case GCS.GCS_VERBW:
                        if (this._verbCanonicalName.Length > cchMax - 1)
                        {
                            Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER);
                        }
                        else
                        {
                            pszName.Clear();
                            pszName.Append(this._verbCanonicalName);
                        }
                        break;

                    case GCS.GCS_HELPTEXTW:
                        if (this._verbHelpText.Length > cchMax - 1)
                        {
                            Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER);
                        }
                        else
                        {
                            pszName.Clear();
                            pszName.Append(this._verbHelpText);
                        }
                        break;
                }
            }
        }

        #endregion

        #region Utility Method

        private static string GetRunningDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        private static string GetConfigPath()
        {
            return Path.Combine(GetRunningDirectory(), ConstFields.CONFIGRATION_FILE_NAME);
        }
        private static string GetSshPath()
        {
            return Path.Combine(GetRunningDirectory(), ConstFields.SSH_PRINT_FILE_NAME);
        }
        private static IList<Dictionary<string, string>> LoadAllPrinters()
        {
            ConfigManager loader = new ConfigManager(GetConfigPath());
            return loader.GetAllLoadedPrinters();
        }

        #endregion
    }
}