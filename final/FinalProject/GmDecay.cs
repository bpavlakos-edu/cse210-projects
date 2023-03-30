using UiMenu = QuickUtils.UiMenu;
using UiOption = QuickUtils.UiOption;
using Msc = QuickUtils.Misc;

class GmDecay : GmGrow
{
    public GmDecay() : base()
    {
        _desc = "Decay Mode:_Has the same rules as classic mode_Shows all dice at first but makes them disappear by a set number of stages_Suggested Grid Size: 8 by 8 or larger_Suggested Stages Count: Less than or equal to dice count";
        _displayName = "Decay Mode";
        //All other attributes are filled by the GmDecay class constructor
    }
    //Fill all attributes constructor
    public GmDecay(int durationSec, bool? showCDown = null, string desc = "Decay Mode:_Has the same rules as classic mode_Shows all dice at first but makes them disappear by a set number of stages_Suggested Grid Size: 8 by 8 or larger_Suggested Stages Count: Less than or equal to dice count", int stages = 10) : base(durationSec, showCDown, desc, stages)
    {
        _displayName = "Decay Mode"; //Decay inherits GmGrow, which doesn't have a constructor that includes the gmName field
        //All attributes are filled by the GmDecay class constructor
    }
    
    //Override the virtual method I made in the grow class to make this easier
    protected override void GrowStart(DiceSet diceSetCopy, Func<bool> gmStatusCheck)
    {
        Grow(diceSetCopy, gmStatusCheck, false, "decayThread");
    }

    //The following methods have been overridden to ensure they correctly read and write the correct game mode

    //Utility
    //An override to change the MakeSettingsMenu message, all the other variables are the same
    protected override UiMenu MakeSettingsMenu(string menuMsg="Decay Mode Settings:")
    {
        UiMenu settingsMenu = base.MakeSettingsMenu("Main Menu > Options > Game Mode Options > Decay Mode Settings:"); //Get the original menu, using the new default parameter
        //Add the new settings at the end before
        settingsMenu.RemoveOptionFromEnd(1); //Remove the grow option
        settingsMenu.AddOptionFromEnd(new UiOption(GetStages, SetStages, "De&cay Stages", 2), 1);
        return settingsMenu;
    }
    //File loading
    //An override to change the gmName because the rest is the same
    public override void LoadFromFile(string[] fileLines, ref int offset, string gmName = "gmDecay")
    {
        base.LoadFromFile(fileLines, ref offset, "gmDecay");
    }

    //File Writing
    public override void WriteToFile(StreamWriter sWriter, string gmName = "gmDecay")
    {
        base.WriteToFile(sWriter, "gmDecay");
    }
}