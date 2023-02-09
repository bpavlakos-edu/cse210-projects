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
        int hideNum = 3;//Controls the number of words to hide
        while(1 == 1)
        {
            //Display sequence
            Console.Clear(); //Clear the console
            Console.WriteLine(RandomNumToString(hideNum)+"");
            myScripture.Display(); //Trigger the scripture display function
            Console.WriteLine();//Blank Line
            string userInput = GetInputLine("Press enter to continue, type a number to adjust the difficulty (negative for a random maximum), type 'reset' to reset, type 'change' to change the scripture, or type 'quit' to finish:");
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
                //Handle numbers
                try
                {
                    int newHideNum = int.Parse(userInput); //Try to get the new hide number by parsing user input
                    if(newHideNum == 0)
                    {
                        throw new ArgumentNullException(); //0 is ignored, just treat it like pressing enter
                    }
                    hideNum = newHideNum; //Store the new number
                }
                catch(OverflowException)
                {
                    throw new ArgumentNullException(); //Error, just treat it like pressing enter
                }
                catch(FormatException)
                {
                    throw new ArgumentNullException(); //Error, just treat it like pressing enter
                }
                catch(ArgumentNullException) //All exceptions will flow here
                {
                    //Unrecognized text input == initalize next turn
                    if(myScripture.HideWords(hideNum) == false)
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

    //Function to confirm the user is empty
    static bool ConfirmExit()
    {
        string userInput = GetInput("Would you like to quit? (enter 'y' or 'quit' to finish, enter anything else to reset the current scripture): ");
        return userInput == "y" || userInput == "quit"; //Return if the user input matches any of the exit keywords
    }
    static void SetScripture()
    {
        string newReference = GetInput("Enter the new scripture reference (press enter to cancel, type 'default' to load the default): ");
        if(newReference == "default") //Keyword to reset to the original
        {
            myScripture = new Scripture("Mosiah 2:17","And behold, I tell you these things that ye may learn wisdom; that ye may learn that when ye are in the service of your fellow beings ye are only in the service of your God.");
        }
        else if(newReference != "") //Scripture reference isn't empty
        {
            string newVerseText = GetInput("Enter the new scripture text (press enter to cancel): ", false);//Do not return lower case
            if(newVerseText != "") //New verse text isn't empty
            {
                myScripture = new Scripture(newReference, newVerseText);//Create the new scripture object
            }
            //Do nothing if it's blank
        }
        //Do nothing if it's blank
    }
    static string RandomNumToString(int hideNum)
    {
        if(hideNum > 0)
        {
            return "Words Removed Each Turn: "+hideNum;
        }
        else
        {
            //hideNum = myScripture.RandomHideMax(hideNum); //Should I write it here too?
            return "Words Removed Each Turn: Random, 1 to "+(myScripture.RandomHideMax(hideNum)-1); //Negative numbers mean a maximum ceiling
        }
    }
}