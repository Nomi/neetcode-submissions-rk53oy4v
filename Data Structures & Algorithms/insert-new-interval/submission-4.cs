public class Solution {
    public int[][] Insert(int[][] intervals, int[] newInterval) {
        IInsertInterval solver = new
                                    Attempt1_OverEngineeredButOptimal
                                ();
        return solver.Insert(intervals, newInterval);
    }
}


public interface IInsertInterval {
    public int[][] Insert(int[][] intervals, int[] newInterval);
}

public class Attempt2_InterviewVersion : IInsertInterval {
    public int[][] Insert(int[][] intervals, int[] newInterval) {
        throw new NotImplementedException();
    }
}

public class Attempt1_OverEngineeredButOptimal : IInsertInterval {
    const int Start = 0;
    const int End = 1;

    public int[][] Insert(int[][] intervals, int[] newInterval) {
        if (intervals.Length == 0) return new int[][] { newInterval };

        // Find the start of the merge zone (first interval that MIGHT OVERLAP)
        int leftBound_Inclusive = FindLeftBound_Inclusive(intervals, newInterval[Start]);
        
        // Find the end of the merge zone (first interval completely AFTER with NO overlap)
        int rightBound_Exclusive = FindRightBound_Exclusive(intervals, newInterval[End]);

        List<int[]> res = new List<int[]>();

        // 1: Skip iteration, bulk add the left side
        for (int i = 0; i < leftBound_Inclusive; i++) {
            res.Add(intervals[i]);
        }

        // 2: Calculate the merged interval mathematically using the bounds
        int mergedStart = newInterval[Start];
        int mergedEnd = newInterval[End];
        
        if (leftBound_Inclusive < rightBound_Exclusive) { // If there is at least one interval to merge
            mergedStart = Math.Min(newInterval[Start], intervals[leftBound_Inclusive][Start]);
            mergedEnd = Math.Max(newInterval[End], intervals[rightBound_Exclusive - 1][End]); // rightBound_Exclusive - 1 is the last overlapping interval
        }
        res.Add([mergedStart, mergedEnd]);

        // 3: Skip iteration, bulk add the right side
        for (int i = rightBound_Exclusive; i < intervals.Length; i++) {
            res.Add(intervals[i]);
        }

        return res.ToArray();
    }

    // Finds the first interval where interval[End] >= targetStart
    int FindLeftBound_Inclusive(int[][] intervals, int targetStart) {
        int l = 0, r = intervals.Length - 1;
        int ans = intervals.Length; // Default to end if all intervals end before target
        
        while (l <= r) {
            int mid = l + (r - l) / 2;
            if (intervals[mid][End] >= targetStart) {
                ans = mid;     // This is a candidate
                r = mid - 1;   // But try to find an earlier one
            } else {
                l = mid + 1;
            }
        }
        return ans;
    }

    // Finds the first interval where interval[Start] > targetEnd
    int FindRightBound_Exclusive(int[][] intervals, int targetEnd) {
        int l = 0, r = intervals.Length - 1;
        int ans = intervals.Length; // Default to end if no intervals start after target
        
        while (l <= r) {
            int mid = l + (r - l) / 2;
            if (intervals[mid][Start] > targetEnd) {
                ans = mid;     // This is a candidate
                r = mid - 1;   // But try to find an earlier one
            } else {
                l = mid + 1;
            }
        }
        return ans;
    }

    // # Wasted 22 minutes here: // tried another attempt later for 14 mintutes lol but I was still unaware that I should be trying to find only the first after which and last intervals before which we absolutely must place this!!
    // public int[][] Insert(int[][] intervals, int[] newInterval) {
    //     int newIdx = FindInsertionIndex(intervals);

    //     List<int[]> newIntervals = new();
    //     var nStart = newInterval[Start];
    //     var nEnd = newInterval[End];
    //     for(int i = 0; i < intervals.Length; i++) {
    //         var cur = intervals[i];
    //         if((newIntervals.Count < 0 && nStart <= cur[Start])&& nEnd < cur[Start]) {

    //         } if(nStart > cur[End] || nEnd < cur[Start]) {
    //             newIntervals.Add()
    //         } else {

    //         }
    //     }
    // }

    // int FindInsertionIndex(int[][] intervals) { //took me like 18 minutes :(
    // //Deprecated: (int start, int end)
    //     if(intervals.Length == 0) return 0;
    //     int l = 0;
    //     int r = intervals.Length-1;
    //     int smallestWindowIdx = -1;
    //     while(l <= r) {
    //         int mid = l + (r-l)/2;
    //         if(intervals[mid][End] < intervals[l][Start]) {
    //             r = mid - 1;
    //         } else if(intervals[mid][Start] > intervals[r][End]){
    //             l = mid + 1;
    //         } else { // it is between start of l and end of r!
    //             smallestWindowIdx = mid;
    //             // Try to find a tighter/smaller insertion window:
    //             if(intervals[mid][Start] >= intervals[l][Start]) {
    //                 l = mid + 1; 
    //             } else if (intervals[mid][End] <= intervals[l][End])
    //                 r = mid - 1;
    //             } else { //we're not in a window where this would exist.
    //                 break;
    //             }
    //         }
    //     }
    //     return smallestWindowIdx;
    // }
}
