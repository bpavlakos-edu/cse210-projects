class Assignment
{
    protected string _studentName = "";
    protected string _topic = "";
    //Discovered that {get; set;} (which is C# shortcut for getters and setters) doesn't actaully make a getter and setter automatically
    //protected string _studentName {get; set;} = "";
    //protected string _topic {get; set;} = "";

    public Assignment()
    {
        
    }

    public Assignment(string studentName, string topic)
    {
        _studentName = studentName;
        _topic = topic;
    }

    public string GetSummary()
    {
        return $"{_studentName} - {_topic}";
    }

    public string GetStudentName()
    {
        return _studentName;
    }
    public void SetStudentName(string studentName)
    {
        _studentName = studentName;
    }

    public string GetTopic()
    {
        return _topic;
    }

    public void SetTopic(string topic)
    {
        _topic = topic;
    }
}