// 2026.1: Solution (review):

public class Solution {
    public int[] TwoSum(int[] nums, int target) {
        Dictionary<int, int> firstIdx = new();

        for(int i=0; i<nums.Length; i++) {
            var curNum = nums[i];
            var complement = target-curNum;
            if(firstIdx.ContainsKey(complement))
                return [firstIdx[complement], i];
            firstIdx.TryAdd(curNum, i);
        }

        throw new Exception("Unreachable because of constraints");
    }
}


// 2026.1: Solution:

// public class Solution {
//     public int[] TwoSum(int[] nums, int target) {
//         Dictionary<int, int> numToIndex = new();
        
//         for(int i=0; i<nums.Length; i++)
//         {
//             int complement = target - nums[i]; //the required number to reach target sum
//             if(numToIndex.ContainsKey(complement))
//                 return [numToIndex[complement], i]; //correct order!
                
//             // numToIndex.Add(nums[i], i); //This didn't support duplicate values properly until before (only worked for cases like [5,5] with target 10 where encountering the duplicate caused solution to be found i.e. added to itself, if not, it would break because we try adding duplicate keys!)
            
//             numToIndex[nums[i]] = i; //This handles duplicate keys (numbers) by overwriting! BTW: C# can do this now! Yay!
//             //Also, note that since this is done afterm checking if its the solution, we know the overwritten duplicate number's index isn't needed 
//             //because either the number sums up with itself to solution ([5,5], target 10), which it doesn't if it gets to this line.
//             //So the only potential solution involving it would need only 1 of it or 0 (none) of it, and hence we don't need to keep both indices! 
//             //The check being before this ensures it!
            
//             //alternative: TryAdd or a if(ContainsKey){.Add}

//         }
//         return [-1, -1];
//     }
// }



// Last Actual Solution:
// public class Solution {
//     public int[] TwoSum(int[] nums, int target) {
//         Dictionary<int, int> numToIndex = new();
        
//         for(int i=0; i<nums.Count(); i++)
//         {
//             if(numToIndex.ContainsKey(target - nums[i]))
//                 return [numToIndex[target - nums[i]], i]; //correct order!
//             numToIndex.Add(nums[i], i);
//         }
//         return [-1, -1];
//     }
// }
