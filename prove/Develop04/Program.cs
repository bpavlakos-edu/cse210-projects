//Mindfullness program
//Week 08

using System;

class Program
{
    //BreathingActivity bAct = new BreathingActivity();
    //ReflectionActivity rAct = new ReflectionActivity();
    //ListingActivity lAct = new ListingActivity();
    static void Main(string[] args)
    {
        //TestActivity();
        UiLoop();
    }
    static void UiLoop()
    {
        //Ui setup
        List<Action> UiActions = new List<Action>(); //Create a list of lambda function calls
        UiActions.Add(new Action(()=>{}));//Breathing activity
        UiActions.Add(new Action(()=>{}));//Reflection activity
        UiActions.Add(new Action(()=>{}));//Listing Activity Start
        //Additional functionality
        UiActions.Add(new Action(()=>{throw new OperationCanceledException();})); //Quit
        List<String> optionName = new List<string>{"[B]reathing Activity","[R]eflection Activity","[L]isting Activity","[Q]uit"};
        List<String> hotkeyList = new List<string>{"b","r","l","q"};

        //Ui Loop
        try
        {
            while(true)
            {
                //Display the UI options
                for(int i=0; i<optionName.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {optionName[i]}");
                }
                Console.WriteLine("");
                string userInput = GetInput("Please select an option.").ToLower();
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
        catch(OperationCanceledException) //Intentional exit exception 
        {
            //Do nothing
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