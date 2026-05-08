/**
 * Definition of Interval:
 * public class Interval {
 *     public int start, end;
 *     public Interval(int start, int end) {
 *         this.start = start;
 *         this.end = end;
 *     }
 * }
 */

public class Solution {
    public bool CanAttendMeetings(List<Interval> intervals) {
        if(intervals.Count == 0) return true;

        intervals.Sort((a,b) => a.end.CompareTo(b.end));

        var prevEnd = intervals[0].end;
        for(int i = 1; i < intervals.Count; i++) {
            var cur = intervals[i];
            // if(!(prevEnd <= cur.start)) //exclusive of end! //Same as below!
            if(prevEnd > cur.start) //exclusive of end!
                return false;
            prevEnd = cur.end;
        }

        return true;
    }
}
