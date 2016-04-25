using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RedSiaCore.Utils;

namespace RedSia.Utils
{
    /// <summary>
    ///    Чтение пользовательского ввода с клавиатуры
    /// </summary>
    public class GlobalKeyboard
    {
        #region Constant, Structure and Delegate Definitions

        /// <summary>
        ///     defines the callback type for the hook
        /// </summary>
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        public struct KeyboardHookStruct
        {
            public int DwExtraInfo;
            public int Flags;
            public int ScanCode;
            public int Time;
            public int VkCode;
        }

        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x100;
        private const int WmKeyup = 0x101;
        private const int WmSyskeydown = 0x104;
        private const int WmSyskeyup = 0x105;

        #endregion

        #region Instance Variables

        /// <summary>
        ///     The collections of keys to watch for
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();

        /// <summary>
        ///     Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        private IntPtr _hhook = IntPtr.Zero;

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        ///     Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see>
        ///         <cref>globalKeyboardHook</cref>
        ///     </see>
        ///     class and installs the keyboard hook.
        /// </summary>
        public GlobalKeyboard()
        {
            HookedKeys.Add(Keys.Add);
            HookedKeys.Add(Keys.A);
            HookedKeys.Add(Keys.Alt);
            HookedKeys.Add(Keys.Apps);
            HookedKeys.Add(Keys.Attn);
            HookedKeys.Add(Keys.B);
            HookedKeys.Add(Keys.Back);
            HookedKeys.Add(Keys.BrowserBack);
            HookedKeys.Add(Keys.BrowserFavorites);
            HookedKeys.Add(Keys.BrowserForward);
            HookedKeys.Add(Keys.BrowserHome);
            HookedKeys.Add(Keys.BrowserRefresh);
            HookedKeys.Add(Keys.BrowserSearch);
            HookedKeys.Add(Keys.BrowserStop);
            HookedKeys.Add(Keys.C);
            HookedKeys.Add(Keys.Cancel);
            HookedKeys.Add(Keys.Capital);
            HookedKeys.Add(Keys.CapsLock);
            HookedKeys.Add(Keys.Clear);
            HookedKeys.Add(Keys.Control);
            HookedKeys.Add(Keys.ControlKey);
            HookedKeys.Add(Keys.Crsel);
            HookedKeys.Add(Keys.D);
            HookedKeys.Add(Keys.D0);
            HookedKeys.Add(Keys.D1);
            HookedKeys.Add(Keys.D2);
            HookedKeys.Add(Keys.D3);
            HookedKeys.Add(Keys.D4);
            HookedKeys.Add(Keys.D5);
            HookedKeys.Add(Keys.D6);
            HookedKeys.Add(Keys.D7);
            HookedKeys.Add(Keys.D8);
            HookedKeys.Add(Keys.D9);
            HookedKeys.Add(Keys.Decimal);
            HookedKeys.Add(Keys.Delete);
            HookedKeys.Add(Keys.Divide);
            HookedKeys.Add(Keys.Down);
            HookedKeys.Add(Keys.E);
            HookedKeys.Add(Keys.End);
            HookedKeys.Add(Keys.Enter);
            HookedKeys.Add(Keys.EraseEof);
            HookedKeys.Add(Keys.Escape);
            HookedKeys.Add(Keys.Execute);
            HookedKeys.Add(Keys.Exsel);
            HookedKeys.Add(Keys.F);
            HookedKeys.Add(Keys.F1);
            HookedKeys.Add(Keys.F2);
            HookedKeys.Add(Keys.F3);
            HookedKeys.Add(Keys.F4);
            HookedKeys.Add(Keys.F5);
            HookedKeys.Add(Keys.F6);
            HookedKeys.Add(Keys.F7);
            HookedKeys.Add(Keys.F8);
            HookedKeys.Add(Keys.F9);
            HookedKeys.Add(Keys.F10);
            HookedKeys.Add(Keys.F11);
            HookedKeys.Add(Keys.F12);
            HookedKeys.Add(Keys.F13);
            HookedKeys.Add(Keys.F14);
            HookedKeys.Add(Keys.F15);
            HookedKeys.Add(Keys.F16);
            HookedKeys.Add(Keys.F17);
            HookedKeys.Add(Keys.F18);
            HookedKeys.Add(Keys.F19);
            HookedKeys.Add(Keys.F20);
            HookedKeys.Add(Keys.F21);
            HookedKeys.Add(Keys.F22);
            HookedKeys.Add(Keys.F23);
            HookedKeys.Add(Keys.F24);
            HookedKeys.Add(Keys.FinalMode);
            HookedKeys.Add(Keys.G);
            HookedKeys.Add(Keys.H);
            HookedKeys.Add(Keys.HanguelMode);
            HookedKeys.Add(Keys.HangulMode);
            HookedKeys.Add(Keys.HanjaMode);
            HookedKeys.Add(Keys.Help);
            HookedKeys.Add(Keys.Home);
            HookedKeys.Add(Keys.I);
            HookedKeys.Add(Keys.IMEAccept);
            HookedKeys.Add(Keys.IMEAceept);
            HookedKeys.Add(Keys.IMEConvert);
            HookedKeys.Add(Keys.IMEModeChange);
            HookedKeys.Add(Keys.IMENonconvert);
            HookedKeys.Add(Keys.J);
            HookedKeys.Add(Keys.JunjaMode);
            HookedKeys.Add(Keys.K);
            HookedKeys.Add(Keys.KanaMode);
            HookedKeys.Add(Keys.KanjiMode);
            HookedKeys.Add(Keys.KeyCode);
            HookedKeys.Add(Keys.L);
            HookedKeys.Add(Keys.LButton);
            HookedKeys.Add(Keys.LControlKey);
            HookedKeys.Add(Keys.LMenu);
            HookedKeys.Add(Keys.LShiftKey);
            HookedKeys.Add(Keys.LWin);
            HookedKeys.Add(Keys.LaunchApplication1);
            HookedKeys.Add(Keys.LaunchApplication2);
            HookedKeys.Add(Keys.LaunchMail);
            HookedKeys.Add(Keys.Left);
            HookedKeys.Add(Keys.LineFeed);
            HookedKeys.Add(Keys.M);
            HookedKeys.Add(Keys.MButton);
            HookedKeys.Add(Keys.MediaNextTrack);
            HookedKeys.Add(Keys.MediaPlayPause);
            HookedKeys.Add(Keys.MediaPreviousTrack);
            HookedKeys.Add(Keys.MediaStop);
            HookedKeys.Add(Keys.Menu);
            HookedKeys.Add(Keys.Modifiers);
            HookedKeys.Add(Keys.Multiply);
            HookedKeys.Add(Keys.N);
            HookedKeys.Add(Keys.Next);
            HookedKeys.Add(Keys.NoName);
            HookedKeys.Add(Keys.None);
            HookedKeys.Add(Keys.NumLock);
            HookedKeys.Add(Keys.NumPad0);
            HookedKeys.Add(Keys.NumPad1);
            HookedKeys.Add(Keys.NumPad2);
            HookedKeys.Add(Keys.NumPad3);
            HookedKeys.Add(Keys.NumPad4);
            HookedKeys.Add(Keys.NumPad5);
            HookedKeys.Add(Keys.O);
            HookedKeys.Add(Keys.Oem1);
            HookedKeys.Add(Keys.Oem2);
            HookedKeys.Add(Keys.Oem3);
            HookedKeys.Add(Keys.Oem4);
            HookedKeys.Add(Keys.Oem5);
            HookedKeys.Add(Keys.Oem6);
            HookedKeys.Add(Keys.Oem7);
            HookedKeys.Add(Keys.Oem8);
            HookedKeys.Add(Keys.Oem102);
            HookedKeys.Add(Keys.OemBackslash);
            HookedKeys.Add(Keys.OemClear);
            HookedKeys.Add(Keys.OemCloseBrackets);
            HookedKeys.Add(Keys.OemMinus);
            HookedKeys.Add(Keys.OemOpenBrackets);
            HookedKeys.Add(Keys.OemPeriod);
            HookedKeys.Add(Keys.OemPipe);
            HookedKeys.Add(Keys.OemQuestion);
            HookedKeys.Add(Keys.OemQuotes);
            HookedKeys.Add(Keys.OemSemicolon);
            HookedKeys.Add(Keys.Oemcomma);
            HookedKeys.Add(Keys.Oemplus);
            HookedKeys.Add(Keys.Oemtilde);
            HookedKeys.Add(Keys.P);
            HookedKeys.Add(Keys.Pa1);
            HookedKeys.Add(Keys.Packet);
            HookedKeys.Add(Keys.PageDown);
            HookedKeys.Add(Keys.PageUp);
            HookedKeys.Add(Keys.Pause);
            HookedKeys.Add(Keys.Play);
            HookedKeys.Add(Keys.Print);
            HookedKeys.Add(Keys.PrintScreen);
            HookedKeys.Add(Keys.Prior);
            HookedKeys.Add(Keys.ProcessKey);
            HookedKeys.Add(Keys.Q);
            HookedKeys.Add(Keys.R);
            HookedKeys.Add(Keys.RButton);
            HookedKeys.Add(Keys.RControlKey);
            HookedKeys.Add(Keys.RMenu);
            HookedKeys.Add(Keys.RShiftKey);
            HookedKeys.Add(Keys.RWin);
            HookedKeys.Add(Keys.Return);
            HookedKeys.Add(Keys.Right);
            HookedKeys.Add(Keys.S);
            HookedKeys.Add(Keys.Scroll);
            HookedKeys.Add(Keys.Select);
            HookedKeys.Add(Keys.SelectMedia);
            HookedKeys.Add(Keys.Separator);
            HookedKeys.Add(Keys.Shift);
            HookedKeys.Add(Keys.ShiftKey);
            HookedKeys.Add(Keys.Sleep);
            HookedKeys.Add(Keys.Snapshot);
            HookedKeys.Add(Keys.Space);
            HookedKeys.Add(Keys.Subtract);
            HookedKeys.Add(Keys.T);
            HookedKeys.Add(Keys.Tab);
            HookedKeys.Add(Keys.U);
            HookedKeys.Add(Keys.Up);
            HookedKeys.Add(Keys.V);
            HookedKeys.Add(Keys.VolumeDown);
            HookedKeys.Add(Keys.VolumeMute);
            HookedKeys.Add(Keys.VolumeUp);
            HookedKeys.Add(Keys.W);
            HookedKeys.Add(Keys.W);
            HookedKeys.Add(Keys.X);
            HookedKeys.Add(Keys.XButton1);
            HookedKeys.Add(Keys.XButton2);
            HookedKeys.Add(Keys.Y);
            HookedKeys.Add(Keys.Z);
            HookedKeys.Add(Keys.Zoom);


            Hook();
        }

        /// <summary>
        ///     Releases unmanaged resources and performs other cleanup operations before the
        ///     <see>
        ///         <cref>globalKeyboardHook</cref>
        ///     </see>
        ///     is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        ~GlobalKeyboard()
        {
            Unhook();
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///     Installs the global hook
        /// </summary>
        public void Hook()
        {
            var hInstance = LoadLibrary("User32");
            _hhook = SetWindowsHookEx(WhKeyboardLl, HookProc, hInstance, 0);
        }

        /// <summary>
        ///     Uninstalls the global hook
        /// </summary>
        public void Unhook()
        {
            UnhookWindowsHookEx(_hhook);
        }

        /// <summary>
        ///     The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        public int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                var key = (Keys) lParam.VkCode;
                if (HookedKeys.Contains(key))
                {
                    var kea = new KeyEventArgs(key);
                    if ((wParam == WmKeydown || wParam == WmSyskeydown) && (KeyDown != null))
                    {
                        if (!KeyboardImulation.ProgrammingInput) KeyDown(this, kea);
                    }
                    else if ((wParam == WmKeyup || wParam == WmSyskeyup) && (KeyUp != null))
                    {
                        if (!KeyboardImulation.ProgrammingInput) KeyUp(this, kea);
                    }
                    if (kea.Handled)
                        return 1;
                }
            }
            return CallNextHookEx(_hhook, code, wParam, ref lParam);
        }

        #endregion

        #region DLL imports

        /// <summary>
        ///     Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance,
            uint threadId);

        /// <summary>
        ///     Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        ///     Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        ///     Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        #endregion
    }
}