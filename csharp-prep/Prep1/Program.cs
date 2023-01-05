//https://byui-cse.github.io/cse210-course-2023/unit01/csharp-1.html
using System;

class Program
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Hello Prep1 World!");
        string firstName = input("What is your first name? ");
        string lastName = input("What is your last name? ");
        Console.WriteLine("");
        Console.WriteLine($"Your name is {lastName}, {firstName} {lastName}.");
    }

    //Function to replicate python style input, to reduce repeated code, this also allows for protections in the future too
    static string input(string inMsg){
        Console.Write(inMsg);
        return Console.ReadLine();
    }
}