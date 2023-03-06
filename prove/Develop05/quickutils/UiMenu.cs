namespace QuickUtils
{
    //This was taken from the project Develop04, and modified to have a customizable menu message, input message, and exit message
    //UI Menu class, displays a list of options, and automatically processes the user's input
    class UiMenu
    {

        //Attributes
        private List<UiOption> _optionList = new List<UiOption>(); //Store the UiOption class
        private string _menuMsg = "Menu Options:";
        private string _inputMsg = "Select a choice or [hotkey] from the menu: ";
        private string _exitMsg = "Now exiting...";

        //Constructors
        //Blank constructor
        public UiMenu(List<object> stringOptions)
        {

        }
        //Fill all attributes
        public UiMenu(List<UiOption> optionList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg = "Now exiting...")
        {
            _optionList = optionList.ToList<UiOption>();
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;
        }

        //Use 3 lists to generate the Option List (For legacy code)
        public UiMenu(List<Action> uiActionList, List<string> optionNameList, List<string> hotkeyList, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="Now exiting...")
        {
            _optionList = new List<UiOption>();//Clear the UI Option list
            for(int i = 0; i<uiActionList.Count; i++)
            {
                _optionList.Add(new UiOption(uiActionList[i],optionNameList[i],hotkeyList[i]));
            }
            _menuMsg = menuMsg;
            _inputMsg = inputMsg;
            _exitMsg = exitMsg;//Update the exit message
        }

        //Generate a menu from a list of any data type, and return it's value when chosen
        //WIP 
        //Needs tested and needs to customize the back message
        //Also needs to accept the other message parameters too!
        /*
        Accepts:
        A list of any data type
        A lambda function, preferably one with a return value stored inside of it
        */
        public UiMenu(List<object> inputCollection, Action<object> lambdaToStoreReturn, bool haveExit = true, string menuMsg = "Menu Options:", string inputMsg = "Select a choice or [hotkey] from the menu: ", string exitMsg="Now exiting...")
        {
            for(int i=0; i < inputCollection.Count; i++)
            {
                int captureIdx = i; //Capture the index outside the lambda, so it doesn't get overwritten by the next loop cycle
                _optionList.Add(
                    new UiOption(
                    //Lambda function to call the function passed to this constructor, using the item from the list as it's parameter
                    new Action(()=>
                    {
                        lambdaToStoreReturn(inputCollection[captureIdx]); //Store the value using the lambda function inserted as a parameter
                        throw new OperationCanceledException(); //Exit the menu upon completion of this Menu option
                    }),
                    inputCollection[i]+"","")
                );
            }
            //Decide if this should be an option or not
            if(inputCollection.Count > 0 && haveExit)
            {
                _optionList.Add(new UiOption(new Action(()=>{throw new OperationCanceledException();}),"[C]ancel","c"));
            }
            _exitMsg = "";//Hide the exit message
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

        //Public and private Methods
        public void UiLoop()
        {
            try
            {
                while(true)
                {
                    Console.Clear(); //Reset the console before printing
                    displayOptions();
                    string userInput = GetInput(_inputMsg).ToLower();
                    if(!TryParseIndex(userInput)) //If parsing the index fails
                    {
                        FindHotkey(userInput); //Try using a hotkey instead
                    }
                    Console.Clear(); //Reset the console before printing
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
            Console.Clear();
            Console.WriteLine(_menuMsg);
            for(int i = 0; i < _optionList.Count; i++)
            {
                Console.WriteLine($"{i+1}. {_optionList[i].GetName()}");
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

        //Attempt to find the hotkey and activate it
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
            Console.Clear();//So we can clear the console before we...
            optionObj.Activate();//Activate!
        }
        private string GetInput(string inMsg)
        {
            return QuickUtils.Inputs.GetInput(inMsg);
        }
        private int GetIntInput(string inMsg)
        {
            return QuickUtils.Inputs.GetIntInput(inMsg);
        }
    }
}
