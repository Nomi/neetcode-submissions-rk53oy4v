public class Solution {
    const int Start = 0, End = 1;
    public int EraseOverlapIntervals(int[][] intervals) {
        if(intervals.Length == 0)   return 0;
        var removals = 0;

        Array.Sort(intervals, (x,y) => x[0].CompareTo(y[0]));

        var prevEnd = intervals[0][End];
        for(int i = 1; i < intervals.Length; i++) {
            var cur = intervals[i];
            
            if(cur[Start] >= prevEnd) { // NO Overlap
                prevEnd = cur[End];
                continue; //no removal!
            }
            
            // Overlap:
            // We keep the interval that ends the earliest (and is the smallest): [obvious, because that's the most likely to cause another overlap]
            if(prevEnd > cur[End]){
                prevEnd = cur[End]; //remove the previous one //Else, remove the new one and keep the oldest, which can be done by just not changing prev!
            }
            
            removals++;
        } 

        return removals;
    }
}
