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
    public Activity()
    {
        _name = "";
        _description = "";
        _messageList = new List<string>();
        _pauseStyle = -1;
    }

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
    public void Run(bool skipReady = false)
    {
        int durationMsec = ShowIntro();
        if(!skipReady) //Added a flag for the breathing activity to use
        {
            TransitionLoad("Get ready...");
        }
        Loop(durationMsec); //Start the main loop
        End(durationMsec); //Initalize the end sequence
    }

    //Override documentation found here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override#example
    public virtual void Loop(int durationMsec)
    {
        Console.WriteLine("My extra message will start here.");
        Console.WriteLine("");
        GetInput("Press enter to start");
        
        long[] tickTimes = GetTickStartEnd(durationMsec); //Store the start and end time
        long curTime = tickTimes[0]; //Item 0 is the current time in ticks
        
        int delayDurationMsec = 2000;
        List<string> previousMessageList = new List<string>(); //Keep track of what messages were used
        while(curTime < tickTimes[1])
        {
            Console.WriteLine("My test message");

            //Pick the next item
            string nextItem = GetRandomMsg(_messageList, previousMessageList); //Use the GetRandomMsg method that accepts a removal list as well
            
            Console.Write("Random List Item: "+ nextItem+" ");
            
            Pause(delayDurationMsec,_pauseStyle); //Request a pause using this activity's pause type
            curTime = (DateTime.Now).Ticks; //Update the current time
        }

        //Extra end behavior code can go here
    }

    protected void End(int durationMsec)
    {
        Console.WriteLine("");
        TransitionLoad("Well done!",true,false);
        TransitionLoad($"You have completed another {durationMsec / 1000} seconds of the {_name.ToLower()}.",true,false);
        //Ask if the user wants to restart
    }

    //Intro and outro helpers
    protected int ShowIntro()
    {
        Console.Clear();
        //Messages are matching the syntax as shown in the example here: https://byui-cse.github.io/cse210-course-2023/unit04/develop.html
        //Print Welcome
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine("");
        
        //Print Description
        Console.WriteLine(_description);
        Console.WriteLine("");
        
        //Get the duration to return it at the end
        int durationMsec = GetIntInput("How long, in seconds, would you like for your session? ") * 1000; //Remember, thread.sleep is in msec, but datetime is in ticks
        return durationMsec;
    }
    protected void TransitionLoad(string inMsg = "Get ready...", bool newLine = true, bool clearAllStart = true, int durationMsec=4000, bool clearAllEnd = true)
    {
        //Flag to clear console at start
        if(clearAllStart)
        {
            Console.Clear();//Clear the console first
        }
        //Newline Flag
        if(newLine)
        {
            inMsg += Environment.NewLine;
        }
        PauseMsg(inMsg,durationMsec,0);//Write the message //Get a spinner
        if(clearAllEnd) //Added a flag for this functionality
        {
            Console.Clear();//Clear the console at the end
        }
    }
    //Pausing
    protected void Pause(int durationMsec, int pauseType)
    {
        //Async was removed because of the bugs it created

        //Non-threaded, for debugging
        RequestAnimation(durationMsec, pauseType);
        
        /*
        //Threaded version is WIP
        Thread animThread = new Thread(()=>{RequestAnimation(durationMsec, pauseType);});
        animThread.Start();
        
        //Thread.Sleep(durationMsec); //Delay for specified time
        
        //Use a seperate thread to wait
        Thread delayTimer = new Thread(()=>{Thread.Sleep(durationMsec);}); 
        delayTimer.Start();//Start the timer thread
        //Record the actual timer desync
        bool timerSync = delayTimer.Join(durationMsec); //Using Join with a time parameter will make it timeout after a specified time, it returns the status on completion or timeout
        long delayOffset = (DateTime.Now).Ticks; //Record then the delay timed out
        if(!timerSync) //The timer timed out!
        {
            delayTimer.Join(); //Wait for the timer to end
            delayOffset = ((DateTime.Now).Ticks) - delayOffset; //Record how long it took to re-sync
        }
        else
        {
            delayOffset = 0;
        }

        //Animation Termination Detection
        //Join with timespan https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.join?view=net-7.0#system-threading-thread-join(system-timespan)
        TimeSpan graceWindow = new TimeSpan(delayOffset);
        bool terminated = animThread.Join(graceWindow);//Order it to join in 0 msec
        if(!terminated)
        {
            animThread.Interrupt(); //Forcibly end a thread early
        }

        //Console.Beep(); //This fixes it completely!!!
        */
    }

    //Combining Pause with a console write
    protected void PauseMsg(string inMsg, int durationMsec, int pauseType)
    {
        Console.Write(inMsg);
        Pause(durationMsec, pauseType);
        Console.WriteLine("");
    }

    //Pick an item from the list at random
    protected string GetRandomMsg(List<string> selectionList)
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
    protected string GetRandomMsg(List<string> selectionList, List<string> removeList)
    {
        if(selectionList.Count > 0) //Accept lists with a item count greater than 0
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
            string returnStr = "";
            if(selectionListCopy.Count == 0) //Selection list has no items
            {
                Console.WriteLine("Resetting remove list!"); //Debugging
                removeList = new List<string>(); //Reset the remove list (it's mutable so we can affect it from here)
                returnStr = GetRandomMsg(selectionList); //Use the original list as the call instead 
            }
            else //We can choose a random item
            {
                returnStr = GetRandomMsg(selectionListCopy);//Call the function using
            }
            removeList.Add(returnStr); //Regardless of what happens, add the newly picked string to the removeList, which will change the original list because of mutability
            return returnStr; //Return the return string
        }
        else //Return "" for 0 length strings
        {
            return "";
        }
    }

    //Calcuations
    //Pointlessly created a function to do this, in the hopes that the code that uses it would be one line, instead I just made a way to get a start and end duration from miliseconds
    protected long[] GetTickStartEnd(int durationMsec)
    {
        //Ticks documentation: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.millisecond?source=recommendations&view=net-7.0
        long curTime = (DateTime.Now).Ticks;
        long endTime = curTime + (durationMsec * 10000); //There are 10000 ticks in a milisecond according to the docs: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=net-7.0
        return new long[]{curTime, endTime};
    }

    //User input
    //Get a generic input from the user
    protected string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    //Get a whole number from a user (supports minimum and maximum)
    protected int GetIntInput(string inMsg, int min = 0, int max = 0)
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

    //Animation helpers
    protected void RequestAnimation(int durationMsec, int pauseType, int fps = 60)
    {
        //There is an error here, saying that because I'm not using "await" this is not asyncronous, but I am calling it using an await from a task action
        //Using Await on this method's call would require adding "async" to the Pause() method, which is not practical
        //Console.WriteLine($"Requesting animation type {pauseType}");
        //Switch case in C#: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/selection-statements#the-switch-statement
        switch (pauseType)
        {
            //Found the syntax for task factory on this stack overflow post: https://stackoverflow.com/a/12144426 (Main post: https://stackoverflow.com/questions/12144077/async-await-when-to-return-a-task-vs-void)
            case 0:
                //Spinner
                //await Task.Factory.StartNew(() => {ActivateLoopAnim(durationMsec, new List<string>{"-","\\","|","/"}, 250);});
                ActivateLoopAnim(durationMsec, new List<string>{"-","\\","|","/"}, 250);
                //ActivateLoopAnim(durationMsec, new List<string>{"1","2","3","4"}, 250);//Debugging
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
                //Do nothing
                break;
        }
    }

    //Activate a loop animation
    private void ActivateLoopAnim(int durationMsec, List<string> frameChars, int msecPerFrame)
    //Async Documentation: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async
    //Documentation for generic lists (list<object>): https://learn.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=net-7.0#remarks
    {
        //Replaced With Threaded Version
        int frameLen = frameChars.Count;
        int maxFrame = (durationMsec / msecPerFrame); //Automatically calculate the maximum frame

        //Console.Write(frameChars[0]);//Add an extra space to overrwrite
        //Console.Write("A");//Add an extra space to overrwrite by a frame
        for(int curFrame = 0; curFrame < maxFrame; curFrame++)
        {
            bool frameShown = DisplayFrame(frameChars[curFrame % frameLen], msecPerFrame); //Increment current frame, and ensure it doesn't exceed the animation length
            if(!frameShown) //The last frame was not displayed properly
            {
                curFrame = maxFrame;//Exit the loop
            }
        }
        lastFrameLengthMem = 0; //Animation has completed, reset the global variable
    }
    //Countdown timer or just a regular timer
    private void ActivateAnimTimer(int durationSec, bool countdownFlag = true)
    {
        //Replaced With Threaded Version
        for(int i = 0; i < durationSec; i++)
        {
            //Console.Write(" ");//Add an extra space to overrwrite by a frame
            //Console.Beep();
            if(countdownFlag) //Countdown
            {
            
                bool frameShown = DisplayFrame(durationSec - i, 1000);
                if(!frameShown) //The last frame was not displayed properly
                {
                    i = durationSec;//Exit the loop
                }
                
            }
            else //Count up timer
            {
                bool frameShown = DisplayFrame(i, 1000);
                if(!frameShown) //The last frame was not displayed properly
                {
                    i = durationSec;//Exit the loop
                }
            }
            
            //3-line version:
            //int boolInt = BitConverter.ToInt32(BitConverter.GetBytes(countdownFlag)); //Convert the boolean to an integer //From: https://learn.microsoft.com/en-us/dotnet/api/system.boolean?view=net-7.0#work-with-booleans-as-binary-values
            //int iValue = (boolInt * durationSec) + (i * (1 - (2 * boolInt))); //Deactivate durationSec when false, flip i negative when true
            //DisplayFrame(iValue, 1000); //Display the result
        }

        lastFrameLengthMem = 0; //Animation has completed, reset the global variable
    }
    //Function overload for using MSEC instead of seconds
    private void ActivateAnimTimerMsec(int durationMsec, bool countdownFlag = true)
    {
        ActivateAnimTimer(durationMsec / 1000, countdownFlag);
    }

    //Display a single animation frame, accepts objects so that numbers are handled too
    private int lastFrameLengthMem = 0; //Private attribute, enables the last frame length to be stored to memory
    private bool DisplayFrame(object frameObj, int msecPerFrame, int lastFrameLength = 1)
    {
        //Thread checkpoints for triggering alternate code on exceptions
        bool madeSpace = false;
        bool writing = false;
        bool wrote = false;
        var lastPos = Console.GetCursorPosition();
        try
        {
            if(true) //Optional flag for ingnoring writing using a global variable
            {
                long renderTicksStart = (DateTime.Now).Ticks; //Time how long rendering takes
                //Allocate space to print
                if(lastFrameLengthMem == 0) //Last length was 0
                {
                    //Console.Write(new string('\b',$"{frame}".Length)); //Create a space to write on
                    Console.Write(new string(' ',$"{frameObj}".Length)); //Create a space to write on
                    madeSpace = true;
                    Console.Write(new string('\b',$"{frameObj}".Length)); //Create a space to write on
                }
                else
                {
                    // Console.Write(new string((lastFrameLengthMem+" ").ToCharArray()[0],lastFrameLengthMem));
                    Console.Write(new string(' ',lastFrameLengthMem));
                    madeSpace = true;
                    Console.Write(new string('\b',lastFrameLengthMem));
                }
                writing = true; //Thread checkpoint 2

                //Write the next frame
                lastFrameLengthMem = $"{frameObj}".Length; //Update the last frame length memory
                Console.Write($"{frameObj}{new string('\b',lastFrameLengthMem)}"); //Print this frame, and go back to it's start
                
                //Update variables for threading exceptions
                lastPos = Console.GetCursorPosition();//Store the updated position
                wrote = true; //We are no longer writing, so if we are interrupted make sure to make the final backspace and clear
                
                //Calculated the render delay and subtract it from our sleep time
                long timeOffset = ((DateTime.Now).Ticks) - renderTicksStart; 
                TimeSpan delayTime = new TimeSpan(((long)msecPerFrame * 10000)-(timeOffset)); //The syntax for timespan accepts ticks when the data type is "Long" aka "int64"
                
                //Sleep until the next frame
                try
                {
                    //Thread sleep by timespan: https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.sleep?view=net-7.0#system-threading-thread-sleep(system-timespan)
                    Thread.Sleep(delayTime);
                }
                catch (IndexOutOfRangeException) //Sleep delay is negative!!! We're already late!
                {
                    //No Sleep
                    Console.Write(" \b"); //Clear the final character
                    return false; //We ended early
                }
                Console.Write(" \b"); //Clear the final character after sleeping
                return true; //We rendered this frame
            }
        }
        catch (ThreadInterruptedException) //Early cancellation
        {
            if(wrote) //We wrote but are now sleeping
            {
                var newPos = Console.GetCursorPosition(); //Get the current position
                Console.SetCursorPosition(lastPos.Left, lastPos.Top); //Return to the last recorded position before the exception
                Console.Write(" \b"); //Clear the final character
                Console.SetCursorPosition(newPos.Left,newPos.Top); //Go back to the position we interrupted
                lastFrameLengthMem = 0; //Clear the last frame length memory, because we've started another animation
            }
            else if(writing) //We are going to write, but we stopped
            {
                //Do nothing
                //Console.ForegroundColor = ConsoleColor.DarkYellow;
                //Console.Write(" \b"); //Clear the final character
            }
            else if(madeSpace) //We made space, but didn't backspace yet
            {
                //Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write((new string('\b',lastFrameLengthMem))+(new string('S',lastFrameLengthMem))+(new string('\b',lastFrameLengthMem)));//Backspace
            }
            else //We haven't even made space yet!
            {
                //Do nothing
                //Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            return false;
        }
    }
}