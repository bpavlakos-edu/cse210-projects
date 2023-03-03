using System.Text.Json;

class GoalManager
{
    //Attributes
    private List<Goal> _goalList = new List<Goal>();
    private long _points = 0;
    //private string _userName = "";
    
    //Constructors
    //Blank
    public GoalManager()
    {

    }
    //Fill all attributes
    public GoalManager(List<Goal> goalList, long points)
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
        _points = points;
    }
    //Generate from file path
    public GoalManager(string filePath)
    {
        //Not implemented
    }
    //Copy from an existing goal manager
    public GoalManager(GoalManager newGoalManager)
    {
        //Update this goal list using the values of the GoalManager argument
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
    }

    //Getters and setters
    public List<Goal> GetGoalList()
    {
        return _goalList.ToList<Goal>();//Copy the list to break the reference to the original
    }
    public void SetGoalList(List<Goal> goalList)
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
    }
    public long GetPoints()
    {
        return _points;
    }
    public void SetPoints(long points)
    {
        _points = points;
    }
    //GetName
    //SetName

    //Methods
    //Main Display
    public void Display()
    {
        //Consider using UI Menu here (Programically generate options)
        Console.WriteLine("");
        for(int i = 0; i < _goalList.Count; i++)
        {
            //Console.WriteLine(_goalList[i].ToDisplayString(i));
        }
    }
    //Goal List Modification
    public void NewGoal()
    {
        //_goalList.Add(new Goal(0));
    }
    public void DeleteGoal()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        //_goalList.RemoveAt(index);
    }
    public void EditGoal()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        //Int input (index of property to change), invalid to cancel
        //input / int input, new value [Write new value]
    }
    //File loading and saving (Method Overloads)
    public void Save()
    {
        string fileName = GetInput("");
        //Save(fileName);
    }
    public void Load()
    {
        string fileName = GetInput("");
        //Load(fileName);
    }

    //Goal Marking
    public void RecordEvent()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        int goalIndex = GetIntInput("");
        //_points += _goalList[goalIndex].Mark();
    }

    //File Management
    public void Save(string filePath)
    {
        try
        {
            //I found File.CreateText() using intellisense, it creates the file if missing, or erases it before writing
            using(StreamWriter sWriter = File.CreateText(filePath))
            {
                string jsonString = ToJson(); //Generate the JSON string if possible
                if(jsonString != "")
                {
                    sWriter.Write(jsonString); //Write the JSON string
                    Console.WriteLine("File successfuly saved");
                }
                else
                {
                    Console.WriteLine("Failed to save file, JSON text was empty!");
                }
            }
        }
        catch(IOException e) //IO Errors
        {
            Console.WriteLine($"Unable to create file, error: {e.ToString()}");
        }
        catch(NotSupportedException){}
        catch(ObjectDisposedException){}
        catch(ArgumentException){}
        //catch(ArgumentNullException){} //Caught by ArgumentException
    }
    public void Load(string filePath)
    {
        try
        {
            string jsonText = "";
            using(StreamReader sReader = File.OpenText(filePath))
            {
                jsonText = sReader.ReadToEnd(); //Read all file lines
            }
            FromJson(jsonText); //Use the FromJson method, which will update the global manager
        }
        catch(IOException e) //IO Errors
        {
            Console.WriteLine($"Unable to load file, error: {e.ToString()}");
        }
        //Other errors
        catch(NotSupportedException){}
        catch(ObjectDisposedException){}
        catch(ArgumentException){}
        //catch(ArgumentNullException){} //Caught by ArgumentException
    }

    //JSON Helpers
    private string ToJson()
    {
        try
        {
            return JsonSerializer.Serialize<GoalManager>(this, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true})??"";
        }
        catch(NotSupportedException)
        {
            Console.WriteLine("Json is not supported on this computer!");
            return "";
        }
        
    }
    private void FromJson(string jsonText)
    {
        if(jsonText != "")
        {
            try
            {
                GoalManager newGoalManager = JsonSerializer.Deserialize<GoalManager>(jsonText, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
                _goalList = newGoalManager.GetGoalList();
                _points = newGoalManager.GetPoints();
                //_name = newGoalManager.GetName();
            }
            catch(JsonException e)
            {
                Console.WriteLine($"Error, {e.ToString()}");
            }
            catch(NotSupportedException e)
            {
                Console.WriteLine($"Incompatible file! {e.ToString()}");
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine($"Error, file was empty! {e.ToString()}");
            }
        }
        else
        {
            Console.WriteLine("Error, file was empty!");
        }
    }

    //User Input
    private string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }
    private int GetIntInput(string inMsg, int min=0, int max=0)
    {
        //Todo: Add Min Max Support
        //Consider putting ParseInt as it's own function in UiMenu
        while(true)
        {
            try
            {
                try
                {
                    int returnVal = int.Parse(GetInput(inMsg));
                    //Insert min/max control here (throw new ArgumentNullException)
                    if(min == max)
                    {
                        return returnVal;
                    }
                    else if(max < min && returnVal >= min) //When max < min it's Minimum only
                    {
                        return returnVal;
                    }
                    /*else if(maximumOnly)
                    {

                    }*/
                    else if(returnVal >= min && returnVal <= max)//Check if the input number is in bounds
                    {
                        return returnVal;
                    }
                    else //Not in bounds
                    {
                        //Do nothing continue with loop
                        //Print message
                    }
                    
                }
                catch(OverflowException){throw new ArgumentNullException();}
                catch(FormatException){throw new ArgumentNullException();}
            }
            catch(ArgumentNullException)
            {
                //Fail message
            }
        }
    }

    //Utility
    private Goal GetGoal(string inMsg="Select the goal you want to change:")
    {
        return _goalList[GetIntInput(inMsg, 1, _goalList.Count) - 1]; //Get the current goal (max is goalList.count because its -1 the user input)
    }

}