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
    public int MinMeetingRooms(List<Interval> intervals) {
        //min number of rooms == maximum overlap

        List<(int ts, bool isEnd)> events = new();

        foreach(var interval in intervals){
            events.Add((interval.start, false));
            events.Add((interval.end, true));
        }
        
        // for sorting, false == 0, isEnd == 1
        events = events.OrderBy(e => e.ts).ThenByDescending(e => e.isEnd).ToList(); //OrderBy Ascending, and ENDS FIRST (because of our requirement `(0,8),(8,10) is NOT considered a conflict at 8.`)

        int maxOverlap = 0;
        int curCount = 0;
        foreach(var e in events){
            if(e.isEnd) {
                curCount --;
            }
            else {
                curCount++;
                if(maxOverlap < curCount) maxOverlap = curCount;
            }
        }

        return maxOverlap;
    }
}
