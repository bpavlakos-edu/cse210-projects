//Classic Game mode, contains the "Random" game mode functionality
using UiMenu = QuickUtils.UiMenu;
class GmRandom : GameMode
{
    //No class specific attributes
    //Constructors
    //Blank Constructor
    public GmRandom() : base()
    {
        _desc = "True Random Mode:_Rules are identical to classic mode_Every Dice will have access to the entire alphabet_Depending on your luck, true randomization could be much more difficult or easy than the Classic Game Mode_Suggested Grid Size: 7 by 7 or larger";
        //All other attributes are filled by the GameMode class constructor
    }
    //Fill Attributes Constructor
    public GmRandom(int durationSec, bool? showCDown = null, string desc = "True Random Mode:_Rules are identical to classic mode_Every Dice will have access to the entire alphabet_Depending on your luck, true randomization could be much more difficult or easy than the Classic Game Mode_Suggested Grid Size: 7 by 7 or larger") : base(durationSec, showCDown, desc)
    {
        //All attributes are filled by the GameMode class constructor
    }

    //Getters and setters are inherited from the GameMode class

    //Methods

    //Main Functionality
    //Main gameplay loop override
    protected override void GameLoop(DiceSet diceSetCopy)
    {
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        diceSetCopy.SetAll('?'); //Use set all to fill all dice in the dice list copy with '?', which enables randomization
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        timerThread.Start(); //Start the timer thread
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
    }

    //Utility
    //An override to change the MakeSettingsMenu message, all the other variables are the same
    protected override UiMenu MakeSettingsMenu(string menuMsg="Random Mode Settings:")
    {
        return base.MakeSettingsMenu(menuMsg); //Get the original menu, using the new default parameter
    }
}