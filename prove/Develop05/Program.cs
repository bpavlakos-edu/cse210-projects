using System;
using UiMenu = QuickUtils.UiMenu; //Importing the custom UI menu class
using UiOption = QuickUtils.UiOption; //Importing the custom Ui Option class

class Program
{
    static GoalManager _myGoals = new GoalManager(); //Create the goal manager object
    static void Main(string[] args)
    {
        UiLoop();
    }
    static void UiLoop()
    {

        Console.WriteLine($"You have {_myGoals.GetPoints()} points.");
        //Console.WriteLine("Menu options:");
        //Console.WriteLine("Select a choice from the menu: ");
        //New UIMenu
        /*
        This new menu automatically generates hotkeys using the index of "&"
        This "&" hotkey system is based on the Real-Time-Strategy video game "Command and Conquer Generals" and it's expansion "Zero Hour"
        When you create modded unit in that game, the button to construct your new unit requires a string.
        That string uses the index of "&" + 1 to determine the letter in the string to use as the hotkey
        This makes creating hotkeys in Command and Conquer easy and infinitely flexible (as long as they don't share the same letter!)
        */
        UiMenu mainUI = new UiMenu(new List<UiOption>
            {
                new UiOption(new Action(()=>{_myGoals.NewGoal();}),"Create &New Goal"),
                new UiOption(new Action(()=>{_myGoals.NewGoal();}),"Create &New Goal"),
            },
            "Menu options:",
            "Select a choice or [hotkey] from the menu",
            "Now exiting..."
        );

    }
}