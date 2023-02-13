class MathAssignment : Assignment
{
    private string _textbookSection = "";
    private string _problems = "";
    //Discovered that {get; set;} (which is C# shortcut for getters and setters) doesn't actaully make a getter and setter automatically
    //private string _textBookSection {get; set;} = "";
    //private string _problems {get; set;} = "";
    public MathAssignment()
    {

    }
    public MathAssignment(string studentName, string topic, string textBookSection, string problems) : base(studentName, topic)
    {
        _textbookSection = textBookSection;
        _problems = problems;
    }

    public string GetHomeworkList()
    {
        return $"{_textbookSection} {_problems}";
    }
    public string GetTextbookSection()
    {
        return _textbookSection;

    }
    public void SetTextbookSection(string textbookSection)
    {
        _textbookSection = textbookSection;

    }
    public string GetProblems()
    {
        return _problems;

    }
    public void SetProblems(string problems)
    {
        _problems = problems;

    }
}