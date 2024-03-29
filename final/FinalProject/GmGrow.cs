using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Msc = QuickUtils.Misc;
class GmGrow : GameMode
{
    //New Attribute
    protected int _stages = 25; //The number of "stages" the game is divided by
    //Constructors
    public GmGrow() : base()
    {
        _desc = "Grow Mode:_Has the same rules as classic mode_Hides all dice at first but makes them re-appear by a set number of stages_Suggested Grid Size: 8 by 8 or larger_Suggested Stages Count: Less than or equal to dice count";
        _displayName = "Grow Mode";
        //All other attributes are filled by the GameMode class constructor
    }
    //Fill all attributes constructor
    public GmGrow(int durationSec, bool? showCDown = null, string desc = "Grow Mode:_Has the same rules as classic mode_Hides all dice at first but makes them re-appear by a set number of stages_Suggested Grid Size: 8 by 8 or larger_Suggested Stages Count: Less than or equal to dice count", int stages = 10) : base(durationSec, showCDown, desc, "Grow Mode")
    {
        _stages = stages;
        //All other attributes are filled by the GameMode class constructor
    }

    //New getters and setters
    public int GetStages()
    {
        return _stages;
    }
    public void SetStages(int stages)
    {
        _stages = stages;
        if(_stages <= 1) //The lowest number we want is 2
        {
            _stages = 2;
        }
    }

    //Main Functionality
    //Main gameplay loop override
    protected override void GameLoop(DiceSet diceSetCopy)
    {
        //This is a little complicated because both threads need to be able to draw at the same time, luckily the grid size is fixed we theoretically can force them to write to the correct positions
        bool hasEnded = false; //Flag to send to the blink thread
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        Thread growThread = new Thread(()=>{GrowStart(diceSetCopy,()=>{return hasEnded;});}); //Create a thread for blinking, yes that's two lambdas, one to put the function in the thread, the other is the Func<bool> parameter in the function we are calling, we use lambdas to pass parameters, since including them would store the result of the function not the function call (which is what we want)
        
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        growThread.Start(); //Start the grow thread first, so the timer isn't cleared
        timerThread.Start(); //Start the timer thread
        
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
        hasEnded = true; //Tell the blink function we've ended so it doesn't print again
        timerThread.Join(); //Wait for the timer thread to join before continuing (this function ends around 1 second faster than the timer thread can handle) //Temporary patch until I can find out how to prevent the timer end message bug from happening when the dice set is very large
    }
    //An overrideable function call to the Grow method, so we can override it in decay
    protected virtual void GrowStart(DiceSet diceSetCopy, Func<bool> gmStatusCheck)
    {
        Grow(diceSetCopy, gmStatusCheck, true, "growThread");
    }

    //Actual Grow Functionality
    protected void Grow(DiceSet diceSetCopy, Func<bool> gmStatusCheck, bool hiddenStateStart = true, string threadName = "growThread")
    {
        //Calculate growth stages and durations
        int remainingDice = diceSetCopy.GetGridArea();
        int cycle = _stages;
        int growCycleMsecGap = ((_durationSec * 1000) / _stages);
        int dicePerCycle = (remainingDice / _stages);
        int finalCycle = ((hiddenStateStart) ? 1 : 0); //For grow, the final cycle is cylce 1, for decay the final cycle is 0
        //Store the current thread
        Thread growThread = Thread.CurrentThread;
        growThread.Name = threadName;
        try
        {
            diceSetCopy.SetAllVisibility(hiddenStateStart);
            while(!gmStatusCheck()) //Repeat until the game mode has ended, check this using the lambda function
            {
                dicePerCycle = ((remainingDice - dicePerCycle) < 0 || (((remainingDice - dicePerCycle) > 0) && ((cycle - 1) == finalCycle))) ? remainingDice : dicePerCycle; //Use a ternary operator to detect if the next cycle will be negative
                diceSetCopy.RandomQueryRun(dicePerCycle,
                    (Dice diceToCheck)=>{return diceToCheck.GetHidden() == hiddenStateStart;}, //Only accept dice that don't match the current state
                    (Dice diceToSet)=>{diceToSet.SetHidden(!hiddenStateStart);} //
                );
                diceSetCopy.Display();
                PausedSleepNoControl(new TimeSpan(0,0,0,0,growCycleMsecGap), gmStatusCheck); //Use paused sleep, but only fill the exit action, because that's all we need to exit this thread
                //Update counters
                cycle--;
                remainingDice -= dicePerCycle;
            }
        }
        catch(ThreadInterruptedException)
        {
            growThread.Interrupt();
        }
    }

    //Utility
    //An override to change the MakeSettingsMenu message, all the other variables are the same
    protected override UiMenu MakeSettingsMenu(string menuMsg="Grow Mode Settings:")
    {
        menuMsg = (menuMsg == "All Game Mode Settings:") ? "Main Menu > Options > Game Mode Options > Grow Mode Options:" : menuMsg; //Detect base class default string, and replace with this classe's default string, when it's not the default string update it so decay can work correctly
        UiMenu settingsMenu = base.MakeSettingsMenu(menuMsg); //Get the original menu, using the new default parameter
        //Add the new settings at the end before
        settingsMenu.AddOptionFromEnd(new UiOption(GetStages, SetStages, "&Growth Stages", 2), 1);
        return settingsMenu;
    }

    //Overrides to change the gmName (Child class default values do not override default values of the Parent Class), and include additional fields
    //File loading
    public override void LoadFromFile(string[] fileLines, ref int offset, string gmName = "gmGrow")
    {
        gmName = (gmName == "Null") ? "gmGrow" : gmName; //Correct the parameter, because when the base gamemode class is called from here the default value is null, but when a child calls this it's the parameter
        base.LoadFromFile(fileLines, ref offset, gmName);
        _stages = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_stages="));
    }

    //File Writing
    public override void WriteToFile(StreamWriter sWriter, string gmName = "gmGrow")
    {
        gmName = (gmName == "Null") ? "gmGrow" : gmName; //Correct the parameter, because when the base gamemode class is called from here the default value is null, but when a child calls this it's the parameter
        base.WriteToFile(sWriter, gmName);
        sWriter.WriteLine($"{gmName}_stages={_stages}");
    }
}