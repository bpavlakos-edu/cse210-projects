class Reference
{
    //Attributes
    private string _refText;

    //Constructor
    public Reference()
    {
        _refText = "";
    }

    public Reference(string refText)
    {
        _refText = refText;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse = -1)
    {

    }


    //Two Dimensional  Verse Array (Jagged Array in C#)
    //Ex ("1Ne",1,new int[][]{{1},{3,4},{6}})
    public Reference(string book, int chapter, int[][] verseMatrix)
    {
        _refText = book+" "+chapter+":"+VersesStringFromMatrix(verseMatrix);
    }

    //Single Verse Array
    //Ex ("1Ne",1,new int[]{1,4,7})
    public Reference(string book, int chapter, int[] singleVerseArr)
    {
        //Found documentation on the .Join function here: https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualbasic.strings.join?view=net-7.0
        //Intellisense let me know that the parameters I want are in reverse order
        //I initalized a new string object to use this function inline
        _refText = book+" "+chapter+":"+string.Join(",",singleVerseArr); //1Ne 1:2,3,4,5
    }

    //Constructor Helper Methods
    private string VersesStringFromMatrix(int[][] versesArr){
        //Process the matrix
        string returnString = ""; //Initalize a string to store the return string in
        for(int i = 0; i < versesArr.Length; i++)
        {
            if(i != 0)
            {
                returnString += ","; //Only add this string when we aren't the first element (when the array has only one entry this will be ignored too)
            }
            returnString += VerseStringFromSet(versesArr[i]); //Get this verse set's string equivalent
        }
        return returnString; //Return the string
    }
    
    //Getters and setters
    //refText
    public string GetRefText()
    {
        return _refText;
    }
    public void SetRefText(string refText)
    {
        _refText = refText;
    }

    //Convert a single verse set into a verse string
    //Ex: "1-2" "1" etc
    private string VerseStringFromSet(int[] verseSet)
    {
        /*Automatically process a verse set to the appropriate string equivalent
        {1,2} = "1-2"
        */
        if(verseSet.Length == 1)
        {
            return verseSet[0]+""; //Single verse entry, simply return the verse by itself
        }
        else if(verseSet.Length > 1)
        {
            return verseSet[0]+"-"+verseSet[verseSet.Length]; //Return the scripture as "Start-Finish"
        }
        else
        {
            return "";//Error, return nothing
        }
    }

    //Methods
    public void Display(string newLine="")
    {
        Console.Write(_refText+newLine);
    }
    //Idea for newline Support
    public void Display(bool newLineFlag)
    {
        Display(Environment.NewLine);
    }
}