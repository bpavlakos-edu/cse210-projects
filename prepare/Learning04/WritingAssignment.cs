class WritingAssignment : Assignment
{
    private string _title {get; set;}= "";

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
    }
}