//Mindfullness program
//Week 08

using System;

class Program
{
    //Global Activity Class Setup
    static BreathingActivity bAct = new BreathingActivity(
        "Breathing Activity",
        "This activity will help you relax by walking your through breathing in and out slowly. Clear your mind and focus on your breathing.",
        new List<string>
        {
            "Breathe in...",
            "Breathe out..."
        },
        1
    );
    static ReflectionActivity rAct = new ReflectionActivity(
        "Reflection Activity",
        "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.",
        new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        }, 
        0, 
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
    //ListingActivity lAct = new ListingActivity();
    
    //Global UI Class setup?
    //UiMenu UiMain = new UiMenu()
    //Setup items
    //UiMain.AddOption(new UiOption(HotkeyString, Action));
    static void Main(string[] args)
    {
        UiLoop();
    }
    static void UiLoop()
    {
        //Ui setup
        List<Action> UiActions = new List<Action>(); //Create a list of lambda function calls
        UiActions.Add(new Action(()=>{bAct.Run(true);}));//Breathing activity
        UiActions.Add(new Action(()=>{rAct.Run();}));//Reflection activity
        UiActions.Add(new Action(()=>{}));//Listing Activity Start
        //Additional functionality
        UiActions.Add(new Action(()=>{throw new OperationCanceledException();})); //Quit
        List<String> optionName = new List<string>{"Start [B]reathing Activity","Start [R]eflection Activity","Start [L]isting Activity","[Q]uit Program"};
        List<String> hotkeyList = new List<string>{"b","r","l","q"};

        //Ui Loop
        try
        {
            while(true)
            {
                //Display the UI options
                Console.Clear();
                Console.WriteLine("Menu Options:");
                for(int i=0; i<optionName.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {optionName[i]}");
                }
                string userInput = GetInput("Select a choice or [hotkey] from the menu: ").ToLower();
                try
                {
                    try
                    {
                        int selectionIdx = int.Parse(userInput) - 1; //Get the action index
                        UiActions[selectionIdx].Invoke();//Activate the chosen option
                    }
                    catch (IndexOutOfRangeException) //Invalid index
                    {
                        throw new ArgumentNullException(); //Treat it like text input
                    }
                    catch (OverflowException) //Integer overflow
                    {
                        throw new ArgumentNullException(); //Treat it like text input
                    }
                    catch (FormatException) //Not a number
                    {
                        throw new ArgumentNullException(); //Treat it like text input
                    }
                    
                }
                catch(ArgumentNullException) //Funnel every error into ArgumentNullException
                {
                    //Text input
                    if(hotkeyList.Contains(userInput))
                    {
                        UiActions[hotkeyList.IndexOf(userInput)].Invoke(); //Activate the UI option
                    }
                }
            }
        } 
        //Title of: https://stackoverflow.com/questions/10226314/what-is-the-best-way-to-catch-operation-cancelled-by-user-exception helped me find this exception type using intellisense
        catch(OperationCanceledException) //Intentional exit exception, also what is run when the QUIT keyword is used
        {
            //Do nothing
            Console.WriteLine("Now exiting...");
        }
        
    }
    static string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }

    static void TestActivity()
    {
        Activity testActivity = new Activity("My Activity","Description goes here",new List<string>{"Item A","Item B","Item C"},1);
        testActivity.Run();
    }
}