class UiMenu
{

    //Attributes
    private List<UiOption> _optionList;
    private string _exitMsg = "Now exiting...";

    //Constructors
    public UiMenu(List<UiOption> optionList, string exitMsg = "Now exiting...")
    {
        _optionList = optionList.ToList<UiOption>();
        _exitMsg = exitMsg;
    }


    //Public and private Methods
    public void UiLoop()
    {
        try
        {
            while(true)
            {
                displayOptions();
                string userInput = GetInput("Select a choice or [hotkey] from the menu: ").ToLower();
                if(!TryParseIndex(userInput)) //If parsing the index fails
                {
                    FindHotkey(userInput); //Try using a hotkey instead
                }
            }
        }
        catch (OperationCanceledException)
        {

        }
    }

    //Display Menu Options
    private void displayOptions()
    {
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
                _optionList[returnInt].Activate(); //Activate the selected index if possible
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

    private void ActivateOption(int index)
    {
        _optionList[index].Activate();
    }

    private void FindHotkey(string hotkey)
    {
        for(int i = 0; i < _optionList.Count; i++)
        {
            if(_optionList[i].CheckHotkey(hotkey))
            {
                _optionList[i].Activate();//Activate this option
                break;
            }
        }
    }

    static string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
}

class UiOption
{
    private Action _runAction;
    private String _name;
    private String _hotkey;

    public UiOption(Action runAction, string name, string hotkey)
    {
        _runAction = runAction;
        _name = name;
        _hotkey = hotkey;
    }

    public void Activate()
    {
        _runAction.Invoke();//Activate the function stored in action
    }

    public bool CheckHotkey(string hotkey)
    {
        return hotkey == _hotkey;
    }

    public string GetName()
    {
        return _name;
    }
}