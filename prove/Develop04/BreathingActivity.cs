//Activity Type 1
class BreathingActivity : Activity
{
    //Constructors
    public BreathingActivity() : base()
    {

    }
    //Fill all parameters
    public BreathingActivity(string name, string description, List<string> messageList, int pauseStyle) : base(name, description, messageList.ToList<string>(), pauseStyle)
    {

    }
    //Main functionality loop (attempting to override the original)
    public void loop(int durationMsec)
    {
        Console.WriteLine("Alternate Function Activated");
        Pause(1000,0);
    }
}