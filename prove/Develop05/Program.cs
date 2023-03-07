/*
Enhancment list:
1. Implemented a new and improved UiMenu class
2. Implemented auto generated hotkeys for UiOptions
3. Utilized namespaces to reduce reduntant code across multiple classes
4. Added the ability to assign a name to the goal list, and display it
5. Added better prompts for saving and loading so the user isn't confused
*/

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

        //Console.WriteLine($"You have {_myGoals.GetPoints()} points.");
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
        UiMenu mainMenu = new UiMenu(new List<UiOption>
            {
                new UiOption(new Action(()=>{_myGoals.NewGoal();}),"Create &New Goal"), //Create a new goal
                new UiOption(new Action(()=>{_myGoals.Display();}),"List &Goals"), //Display the goals
                new UiOption(new Action(()=>{_myGoals.Save();}),"&Save Goals"), //Save the goals
                new UiOption(new Action(()=>{_myGoals.Load();}),"&Load Goals"), //Load the goals
                new UiOption(new Action(()=>{_myGoals.RecordEvent();}),"&Record Event"), //Mark a goal as done
                new UiOption(new Action(()=>{_myGoals.ChangeName();}),"&Change User Name"), //Change the user name
                //new UiOption(new Action(()=>{_myGoals.EditGoals();}),"&Edit Goals"), //Change the user name
                new UiOption(new Action(()=>{throw new OperationCanceledException();}),"E&xit") //Exit the menu
            },
            "Menu options:", //The intro message, it's special because it has 3 lines, including one that needs to be dynamically updated
            "Select a choice or [hotkey] from the menu: ", //User prompt
            "Now exiting...", //Exit message
            "  " //2 blank spaces as shown in the example video
        );
        mainMenu.UiLoop(new Action(DisplayPreLoopMessage)); //Start the main menu loop, don't clear the console ever
    }

    //Because points cannot be dynamically displayed and updated using a template string in the UiMenu class, 
    //I will instead make it so you can optionally pass an action to the UiLoop function, so that if this needs to be used again for a different reason it can
    static void DisplayPreLoopMessage()
    {
        Console.WriteLine("");
        Console.WriteLine($"You have {_myGoals.GetPoints()} points.");
        Console.WriteLine("");
    }
}