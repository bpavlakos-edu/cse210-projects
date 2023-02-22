//UI Menu class, displays a list of options, and automatically processes the user's input
class UiMenu
{

    //Attributes
    private List<UiOption> _optionList; //Store the UiOption class
    private string _exitMsg = "Now exiting...";

    //Constructors
    //Blank constructor
    public UiMenu()
    {
        
    }
    //Fill all attributes
    public UiMenu(List<UiOption> optionList, string exitMsg = "Now exiting...")
    {
        _optionList = optionList.ToList<UiOption>();
        _exitMsg = exitMsg;
    }

    //Use 3 lists to generate the Option List (For legacy code)
    public UiMenu(List<Action> uiActionList, List<string> optionNameList, List<string> hotkeyList, string exitMsg="Now exiting...")
    {
        _optionList = new List<UiOption>();//Clear the UI Option list
        for(int i = 0; i<uiActionList.Count; i++)
        {
            _optionList.Add(new UiOption(uiActionList[i],optionNameList[i],hotkeyList[i]));
        }
        _exitMsg = exitMsg;//Update the exit message
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
                string userInput = GetInput("Select a choice or [hotkey] from the menu: ").ToLower();
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
        Console.WriteLine("Menu Options:");
        for(int i = 0; i < _optionList.Count; i++)
        {
            Console.WriteLine($"{i+1}. {_optionList[i].GetName()}");
        }
    }

    //Try to get a valid index, return the success of this operation

    private bool TryParseIndex(string inputStr)
    {
        try
        {
            try
            {
                //Valid index
                int returnInt = int.Parse(inputStr) - 1; //Get the action index
                ActivateOption(returnInt); //Activate the selected index if possible
                return true;
            }
            catch (IndexOutOfRangeException) //Invalid index
            {
                throw new ArgumentNullException(); //Treat it like text input
            }
            catch (OverflowException) //Integer overflow
            {
                throw new ArgumentNullException(); //Treat it like text input
            }
            catch (FormatException) //Not a number
            {
                throw new ArgumentNullException(); //Treat it like text input
            }
        }
        catch(ArgumentNullException) //Funnel every error into ArgumentNullException
        {
            return false;
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
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    private int GetIntInput(string inMsg, int min = 0, int max = 0)
    {
        while(true) //Repeat until a valid number is found
        {
            //Catch parsing errors
            try 
            {
                int returnInt = int.Parse(GetInput(inMsg)); //Parse the user input
                //Determine if the current integer is a valid number
                if(min == max) //This means no minimum or maximum was set
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else if(returnInt <= max && returnInt >= min) //is the number between the minimum and maximum?
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else //Invalid number
                {
                    Console.WriteLine($"That's not a number between {min} and {max}, please try again!");
                }
            }
            catch(FormatException) //Not a number
            {
                Console.WriteLine($"That's not a valid whole number, please try again!");
            }
            catch(ArgumentNullException) //Empty input
            {
                Console.WriteLine("Please enter a number to continue!");
            }
            catch(OverflowException) //Overflow
            {
                Console.WriteLine("That's not a number the program can process, please try again!");
            }
        }
    }
}

//The option in the UI list, triggers the run action and can query a hotkey
class UiOption
{
    //Attributes
    private Action _runAction;
    private String _name;
    private String _hotkey;

    //Constructors
    //Empty Constructor
    public UiOption()
    {

    }
    //Fill all attributes
    public UiOption(Action runAction, string name, string hotkey)
    {
        _runAction = runAction;
        _name = name;
        _hotkey = hotkey;
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
        return hotkey == _hotkey;
    }
}