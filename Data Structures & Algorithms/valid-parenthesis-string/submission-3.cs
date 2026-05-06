public class Solution {
    const char open = '(';
    const char close = ')';
    const char wildcard = '*'; // can be '(', ')', or '' [empty].
    
    public bool CheckValidString(string s) {
        //Wildcard can be empty string, so WRONG: // if(s.Length % 2 != 0)   return false;


        int minOpen = 0; //MIN POSSIBLE UNBALANCED OPENS (PREFER WILDCARDS TO BE CLOSED, BUT CAN BE REBALANCED BY TREATING SOME AS EMPTY OR '(' BY RESETTING BACK TO 0 AS LONG AS THERE ARE ENOUGH WILDCARDS (proven by maxOpen > 0))
        int maxOpen = 0; //MAX POSSIBLE UNBALANCED OPENS (when all wildcards are `open`)
        foreach(var c in s) {
            if(c == open) {
                minOpen++;
                maxOpen++;
            }
            else if(c == close) {
                minOpen--;
                maxOpen--;
            }
            else { //wildcard (or could check and throw for unsupported)
                minOpen--;
                maxOpen++;
            }

            if(maxOpen < 0)
                return false;
            
            //*IMPORTANT* The rebalancing is THE GREEDY PART! We just set the minOpen path to the smallest amount of non-closing as makes sense! (we don't need to simulate the whole sequence, just the maths)

            // Rebalancing: we treat earlier `*` as `` [empty] OR `)`
            //
            // If there were not enough (or at all) wildcards before this to rebalance, MAX open would be < 0 and
            // hence the above condition would handle it. If maxOpen == 0, minOpen path is the same as maxOpen 
            //   (the lowest number of '*' we need to rebalance in the min path happens to be all of them!)
            if(minOpen < 0) 
                minOpen = 0; //I didn't even think of this, got it from the neetcode solution I peeked at earlier :'( 
        }

        return (minOpen == 0);
    }
}
