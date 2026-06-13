public class Solution {  
    public List<string> GenerateParenthesis(int n) {

        // # Soln. from March 2026:
        return (new NuAttempt1()).GenerateParenthesis(n); // Read comments from 2024 solution! (and also from NuAttempt1 ?)
        //Older solution from 2024 is now officialy worse (but maybe with good comments) because it uses string current unlike my new list!

        // # Soln from September 2024:
        // int openingRemaining = n;
        // int closingRemaining = n;

        //only 1 type of bracket means we
        //don't need a stack to check and we can
        //just use the count of open and closed brackets
        //to figure out if it is valid!
        
        // List<string> res = new();
        // recur1(n, 0, 0, "", res); //USE THE SECOND IF CONDITION FROM THE PROVIDED SOLUTION ON NC.IO
        // return res;
    }
    void recur1(int n, int opn, int cls, string current, List<string> res)
    {
        if(cls==n&&opn==n) //used all brackets.
        {
            res.Add(current);
            return;
        }

        if(opn<n)
            recur1(n, opn+1, cls, current+"(",res);
        if(cls<opn) //(it is this simple because only one type of brackets!)
            recur1(n, opn, cls+1, current+")",res);
    }
}



public class NuAttempt1 {  
    public List<string> GenerateParenthesis(int n) {
        List<string> validStrings = new();
        Backtracking(n, 0, 0, new(), validStrings);
        return validStrings;
    }
    

    // Complexities:
    // TC: Not exact, but strictly upperbounded by basically all possible ways to pick either '('or ')' (2 choices/options) 
    //          for all the 2N places (independently, i.e. elements are allowed to repeat!)
    //      == O(2^(2N)) == O(4^N) [do note that since we don't account for the pruned branches, this is just something where we will never go, nevermind above.]
    // Aux. SC: O(N) [at any moment cur or recursion stack can only go upto 1 full path, i.e. 2N]
    // Total Space: O(N*(4^N)), just like TC this is an overshooting upperbound. Read up on that to know more.

    // 13-06-26 Update: In combinatorics, the exact number of valid parenthesis combinations for $n$ pairs is 
    // mathematically proven to be the $n$-th Catalan Number == 4^n/n*sqrt(n), so tc is just that multiplied by n for string creation.
    
    //Wrong, so DEPRECATED: // NVM seems like i'm wrong (it's somehow more like subsets???) // TC: (this is just permutations, but with some limits so) O(N!) is the upperbound // Aux.  SC: O(N) //at any point we can only recurse upto depth N (since all elements are used) // Total Space: O(N + N*N!) = O(N*N!) //N! is upperbound of number of //                        valid strings as this is permutations with limits and N is the length of each string  
    public void Backtracking(int pairs, int open, int close, List<char> cur, List<string> validStrings) //Finished in 22 minutes approx.
    {
        //At first glance seemed just like pick / not pick
        //Also, a moment later I realized all I needed was to track an `int balance` state.
        //On writing the function signature, I realized I should use open and close separately.
        // Checked the example on NC and realized we have to use all the parenthesis!

        // if(close > open || open > pairs || close > pairs) //already checking in the main body!
        //     return; //invalid!

        if(close == pairs && open == pairs) //assuming that n is never 0, which i just checked is correct according to constraints!
        {
            validStrings.Add(string.Concat(cur)); //had some trouble with the syntax here, initially using new(cur) [this was a silly miss], then string.Join, then finally Concat worked
            return;
        }

        // Decision: which can we even pick for this place?     (did this after finishing, so not included in time)

        if(open != pairs) //Pick '('
        {
            cur.Add('(');
            Backtracking(pairs, open+1, close, cur, validStrings);
            cur.RemoveAt(cur.Count-1);
        }

        if(close < open) //Pick ')'
        {   //open<=pairs, so close<open ensures close<pairs => it is not needed!
            cur.Add(')');
            Backtracking(pairs, open, close+1, cur, validStrings);
            cur.RemoveAt(cur.Count-1);
        } //Unlike `not pick`, we need to do cleanup here for above recursion level
    }
}
