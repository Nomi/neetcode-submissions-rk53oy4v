public class Solution {

    public int Jump(int[] nums) {
        //Greedy: TC = O(N), SC = O(1)
        
        // Should watch the NC Video Soln. I guess (did this mostly while looking around resources to learn, so it's partially a copy but only because I was banging my head at the wall for too long :(.)
        return
            Greedy
            // Greedy_Alt
            (nums);
    }

    // TC = O(N), SC = O(1)
    private int Greedy(int[] nums) {
        int Destination = nums.Length - 1; 

        int jumps = 0, cur = 0, curFarthestJumpIdx = 0;

        while(curFarthestJumpIdx < Destination) {

            int nxtFarthestJumpIdx = 0;

            for(int nxt = cur; nxt <= curFarthestJumpIdx; nxt++) {
                nxtFarthestJumpIdx = Math.Max(nxtFarthestJumpIdx, nxt + nums[nxt]); //the farthest we can jump in a single jump from each index in our current jump window!
            }

            if(nxtFarthestJumpIdx <= cur) throw new Exception("Unreachable");

            cur = curFarthestJumpIdx + 1;
            curFarthestJumpIdx = nxtFarthestJumpIdx;
            jumps++;
        }

        return jumps; //WE DON'T NEED TO DO '+1' HERE BECAUSE WE DO AN EXTRA LOOP IN THE BEGINNING WHICH INCREMENTS JUMP 1 EXTRA TIME ANYWAY!
    }

    // TC = O(N), SC = O(1)
    private int Greedy_Alt(int[] nums) {
        int Destination = nums.Length - 1; 
        if(Destination == 0) //needed or the last return breaks for One length nums.
            return 0;

        int jumps = 0, cur = 0, curFarthestJumpIdx = nums[cur];

        while(curFarthestJumpIdx < Destination) {

            int nxtFarthestJumpIdx = 0;

            for(int nxt = cur; nxt <= curFarthestJumpIdx; nxt++) {
                nxtFarthestJumpIdx = Math.Max(nxtFarthestJumpIdx, nxt + nums[nxt]); //the farthest we can jump in a single jump from each index in our current jump window!
            }

            if(nxtFarthestJumpIdx <= curFarthestJumpIdx) throw new Exception("Unreachable");

            cur = curFarthestJumpIdx + 1;
            curFarthestJumpIdx = nxtFarthestJumpIdx;
            jumps++;
        }

        return jumps+1;
    }
}


// # Overcomplicated attempt from earlier:

// public class Solution {
//     // public const int NotFound = -1;
//     public int Jump(int[] nums) {
//         // Breakdown:
//         // nums[i] -> max length of jump from `i` index
//         // j -> any length of jump we can make from i, => j <= nums[i] 
//         // i -> src
//         // i+j -> LESS THAN nums[i] [obviously]


//         int l = 0, r = 0, jumps = 0, Destination = nums.Length - 1;

//         while(l < nums.Length) { //TC = O(2*N), NOT N^2 because we move through ??? NVM, might be wrong
//             // 1. Did we already get to the end?
//             if(l >= Destination) 
//                 return jumps;
 
//             // 2. What's the next index where we can jump to which allows us to jump farthest in next turn? [Greedy]
//             for(int dist = 1; dist <= nums[l]; dist++) {
//                 //Iterate through all possible jumps from l
//                 var distIdx = l + dist;
//                 if(distIdx >= Destination) {
//                     r = distIdx;
//                     break;
//                 }

//                 // var rNextMax = r + nums[r];
//                 var distIdxNextMax = distIdx + nums[distIdx];
//                 if(r == l || r + nums[r] < distIdxNextMax) { // WRONG: && if(nums[distIdx] != 0
//                     r = distIdx; //new index with farthest reach found
//                     if(r + nums[r] >= Destination) //We want EARLIEST jump to reach destination within the loop, and this is it!!!
//                         break;             //Could also achieve this by clamping distIdx + nums[distIdx] and r+nums[r] to max nums.Length-1! e.g. Math.Clamp(value, min, max)
//                 }
//             }
//             // if(r == l) //no next possible jump
//             //     return NotFound;
            
//             // 3. Make jump:
//             l = r;
//             jumps++;
//         }

//         throw new Exception("Unreachable");
//     }
// }
