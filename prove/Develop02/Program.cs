using System;
using System.IO;
using System.Windows.Input;
using System.Text.Json;
class Program
{
    //Global variables
    static Journal myJournal = new Journal();
    static int unsavedChanges = 0;
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Journal Program!");
        MenuLoop();//Initialize the menu
    }
    static void MenuLoop(){
        //string userInput = "";
        while(1 == 1)
        {
            DisplayOptions();
            string uInput = GetStrInput("What would you like to do? ").ToLower(); //Get user input

            //Trigger valid UI functions
            
            /*This could be turned into a dynamic menu by storing menu options in a custom class
            This class would store all options and hotkeys inside itself, and its behavior
            This class could also be nested inside itself with a "trigger" function that is overridden during class declaration (or a function passed as a parameter) to make triggerable UI links
            Or this could also be done using a 2D list, where each entry has 3 items: 
            0: it's full name 
            1: It's "hotkey" 
            3: It's function*/
            //But it's much "simpler" just to use an if statement because there's no UI nesting 
            if(uInput == "1" || uInput == "w")
            {
                //Write
                myJournal.WriteNewEntry();
                unsavedChanges += 1;
            }
            else if(uInput == "2" || uInput == "d")
            {
                //Display
                myJournal.Display();
            }
            else if(uInput == "3" || uInput == "l")
            {
                //Load from JSON
                string filePath = GetStrInput("Enter the file name you want to load the journal data from (leave blank to cancel): ");
                if(filePath != "")
                {
                    try
                    {
                        myJournal.FromJson(ReadFileLines(filePath));
                        Console.WriteLine("Journal Data Loaded from file");
                    }
                    catch(FileNotFoundException e)
                    {
                        Console.WriteLine($"{filePath} cannot be located, failed to load data");
                    }
                    catch(IOException e)
                    {
                        Console.WriteLine($"Error {e.ToString()}"); //Print the exception
                    }
                    catch(JsonException e)
                    {
                        Console.WriteLine($"Error {e.ToString()}"); //Print the exception
                    }
                    
                }
            }
            else if(uInput == "4" || uInput == "s")
            {
                //Save as JSON
                string filePath = GetStrInput("Enter the file name you want to save the journal data to (leave blank to cancel): ");
                if(filePath != "")
                {
                    try
                    {
                        WriteToFile(filePath, myJournal.ToJson());
                        Console.WriteLine("Journal Data Saved to file");
                        unsavedChanges = 0;
                    }
                    catch(IOException e)
                    {
                        Console.Write($"Error {e.ToString()}"); //Print the exception
                    }
                    catch(JsonException e)
                    {
                        Console.Write($"Error {e.ToString()}"); //Print the exception
                    }
                }
            }
            else if(uInput == "5" || uInput == "e")
            {
                //Export as text file
                string filePath = GetStrInput("Enter the file name you want to export the journal text to (leave blank to cancel): ");
                if(filePath != "")
                {
                    WriteToFile(filePath, myJournal.ToString());
                    Console.WriteLine("Journal Data Exported to file");
                }
            }
            else if(uInput == "6" || uInput == "c")
            {
                myJournal.SetOwnerName(); //Change journal owner name
                unsavedChanges += 1;
            }
            else if(uInput == "7" || uInput == "q")
            {
                //Quit
                if(unsavedChanges != 0)
                {
                    string quitConfirmInput = GetStrInput($"Are you sure you would like to quit? You have {GetUnsavedChanges()} (Enter \"Y\" to quit): ").ToLower();
                    if(quitConfirmInput == "y")
                    {
                        break; //Exit the while loop
                    }
                }
                else
                {
                    break; //Exit the while loop
                }
            }
            else
            {
                //Unrecognized input
                Console.WriteLine("Sorry, that's not a recognized option, please try again!");
            }
        }
    }

    //File reading / writing functions
    //Write a line to a file
    //Could be changed to return a boolean that stores if writing was successful
    public static void WriteToFile(string filePath, string writeString, bool newLine = false)
    {
        //Try catch was explained here:
        // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/try-catch
        //Catch IO errors
        try
        {
            //Stream writers explained in the following locations:
            // https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-7.0
            //Code helps section of: https://byui-cse.github.io/cse210-course-2023/unit02/develop.html
            using(StreamWriter sWriter = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                if(newLine)
                {
                    writeString += Environment.NewLine; //add the newline character onto the end if newline is true (which it likely won't be)
                }
                sWriter.Write(writeString);//Write the message
            }
        }
        catch(IOException e)
        {
            Console.Write($"Error {e.ToString()}"); //Print the exception
        }
    }

    static string ReadFileLines(string filePath)
    {
        using(StreamReader sReader = new StreamReader(filePath))
        {
            return sReader.ReadToEnd();
        }
    }

    //Utility functions
    //Get a string from the console
    //Public has been added to this function to enable reuse inside other classes, it can be static since it's a string
    public static string GetStrInput(string inMsg, bool newLine = false){
        if(newLine)
        {
            inMsg += Environment.NewLine;
        }
        Console.Write(inMsg);
        return Console.ReadLine();
    }

    //Display static options for the menu
    static void DisplayOptions(){
        Console.WriteLine("Please select one of the following choices or [H]otkeys:");
        Console.WriteLine("1. [W]rite");
        Console.WriteLine("2. [D]isplay");
        Console.WriteLine("3. [L]oad Journal Data");
        Console.WriteLine($"4. [S]ave Journal Data{GetUnsavedChanges(new string[] {" (",")"})}");
        Console.WriteLine("5. [E]xport as Text File");
        Console.WriteLine("6. [C]hange Journal Owner Name");
        Console.WriteLine("7. [Q]uit");
    }

    //Java-style function overload
    static string GetUnsavedChanges(){
        return GetUnsavedChanges(new string[]{"",""});
    }
    static string GetUnsavedChanges(string[] cChars)
    {
        if(unsavedChanges > 1)
        {
            return cChars[0]+unsavedChanges+" Unsaved Changes"+cChars[1];
        }
        else if (unsavedChanges == 1)
        {
            return cChars[0]+"1 Unsaved Change"+cChars[1];
        }
        return ""; //Else return nothing
    }
}