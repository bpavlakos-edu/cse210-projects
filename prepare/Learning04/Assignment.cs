class Assignment
{
    protected string _studentName = "";
    protected string _topic = "";

    public string GetSummary()
    {
        return $"{_studentName} {_topic}";
    }

}