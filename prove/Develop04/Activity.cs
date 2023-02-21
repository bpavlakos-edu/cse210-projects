using System.Collections;
//Super Class - Activity: Contains core functionality for all Activity Sub Classes

class Activity
{
    //Attributes
    protected string _name;
    protected string _description;
    protected List<string> _messageList;
    protected int _pauseStyle;

    //Constructors

    //Fill Attribute Constructor
    public Activity(string name, string description, List<string> messageList, int pauseStyle)
    {
        _name = name;
        _description = description;
        _messageList = messageList.ToList<string>();
        _pauseStyle = pauseStyle;
    }

    //Getters and Setters
    public string GetName()
    {
        return _name;
    }
    public void Set(string name)
    {
        _name = name;
    }
    public string GetDescription()
    {
        return _description;
    }
    public void SetDescription(string description)
    {
        _description = description;
    }
    public List<string> GetMessageList()
    {
        return _messageList.ToList<string>(); //Clone the list so the original isn't changed
    }
    public void SetMessageList(List<string> messageList)
    {
        _messageList = messageList.ToList<string>(); //Clone the list so the input list isn't changed
    }
    public int GetPauseStyle()
    {
        return _pauseStyle;
    }
    public void SetPauseStyle(int pauseStyle)
    {
        _pauseStyle = pauseStyle;
    }

    //Methods

    //Main Functionality Flow (Run and loop are Placeholders/templates for subclass overrides)
    public void Run()
    {
        int durationMsec = ShowIntro();
        Loop(durationMsec);
        End(durationMsec);
    }

    protected void Loop(int durationMsec)
    {
        GetInput("Press enter to start");
        long curTime = (DateTime.Now).Ticks;
        long endTime = curTime + (durationMsec * 10000); //There are 10000 ticks in a milisecond according to the docs: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0
        
        int delayDurationMsec = 2000;
        while(curTime < endTime)
        {
            Console.WriteLine("Test message");
            
            Pause(delayDurationMsec,_pauseStyle); //Request a pause using this activity's pause type
            curTime += 10000 * delayDurationMsec;
        }

        //Extra end code can go here
    }

    protected void End(int durationMsec)
    {
        Console.WriteLine("Well done!");
        TransitionLoad($"You have completed another {durationMsec / 1000} seconds of {_name}");
    }

    //Intro and outro helpers
    protected int ShowIntro()
    {
        //Messages are matching the syntax as shown in the example here: https://byui-cse.github.io/cse210-course-2023/unit04/develop.html
        //Print Welcome
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine("");
        //Print Description
        Console.WriteLine(_description);
        Console.WriteLine("");
        //Get the duration to return it at the end
        int durationMsec = GetIntInput("How long, in seconds, would you like for your session? ") * 1000; //Remember, thread.sleep is in msec, but datetime is in ticks
        TransitionLoad("Get ready...");
        return durationMsec;
    }
    protected void TransitionLoad(string inMsg = "Get ready...", bool newLine = true)
    {
        Console.Clear();//Clear the console first
        //Newline Flag
        if(newLine)
        {
            inMsg += Environment.NewLine;
        }
        Console.Write(inMsg);//Write the message
        Pause(6000,0); //Get a spinner
        Console.Clear();//Clear the console again
    }
    //Pausing
    protected void Pause(int durationMsec, int pauseType)
    {
        //Ticks documentation: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.millisecond?source=recommendations&view=net-7.0
        long curTime = (DateTime.Now).Ticks;
        long endTime = curTime + (durationMsec * 10000); //There are 10000 ticks in a milisecond according to the docs: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0
        
        //Intellisense helped me figure out the syntax for this:
        Task animTask = new Task(new Action(async ()=>{await RequestAnimation(pauseType, durationMsec);})); //Start the async function
        //Wait and start were located here
        animTask.Start();
        /*while(curTime < endTime)
        {
            //Run animation?
            curTime = (DateTime.Now).Ticks; //Update current time
        }*/
        Thread.SpinWait(durationMsec);
        animTask.Wait();
        
    }
    

    //Animation helpers
    protected async Task RequestAnimation(int durationMsec, int pauseType, int fps = 60)
    {
        //Switch case in C#: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement
        switch (pauseType)
        {
            case 0:
                //Spinner
                ActivateLoopAnim(durationMsec, new List<string>{"-","\\","|","/"}, 250);
                break;
            case 1:
                //Count down timer
                ActivateAnimTimerMsec(durationMsec);
                break;
            case 2:
                //Count up timer
                ActivateAnimTimerMsec(durationMsec, false);
                break;
            default:
                //No display (such as -1 which means the loop handles the request)
                break;
        }
    }

    //Activate a loop animation
    private void ActivateLoopAnim(int durationMsec, List<string> frameChars, int msecPerFrame)
    //Async Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async
    //Documentation for generic lists (list<object>): https://learn.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=net-7.0#remarks
    {
        int frameLen = frameChars.Count;
        int maxFrame = (durationMsec / msecPerFrame); //Automatically calculate the maximum frame

        //Console.Write(frameChars[0]);//Add an extra space to overrwrite
        Console.Write(" ");//Add an extra space to overrwrite by a frame
        for(int curFrame= 0; curFrame <= maxFrame; curFrame++)
        {
            DisplayFrame(frameChars[curFrame % frameLen], msecPerFrame); //Increment current frame, and ensure it doesn't exceed the animation length
        }
    }
    //Countdown timer or just a regular timer
    private void ActivateAnimTimer(int durationSec, bool countdownFlag = true)
    {
        Console.Write(" ");//Add an extra space to overrwrite by a frame
        for(int i = 0; i < durationSec; i++)
        {
            if(countdownFlag) //Countdown
            {
                DisplayFrame(durationSec - i, 1000);
            }
            else //Count up timer
            {
                DisplayFrame(i, 1000);
            }
            
            //3-line version:
            //int boolInt = BitConverter.ToInt32(BitConverter.GetBytes(countdownFlag)); //Convert the boolean to an integer //From: https://learn.microsoft.com/en-us/dotnet/api/system.boolean?view=net-7.0#work-with-booleans-as-binary-values
            //int iValue = (boolInt * durationSec) + (i * (1 - (2 * boolInt))); //Deactivate durationSec when false, flip i negative when true
            //DisplayFrame(iValue, 1000); //Display the result
        }
    }
    //Function overload for using MSEC instead of seconds
    private void ActivateAnimTimerMsec(int durationMsec, bool countdownFlag = true)
    {
        ActivateAnimTimer(durationMsec / 1000, countdownFlag);
    }

    //Display a single animation frame, accepts objects so that numbers are handled too
    private void DisplayFrame(object frameObj, int msecPerFrame)
    {
        Console.Write("\b \b"); //Backspace to clear
        Console.Write(frameObj); //Write this frame
        Thread.Sleep(msecPerFrame); //Make the tread sleep
    }
    //Function overload for display frame for code completion to recognize
    private void DisplayFrame(string frameStr, int msecPerFrame)
    {
        DisplayFrame(frameStr, msecPerFrame);
    }

    //Pick an item from the list at random
    public string GetRandomMsg(List<string> selectionList)
    {
        if(selectionList.Count > 1)
        {
            return selectionList[new Random().Next(selectionList.Count())];
        }
        else if(selectionList.Count == 1)
        {
            return selectionList[0];
        }
        else //Empty list
        {
            return "";
        }
    }

    //Overload to handle a second list input, which keeps track of which items have been used already
    public string GetRandomMsg(List<string> selectionList, List<string> removeList)
    {
        List<string> selectionListCopy = selectionList.ToList<string>();//Copy the list so we don't modify the original
        //Remove all instances of the remove list values
        for (int i =0;i<removeList.Count;i++)
        {
            while(selectionListCopy.Contains(removeList[i]))
            {
                selectionListCopy.Remove(removeList[i]);
            }
        }
        if(selectionListCopy.Count == 0) //Selection list has no items
        {
            removeList = new List<string>(); //Reset the remove list (it's mutable so we can affect it from here)
            return GetRandomMsg(selectionList); //Use the original list as the call instead 
        }
        else
        {
            return GetRandomMsg(selectionListCopy);//Call the function using
        }
        
    }

    //User input
    //Get a generic input from the user
    public string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    //Get a number from a user
    public int GetIntInput(string inMsg, int min = 0, int max = 0)
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

    

}