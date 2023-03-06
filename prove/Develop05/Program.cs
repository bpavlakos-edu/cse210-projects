using System;

class Program
{
    static GoalManager _myGoals = new GoalManager(); //Create the goal manager object
    static void Main(string[] args)
    {
        UiLoop();
    }
    static void UiLoop()
    {
        while(true)
        {
            Console.WriteLine($"You have {_myGoals.GetPoints()} points.");
            //Console.WriteLine("Menu options:");
            //Console.WriteLine("Select a choice from the menu: ");
            //New UIMenu
        }
    }
}