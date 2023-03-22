//Blink Game mode, contains the "Blink" game mode functionality
using UiMenu = QuickUtils.UiMenu;
class GmBlink : GameMode
{
    //Subclass specific attributes 
    private int _blinkMsecGap = 5000;
    private int _blinkRanChance = 1;
    private int _blinkRanChanceMax = 4;

    //Constructors
    //Blank Constructor
    public GmBlink() : base()
    {
        _desc = "Blink Mode:_Same rules as classic mode_WIP_Suggested Grid Size: 5 by 5 or larger";
        //All other attributes are filled by the GameMode class constructor
    }
    //Fill Attributes Constructor
    public GmBlink(int durationSec, bool? showCDown = null, string desc = "Blink Mode:_Same rules as classic mode_WIP_Suggested Grid Size: 5 by 5 or larger", int? blinkMsecGap = null, int? blinkRanChance = null, int? blinkRanChanceMax = null) : base(durationSec, showCDown, desc)
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
        //This is basically a carbon copy of the GameMode basic behavior. This is because the GameMode class itself will not be invoked directly except for testing, and in the future I want it to have inheritable functionality
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        Thread blinkThread = new Thread(()=>{}); //Create a thread for blinking
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        timerThread.Start(); //Start the timer thread
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
    }
    //Blink threaded function
    private void Blink(DiceSet curDiceSet)
    {

    }

    //Utility
    //An override to change the MakeSettingsMenu message, all the other variables are the same
    protected override UiMenu MakeSettingsMenu(string menuMsg="Blink Mode Settings:")
    {
        return base.MakeSettingsMenu(menuMsg); //Get the original menu, using the new default parameter
    }
}