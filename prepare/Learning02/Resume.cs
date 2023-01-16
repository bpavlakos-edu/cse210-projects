public class Resume
{
    //Fields
    public string _name = "";
    public List<Job> _jobs = new List<Job>();

    //Constructors
    //Blank Constructor
    public Resume(){}
    public Resume(string name, List<Job> jobs)
    {
        _name = name;
        _jobs = jobs.ToList(); //According to one of my old C# projects, the way to clone a list is to use the "ToList()" function, which forces the list to make a clone of itself
        //Solution Documented from my original program:
        //https://stackoverflow.com/questions/15330696/how-to-copy-list-in-c-sharp
    }
    public void Display()
    {
        //Formatting specified in step 8
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        for(int i=0; i<_jobs.Count();i++){
            _jobs[i].Display();
        }

    }
}