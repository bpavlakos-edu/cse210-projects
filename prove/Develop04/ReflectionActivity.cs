class ReflectionActivity : Activity
{
    //New Attributes
    List<string> _refQuestions;

    //Constructor
    public ReflectionActivity():base()
    {

    }
    //Fill All Attributes
    public ReflectionActivity(string name, string description, List<string>messageList, int pauseStyle, List<string> refQuestions):base(name, description, messageList, pauseStyle)
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

    public override void Loop(int durationMsec)
    {
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
        Console.Write("You may begin in: ");
        Pause(5000,1); //Countdown timer
        Console.Clear(); //Clear the console

        long[] tickTimes = GetTickStartEnd(durationMsec); //Get the start and end times
        long curTime = tickTimes[0]; //Initalize curTime to the start time
        
        List<string> usedPrompts = new List<string>(); //List to store the used prompts
        int cyclePauseMsec = 10 * 1000; //each prompt should get a 10 second pause
        bool lastCycle = false;
        while (curTime < tickTimes[1])
        {
            if(curTime + (cyclePauseMsec * 10000) >= tickTimes[1])//Check if this is the last cycle
            {
                lastCycle = true;
                Console.WriteLine("");
            }
            //Display the prompt
            string curPrompt = GetRandomMsg(_refQuestions, usedPrompts); //Get a random question
            Console.Write($"> {curPrompt} ");
            //Console.Write($"> {GetRandomMsg(_refQuestions, usedPrompts)} ");//Single Line version
            Pause(cyclePauseMsec, 0);//Spinner pause type
            if(!lastCycle)
            {
                Console.WriteLine("");//Only print a line on every cycle except the last
            }
        }
    }
}