class WritingAssignment : Assignment
{
    private string _title = "";

    WritingAssignment(string studentName, string topic, string title): base(studentName, topic)
    {
        _title = title;
    }
}