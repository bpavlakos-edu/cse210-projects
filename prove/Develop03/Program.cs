using System;

class Program
{
    public static Scripture myScripture = new Scripture("Mosiah 2:17","And behold, I tell you these things that ye may learn wisdom; that ye may learn that when ye are in the service of your fellow beings ye are only in the service of your God.");
    static void Main(string[] args)
    {
        UiLoop();
    }
    static void UiLoop()
    {
        while(1 == 1)
        {
            Console.Clear();
            myScripture.Display();
            Console.WriteLine();//Blank Line
            string userInput = GetInput("");
            if(userInput == "quit")
            {
                break;
            }
            else if(userInput == "reset")
            {

            }
            else
            {
                if(myScripture.NextTurn() == false)
                {

                }
            }
        }
    }
    public static string GetInput(string inMsg="", bool returnLowerCase = true)
    {
        Console.Write(inMsg);
        string userInput = Console.ReadLine();
        if(returnLowerCase)
        {
            return userInput.ToLower(); //Return lower case input
        }
        //We can't reach this code if returnLowerCase is true
        return userInput; //Return the raw input
    }
    static bool ConfirmExit()
    {
        return GetInput("Would you like to quit? (enter Y to quit, anything else to restart): ") == "y";
    }
}