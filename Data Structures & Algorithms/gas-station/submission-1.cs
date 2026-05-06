public class Solution {
    
    const int CannotComplete = -1;
    
    public int CanCompleteCircuit(int[] gas, int[] cost) {
        // Has a touch of Kadane's algorithm to it, no?

        // IMPORTANT: if there's not enough gas for the whole journey, we can't complete our journey. 
        if(gas.Sum() < cost.Sum()) { 
            return CannotComplete;
        }

        // IMPORTANT: if there is enough gas for the journey, then we can ALWAYS complete the journey.
        // Here, we are guaranteed only 1 valid starting point from problem constraints.
        // After finding a `start` that gets us from it to the end of the array, we know:
        // 1. Already proven it works for the rest (later/right part) of the array.
        //
        // 2. The only thing left to "prove" is the wrap-around (getting from the end back to the candidate).
        //
        // 3. But because the Total Sum is positive, if you had enough gas to finish the first part, 
        //    the "leftover" gas at the end is mathematically guaranteed to be enough to cover the 
        //    "negative dips" you encountered earlier in the array.
        // > Think of it like a deck of cards. If you know the total value of the deck is positive, and you've found a starting point that doesn't "go broke" before reaching the end of the deck, you don't need to re-check the beginning. The "profit" you've accumulated by the end of the deck is exactly what you need to pay off the "debts" at the very beginning.
        int tank = 0;
        int start = 0;

        for(int i = 0; i < gas.Length; i++) {
            
            tank += gas[i] - cost[i]; // 1. refill we got at i | 2. cost to travel to next station (i+1)

            // Check if current start is invalid [and hence all the indices in path from `start` to now (`i`) must NOT be startpoint as well.]
            // NC Explantion: If at some index the tank becomes negative, it means we cannot start from any station between the previous start and this index, because they would all run out of gas at the same point.
            if(tank < 0) {
                tank = 0;
                start = i + 1; 
            }
        }

        return start;
    }
}
