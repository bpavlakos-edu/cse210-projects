//Blink Game mode, contains the "Blink" game mode functionality
using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Msc = QuickUtils.Misc;
class GmBlink : GameMode
{
    //Subclass specific attributes 
    private int _blinkMsecGap = 10000;
    private int _blinkRanChance = 1;
    private int _blinkRanChanceMax = 4;

    //Constructors
    //Blank Constructor
    public GmBlink() : base()
    {
        _desc = "Blink Mode:_Same rules as classic mode_Randomly hides/shows dice periodically_It chooses dice to hide using the settings the game mode currently has_Suggested Grid Size: 5 by 5 or larger";
        //All other attributes are filled by the GameMode class constructor
    }
    //Fill Attributes Constructor
    public GmBlink(int durationSec, bool? showCDown = null, string desc = "Blink Mode:_Same rules as classic mode_Randomly hides/shows dice periodically_It chooses dice to hide using the settings the game mode currently has_Suggested Grid Size: 5 by 5 or larger", int? blinkMsecGap = null, int? blinkRanChance = null, int? blinkRanChanceMax = null) : base(durationSec, showCDown, desc)
    {
        //All base attributes are filled by the GameMode class constructor
        //Fill the new attributes or use their default values
        _blinkMsecGap = blinkMsecGap ?? _blinkMsecGap;
        _blinkRanChance = blinkRanChance ?? _blinkRanChance;
        _blinkRanChanceMax = blinkRanChanceMax ?? _blinkRanChanceMax;
    }
    //Subclass specific Getters and Setters (Normal external access Getters and Setters were auto generated using my AutoGetterSetter Python Script in C# mode)
    public int GetBlinkMsecGap()
    {
        return _blinkMsecGap;
    }
    public void SetBlinkMsecGap(int blinkMsecGap)
    {
        _blinkMsecGap = blinkMsecGap;
    }
    public int GetBlinkRanChance()
    {
        return _blinkRanChance;
    }
    public void SetBlinkRanChance(int blinkRanChance)
    {
        _blinkRanChance = blinkRanChance;
    }
    public int GetBlinkRanChanceMax()
    {
        return _blinkRanChanceMax;
    }
    public void SetBlinkRanChanceMax(int blinkRanChanceMax)
    {
        _blinkRanChanceMax = blinkRanChanceMax;
    }
    //Other Getters and setters are inherited from the GameMode class

    //Methods

    //Main Functionality
    //Main gameplay loop override
    protected override void GameLoop(DiceSet diceSetCopy)
    {
        //This is a little complicated because both threads need to be able to draw at the same time, luckily the grid size is fixed we theoretically can force them to write to the correct positions
        bool hasEnded = false; //Flag to send to the blink thread
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        Thread blinkThread = new Thread(()=>{Blink(diceSetCopy,()=>{return hasEnded;});}); //Create a thread for blinking, yes that's two lambdas, one to put the function in the thread, the other is the Func<bool> parameter in the function we are calling, we use lambdas to pass parameters, since including them would store the result of the function not the function call (which is what we want)
        
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        blinkThread.Start(); //Start the blink thread first, so the timer isn't cleared
        timerThread.Start(); //Start the timer thread
        
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
        hasEnded = true; //Tell the blink function we've ended so it doesn't print again
        timerThread.Join(); //Wait for the timer thread to join before continuing (this function ends around 1 second faster than the timer thread can handle) //Temporary patch until I can find out how to prevent the timer end message bug from happening when the dice set is very large
    }
    //Blink threaded function
    //Accepts the current dice set so we have access to randomHide, accepts a function to check if the game has ended or not
    private void Blink(DiceSet curDiceSet, Func<bool> gmStatusCheck)
    {
        while(!gmStatusCheck()) //Repeat until the game mode has ended, check this using the lambda function
        {
            curDiceSet.RandomHide(_blinkRanChance, _blinkRanChanceMax); //Trigger random hiding using the settings this game mode currently has
            Thread.Sleep(_blinkMsecGap); //Sleep for the blink gap
        }
    }

    //Utility
    //An override to change the MakeSettingsMenu message, and add additional settings for each parameter
    protected override UiMenu MakeSettingsMenu(string menuMsg="Blink Mode Settings:")
    {
        UiMenu settingsMenu = base.MakeSettingsMenu(menuMsg); //Get the original menu, using the new default parameter
        //Add the new settings at the end before
        settingsMenu.AddOptionFromEnd(new UiOption(GetBlinkMsecGap, SetBlinkMsecGap, "Bli&nk Delay in Milliseconds", 10), 1);
        settingsMenu.AddOptionFromEnd(new UiOption(GetBlinkRanChance, SetBlinkMsecGap, "Blink Odds &Chance" , 1), 1);
        settingsMenu.AddOptionFromEnd(new UiOption(GetBlinkRanChanceMax, SetBlinkRanChanceMax, "Blink Odds &Maximum", 1), 1);
        return settingsMenu;
    }

    //Load from a file
    public override void LoadFromFile(string[] fileLines, ref int offset, string gmName = "gmBlink")
    {
        base.LoadFromFile(fileLines, ref offset, gmName); //Load the shared values from the original
        //Load all blink-specific fields
        _blinkMsecGap = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_hideMsecGap="));
        _blinkRanChance = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_hideRanChance="));
        _blinkRanChanceMax = int.Parse(Msc.ReadFileLine(fileLines, ref offset, $"{gmName}_hideRanChanceMax="));
    }
}