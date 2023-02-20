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
}