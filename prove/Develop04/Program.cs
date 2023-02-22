//Mindfullness program - CSE 210
//Week 08: 2/20/2023 - 2/21/2023
//Created By Bryce Pavlakos

/*
Ehancement list:
1. Created a UI system that processes User input, hotkeys, and runs functions based on their position in the list
2. Gave the program the ability to force unique prompts, and keep track of when to reset the list (listing activity only)
3. Gave the Activities the ability to detect the final cycle, and notify the user
4. Gave the program threaded spinners, and the ability for any Activity class to request a non-threaded animation display
5. Added threading to the listing activity, to prompt the user when the timer has expired.
6. Gave the program 6 additional spinners
7. Gave the program the ability to set the spinner type from the menu
8. Created a UI Menu Class that can be quickly implemented in other programs
9. Tracked the number of times a user picked an activity, and added an option to display them
10. Utilized the UiMenu Class to pick spinner types, as a way to demonstrate that UiMenus can be nested
11. Added time control to the breathing activity and Listing Activity
12. Gave the actvities the ability to display "Overtime" in the end sequence
13. Gave the user the option to disable threading, since it can be unpredictable on different hardware. Used the UiMenu to control it
Additional Improvements that could be made in the future:
* Activity Import and exporting from a sub menu (JSON serialization)
* Prompt Modification in the program from a sub menu that detects a class, and any members it might have
* Programically generating menu settings, or using functions to generate a repetative menu type as a template, that multiple places use and fill with parameters
* Add additional functionality to the UiMenu class, to make it even more useful
* Add the ability to restart an activity
*/

using System;

class Program
{
    //Global Activity Class Setup
    //List Items, Descriptions, and Names are from the directions: https://byui-cse.github.io/cse210-course-2023/unit04/develop.html
    static BreathingActivity bAct = new BreathingActivity
    (
        "Breathing Activity",
        "This activity will help you relax by walking your through breathing in and out slowly. Clear your mind and focus on your breathing.",
        new List<string>
        {
            "Breathe in...",
            "Breathe out..."
        },
        0//Default spinner
    );
    static ReflectionActivity rAct = new ReflectionActivity
    (
        "Reflection Activity",
        "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.",
        new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        }, 
        0, //Default spinner
        new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?"
        }
    );
    static ListingActivity lAct = new ListingActivity
    (
        "Listing Activity",
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.",
        new List<string>{
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        },
        0 //Default spinner
    );

    static int[] activityTracker = {0,0,0,0}; //Stat tracker
    
    //Global UI Class setup?
    //UiMenu UiMain = new UiMenu()
    //Setup items
    //UiMain.AddOption(new UiOption(HotkeyString, Action));
    static void Main(string[] args)
    {

        //UI Setup
        UiMenu UserInterface = new UiMenu
        (
            new List<UiOption>()
            {
                new UiOption(new Action(()=>{StartActivity(0);}),"Start [B]reathing Activity","b"),
                new UiOption(new Action(()=>{StartActivity(1);}),"Start [R]eflection Activity","r"),
                new UiOption(new Action(()=>{StartActivity(2);}),"Start [L]isting Activity","l"),
                // new UiOption(new Action(()=>{StartActivity(2);}),"Start S[p]inner Activity","p"),
                new UiOption(new Action(()=>{SetSpinnerStyleMenu();activityTracker[3]++;}),"[S]et Spinner Style","s"), //Its not really an activity! But we are tracking it for fun
                new UiOption(new Action(()=>{ThreadingOptionMenu();}),"Enable / Disable [T]hreading","t"), //Its not really an activity! But we are tracking it for fun
                new UiOption(new Action(()=>{DisplayStats();}),"[D]isplay Activity Statistics","d"),
                new UiOption(new Action(()=>{throw new OperationCanceledException();}),"[Q]uit Program","q")
            }
        );
        UserInterface.UiLoop(); //Start the custom class UI loop
        
        //Old way to setup the UI

        //Ui setup
        /*
        List<Action> uiActions = new List<Action>(); //Create a list of lambda function calls
        uiActions.Add(new Action(()=>{StartActivity(0);}));//Breathing activity
        uiActions.Add(new Action(()=>{StartActivity(1);}));//Reflection activity
        uiActions.Add(new Action(()=>{StartActivity(2);}));//Listing Activity Start
        uiActions.Add(new Action(()=>{SetSpinnerStyle();activityTracker[3]++;}));//Set the spinner style //Additional functionality
        uiActions.Add(new Action(()=>{DisplayStats();}));
        uiActions.Add(new Action(()=>{throw new OperationCanceledException();})); //Quit
        List<string> optionNames = new List<string>{"Start [B]reathing Activity","Start [R]eflection Activity","Start [L]isting Activity","[S]et Spinner Style","[D]isplay Activity Statistics","[Q]uit Program"};
        List<string> hotkeyList = new List<string>{"b","r","l","s","d","q"};

        UiMenu UserInterface = new UiMenu(uiActions,optionNames,hotkeyList); //New Custom class
        UserInterface.UiLoop(); //Start the custom class UI loop
        */
    }

    static void StartActivity(int activityIndex)
    {
        if(activityIndex == 0)
        {
            bAct.Run(true);
        }
        else if(activityIndex == 1)
        {
            rAct.Run(true);
        }
        else if(activityIndex == 2)
        {
            lAct.Run();
        }
        activityTracker[activityIndex]++; //Increase the counter
    }

    static void DisplayStats()
    {
        Console.WriteLine("Activity Completions: ");
        Console.WriteLine($"{bAct.GetName()}: {activityTracker[0]}");
        Console.WriteLine($"{rAct.GetName()}: {activityTracker[1]}");
        Console.WriteLine($"{lAct.GetName()}: {activityTracker[2]}");
        Console.WriteLine($"Opened Spinner Settings: {activityTracker[3]}");
        GetInput("Press Enter to Continue");
    }
    
    static void TestActivity()
    {
        Activity testActivity = new Activity("My Activity","Description goes here",new List<string>{"Item A","Item B","Item C"},1);
        testActivity.Run();
    }

    static void SetSpinnerStyle()
    {
        string[] spinnerNames = {"Regular","Reverse","Pointer","Reverse Pointer","Weird","Math"};
        int[] spinnerIds = {0,3,4,5,6,7};

        Console.WriteLine("Please Select from the following options: ");
        for(int i=0;i<spinnerNames.Length;i++)
        {
            Console.WriteLine((i+1)+". "+spinnerNames[i]);
        }

        int newSpinner = GetIntInput("Which spinner would you like to choose? (Enter 0 to cancel): ",0,6);

        if(newSpinner != 0)
        {
            newSpinner = spinnerIds[newSpinner - 1]; //Get the actual spinner Id from the list
            bAct.SetSpinnerStyle(newSpinner);
            rAct.SetSpinnerStyle(newSpinner);
            lAct.SetSpinnerStyle(newSpinner);
            new Activity().ShowSpinner(newSpinner, 1500);
        }
        //Exit
    }

    //A complex example of the UiMenu class, it "returns" by updating a variable captured in the Action's Lambda function
    static void SetSpinnerStyleMenu()
    {
        int newSpinner = -1; //A variable to store the result of the menu (variable reference will be captured by lambda functions)
        //Create a new UI to initalize
        UiMenu spinnerMenu = new UiMenu
        (
            new List<UiOption>
            {   //Each option should change the new spinner value, and then throw the exception to exit
                new UiOption(new Action(()=>{newSpinner = 0; throw new OperationCanceledException();}),"[R]egular","e"), 
                new UiOption(new Action(()=>{newSpinner = 3; throw new OperationCanceledException();}),"Re[V]erse","v"),
                new UiOption(new Action(()=>{newSpinner = 4; throw new OperationCanceledException();}),"Poin[T]er","t"),
                new UiOption(new Action(()=>{newSpinner = 5; throw new OperationCanceledException();}),"Rever[S]e Pointer","s"),
                new UiOption(new Action(()=>{newSpinner = 6; throw new OperationCanceledException();}),"[W]eird","w"),
                new UiOption(new Action(()=>{newSpinner = 7; throw new OperationCanceledException();}),"[M]ath","m"),
                new UiOption(new Action(()=>{newSpinner = 8; throw new OperationCanceledException();}),"[A]lphabet","a"),
                new UiOption(new Action(()=>{throw new OperationCanceledException();}),"[B]ack","b") //Equivalent to exit (don't change the spinner value)
            },
            "" //No Exit Message
        );
        spinnerMenu.UiLoop();//Trigger the loop
        if(newSpinner != -1)//Set the spinner if it's not the default value (which means exit)
        {
            bAct.SetSpinnerStyle(newSpinner);
            rAct.SetSpinnerStyle(newSpinner);
            lAct.SetSpinnerStyle(newSpinner);
            new Activity().ShowSpinner(newSpinner, 3500);
        }
        //Go back to the previous menu
    }

    //Toggle threading
    static void ThreadingOptionMenu()
    {
        bool newSetting = bAct.GetAllowThreading();
        UiMenu threadingMenu = new UiMenu
        (
            new List<UiOption>
            {   //Each option should change the new spinner value, and then throw the exception to exit
                new UiOption(new Action(()=>{newSetting = true; throw new OperationCanceledException();}),"[E]nable Threading","e"),
                new UiOption(new Action(()=>{newSetting = false; throw new OperationCanceledException();}),"[D]isable Threading","d"),
                new UiOption(new Action(()=>{throw new OperationCanceledException();}),$"Go [B]ack, and keep current setting: {DisabledEnabled(newSetting,true)}","b") //Equivalent to exit (don't change the spinner value)
            },
            "" //No exit message
        );
        threadingMenu.UiLoop();//Start the threading menu
        if(newSetting != bAct.GetAllowThreading())
        {
            //Update activities
            bAct.SetAllowThreading(newSetting);
            rAct.SetAllowThreading(newSetting);
            lAct.SetAllowThreading(newSetting);
            
            GetInput($"Threading {DisabledEnabled(newSetting)}, press enter to continue");//Let the user know the option changed
        }
        //Go back to the previous menu
    }

    //User input
    static string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }

    static int GetIntInput(string inMsg, int min = 0, int max = 0)
    {
        while(true) //Repeat until a valid number is found
        {
            //Catch parsing errors
            try 
            {
                int returnInt = int.Parse(GetInput(inMsg)); //Parse the user input
                //Determine if the current integer is a valid number
                if(min == max) //This means no minimum or maximum was set
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else if(returnInt <= max && returnInt >= min) //is the number between the minimum and maximum?
                {
                    return returnInt;//Exit the while loop by returning the value
                }
                else //Invalid number
                {
                    Console.WriteLine($"That's not a number between {min} and {max}, please try again!");
                }
            }
            catch(FormatException) //Not a number
            {
                Console.WriteLine($"That's not a valid whole number, please try again!");
            }
            catch(ArgumentNullException) //Empty input
            {
                Console.WriteLine("Please enter a number to continue!");
            }
            catch(OverflowException) //Overflow
            {
                Console.WriteLine("That's not a number the program can process, please try again!");
            }
        }
    }

    //String Helpers

    //Simple string helper, didn't want to repeat it twice, this allows it to be used for other settings too!

    static string DisabledEnabled(bool status, bool capitalize = false)
    {
        string disOrEn = "dis";
        if(status) //When it's enabled
        {
            disOrEn = "en";
        }
        if(!capitalize)
        {
            return $"{disOrEn}abled";
        }
        else
        {
            return $"{CapitalizeString(disOrEn)}abled";
        }
    }

    //Capitalize as string, why is this not built in?
    //Also string builders are what you use for doing this right: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#accessing-individual-characters
    static string CapitalizeString(string inputStr)
    {
        return inputStr.Substring(0,1).ToUpper()+inputStr.Substring(1).ToLower();
    }
}