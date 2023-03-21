using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException;
class GameMode
{
    
    //Attributes
    protected int _durationSec = 120;
    protected string _desc = "";
    protected bool _showCDown = true; //Flag to control whether to display a timer or not

    //Constructors
    //Blank Constructor
    public GameMode()
    {
        
    }
    //Fill all attributes (Supports using null to get default values)
    public GameMode(int durationSec, string desc = null, bool? showCDown = null)
    {
        _durationSec = durationSec;
        _desc = desc ?? _desc;
        _showCDown = showCDown ?? _showCDown;
    }

    //Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public int GetDurationSec()
    {
        return _durationSec;
    }
    public void SetDurationSec(int durationSec)
    {
        _durationSec = durationSec;
    }
    public string GetDesc()
    {
        return _desc;
    }
    public void SetDesc(string desc)
    {
        _desc = desc;
    }
    public bool GetShowCDown()
    {
        return _showCDown;
    }
    public void SetShowCDown(bool showCDown)
    {
        _showCDown = showCDown;
    }

    //Methods
    //Main Functionality
    public void Start(DiceSet curDiceSet)
    {

    }
    //Main gameplay loop (overidden by child classes)
    protected virtual void GameLoop()
    {

    }
    //Display this game mode's description
    public void DisplayHelp()
    {

    }
    //Open the settings menu
    public void OpenSettings()
    {
        MakeSettingsMenu().UiLoop();
    }
    //Utility
    public void CountDown(int msec)
    {

    }
    public virtual UiMenu MakeSettingsMenu()
    {
        return new UiMenu();
    }
    public string TicksToTimerStr(long ticks)
    {
        return "";
    }
    private void ShowEndMsg()
    {

    }
}