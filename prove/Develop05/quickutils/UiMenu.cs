namespace QuickUtils
{
    //This was taken from the project Develop04, and modified to have a customizable menu message, input message, and exit message
    //UI Menu class, displays a list of options, and automatically processes the user's input
    class UiMenu
    {

        //Attributes
        private List<UiOption> _optionList = new List<UiOption>(); //Store the UiOption class
        private string _indentString = "";
        private string _menuMsg = "Menu Options:";
        private string _inputMsg = "Select a choice or [hotkey] from the menu: ";
        private string _exitMsg = "Now exiting...";

        private bool _clearConsole = false;

        //Constructors
        //Blank constructor
        public UiMenu()
        {

        }
        //Fill all attributes
        public UiMenu(List<UiOption> optionList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg = "Now exiting...", string indentString = "", bool clearConsole = false)
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
        public UiMenu(List<Action> uiActionList, List<string> optionNameList, List<string> hotkeyList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="Now exiting...", string indentString = "", bool clearConsole = false)
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
        public UiMenu(List<object> inputCollection, Action<object> lambdaToStoreReturn, List<string> displayStrings = null, bool haveExit = true, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="", string indentString = "", bool clearConsole = false)
        {
            for(int i=0; i < inputCollection.Count; i++)
            {
                int captureIdx = i; //Capture the index outside the lambda, so it doesn't get overwritten by the next loop cycle
                //Make the option display string if the displayString list isn't empty
                string displayString = i+"";
                if(displayString != null)
                {
                    try
                    {
                        displayString = displayStrings[i]; //Use the entry from the display strings parameter
                    }
                    catch(IndexOutOfRangeException)
                    {
                        //Don't change 
                    } 
                }
                //Every item in the input list needs to be assigned to a UiOption
                _optionList.Add(
                    new UiOption(
                    //Lambda function to call the function passed to this constructor, using the item from the list as it's parameter
                    new Action(()=>
                    {
                        lambdaToStoreReturn(inputCollection[captureIdx]); //Store the value using the lambda function inserted as a parameter
                        throw new OperationCanceledException(); //Exit the menu upon completion of this Menu option
                    }),
                    displayString)
                );
            }
            //Add a cancel option
            if(inputCollection.Count > 0 && haveExit)
            {
                _optionList.Add(new UiOption(new Action(()=>{throw new OperationCanceledException();}),"Go &Back"));
            }
            //Update remaining attributes
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;//Hide the exit message by default
            _indentString = indentString;
            _clearConsole = clearConsole;
        }

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
                    displayOptions(); //Display the options
                    string userInput = QuickUtils.Inputs.GetInput(_inputMsg); //Get the user input
                    if(!TryParseIndex(userInput)) //If parsing the index fails
                    {
                        FindHotkey(userInput); //Try using a hotkey instead
                    }
                    if(_clearConsole) //Added a flag to control if the console is cleared or not
                    {
                        Console.Clear(); //Reset the console before printing
                    }
                }
            }
            //The title of: https://stackoverflow.com/questions/10226314/what-is-the-best-way-to-catch-operation-cancelled-by-user-exception helped me find this exception type using intellisense
            catch (OperationCanceledException) //Exiting this menu
            {
                if(_exitMsg != "")
                { 
                    Console.WriteLine(_exitMsg); //Display non-empty exit messages
                }
                //Do nothing, just return
            }
        }

        //Display Menu Options
        private void displayOptions()
        {
            //Display the UI options
            if(_clearConsole) //Added a flag to control if the console is cleared or not
            {
                Console.Clear();
            }
            Console.WriteLine(_menuMsg); //Write the menu message
            for(int i = 0; i < _optionList.Count; i++)
            {
                Console.WriteLine($"{_indentString}{i+1}. {_optionList[i].GetName()}");
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
        private void FindHotkey(string hotkey)
        {
            for(int i = 0; i < _optionList.Count; i++)
            {
                if(_optionList[i].CheckHotkey(hotkey))
                {
                    ActivateOption(i);//Activate this option
                    break;
                }
            }
        }

        //Activate an option from the option list
        private void ActivateOption(int index)
        {
            UiOption optionObj = _optionList[index]; //Trigger any index errors here
            if(_clearConsole) //Added an attribute
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
}
