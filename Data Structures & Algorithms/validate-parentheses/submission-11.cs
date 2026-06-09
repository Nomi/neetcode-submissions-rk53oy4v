public class Solution {
    public bool IsValid(string s) { //TC=O(N), SC=O(N)
        Dictionary<char, char> closeToOpen = new() {
            [')'] = '(',
            ['}'] = '{',
            [']'] = '['
        };
        HashSet<char> openers = new(closeToOpen.Values);

        Stack<char> stk = new();
        foreach(char c in s)
        {
            if(closeToOpen.ContainsKey(c) && (stk.Count == 0 || stk.Peek() != closeToOpen[c])) //1. unbalanced closing OR 2. not a valid bracket to close!)
            {
                return false;
            }
            else if(closeToOpen.ContainsKey(c)) //is a closer, and we have matching opener at top of stack already!
            {
                stk.Pop();
            }
            else if(openers.Contains(c)) //is an opener
            {
                stk.Push(c);
            }
            //else it is a random char we don't care about (not opener or closer)
            //well the problem doesn't say what to do otherwise anyway??
        }

        return stk.Count == 0; //took 13 minutes!
    }
}


// Last Actual Solution
// public class Solution {
//     public bool IsValid(string s) {
//         return attempt1(s);
//     }
    
//     public bool attempt1(string s)
//     {
//         Stack<char> stack = new();
//         Dictionary<char,char> openToClose = new(){
//             {'(',')'},
//             {'{','}'},
//             {'[',']'}};
//         HashSet<char> close = new(){')','}',']'};

//         for(int i=0;i<s.Length;i++)
//         {
//             char c = s[i];
//             if(stack.Count==0||openToClose.ContainsKey(c))
//             {
//                 if(close.Contains(c))
//                     return false;
//                 stack.Push(c);
//                 continue;
//             }
//             char lastChar = stack.Pop();
//             if(!openToClose.ContainsKey(lastChar))
//                 return false;
//             if(c!=openToClose[lastChar])
//                 return false;
//         }
//         if(stack.Count>0)
//             return false;
//         return true;
//     }
// }

