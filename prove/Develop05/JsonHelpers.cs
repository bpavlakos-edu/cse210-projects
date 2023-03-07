//These classes are unused for two reasons:
//1: It's the same as just slapping "public" on all class varaibles, and defeats the purpose of encapsulation
//2: I'm pretty much finished with the other janky way of making Json files
//This is really all to get around the limitation that System.Text.Json cannot serialize or deserialize private fields!
/* 
using System.Text.Json;
class JsonGoalManager
{
    public List<JsonGoal> _goalList = new List<JsonGoal>();
    public long _points = 0;
    public string _userName = "My";
    public JsonGoalManager(GoalManager sourceGoalManager)
    {
        _goalList = ToJsonGoals(sourceGoalManager.GetGoalList());
        _points = sourceGoalManager.GetPoints();
        _userName = sourceGoalManager.GetUserName();
    }
    private List<JsonGoal> ToJsonGoals(List<Goal> sourceGoalList)
    {
        List<JsonGoal> returnList = new List<JsonGoal>();
        for(int i = 0; i < sourceGoalList.Count; i++)
        {
            returnList.Add(new JsonGoal(sourceGoalList[i]));
        }
        return returnList;
    }
    public string ToJson()
    {
        return JsonSerializer.Serialize<JsonGoalManager>(this, new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
    }
    public void FromJson(string jsonString)
    {
        JsonGoalManager newJsonGoalManager = JsonSerializer.Deserialize<JsonGoalManager>(jsonString,new JsonSerializerOptions{IncludeFields = true, WriteIndented = true});
        _goalList = (newJsonGoalManager._goalList).ToList<JsonGoal>();//copy the list
        _points = newJsonGoalManager._points;
        _userName = newJsonGoalManager._userName;
    }
}

//Json equivalent of the goal class
class JsonGoal
{
    public int _goalType = -1;
    public string _name = "";
    public string _desc = "";
    public int _value = 0;
    public int _compCount = 0;
    public List<object> _extraParams = new List<object>();
    public JsonGoal(Goal goalInput)
    {
        //Use the same trick as binary reading/writing where we assign goal type to an index
        List<Type> typeToIndexList = new List<Type>(){new SimpleGoal().GetType(),new EternalGoal().GetType(), new ChecklistGoal().GetType()}; 
        _goalType = typeToIndexList.IndexOf(goalInput.GetType()); //Find the current type from the list we just made
        _name = goalInput.GetName();
        _desc = goalInput.GetDesc();
        _value = goalInput.GetValue();
        _compCount = goalInput.GetCompCount();
        if(_goalType == 2)
        {
            ChecklistGoal checklistGoalForced = (ChecklistGoal) goalInput;
            _extraParams = new List<object>(){checklistGoalForced.GetBonusCompGoal(),checklistGoalForced.GetBonusValue()};
        }
    }
    public Goal DecodeGoal()
    {
        if(_goalType == 0)
        {
            return new SimpleGoal();
        }
        else if(_goalType == 1)
        {
            return new EternalGoal();
        }
        else if(_goalType == 2)
        {
            return new ChecklistGoal();
        }
        else
        {
            return null;
        }
    }
} */