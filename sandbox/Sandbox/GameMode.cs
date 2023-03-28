using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using UiMenuExitException = QuickUtils.UiMenuExitException;
using Inp = QuickUtils.Inputs; //User inputs
using Msc = QuickUtils.Misc; //List copying and other redundant code
class GameMode
{
    //Attributes
    protected int _durationSec = 180; //Game mode duration
    protected bool _showCDown = true; //Flag to control whether to display a timer or not
    protected string _desc = ""; //Game mode description for the help menu, shouldn't really be changed

    //Constructors
    //Blank Constructor
    public GameMode()
    {
        
    }
    //Fill all attributes (Supports using null to get default values)
    public GameMode(int durationSec, bool? showCDown = null, string desc = null)
    {
        _durationSec = durationSec;
        _showCDown = showCDown ?? _showCDown;
        _desc = desc ?? _desc;
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
    public bool GetShowCDown()
    {
        return _showCDown;
    }
    public void SetShowCDown(bool showCDown)
    {
        _showCDown = showCDown;
    }
    public string GetDesc()
    {
        return _desc;
    }
    public void SetDesc(string desc)
    {
        _desc = desc;
    }

    //Methods
    //Main Functionality
    //Start this game mode
    public void Start(DiceSet curDiceSet)
    {
        do
        {
            DiceSet diceSetCopy = new DiceSet(curDiceSet, false); //Copy the current dice set so the main dice set isn't modified during the game mode
            GameLoop(diceSetCopy); //Start the Game Mode Loop
            ShowEndMsg(diceSetCopy); //Print the end message when it finishes
        }
        while(Inp.GetBoolInput("Would you like to play again?: ", curValue:false, hideCurValue:true) == true); //Repeat until the user says they are finished. This input configuartion will default to false if it's left blank
    }
    //Main gameplay loop (overidden by child classes)
    protected virtual void GameLoop(DiceSet diceSetCopy)
    {
        //Example code of how a GameLoop would go:
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        timerThread.Start(); //Start the timer thread
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
        timerThread.Join();//Temporary patch until I can find out how to prevent the timer end message bug from happening when the dice set is very large
    }
    //Show the end message, and let the user check the current dice set
    private void ShowEndMsg(DiceSet diceCopy)
    {
        diceCopy.SetAllVisibility(false); //Reset all dice visibility
        diceCopy.Display();//Print the dice
        Console.WriteLine("Time's up!");
        if(OperatingSystem.IsWindows()) //Required check for console.beep (see here: https://learn.microsoft.com/en-us/dotnet/core/compatibility/code-analysis/5.0/ca1416-platform-compatibility-analyzer )
        {
            Console.Beep(500, 1000); //Make the console beep
        }
        Console.WriteLine("Please check your words using the display above");
        Inp.GetInput("Press enter to continue");
    }
    //Display this game mode's description
    public void DisplayHelp()
    {
        Console.WriteLine(_desc.Replace("_",Environment.NewLine)); //Write the game mode description, replace underscore with newline since we cant use Enviroment.Newline as a parameter default
        Console.WriteLine("");
        Inp.GetInput("Press enter to continue");
    }
    //Open the settings menu
    public void OpenSettings()
    {
        MakeSettingsMenu().UiLoop(); //Activate the UiLoop of the generated menu
    }

    //Utility
    protected void CountDown(int msecDuration, int refreshMsecDelay = 1000)
    {
        if(_showCDown) //Check the show countdown setting, to make sure we need to display anything
        {
            Console.CursorVisible = false; //Disable the cursor marker [Credit: http://dontcodetired.com/blog/post/Creating-a-Spinner-Animation-in-a-Console-Application-in-C ] [Microsoft Docs: http://msdn.microsoft.com/en-us/library/system.console.cursorvisible%28v=vs.110%29.aspx ]
            int strBufferSize = 0; //Use this to keep track of the previous string's length
            for(int remainingTimeMsec = (msecDuration - (msecDuration % refreshMsecDelay)); remainingTimeMsec > 0; remainingTimeMsec -= refreshMsecDelay) //Use a for loop to calculate the remaining time, it starts at the msecDuration with any leftover miliseconds that don't fit into the refresh rate removed, every loop cycle it updates the remaining time by subtracting the refreshDelay
            {
                long cycleStartTime = DateTime.Now.Ticks; //Capture the start time

                string tStr = TicksToTimerStr(remainingTimeMsec * 10000); //multiply remaining time by 10,000 to get ticks, then use it to get the countdown string
                int newBufferSize = tStr.Length; //Store the new strings length, because we're going to change it next
                tStr = (strBufferSize > tStr.Length) ? (tStr + new string(' ', strBufferSize - tStr.Length)) : tStr; //Add gaps to overwrite the previous timer if the length isn't the same. Using the ternary operator, do this only if the last string length is greater than the current string length
                Console.Write(tStr + new String('\b', (strBufferSize >= tStr.Length) ? strBufferSize : tStr.Length)); //Write the timer string, make sure to backspace (using '\b') everything (including the extra spaces) so the next timer string overwrites this one. Use the ternary operator to detect when the new string's length is longer than the original so it can backspace with that instead
                strBufferSize = newBufferSize; //Update the buffer size, so the next timer string has an accurate length to overwrite

                Thread.Sleep((new TimeSpan(((long) refreshMsecDelay * 10000) - (DateTime.Now.Ticks - cycleStartTime)))); //Calculate the remaining time we have until the next cycle and sleep by that amount of time
            }
            Console.CursorVisible = true; //Timer has ended, restore console cursor visibility
        }
        else //Simple thread sleep for the requested duration
        {
            Thread.Sleep(msecDuration);
        }
    }
    //Function overload for counting down by seconds, needs a different name to make it not conflict due to having the same parameter types
    protected void CountDownSec(int secDuration, int refreshMsecDelay = 1000)
    {
        CountDown(secDuration * 1000, refreshMsecDelay);
    }
    //Return the string representation of ticks, using a TimeSpan class to print it in MM:SS
    protected string TicksToTimerStr(long ticks)
    {
        return (new TimeSpan(ticks)).ToString(@"mm\:ss"); //@ means absolute string. Found the formatting specifications for timespan here: https://learn.microsoft.com/en-us/dotnet/api/system.timespan.tostring?view=net-7.0#system-timespan-tostring(system-string)
    }

    //Make a UiMenu for this class, to modify the game mode's settings
    protected virtual UiMenu MakeSettingsMenu(string menuMsg="All Game Mode Settings:")
    {
        return new UiMenu(
            new List<UiOption>
            {
                new UiOption(GetDurationSec,SetDurationSec,"&Timer Length in Seconds", 1, null),
                new UiOption(GetShowCDown,SetShowCDown,"Show Timer &Display"),
                new UiOption(()=>{throw new UiMenuExitException();},"Go &Back")
            },
            menuMsg,
            exitMsg:"" //Hide the exit message
        );
    }

    //Update Settings using a GameMode class, this will be used for the "All Game Modes" setting option in the main menu
    public void UpdateFields(GameMode newGameMode, bool updateDesc = false)
    {
        _durationSec = newGameMode.GetDurationSec();
        _showCDown = newGameMode.GetShowCDown();
        if(updateDesc) //Optional control for updating Desc, which really shouldn't be changed for sub classes at run time
        {
            _desc = newGameMode.GetDesc();
        }
    }

    //File Loading
    public virtual void LoadFromFile(string[] fileLines, ref int offset, string gmName = "Null") //Use the reference type to update the offset ( https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref#passing-an-argument-by-reference-an-example )
    {
        _durationSec = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_durSec=")); //Store the countdown duration read from the file//gmName_durSec=getThisInteger
        _showCDown = Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_showCountDown=").ToLower() != "false"; //Store the timer visiblity option value read from the file, treat unrecognized values as "true" //gmName_showCountDown=true
    }
    //When writing, write an extra line telling us what game mode this is
    public virtual void WriteToFile(StreamWriter sWriter, string gmName = "Null")
    {

    }
}