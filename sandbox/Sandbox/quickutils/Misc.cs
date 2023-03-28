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

        //Range List Merging Function
        //Based on an algorithim I made in my python class (CSE-110) that resets the loop every time an overlapping range is detected
        //The only way to exit the loop is to sort the list completely
        public static void MergeRangeList(List<int[]> inputList) //Hopefully we'll be able to use list mutability to our advantage here
        {
            //inputList.Sort(); //Sort the input list first
            for(int i = 0; i < inputList.Count; i++)
            {
                bool rangeExists = false; //A boolean to track if the range exists
                //Load start values
                int curRangeStart = inputList[i][0]; //Get the first item
                int curRangeEnd = inputList[i][inputList[i].Length - 1]; //Get the last item
                //Create variables to store the newRange Start and End inside
                int newRangeStart = -1;
                int newRangeEnd = -1;
                //Check every existing range in the input list
                for(int j = 0; j < inputList.Count; j++)
                {
                    if(inputList.FindLastIndex((int[] inputArr) => {return inputArr == inputList[i];}) == i) //If the last index of the current item is i, it's unique
                    {
                        if(j != i) //Ignore testing the same range
                        {
                            //inputList.Sort(); //Sort the list first
                            int testRangeStart = inputList[i][0]; //Get the first entry
                            int testRangeEnd = inputList[i][inputList[i].Length - 1]; //Get the last entry
                            //Find the lowest start, and the highest end points
                            //As my python code explained: "Just imagine they are horizontal lines parallel to eachother where if they overlap at all they are combined into one big line"
                            if((curRangeStart <= testRangeEnd && curRangeEnd >= testRangeStart) || (curRangeStart >= testRangeEnd && curRangeEnd <= testRangeStart)) //"If the following line is within the range of the other line it will meet this condition"
                            {
                                newRangeStart = (curRangeStart <= testRangeStart) ? curRangeStart : testRangeStart; //Set new range start to the lowest value
                                newRangeEnd = (curRangeEnd >= testRangeEnd) ? curRangeEnd : testRangeEnd; //Set new range end to the greatest value
                                rangeExists = true; //"Regardless of the results of the if statement the range is in one that exists, so this is true"
                            }
                            if(rangeExists) //Ranges existing already, means we need to remove / merge the duplicates based on the indexes we found
                            {
                                inputList.RemoveAt((i > j) ? i : j); //Remove the greatest index first, so we don't get an index error
                                inputList.RemoveAt((i > j) ? j : i); //Remove the other index
                                inputList.Insert(0, (newRangeStart == newRangeEnd) ? new int[]{newRangeStart} : new int[]{newRangeStart, newRangeEnd}); //When the new range start and end are the same send 1 number, if they are different send the array with the newRangeStart and newRangeEnd
                                break; //"End the for loop early, any other duplicates will be tested in the next part of the [for] loop"
                            }
                        }
                    }
                    else //Not a unique entry
                    {
                        rangeExists = true; //The range definitely exists
                        inputList.RemoveAt(i); //Remove the item if it's not unique, the next one will automatically be tested
                    }
                    //When the range exists reset i to -1 (which will make it 0 after i++)
                    if(rangeExists)
                    {
                        i = -1; //Reset the loop (Which I couldn't do in a python for loop, this is nice!)
                    }
                }
            }
        }
        //Sort a 2d list using the last entry of each sub list
        public static void Sort2dList(List<int[]> inputList)
        {
            inputList.Sort((int[] a, int[] b) => {return (a.Last() >= b.Last()) ? a.Last() : b.Last();});
        }
    }
}