namespace QuickUtils
{
    using System.Reflection;
    /*
    Changes in version 2 (Develop05 Version):
        - Added a constructor that supports an input collection, and a lambda designed to store the result in a captured return value (Settings Menu Constructor)
        - Added an option parameter to UiLoop, PreLoopFunction, that allows an additional message to be printed before the Ui runs
        - Outside this class, auto-hotkey support was added to UiOption, allowing hotkeys to be auto generated in CnC Generals style using the index of & this can be used in the UiOption constructor
    Changes in Version 3 (Sandbox Update Version):
        - Added proper nullable type support after finding out C# uses "?" to enable null
        - Created a settings menu constructor
        Settings Menu Constructor:
        - Fixed a typo where it was checking the null status of "displayString" instead of "displayStrings"
        - Changed "displayStrings" to "displayStringList" to prevent confusion
        

    */

    //UI Menu class, displays a list of options, and automatically processes the user's input
    class UiMenu
    {

        //Attributes
        private List<UiOption> _optionList = new List<UiOption>(); //Store the UiOption class
        private string _indentString = "";
        private string _menuMsg = "Menu Options:";
        private string _inputMsg = "Select a choice or [hotkey] from the menu: ";
        private string _exitMsg = "Now exiting...";

        private bool _clearConsole = true;

        //Constructors
        //Blank constructor
        public UiMenu()
        {

        }
        //Fill all attributes
        public UiMenu(List<UiOption> optionList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg = "Now exiting...", string indentString = "", bool clearConsole = true)
        {
            //Update all attributes
            _optionList = optionList.ToList<UiOption>();
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;
            _indentString = indentString;
            _clearConsole = clearConsole;
        }

        //Use 3 lists to generate the Option List (For legacy code) 
        public UiMenu(List<Action> uiActionList, List<string> optionNameList, List<string> hotkeyList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="Now exiting...", string indentString = "", bool clearConsole = true)
        {
            _optionList = new List<UiOption>();//Clear the UI Option list
            for(int i = 0; i<uiActionList.Count; i++)
            {
                _optionList.Add(new UiOption(uiActionList[i],optionNameList[i],hotkeyList[i]));
            }
            //Update remaining attributes
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;//Update the exit message
            _indentString = indentString;
            _clearConsole = clearConsole;
        }

        //Generate a menu from a list of any data type, and return it's value when chosen
        //WIP 
        //Needs tested and needs to customize the back message
        //It needs to have the ability to adjust the option strings too, if only we could add a template
        /*
        Accepts:
        A list of any data type
        A lambda function, preferably one with a variable stored inside of it to set to the return value, and accepts a parameter being the same data type as the list of objects
        Example:
        List<object> stringOptions = new List<object>{"A","B","C","D"}; //Create a list of objects that have the value we want to pick from
        string resultVar = ""; //Make a variable to return
        UiMenu listTestMenu = new UiMenu(stringOptions,new Action<object>((inStr)=>{resultVar = (string)inStr;}));
        listTestMenu.UiLoop();

        This method uses composite formatting to generate the option string: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#composite-formatting
        */
        //Choice Menu Constructor (essentially a user input function)
        public UiMenu(List<object> inputCollection, Action<object> lambdaToStoreReturn, List<string> displayStringList = null, string exitOptionStr = "Go &Back", string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="", string indentString = "", bool clearConsole = true)
        {
            for(int i = 0; i < inputCollection.Count; i++)
            {
                int captureIdx = i; //Capture the index outside the lambda, so it doesn't get overwritten by the next loop cycle
                //Make the option display string if the displayString list isn't empty
                string displayString = i+"";
                if(displayStringList != null) //[Fixed typo in version 3]
                {
                    displayString = displayStringList[i]; //Use the entry from the display strings parameter
                }
                //Every item in the input list needs to be assigned to a UiOption
                _optionList.Add(
                    new UiOption(
                    //Lambda function to call the function passed to this constructor, using the item from the list as it's parameter
                    new Action(() =>
                    {
                        lambdaToStoreReturn(inputCollection[captureIdx]); //Store the value using the lambda function inserted as a parameter
                        throw new UiMenuExitException(); //Exit the menu upon completion of this Menu option
                    }),
                    displayString)
                );
            }
            //Add a cancel option
            if(inputCollection.Count > 0 && exitOptionStr != "")
            {
                _optionList.Add(new UiOption(new Action(()=>{throw new UiMenuExitException();}),exitOptionStr));
            }
            //Update remaining attributes
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;//Hide the exit message by default
            _indentString = indentString;
            _clearConsole = clearConsole;
        }

        //Class-List Method Invoking Menu Auto-Constructor
        //This constructor takes an input of a list of classes and invokes a method on them by capturing the method in a lambda
        //This lets us set each MenuOption to invoke a method of a class on each corresponding item from the Class Collection
        //There's also a itemDisplayFunction, which lets us iterate over each object and generate a string using them, or using a function they have by calling it in the lambda function
        //Example:
        /*
        UiMenu myMinorClassListMenu = new UiMenu(
            minorClassList.ToList<object>(),
            (inputMinorClass) => {((inputMinorClass)inputMinorClass).MyMethod();},
            "MyClass as String $",
            (inputMinorClass)=>{((inputMinorClass)inputMinorClass).ToDisplayString();}
        );
        */
        public UiMenu(List<object> classCollection, Action<object> classActionMethod, string sharedDisplayString, Func<object, string> classStringMethod, string exitOptionStr="Go &Back", string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="", string indentString = "", bool clearConsole = true)
        {
            for(int i = 0 ; i < classCollection.Count; i++)
            {
                int index = i; //Make sure the current index is stored in the lambda, not the memory address of i
                _optionList.Add(new UiOption(
                    ()=>{classActionMethod(classCollection[index]);}, //This captures the current list item as the input of the action that will run the desired method in the class
                    sharedDisplayString.Replace("$",(index+1)+"")+": ", //This string will be the same for each item in the item list, the rest will be generated using the updateStrFun //Replace $ with the index if present
                    ()=>{return classStringMethod(classCollection[index]);} //Capture the current item as the input of the classStringMethod, put that inside a lambda which will be the "updateStrFun" the UiOption uses to update this item
                ));
            }
            //Add a cancel option
            if(classCollection.Count > 0 && exitOptionStr != "")
            {
                _optionList.Add(new UiOption(new Action(()=>{throw new UiMenuExitException();}), exitOptionStr));
            }
            //Update remaining attributes
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;
            _indentString = indentString;
            _clearConsole = clearConsole;
        }

        //Input Menu Constructor
        /*public UiMenu(object inputObject, List<string> fieldNameStrings = null) //Add optional parameters
        {
            List<Action> actionList = new List<Action>(); //Create a list to store the actions in
            //Find all setters, getters, and fields
            Type objType = inputObject.GetType();
            FieldInfo[] objFields = objType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public); //Including private fields
            MethodInfo[] objMethods = objType.GetMethods(); //Only get public methods
            //objType.GetConst
            
            //Auto generate strings for each option
            //Add the back button
        }*/

        //Getters and setters (Please don't modify the menu at runtime! That's crazy!!!)
        public List<UiOption> GetOptionList()
        {
            return _optionList.ToList<UiOption>();
        }
        public void SetOptionList(List<UiOption> optionList)
        {
            _optionList = optionList.ToList<UiOption>();
        }
        public string GetMenuMsg()
        {
            return _menuMsg;
        }
        public void SetMenuMsg(string menuMsg)
        {
            _menuMsg = menuMsg;
        }
        public string GetInputMsg()
        {
            return _inputMsg;
        }
        public void SetInputMsg(string inputMsg)
        {
            _inputMsg = inputMsg;
        }
        public string GetExitMsg()
        {
            return _exitMsg;
        }
        public void SetExitMsg(string exitMsg)
        {
            _exitMsg = exitMsg;
        }
        public string GetIndentString()
        {
            return _indentString;
        }
        public void SetIndentString(string indentString)
        {
            _indentString = indentString;
        }
        public bool GetClearConsole()
        {
            return _clearConsole;
        }
        public void SetClearConsole(bool clearConsole)
        {
            _clearConsole = clearConsole;
        }
        

        //Public and private Methods
        //Issue with compile time constants, which prevents you from setting a default value of a class: https://stackoverflow.com/questions/18740421/default-parameter-for-value-must-be-a-compile-time-constant
        //It seems the only viable solution is to use "null" as suggested by this first comment to this: https://stackoverflow.com/a/18740471
        //I've already been using null, but I really wanted to know if there's a better way to use a default value, other than a manual function overload
        public bool UiLoop(Action preLoopAction = null, bool debugMode = false)
        {
            try
            {
                while(true)
                {
                    if(_clearConsole) //Added a flag to control if the console is cleared or not
                    {
                        Console.Clear(); //Reset the console before printing
                    }
                    if(debugMode) //Added a flag to check if there are duplicate hotkeys in the list
                    {
                        validateHotkeys();
                    }
                    if(preLoopAction != null)
                    {
                        preLoopAction.Invoke(); //Invoke any pre loop action, unless it's not set
                    }
                    DisplayOptions(); //Display the options
                    string userInput = Inputs.GetInput(_inputMsg); //Get the user input
                    try //Lets us catch the UiMenuRemoveException
                    {
                        if(!TryParseIndex(userInput)) //If parsing the index fails
                        {
                            ActivateHotkey(userInput); //Try using a hotkey instead
                        }
                    }
                    catch (UiMenuRemoveException) //Remove this option from the list
                    {
                        //Needs a way to track which input was triggered, likely by going through the same activation sequence
                        //_optionList.RemoveAt();
                    }
                    if(_clearConsole) //Added a flag to control if the console is cleared or not
                    {
                        Console.Clear(); //Reset the console before printing
                    }
                }
            }
            //Exit Sqeuences
            catch (UiMenuExitException) //Exiting this menu
            {
                if(_exitMsg != "")
                { 
                    Console.WriteLine(_exitMsg); //Display non-empty exit messages
                }
                return false; //Do nothing, just return (false means exit, not necessary for UiMenus that don't need to update)
            }
            catch (UiMenuRefreshException) // Exiting this menu while requesting a refresh
            {
                if(_clearConsole)
                {
                    Console.Clear();
                }
                return true; //Tell the source function to re-generate the UiMenu
            }
            //Legacy code
            //The title of: https://stackoverflow.com/questions/10226314/what-is-the-best-way-to-catch-operation-cancelled-by-user-exception helped me find this exception type using intellisense
            catch (OperationCanceledException) //Exiting this menu with legacy code (Will be removed eventually)
            {
                if(_exitMsg != "")
                { 
                    Console.WriteLine(_exitMsg); //Display non-empty exit messages
                }
                return false; //(false means exit, not necessary for UiMenus that don't need to update)
            }
        }

        //Display Menu Options
        private void DisplayOptions()
        {
            //Display the UI options
            if(_clearConsole) //Added a flag to control if the console is cleared or not
            {
                //Console.Clear(); //Displaying options shouldn't refresh the list, it's already being refreshed before this
            }
            Console.WriteLine(_menuMsg); //Write the menu message
            for(int i = 0; i < _optionList.Count; i++)
            {
                Console.WriteLine($"{_indentString}{i+1}. {_optionList[i].ToMenuStr()}");
            }
        }

        //Try to get a valid index in the action list from user input, return the success of this operation
        private bool TryParseIndex(string inputStr)
        {
            try
            {
                try
                {
                    //Valid index
                    int returnInt = int.Parse(inputStr) - 1; //Get the action index
                    ActivateOption(returnInt); //Activate the selected index if possible
                    return true; //Menu successfully activated
                }
                //Catch Statements have been condensed for readability, basically they just throw the "ArgumentNullException" so that the next code (checking for hotkeys) is run, regardless of the known exception
                catch (IndexOutOfRangeException) {throw new ArgumentNullException();} //Invalid index, treat it like text input (Comes from ActivateOption())
                catch (ArgumentOutOfRangeException) {throw new ArgumentNullException();} //Invalid index, apparently when an IndexError is thrown from an attirbute, it's a different error...
                catch (OverflowException){throw new ArgumentNullException();} //Integer overflow, treat it like text input
                catch (FormatException){throw new ArgumentNullException();} //Not a number, treat it like text input
            }
            catch(ArgumentNullException) //Funnel every error into ArgumentNullException
            {
                return false; //Menu didn't activate
            }
        }

        //Attempt to find the hotkey in our UiOptions and activate it
        private void ActivateHotkey(string hotkey)
        {
            int hotkeyIdx = FindHotkeyIdx(hotkey);
            if(hotkeyIdx >= 0)
            {
                ActivateOption(hotkeyIdx);
            }
        }
        //Find the hotkey
        private int FindHotkeyIdx(string hotkey)
        {
            for(int i = 0; i < _optionList.Count; i++)
            {
                if(_optionList[i].CheckHotkey(hotkey))
                {
                    return i; //Return the found index
                }
            }
            return -1; //Return the faliure status
        }

        //Activate an option from the option list
        private void ActivateOption(int index)
        {
            UiOption optionObj = _optionList[index]; //Trigger any index errors here
            if(_clearConsole) //Added a global flag for clear console
            {
                Console.Clear();//So we can clear the console before we...
            }
            optionObj.Activate();//Activate!
        }

        //Add a new option to the option list
        public void AddOption(UiOption newOption, int? index = null)
        {
            if((index ?? _optionList.Count) >= _optionList.Count ) //Above list boundaries (When null it will always be above list boundaries)
            {
                _optionList.Add(newOption); //Add to the end
            }
            else if(index < 0) //Negative index error
            {
                _optionList.Insert(0,newOption); //Insert the option at the beginning
            }
            else //Within index
            {
                _optionList.Insert(index ?? 0, newOption); //Insert the option at the location specified as a parameter (?? was needed to convince the compiler that it can't be null here, even though it pyhsically can't be null here)
            }
        }
        public void AddOptionFromEnd(UiOption newOption, int index = 1)
        {
            AddOption(newOption, (_optionList.Count) - index);
        }
        //Add multiple options from the same index at the end (very useful for adding lots of options all at once)
        public void AddOptionFromEnd(List<UiOption> newOptionList, int index = 1)
        {
            foreach(UiOption newOption in newOptionList)
            {
                AddOption(newOption, (_optionList.Count) - index);
            }
        }
        //Remove an option from the list
        public void RemoveOption(int index)
        {
            if(index >= 0 || index < _optionList.Count) //Within list boundaries
            {
                _optionList.RemoveAt(index);
            }
            //Index Errors are ignored
        }
        public void RemoveOptionFromEnd(int index)
        {
            RemoveOption((_optionList.Count - 1) - index); //Remove from the last possible index minus the offset sent by the function call
        }
        //Check all hotkeys to make sure there's no duplicates (For debugging, because running this every loop is very laggy!)
        public void validateHotkeys()
        {
            List<string> hotkeyList = new List<string>();
            List<int> duplicatedIndexes = new List<int>(); //List to keep track of duplicates
            for(int i = 0; i<_optionList.Count;i++)
            {
                string curHotkey = _optionList[i].GetHotkey(); //Get the hotkey from this option
                if(hotkeyList.Contains(curHotkey) && curHotkey != "")
                {
                    Console.WriteLine($"Duplicate hotkey detected! {curHotkey} {i+1}");
                    duplicatedIndexes.Add(i);
                    //break; //Continue to report duplicate hotkeys
                }
                else if(curHotkey != "")
                {
                    hotkeyList.Add(curHotkey);
                }
            }
            //Suggest new hotkeys
            for(int i = 0; i < duplicatedIndexes.Count; i++)
            {
                SuggestNewHotkey(duplicatedIndexes[i], hotkeyList);
            }
        }

        //Accept an option index and a list of current hotkeys as string, then search for an unused hotkey
        private void SuggestNewHotkey(int optionIdx, List<string> curHotkeyList)
        {
            //I probably should change hotkey to a char... but it's too late now
            char[] displayStringChars = _optionList[optionIdx].GetDispStr().ToLower().Replace(" ","").Replace("&","").ToCharArray();
            List<string> suggestedCharList = new List<string>();
            for(int i = 0; i < displayStringChars.Length; i++)
            {
                string scannedChar = displayStringChars[i]+"";
                if(!curHotkeyList.Contains(scannedChar) && !suggestedCharList.Contains(scannedChar) && char.IsAsciiLetter(scannedChar[0])) //Current hotkey list doesn't have it, it wasn't already scanned, and it's a letter
                {
                    suggestedCharList.Add(scannedChar); //Add it to the suggestion list
                }
            }
            if(suggestedCharList.Count > 0) //Print all suggestions
            {
                Console.WriteLine($"Option: {optionIdx} \"{_optionList[optionIdx].GetDispStr()}\" suggested hotkey alternatives: {string.Join(',',suggestedCharList)}");
            }
            else //Alert the user there are no suggestions
            {
                Console.WriteLine($"Cannot find a new hotkey! Option: {optionIdx} \"{_optionList[optionIdx].GetDispStr()}\"");
            }
        }
    }

    //Custom menu exceptions
    //VSCode had a template for this, which made creating them much easier, but I knew beforehand that it worked by inheriting an already existing exception
    //Exit Exception
    [System.Serializable]
    public class UiMenuExitException : System.Exception
    {
        public UiMenuExitException() { } //Default constructor
        public UiMenuExitException(string message) : base(message) { } //Constructor with a message
        public UiMenuExitException(string message, System.Exception inner) : base(message, inner) { } //Constructor with a message and another exception inside???
        protected UiMenuExitException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }//Unknown constructor, perhaps it has to do with runtime location? Or is this the one that Catch statements use?
    }

    //Remove Exception, removes the current UiOption from the list
    [System.Serializable]
    public class UiMenuRemoveException : System.Exception
    {
        public UiMenuRemoveException() { }
        public UiMenuRemoveException(string message) : base(message) { }
        public UiMenuRemoveException(string message, System.Exception inner) : base(message, inner) { }
        protected UiMenuRemoveException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    //Refresh Menu Exception, for telling the source function that this UiMenu needs to refresh
    [System.Serializable]
    public class UiMenuRefreshException : System.Exception
    {
        public UiMenuRefreshException() { }
        public UiMenuRefreshException(string message) : base(message) { }
        public UiMenuRefreshException(string message, System.Exception inner) : base(message, inner) { }
        protected UiMenuRefreshException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
