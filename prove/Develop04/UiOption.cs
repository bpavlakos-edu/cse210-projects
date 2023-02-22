
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