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
    protected bool _paused = false; //This is not an externally accessible property, it's a global variable for threaded functions

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
            try
            {
                Console.CursorVisible = false; //Hide the cursor
                GameLoop(diceSetCopy); //Start the Game Mode Loop
            }
            catch(UiMenuExitException){} //Exit immediately when the user requests an early exit
            Console.CursorVisible = true; //Show the cursor
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
        Thread countDownThread = Thread.CurrentThread; //Store the current thread for the PausedSleep to interrupt
        countDownThread.Name = "countDownThread"; //Name the thread for debugging
        try
        {
            _paused = false;
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
                    TimeSpan SleepDuration = (new TimeSpan(((long) refreshMsecDelay * 10000) - (DateTime.Now.Ticks - cycleStartTime))); //Calculate the remaining time we have until the next cycle and sleep by that amount of time
                    PausedSleep(_paused, SleepDuration, (bool pauseStatus) => {_paused = true; WritePauseStatus(pauseStatus);}, () => {countDownThread.Interrupt();}); //Use the new pauseable timer
                }
                Console.CursorVisible = true; //Timer has ended, restore console cursor visibility
            }
            else //Simple thread sleep for the requested duration
            {
                PausedSleep(_paused, new TimeSpan(0,0,0,0,msecDuration), (bool pauseStatus)=>{_paused = true; WritePauseStatus(pauseStatus);},() => {countDownThread.Interrupt();});
            }
        }
        catch(ThreadInterruptedException)
        {
            //End the timer early
        }
    }

    protected void WritePauseStatus(bool paused, string pauseMsg = "~ Paused Press 'p' to Continue ~")
    {
        Console.Write((paused) ? pauseMsg : new string('\b',pauseMsg.Length) + new String(' ',pauseMsg.Length) + new string('\b',pauseMsg.Length));
    }

    //Paused Sleep function, developed in offline sandbox
    protected void PausedSleep(bool pauseInput, TimeSpan duration, Action<bool> setPausedFunction, Action exitAction, int msecPerCheck = 100)
    {
        bool paused = false;
        bool timerEnded = false;
        bool exiting = false;
        //The primary reason why this is here and not in it's own function is so that it can access both paused and set paused function easily
        Thread pauseThread = new Thread(()=>
            {
                try
                {
                    while(!timerEnded) //Repeat Until Timer has ended or it's interrupted
                    {
                        if(Console.KeyAvailable) //The solution to avoiding the Console.ReadKey freeze here is to use console.KeyAvailable: https://stackoverflow.com/questions/14385044/console-readkey-canceling https://learn.microsoft.com/en-us/dotnet/api/system.console.keyavailable?view=net-7.0
                        {
                            ConsoleKey ReadKey = Console.ReadKey(true).Key; //Console.readKey pauses the thread //Setting readkey to true hides it
                            if(ReadKey == ConsoleKey.X /* && !paused */) //Exit sequence
                            {
                                /* duration = new TimeSpan(0);
                                paused = false; */
                                exiting = true;
                                exitAction(); //Activate the exit action
                            }
                            else if(ReadKey == ConsoleKey.P) //if p is pressed
                            {
                                paused = !paused; //Flip the paused state , otherwise just keep it the same
                                setPausedFunction(paused); //Update the callback parameter to update the paused state in the function that uses this timer
                            }
                        }
                        Thread.Sleep(msecPerCheck); //If we don't restrict the while loop it will drive up CPU usage (it also gives a location to be interuppted)
                    }
                }
                catch(ThreadInterruptedException)
                {
                    if(exiting)
                    {
                        exiting = false;
                        exitAction(); //Return to main thread when interrupt is ordered, activate the exit action for the third time!
                    }
                }
            }
        );
        pauseThread.Name = "PauseThread";
        //Sleeping timer, decrements by the desired timespan
        pauseThread.Start(); //Start the pause thread
        try
        {
            while(duration.Ticks > 0)
            {
                Thread.Sleep(msecPerCheck); //Sleep for the refresh duration, it needs to be longer than the computation time of this function, otherwise we'll sleep longer than needed
                if(!paused) //When it's not paused
                {
                    duration -= new TimeSpan(0,0,0,0,msecPerCheck); //Subtract from the remaining time
                }
            }
            timerEnded = true; //Tell the pause thread the waiting is over
            paused = false; //Tell the original function that pausing is over
            if(!pauseThread.Join(0)) //Tell it to join instantly, use it's Join status (which is a boolean) to activate this code
            {
                pauseThread.Interrupt(); //When it fails to join in time, Interrupt it manually
            }
        }
        catch(ThreadInterruptedException) //When this thread is interrupted, catch the error before forcing the other one to join
        {
            pauseThread.Interrupt();
            pauseThread.Join();
            Thread.CurrentThread.Interrupt(); //Force the current thread to exit (I'm really not sure if this works)
        }
    }

    //Paused sleep with no input control
    protected void PausedSleepNoControl(TimeSpan duration, Func<bool> checkExitStatus, int msecPerCheck = 100)
    {
        while(duration.Ticks > 0 && !checkExitStatus())
        {
            Thread.Sleep(msecPerCheck); //Sleep for the refresh duration, it needs to be longer than the computation time of this function, otherwise we'll sleep longer than needed
            if(!_paused) //When it's not paused
            {
                duration -= new TimeSpan(0,0,0,0,msecPerCheck); //Subtract from the remaining time
            }
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
        _durationSec = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_durationSec=")); //Store the countdown duration read from the file//gmName_durSec=getThisInteger
        _showCDown = Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_showCountDown=").ToLower() != "false"; //Store the timer visiblity option value read from the file, treat unrecognized values as "true" //gmName_showCountDown=true
    }
    //When writing, write an extra line telling us what game mode this is
    public virtual void WriteToFile(StreamWriter sWriter, string gmName = "Null")
    {
        sWriter.WriteLine($"GmName={gmName}");
        sWriter.WriteLine($"{gmName}_durationSec={_durationSec}");
        sWriter.WriteLine($"{gmName}_showCountDown=" + ((_showCDown) ? "true" : "false")); //Use the ternary operator to write true or false depending on what _showCDown is set to
    }
}