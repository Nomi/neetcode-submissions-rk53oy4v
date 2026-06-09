// 2026.2: Solution (review):

public class MinStack { //One stack, encoded diff version.

    long min; //Stores current minimum value
    Stack<long> encodedStk; // Stores value - minimum (in case new value is new minimum, stores value-previous_minimum)

    public MinStack() {
        // min = 0; //SHOULD BE IN PUSH! (first push only!)
        encodedStk = new();
    }
    
    public void Push(int val) {
        if(encodedStk.Count == 0) {
            encodedStk.Push(0);
            min = val;
            return;
        }

        var diff = val - min; // new val, previous min
        encodedStk.Push(diff);

        if(diff < 0) //new min (val < min)
            min = val; // new min
        // Clearly, we can get current value val by: 1. if diff >= 0: by diff + min(cur) OR 2. if diff<0: by min!
    }
    
    public void Pop() {
        var curTop = this.Top();
        var diff = encodedStk.Pop();    
        if(diff >= 0)
            return; // This was not a new minimum at the time of adding.

        // It was a new min, overwriting previous min:
        min = min - diff; //(maths explained below)


        // Whenever new value added is the new minimum:
        // We have:
        //  `new_diff = new_value - prev_min`
        // => `prev_min = new_value - new_diff`
        // And, 
        //  `new_min = new_value`
        // As such:
        // `prev_min = new_min - new_diff`
    }
    
    public int Top() {
        var diff = encodedStk.Peek();
        if(diff < 0) //if the top is the new minimum
            return (int)min;
        return (int)(diff + min);
    }
    
    public int GetMin() {
        return (int) min;
    }
}



// 2026.1: Solution (s):

public class MinStack_TwoStacks {
    // Take a look at the One Stack encoded MinStack solution (MinStack_OneStack_EncodedValue)?  It's kinda like diff and
    //tbf I had some idea on how to do it, but I did read the hint about "Perhaps we should consider a prefix approach.", but from what I remember, I was already somewhat (a lil bit) in that train of thought, but maybe not exactly!
    Stack<int> values;
    Stack<int> minimumHistory;

    public MinStack_TwoStacks() {
        values = [];
        minimumHistory = [];
    }
    
    public void Push(int val) {
        values.Push(val);
        if(minimumHistory.Count == 0)
            minimumHistory.Push(val);
        else
            minimumHistory.Push(Math.Min(minimumHistory.Peek(), val)); 
    }
    
    public void Pop() {
        //could throw exception in production on empty stack
        values.Pop();
        minimumHistory.Pop();
    }
    
    public int Top() {
        //could throw exception in production on empty stack or return null!
        return values.Peek();
    }
    
    public int GetMin() {
        return minimumHistory.Peek();
    }

    //Done in 10 min, but did take tiiiinyyy hint (also i was exhausted tbf)
}

/* * Was that enough for L4?
 * The short answer: For that specific problem, yes. But from a Scientific Realist perspective, 
 * "enough" at the L4 level usually includes the The Trade-off Talk. An L4 interviewer 
 * isn't just looking for the Stack solution; they want to know if you can navigate the 
 * "Single Stack" trade-offs.
 * * The "Single Stack" Follow-up (The Nudge)
 * They might ask: "This works great, but we are in a memory-constrained environment. 
 * Can you do this with only one stack of integers?"
 * * There are two ways to handle this follow-up:
 * * 1. The "Interleaved" Approach: 
 * - Logic: You push the value, and if it's a new minimum, you push the old minimum 
 * first as a "checkpoint."
 * - Risk: High logic complexity in the Pop() function.
 * * 2. The "Encoded" Approach (The Math Way): 
 * - Logic: You store the difference between the value and the current minimum. 
 * If the stored value is negative, it means a new minimum was found.
 * - Risk: Potential for integer overflow (val - min) when dealing with 
 * int.MinValue or int.MaxValue.
 */


public class MinStack_OneStack_EncodedValue {
    private long min;
    private Stack<long> stack;

    public MinStack_OneStack_EncodedValue() {
        stack = new Stack<long>();
    }

    public void Push(int val) {
        if (stack.Count == 0) {
            stack.Push(0L);
            min = val;
        } else {
            stack.Push(val - min);
            if (val < min) min = val;
        }
    }

    public void Pop() {
        if (stack.Count == 0) return;

        long pop = stack.Pop();

        if (pop < 0) min -= pop;
    }

    public int Top() {
        long top = stack.Peek();
        return top > 0 ? (int)(top + min) : (int)(min);
    }

    public int GetMin() {
        return (int)min;
    }
}


// Last Actual Solution:
// public class MinStack {
//     //THESE PROVIDED CONSTRAINTS MIGHT SERVE AS GOOD CLARIFYING QUESTIONS???
//     //-2^31 <= val <= 2^31 - 1. //fits in int32?
//     //pop, top and getMin will always be called on non-empty stacks.
    

//     //Solution: We clearly just use another place that keeps
//     // track of the minimum at each level of insertions,
//     // so that when Pop is done, instead of recalculating
//     // we can just remove the last value from both values
//     // and minumum and have the last elements be the up to date
//     // without having to recalculate maximum.
    
//     // LinkedList<int> values;
//     // LinkedList<int> minimums;
//     Stack<int> values;
//     Stack<int> minimums;
//     public MinStack() {
//         values = new();
//         minimums = new();
//     }
    
//     public void Push(int val) {
//         values.Push(val);
//         if(minimums.Count==0)
//             minimums.Push(val);
//         else
//             minimums.Push((int)Math.Min(val,minimums.Peek()));
//     }
    
//     public void Pop() {
//         values.Pop();
//         minimums.Pop();
//     }
    
//     public int Top() {
//         return values.Peek();
//     }
    
//     public int GetMin() {
//         return minimums.Peek();
//     }
// }

