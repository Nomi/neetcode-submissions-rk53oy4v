public class Solution {
    public bool CanJump(int[] nums) {
        // *IMP* NOTE: THIS IS JUST THE MOST EFFICIENT [O(N)] WAY TO FIND IF IT IS POSSIBLE
        // IT IS ** NOT ** THE OPTIMAL NUMBER OF JUMPS! Why? Because:
        // - We start from the back and only choose the right most place we can jump to goal from
        // - There might be an index further to the left from which we can reach the goal bypassing the extra jump(s)
        //      e.g. the most extreme case would be that on index 0 you can make 10 length jump and our goal
        //           is directly reachable from there, but we still use another index between 0 and goal.
        // - If we wanted to get the least number of steps, we'd have to actually try to find the left-most spot we can jump to goal from!
        //      (check my Jump Game II solution!)

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
