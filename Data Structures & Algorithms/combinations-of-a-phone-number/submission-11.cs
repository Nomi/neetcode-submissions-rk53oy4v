/*
========================================================================
🔥 COMPLEXITY ANALYSIS: LETTER COMBINATIONS
========================================================================
*/

// ⏱️ TIME COMPLEXITY: O(N * 4^N)
// ---------------------------------------------------------------------
// -> THE 4^N PART (The Tree Size): 
//    For every digit, we have AT MOST 4 choices (e.g., '7' is "pqrs").
//    Because the choices are independent, the total number of combinations
//    (leaf nodes in our recursion tree) is upper-bounded by 4^N.
//
// -> THE N PART (The Work per Leaf):
//    Every time we reach a valid leaf node, we have to build the final
//    string to add to our result list using `string.Concat(str)`. 
//    Constructing a string of length N takes O(N) time.
//
// -> THE RESULT: 
//    O(N) work done across O(4^N) leaf nodes = O(N * 4^N).

// 💾 SPACE COMPLEXITY: O(N) Auxiliary Space
// ---------------------------------------------------------------------
// -> RECURSION DEPTH: 
//    The call stack only ever goes as deep as the length of the digits (N).
//
// -> CURRENT LIST: 
//    Our tracking list `List<char> str` only grows to a max size of N.
//
// -> THE RESULT: 
//    Algorithmic/Auxiliary space is strictly O(N). 
//    (Note: Total space including the output array is O(N * 4^N), 
//    because we store 4^N strings of length N).


public class Solution {
    public List<string> LetterCombinations(string digits) {
        
        // # Soln. from Late 2024
        // //GO THROUGH THE SOLUTION ONCE!
        // //(also, check neetcodeio soln for less verbose solution (and it is less syntax error prone e.g. forgetting new() before list initialization)!)
        // return (new Attempt1()).LetterCombinations(digits);

        // # Soln. from March 2026:
        // I'm a bit iffy on the TC and SC calculations! //tbf I was right, and just needed more experience I guess?
        return (new NuAttempt1()).LetterCombinations(digits);
    }
}



// # Soln. from Late 2024:
public class Attempt1 {
    public List<string> LetterCombinations(string digits) {
        //GO THROUGH THE SOLUTION ONCE!
        //(also, check neetcodeio soln for less verbose solution (and it is less syntax error prone e.g. forgetting new() before list initialization)!)
        return backtrack1(digits);
    }

    public List<string> backtrack1(string digits)
    {
        if(digits.Length == 0) //WHY DID I NOT THINK OF THIS??? (had to look at example to get it!) //GOOD EXAMPLE OF CLARIFICATION QUESTIONS!!
            return new();

        //THE SECOND CONSTRAINT IS ALSO A GOOD CLARIFICATION QUESTION! (will there be 1 or any other number that doesn't have representation, how about 0??)
        //ALSO, MAYBE SHOULD ASK IF THE OUTPUT SHOULD BE UPPERCASE OR LOWERCASE (had used uppercase so I had to change after looking at example output)
        Dictionary<char, List<string>> map = new(){
            {'2', new(){"a", "b", "c"}},
            {'3', new(){"d", "e", "f"}},
            {'4', new(){"g", "h", "i"}},
            {'5', new(){"j", "k", "l"}},
            {'6', new(){"m", "n", "o"}},
            {'7', new(){"p", "q", "r", "s"}},
            {'8', new(){"t", "u", "v"}},
            {'9', new(){"w", "x", "y", "z"}},
        };
        List<string> res = new(1<<digits.Length);

        backtrack1Helper(digits, idx: 0, map, curStr: "", res);

        return res;
    }

    public void backtrack1Helper(string digits, int idx, Dictionary<char, List<string>> map, String curStr, List<string> res)
    {
        if(idx == digits.Length)
        {
            res.Add(curStr);
            return;
        }
        
        foreach(var c in map[digits[idx]])
        {
            //curStr.Append() is O(m) where m is max length of digits string, here 4, so very small impact.
            backtrack1Helper(digits, idx+1, map, curStr + c, res);
        }
        return;
    }
}



// # Soln. from March 2026:
public class NuAttempt1 { // Took 16 minutes   but needed some outside help because I had some syntax issues earlier (and was doing some experimentation so that took some extra time too)
    // DEPRECATED: //Assuming no input other than digits 0-9 (well question specifies 2-9, but my solution works regardless)
    // Deprecated: readonly string[] map = ["+","", "ABC","DEF","GHI", "JKL","MNO","PQRS","TUV","WXYZ"]; //order preserved (1st pos = 0 index, and so on!)
    readonly Dictionary<char, string> digChars = new(){['2'] = "abc", ['3'] = "def", ['4'] = "ghi", 
    ['5'] = "jkl", ['6'] = "mno", ['7'] = "pqrs", ['8'] = "tuv", ['9'] = "wxyz"}; //Post Solution Remark: would be better to do it using the array method above and use digits[i]-'0' of course but I chose convenience here (and I wasn't sure the -'0' (or -'2') trick worked here :) )

    public List<string> LetterCombinations(string digits) {
        List<string> result = new();
        Backtracking(digits, new(), result); //While StringBuilder would be better in performance, List is easier to handle and I know syntax better!
        return result;
    }

    // *IMPORTANT* Complexity analysis was a bit iffy (and took a while and a small peek at neetcode's TC at the end)!!
    
    //Let N = digits.Length
    //TC: O(N*4^N)                                                        //DEPRECATED because it's wrong: //[at most 4 choices for each digit] (we explore all possible "subsets", add only ones that reach full length)
    //  - We have at most 4 choices (independent of prior choices) for each of the N digits! (Total number of results!)
    //  - N is the length of string being cloned?
    //Aux. SC: O(N) [recursion stack scales linearly to AND each str grows exactly to `digits.Length` before being added]
    //Total Space: O(N*(4^N)) [max length of strings * total number of valid strings]
    public void Backtracking(ReadOnlySpan<char> digits, List<char> str, List<string> result) //code finished in 16mins 46secs
    {
        if(digits.Length == 0)
        {
            if(str.Count > 0) //this needs to be there for the case where digits.Length == 0!
                result.Add(string.Concat(str));
            return;
        }
        
        //Note that we must pick from digits in order!

        //Decision: Which of the options could it be?
        foreach(var c in digChars[digits[0]]) //the fact that we only check digits that match is basically the "backtracking" part (added this line after finishing code while writing TC and SC so it is not included in the finish time!)
        {
            //Subdecision: Should it be this char or not?
            str.Add(c);
            Backtracking(digits[1..], str, result);
            str.RemoveAt(str.Count-1);
        }
        
        return;
    }
}
