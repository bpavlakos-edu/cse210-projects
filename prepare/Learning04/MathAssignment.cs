class MathAssignment : Assignment
{
    private string _textBookSection {get; set;} = "";
    private string _problems {get; set;} = "";
    public MathAssignment()
    {

    }
    public MathAssignment(string studentName, string topic, string textBookSection, string problems) : base(studentName, topic)
    {
        _textBookSection = textBookSection;
        _problems = problems;
    }
}