// 2026.2: Soltuion (review):
public class Solution {
    // Time Taken: 10 minutes.
    
    // TC = O(N) 
    // [creation of set is O(N) and then the TWO nested forloops actually only visit any element twice in total at most due to set removal!]
    
    // SC = O(N)

    public int LongestConsecutive(int[] nums) { //BEST VERSION (better structurally and in simplicity):
        
        // Series Starter Pattern!
        HashSet<int> set = new(nums); 
        //if we wanted ALL sequences, we'd also store the count of each and use it. But here we just want longest.

        var maxLen = 0; //was int.MinValue before but that's wrong for empty arrays, it should be 0 and works for everything :)
        
        foreach(var num in nums) {
            if(set.Contains(num-1)) //already processed
                continue;

            var sequenceStart = num; //we can get rid of the whole first inner forloop used to find sequence start in the other solution!
            
            var curLen = 0;
            for(int i = sequenceStart; set.Contains(i); i++) {
                set.Remove(i);
                curLen++;
            }

            maxLen = Math.Max(maxLen, curLen);
        }

        return maxLen;
    }

    // TC = O(N) 
    // [creation of set is O(N) and then the TWO nested forloops actually only visit any element twice in total at most due to set removal!]
    
    // SC = O(N)

    public int LongestConsecutive_OLD(int[] nums) {
        
        // Series Starter Pattern!
        HashSet<int> set = new(nums); 
        //if we wanted ALL sequences, we'd also store the count of each and use it. But here we just want longest.

        var maxLen = 0; //was int.MinValue before but that's wrong for empty arrays, it should be 0 and works for everything :)
        
        foreach(var num in nums) {
            if(!set.Contains(num)) //already processed
                continue;
            var sequenceStart = num;
            for(sequenceStart = num; set.Contains(sequenceStart-1); sequenceStart--) {}
            var curLen = 0;
            for(int i = sequenceStart; set.Contains(i); i++) {
                set.Remove(i);
                curLen++;
            }

            
            // var curLen = num - sequenceStart + 1;

            maxLen = Math.Max(maxLen, curLen);
        }

        return maxLen;
    }
}

// 2026.1: Solution:

public class Solution__2026_1 {
// public class Solution {
    //**IMPORTANT** This uses the SERIES STARTER PATTERN !!! 
    //(ONLY LEARNED ABOUT IT AFTER SOLVING!)
    public int LongestConsecutive(int[] nums) //TC: O(N), SC = O(U) = O(N) where N is nums.Length and U is number of unique numbers in nums (and U <= N).
    {
        if(nums == null || nums.Length < 1)
        {
            return 0;
        }

        HashSet<int> numSet = new(nums); //TC: O(N), SC: O(U) = O(N) where N is nums.Length and U is number of unique numbers in nums (and U <= N).
        
        //can I join these loops? Update: From what I can see midway through solution, no.
        var maxLen = 1;
        foreach(var num in numSet) // **IMPORTANT NOTE:** SHOULD HAVE STARTED WITH ITERATING OVER SET INSTEAD OF NUMS! 
                                   //TC: O(U) even though there are 2 elements because the actual work is only being done for any element once.
        {
            // --*IMPORTANT NOTE:*-- Had to check Recommended time and space complexity, Hint 1, and Hint 2 to be able to solve it now. 
            // Hint 1 was kinda useless but hint 2 was the best (and recommended TC and SC was useful). I need to practice more, I guess :'(

            if(numSet.Contains(num-1))
            {
                continue;
            }

            int curLen = 1; //only length needed, so a hashset and this count, combined with our approach, is enough.
            for(int cur = num+1; numSet.Contains(cur); cur++)
            {
                curLen++;
            }
            if(maxLen < curLen)
                maxLen = curLen;
        }

        return maxLen;
    }
}


// Last Actual Solution:

// public class Solution {
//     public int LongestConsecutive(int[] nums) {
//         // return attempt1(nums); //READ COMMENTS FROM THIS ONE!
//         return attempt2(nums);
//     }

//     public int attempt1(int[] nums)
//     {
//         int maxLength = 0;
//         //TC - O(N):
//         HashSet<int> hs = new(nums);

//         //TC - O(N) [because there's N elements in HS and we only go over any sequence once by skipping any non-starting elements.]
//         foreach(var n in hs)
//         {
//             if(hs.Contains(n-1))
//                 continue; //Not the start of a series. This skipping is the reason the algorithm TC is O(N) and NOT O(N^2)
            
//             int length=1;
//             int next = n+1;
//             while(hs.Contains(next))
//             {
//                 next+=1;
//                 length++;
//             }

//             maxLength = length>maxLength ? length : maxLength;
//         }

//         return maxLength;
//     }


//     public int attempt2(int[] nums)
//     {
//         HashSet<int> hs = new(nums);
        
//         var maxLength = 0;
//         foreach(var num in hs)
//         {
//             if(hs.Contains(num-1)) //not the starting point of a sequence (an element exists before this).
//                 continue;
//             var length=1;
//             var nextNum = num+1;
//             while(hs.Contains(nextNum))
//             {
//                 length++;
//                 nextNum++;
//             }
//             if(length>maxLength)
//                 maxLength=length;
//         }
//         return maxLength;
//     }
// }

