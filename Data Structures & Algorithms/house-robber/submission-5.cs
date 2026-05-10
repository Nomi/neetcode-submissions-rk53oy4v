// # Solution from 09-05(May)-2026:
// TC = O(N)
// SC = O(1)

public class Solution {
    public int Rob(int[] nums) {
        // return Attemp1_BottomUpOptimized(nums);
        return MadeByAI_Attempt1_BottomUpOptimized__CLEANER_VERSION(nums);
    }

    public int Attemp1_BottomUpOptimized(Span<int> nums) {
        if(nums == null || nums.Length == 0)
            return 0;
        if(nums.Length == 1)
            return nums[0];

        var prevPrevMaxMoney = nums[0];
        var prevMaxMoney = Math.Max(nums[0], nums[1]); // ALMOST MESSED THIS UP BEFORE AI POINTED OUT TO ME!!!! (tbf inner loop stuff was also the same :( .)

        for(int i = 2; i < nums.Length; i++) {
            var maxMoneySoFar = Math.Max(prevPrevMaxMoney, prevMaxMoney); //maxMoneySoFar excludes current IDX
            prevMaxMoney = nums[i] + prevPrevMaxMoney;
            prevPrevMaxMoney = maxMoneySoFar; // This handles the choice of rob or not rob the previous house!
        }

        return Math.Max(prevPrevMaxMoney, prevMaxMoney);
    }

    public int MadeByAI_Attempt1_BottomUpOptimized__CLEANER_VERSION(Span<int> nums) {
        int robPrev2 = 0; // Max money if we stopped 2 houses ago
        int robPrev1 = 0; // Max money if we stopped 1 house ago

        foreach (int n in nums) {
            // Decision: Rob current house (+ robPrev2) OR skip current (keep robPrev1)
            int currentMax = Math.Max(n + robPrev2, robPrev1);
            
            // Slide the window forward
            robPrev2 = robPrev1;
            robPrev1 = currentMax;
        }

        return robPrev1;
    }
}



// # Last Actual Solution (from Early December 2024):

//(On 09-05-2026, I added a fix though, which is the setting of maxProfitAt_hPlus1 needed to have a Math.Max between it and the value after (since you can skip houses in between regardless of if you already skipped a prior house))

// public class Solution {
//     public int Rob(int[] nums) {
//         // return dfsWrapper1(nums);
//         return optimizedBottomUp(nums);
//     }

//     int dfsWrapper1(int[] nums)
//     {
//         var maxProfit = new int[nums.Length];
//         Array.Fill(maxProfit, -1);
//         return dfs(0, nums, maxProfit);
//     }

//     int dfs(int house, int[] nums, int[] maxProfit) // MEMOIZED TC: O(HOUSES) (each house computed only once) MEMOIZED SC: O(HOUSES) (because max stored for each house) //BRUTEFORCE (no memo) TC: O(2^HOUSES)
//     {
//         if(house >= nums.Length) //HAD > earlier
//             return 0;
        
//         if(maxProfit[house] == -1)
//         {
//             int dontRobCur = dfs(house+1, nums, maxProfit);

//             int robCur = nums[house] + dfs(house+2, nums, maxProfit);//cant rob next house
            
//             maxProfit[house] = robCur > dontRobCur ? robCur : dontRobCur;
//         }

//         return maxProfit[house];
//     }

//     int optimizedBottomUp(int[] nums) //TC: O(HOUSES), SC: O(1) 
//     {
//         int maxProfitAt_hPlus2 = nums[nums.Length-1];
//         int maxProfitAt_hPlus1 = Math.Max(nums[nums.Length-1], nums[nums.Length-2]);

//         for(int h=nums.Length-3; h>=0; h--)
//         {
//             int curMaxProfit = (int) Math.Max(nums[h]+maxProfitAt_hPlus2, maxProfitAt_hPlus1);
//             maxProfitAt_hPlus2 = maxProfitAt_hPlus1;
//             maxProfitAt_hPlus1 = curMaxProfit;
//         }

//         return (int) Math.Max(maxProfitAt_hPlus1, maxProfitAt_hPlus2); //h==-1 after last loop
//     }
// }
