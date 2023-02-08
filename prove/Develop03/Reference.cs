class Reference{
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

    public Reference(string book, int chapter, int[][] verseMatrix)
    {
        SetRefText(book, chapter, verseMatrix);
    }

    //Single Verse Array
    public Reference(string book, int chapter, int[] singleVerseArr)
    {
        //The syntax for a multi-dimensional array in Java is int[][], but in C# it is int[,]
        //What I am using is a "jagged array" and it's syntax information is found here on the .NET docs: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/jagged-arrays
        //I figure that no harm will be done as long as I process it correctly (by reading the syntax from the .NET docs)!
        int[][] verseMatrix = new int[singleVerseArr.Length][]; //Initalize a blank jagged array of equal length to the input array
        for(int i = 0; i < singleVerseArr.Length; i++)
        {
            verseMatrix[i] = new int[]{singleVerseArr[i]}; //Store the value from the input array as an entry of the new matrix
        }
        SetRefText(book,chapter,verseMatrix); //Now that the array has been stored in a 2d array run the normal SetRefText that accepts the 2d array as the input
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
    //Function overload for SetRefText() so that the 1D array constructor can use the same code the 2D array constructor uses
    private void SetRefText(string book, int chapter, int[][] verseMatrix)
    {
        SetRefText(book+" "+chapter+":"+VersesStringFromMatrix(verseMatrix));
    }

    //Constructor Helper Methods
    
    private string VersesStringFromMatrix(int[][] versesArr){
        //Process the list
        string returnString = ""; //Initalize a string to store the return string in
        for(int i = 0; i < versesArr.Length; i++)
        {
            if(i != 0)
            {
                returnString += ","; //Only add this string when we aren't the first element
            }
            returnString += VerseStringFromArr(versesArr[i]); //Get this verse
        }
        return returnString;
    }

    //Convert a single verse set into a verse string
    //Ex: "1-2" "1" etc
    private string VerseStringFromArr(int[] verseSet){
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
            //Error, return nothing
            return "";
        }
    }

    //Methods

}