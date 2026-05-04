public class Solution {
    public int MaxSubArray(int[] nums) { //TC=O(N), SC=O(1)
        
        var maxSum = int.MinValue;
        
        var curSum = 0;
        for(int i = 0; i < nums.Length; i++) {
            curSum += nums[i];
            if(maxSum < curSum)
                maxSum = curSum;
            if(curSum < 0)
                curSum = 0;
        }

        return maxSum;
    }
}
