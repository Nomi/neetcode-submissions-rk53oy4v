public class Solution {
    public int LeastInterval(char[] tasks, int n) {
        // return attempt1(tasks, n);
        return NuAttempt1(tasks, n);
    }

    //# Solution from 10-03(March)-2026:
    // (check botched first attempt from today at the bottom of this file!)

    public int NuAttempt1(char[] tasks, int n) { 
    //TC: O(N), SC: O(N) 
    //*IMPORTANT* ALMOST FORGOT, SC is O(N) because the heap part can only have 26 tasks (chars)! 
    //So PQ/maxHeap operations become log(26)! (==constant!)
        if(n<=0) //13-03-2026 update
            return tasks.Length;
        Dictionary<char, int> taskFreq = new();
        foreach(var task in tasks)
        {
            taskFreq.TryAdd(task, 0);
            taskFreq[task]++;
        }

        PriorityQueue<char, int> tasksPq = new( //max heap!
            taskFreq.Select(kvp => (kvp.Key, -kvp.Value))
        );
        
        Queue<(char task, int lastRun)> cooldownQ = new();
    
        int cycle = 0;
        while(tasksPq.Count > 0 || cooldownQ.Count > 0) //I was being STUPID! DEPRECATED:// DON'T NEED BECAUSE IF tasksPq is over that's it!: || cooldownQ.Count > 0)
        {
            cycle++;
            
            // START Edit from a later day : (20 April 2026)
            // fix for the "busy wait" cycle simulation (we don't need to waste those cycles when we can just skip)
            //
            // If we have no tasks to run, but some are waiting on cooldown. //*IMPORTANT* Didn't realize it until I tried this problem in mock interview and the idea just popped up in my head.
            if (tasksPq.Count == 0 && cooldownQ.Count > 0) 
            {
                int nextAvailableTime = cooldownQ.Peek().lastRun + n + 1;
                
                // Only jump forward if the next available time is actually in the future
                if (nextAvailableTime > cycle) 
                {
                    cycle = nextAvailableTime;
                }
            }
            // END Edit from a later day : (20 April 2026)

            while(cooldownQ.Count > 0) //doesn't even need to be a while loop given our constraints (comment from a later day, 20 April 2026)
            {
                (char task, int lastRun) = cooldownQ.Peek();
                if(cycle - lastRun <= n) // == n => this is the n'th cycle, <n => this is before nth cycle, we need n+1th (n cycles in between!)
                    break;
                cooldownQ.Dequeue();
                tasksPq.Enqueue(task, -taskFreq[task]);
            }

            if(tasksPq.Count == 0) //can remove this due to above edit (comment from a later day, 20 April 2026)
                continue;

            var curTask = tasksPq.Dequeue();
            taskFreq[curTask]--;

            if(taskFreq[curTask] > 0)
            {
                cooldownQ.Enqueue((curTask, cycle));
            }
            else
            {
                taskFreq.Remove(curTask);
            }
        }

        return cycle; 
        //Finished in 21 minutes but needed help from AI to detect 
        //(the time is excluding the 25-ish minutes I spent on the botched solution before getting a hint about the cooldown queue from AI)
        // And I am very sleep deprived (slept an hour total today, and only 4-5 hours per day for the last 2 days)!
        // And exhuasted after a day of work! (though tbf I had forgotten the cooldown queue and 
        // didn't get it the first time (after 2024) I read the problem yesterday)
    }

    //# Solution from 25-10(October)-2024 00:45 CET!
    //[IMPORTANT NOTES] READ THE COMMENTS!!! (and maybe watch neetcodeio video? because I had to watch his video too{the intuition and solution explanation part, not the code})
    //time complexity: O(tasks.Length)
    
    //space complexity: O(tasks.Length)
    public int attempt1(char[] tasks, int n) 
    {
        // >>> Starting out with the most frequent ones first gives us more opportunity to reduce idle time
        //e.g. AB_AB_A (n=2, tasks=a,a,a,b,b) is better than BA_BA__A [where'_' represents idle time]
        //or for tasks=a,a,a,b,b,c,c, n=1: ABCABCA is better than CBCBA_A_A

        // >>> We fill the ones with more elements (that aren't finished waiting for cooldown) because that way we can fill more of the blanks. 
        // >>> We maintain a cooldown Queue that stores all the elements (and when we can execute them next) in order of execution, using this we can execute only the max ones that aren't finished coolingdown.
        // >>> We remove elements from PriorityQueue until they haven't finished coolingdown, and put them back when they have.
        // This is how we achieve only executing the ones with most element that don't need to cooldown yet.

        // >>> As such, at any point, we try to pick the remaining character with the most remaining letters at the time (that isn't in the cooldown queue).
        //e.g. for tasks=a,a,a,b,b,c,c, n=1: ABCABCA is better than ABABAC_C (but equally good/same as ABABCAC, but that doesn't matter because it still can't be smaller than picking most frequent first so we can fill more of the blanks)
        
        ////// SOLUTION (code) :::
        int cpuCycles = 0;
        int[] charCountMap = new int[26];
        PriorityQueue<int, int> maxHeap = new(26);
        Queue<(int count, int nextAvailableTime)> q = new(26);

        //Count tasks count and populate inital maxheap:
        for(int i=0; i<tasks.Length; i++)
        {
            charCountMap[tasks[i] - 'A']++;
        }

        for(int i=0; i<charCountMap.Length;i++)
        {
            if(charCountMap[i]>0)
            {
                maxHeap.Enqueue(charCountMap[i],-charCountMap[i]);
            }
        }

        //Simulate cycles:
        while(maxHeap.Count>0 || q.Count>0)
        {            
            //Any tasks finsihed cooldown?
            if(q.Count>0 && q.Peek().nextAvailableTime <= cpuCycles) //we don't need to make this into a while loop because only 1 task runs at a time, so only 1 task can finish at a time (cuz n is fixed)
            {
                int temp = q.Dequeue().count;
                maxHeap.Enqueue(temp, -temp);
            }
            
            //Are there any tasks that aren't in cooldown?
            if(maxHeap.Count > 0)
            {
                int curCnt = maxHeap.Dequeue(); //Gives the max count becuz it's a MaxHeap //Note: Since we only have 26 possible characters, the time complexity of minheap part is O(log2(26)) which is <=> O(1) (asymptotically bound).
                if(--curCnt > 0)
                    q.Enqueue((curCnt,(cpuCycles+1)+n));//(n+1) because we want to exclude the current (cpuCycles+1)+n in this count.      //can avoid if we move the cpuCycle increment between this and the above condition.
            }

            //increment cycles count
            cpuCycles++;
        }

        return cpuCycles;
    }
}



// # Abandoned, Botched Solution from 10-03(March)-2026:
//
// Forgot the cooldown queue (I would pick nothing while waiting for max freq task to be over!) 
// Abandoned after 25 minutes!
// public int LeastInterval(char[] tasks, int n) {
//         //Get frequency of tasks
//         Dictionary<char, int> taskFreq = new();
//         foreach(var task in tasks)
//         {
//             taskFreq.TryAdd(task, 0);
//             taskFreq[task]++;
//         }

//         //
//         PriorityQueue<(char task, int freq), int> maxHeap = new(taskFreq.Select(kvp => ((kvp.Key, kvp.Value), -kvp.Value))); //had to check the dictionary select syntax
//         Queue<(char task, int freq)> 
//         // 
//         Dictionary<char, int> lastRuns = new();
//         int cycle = 0;
//         while(maxHeap.Count > 0)
//         {
//             cycle++;
            
//             var lastRun = int.MaxValue; //was setting this to min value, only noticed at the end!
//             lastRuns.TryGetValue(maxHeap.Peek().task, out lastRun);
            
//             if((lastRun - cycle) >= n)
//             {
//                 var cur = maxHeap.Dequeue();
//                 lastRuns[cur.task] = cycle;
//                 cur.freq--;
//                 maxHeap.Enqueue(cur, -cur.freq);
//             }
//         }

//         return cycle; //had a lot of syntax errors that I fixed with help of NC compiler
//     }