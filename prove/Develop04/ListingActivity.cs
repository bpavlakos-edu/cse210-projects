class ListingActivity : Activity
{
    //Hidden attributes
    private bool _loopExited = false;
    //Constructors
    public ListingActivity() : base()
    {

    }
    //Fill all attributes
    public ListingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name,description, messageList, pauseStyle)
    {
        //ToList is handled by the base class constructor
    }

    //Methods

    public override void Loop(int durationMsec)
    {
        Console.WriteLine("List as many responses as you can to the following prompt:");
        Console.WriteLine("");
        Console.WriteLine($" --- {GetRandomMsg(_messageList)} ---");//Use GetRandomMessage to pick the prompt

        PauseMsg("You may begin in: ",5000,1); //Countdown timer

        long[] tickTimes = GetTickStartEnd(durationMsec); //Get the start and end times
        long curTime = tickTimes[0]; //Initalize curTime to the start time

        //Loop setup
        int enterCounter = 0;
        while(curTime < tickTimes[1])
        {
            if(GetInput("> ") != "")
            {
                enterCounter++; //Count non empty strings
            }
            //Use the spinner animation to generate the new line instead
            curTime = (DateTime.Now).Ticks; //Update the current time
        }
        ToggleLoopExited(); //Trigger the exit status
        //Handle "items" vs "item"
        string s_string = "s";
        if(enterCounter == 1)
        {
            s_string="";
        }
        Console.WriteLine($"You listed {enterCounter} item{s_string}!");
        //if(!_loopExited)
    }

    private bool ToggleLoopExited()
    {
        _loopExited = !_loopExited; //Invert the status of LoopExited, true -> false, false -> true
        return _loopExited; //Return the current status
    }

    private void TimerExpireNotification(int msecDelay)
    {
        Thread.Sleep(msecDelay);
        if(ToggleLoopExited())//_loopExited was false, but is now set to true (We can print the marker)
        {
            
        }
        //_loopExited was true, but is now set to false (We cannot print the marker the loop has finished)
    }
}