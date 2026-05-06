public class Solution {
    const char open = '(';
    const char close = ')';
    const char wildcard = '*'; // can be '(', ')', or ' '.
    
    public bool CheckValidString(string s) {
        //Wildcard can be empty string, so WRONG: // if(s.Length % 2 != 0)   return false;


        int minOpen = 0; //MIN POSSIBLE UNBALANCED OPENS (when all wildcards are `close` OR empty)
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
            
            // Rebalancing: we treat earlier `*` as ` ` OR `)`
            //
            // If there were not enough (or at all) wildcards before this to rebalance, MAX open would be < 0 and
            // hence the above condition would handle it (wait but what if maxOpen == 0??? We can't rebalance thhen??) 
            if(minOpen < 0) 
                minOpen = 0; //I didn't even think of this, got it from the neetcode solution I peeked at earlier :'( 
        }

        return (minOpen == 0);
    }
}
