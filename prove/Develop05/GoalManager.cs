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
}