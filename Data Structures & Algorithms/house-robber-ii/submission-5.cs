// # Last Actual Solution (from 04-12(May)-2024): 

// public class Solution {
//     public int Rob(int[] nums) {
//         //MY FIRST IDEA: WE JUST ROB UNTIL WE GET TO THE END AND THEN FOR THE LAST HOUSE WE DECIDE WHETHER TO ROB IT OR NOT BASED ON IF THE FIRST HOUSE WAS ROBBED.
//         //WE ONLY NEED TO ENSURE ONE THING (im not fully sure, so watch neetcode or greghogg or striver video afterwards):
//         //  IF WE ROB FIRST HOUSE, DON'T ROB LAST HOUSE.
//         return dfsWrapper1(nums);

//         //Actually, Bottom-Up and Space Optimized Bottom-Up solutions were easier for me to concieve here because you could have separate arrays for each (robbing first house and not robbing).
//         //I checked the neetcodeio solutions and it's basically the same thing. Though, they did do them in a smarter way I guess. (creating a helper and calculate for each robbing first house and not robbing and then return the max among them) 
//         //As such, in interest of time, I'll just take another good look at them and move to the next problem!  [ my interview is in 2 days :'( ]

//         //DO MAKE SURE TO DO THEM WHEN DONE WITH THE INTERVIEW (and practice the memoized solution!)!!
//         //ALSO THIS IS CLEARLY A 0/1 KNAPSACK PROBLEM (bounded)
        
//         //BOTTOM UP WOULD BE JUST RUNNING THE NORMAL HOUSE ROBBER BUT 1 THAT INCLUDES START BUT EXCLUDES END AND ONE THAT EXCLUDES START BUT INCLUDES END (My own idea.)
//     }
//     int dfsWrapper1(int[] nums) // private int[][] memo;
//     {
//         var maxProfit = new int[nums.Length][]; //2 for bool;
//         for (int i = 0; i < nums.Length; i++) 
//         {
//             maxProfit[i] = new int[] {-1, -1};//neetcodeio doesn't support yet: [-1, -1];
//         }
//         return dfs(0, nums, maxProfit);
//     }
//     //Had to take a look at the neetcodeio soln to see exactly how to do this (after trying a little, but hadn't added the flag as second argument of memo (TBF, I was thinking whether I should are not))
//     int dfs(int house, int[] nums, int[][] maxProfit, int wasFirstHouseRobbed = 0) // MEMOIZED TC: O(HOUSES) (each house computed only once) MEMOIZED SC: O(HOUSES) (because max stored for each house) //BRUTEFORCE (no memo) TC: O(2^HOUSES)
//     {
//         if(house >= nums.Length || (house == nums.Length-1 && wasFirstHouseRobbed == 1))
//             return 0;
//         if(maxProfit[house][wasFirstHouseRobbed] == -1)
//         {
//             int dontRobCur = dfs(house+1, nums, maxProfit, wasFirstHouseRobbed);

//             int robCur = nums[house] + dfs(house+2, nums, maxProfit, (house == 0 || wasFirstHouseRobbed ==  1? 1 : 0));//can't rob next house
            
//             maxProfit[house][wasFirstHouseRobbed] = robCur > dontRobCur ? robCur : dontRobCur; //remember, before first house is robbed, it isn't robbed so flag should be 0/false then too.
//         }

//         return maxProfit[house][wasFirstHouseRobbed];
//     }

// }


// # Solution from 10-05(May)-2026:

public class Solution {
    // Just like last time (Last Actual Solution from 2024), 
    // I NEEDED A HINT on the fact that you JUST DO IT TWICE, SEPARATELY: 
    //  - once for WITH FIRST house but WITHOUT LAST house 
    //  - and once for WITHOUT FIRST house and WITH LAST house
    // AND I ALSO ENDED UP FORGETTING MY SOLUTION NOW REQUIRED ANOTHER HANDLING FOR 1 LENGTH INPUT!!!

    public int Rob(int[] nums) {
        if(nums.Length == 1) // IMPORTANT: I forgot about this until the single element test case failed!!!! 
            return nums[0];

        // Rob First house (without Last):
        int robFirstMax = My_HouseRobberI_Soln_ModifiedToSpanTho(nums[..^1]); //[0, ^1) (start inclusive, end exclusive)

        // Rob Last house (without First):
        int robLastMax = My_HouseRobberI_Soln_ModifiedToSpanTho(nums[1..]);

        return Math.Max(robFirstMax, robLastMax);
    }

    public int My_HouseRobberI_Soln_ModifiedToSpanTho(Span<int> nums) { //copy pasted from there with just parameter type changed to span
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
}
