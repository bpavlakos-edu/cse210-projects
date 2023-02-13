class WritingAssignment : Assignment
{
    private string _title = "";
    //Discovered that {get; set;} (which is C# shortcut for getters and setters) doesn't actaully make a getter and setter automatically
    // private string _title {get; set;}= "";

    public WritingAssignment()
    {

    }
    public WritingAssignment(string studentName, string topic, string title): base(studentName, topic)
    {
        _title = title;
    }

    public string GetWritingInformation()
    {
        return $"{_title} by {_studentName}";
        //Alternative is:
        //return $"{_title} by {base.GetStudentName()}";
    }

    public string GetTitle()
    {
        return _title;
    }
    public void SetTitle(string title)
    {
        _title = title;
    }
}