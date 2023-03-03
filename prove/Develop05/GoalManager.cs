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
    //File loading and saving
}