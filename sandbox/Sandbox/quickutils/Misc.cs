namespace QuickUtils
{
    static class Misc
    {
        //Display friendly strings for common data types, this was occuring too much in the code, and I want to control it from one place
        public static string BoolStr(bool inBool, string trueStr = "Enabled", string falseStr = "Disabled")
        {
            return (inBool) ? trueStr : falseStr;
        }
        public static string QuoteStr(string inStr, char qChar = '"')
        {
            return $"{qChar}{inStr}{qChar}"; //I think this is the fastest way to concatonate them
        }
        public static string RoundFStr(float inFloat, int? digits = null)
        {
            //Found this: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#currency-format-specifier-c
            //Which allows me to manually format it using the parameter digits
            return inFloat.ToString((digits != null) ? $"F{digits}" : null);
        }
        //Found out about object constraints:
        //Stack Overflow post: https://stackoverflow.com/a/29345294
        //MSDN: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-generic-type-constraint?redirectedfrom=MSDN
        //Detailed Syntax: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters
        //Idea to fix it: Use a Func<objType,objType> to call the constructor
        //Here's the solution I made: Use a lambda to capture the classes deep cloning constructor, while this means you'll have to specify the constructor in the function call, and the original class, 
        //It's a small sacrifice to have a reusable list copy function
        //Example of function call: 
        //ListCopy<Dice>(new List<Dice>{items}, (Dice inputDice) => {return new Dice(inputDice);});
        public static List<objType> ListCopy<objType>(List<objType> inputList, Func<objType,objType> objConstructorFun)// where objType : new() //Code that lets you use New() but not any other constructor
        {
            List<objType> returnList = new List<objType>(); //Create a list to store the items with
            inputList.ForEach((objType inObj) => {returnList.Add(objConstructorFun(inObj));});//Use the captured constructor to add items to list
            return returnList;
            //inputList.ForEach((objType inObj) => {objType newObj = new objType();}); //Attempted usage of a constructor
        }
    }
}