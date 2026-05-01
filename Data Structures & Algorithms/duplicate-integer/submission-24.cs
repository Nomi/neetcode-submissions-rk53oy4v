public class Solution {
    public bool hasDuplicate(int[] nums) {
        var set = new HashSet<int>(nums.Count());
        foreach(var num in nums)
        {
            if(!set.Add(num))
                return true;
        }
        return false;
    }
}

// Last Actual Solution:
// public class Solution {
//     public bool hasDuplicate(int[] nums) {
//         HashSet<int> seen = new();
//         for(int i=0;i<nums.Count();i++)
//         {
//             if(!seen.Add(nums[i]))
//                 return true;
//         }
//         return false;
//     }
// }
 