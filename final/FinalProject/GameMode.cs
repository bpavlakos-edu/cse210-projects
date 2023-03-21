using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException;
using Inp = QuickUtils.Inputs;
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
        Console.WriteLine(_desc);
        Inp.GetInput("Press Enter to Continue");
    }
    //Open the settings menu
    public void OpenSettings()
    {
        MakeSettingsMenu().UiLoop();
    }
    //Utility
    public void CountDown(int msecDuration, int refreshMsecDelay = 1000)
    {
        int timerCycles = (msecDuration - (msecDuration % refreshMsecDelay)) / refreshMsecDelay; //Calculate the number of cycles Trim excess cycles using modulus
        int strBufferSize = 0; //Use this to keep track of the previous string's length
        for(int tOffset = 0; tOffset < timerCycles; tOffset += refreshMsecDelay) //And this is why for loops are cool! //Calculate timer offset, because I don't want to do a costly multiplaction operation on a time sensitive function
        {
            long startTime = DateTime.Now.Ticks; //Capture the start time
            string tStr = TicksToTimerStr((msecDuration - tOffset) * 10000); //Subtract the offset from the total time duration to get the countdown, multiply by 10,000 to get ticks
            int newBufferSize = tStr.Length; //Store the new string's original length
            tStr = (strBufferSize > tStr.Length) ? tStr + new string(' ', strBufferSize - tStr.Length) : tStr; //Add gaps to overwrite the previous timer if the length isn't the same. Using the ternary operator, do this only if the size is greater than the current string length
            Console.Write(tStr + new String('\b', strBufferSize)); //Write the timer string, make sure to backspace (using '\b') everything (including the extra spaces) so the next timer string overwrites this one
            strBufferSize = newBufferSize; //Update the buffer size, so the next timer string has an accurate measurement
            Thread.Sleep((new TimeSpan(((long) refreshMsecDelay * 10000) - (DateTime.Now.Ticks - startTime)))); //Calculate the remaining time we have until the next cylce and sleep by that amount of time
        }
    }
    public virtual UiMenu MakeSettingsMenu()
    {
        return new UiMenu();
    }
    public string TicksToTimerStr(long ticks)
    {
        TimeSpan remainingTimeSpan = new TimeSpan(ticks);
        return remainingTimeSpan.ToString(@"mm\:ss"); //@ means absolute string. Found the formatting specifications for timespan here: https://learn.microsoft.com/en-us/dotnet/api/system.timespan.tostring?view=net-7.0#system-timespan-tostring(system-string)
    }
    private void ShowEndMsg()
    {

    }
}