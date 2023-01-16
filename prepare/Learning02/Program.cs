//Week 03: Learning Activity 02
using System;

class Program
{
    static void Main(string[] args)
    {
        //Step 4.2 - 4.3
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer"; //Field value given By Step 4
        job1._company = "Microsoft"; //Field value given By Step 4
        job1._startYear = 2019; //Field value given by Step 5
        job1._endYear = 2022; //Field value given by Step 5
        //With Constructor:
        //Job job2 = new Job("Microsoft","Software Engineer",2019,2022);

        
        //Step 4.5:
        Job job2 = new Job();
        job2._jobTitle = "Manager"; //Field value given by step 5
        job2._company = "Apple"; //Field value given By Step 4
        job2._startYear = 2022; //Field value given by Step 5
        job2._endYear = 2023; //Field value given by Step 5
        //With Constructor:
        //Job job2 = new Job("Apple","Manager",2022,2023);

        /*
        //Step 4.4:
        Console.WriteLine($"{job1._company}");
        //Step 4.5 (cont.):
        Console.WriteLine($"{job2._company}");
        */ //Step 5.3 called for removal
        /*
        //Step 5.3:
        job1.Display();
        job2.Display();
        */ //Step 8 Called for removal

        //Step 7:
        Resume resume1 = new Resume();
        resume1._name = "Allison Rose";
        resume1._jobs.Add(job1);
        resume1._jobs.Add(job2);
        resume1.Display();

        //Compact version:
        //I needed to learn how to declare a list with items in it, which I learned here:
        // https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/arrays-and-collections
        //Resume resumeCompact = new Resume("Allison Rose", new List<Job>{new Job("Microsoft","Software Engineer",2019,2022), new Job("Apple","Manager",2022,2023)});
    }
}