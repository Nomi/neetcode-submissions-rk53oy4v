// Notes from Last Actual Solution (from 2024) MIGHT be better...

// # Solution from 10-05(May)-2026:

public class Solution {

    public int NumDecodings(string s) {
        return 
            // NuAttempt1_Memoized_TopDown
            NuAttempt2_BottomUp_Optimized__AI_Generated
            (s);
    }

    public int NuAttempt2_BottomUp_Optimized__AI_Generated(string str) {
        // TRACE for s = "10"
        if (string.IsNullOrWhiteSpace(str) || str[0] == '0') 
            return 0;
        var s = str.AsSpan();


        // Start of trace: s[0] is '1', so we continue...
        
        // ways to decode s[-1] == "" (empty string) == 1
        int twoStepsBack = 1; 
        
        // ways to decode s[0] == 1
        int oneStepBack = 1; 

        for (int i = 1; i < s.Length; i++) {
            // i = 1, s[i] = s[1] = '0'
            int curWays = 0;

            // 1. Single Digit Check: Is '0' valid?
            // s[i] != '0' is FALSE. 
            // Logic: We CANNOT treat '0' as its own letter.
            // Result: current remains 0.
            if (s[i] != '0') {
                curWays += oneStepBack;
            }

            // 2. Two Digit Check: Is "10" valid?
            // twoDigit = (1 * 10) + 0 = 10.
            // 10 is >= 10 and <= 26. This is TRUE.
            // Logic: We pair the '1' and '0' together.
            // current = 0 + twoStepsBack (1) = 1.
            int twoDigit = int.Parse(s[(i-1)..(i+1)]); //END EXCLUSIVE so we get 2 digits starting from (i-1) and second from (i-2)!
            if (twoDigit >= 10 && twoDigit <= 26) {
                curWays += twoStepsBack;
            }

            // current is 1, so we don't exit.
            if (curWays == 0) return 0;

            // Shift the window:
            // twoStepsBack becomes 1 (previous oneStepBack)
            // oneStepBack becomes 1 (the current result)
            twoStepsBack = oneStepBack;
            oneStepBack = curWays;
        }

        // Loop ends. We return oneStepBack, which is 1.
        // Correct: "10" can only be decoded as 'J'.
        return oneStepBack;
    }

    // public int NuAttempt2_BottomUp_Optimized(string s) {
    //     if(s.Length < 2)
    //         return 1;

    //     int ways1StepBack = 1;
    //     int ways2StepBack = 1; 
        
    //     int i = s[1] != '0' ? 1 : 2;
        
    //     for(; i < s.Length; i++) { //WAIT I STARTED FROM 0 BUT BOTTOM UP IS USUALLY OPPOSITE OF MEMOIZED ???
    //         if(s[i] == '0') continue;

    //         var curChar = s[i];
    //         var curWays = 0;

    //         if(s[i]) { //Cur is a valid non-zero number with no leading 0s
    //             curWays += ways1StepBack;
    //         }            
    //         if(i > 1 && int.Parse(s[(i-1)..(i+1)]) >= 10 && int.Parse(s[(i-1)..(i+1)]) <= 26) {
    //             curWays += ways2StepBack;
    //         }
    //     }

    //     return WaysToDecode(s.AsSpan(), new());
    // }

    public int NuAttempt1_Memoized_TopDown(string s) {
        return WaysToDecode_TD_Mem(s.AsSpan(), new());
    }

    

    int Ch2Int(char s) => s - '0'; //char to int

    int WaysToDecode_TD_Mem(ReadOnlySpan<char> s, Dictionary<int, int> waysCache) { //NuAttempt1
        if(s.Length == 0)
            return 1; //*IMPORTANT* We found 1 valid way of grouping!!! (I ALMSOT DID RETURN 0 EARLIER!)
        
        if(s[0] == '0') //leading 0s 
            return 0;

        if(waysCache.ContainsKey(s.Length))
            return waysCache[s.Length];

        //Just current:
        var curAlone = WaysToDecode_TD_Mem(s[1..], waysCache);
        
        //Current + next:
        var curWithNext = 0;
        if(s.Length > 1 && int.Parse(s[..2]) <= 26) {
            curWithNext = WaysToDecode_TD_Mem(s[2..], waysCache);
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
