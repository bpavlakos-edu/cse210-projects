class ReflectionActivity : Activity
{
    //New Attributes
    private List<string> _refQuestions; //Private, because no other class extends this class

    //Constructor
    public ReflectionActivity() : base()
    {
        //ToList is handled by the base class constructor
    }
    //Fill All Attributes
    public ReflectionActivity(string name, string description, List<string>messageList, int spinnerStyle, List<string> refQuestions) : base(name, description, messageList, spinnerStyle)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

    //Getters and setters
    public List<string> GetRefQuestions()
    {
        return _refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }
    public void SetRefQuestions(List<string> refQuestions)
    {
        _refQuestions = refQuestions.ToList<string>(); //Use toList to erase the reference to the original list
    }

    public override long Loop(int durationMsec)
    {
        //Special input
        int cyclePauseSec = GetIntInput("How much time do you want to think about each question? (Enter 0 for default): ",0,120);
        if(cyclePauseSec == 0)
        {
            cyclePauseSec = 10;
        }

        TransitionLoad("Get ready...");//Get Ready

        //Pick and display the prompt
        Console.WriteLine("Consider the following prompt:");
        Console.WriteLine("");
        Console.WriteLine($" --- {GetRandomMsg(_messageList)} ---"); //Use GetRandomMessage to pick the prompt
        Console.WriteLine("");

        //Wait for user input to continue
        GetInput("When you have something in mind, press enter to continue.");
        Console.WriteLine("");

        //Prepare for main loop
        Console.WriteLine("Now ponder on each of the following questions as they are related to this experience.");
        PauseMsg("You may begin in: ",5000,1); //Countdown timer
        Console.Clear(); //Clear the console

        long[] tickTimes = GetTickStartEnd(durationMsec); //Get the start and end times
        long curTime = tickTimes[0]; //Initalize curTime to the start time
        
        //Loop setup
        List<string> usedPrompts = new List<string>(); //List to store the used prompts
        int cyclePauseMsec = cyclePauseSec * 1000; //each prompt should get a 10 second pause
        while (curTime < tickTimes[1])
        {
            bool lastCycle = (curTime + (cyclePauseMsec * 10000) >= tickTimes[1]); //Check if this is the last cycle
            if(lastCycle)
            {
                Console.WriteLine("Final Question:");
            }
            //Display the prompt
            string curPrompt = GetRandomMsg(_refQuestions, usedPrompts); //Get a random question
            //PauseMsg($"> {GetRandomMsg(_refQuestions, usedPrompts)} ",cyclePauseMsec, _spinnerStyle);//Single Line version
            PauseMsg($"> {curPrompt} ", cyclePauseMsec, base.GetSpinnerStyle()); //Spinner pause type
    
            //Use the spinner animation to generate the new line instead
            curTime = (DateTime.Now).Ticks; //Update the current time
        }
        Console.WriteLine("");
        
        //Return overtime
        return CalcOvertime(curTime,tickTimes[1]);
    }
}