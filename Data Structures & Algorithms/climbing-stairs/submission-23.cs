// # Last Actual Solution (from Early December 2024):

// For newer solution check below this one, BUT, the notes (and comments!) from this solution are still gold!

//CHECK COIN CHANGE SOLUTION AND EVERY SOLUTION IN DP SECTION (at least ones that are starred)

//--------------
// COIN CHANGE IS SUBSET SUM PATTERN DP: 
//1. Objective Differences
// Coin Change Problem: The objective is to find the minimum number of coins needed to make a given amount. Alternatively, some variations may ask for the number of ways to form the amount using the coins.
// Unbounded Knapsack Problem: The objective is to maximize the value of items you can include in the knapsack, given an unlimited supply of each item and a maximum capacity.
// The difference lies in:
// - Coin change minimizes the number of coins or counts combinations.
// - Unbounded knapsack maximizes a value.
// 2. Nature of Constraints
// Coin Change Problem:
//  - The coins have a fixed denomination but no inherent "value" apart from their use in forming the target amount.
//  - The weight (coin denomination) must sum up exactly to the target amount.
// Unbounded Knapsack Problem:
//  - Each item has a weight and value, and the goal is to maximize the total value without exceeding the weight limit.
//  - In coin change, there’s no "value" associated with coins—only their denominations are used. This makes the problem conceptually simpler but distinct.
//---------------

//[IMPORTANT!] NOT REALLY HARD! STARRED FOR BRINGING ATTENTION TO THESE NOTES ABOUT DP:

// ## DP PATTERNS: {Get Grokking DP patterns course??}

// JUST NOTICED: FOR DP, TC IS USUALLY == SC (for topdown/memo/cache) BECAUSE THE ACTUAL HEAVY LIFTING IS NEEDED ONLY TO PUT IN THEN VALUES IN THE MEMO!!!

// JUST NOTICED THE PALINDROME PATTERN CAN BE USED FOR SOMETHING LIKE `MAXIMUM SYMMETRIC SUBSEQUENCE` FROM ARRAYS (I made that phrase!) (e.g. treating array as a picture or a skyline, etc.)

// For the interview just do memoization first then explain how you might create tabulation?

// DP GENERALLY ONLY USED TO GET NUMERIC RESULTS (like number of ways to do something than the ways themselves (which would generally require recursive backtracking))
// MAYBE (at least for Palindrome problems like Longest Palindromic Substring where we can return indices which we can use to get longest str, maybe we can also return some small strings(or type) like the LONGEST STRING BUT (like in Longest Palindromic Substring) not a collection/list of strings)?

// Got these (except Fibonacci) from Neetcode's Advanced Algorithms course videos about them.

// My observation: Non-optimized tabulation ends up with a similar array to the memo we would have otherwise built for memoization (even what's inside / how to build it). 
//      (I think this can make solving tabulation based DP easier because now you know exactly what kind of array you want to build, and you have a general idea of how to build it.)

// My observation: Generally, in tabulation DP (bottom up) the order of the table (like where we start or end) doesn't matter (so you can disregard any confused comments I made about this initially),
//                      we only chose a specific order for the table if we find a case where doing so could be useful or inuitive (e.g. mirroring original martix, etc.)

// :::::::::::::::::::::::::::::
// ### Fibonacci [to calculate a value]: {wrote this one on my own, so there might be some incorrect stuff}
//  - Memoization (Top-down):
//      Really just stores the result for each input (as 1D array).
//  - Tabulation (Bottom-Up):
//      Uses a similar array to Memoization and iteratively 
//      fills the array starting from before (the first k e.g. k==2 in actual fibonacci) are manually set (to values of base cases in memoization's recursion)
//  - Optimized Tabulation (Bottom-Up): 
//      Constant space you realize you only really need as many as last k values (the number of base cases) (e.g. k==2 in actual fibonacci), so you use an array to store them.
// :::::::::::::::::::::::::::::
// ### 0/1 Knapsack (take / not take) [Optimitze cost-to-value] [More explanation: to maximize (or minimize?) cost/weight_or_count/population/profit within budget/storage/space/capacity limits]: {WROTE THIS MOSTLY, if not fully, ON MY OWN!!!}
//  TC: O(2^N) where N is number of items [from combinatrics]
//  - Memoization (top-down):
//      Uses recursive backtracking where for each item we consider adding it or not adding it (then moving onto the next one).
//          Our conditions to stop are 1. going over the weight limit or 2. running out of items.
//      We only calculate (and return) the final number of ways for each item with given capacity in POST ORDER.
//      Uses a 2D memo array that stores the result of the function call for each input pairing of indexOfElementConsidered and spaceLeft
//      Since it is memoization, it is still based on a recursive backtracking take-or-not approach.
//  - Tabulation (Bottom-Up): 
//      (Notice how the last level of post-order recursive calls in the backtracking solution will basically (since it has no context of total profit in the above recursive level) calculate max profit we can get if we only consider adding the current level 1 or 0 times given the current capacity)
//      (Our tabulation based approach uses the above idea, but since the order of items (rows) doesn't really matter, order of the table based on items doesn't matter either and there is no specific benefit of choosing any order, so we choose to start from any item(row), so we start from the first row to keep things simpler)
//      Note that order of picking doesn't matter, but we will manually enforce one to make it work/efficient.
//      As usual, you use a similar array to the memoization soln where memo[itemIdx][capacity] stores the maximum profit possible only considering items from 0 to itemIdx (whether we take it or not) when `capacity` is the capacity left.
//      We start by manually filling the whole first row(its base case) (maximum profit possible when only considering item at idx 0 for capacities 0 through maximum capacity (starting/original capacity of bag (empty bag)))
//      For each column in the rest of the rows, let row = r and c = col, we SET 
//          `memo[r][c]` TO THE MAXIMUM_OF `memo[r-1][c](skipping item at r: i.e. max profit using only items from [0,r-1] with capacity c)`
//          and `profit[r] (profit by picking only current element) + memo[r-1][c-weight[r]] (max profit possible by only considering items 0 to r-1 when capacity is the capacity left after picking current item)`
//      Clearly, at memo[N][C] where N is last item index and C is starting capacity (of empty bag),
//          we set `maximum profit when all items are being considered and the bag is empty (starting capacity)`
//          which is our answer. So, we can return it.
//  - Optimized Tabulation (Bottom-Up):
//      If you look at the above algorithm, you'll realize, for any given row, 
//      we only read values from the row prior to it,
//      and we only set values to the current row. 
//      Therefore, we can only use a two row array to get optimize SC from O(N*C) to O(2*C)==O(N)
//  - QUESTION: Does choosing the order of the indices of memo matter?

// :::::::::::::::::::::::::::::
// ### [2D] For Number of Unique Paths type problems [num unique paths from (0,0) to (ROWS-1,COLS-1) of matrix]: {WROTE THIS ON MY OWN!}
//  - It is kinda similar to 0/1 Knapsack (BOUNDED) because you can only choose 1 out of 2 nodes (down or right) for each unique path.
//  - The recursive call would start POSTORDER from (0,0) and recursively figure out how to get there from nodes above and below it. (and same for each of them) [Base cases: 1. out of bounds => 0, 2. Reached Destination=> 1]
//  - This is like Knapsack, but we reverse the order of items to basically mirror how our graph/matrix would be originally to keep things simpler/easier (to do and a lot more to be readable/understandable)
//  - However, the differences are that:
//      * We start from computing the only the last row first (base case) (each node from there has only one way to go, which is right, so everything == 1) 
//              [Because (not sure!?)
//                  1. We sort of inverted the problem to: How many ways from each node can we get to the ending, because that makes it easier/possible to calculate what we want 
//                  2. While order matters unlike in knapsack (because we need the last index of the last row first and the ones directly connected to it first), 
//                          we can flip the matrix and still keep the same order as long as we also 'flip' our logic.
//                  3. This mirrors the original matrix that was provided (or should exist if only given dimensions) and this makes things simpler  (e.g. directions are consistently up and down). 
//                          So, even if we could flip this whole matrix (which would require changing the logic a LITTLE), we won't do it because this way we get to keep things consistent with orignal matrix.
//      * Then for each node, number of ways to get from there to original node is the number of ways to get to the destination is the 
//          sum of number of ways to get to destination from the node to the right and node to the left (this is where DP saves us).
//      * Recursion based solution also uses similar approach just using recursion to do it and memoization just memoizes these results.

// :::::::::::::::::::::::::::::
// ### Unbounded Knapsack [Optimized cost-to-value, but can pick same item unlimited times (unlimited quantities of items)] {wrote all of this on my own LET'S GOOO!!! Will watch neetcode video anyway, but won't edit notes unless something new comes up,}
//  {Like 0/1 Knapsack. Seems to be more common in interviews (according to NeetCode).}
//  TC: O(2^C) where C is capacity [from combinatrics]
//  - Memoization (top-down):
//      Uses Post-Order Recursive Backtracking to decide whether to add current item or not, BUT
//          there are more than n+2 branches in each recursion where n is the number of times this item can be kept with the current capacity: 
//              1. {RECURSIVELY} Do not pick item and move to next index. 
//              3. `FOR i=1; i*curItm.Weight <= capacityAtCurrentBranch; i++`:
//                      Inside Loop: {RECURSIVELY} Pick item for i-th time and move to next index. 
//                  LIKE WE STUDIED IN BACKTRACKING SECTION: 
//                      HERE, CLEARLY THE ORDER WE PICK ITEMS IN DOESN'T MATTER (so it's fine to pick all of this item we will be taking at once)
//                      THIS LOOP WILL COVER ALL POSSIBLE NUMBER OF TIMES WE CAN PICK THIS ITEM.
//              4. Note that we only really need to store the max profit at given capacity, so number of repetitions don't need to be tracked, just all repetions should be considered and then the max one of those is stored in the memo for a node/function_call.
//      Here, an interesting observation would be, if we have already computed MAX PROFIT FOR EACH (curItemIdx, capacity), we can just cache that and return everytime this question is asked.
//          This is where the memo/cache comes in.
//  - Tabulation (Bottom-Up):
//      As always, non-optimized tabulation ends up with a similar array to memoization, therefore, let's think of how we can build it iteratively.
//      (Notice how the last level of post-order recursive calls in the backtracking solution will basically (since it has no context of total profit in the above recursive level) calculate max profit we can get if we only consider adding the current level any number of times given the current capacity)
//      (Our tabulation based approach uses the above idea, but since the order of items doesn't really matter, we can start from any item, so we start from the first row)
//      Due to the above 2 lines, we start from the generating the first sub array of what would have been the memo:
//          First, foreach capacity in [0, COLS): table[0][capacity] = max profit if we can only include first item unlimited times for given capacity. (should be easy)
//          Then, foreach i-th row(1 row for each item), starting from i = 1:
//                      table[i][1] = table[i]
//                      foreach capacity in [1, COLS): //we start form 1 because 0 capacity => 0 profit.
//                          noTakeProfit = table[i-1][capacity]; //MAX CAPACITY IF WE DON'T INCLUDE CURRENT ITEM
//                          takeProfit = int.MinValue;
//                          if(capacity-weight[i] >= 0)
//                          {
//                              takeProfit = profit[i] + table[i][capacity-weight[i]];
//                              //The above line is Different from Bounded Knapsack (to handle repeatability of same item)
//                              //It is == profit of including current item once + table[i][capacity-weight[i]]
//                              //Where table[i][capacity-weight[i]] == Max Profit if we could include any items from [0, i] with the remaining capacity after including 1st item once.
//                              //    (notice i can be used again here, unlike in Bounded Knapsack).
//                          }
//                          table[i][capacity] = Math.Max(takeProfit, noTakeProfit); // MAX_PROFIT_AT_CURRENT_CAPACITY_IF_WE_ONLY_CONSIDER_CURRENT_ITEMS_OR_ONES_BEFORE_IT
//                      NOTICE THAT THIS WORKS BECAUSE IN ANY row table[i], table[i][1] gets filled by `noTake`, and the rest use it directly or indirectly for their take (repeating same item), while comparing with the row above for noTake.
//                      Then at table[i][2], if weight limit allows, we set it to MAXIMUM_OF profit[i] + max of repeating at i most 1 times just OR not taking it at all.
//                      Here, we don't really care HOW (repetition or not (take/noTake chain)), we just want the maximum profit if we have this much capacity.

//          Then the result is in the final column of the final row of the table, because it STORES MAXIMUM PROFIT IF WE CAN CONSIDER ALL COMBINATION OF
//              ITEMS ([0,numAllItems]), FOR THE FULL CAPACITY (capacity of EMPTY KNAPSACK).
//  - Optimized Tabulation (Bottom-Up):
//      Notice we only need first two rows. The rest is like Bounded KnapSack.
// :::::::::::::::::::::::::::::
// ### Longest Common Sequence: (sometimes Longest Subsequence Following A Property)
//  {NC: TC=2^(s1Len+s2Len), SC: O(N+M) recursion/call_stack max depth}
// {FOR NOW JUST WATCHED THE NEETCODE VIDEO (only the recursive part and a little of memoization)}
// {NC: APPARENTLY QUIRE EASY TO OPTIMIZE AFTER FINISHING BRUTEFORCE DFS}
// {NC: Apparently, THERE ARE MULTIPLE VARIATIONS OF THIS e.g. LONGEST INCRESING SUBSEQUENCE, array input LCS, string input LCS, and maybe even other factors, etc.}
// {Recursion/Sub problems on mismatch: 1. skip(++) current index for first string (other remains as is), 2. skip(++) current index for second string (other remains as is).}
// {Recursion/Sub problems on equality: skip current index for both strings (++)}
// {SELF NOTE: 
//      THERE'S A **GREEDY** ASPECT HERE:
//      WE DON'T NEED CASES WHERE WE PICK NEITHER OR ONLY ONE OF THEM EACH BECAUSE FOR A SUBSEQUENCE: 
//      Assume "A" MATCHES, THEN EVEN IF THERE'S ANOTHER A LATER, IT DOESN'T MATTER BECAUSE there's two possible cases:
//          1. "AAB" "AB" then even if we only consider first A and skip the second one OR skip the first a pick the second one, we still get the subsequence. But 
//                  But as you can see, for this case, it doesn't matter.
//          2. BUT if it was "ABA" ONLY picking the first A would work.
//      From 1 and 2, we can see it is better to be GREEDY and pick the first matching element.
//           (cuz this is a subsequence not a substring, we don't need to check the case where both match yet we skip 1 (once for each) or skip both to cover all cases).
//  }
//    {Obviously, better to pass start indices than substrings due to time complexity / performance.)
//  {Finish condition: the smaller string was finished being traversed}
//  {for equality: 1+result from call, for inequality: MAX of results from the two calls}
//  - Memoization (top-down):

//  - Tabulation (Bottom-Up):

//  - Optimized Tabulation (Bottom-Up):
// (finish this later??)

// :::::::::::::::::::::::::::::
// ### Palindromes: [NOT **TYPICAL** DP (memoization, or tabulation, etc. doesn't apply), BUT A DIFFERENT FORM OF DP] (DP in layman's terms really only cares about reusing what we already found out to solve any problems that would require that same knowledge)
// [ALL PALINDROMIC SUBSTRINGS]
// {FOR NOW JUST WATCHED THE NEETCODE VIDEO (only the recursive part and a little of memoization)}
//
// JUST NOTICED THE PALINDROME PATTERN CAN BE USED FOR SOMETHING LIKE `MAXIMUM SYMMETRIC SUBSEQUENCE` FROM ARRAYS (I made that phrase!) (e.g. treating array as a picture or a skyline, etc.)
//
//  Uses the fact that a{nonPalindromicSubstring}a is NOT a palindormic substring (for search space culling, kinda like backtracking)
//  NEED TO CHECK FOR TWO SEPARATE CASES:
//  Case 1: Longest odd length palindrommic substring:
//      We start by checking all palindromic substrings where current character is middle character, 
//          assuming it is palindromic (because it is a single character) and keep expanding outwards until first time the new left and right characters don't match/are_unequal (or go out of array/string bounds).
//          (we stop due to above condition(l!=r) or if we go out of array/string bounds)
//  Case 2: Longest even length palindromic substring:
//      For each pair of 2 consecutive characters in the string, we check if they're both equal. If they are:
//          We keep expanding both sides by 1 until we get unequal characters (l!=r) or we hit array/string bounds.
// Return: Maximum of both cases.
// CHECK MY SOLUTION FOR `LONGEST PALINDROMIC SUBSTRING` AND `PALINDROMIC SUBSTRINGS`
// (finish this later??)

// :::::::::::::::::::::::::::::
// ### READ MY SOLUTION ON RACECAR LEETCODE PROBLEM??
// :::::::::::::::::::::::::::::
// ### DP on strings, bitmask, and digits???
// :::::::::::::::::::::::::::::

// public class Solution {
//     public int ClimbStairs(int n) {     
//         if(n<3)
//             return n;
//         //Seems to be the Fibonacci pattern for DP.
//         //at i==0 we are at the ground.
//         int prevPrev = 1; //i==1 from i==0
//         int prev = 2; //i==2 from i == 0 (two once or one twice)
//         for(int i = 3; i < n+1; i++)
//         {
//             //curNumWays = (numWaysToGetAtPrev*numWaysFromPrev + numWaysToGetAtPrevPrev*numWaysFromPrevPrev). Here, as explained in the actual assignment, numWaysFromPrev == 1 & numWaysFromPrevPrev == 1.
//             int curWays = 
//                 (prev) //Only one possible choice to get here from prev, so we don't need prev+1 or prev+2 as I tried doing earlier.
//                 + (prevPrev); //Only one possible choice to get here from prevPrev: two steps at once  (one step twice NOT INCLUDED because it is included in prev)
//             prevPrev = prev;
//             prev = curWays;

//             //Note: at i-th iteration, prevPrev = {curWays at i-2th iteration} and prev = {curWays at idx i-1th iteration}
//             //So, at i == (n-1), prev becomes == `curWaysAtN-1 = curWaysAtN-3 + curWaysAtN-2`
//         }

//         return prev; //for i==n
//     }
// }



// Solution from 09-05(May)-2026: (NuAttempt1)
public class Solution {
    public int ClimbStairs(int n) {     
        // I MESSED UP EARLIER BECAUSE I WAS TRYING BASICALL 2 * waysAfter1Step * waysAfter2Steps!
        // *IMPORTANT* number of ways at any step is just: waysAfter1Step +  waysAfter2Steps [for top down]
        // OR ways2StepsAgo + ways1StepAgo [for bottom up]

        return 
            // RecPostOrder(n, new());
            BottomUpSpaceOptimized(n);
    }

    //TC: O(n), SC: O(1)
    private int BottomUpSpaceOptimized(int n) {
        if(n < 3)
            return n;
        
        int prevPrev = 1;
        int prev = 2;

        for(int i = 3; i < n; i++) { //How many ways are there to get to `i`
            (prevPrev, prev) = (prev, prevPrev + prev);
        }

        return prevPrev + prev;

        //just as a side note, the non-space optimized solution would be using an array to store all of computed ways for each i, so in that case SC would be O(N)
    }

    //TC: O(n), SC: O(n) [max depth of recursion tree and thus the recursion stack]
    private int RecPostOrder(int n, Dictionary<int, int> memo) {
        // WRONG: if(n == 0) return 1;
        if(n == 1) return 1;
        if(n == 2) return 2;

        if(memo.ContainsKey(n)) 
            return memo[n];

        var waysAfter1Step = RecPostOrder(n-1, memo);
        var waysAfter2Steps = RecPostOrder(n-2, memo);

        //const int numberOfPossibleStepsHere = 2; //because we handled n == 1 (and == 0) above as base case(s)!
        var totalNumberOfWays = waysAfter1Step + waysAfter2Steps; //numberOfPossibleStepsHere * waysAfter1Step * waysAfter2Steps;
        
        memo[n] = totalNumberOfWays;

        return totalNumberOfWays;
    }
}
