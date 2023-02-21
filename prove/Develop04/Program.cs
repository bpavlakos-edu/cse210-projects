//Mindfullness program
//Week 08

using System;

class Program
{
    static BreathingActivity bAct = new BreathingActivity("Breathing Activity","This activity will help you relax by walking your through breathing in and out slowly. Clear your mind and focus on your breathing.",new List<string>{"Breathe in...","Breathe out..."},1);
    //ReflectionActivity rAct = new ReflectionActivity();
    //ListingActivity lAct = new ListingActivity();
    static void Main(string[] args)
    {
        UiLoop();
    }
    static void UiLoop()
    {
        //Ui setup
        List<Action> UiActions = new List<Action>(); //Create a list of lambda function calls
        UiActions.Add(new Action(()=>{bAct.Run();}));//Breathing activity
        UiActions.Add(new Action(()=>{}));//Reflection activity
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