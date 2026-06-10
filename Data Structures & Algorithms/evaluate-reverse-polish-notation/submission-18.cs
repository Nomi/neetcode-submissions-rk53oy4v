// 2026.2 - Solution (review):

public class Solution {
    static readonly IReadOnlyDictionary<string, Func<int, int, int>> Ops = new Dictionary<string, Func<int, int, int>>() {
        ["+"] = ((x, y) => x + y),
        ["-"] = ((x, y) => x - y),
        ["/"] = ((x, y) => x/y),
        ["*"] = ((x, y) => x * y)
    }; //Would've been populated by Dependency Injection in Prod. (is Strategy Pattern, btw.)
    
    // TC = O(N)
    // Aux. SC = O(N)       //Actually, Aux. SC = (T+1)/2.0 [exact max number of elements in stack for RPN]
    public int EvalRPN(string[] tokens) {
        Stack<int> stk = new();
        foreach(string tok in tokens) {
            if(Ops.TryGetValue(tok, out var op)) {
                var y = stk.Pop();
                var x = stk.Pop();
                var res = op(x, y);
                stk.Push(res);
                continue;
            }
            stk.Push(int.Parse(tok));
        }    

        return stk.Pop(); //Last element
    }
}


// 2026.1 - Solution:

// public class Solution {
//     public int EvalRPN(string[] tokens) { //TC- O(N) where N = num tokens SC- O(N+M) where M is number of operations, but O(N) in current state (and possibly any reasonable state it could be in)
//         //Assuming valid input! (including order and what characters appear)

//         //Given constraints don't seem to show any overflow concerns!
//         //"Assume that division between integers always truncates toward zero." => Integer division can be used directly
        
//         //## Soln. Start:
//         Dictionary<string, Func<int, int, int>> op;// = new(); //nvm. should've been string. I'm too tired for this. (or I miss details?)//had <int, instead of <char :( //might have been better to create a switch case based function (strategy pattern?)
//         //Actually no, this ended up being better due to containskey! //of course performance wise, normal functions would be better.
//         //Also, this also counts for strategy pattern!

//         op = new() { //almost forgot to change these keys to double quotes even after changing above to string XD
//             { "+", (x, y) => x + y }, //had to check my syntax on google but I was right!
//             { "-", (x, y) => x - y },
//             { "*", (x, y) => x * y },
//             { "/", (x, y) => x / y }
//         };
        
//         Stack<int> stk = new(); //SC- O(N)
//         foreach(var str in tokens)
//         {
//             var top = stk.Count > 0 ? stk.Peek().ToString() : "N/A";
//             Console.WriteLine($"str = {str}, stack top = {top}");
//             if(op.ContainsKey(str))
//             {
//                 // stk.Push(op[str](stk.Pop(),stk.Pop())); //Order here was probably messed up, even I dont know what the order is.
//                 int b = stk.Pop();
//                 int a = stk.Pop();
//                 //[Important] the above order is because for [a,b,'-'],
//                 //the stack looks like [b,a] when we encounter '-', 
//                 //where top is on the left. Therefore, to get a-b as 
//                 //we want, we pop to get b first, then pop to get
//                 //a first and b second.
//                 stk.Push(op[str](a, b));
//                 continue; //forgot this. I really am too sleepy and tired for this!
//             }
//             stk.Push(int.Parse(str)); //would've used tryparse in prod and handled cases
//         }
        
//         //would check length of stk in prod!
//         return stk.Pop();
//         //Finished in 21:08, but I would consider this 15 minutes, because I wouldn't make this many dumb mistakes if I was awake in the truest sense.
//         // In fact, I had base logic ready at 8 minutes, just the fixes here and there I forgot or overlooked due to exhaustion and headache/migraine!

//         //Gets 100% in both runtime and memory on NC!
//     }
// }


// Last Actual Solution:
// public class Solution {
//     public int EvalRPN(string[] tokens) {
//         //are we guaranteed input is correct?
//         return attempt1(tokens);
//     }

//     static readonly Dictionary<string, Func<int, int, int>> operations = new(){
//         { "+", (x, y) => x + y },
//         { "*", (x, y) => x * y },
//         { "-", (x, y) => x - y },
//         { "/", (x, y) => (int)((double)x / y) } //GOTTA REMEMBER!?!?!
//     };

//     public int attempt1(string[] tokens)
//     {
//         Stack<string> stk = new();
//         foreach(string s in tokens) //Currently assuming the input is always correct.
//         {
//             if(int.TryParse(s, out int num))
//             {
//                 stk.Push(s);
//                 continue;
//             }
//             //else:
//             int.TryParse(stk.Pop(), out int b);//later number is on top of stack!
//             int.TryParse(stk.Pop(), out int a);//earlier number will be deeper in the stack, of course!
//             //[Important] the above order is because for [a,b,'-'],
//             //the stack looks like [b,a] when we encounter '-', 
//             //where top is on the left. Therefore, to get a-b as 
//             //we want, we pop to get b first, then pop to get
//             //a first and b second.
//             stk.Push(operations[s](a,b).ToString());
//             // Console.WriteLine($"{a} {s} {b} = {stk.Peek()}");
//         }
//         int.TryParse(stk.Pop(), out int res);
//         return res;
//     }
// }

