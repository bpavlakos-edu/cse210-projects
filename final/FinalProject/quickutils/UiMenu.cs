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
        //Choice Menu Constructor
        public UiMenu(List<object> inputCollection, Action<object> lambdaToStoreReturn, List<string> displayStringList = null, bool haveExit = true, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="", string indentString = "", bool clearConsole = true)
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
            if(inputCollection.Count > 0 && haveExit)
            {
                _optionList.Add(new UiOption(new Action(()=>{throw new UiMenuExitException();}),"Go &Back"));
            }
            //Update remaining attributes
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;//Hide the exit message by default
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
        public void UiLoop(Action preLoopAction = null, bool debugMode = false)
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

                    }
                    
                    if(_clearConsole) //Added a flag to control if the console is cleared or not
                    {
                        Console.Clear(); //Reset the console before printing
                    }
                }
            }
            //The title of: https://stackoverflow.com/questions/10226314/what-is-the-best-way-to-catch-operation-cancelled-by-user-exception helped me find this exception type using intellisense
            catch (UiMenuExitException) //Exiting this menu
            {
                if(_exitMsg != "")
                { 
                    Console.WriteLine(_exitMsg); //Display non-empty exit messages
                }
                //Do nothing, just return
            }
            catch (OperationCanceledException) //Exiting this menu with legacy code (Will be removed eventually)
            {
                if(_exitMsg != "")
                { 
                    Console.WriteLine(_exitMsg); //Display non-empty exit messages
                }
            }
        }

        //Display Menu Options
        private void DisplayOptions()
        {
            //Display the UI options
            if(_clearConsole) //Added a flag to control if the console is cleared or not
            {
                Console.Clear();
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
            if(hotkeyIdx != 0)
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
                    return i;
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
        public void AddOption(UiOption newOption, int index = -1)
        {
            if(index < 0 || index > _optionList.Count)
            {
                _optionList.Add(newOption);
            }
            else
            {
                _optionList.Insert(index, newOption);
            }
        }
        public void AddOptionFromEnd(UiOption newOption, int index = 0)
        {
            AddOption(newOption, (_optionList.Count - 1) - index);
        }
        public void validateHotkeys()
        {
            List<string> hotkeyList = new List<string>();
            for(int i = 0; i<_optionList.Count;i++)
            {
                string curHotkey = _optionList[i].GetHotkey();
                if(hotkeyList.Contains(curHotkey))
                {
                    Console.WriteLine($"Duplicate hotkey detected! {curHotkey}");
                    //break; //Continue to report duplicate hotkeys
                }
                else
                {
                    hotkeyList.Add(curHotkey);
                }
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
}
