// 2026.2: Solution (review):
public class Solution {
    public int MaxProfit(int[] prices) {
        if(prices is not {Length: > 0})
            return 0;

        int minPriceSoFar = prices[0];
        var maxProfit = 0;//since we can choose to not make any transactions
        for(int i = 1; i < prices.Length; i++) {
            if(minPriceSoFar <= prices[i]) {
                var curProfit = prices[i]-minPriceSoFar;
                maxProfit = Math.Max(maxProfit, curProfit);
            } 
            else {
                minPriceSoFar = prices[i];
            }
        }

        return maxProfit;
    }
}




// 2026.1: Solution:

// public class Solution {
//     public int MaxProfit(int[] prices) { //TC: O(N), SC:O(1)
//         // price[i] -> price on i-th day
//         // Output:
//         // - let x = 1 day where we buy all
//         // - let y = 1 day where we sell all
//         // - x < y
//         // return max profit (prices[i]-prices[j]) where i and j is the interval of selling and buying that makes us the most money

//         //first thoughts: since this is in sliding window section of problems, it has to do with sliding windows.
//         // Clearly variable sized windows. 
//         // We could possibly use a variation of kadane's algorithm here

//         if(prices==null || prices.Length<2)
//             return 0;
            
//         int minPriceSoFar = int.MaxValue;
//         // int maxPriceSinceMin = int.MinValue; //We need to keep track of the profit because that's what we return and it would make our loops and logic more complicated to use this instead
//         int maxProfit = 0;
//         // Okay, so my Last Actual Solution was basically this approach, but better. I used a different approach today tho.
//         // for(int l=0; l<(prices.Length-1); l++) //detected after writing next solution, but l++ should not be here. It would be handled by l=r.
//         // {
//         //     if(prices[l] < minPriceSoFar)
//         //         minPriceSoFar = prices[l];
//         //     int r=l+1;

//         //     int curProfit = 0;

//         //     while(r<prices.Length && curProfit >= 0) //almost messed up by having only `curProfit > 0` instead of `>=`
//         //     {
//         //         curProfit = prices[r] - prices[l];
//         //         if(curProfit > maxProfit)
//         //         {
//         //             maxProfit = curProfit;
//         //         }
//         //         r++;
//         //     }

//         //     if(curProfit<0)
//         //     {
//         //         l = r;
//         //     }
//         // }

//         //Midway through above solution I found the actual solution, which was just a bit of a rewrite of the inner loop of the above solution.
//         // Why? I could have used the above loop, but the minPriceSoFar variable would have been redundant as l should be the index of min price so far in that loop.
//         //this solution also better shows why it is o(n) though tbh it's just as h
//         minPriceSoFar = prices[0]; //if we treat this as the left pointer:
//         for(int i=1; i<prices.Length; i++)
//         {
//             var curProfit = prices[i] - minPriceSoFar;
//             if(curProfit > maxProfit)
//             {
//                 maxProfit = curProfit;
//             }
//             if(curProfit < 0) //we found new min price
//             {
//                 minPriceSoFar = prices[i];
//             }
//         }
//         return maxProfit; //took me 25 minutes overall, but I did try to do it while I was sleepy yesterday and I remembered not much from it so not that helpful, but I did remember seeing the max and min variables somewhere.
//     }
// }


//Last Actual Solution:
// public class Solution {
//     public int MaxProfit(int[] prices) {
//         if(prices.Count()<2)
//             return 0;
//         int maxProfit = 0;
//         int l=0; int r=1;
//         while(r<prices.Count())
//         {
//             if(prices[r]>prices[l])
//                 maxProfit=(int)Math.Max(maxProfit, prices[r]-prices[l]);
//             else
//                 l=r;
//             r++;
//         }
//         return maxProfit;
//     }
// }

