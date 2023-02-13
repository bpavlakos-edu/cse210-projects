//Assignment Specifications: https://byui-cse.github.io/cse210-course-2023/unit04/prepare.html

using System;

class Program
{
    static void Main(string[] args)
    {
        //Assignment class test
        Assignment BaseClassTest = new Assignment("Samuel Benet", "Multiplication");
        Console.WriteLine(BaseClassTest.GetSummary());

        Console.WriteLine();

        //MathAssignment Class Test
        MathAssignment MathATest = new MathAssignment("Roberto Rodriguez","Fractions","Section 7.3","Problems 8-19");
        Console.WriteLine(MathATest.GetSummary());
        Console.WriteLine(MathATest.GetHomeworkList()); //Better code readability

        Console.WriteLine();

        //WritingAssignment Class Test
        WritingAssignment WritingATest = new WritingAssignment("Mary Waters","Eurpoean History", "The Causes of World War II");
        Console.WriteLine(WritingATest.GetSummary());
        Console.WriteLine(WritingATest.GetWritingInformation());
    }
}