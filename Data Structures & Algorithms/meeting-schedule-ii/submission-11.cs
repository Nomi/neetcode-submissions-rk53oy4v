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
        return 
                Attempt1_SweepLine_EventsVersion
                // Attempt2_SweepLine_OrderedMapVersion_MicroOptimizedForSpace // This implementation is specific for [start, end) {start inclusive, end exclusive}. For more details, read the comments inside the function!
            (intervals);
    }

    // Took me 10minutes 41seconds to write the code
    // TC = O(n + (2n * log(2n))) = O(n*log(n))
    // SC = O(2n) = O(n)
    public int Attempt1_SweepLine_EventsVersion(List<Interval> intervals) {
        //min number of rooms == maximum overlap

        List<(int ts, bool isEnd)> events = new();

        foreach(var interval in intervals){
            events.Add((interval.start, false));
            events.Add((interval.end, true));
        }
        
        // for sorting, false == 0, isEnd == 1; so false < true
        //OrderBy Ascending, and ENDS FIRST (isEnd=True first) (because intervals are of form [a,b) as per the requirement `(0,8),(8,10) is NOT considered a conflict at 8.`)
        // events = events.OrderBy(e => e.ts).ThenByDescending(e => e.isEnd).ToList(); 
        events.Sort((a,b) => ((ts: a.ts, isStart: !a.isEnd).CompareTo((ts: b.ts, isStart: !b.isEnd))));

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

    // TC = O(n + (2n * log(2n))) = O(n*log(n))
    // SC = O(2n) = O(n)
    // Best case TC = O(n) and SC = O(1) if all events start and end on the same time
    public int Attempt2_SweepLine_OrderedMapVersion_MicroOptimizedForSpace(List<Interval> intervals) {
        //min number of rooms == maximum overlap

        // This implementation is specific for [start, end) {start inclusive, end exclusive}
        // For end inclusive or start exclusive, you'd probably need different startCount and endCount maps then do custom logic in the foreach loop.
        // Or using (int ts, bool isEnd) as a custom key and somehow writing a custom comparer for it, which I don't even remember how to. 

        SortedDictionary<int, int> timeToCountMap = new();

        foreach(var interval in intervals){
            timeToCountMap.TryAdd(interval.start, 0);
            timeToCountMap[interval.start]++;
            
            timeToCountMap.TryAdd(interval.end, 0);
            timeToCountMap[interval.end]--;
        }
        
        int maxOverlap = 0;
        int count = 0;

        foreach((int ts, int eventsSum) in timeToCountMap) {
            count += eventsSum;
            maxOverlap = Math.Max(maxOverlap, count);
        }

        return maxOverlap;
    }
}
