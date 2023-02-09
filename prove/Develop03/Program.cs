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
            string userInput = GetInputLine("Press enter to continue, type 'reset' to restart, or type 'quit' to finish:");
            if(userInput == "quit")
            {
                break;//Quit the loop
            }
            else if(userInput == "reset")
            {
                myScripture.Reset();
            }
            else
            {
                //Unrecognized input == initalize next turn
                if(myScripture.NextTurn() == false)
                {
                    //The next turn failed, initalize exit sequence
                    if(ConfirmExit())
                    {
                        break;
                    }
                    else
                    {
                        myScripture.Reset();
                    }

                }
            }
        }
    }
    //New line overload created to match original
    public static string GetInputLine(string inMsg="", bool returnLowerCase = true)
    {
        return GetInput(inMsg+Environment.NewLine, returnLowerCase);
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
        string userInput = GetInput("Would you like to quit? (enter 'y' or 'quit' to finish, enter anything else to restart): ");
        return  userInput == "y" || userInput == "quit";
    }
}