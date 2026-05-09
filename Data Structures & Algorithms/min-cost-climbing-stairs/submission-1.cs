// # Last Actual Solution (from Early December 2024):
// public class Solution {
//     public int MinCostClimbingStairs(int[] cost) {
//         //This is like fibonacci, so the shared basecase would be right at the last step.
//         //Also like my solution to `climbing stairs` so check that.
//         //I HAD AN ERROR BECAUSE JUST PAST LAST INDEX IMPLIES nubCistAtiPlust2 = 0 and our loop starts from i = cost.Length-2
//         int minCostAt_iPlus2 = cost[cost.Length - 1];
//         int minCostAt_iPlus1 = cost[cost.Length - 2];
        
//         for(int i = cost.Length - 3; i >= 0; i--)
//         {
//             int curCost = cost[i];
//             curCost += (int)Math.Min(minCostAt_iPlus1, minCostAt_iPlus2);
            
//             minCostAt_iPlus2 = minCostAt_iPlus1;
//             minCostAt_iPlus1 = curCost;
//         }
        
//         //TOOK ME  20 MINUTES TO FIGURE OUT (and even chatGPT had to find it) that:
//         //  After iterating through all steps, the minimum cost to reach the top can be achieved by starting either from step 0 or step 1. Therefore, the final return value should be the minimum of the costs associated with these two starting points.
//         // SHOULD'VE READ THE DESCRIPTION BECAUSE IT STATES: You may choose to start at the index 0 or the index 1 floor.
//         return (int)Math.Min(minCostAt_iPlus1, minCostAt_iPlus2);;
//     }
// }



// # Solution from 09-05(May)-2026:
public class Solution { //Took 10 minutes for the code, but 6 minutes for debugging the stupid return line! Also, yet again, I missed the choice to start at first or second index!
    public int MinCostClimbingStairs(int[] cost) {
        // Set up base cases:
        int minCostFrom2StepsBack = cost[0];
        int minCostFrom1StepBack = cost[1];
        
        for(int i = 2; i < cost.Length; i++) { 
            //we're calculating total cost to jump from each (could be for any of the next 2!)
            
            var minCostToGetHere = Math.Min(minCostFrom2StepsBack, minCostFrom1StepBack);

            // Set up for next:
            minCostFrom2StepsBack = minCostFrom1StepBack;
            minCostFrom1StepBack = cost[i] + minCostToGetHere;
        }

        return Math.Min(minCostFrom2StepsBack, minCostFrom1StepBack); //at i == n, this is just the last and second last step (which are the ones we need to choose from!)
        //I SPENT 6 MINUTES DEBUGGING WHY MY SOLUTION WASNT WORKING,
        // WHEN I WAS JUST STUPIDLY RETURNING `cost[^1] + Math.Min(prevPrevCost,  prevCost)` instead of the above!
        // I EVEN STUPIDLY TRIED `Math.Min(prevPrevCost, cost[^1] + prevCost)`
    }
}
