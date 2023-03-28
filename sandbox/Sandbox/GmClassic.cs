//Classic Game mode, contains the "original" game mode functionality
using UiMenu = QuickUtils.UiMenu;
class GmClassic : GameMode
{
    //No class specific attributes
    //Constructors
    //Blank Constructor
    public GmClassic() : base()
    {
        _desc = "Classic Mode:_Rolls all dice and starts a timer_Write down as many words as you can find_The player with the most unique words wins!_Suggested Grid Size: 5 by 5 or larger";
        //All other attributes are filled by the GameMode class constructor
    }
    //Fill Attributes Constructor
    public GmClassic(int durationSec, bool? showCDown = null, string desc = "Classic Mode:_Rolls all dice and starts a timer_Write down as many words as you can find_The player with the most unique words wins!_Suggested Grid Size: 5 by 5 or larger") : base(durationSec, showCDown, desc)
    {
        //All attributes are filled by the GameMode class constructor
    }

    //Getters and setters are inherited from the GameMode class

    //Methods

    //Main Functionality
    //Main gameplay loop override
    protected override void GameLoop(DiceSet diceSetCopy)
    {
        //This is basically a carbon copy of the GameMode basic behavior. This is because the GameMode class itself will not be invoked directly except for testing, and in the future I want it to have inheritable functionality
        Thread timerThread = new Thread(()=>{CountDownSec(_durationSec);}); //Create a thread that calls the timer function
        diceSetCopy.RollAll(); //Roll all the dice, which will display them
        timerThread.Start(); //Start the timer thread
        bool threadEndedOnTime = timerThread.Join(_durationSec * 1000); //Join by the duration specified for this game mode, store whether it Joined in time into a boolean
        timerThread.Join(); //Temporary patch until I can find out how to prevent the timer end message bug from happening when the dice set is very large
    }

    //Utility
    //An override to change the MakeSettingsMenu message, all the other variables are the same
    protected override UiMenu MakeSettingsMenu(string menuMsg="Classic Mode Settings:")
    {
        return base.MakeSettingsMenu(menuMsg); //Get the original menu, using the new default parameter
    }

    //File loading
    //An override to change the gmName because the rest is the same
    public override void LoadFromFile(string[] fileLines, ref int offset, string gmName = "gmClassic")
    {
        base.LoadFromFile(fileLines, ref offset, gmName);
    }

    //File Writing
    public override void WriteToFile(StreamWriter sWriter, string gmName = "gmClassic")
    {
        base.WriteToFile(sWriter, "gmClassic");
    }

}