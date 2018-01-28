namespace PlayerUnknown.Events.Hooks
{
    /// <summary>
    /// Virtual Keys
    /// </summary>
    public enum VKeys
    {
        Lbutton = 0x01, // Left mouse button

        Rbutton = 0x02, // Right mouse button

        Cancel = 0x03, // Control-break processing

        Mbutton = 0x04, // Middle mouse button (three-button mouse)

        Xbutton1 = 0x05, // Windows 2000/XP: X1 mouse button

        Xbutton2 = 0x06, // Windows 2000/XP: X2 mouse button

        // 0x07   // Undefined
        Back = 0x08, // BACKSPACE key

        Tab = 0x09, // TAB key

        // 0x0A-0x0B,  // Reserved
        Clear = 0x0C, // CLEAR key

        Return = 0x0D, // ENTER key

        // 0x0E-0x0F, // Undefined
        Shift = 0x10, // SHIFT key

        Control = 0x11, // CTRL key

        Menu = 0x12, // ALT key

        Pause = 0x13, // PAUSE key

        Capital = 0x14, // CAPS LOCK key

        Kana = 0x15, // Input Method Editor (IME) Kana mode

        Hangul = 0x15, // IME Hangul mode

        // 0x16,  // Undefined
        Junja = 0x17, // IME Junja mode

        Final = 0x18, // IME final mode

        Hanja = 0x19, // IME Hanja mode

        Kanji = 0x19, // IME Kanji mode

        // 0x1A,  // Undefined
        Escape = 0x1B, // ESC key

        Convert = 0x1C, // IME convert

        Nonconvert = 0x1D, // IME nonconvert

        Accept = 0x1E, // IME accept

        Modechange = 0x1F, // IME mode change request

        Space = 0x20, // SPACEBAR

        Prior = 0x21, // PAGE UP key

        Next = 0x22, // PAGE DOWN key

        End = 0x23, // END key

        Home = 0x24, // HOME key

        Left = 0x25, // LEFT ARROW key

        Up = 0x26, // UP ARROW key

        Right = 0x27, // RIGHT ARROW key

        Down = 0x28, // DOWN ARROW key

        Select = 0x29, // SELECT key

        Print = 0x2A, // PRINT key

        Execute = 0x2B, // EXECUTE key

        Snapshot = 0x2C, // PRINT SCREEN key

        Insert = 0x2D, // INS key

        Delete = 0x2E, // DEL key

        Help = 0x2F, // HELP key

        Key0 = 0x30, // 0 key

        Key1 = 0x31, // 1 key

        Key2 = 0x32, // 2 key

        Key3 = 0x33, // 3 key

        Key4 = 0x34, // 4 key

        Key5 = 0x35, // 5 key

        Key6 = 0x36, // 6 key

        Key7 = 0x37, // 7 key

        Key8 = 0x38, // 8 key

        Key9 = 0x39, // 9 key

        // 0x3A-0x40, // Undefined
        KeyA = 0x41, // A key

        KeyB = 0x42, // B key

        KeyC = 0x43, // C key

        KeyD = 0x44, // D key

        KeyE = 0x45, // E key

        KeyF = 0x46, // F key

        KeyG = 0x47, // G key

        KeyH = 0x48, // H key

        KeyI = 0x49, // I key

        KeyJ = 0x4A, // J key

        KeyK = 0x4B, // K key

        KeyL = 0x4C, // L key

        KeyM = 0x4D, // M key

        KeyN = 0x4E, // N key

        KeyO = 0x4F, // O key

        KeyP = 0x50, // P key

        KeyQ = 0x51, // Q key

        KeyR = 0x52, // R key

        KeyS = 0x53, // S key

        KeyT = 0x54, // T key

        KeyU = 0x55, // U key

        KeyV = 0x56, // V key

        KeyW = 0x57, // W key

        KeyX = 0x58, // X key

        KeyY = 0x59, // Y key

        KeyZ = 0x5A, // Z key

        Lwin = 0x5B, // Left Windows key (Microsoft Natural keyboard)

        Rwin = 0x5C, // Right Windows key (Natural keyboard)

        Apps = 0x5D, // Applications key (Natural keyboard)

        // 0x5E, // Reserved
        Sleep = 0x5F, // Computer Sleep key

        Numpad0 = 0x60, // Numeric keypad 0 key

        Numpad1 = 0x61, // Numeric keypad 1 key

        Numpad2 = 0x62, // Numeric keypad 2 key

        Numpad3 = 0x63, // Numeric keypad 3 key

        Numpad4 = 0x64, // Numeric keypad 4 key

        Numpad5 = 0x65, // Numeric keypad 5 key

        Numpad6 = 0x66, // Numeric keypad 6 key

        Numpad7 = 0x67, // Numeric keypad 7 key

        Numpad8 = 0x68, // Numeric keypad 8 key

        Numpad9 = 0x69, // Numeric keypad 9 key

        Multiply = 0x6A, // Multiply key

        Add = 0x6B, // Add key

        Separator = 0x6C, // Separator key

        Subtract = 0x6D, // Subtract key

        Decimal = 0x6E, // Decimal key

        Divide = 0x6F, // Divide key

        F1 = 0x70, // F1 key

        F2 = 0x71, // F2 key

        F3 = 0x72, // F3 key

        F4 = 0x73, // F4 key

        F5 = 0x74, // F5 key

        F6 = 0x75, // F6 key

        F7 = 0x76, // F7 key

        F8 = 0x77, // F8 key

        F9 = 0x78, // F9 key

        F10 = 0x79, // F10 key

        F11 = 0x7A, // F11 key

        F12 = 0x7B, // F12 key

        F13 = 0x7C, // F13 key

        F14 = 0x7D, // F14 key

        F15 = 0x7E, // F15 key

        F16 = 0x7F, // F16 key

        F17 = 0x80, // F17 key  

        F18 = 0x81, // F18 key  

        F19 = 0x82, // F19 key  

        F20 = 0x83, // F20 key  

        F21 = 0x84, // F21 key  

        F22 = 0x85, // F22 key, (PPC only) Key used to lock device.

        F23 = 0x86, // F23 key  

        F24 = 0x87, // F24 key  

        // 0x88-0X8F,  // Unassigned
        Numlock = 0x90, // NUM LOCK key

        Scroll = 0x91, // SCROLL LOCK key

        // 0x92-0x96,  // OEM specific
        // 0x97-0x9F,  // Unassigned
        Lshift = 0xA0, // Left SHIFT key

        Rshift = 0xA1, // Right SHIFT key

        Lcontrol = 0xA2, // Left CONTROL key

        Rcontrol = 0xA3, // Right CONTROL key

        Lmenu = 0xA4, // Left MENU key

        Rmenu = 0xA5, // Right MENU key

        BrowserBack = 0xA6, // Windows 2000/XP: Browser Back key

        BrowserForward = 0xA7, // Windows 2000/XP: Browser Forward key

        BrowserRefresh = 0xA8, // Windows 2000/XP: Browser Refresh key

        BrowserStop = 0xA9, // Windows 2000/XP: Browser Stop key

        BrowserSearch = 0xAA, // Windows 2000/XP: Browser Search key

        BrowserFavorites = 0xAB, // Windows 2000/XP: Browser Favorites key

        BrowserHome = 0xAC, // Windows 2000/XP: Browser Start and Home key

        VolumeMute = 0xAD, // Windows 2000/XP: Volume Mute key

        VolumeDown = 0xAE, // Windows 2000/XP: Volume Down key

        VolumeUp = 0xAF, // Windows 2000/XP: Volume Up key

        MediaNextTrack = 0xB0, // Windows 2000/XP: Next Track key

        MediaPrevTrack = 0xB1, // Windows 2000/XP: Previous Track key

        MediaStop = 0xB2, // Windows 2000/XP: Stop Media key

        MediaPlayPause = 0xB3, // Windows 2000/XP: Play/Pause Media key

        LaunchMail = 0xB4, // Windows 2000/XP: Start Mail key

        LaunchMediaSelect = 0xB5, // Windows 2000/XP: Select Media key

        LaunchApp1 = 0xB6, // Windows 2000/XP: Start Application 1 key

        LaunchApp2 = 0xB7, // Windows 2000/XP: Start Application 2 key

        // 0xB8-0xB9,  // Reserved
        Oem1 = 0xBA, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the ';:' key
        OemPlus = 0xBB, // Windows 2000/XP: For any country/region, the '+' key

        OemComma = 0xBC, // Windows 2000/XP: For any country/region, the ',' key

        OemMinus = 0xBD, // Windows 2000/XP: For any country/region, the '-' key

        OemPeriod = 0xBE, // Windows 2000/XP: For any country/region, the '.' key

        Oem2 = 0xBF, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the '/?' key
        Oem3 = 0xC0, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the '`~' key
        // 0xC1-0xD7,  // Reserved
        // 0xD8-0xDA,  // Unassigned
        Oem4 = 0xDB, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the '[{' key
        Oem5 = 0xDC, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the '\|' key
        Oem6 = 0xDD, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the ']}' key
        Oem7 = 0xDE, // Used for miscellaneous characters; it can vary by keyboard.

        // Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key
        Oem8 = 0xDF, // Used for miscellaneous characters; it can vary by keyboard.

        // 0xE0,  // Reserved
        // 0xE1,  // OEM specific
        Oem102 = 0xE2, // Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard

        // 0xE3-E4,  // OEM specific
        Processkey = 0xE5, // Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key

        // 0xE6,  // OEM specific
        Packet = 0xE7, // Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP

        // 0xE8,  // Unassigned
        // 0xE9-F5,  // OEM specific
        Attn = 0xF6, // Attn key

        Crsel = 0xF7, // CrSel key

        Exsel = 0xF8, // ExSel key

        Ereof = 0xF9, // Erase EOF key

        Play = 0xFA, // Play key

        Zoom = 0xFB, // Zoom key

        Noname = 0xFC, // Reserved

        Pa1 = 0xFD, // PA1 key

        OemClear = 0xFE // Clear key
    }
}