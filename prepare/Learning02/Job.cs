using System;
public class Job
{
    public string _company = "";
    public string _jobTitle = "";
    public int _startYear = 0;
    public int _endYear = 0;

    //Empty constructor
    public Job()
    {

    }

    //Constructors in C# (It was my second guess, but intellisense didn't recognize it quickly enough):
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors

    public Job(string company, string jobTitle, int startYear, int endYear)
    {
        _company = company;
        _jobTitle = jobTitle;
        _startYear = startYear;
        _endYear = endYear;
    }
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }


}