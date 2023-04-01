using System.Security.Cryptography; //For cryptographically secure random number generation
namespace QuickUtils
{
    static class Misc
    {
        //Internal Fields (Do not externally access or redefine random values, it leads to really bad randomziation! https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-7.0#avoiding-multiple-instantiations)
        //private static Random rngGen = new Random(); //Standard random can be used instead of RandomNumberGenerator
        private static RandomNumberGenerator _randomGen = RandomNumberGenerator.Create(); //Random number generator, cryptographically secure (https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.randomnumbergenerator?view=net-7.0)
        
        //Display friendly strings for common data types, this was occuring too much in the code, and I want to control it from one place
        //Represent Boolean as Enabled or Disabled
        public static string BoolStr(bool inBool, string trueStr = "Enabled", string falseStr = "Disabled")
        {
            return (inBool) ? trueStr : falseStr;
        }
        //Put a quote around a string automatically
        public static string QuoteStr(string inStr, char qChar = '"')
        {
            return $"{qChar}{inStr}{qChar}"; //I think this is the fastest way to concatonate them
        }
        //Float string rounded to a number
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
        //Generic list copy function, can copy custom classes if they have a constructor that individually copies each attribute from the class put in to the class instances attribute value (deep copy constructor)
        public static List<objType> ListCopy<objType>(List<objType> inputList, Func<objType,objType> objConstructorFun)// where objType : new() //Code that lets you use New() but not any other constructor
        {
            List<objType> returnList = new List<objType>(); //Create a list to store the items with
            inputList.ForEach((objType inObj) => {returnList.Add(objConstructorFun(inObj));});//Use the captured constructor to add items to list
            return returnList;
            //inputList.ForEach((objType inObj) => {objType newObj = new objType();}); //Attempted usage of a constructor
        }

        //List Map (Similar to JavaScript "Map" function)
        /*Params:
        inType = input list data type
        outType = expected output type
        inputList = list to convert
        mappingFunction = function to run on each item in the input list, to store it in the return list
        Returns: A list of the desired output type, generated using the function passed as "mappingFunction"*/
        public static List<outType> ListMap<inType, outType>(List<inType> inputList, Func<inType,outType> mappingFunction)
        {
            List<outType> returnList = new List<outType>(); //Initalize the return list
            inputList.ForEach((inType inputValue) =>{returnList.Add(mappingFunction(inputValue));}); //Use the mapping function on each entry
            return returnList;
        }

        //Convert a Dictionary to a list of tuple
        public static List<(keyType, valType)> DictToTupleList<keyType, valType>(Dictionary<keyType, valType> dictionaryInput)
        {
            List<(keyType, valType)> returnList = new List<(keyType, valType)>();
            foreach(keyType key in dictionaryInput.Keys) //Get every key
            {
                returnList.Add((key, dictionaryInput[key])); //And use it to fill the tuple
            }
            return returnList; //Return the generated list
        }

        //Range List Merging Function
        //Based on an algorithim I made in my python class (CSE-110) that resets the loop every time an overlapping range is detected
        //The only way to exit the loop is to sort the list completely
        public static void MergeRangeList(List<int[]> inputList) //Hopefully we'll be able to use list mutability to our advantage here
        {
            //inputList.Sort(); //Sort the input list first
            for(int loadedIndex = 0; loadedIndex < inputList.Count; loadedIndex++) //Load each range item
            {
                bool rangeDuplicate = false; //A boolean to track if the range exists
                //Load start values
                int loadRangeStart = inputList[loadedIndex][0]; //Get the first item
                int loadRangeEnd = inputList[loadedIndex][inputList[loadedIndex].Length - 1]; //Get the last item
                //Create variables to store the newRange Start and End inside
                int minRangeStart = -1;
                int maxRangeEnd = -1;
                //Check every existing range in the input list
                for(int checkIndex = 0; checkIndex < inputList.Count; checkIndex++)
                {
                    if(inputList.FindLastIndex((int[] inputArr) => {return inputArr == inputList[loadedIndex];}) == loadedIndex) //If the last index of the current item is i, it's unique
                    {
                        if(checkIndex != loadedIndex) //Ignore testing the same range against itself
                        {
                            //inputList.Sort(); //Sort the list first
                            int checkRangeStart = inputList[checkIndex][0]; //Get the first entry
                            int checkRangeEnd = inputList[checkIndex][inputList[checkIndex].Length - 1]; //Get the last entry
                            //Find the lowest start, and the highest end points
                            //As my python code explained: "Just imagine they are horizontal lines parallel to eachother where if they overlap at all they are combined into one big line"
                            if((loadRangeStart <= checkRangeEnd && loadRangeEnd >= checkRangeStart) || (loadRangeStart >= checkRangeEnd && loadRangeEnd <= checkRangeStart)) //"If the following line is within the range of the other line it will meet this condition"
                            {
                                minRangeStart = (loadRangeStart <= checkRangeStart) ? loadRangeStart : checkRangeStart; //Set new range start to the lowest value
                                maxRangeEnd = (loadRangeEnd >= checkRangeEnd) ? loadRangeEnd : checkRangeEnd; //Set new range end to the greatest value
                                rangeDuplicate = true; //"Regardless of the results of the if statement the range is in one that exists, so this is true"
                            }
                            if(rangeDuplicate) //Ranges existing already, means we need to remove / merge the duplicates based on the indexes we found
                            {
                                inputList.RemoveAt((loadedIndex > checkIndex) ? loadedIndex : checkIndex); //Remove the greatest index first, so we don't get an index error
                                inputList.RemoveAt((loadedIndex > checkIndex) ? checkIndex : loadedIndex); //Remove the other index
                                inputList.Insert(0, (minRangeStart == maxRangeEnd) ? new int[]{minRangeStart} : new int[]{minRangeStart, maxRangeEnd}); //When the new range start and end are the same send 1 number, if they are different send the array with the newRangeStart and newRangeEnd
                                break; //"End the for loop early, any other duplicates will be tested in the next part of the [for] loop"
                            }
                        }
                    }
                    else //Not a unique entry
                    {
                        rangeDuplicate = true; //The range definitely exists
                        inputList.RemoveAt(loadedIndex); //Remove the item if it's not unique, the next one will automatically be tested
                    }
                    inputList[loadedIndex] = CleanRange(inputList[loadedIndex]); //Sort this range entry
                    //When the range exists reset i to -1 (which will make it 0 after i++)
                    if(rangeDuplicate) //When the range exist's it's time to rest loop
                    {
                        loadedIndex = -1; //Reset the loop (Which I couldn't do in a python for loop, this is nice!)
                    }
                }
                Sort2dList(inputList); //Sort the entire list
            }
        }
        //Sort a 2d list using the last entry of each sub list
        public static void Sort2dList(List<int[]> inputList)
        {
            inputList.Sort((int[] a, int[] b) => {return (a.Last() >= b.Last()) ? 1 : -1;}); //Example where it tells you to use 1 and -1 to sort the lists is found here: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.sort?view=net-8.0#system-collections-generic-list-1-sort(system-comparison((-0)))
        }
        
        //Pick the highest number as the end, and the lowest number as the start, if they are identical merge them
        private static int[] CleanRange(int[] inputArr)
        {
            if(inputArr.Length > 0)
            {
                if(inputArr.Length == 1) //Exit early when the length is 1, because it won't be changed at the ended
                {
                    return inputArr;
                }
                //Find the minimum and maximum of this array
                int? min = null;
                int? max = null;
                for(int i = 0; i < inputArr.Length;i++)
                {
                    if(min == null || min > inputArr[i]) //Found a lower minimum
                    {
                        min = inputArr[i];
                    }
                    if(max == null || max < inputArr[i]) //Found a higher maximum
                    {
                        max = inputArr[i];
                    }
                }
                return (min == max) ? new int[]{(int)min} : new int[]{(int)min,(int)max}; //Return 1 number if they are equal, or min/max if they are not
            }
            else
            {
                return new int[]{}; //Return a blank array
            }
        }

        //Small file reading and writing functions
        public static string ReadFileLine(string[] fileLines, ref int offset, string expectedStartString = null)
        {
            string returnStr = fileLines[offset]; //Initalize the return value to the current entry in fileLines (skips having to manually return it)
            if(expectedStartString != null) //Has a valid expectedStartString, process it so that we can remove it from the return
            {
                if(!fileLines[offset].Contains(expectedStartString)) //Check for the start string
                {
                    throw new IOException($"Error! \"{expectedStartString}\" was not found on file line {offset}!"); //Throw an exception to cancel file reading
                }
                returnStr = fileLines[offset].Replace(expectedStartString, null); //Otherwise, remove everything except for the expected start string, now it should return the string we're actually looking for
            }
            offset += 1; //Increment the offset by 1, this change should effect the offset all the way up to the original declaration of it
            return returnStr; //Return the string
        }

        //Randomization function, to prevent stale randomness
        public static int RandomInt(int min, int max)
        {
            return RandomNumberGenerator.GetInt32(min, max);
        }
        //Function overload for only providing the max
        public static int RandomInt(int max)
        {
            return RandomInt(0, max);
        }
        //Random Float (Single Precision Decimal Number / FloatInt)
        public static float RandomFloat()
        {
            return new Random(RandomNumberGenerator.GetInt32(int.MaxValue)).NextSingle(); //Use the cryptographically secure random to generate the seed the regular random uses to make float values
        }
        //Random Double (Double Precision Decimal Number / Float64)
        public static double RandomDouble()
        {
            return new Random(RandomNumberGenerator.GetInt32(int.MaxValue)).NextDouble();  //Use the cryptographically secure random to generate the seed the regular random uses to make double values
        }

        //String Grammar
        //Pluralizer
        public static string Pluralize(string singularStr, int count)
        {
            return (count != 1) ? singularStr+"s" : singularStr; //Use the ternary operator to append s to the string
        }
    }
}