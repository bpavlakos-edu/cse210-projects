//Activity Instructions: https://byui-cse.github.io/cse210-course-2023/unit05/prepare.html
using System;

class Program
{
    static void Main(string[] args)
    {
        //Step 5: Test the square class
        Square squareTest = new Square("Red",5);
        Console.WriteLine($"Area of a 5 by 5 square: {squareTest.GetArea()}");
    }
}