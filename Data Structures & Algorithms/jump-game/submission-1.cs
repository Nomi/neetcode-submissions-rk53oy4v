public class Solution {
    public bool CanJump(int[] nums) {
        // This chooses the optimal path greedily:
        // - The "Movable Goalpost":
        //      By working backward, you only care if the current index can reach the nearest reachable point to the end.

        int goal = nums.Length - 1;

        for(int i = goal - 1; i >= 0; i--) { //since we start from back, we always check which is the furthest (from start) spot we can jump to from a leftward position
            var goalDist = goal - i; // Could do +1 but not needed for us as we care about relative distance comparisons!
            var curJumpDist = nums[i]; // Could do +1 but not needed for us as we care about relative distance comparisons!
            if(curJumpDist >= goalDist) {
                goal = i; // This is the closest index to our goal where we can reach the goal, so now we find how to get here most optimally.
            }
        }

        return goal == 0; //we can reach goal through a chain of jumps from end to here.
    }
}
