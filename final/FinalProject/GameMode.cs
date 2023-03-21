using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException;
using Inp = QuickUtils.Inputs; //User inputs
using Msc = QuickUtils.Misc; //List copying and other redundant code
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
        DiceSet diceSetCopy = new DiceSet(curDiceSet); //Copy the current dice set so the main dice set isn't modified during the game mode
        GameLoop(diceSetCopy);
        ShowEndMsg(diceSetCopy);
    }
    //Main gameplay loop (overidden by child classes)
    protected virtual void GameLoop(DiceSet diceSetCopy)
    {
        //Example code of how a GameLoop would go:
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        timerThread.Start(); //Start the timer thread
        timerThread.Join(_durationSec * 1000); //Join
    }
    //Show the end message, and let the user check the current dice set
    private void ShowEndMsg(DiceSet diceCopy)
    {
        //Console.Beep(); //Make the console beep
        diceCopy.SetAllVisibility(false); //Reset all dice visibility
        diceCopy.Display();//Print the dice
        Console.WriteLine("Time's up!");
        Inp.GetInput("Please check your words using the display above. Press enter to continue when finished.");
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
        MakeSettingsMenu().UiLoop(); //Activate the UiLoop of the generated menu
    }
    //Utility
    public void CountDown(int msecDuration, int refreshMsecDelay = 1000)
    {
        if(_showCDown) //Check the show countdown setting
        {
            int strBufferSize = 0; //Use this to keep track of the previous string's length
            for(int remainingTimeMsec = (msecDuration - (msecDuration % refreshMsecDelay)); remainingTimeMsec > 0; remainingTimeMsec -= refreshMsecDelay) //Use a for loop to calculate the remaining time, it starts at the msecDuration with any leftover miliseconds that don't fit into the refresh rate removed, every loop cycle it updates the remaining time by subtracting the refreshDelay
            {
                long cycleStartTime = DateTime.Now.Ticks; //Capture the start time

                string tStr = TicksToTimerStr(remainingTimeMsec * 10000); //multiply remaining time by 10,000 to get ticks, then use it to get the countdown string
                int newBufferSize = tStr.Length; //Store the new strings length, because we're going to change it next
                tStr = (strBufferSize > tStr.Length) ? (tStr + new string(' ', strBufferSize - tStr.Length)) : tStr; //Add gaps to overwrite the previous timer if the length isn't the same. Using the ternary operator, do this only if the last string length is greater than the current string length
                Console.Write(tStr + new String('\b', strBufferSize)); //Write the timer string, make sure to backspace (using '\b') everything (including the extra spaces) so the next timer string overwrites this one
                strBufferSize = newBufferSize; //Update the buffer size, so the next timer string has an accurate length to overwrite

                Thread.Sleep((new TimeSpan(((long) refreshMsecDelay * 10000) - (DateTime.Now.Ticks - cycleStartTime)))); //Calculate the remaining time we have until the next cycle and sleep by that amount of time
            }
        }
        else //Simple thread sleep for the requested duration
        {
            Thread.Sleep(msecDuration);
        }
    }
    //Function overload for counting down by seconds, needs a different name to make it not conflict due to having the same parameter types
    public void CountDownSec(int secDuration, int refreshMsecDelay = 1000)
    {
        CountDown(secDuration * 1000, refreshMsecDelay);
    }
    public string TicksToTimerStr(long ticks)
    {
        TimeSpan remainingTimeSpan = new TimeSpan(ticks);
        return remainingTimeSpan.ToString(@"mm\:ss"); //@ means absolute string. Found the formatting specifications for timespan here: https://learn.microsoft.com/en-us/dotnet/api/system.timespan.tostring?view=net-7.0#system-timespan-tostring(system-string)
    }
    //Make a UiMenu for this class, to modify the game mode's settings
    public virtual UiMenu MakeSettingsMenu()
    {
        return new UiMenu(
            new List<UiOption>
            {
                new UiOption(GetDurationSec,SetDurationSec,"&Timer Length in Seconds", 1, null),
                new UiOption(GetShowCDown,SetShowCDown,"&Enable Timer Display"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back")
            },
            "Game Mode Options:",
            exitMsg:"" //Hide the exit message
        );
    }
}