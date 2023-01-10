using System;

class Program
{
    static void Main(string[] args)
    {
        DisplayWelcome();
        DisplayResult(PromptUserName(),SquareNumber(PromptUserNumber()));
    }


    //Required Functions

    //"DisplayWelcome - Displays the message, "Welcome to the Program!""
    static void DisplayWelcome()
    {
        Console.WriteLine("Welcome to the Program");
    }
    //PromptUserName - Asks for and returns the user's name (as a string)
    static string PromptUserName()
    {
        return GetInput("Please enter your name:");
    }
    //PromptUserNumber - Asks for and returns the user's favorite number (as an integer)
    static int PromptUserNumber()
    {
        while (1 == 1)
        {
            try
            {
                return int.Parse(GetInput("Please enter your favorite number:"));
            }
            catch
            {
                Console.WriteLine("That is not a valid whole number, please try again!");
            }
        }
    }
    //SquareNumber - Accepts an integer as a parameter and returns that number squared (as an integer)
    static int SquareNumber(int inNum)
    {
        return inNum * inNum;
    }
    //DisplayResult - Accepts the user's name and the squared number and displays them.
    static void DisplayResult(string userName, int squaredNumber)
    {
        Console.WriteLine($"{userName}, the square of your favorite number is {squaredNumber}");
    }

    //Additional Functions
    //The core function to get user input
    static string GetInput(string inMsg)
    {
        Console.Write(inMsg+" ");//Add the space by default to prevent redunant work
        return Console.ReadLine();
    }
}