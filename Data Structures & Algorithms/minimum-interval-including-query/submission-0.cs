public class Solution {
    const int Start = 0, End = 1;
    
    public int[] MinInterval(int[][] intervals, int[] queries) {
        Dictionary<int, int> queryTimeToMinInterval = new();
        
        Array.Sort(intervals, (a,b) => a[Start].CompareTo(b[Start]));
        var sortedQueries = queries.OrderBy(x => x).ToArray();
        var minHeap = new PriorityQueue<(int size, int end), (int size, int end)>();

        int lastUnusedIntervalIdx = 0;
        foreach(var queryTime in sortedQueries) {
            if(queryTimeToMinInterval.ContainsKey(queryTime)) continue; //micro-optimization!
            
            // Dequeue irrelevant intervals
            while(minHeap.Count > 0 && minHeap.Peek().end < queryTime) // < because inclusive on right!
            {
                minHeap.Dequeue();
            }

            // Enqueue Relevant Intervals
            while(lastUnusedIntervalIdx < intervals.Length && intervals[lastUnusedIntervalIdx][Start] <= queryTime) { //<= because inclusive on left!
                int l = intervals[lastUnusedIntervalIdx][Start], r = intervals[lastUnusedIntervalIdx][End];

                // Ignore invalid intervals:
                // **NOT** OPTIONAL NOW SINCE I MOVED THE WHILE LOOP FOR DEQUEUEING THE THE WRONG INTERVALS ABOVE THIS LOOP SO WE MUST AVOID ADDING THEM HERE AT ALL COSTS!: // DEPRECATED: //Optional Optimization (not on NeetCode soln.):
                if(r < queryTime) // < because inclusive on right!
                {
                    lastUnusedIntervalIdx++; //*IMPORTANT* Almost forgot to do this until asked AI to check for syntax issues (it's more of a logic one tho)!
                    continue;
                }

                int length = r - l + 1;
                var entry = (length, r);

                minHeap.Enqueue(entry, entry); //Sorts first by LENGTH then TieBreaker is the ending (a later interval may be useful for a later query!)
                
                // Mark as used:
                lastUnusedIntervalIdx++; // Almost forgot this!
            }
            
            // Set results:
            queryTimeToMinInterval[queryTime] = minHeap.Count == 0 ? -1 : minHeap.Peek().size;
        }

        // Make output array:
        var res = new int[queries.Length];
        for(int i = 0; i < queries.Length; i++) {
            res[i] = queryTimeToMinInterval[queries[i]];
        }

        return res;
    }
}
