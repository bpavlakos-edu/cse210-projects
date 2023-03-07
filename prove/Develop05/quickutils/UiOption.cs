namespace QuickUtils
{
    //This was taken from Develop04, and modified to generate hotkeys with input text that has "&" in it
    //The option in the UI list, triggers the run action and can query a hotkey
    class UiOption
    {
        //Attributes
        private Action _runAction; //Action to run
        private String _name; //Display Name
        private String _hotkey; //Hotkey for activation

        //Constructors
        //Empty Constructor
        public UiOption()
        {
            _runAction = new Action(()=>{});
            _name = "";
            _hotkey = "";
        }
        //Fill all attributes
        public UiOption(Action runAction, string name, string hotkey)
        {
            _runAction = runAction;
            _name = name;
            _hotkey = hotkey;
        }
        //Fill using a the "Command and Conquer Generals" style hotkey system
        public UiOption(Action runAction, string hotkeyName)
        {
            _runAction = runAction;
            _name = MakeDisplayName(hotkeyName);
            _hotkey = MakeHotkey(hotkeyName);
        }

        //Getters and setters (Please don't modify the menu at runtime! That's crazy!!!)
        public Action GetAction()
        {
            return _runAction;
        }
        public void SetAction(Action runAction)
        {
            _runAction = runAction;
        }
        public string GetName()
        {
            return _name;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public string GetHotkey()
        {
            return _hotkey;
        }
        public void SetHotkey(string hotkey)
        {
            _hotkey = hotkey;
        }
        //Methods
        //Invoke our action
        public void Activate()
        {
            _runAction.Invoke();//Activate the function stored in action
        }
        //Return if the string matches our hotkey
        public bool CheckHotkey(string hotkey)
        {
            if(hotkey != "")
            {
                return hotkey == _hotkey;
            }
            else //Always return false when the hotkey is ""
            {
                return false;
            }
        }

        //The following methods were created for the original version of the UiMenu called "UiLink" where UiMenu and UiOption were one singular class

        //The "Command and Conquer Generals" style hotkey system: "&" precedes the letter that will be the hotkey
        //Get a display string using the name string
        public string MakeDisplayName(string hotkeyName)
        {
            //TODO***: Actually implement caps handling, caps on first letter, and hotkey, and substring
            string returnString = hotkeyName; //Store the original string //Removed ToLower
            int hotkeyIndex = FindHotkeyIndex(hotkeyName.ToLower()); //Added toLower here so it functions correctly
            if(hotkeyIndex > 0)
            {
                string beforeHotkey = returnString.Substring(0,hotkeyIndex) ?? ""; //Get everything before the hotkey if possible
                string hotkeyString = ("["+returnString[hotkeyIndex]+"]"); //Change the hotkey character to upper case //Removed .ToUpper() so that words aren't auto capitalized
                string afterHotkey = returnString.Substring(hotkeyIndex + 1, (returnString.Length - (hotkeyIndex + 1))) ?? ""; //Add the remainder of the string if possible
                return (beforeHotkey+hotkeyString+afterHotkey).Replace("&","");//Remove & and return
                //return (returnString.Substring(0,hotkeyIndex)+"["+returnString[hotkeyIndex].ToString().ToUpper()+"]"+returnString.Substring(hotkeyIndex+1,returnString.Length-hotkeyIndex)).Replace("&","");
            }
            return returnString;
        }
        //Return the hotkey as a string, from a hotkeyName string (a name string with a & in it)
        private string MakeHotkey(string hotkeyName)
        {
            //Use C&C Hotkey System
            int hotkeyIndex = FindHotkeyIndex(hotkeyName);
            if(hotkeyIndex > 0)
            {
                return hotkeyName.Substring(hotkeyIndex, 1).ToLower();//Return the hotkey using the index
            }
            //Ignore last index and non existant index
            return ""; //Return nothing
        }

        //Find the index of the hotkey by finding the index of "&" + 1, and determining if it's not 0 or string.length, because they will be ignored
        private int FindHotkeyIndex(string hotkeyName)
        {
            int hotkeyIndex = hotkeyName.IndexOf("&"); //This is how the "Command and conquer generals" style hotkeys work, & precedes the hotkey letter
            if(hotkeyIndex < 0 || hotkeyIndex >= hotkeyName.Length - 1) //Ignore failed searches and & at the last index
            {
                return -1; //-1 will always be ignored
            }
            else if(hotkeyName.Substring((hotkeyIndex + 1), 1) == " ")
            {
                return -1; //-1 will always be ignored
            }
            return hotkeyIndex + 1; //Return + 1 because the letter after is the hotkey
            //Because of the conditions we created it will never be an index of 0 or > length of the string - 1 (the last index)
        }
    }
}
