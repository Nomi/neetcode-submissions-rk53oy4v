// Solution from 2026
public class Solution {
    public List<List<int>> CombinationSum(int[] nums, int target) {
        List<List<int>> results = new();
        // Backtracking(nums, target, results, [], 0);
        return CombinationSum_Optimal_SearchSpacePruning_BT(nums, results, target);
        return results;
    }

    // From 13-03-2026
    public List<List<int>> CombinationSum_Optimal_SearchSpacePruning_BT(int[] nums, List<List<int>> results, int target)
    {
        // 13-06-2026 Update:  [IMP]
        // SC is usally just n*TC. (n is for actually putting the cur path into results). NOT IN THIS PROBLEM THOUGH !!!
        // This is because due to nature of backtracking (cutting of as soon as solution in current path becomes impossible),
        // the TC reflects the result length.

        //Asc. Sort: (SearchSpacePruning!)
        Array.Sort(nums); //Helps us with Search Space Pruning and end our search early! (and since 2^n >> nlog(n), time complexity is the same!)
        Backtracking_SearchSpacePruning_Optimal(nums, target, results, [], 0);
        return results;
    }
    public void Backtracking_SearchSpacePruning_Optimal(Span<int> nums, int target, List<List<int>> results, List<int> cur, int curSum)
    {
        if(curSum == target)
        {
            results.Add(new(cur));
            return;
        }
        if(nums.Length == 0)
            return;

        //Figure out which we pick at this step!
        for(int i=0; i<nums.Length; i++)
        {
            curSum += nums[i];
            if(curSum > target) 
                return; //any future elements (e.g. nums[i+1]) to add can only be bigger to nums[i], so no point considering henceforth!

            cur.Add(nums[i]);
            Backtracking_SearchSpacePruning_Optimal(nums[i..], target, results, cur, curSum); //forgot same number can be chosen again, so we do from i..
            cur.RemoveAt(cur.Count-1);
            curSum -= nums[i];
        }

        return;
    }

    //From 13-03(March)-2026 00:58 CET:
    public void Backtracking(Span<int> nums, int target, List<List<int>> results, List<int> cur, int curSum)
    {
        // How do I get TC and SC?
        // 13-06-2026 Update: 
        // SC is usally just n*TC. (n is for actually putting the cur path into results). NOT IN THIS PROBLEM THOUGH !!!
        // This is because due to nature of backtracking (cutting of as soon as solution in current path becomes impossible),
        // the TC reflects the result length.

        // *IMPORTANT* Just realized I did not even think of the Optimal Sorting optimization :'(!

        //Easy to tell it's backtracking since the constraints are so small!

        //Base cases
        if(curSum == target) //condition met, any further recursion would break constraints
        {
            results.Add(cur.ToList());
            return;
        }
        
        if(curSum > target || nums.Length == 0) //constraints broken, any further recursion is pointless given that numbers are positive! (2 <= nums[i] <= 30)
            return;

        // Main code:

        // Nevermind, this was dumb:
        // for(int i=0; i<nums.Length; i++)
        // {

        // }

        // UNDEPRECATED! (comment was invalid!)// Deprecated because if we skip of this number, we need to skip ALL of it!
        // That only applies if the array entries are duplicated! (since here, skipping 1 element means skipping all of them!)

        // Pick current:
        cur.Add(nums[0]);
        curSum += nums[0];
        Backtracking(nums, target, results, cur, curSum);

        // Do not pick current at all going forward:
        cur.RemoveAt(cur.Count-1);
        curSum -= nums[0];
        Backtracking(nums[1 ..], target, results, cur, curSum); 
        //*IMPORTANT* note that nums[1..] for count < 2 gives a 0 lengthed array!
    }
}



// Last Actual Solution: (From November 2024 / 03-11(Nov.)-2024 03:12 CET)
// public class Solution {
//     public List<List<int>> CombinationSum(int[] nums, int target) 
//     {
//         //:::IMPORTANT NOTES:::
//         //READ THE COMMENTS!!!
//         //WATCH THE NEETCODE VIDEO (just skim through it)
//         //CHECK NEETCODEIO SOLNS (incl. the OPTIMAL verision!)

//         //Check the sortedOptimalBacktrack1 approach too (aside from the normal backtrack1) 
//         //backtrack1 is more important I guess? Just need to have an idea of how optimal backtrack works! (and remember it!)
        
//         // return backtrack1(nums, target);
//         return sortedOptimalBacktrack1(nums, target);
//     }


    
//     //Time complexity: O(2^ target) because the smallest possible number in nums is 1, so we can have at most target number of 1s as the height of our decision tree (depth of deepest branch) and for each branch there can only be 2 decisions USE or DISCARD, we get 2^target as MAX number of possible nodes in the decision tree..
//     //Space complexity: O((2^target+1)/2) // L = (N + 1)/2 where L is number of leaves for a tree with N nodes.
//     //CAN ALSO READ MORE ABOUT THIS IN NEETCODEIO SOLNS
//     public List<List<int>> backtrack1(int[] nums, int target)
//     {
//         List<List<int>> res = new();

//         backtrack1Helper(nums, idx: 0, target, sumThusFar: 0, subset: new(), res);
        
//         return res;
//     }
//     //Time complexity: O(2^ target) because the smallest possible number in nums is 1, so we can have at most target number of 1s as the height of our decision tree (depth of deepest branch) and for each branch there can only be 2 decisions USE or DISCARD, we get 2^target as MAX number of possible nodes in the decision tree..
//     //Space complexity: O((2^target+1)/2) // L = (N + 1)/2 where L is number of leaves for a tree with N nodes.
//     //CAN ALSO READ MORE ABOUT THIS IN NEETCODEIO SOLNS
//     public void backtrack1Helper(int[] nums, int idx, int target, int sumThusFar, List<int> subset, List<List<int>> res)
//     {
//         //WATCH NEETCODE VIDEO!!!

//         if(sumThusFar>target || idx==nums.Length) //Almost forgot about the idx>nums.Length condition
//             return;
//         if(target==sumThusFar)
//         {
//             res.Add(new(subset)); // I KEEP FORGETTING TO ADD A COPY, NOT THE REFERENCE!!! (using the copy constructor)
//             return;
//         }

//         //Case 1: We use current element
//         subset.Add(nums[idx]);
//         backtrack1Helper(nums, idx, target, sumThusFar+nums[idx], subset, res);

//         //Case 2: We never use current element again
//         subset.RemoveAt(subset.Count-1);
//         backtrack1Helper(nums, idx+1, target, sumThusFar, subset, res);

//         return;
//     }

//     //THE OPTIMAL part doesn't come from the time complexity or space complexity, but from SEARCH SPACE PRUNING!
//     //Time complexity: O(2^target) because the smallest possible number in nums is 1, so we can have at most target number of 1s as the height of our decision tree (depth of deepest branch) and for each branch there can only be 2 decisions USE or DISCARD, we get 2^target as MAX number of possible nodes in the decision tree..
//     //Space complexity: O(2^target) // L = (N + 1)/2 where L is number of leaves for a tree with N nodes.
//     public List<List<int>> sortedOptimalBacktrack1(int[] nums, int target)
//     {
//         Array.Sort(nums); //default sort by ascending I think
//         List<List<int>> res = new();

//         sortedOptimalBacktrack1Helper(nums, idx: 0, target, sumThusFar: 0, subset: new(), res);
        
//         return res;
//     }
//     public void sortedOptimalBacktrack1Helper(int[] nums, int idx, int target, int sumThusFar, List<int> subset, List<List<int>> res)
//     {
//         //SEARCH SPACE PRUNING!
//         //BASED ON: https://blog.seancoughlin.me/solving-leetcodes-combination-sum-problem-optimized-techniques-for-efficient-solutions#heading-optimizing-the-backtracking-solution 
//         //Archived URL: https://web.archive.org/web/*/https://blog.seancoughlin.me/solving-leetcodes-combination-sum-problem-optimized-techniques-for-efficient-solutions#heading-optimizing-the-backtracking-solution

//         if(target==sumThusFar)
//         {
//             res.Add(new(subset)); // I KEEP FORGETTING TO ADD A COPY, NOT THE REFERENCE!!! (using the copy constructor)
//             return;
//         }

//         for(int i=idx; i<nums.Length;i++)
//         {
//             if(sumThusFar+nums[i] > target)
//                 return; //No other branch under of this node from this point on will have the solution, so we can safely exclude/prune them from our search space!
//             //Case: We pick the number at i-th index but none before that:
//             subset.Add(nums[i]);
//             sortedOptimalBacktrack1Helper(nums, i, target, sumThusFar+nums[i], subset, res);
//             subset.RemoveAt(subset.Count-1);  //Removing from end of list is always complexity O(1)!!!
//         }
//         return;
//     }


// }
