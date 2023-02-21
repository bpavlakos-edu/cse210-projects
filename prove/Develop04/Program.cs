//Mindfullness program
//Week 08

using System;

class Program
{
    //BreathingActivity bAct = new BreathingActivity();
    //ReflectionActivity rAct = new ReflectionActivity();
    //ListingActivity lAct = new ListingActivity();
    static void Main(string[] args)
    {
        TestActivity();
        //UiLoop();
    }
    static void UiLoop()
    {

    }
    static string GetInput(string inMsg)
    {
        Console.Write(inMsg);
        return Console.ReadLine();
    }

    static void TestActivity()
    {
        Activity testActivity = new Activity("My Activity","Description goes here",new List<string>{"Item A","Item B","Item C"},1);
        testActivity.Run();
    }
}