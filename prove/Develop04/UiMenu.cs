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
        Console.Write(inMsg);
        return Console.ReadLine();
        //Todo: Add a thread based timeout!
        //Start Timer Thread
        //Start console read Thread
        //Timer.Join(Msec)
        //If ConsoleRead.Join(1);
        //If Console Read times out, throw an error or return null
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