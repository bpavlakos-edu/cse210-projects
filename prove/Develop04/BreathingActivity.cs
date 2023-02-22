//Activity Type 1
class BreathingActivity : Activity
{
    //Constructors
    public BreathingActivity() : base()
    {

    }
    //Fill all parameters
    public BreathingActivity(string name, string description, List<string> messageList, int spinnerStyle) : base(name, description, messageList, spinnerStyle)
    {
        //ToList is handled by the base class constructor
    }
    //Main functionality Loop (attempting to override the original)
    //Override documentation found here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override#example
    //Finally understood it by looking at this page: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#1465-override-methods
    //And also noticing that I had the wrong function name!!!
    public override long Loop(int durationMsec)
    {
        int cycleTime = GetIntInput("Enter how long you you will hold a breath (in seconds): ",1,15) * 1000; //Custom input for breathing length
        TransitionLoad("Get ready...");
        _allowThreading = false;

        long[] tickTimes = GetTickStartEnd(durationMsec);
        long curTime = tickTimes[0]; //Get the start time
        
        //Main sequence loop
        while(curTime < tickTimes[1])
        {
            bool lastCycle = (curTime + ((2 * cycleTime) * 10000) >= tickTimes[1]);
            if(lastCycle) //Check for final set
            {
                Console.WriteLine("Final Set!");
            }
            //Breathe in
            PauseMsg(_messageList[0]+" ",cycleTime,1);
            //Breathe out
            PauseMsg(_messageList[1]+" ",cycleTime,1);
            Console.WriteLine(""); //Next step (Changed to flush buffer before next spinner)

            curTime = (DateTime.Now).Ticks; //Update timer
        }
        Console.Clear(); //Flush buffer before exit!
        _allowThreading = true; //Re-enable threading
        
        //Return overtime
        return CalcOvertime(curTime,tickTimes[1]);
    }
}