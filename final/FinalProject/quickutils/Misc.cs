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
        /*public static List<objType> CloneList<objType>(List<objType> inputList) where objType : new()
        {
            List<objType> returnList = new List<objType>();
            inputList.ForEach((objType inObj) => {objType newObj = new objType();});
            return returnList;
        }*/
    }
}