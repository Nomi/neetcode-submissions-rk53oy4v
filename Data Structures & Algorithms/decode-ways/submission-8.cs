// # Solution from 10-05(May)-2026:

public class Solution {
    public int NumDecodings(string s) {
        return WaysToDecode(s.AsSpan(), new());
    }

    int Ch2Int(char s) => s - '0'; //char to int

    int WaysToDecode(ReadOnlySpan<char> s, Dictionary<int, int> waysCache) {
        if(s.Length == 0)
            return 1; //*IMPORTANT* We found 1 valid way of grouping!!! (I ALMSOT DID RETURN 0 EARLIER!)
        
        if(s[0] == '0') //leading 0s 
            return 0;

        if(waysCache.ContainsKey(s.Length))
            return waysCache[s.Length];

        //Just current:
        var curAlone = WaysToDecode(s[1..], waysCache);
        
        //Current + next:
        var curWithNext = 0;
        if(s.Length > 1 && int.Parse(s[..2]) <= 26) {
            curWithNext = WaysToDecode(s[2..], waysCache);
        }

        var res = curAlone + curWithNext;

        waysCache[s.Length] = res;

        return res;
    }
}




// Last Actual Solution(s)  [from 02-12(Dec)-2024]:

// public class Solution {
//     public int NumDecodings(string s) {
//         //PATTERN: DP ON STRING I GUESS?
//         return topDownDp1(s); //Kind of like 0/1 knapsack?
//         //Skipping bottom up for now (limited time till interview) but I will circle back to this 
//         //      (I also NEED to practice this since I took help from neetcodeio soln.)
//         //      Also, I will watch its video.
//         //      For now, I will just Star this but not mark it as complete.
//         //Though bottomUp soln wouldn't really be hard as long as we follow the same approach. (not the space optimized version, because of my headache I have no idea how to do it and even though I checked neetcodeio soln, I'm not really sure.)

//         //I THOUGHT I WAS SMART LMAO!!:::
//         // //DEPRECATED (used learnings form here to get next one)::: Notice that for "1012", we can check ways 2 maps, then ways (1 maps * ways 2(nextChar) maps [combinatrics]) + 12 mapps, then what the (curChar maps * nextMaps) + curCharMaps
//         // //To understand the following comment, we need to understand that: 
//         // //      For 1 digit, as long as it is not 0, can always map to something. [Assuming validity of input is guaranteed]
//         // //          If there's a 0, the previous one has to be a double digit! (upto 26) [so always include 0 as  part of last number] 
//         // //Notice that for "1012", we can check ways 2(i==3) maps, then ways at (i==2) == (2 * ways the rest of the substring maps [combinatrics, because first we can map all, then add this as prefix to next char(or substrings the next char is part of) in all or map it separately]), and so on.
//         // //Clearly we can see the repeated work cause at any point we only want to know number of ways the next substring can be decoded.

//         // //Took me 16 minutes to come up with (and write) the above comments  and think how to solve. Though now I can do it bottmUp unlike the topDown I was planning.
        
//         // //OHH IS THIS A KNAPSACK PROBLEM!!????!???!
//         // //OHH A MINOR ADDITION TO WHAT I SAID EARLIER, WE CAN JUST IGNORE ALL 0s.
//         // return optimizeBottomUp(s);
//     }

//     int topDownDp1(string s) //TC: O(N) SC: O(N) //FOR DP, TC IS USUALLY == SC (for topdown/memo/cache) BECAUSE THE ACTUAL HEAVY LIFTING IS NEEDED ONLY TO PUT IN THEN VALUES IN THE MEMO!!!
//     {
//         Dictionary<int, int> waysToDecodeFrom = new(s.Length+1);
//         waysToDecodeFrom[s.Length] = 1; // only 1 way to decode nothing (into nothing).

//         return dfs1(s, 0, waysToDecodeFrom);
//     }

//     int dfs1(string s, int idx, Dictionary<int, int> waysToDecodeFrom)
//     {
//         // if(idx==s.Length) //Handled by a nifty trick with the memo that I got from neetcodeio soln
//         //     return 1; // only 1 way to decode nothing (into nothing).
//         if(waysToDecodeFrom.ContainsKey(idx)) //this has to be before next or it breaks!
//             return waysToDecodeFrom[idx];
//         if(s[idx] == '0')
//             return 0; //Invalid grouping, return 0 and stop considering this any further. 
        
//         int res = dfs1(s, idx+1, waysToDecodeFrom);
        
//         //had to check neetcodeio solution after this.
//         if(idx < s.Length-1)
//         {
//             int num = map(s[idx..(idx+2)]); //REMEMBER, the `to` parameter FOR RANGE IS EXCLUSVE!
//             if(num != -1)
//             {
//                 res += dfs1(s, idx+2, waysToDecodeFrom);
//             }
//         }

//         waysToDecodeFrom[idx] = res;

//         return res;
//     }

//     int map(string numStr)
//     {
//         int num = int.Parse(numStr);
//         if( num > 26 || num <= 0 )
//             return -1;
//         return num;
//     }

// //     int optimizeBottomUp(string s)
// //     {
// //         if(s.Length==0)
// //             return 1;
// //         int i=s.Length-1;
// //         int numEnd = s.Length-1;
// //         while(s[i]==0)
// //             i--; //Because we include 0! (Though this didn't need to be a loop since valid input will only be 2 digits.)
// //         i--;// We need to get to second last number!
// //         int waysAt_iPlus1 = 1; //The last number can only be decoded once. //WaysAtPreviousNonZeroDigit?
// //         int waysAt_iPlus2 = 1; //only 1 way to decode nothing
        
// //         for(;i>=0;i--, numEnd--)
// //         {
// //             while(s[i]==0)
// //                 i--; //Because we include 0! (Though this didn't need to be a loop since valid input will only be 2 digits.)
// //             if(i<0) //Can be leading 0!
// //                 break;
// //             int curWays = 2 * (waysAt_iPlus1; //EARLIER: IDK why I think I might need a -1 at the end!

// //             waysAt_iPlus2 = waysAt_iPlus1;
// //             waysAt_iPlus1 = curWays;
// //         }

// //         return waysAt_iPlus1; // i==-1 here.
// //     }

// //     // int dfs1(string s, (int l, int r), int[,] memo)
// }
