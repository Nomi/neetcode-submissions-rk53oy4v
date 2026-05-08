public class Solution {
    const int Start = 0, End = 1;

    public int[][] Merge(int[][] intervals) {
        if(intervals.Length == 0) return intervals;

        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));

        List<int[]> res = [intervals[0]];
        
        for(int i = 1; i < intervals.Length; i++) {
            // Since we sorted by start earlier,
            // cur STARTS after or at the same time as prev!
            var prev = res[^1];
            var cur = intervals[i];
            
            if(cur[Start] > prev[End]) { //absolutely no overlap!
                res.Add(cur);
            }
            else { //OVERLAP!
                prev[End] = Math.Max(prev[End], cur[End]); //Maybe if I sort by end too, I could directly assign without Math.Max? //Almost did a min here by mistake!
                // prev[Start] = Math.Min(prev[Start], cur[Start]); // Lol, it's already sorted, so I don't need to do this. 
            }
        }

        return res.ToArray();
    }
}
