using System;

class Program
{
    public static Scripture myScripture = new Scripture("Mosiah 2:17","And behold, I tell you these things that ye may learn wisdom; that ye may learn that when ye are in the service of your fellow beings ye are only in the service of your God.");
    static void Main(string[] args)
    {
        UiLoop(); //Trigger the main UI loop
    }
    static void UiLoop()
    {
        while(1 == 1)
        {
            //Display sequence
            Console.Clear(); //Clear the console
            myScripture.Display(); //Trigger the scripture display function
            Console.WriteLine();//Blank Line
            string userInput = GetInputLine("Press enter to continue, type 'reset' to reset, type 'change' to change the scripture, or type 'quit' to finish:");
            if(userInput == "quit")
            {
                break;//Quit the loop
            }
            else if(userInput == "reset")
            {
                myScripture.Reset(); //Reset the scripture
            }
            else if(userInput == "change")
            {
                SetScripture(); //Change the scripture
            }
            else
            {
                //Unrecognized input == initalize next turn
                if(myScripture.HideWords() == false)
                {
                    //The next turn failed, initalize exit sequence
                    if(ConfirmExit())
                    {
                        break; //Quit the loop
                    }
                    else
                    {
                        myScripture.Reset(); //Reset the scripture
                    }
                }
            }
        }
    }
    //New line function overload created to match original project video (https://byui-cse.github.io/cse210-course-2023/unit03/develop.html)
    public static string GetInputLine(string inMsg="", bool returnLowerCase = true)
    {
        return GetInput(inMsg+Environment.NewLine, returnLowerCase); //Append the system newline to the message
    }
    public static string GetInput(string inMsg="", bool returnLowerCase = true)
    {
        Console.Write(inMsg); //Write the message
        string userInput = Console.ReadLine(); //Get the user input
        if(returnLowerCase)
        {
            return userInput.ToLower(); //Return lower case input
        }
        //We can't reach this code if returnLowerCase is true
        return userInput; //Return the raw input
    }
    static bool ConfirmExit()
    {
        string userInput = GetInput("Would you like to quit? (enter 'y' or 'quit' to finish, enter anything else to reset the current scripture): ");
        return userInput == "y" || userInput == "quit"; //Return if the user input matches any of the exit keywords
    }
    static void SetScripture()
    {
        string newReference = GetInput("Enter the new scripture reference (press enter to cancel, type 'default' to load the default): ");
        if(newReference == "default") //
        {
            myScripture = new Scripture("Mosiah 2:17","And behold, I tell you these things that ye may learn wisdom; that ye may learn that when ye are in the service of your fellow beings ye are only in the service of your God.");
        }
        else if(newReference != "") //Scripture reference isn't empty
        {
            string newVerseText = GetInput("Enter the new scripture text (press enter to cancel): ", false);//Do not return lower case
            if(newVerseText != "") //New verse text isn't empty
            {
                myScripture = new Scripture(newReference, newVerseText);
            }
            //Do nothing if it's blank
        }
        //Do nothing if it's blank
    }
}