namespace QuickUtils
{
    //This was taken from Develop04, and modified to generate hotkeys with input text that has "&" in it
    //The option in the UI list, triggers the run action and can query a hotkey
    //Changes in version 3:
    //- Changed String _name and String _hotkey to string data type
    //- Changed hotkey constructor to reference original constructor which required changing MakeDisplayName, MakeHotkey, and FindHotkeyIndex to static

    class UiOption
    {
        //Attributes
        private Action _runAction; //Action to run
        private string _dispStr; //Display Name
        private string _hotkey; //Hotkey for activation
        private Func<string>? _updateStrFun; //Optional string updating function

        //Constructors
        //Empty Constructor
        public UiOption()
        {
            _runAction = ()=>{};
            _dispStr = "";
            _hotkey = "";
            _updateStrFun = null;
        }
        //Fill all attributes
        public UiOption(Action runAction, string name, string hotkey, Func<string>? updateStrFun = null)
        {
            _runAction = runAction;
            _dispStr = name;
            _hotkey = hotkey;
            _updateStrFun = updateStrFun;
        }
        //Fill using a the "Command and Conquer Generals" style hotkey system
        public UiOption(Action runAction, string hotkeyName, Func<string>? updateStrFun = null) : this(runAction, MakeDisplayName(hotkeyName), MakeHotkey(hotkeyName), updateStrFun)
        {

        }
        //Fill using a Getter, Setter and a hotkey string (For the settingsMenu constructor), I really wish these could all be 1 shared constructor, but the typing on getterFun and setterFun prevents that
        public UiOption(Func<string> getterFun, Action<string> setterFun, string hotkeyVarName, bool newLine = false, bool toLower = false) //ToLower is false by default here
        {
            //This is quite complex so I'll do it in the constructor body
            _runAction = () => {setterFun(Inputs.GetInput($"What would you like to change {hotkeyVarName.Replace("&","").ToLower()} to?: ", toLower, newLine, getterFun())); /*throw new OperationCanceledException();*/};
            _dispStr = MakeDisplayName("Change "+hotkeyVarName);
            _hotkey = MakeHotkey(hotkeyVarName);
            _updateStrFun = () => {return " (Currently: "+Misc.QuoteStr(getterFun())+")";};
        }
        //Integer
        public UiOption(Func<int> getterFun, Action<int> setterFun, string hotkeyVarName, int? min = null, int? max = null, bool newLine = false)
        {
            //This is quite complex so I'll do it in the constructor body
            _runAction = () => {setterFun(Inputs.GetIntInput($"What would you like to change {hotkeyVarName.Replace("&","").ToLower()} to?: ", min, max, newLine, getterFun())); /*throw new OperationCanceledException();*/};
            _dispStr = MakeDisplayName("Change "+hotkeyVarName);
            _hotkey = MakeHotkey(hotkeyVarName);
            _updateStrFun = () => {return " (Currently: "+getterFun()+")";};
        }
        //Float
        public UiOption(Func<float> getterFun, Action<float> setterFun, string hotkeyVarName, float? min = null, float? max = null, bool newLine = false)
        {
            //This is quite complex so I'll do it in the constructor body
            _runAction = () => {setterFun(Inputs.GetFloatInput($"What would you like to change {hotkeyVarName.Replace("&","").ToLower()} to?: ", min, max, newLine, getterFun())); /*throw new OperationCanceledException();*/};
            _dispStr = MakeDisplayName("Change "+hotkeyVarName);
            _hotkey = MakeHotkey(hotkeyVarName);
            _updateStrFun = () => {return " (Currently: "+getterFun()+")";}; //Consider using Misc.RoundFStr()
        }
        //Boolean
        public UiOption(Func<bool> getterFun, Action<bool> setterFun, string hotkeyVarName, char yesChar='y', char noChar='n', bool newLine = false)
        {
            //This is quite complex so I'll do it in the constructor body
            _runAction = () => {setterFun(Inputs.GetBoolInput($"What would you like to change {hotkeyVarName.Replace("&","").ToLower()} to?:", yesChar, noChar, newLine, getterFun())); /*throw new OperationCanceledException();*/};
            _dispStr = MakeDisplayName("Change "+hotkeyVarName);
            _hotkey = MakeHotkey(hotkeyVarName);
            _updateStrFun = () => {return " (Currently: "+Misc.BoolStr(getterFun())+")";};
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
        public string GetDispStr()
        {
            return _dispStr;
        }
        public void SetDispStr(string dispStr)
        {
            _dispStr = dispStr;
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
            return (hotkey != "") ? hotkey == _hotkey : false; //Always return false when the hotkey is ""
        }
        //Generate the automatically updated display string, use the update display string function to update the value for settings
        public string ToMenuStr()
        {
            return (_updateStrFun == null) ? _dispStr : _dispStr + _updateStrFun(); //Return either the string as normal if the update function is empty, or use the string update function to update the string
        }

        //The following methods were created for the original version of the UiMenu called "UiLink" where UiMenu and UiOption were one singular class
        //The "Command and Conquer Generals" style hotkey system: "&" precedes the letter that will be the hotkey
        //Get a display string using the name string
        public static string MakeDisplayName(string hotkeyName)
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
        private static string MakeHotkey(string hotkeyName)
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
        private static int FindHotkeyIndex(string hotkeyName)
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
