public class Solution {
    public int ClimbStairs(int n) {     
        return RecPostOrder(n, new());
    }

    public int RecPostOrder(int n, Dictionary<int, int> memo) {
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
