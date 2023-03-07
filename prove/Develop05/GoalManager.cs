using System.Text.Json; //Lets us use JSON serialization and deserialization
using System.Text.Json.Serialization;
//using System.Text.Json.Nodes;
using UiMenu = QuickUtils.UiMenu; //Importing the custom UI menu class
using UiOption = QuickUtils.UiOption; //Importing the custom Ui Option class
//using System.Runtime.Serialization;

class GoalManager
{
    //How to serialize private members: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/required-properties
    //Note that the import in the example I used (example 2) is incorrect! It should be "using System.Text.Json.Serialization", not "using System.Text.Json"!
    //Also you need a seperate tag for serialzation: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability?source=recommendations&pivots=dotnet-7-0#non-public-property-accessors
    /*
    Failed attempts at using private fields in JSON:
    [JsonPropertyName("propertyNameGoesHere")] //Does nothing
    [JsonRequired] //Does nothing
    [JsonInclude] //Throws an error
    */
    //Attributes

    private List<Goal> _goalList = new List<Goal>() ;

    private long _points = 0;

    private string _userName = "My";
    
    //Constructors
    //Blank
    public GoalManager()
    {
        _goalList = new List<Goal>();
        _points = 0;
        _userName = "My";
    }
    //Fill all attributes
    public GoalManager(List<Goal> goalList, long points, string userName = "My")
    {
        _goalList = goalList.ToList<Goal>();//Copy the list to break the reference to the original
        _points = points;
        _userName = userName;
    }
    //Generate from file path
    public GoalManager(string filePath)
    {
        Load(filePath); //Use existing functionality to load the goal manager to the attributes
    }
    //Copy from an existing goal manager
    public GoalManager(GoalManager newGoalManager)
    {
        //Update this goal list using the values of the GoalManager argument
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
        _userName = newGoalManager.GetUserName();
    }
    //Create a Goal Manager using a flat list
    public GoalManager(List<object> dataList)
    {
        SetGoalManager(dataList); //Use the existing logic
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
    public string GetUserName()
    {
        return _userName;
    }
    public void SetUserName(string userName)
    {
        _userName = userName;
    }
    //Copy the values of another goal manager to this goal manager
    public void SetGoalManager(GoalManager newGoalManager)
    {
        _goalList = newGoalManager.GetGoalList();
        _points = newGoalManager.GetPoints();
        _userName = newGoalManager.GetUserName();
    }
    //Set a goal manager using a list of objects
    public void SetGoalManager(List<object> dataList)
    {
        try
        {
            //This is similar to binary reading, where the list is the data entries, and the offset is advanced as we read from the list
            int offset = 0;
            int goalListCount = (int)dataList[0];
            offset++;
            List<Goal> newGoalList = new List<Goal>();
            for(int i = 0; i < goalListCount; i++)
            {
                int goalType = (int)dataList[offset];
                offset++;
                List<Type> typeList = new List<Type>{new SimpleGoal().GetType(),new EternalGoal().GetType(),new ChecklistGoal().GetType()};
                //See: https://learn.microsoft.com/en-us/dotnet/api/system.type.invokemember?view=net-7.0#code-try-6 for an example of InvokeMember
                //https://learn.microsoft.com/en-us/dotnet/api/system.type.invokemember?view=net-7.0
                //Parameter 1 when null means we aren't calling a field or method
                //Parameter 2 is the Invoking type, it can have lots of flags, but the only one I'm interested in right now is Create Instance
                //Parameter 3 is the binder, its only important when you want to select method overloads
                //Parameter 4 is the target, the documentation says that it's an object to invoke the function on, but isn't that what I'm doing already???
                //Parameter 5 is the arguments for the function
                object newGoal = typeList[goalType].InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, new Object[]{dataList, offset});
                offset += 4;
                if(offset == 2)
                {
                    offset += 2;
                }
                /* switch(goalType) //If we had a way of using the type integer as a constructor for creating each goal type, this would be much smaller!
                {
                    case(0): //SimpleGoal
                        newGoalList.Add(new SimpleGoal(dataList, offset));
                        offset += 4;
                        break;
                    case(1): //EternalGoal
                        newGoalList.Add(new EternalGoal(dataList, offset));
                        offset += 4;
                        break;
                    case(2): //ChecklistGoal
                        newGoalList.Add(new ChecklistGoal(dataList, offset));
                        offset += 6; //It has 2 extra fields
                        break;
                    default:
                        break;
                } */
            }
            //All remaining fields of the goal manager
            long points = (long)dataList[offset];
            offset++;
            string userName = (string)dataList[offset];
            offset++;//We don't really need it at this point...

            //Write the new values to the current goal manager
            _goalList = newGoalList.ToList<Goal>();
            _points = points;
            _userName = userName;
        }
        catch(ArgumentOutOfRangeException e)
        {
            Console.WriteLine($"Error! Offset was misaligned! {e.ToString()}");
        }
    }
    //Generate the List<object> equivalent of the GoalManager, for using in JSON files
    public List<object> GetGoalManager()
    {
        List<object> newDataList = new List<object>();
        newDataList.Add(_goalList.Count); //Add the list count, so we can read it when loading
        for(int i = 0; i < _goalList.Count; i++) //Store all goals
        {
            newDataList.AddRange(_goalList[i].ToObjectList());//Add this goal as an object list to the object list
        }
        //Add the remaining fields
        newDataList.Add(_points);
        newDataList.Add(_userName);
        return newDataList.ToList<object>();
    }

    //Methods
    //Main Display (for goals)
    public void Display()
    {
        Console.WriteLine($"{_userName} goals are:");
        for(int i = 0; i < _goalList.Count; i++)
        {
            Console.WriteLine(_goalList[i].ToDisplayString(i));
        }
    }
    //Goal List Modification
    public void NewGoal()
    {
        //Determine goal Type
        //"The types of Goals are: "
        /*
        "Which type of goal would you like to create? "
        */
        //Add to list actions are stored in lambda functions, to be called by the Action that the menu will use with each item
        List<Action> addToListActions = new List<Action>
        {
            new Action(()=>{_goalList.Add(new SimpleGoal(true));}), //Remember, having true in the constructor will trigger the user input version!
            new Action(()=>{_goalList.Add(new EternalGoal(true));}),
            new Action(()=>{_goalList.Add(new ChecklistGoal(true));})
        };
        //Create a UiMenu that will add a new Child of the Goal class to the Goal list
        UiMenu addGoalMenu = new UiMenu(
            new List<object>(){0,1,2}, //Each of these numbers corresponds to an index in the addToListActions
            new Action<object>((pickedOptionIdx) => {addToListActions[(int)pickedOptionIdx].Invoke();}), //Directly use the result to call the appropriate _goalList.add() action
            new List<string>{"&Simple Goal","&Eternal Goal","&Checklist Goal"}, //Use the new UiOption constructor to auto generate the hotkeys using the index of "&"
            true, //Yes this menu can cancel and return to the previous menu
            "The types of Goals are:", //The menu message
            "Which type of goal would you like to create? " //The input prompt
            //All the other attributes should be set to default, which hides the exit/cancel message by default when this style of constructor is used
        );
        addGoalMenu.UiLoop(); //Start the add goal menu
        //The alternative is to use a default UiMenu, but every action needs a "throw new OperationCancelledError();" at the end
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
    //File loading and saving (Method Overloads), uses JSON
    public void Save()
    {
        //string fileName = GetInput("What is the filename for the goal file? "); //Original
        string fileName = GetInput("What is the filename you would like to save the goal file as? ");
        Save(fileName); //Save the file by calling the actual save file method
    }
    public void Load()
    {
        //string fileName = GetInput("What is the filename for the goal file? "); //Original
        string fileName = GetInput("What is the filename of the goal file you want to load? ");
        Load(fileName); //Load the file by calling the actual load file method
    }
    //Binary Loading and Saving
    public void LoadData()
    {
        string fileName = GetInput("What is the filename of the goal file you want to load? ");
        LoadBinaryFile(fileName);
    }
    public void SaveData()
    {
        string fileName = GetInput("What is the filename you would like to save the goal file as? ");
        SaveBinaryFile(fileName); //Save the file in binary form
    }

    //Goal Marking
    public void RecordEvent()
    {
        //Consider using UI Menu Here
        //Int input (index of goal in list), invalid to cancel
        Display(); //Display the goals before asking which one the user completed
        int goalIndex = GetIntInput("Which goal did you accomplish? ",1,_goalList.Count);
        int newPoints = _goalList[goalIndex-1].Mark(); //Mark the goal as completed
        if(newPoints != 0)
        {
            _points += newPoints;
            Console.WriteLine($"You now have {newPoints} points.");
        }
        else
        {
            Console.WriteLine("That goal has been completed already!");
        }
    }

    //Ported from Journal.cs in Develop 02
    //Change Goal Manager Owner name (obey grammar rules with "'s")
    public void ChangeName()
    {
        string uInput = GetInput("Please Enter Your Name (Leave blank to cancel): ");
        if(uInput != "")
        {
            if(uInput.ToLower().Substring(uInput.Length - 1) == "s") //Check if the last letter is "s"
            { 
                _userName = uInput + "'"; //James -> James' Goals
            }
            else
            {
                _userName = uInput + "'s"; //John -> John's Goals
            }
        }
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
    //Json Serialization is based on Develop02 Journal.cs  "ToJson()" method
    private string ToJson()
    {
        try
        {
            //Uses JSON settings to include fields and indentation
            //return JsonSerializer.Serialize<GoalManager>(this, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
            return JsonSerializer.Serialize<List<object>>(GetGoalManager(),new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
        }
        catch(NotSupportedException e) //This exception has multiple meanings, so we better print it
        {
            Console.WriteLine($"Error {e.ToString()}");
            return "";
        }
        
    }
    //Json Deserialization is based on Develop02 Journal.cs "FromJson()" method
    private void FromJson(string jsonText)
    {
        if(jsonText != "")
        {
            try
            {
                //Uses JSON settings to include fields and indentation
                //GoalManager newGoalManager = JsonSerializer.Deserialize<GoalManager>(jsonText, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
                List<object> dataList = JsonSerializer.Deserialize<List<object>>(jsonText, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true}); //Generate a list of objects using the JsonText
                GoalManager newGoalManager = new GoalManager(dataList);
                //Set current goal manager values to the ones from the deserialized object
                _goalList = newGoalManager.GetGoalList();
                _points = newGoalManager.GetPoints();
                _userName = newGoalManager.GetUserName();
            }
            catch(JsonException e)
            {
                Console.WriteLine($"Error, {e.ToString()}");
            }
            catch(NotSupportedException e) //This exception has multiple meanings, so we better print it
            {
                Console.WriteLine($"Error, {e.ToString()}");
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
        Console.Write(inMsg); //Write the message
        return Console.ReadLine(); //Read the input
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
                        return returnVal;
                    }*/
                    else if(returnVal >= min && returnVal <= max)//Check if the input number is in bounds
                    {
                        return returnVal;
                    }
                    else //Not in bounds
                    {
                        //Do nothing continue with loop
                        //Print message
                        Console.WriteLine("Sorry, that's not a valid number, please try again!");
                    }
                    
                }
                catch(OverflowException){throw new ArgumentNullException();}
                catch(FormatException){throw new ArgumentNullException();}
            }
            catch(ArgumentNullException)//Throw every exception into this block of code
            {
                Console.WriteLine("Sorry, that's not a valid number, please try again!"); //Fail message
            }
        }
    }

    //Utility
    private Goal GetGoal(string inMsg="Select the goal you want to change: ")
    {
        return _goalList[GetIntInput(inMsg, 1, _goalList.Count) - 1]; //Get the current goal (max is goalList.count because its -1 the user input)
    }
    //Binary Writing
    private void SaveBinaryFile(string filePath)
    {
        try
        {
            using (FileStream binaryStream = File.Open(filePath, FileMode.Create))
            {
                BinaryWriter binWriter = new BinaryWriter(binaryStream);//Initalize the binary writer, which is what can actually write bytes
                binWriter.Write(new char[]{'G','O','A','L'}); //Write a 4 byte header

                binWriter.Write(_goalList.Count); //Write the goal list count

                //Make a list to convert each custom type to a corresponding index 
                for(int i=0;i<_goalList.Count;i++) //Write every goal in binary form
                {
                    _goalList[i].WriteGoalHex(binWriter); //Write the goal using the WriteGoalHex function
                }
                //Write the remaining attributes of GoalManager
                binWriter.Write(_points);
                binWriter.Write(_userName);
            }
        }
        catch(IOException e)
        {
            Console.WriteLine($"File writing failed, IO Error: {e.ToString()}");
        }
        catch(ArgumentException e)
        {
            Console.WriteLine($"File writing failed Argument Error: {e.ToString()}");
        }

    }

    private void LoadBinaryFile(string filePath)
    {
        try
        {
            using(FileStream binaryStream = File.OpenRead(filePath))
            {
                BinaryReader binReader = new BinaryReader(binaryStream); //Turn the binary stream into a binary reader so we can use it

                //Read the header to make sure we are reading a file with the correct format
                char[] headerByte = binReader.ReadChars(4);
                if(string.Concat(headerByte) != "GOAL")
                {
                    Console.WriteLine("Error! The GOAL file header is missing, this is not a valid file!");
                    return; //Return early
                }

                //Start reading the data

                //Read the goal list goal entries
                int goalListCount = binReader.ReadInt32(); //Get the number of goals in the goal list
                List<Goal> newGoalList = new List<Goal>(); //Initalize a blank goal list
                for (int i = 0; i < goalListCount; i++)
                {
                    int goalType = (int)binReader.ReadByte(); //Identify the goal type of this entry
                    //Add the goal based on the type read from the goalType byte
                    switch(goalType)
                    {
                        case(0): //Simple Goal
                            newGoalList.Add(new SimpleGoal(binReader)); //Use the binary reader constructor
                            break;
                        case(1): //Eternal Goal
                            newGoalList.Add(new EternalGoal(binReader)); //Use the binary reader constructor
                            break;
                        case(2): //Checklist goal
                            newGoalList.Add(new ChecklistGoal(binReader)); //Use the binary reader constructor
                            break;
                        default: //Invalid goal type
                            //Do nothing
                            Console.WriteLine($"Error! Invalid goal type: {goalType}"); //Okay, fine, alert the user!
                            return;//Return early
                    }
                }

                //Read all additional GoalManager parameters
                long points = binReader.ReadInt64(); //Points, it's a long, so it's 64 bytes long!!!
                string userName = binReader.ReadString(); //User name

                //Now finally update the properties of this GoalManager, if we don't make it here, it means something went wrong, so we don't want to save corrupted values anyways
                _goalList = newGoalList.ToList<Goal>();
                _points = points;
                _userName = userName;
            }
        }
        catch(IOException e)
        {
            Console.WriteLine($"File Loading failed, IO Error! {e.ToString()}");
        }
        catch(ArgumentException e)
        {
            Console.WriteLine($"File Loading failed, Argument Exception! {e.ToString()}");
        }
        
    }

    //Method to quickly write a string and its string length
    //Obsolete because binary Writers write the string length by default
    /*private void WriteString(string inputStr, BinaryWriter binWriter)
    {
        binWriter.Write((short)inputStr.Length); //Always write string length as a short, it takes less space
        binWriter.Write(inputStr);
    } */
    /*private string ReadString()
    {
        string returnStr = "";
        return returnStr;
    }*/
}