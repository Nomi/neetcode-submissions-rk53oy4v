public class Solution {
    public int[] MaxSlidingWindow(int[] nums, int k) { 
        // return MaxSlidingWindow_CleanAndOptimized_2(nums, k);    
        return MaxSlidingWindow_CleanAndOptimized(nums, k);    
        // return MaxSlidingWindow_Optimal(nums, k); //separate loop structure of this is better! 
        // return MaxSlidingWindow_SubOptimal(nums, k);
        // return MaxSlidingWindow_LastActualSolution_CleanAndOptimizedButDuplicatesBased_GoodExplanationsTho(nums, k);
    }

    // public int[] MaxSlidingWindow_CleanAndOptimized_2(int[] nums, int k) {
        
    // }


    public int[] MaxSlidingWindow_CleanAndOptimized(int[] nums, int k) {
        if (nums == null || nums.Length == 0) return Array.Empty<int>();
        
        int n = nums.Length;
        int[] result = new int[n - k + 1];

        // We use a LinkedList as a Deque to store indices in a monotonically decreasing order of their values.
        var deque = new LinkedList<int>(); 

        for (int i = 0; i < n; i++) {
            // 1. Maintain Monotonicity: Remove indices of elements smaller than the current element
            // from the back, as they will never be the maximum for any future window.
            while (deque.Count > 0 && nums[deque.Last.Value] <= nums[i]) {
                deque.RemoveLast();
            }
            deque.AddLast(i);

            // 2. Remove Stale Indices: If the index at the front is outside the current window, discard it.
            if (deque.First.Value <= i - k) { //`if`, NOT `while` because it is impossible for more than one index to become stale at each +1 step of i.
                deque.RemoveFirst();
            }

            // 3. Record Result: Once the first window is complete (index >= k-1), 
            // the element at the front of the deque is the maximum for the current window.
            if (i >= k - 1) {
                result[i - k + 1] = nums[deque.First.Value];
            }
        }

        return result;
    }

    public int[] MaxSlidingWindow_Optimal(int[] nums, int k) { //TC = O(N), SC = O(N)
        //Used suboptimal solution (MaxSlidingWindow_SubOptimal()) as the base, so some notes of learning from there still apply. Optimizing it here tho!
        //Might be a good idea to refer to it!
        
        int[] maxes = new int[nums.Length-(k-1)]; //IN MY FIRST ATTEMPT (MaxSlidingWindow_SubOptimal()) I DID NOT FIGURE IT OUT UNTIL RUNNING!
        LinkedList<(int num, int idx)> deque = new(); //monotonic (decreasing) queue, we only need to keep elements in decreasing order. If we find new max? None of the prior values (at their current indices) can be maxes of the future windows!

        //S0: Initial state: nums = [1,3,2,4],  k=3, dq=[]. maxes=[0, 0]
        for(int i = 0; i<k; i++) //Already given 1 <= k <= nums.length, so we can do i<k in the first loop!
        {
            // # DEPRECATED:
            // deque.AddLast((nums[i], i));
            // //OHH A MAJOR BUG, I HAD THIS (the above line) AFTER THE WHILE AFTER COPYING EARLIER, 
            // //BUT THAT WOULD NOT WORK BECAUSE NOW IT'S NOT AUTO SORTING! WE NEED TO MAINTAIN MONTONICITY!
            // //OF COURSE, IT IS ALSO IMPORTANT TO NOTE THAT: 
            // // We also have to modify the if condition here to be `if(deque.Count > 1` instead of `>0`
            // while(deque.Count > 0 && deque.First().num <= nums[i]) //was if, but should've been while //had `deque.First().num < nums[i]` earlier instead of `<=`
            // {
            //     deque.RemoveFirst();
            // }
            //--------------

            // **IMPORTANT** NOTE: Deques need to be kept monotonic like this: (from tail. I was only checking when head wasn't larger earlier!)
            // I wasn't even keeping the whole queue monotnic XD, TBF I haven't touched anything monotonic for a while, 
            // and I am VERY TIRED after a long day of working and studying! I also shouldn't have tried to use the maxHeap based solution as base!
            // There's a lot of differences from astructure that keeps itself auto-sorted.

            while(deque.Count > 0 && deque.Last().num <= nums[i]) //forgot to change it to <= here somehow what (was `>` lol)??? I need to rest for today lmao
            {
                deque.RemoveLast();
            }
            deque.AddLast((nums[i], i));
        }
        maxes[0] = deque.First().num; //at 0 of monotonic (decreasing) queue, we keep the biggest!
        
        //S1: nums = [1,3,2,4],  k=3 | dq=[(3,1),(2,2)]. maxes=[3, 0]
        
        int r = k; //obviously, from above loop!
        while(r < nums.Length) //SC = O(k) = O(N), TC = O(N) [1,3,2,4], k=3: d32
        { 
            var nuWindowStart = r-k+1; //Be careful here!            
            // # DEPRECATED:
            // deque.AddLast((nums[r], r)); //idk why I was doing RemoveLast instead lol, I was even doing it correctly in the above loop XD I'm too tired
            // //OHH A MAJOR BUG, I HAD THIS (the above line) AFTER THE WHILE AFTER COPYING EARLIER, 
            // //BUT THAT WOULD NOT WORK BECAUSE NOW IT'S NOT AUTO SORTING! WE NEED TO MAINTAIN MONTONICITY!
            // //OF COURSE, IT IS ALSO IMPORTANT TO NOTE THAT: 
            // // We also have to modify the if condition here to be `if(deque.Count > 1` instead of `>0`

            //READ COMMENT RIGHT AFTER THIS S2 STUFF (TO ITS RIGHT) -> S2: nums = [1,3,2,4],  k=3 | r = 3, nuWindowStart= 3-3+1 = 1, dq=[(3,1),(2,2),(4,3)]. maxes=[3, 0] <- THIS IS WHERE THE OLD SOLUTION BROKE MONTONICITY, but as for reasons described above, I didin't even catch it here and had to straight up ask AI for a bigger hint!
            
            
            if(deque.First().idx<nuWindowStart)
                deque.RemoveFirst(); //Remove the index at the beginning! //need this after changes to the loop below //Also, deque means only 1 if condition needed, no loop!

            // **IMPORTANT** NOTE: Deques need to be kept monotonic like this: (from tail. I was only checking when head wasn't larger earlier!)
            // I wasn't even keeping the whole queue monotnic XD, TBF I haven't touched anything monotonic for a while, 
            // and I am VERY TIRED after a long day of working and studying! I also shouldn't have tried to use the maxHeap based solution as base!
            // There's a lot of differences from astructure that keeps itself auto-sorted.
            while(deque.Count > 0 && deque.Last().num <= nums[r]) //CAREFUL!! `< nums[r]` instead of `<=`
            {
                deque.RemoveLast(); //DEPRECATED: //idk why I was doing RemoveLast instead lol, I was even doing it correctly in the above loop XD I'm too tired
            }
            deque.AddLast((nums[r], r));
            //S3: nums = [1,3,2,4],  k=3 | r = 3, nuWindowStart= 1, dq=[(4,3)]. maxes=[3, 0], r-(k-1)= 1
            maxes[r-(k-1)] = deque.First().num; //This was wrong in my earlier attempt as part of the array length had to change index from r to appropriate. 
            //S4: nums = [1,3,2,4],  k=3 | r = 3, nuWindowStart= 3-3+1, dq=[(4,3)]. maxes=[3, 4]

            r++;
            //S5: nums = [1,3,2,4],  k=3 | r = 5, maxes=[3, 4]

        }
        //s6:  nums = [1,3,2,4],  k=3 | maxes = [3,4]
        return maxes; //refactor took 16 mins. (though I was going JUUST a bit leasurely and writing comments.) //well there were some stupid bugs that took it to 25, but that was literally just braindead from tiredness, as I did it correctly just a few lines earlier XD
        //Okay, it took me a total of an hour for forgetting how to maintain montonicity. Basically:
        // I wasn't even keeping the whole queue monotnic XD, TBF I haven't touched anything monotonic for a while, 
        // and I am VERY TIRED after a long day of working and studying! I also shouldn't have tried to use the maxHeap based solution as base!
        // There's a lot of differences from astructure that keeps itself auto-sorted.
        // Also, I didn't have much energy.
        // Excuses, excueses, excuses. Fair, but some more valid than others.
    }

    public int[] MaxSlidingWindow_SubOptimal(int[] nums, int k) { //TC = O(N*log2(k)) where k<=N, SC = SC = O(k)
      
        //BUG 2: I was making an array of size nums.Length!
        int[] maxes = new int[nums.Length-(k-1)]; //result array, so Auxiliary SC= O(1) [non-aux sc= O(N)] 
        PriorityQueue<(int num, int idx), int> maxHeap = new(); //to get min in a window;
        int r = 0;
        //Looked at the constraints after coming up with the above approach (in first 10 mins!)! 
        // What I found is:
        //Already given 1 <= k <= nums.length, so we can do r<k in the first loop!
        //Also, 1 <= nums.length <= 1000 implies we can go with a less-than-efficient solution

        //Initializing maxHeap including elements of our first window of size k => SC = O(k) <= O(N) and TC = O(k*log2(k))
        for(; r<k; r++)
        {
            maxHeap.Enqueue((nums[r], r), -nums[r]); //-nums[r] max it a maxHeap!
        }
        maxes[0] = maxHeap.Peek().num;

        while(r < nums.Length) // SC = O(N) [because we can end up keeping em all depending on input], 
                               // TC=O((N-k)*log2(2*k)) //at any time there can only (2*k)-1 elements in minHeap because at that point 
        { //BUG 1: had dumb `r<=nums.Length` error found on run!
            var nuWindowStart = r-k+1; //DEPRECATED BECAUSE we do need +1 (?) (confirmed by "running" on example): // we're r-k is actually the last index becasue r was incremented before, so no need of extra -(k-1) = -k + 1 {to account for k being 1-indexed/length}, and we can keep it r-k instead!!
            // Console.WriteLine($"{r}, {nuWindowStart}");
            while(maxHeap.Count > 0 && maxHeap.Peek().idx < nuWindowStart) 
            {
                maxHeap.Dequeue();
                //WAIT THIS IS THE SAME STALE STATE PATTERN COMMON IN OTHER SLIDING WINDOWS!! W I MIGHT BE A GENIUS FINALLY!
            }
            maxHeap.Enqueue((nums[r], r), -nums[r]);

            maxes[r-(k-1)] = maxHeap.Peek().num; //Part of "BUG 2:", had to change index from r to appropriate. 

            r++;
        }

        return maxes; //everything in 35 minutes cuz it was easy. (i was taking it leisurely a bit, tbf.) And I had no hints in the first 10 minutes. Should've just started doing stuff and i'd get it, maybe?
        //Well turns out there were minor bugs: "BUG 1:" and "BUG 2:" in code, which took it to 41 minutes.
        //Since the 41 min mark, it's been 46 min and a huge test case is failing (with k= 45). This is the only case failing though (19/20 pass), so that's a saving grace
        //Wait what, it suddenly passed without making any changes???? I even diff'ed the submission submitted directly after it to be sure, and only comments changed?
        //I also just noticed it had `No Output` on that run for my output! (there were 2 big arrays and I got lost and didn't see, because the text was so small!)
        //It also runs with the write line uncommented, so it wasnt that! Random!
    }

    public int[] MaxSlidingWindow_LastActualSolution_CleanAndOptimizedButDuplicatesBased_GoodExplanationsTho(int[] nums, int k) { 
        //TC = O(N), SC = O(N)

        //Number of windows is obviously = 1+nums.Count()-k
        int[] output = new int[nums.Count()-k+1];
        var q = new LinkedList<int>();
        int l=0, r=0;
        int windowIdx=0;
        while(r<nums.Length)
        {
            //We check and remove from last because
            //e.g. the first element(maximum) is equal to the new 
            //element, so to keep position correct, we remove 
            //every value we met after that 
            //      (all smaller to it, and if there's
            //      another duplicate, those will be 
            //      right at the beginning because it has to 
            //      be max number by definition of monotonically
            //      increasing queue.)
            // and then later add the next instance of that number
            // to the deque. If new number is greater, then we
            // can just empty the dequeue (simulated by Linkedlist);
            while(q.Count!=0&&q.Last.Value<nums[r]) //To keep it a monotonically decreasing queue, we remove all elements smaller than this.
                q.RemoveLast();
            //Add last because nums[r] can possibly be:
            // 1. equal to first/largest element, in which case order doesn't matter (we cleared out any smaller numbers before it already).
            // 2. was bigger than the earlier largest element, which was removed earlier. (queue is empty).
            // 3. smaller, in which case we need to add it at end to keep monotonically increasing nature.
            q.AddLast(nums[r]);
            if(r-l+1==k) //Window is complete
            {
                //Note that first element of q is the 
                //largest element in this window.

                output[windowIdx]=q.First.Value;
                if(nums[l]==q.First.Value) //if the first element of the window (i.e. at nums[l]) is the same as the number we just removed, it is guaranteed to be that element and not affect any further calculations because even if we encounter a duplicate after that, we have stored them directly after it.
                    q.RemoveFirst();
                l++;
                windowIdx++;
            }
            r++;
        }
        return output;
    }
}


// public class Solution {
//     public int[] MaxSlidingWindow(int[] nums, int k) {
//         //At any point when moving window to right,
//         //if we meet a new maximum, we can clearly disregard earlier maximums (any future windows will have at least that as the max)
//         //Oh, there's a pitfall, we have to disregard all elements smaller than the current element in our candidates list as well!
//         // So, this is a monotonic queue pattern!

//         //WRONG: int[] res =  new int[nums.Length-1]; // how many windows in nums? e.g. k=2, 1,2,3, -> windows = [1,2], [2,3] //if we also had 4: [3,4] => total res length = nums.Length-1 
//         //Actually, we need k windows, and at the start we cannot make k windows until we have the at least k elements,

//         //HMM it might be better to think of it as:
//         // Each index can be the end of a window nums.Length times!
//         // BUT NOT REALLY, WHY? BECAUSE: 
//         // The first `k-1` indices cannot be the ends of any window! so we get :

//         //EVEN BETTER to think of it as: the start of each window! (since start and end both encode each other by end being the k'th element from start (inclusive))
//         // The last `k-1` indices cannot be ends of any window! so we get:

//         int[] res = new int[nums.Length-k+1]; //almost put -1 there whew! 

//         LinkedList<int> candidates = new();

//         // // int start = 0;
//         // int cur = 0; //also ends up being the best way to put numbers into res!
//         // int end = k-1;
//         // while(end<nums.Length)
//         // {
//         //     while(candidates.Count>0 && candidates.Last.Value > nums[cur])
//         //     {
//         //         candidates.RemoveLast();
//         //     }
//         //     candidates.AddLast(cur);

//         //     res[cur] = 0;
//         // }


//         // int start = 0;
//         int l = 0; //also ends up being the best way to put numbers into res!
//         int r = k; //this is one index after the window ends
//         while(r<=nums.Length) //NEVERMIND I WAS CORRECT BEFORE, IT WORKS FOR THE CASE WHEN THE LAST WINDOW ENDS EXACTLY AT THE END // WRONG: Because r can be > nums.Length, so we'll just clamp it in our loop, the following is HALF WRONG: //<= ensures the last index is processed
//         {
//             // *IMPORTANT*:
//             //29 minutes AND 2 NeetCode runs later, I realized I should've included the index (CAUGHT IN SECOND NEETCODE RUN)
//             // WAIT I JUST REALIZED I NEED TO STORE THE IDX, WILL GET VALUE FROM NUMS
//             //Also, I realized I had also mistyped and adding index instead to candidates when I MEANT TO add values (which was wrong anyway), 
//             //though that ended up making more sense later.
//             // Just needed to make condition this: (candidates.Last.Value<(r-k)||nums[candidates.Last.Value] < nums[l])

//             var windowStart = l; //*IMPORTANT*: instead of doing <r-k and being unsure, this is better! And more readable!
//             // var resIdx = l;      dont even need this now that windowStart is here!      
//             while(l<r)
//             {
//                 while(candidates.Count>0 && (candidates.Last.Value<windowStart || nums[candidates.Last.Value] < nums[l])) //*IMPORTANT* had the mistake of > than instead :'( (CAUGHT IN FIRST NEETCODE RUN)
//                 {
//                     candidates.RemoveLast();
//                 }
//                 candidates.AddLast(l); //*IMPORTANT* I WAS EVEN STORING THE INDEX (I meant to store the value)
//                 l++;
//             }
//             res[windowStart] = nums[candidates.First.Value]; //Also, I'm stupid and was doing  res[l-(k-1)] =... even though I had resIdx

//             // l = r; //next window start index (first element in it)
//             //wait l, was already r from the loop!
//             // r += k; //index after next window ends
//             r++; //I WAS STUPID IT NEEDS TO BE `r+1` BECAUSE WE MOVE WINDOW BY 1!!! WTF WAS I THINKING!!!
//             l = windowStart+1;//ALSO STUPID BECAUSE I DIDN'T REALIZE L NEEDS TO BE RESET
//             // r = Math.Min(r, nums.Length); //was about to make it wrong :O
//         }

//         return res;
//     }
// }




//# AI Proposed Solution (best):
// public int[] MaxSlidingWindow(int[] nums, int k) {
//     int n = nums.Length;
//     int[] res = new int[n - k + 1];
//     LinkedList<int> deque = new(); // Stores INDICES

//     for (int i = 0; i < n; i++) {
//         // 1. TAIL: Remove indices that are out of bounds (older than i-k+1)
//         if (deque.Count > 0 && deque.First.Value < i - k + 1) {
//             deque.RemoveFirst();
//         }

//         // 2. BODY: Monotonic check (Remove smaller elements from back)
//         while (deque.Count > 0 && nums[deque.Last.Value] < nums[i]) {
//             deque.RemoveLast();
//         }
//         deque.AddLast(i);

//         // 3. HEAD: If window is big enough, record the max (front of deque)
//         if (i >= k - 1) {
//             res[i - k + 1] = nums[deque.First.Value];
//         }
//     }
//     return res;
// }
