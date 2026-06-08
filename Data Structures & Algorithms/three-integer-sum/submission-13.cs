public class Solution {
    public List<List<int>> ThreeSum(int[] nums) {
        //READ THIS: https://leetcode.com/problems/3sum/editorial/comments/1056876
        // return attemptNoSort1(nums);

        //Main Way to Solve:
        // return attemptTwoPtr1_NeetCodeSolnBasedAttempt(nums);
        return attemptTwoPtr2(nums);
    }

    //TC: O(n^2) // O(nlog(n)+n^2) but n^2 dominates nlog(n) asympotitcally
    //SC: O(1)
    public List<List<int>> attemptTwoPtr1_NeetCodeSolnBasedAttempt(int[] nums) 
    {
        List<List<int>> res = new();
        Array.Sort(nums);//TC: O(nlog(n))
        for(int i=0; i< nums.Count(); i++)//TC:(O(n^2))
        {
            if(nums[i]>0) // => nums[l] > 0 && nums[r] > 0
                break;
            if(i>0&&nums[i]==nums[i-1])
            {
                continue; 
                /// [IMPORANT] My explanation:
                //Avoids duplicates in a sorted set because 
                //there's never a common starting point/element
                // As long as we can discard all the non-distinct 
                // pairs of numbers that we add to the first element 
                //for the triplet to sum to 0.

                ////// [optional] More detailed explanation/continuation:
                //so when our result array contains these outputs sorted,
                //making starting numbers distinct (no duplicate first elem in the 3 sum elems),
                //ensures that there will never be another similar pair (even with different order)
                //because sorting cements the order.
                //Also, another interesting fact is that the two numbers that sum together will also never be together again due to the fact that their sum was only usable once, and it kind of becomes like those two sum up to one number (because we consider them together) and as such the only other option as an element would be the one we have already considered and have skipped since.
            }
            int target = -nums[i];
            int l=i+1, r=nums.Count()-1;
            while(l<r)
            {
                int cur2Sum = nums[l]+nums[r];
                if(cur2Sum>target)
                    r--;
                else if(cur2Sum<target)
                    l++;
                else //==
                {
                    res.Add(new(){nums[i],nums[l],nums[r]});
                    r--;
                    while(r>l&&nums[r]==nums[r+1])
                        r--;
                    l++;
                    while(l<r&&nums[l]==nums[l-1])
                        l++;
                    continue;
                }
            }
        }
        return res;
    }

    //TC: O(n^2)
    //SC: O(n)
    //This is only better when you're not allowed to change the nums array
    //as then you'd have to store a copy of nums array as well, 
    //at which point it'd be better to just use this method where you keep a hashset to keep track of duplicates partially.
    public List<List<int>> attemptNoSort1(int[] nums) 
    {
        List<List<int>> res = new();
        HashSet<string> used = new(); //could've used tuple?
        for(int i=0;i<nums.Count();i++) //TC: O(n^2)
        {
            HashSet<int> hs = new();
            int subSumTarget = -nums[i]; //target for the other two numbers.
            for(int j=i+1;j<nums.Count();j++) //this is pretty much twosum 2
            {
                if(hs.Contains(subSumTarget-nums[j])) //the only times duplicate numbers matter is when they're both the solution, which this condition helps with to make sure that the hashset doesn't affect our ability to handle that case by having the check be here.
                {
                    List<int> curr = new(){nums[i],subSumTarget-nums[j],nums[j]};

                    //Could've been used tuples here (instead of str)!
                    string str = string.Join(',',curr.OrderBy(x=>x).ToList()); //This sort is constant time because there's always 3 comparisons!
                    if(used.Contains(str))
                        continue;
                    used.Add(str);
                    res.Add(curr);
                }
                hs.Add(nums[j]);
            }
        }
        return res;
    }


    //***** IMPORTANT NOTE (from a later attempt): *****
    // We SORT AND START FROM `l = p + 1` BECAUSE for everything that has already been a pivot, 
    // we have already calculated every possible UNIQUE SET of indices involving that number. 
    // By only looking to the right of `p`, we:
    // 1. Guarantee that each triplet is in non-decreasing order of indices (p < l < r), 
    //    which naturally prevents duplicate triplets like [-1, 0, 1] and [0, -1, 1].
    // 2. Eliminate the need to check if `l == p` or `r == p`.
    // 3. Systematically shrink the search space, ensuring O(N^2) complexity without redundant checks.
    public List<List<int>> attemptTwoPtr2(int[] nums)
    {
        List<List<int>> res = new();
        //For avoiding duplicates, this sorting is better because of space complexity being O(1).
        Array.Sort(nums);
        for(int i=0; i<nums.Count();i++)
        {
            // IMPORTANT: Missed this earlier! Is a good optimization!
            //ADDED THIS A FEW MONTHS LATER ON MY REVIEW!!
            if(nums[i]>0) // => nums[l] > 0 && nums[r] > 0
                break;
            if(i>0&&nums[i]==nums[i-1])
                continue; //Avoiding duplicates (cuz array sorted) (and we process only first element of duplicates so l and r can have a chance to be one of those)
            int target = -nums[i];
            int l=i+1, r=nums.Count()-1;
            while(l<r)
            {
                var sum = nums[l]+nums[r];
                if(sum>target)
                    r--;
                else if(sum<target)
                    l++;
                else //==
                {
                    res.Add(new(){nums[i],nums[l],nums[r]});
                    l++;
                    r--;
                    while(l<r&&nums[r]==nums[r+1])
                        r--;
                    while(l<r&&nums[l]==nums[l-1])
                        l++;
                }
            }
        }
        return res;
    }
    
}




// // New solution: (DEPRECATED, just because. Any problems? )

// public class Solution {
//     public List<List<int>> ThreeSum(int[] nums) { //TC: O(N), SC= O(1) [because we ended up using result array] (Post Solution TC analysis)
//         // # Problem statement breakdown:
//         //
//         // nums -> int array, Assuming (not explicitly banned, in interview I would explain or ask): no sorting constraint, no pattern in the data, can have duplicates 
//         // and nums can be negative(!) {!!!didn't think of the question until I saw the example, damn!!!}
//         // ## Return:
//         //  List<List<int>> output 
//         //  WHERE {
//         //  output[idx] = [nums[i], nums[j], nums[k]] 
//         //  WHERE:
//         //  - nums[i] + nums[j] + nums[k] == 0
//         //  - i, j, k are all distinct (none of them equal the other)
//         // }
//         //
//         // ## Stated Constraints:
//         // - "The output should not contain any duplicate triplets." => What do we use here?? Actually, skipping over already encountered one makes sense? If not, hashmap/set?
//         //
//         // ## Constraints:
//         // - 3 <= nums.length <= 1000
//         // - -10^5 <= nums[i] <= 10^5
//         // 
//         // ## Example: 
//         // SKIPPING OVER because already spent 13 minutes writing this breakdown!
        
//         //# SOLUTION BEGINS:

//         List<List<int>> result  = new();
//         Array.Sort(nums); //inplace sorting! (if it was list input, the OrderBy function itself would take O(N) space complexity :O) //O(N^2) also dominates O(log(n)) in TC.
        
//         int p=0;
//         while(p<nums.Length) //p is the pivot
//         {
//             int l = p+1, r = nums.Length-1; 
//             //we start from `l=p+1` because for everything that has already been a pivot, we have calculated every possible UNIQUE SET of all indices
//             //
//             while(l<r) //p is our target for the 2 sum (sorted variant with 2ptrs)
//             {
//                 //NOTE: Apparently duplicate elimination while loops should be done at/towards the end of the loop instead of near start? 
//                 var curSum = nums[p]+nums[l]+nums[r];
//                 if(curSum > 0) 
//                 {
//                     r--;
//                 }
//                 else if (curSum < 0)
//                 {
//                     l++;
//                 }
//                 else //curSum == 0 (our actual target!)
//                 {
//                     result.Add([nums[p],nums[l],nums[r]]);
//                     //***** IMPORTANT NOTE: *****
//                     // We start from `l = p + 1` because for everything that has already been a pivot, 
//                     // we have already calculated every possible UNIQUE SET of indices involving that number. 
//                     // By only looking to the right of `p`, we:
//                     // 1. Guarantee that each triplet is in non-decreasing order of indices (p < l < r), 
//                     //    which naturally prevents duplicate triplets like [-1, 0, 1] and [0, -1, 1].
//                     // 2. Eliminate the need to check if `l == p` or `r == p`.
//                     // 3. Systematically shrink the search space, ensuring O(N^2) complexity without redundant checks.
//                     l++;
//                     r--;
//                     while(l<r && nums[l-1] == nums[l]) //If this is duplicate of previous number, move l to next unused unique number from left
//                         l++;
//                     while(r>l && nums[r] == nums[r+1]) //If this is duplicate of previous number, move r to next unused unique number from right
//                         r--;
//                 }
//             }
//             p++;
//             while(p<nums.Length && nums[p] == nums[p-1])
//                 p++;
//         }

//         return result; //empty for no match
//     }
// }
