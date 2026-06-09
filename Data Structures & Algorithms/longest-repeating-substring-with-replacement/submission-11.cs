public class Solution {

    public int CharacterReplacement(string s, int k) { //Final TC: O(26*N), SC: O(26), which also happens to be the max number of unique characters possible EVER in given setup
        // READ THE COMMENTS IN `CharacterReplacement_Worse` method to know why this is so good! (and so much simpler and cleaner!)
        
        //## ** IMPORTANT **: 
        // - Basically, I need to be more fine with state being stale when it doesn't matter to us! 
        //     (e.g. not updating maximum here or skipping hashmap update in previous problem on leetcode)
        // - Had to ask help to get to this approach! Must study!
        // - Read comments from this solution!
        // - Uses Lazy Max
        
        int max = 0;
        int maxFreq = 0;
        int l=0, r=0;

        Span<int> freq = stackalloc int[26]; 
        while(r < s.Length) //NOTE: had to ask help to get to this approach! Must study!
        {
            // TRICKY LOGIC: We use a "Historical" maxFreq here (check inner while loop below!), not the current real maxFreq.
            // If the real maxFreq drops, this condition might return false early, leaving 
            // the window technically invalid (replacements needed > k).
            // WHY IT WORKS: We only care about beating the record 'max'.
            // An invalid window of the same size as our record doesn't hurt us; 
            // we just slide it until we find a higher frequency that allows it to grow.
            freq[s[r]-'A']++;
            if(freq[s[r]-'A'] > maxFreq) //There IS a new maxIdx in town! [And this is the only time we care (we don't care if we never decrement maxFreq to match state when l++ because any smaller than current maxFreq would just gie us smaller repeating sequences beause we'd need to use more replacement characters)]
                maxFreq = freq[s[r]-'A']; 

            while(r-l+1 - maxFreq > k) //while more replacments needed than allowed // windowLen - maxFreq == REPLACEMENTS (Obviously!) //So, we're saying while `Replacements > k`
            {
                freq[s[l]-'A']--;
                l++;

                // 09/06/26 Update: Neetcode explanation:
                // After we shrink from the left, maxf may be stale because we do not decrease it.
                // That can temporarily make the current window look valid even when its true current maximum frequency is smaller.
                // This is still correct because such a stale value never increases the answer beyond a window length that was already achievable when maxf was accurate.
                // Because the window can never grow larger than that size until a real character frequency breaks the old record and increments maxFreq.

                // Old Explanation: # WHY NO CHANGE TO `maxFreq` (The "Lazy Max" Logic):
                // 1. The Goal: We are only interested in finding the LONGEST valid window.
                // 2. The Math: The maximum valid length we can support is always (MaxFreq + k).
                // 3. The Drop: If shrinking the window causes the actual maxFreq to drop, 
                //    the "supportable length" also drops. 
                // 4. The Result: A window with a lower maxFreq can NEVER beat our current record.
                //    Therefore, we don't need to track the dip. We only care if maxFreq INCREASES,
                //    because that is the only way to get a longer result.
            }

            max = Math.Max(r-l+1, max); //I know we do this after shrinking in a loop, but before we extended r to include an extra duplicate, we have already calculated the max length of the last valid window here.
            r++;
        }

        return max; //solved in 50 minutes, mostly because of dumb mistakes on my part leading to errors on test cases.
    }

    public int CharacterReplacement_Worse(string s, int k) { //Final TC: O(26*N), SC: O(26), which also happens to be the max number of unique characters possible EVER in given setup


        // ## IMPORTANT NOTE: Nevermind, I had the dumb moment of not reading the actual question. This has happened a lot in my last study session, 
        // ## but till now I had avoided it in my current cycle of ADS studies. Damn. No excuse.
        // ## It was actually for FINDING consecutive repetition, not AVOIDING duplicates, 
        // ## which also makes my initial assumption moot and it all makes sense now.
        // ## Spent 20 minutes on this BTW.
        
        int max = 0;
        int maxFreqIdx = s[0]-'A'; //well this kinda half-complicatedly resolves itself
        int l=0, r=0;

        Span<int> freq = stackalloc int[26]; 
        while(r < s.Length) //a small part of the solution was kinda easy cuz I read a small trick to use in it by mistake!
        {
            //Process Expanded Window
            freq[s[r]-'A']++;
            if(freq[s[r]-'A'] > freq[maxFreqIdx]) //There IS a new maxIdx in town!
                maxFreqIdx = s[r]-'A'; // this was the bug XD `=freq[s[r]-'A']`! Forgot to change it when switching from plain max to idx!

            while(r-l+1 - freq[maxFreqIdx] > k) //while more replacments needed than allowed // windowLen - maxFreq == REPLACEMENTS (Obviously!) //So, we're saying while `Replacements > k`
            {
                freq[s[l]-'A']--;
                if(s[l]-'A' == maxFreqIdx) //NOTE: WAIT WHY WAS I DOING THIS DUMB THING:  if(freq[s[l]-'A'] == freq[maxFreqIdx]) WHEN I COULD JUST COMPARE INDICES! AMORTIZED O(N) BAYBYYY //This was also why initially avoided putting decrement above, solution was so simple!
                {   
                    //There MIGHT BE a new maxIdx in town! 
                    for(int i=0; i<freq.Length; i++)
                    {
                        if(freq[i] > freq[maxFreqIdx])
                            maxFreqIdx = i;
                    }

                    // **ULTRA IMPORTANT NOTE**: Yeah, this was just fundamentally wrong (had to ask AI to debug for 1 case specifically after 46 minutes (and 20 minutes before that spent on prior solution))
                    // I added hodgepodge of changes to fix this every break, but clearly bandaid solutions suck. As I had said in the comments too, just moving decrement above is better! 

                    // for(int i=0; i < freq.Length; i++) //O(26)
                    // {
                    //     if(freq[i] > freq[maxFreqIdx] - 1) //*IMPORTANT NOTE*! THIS WAS THE OTHER BUG! We decrement maxFreqIdx after this loop, so either move that up or subtract 1 here! if(freq[i] > freq[maxFreqIdx]) 
                    //     {
                    //         maxFreqIdx = i;
                    //         break; //*IMPORTANT NOTE*! NOT HAVING BREAK HERE THIS WAS THE SECOND AND LAST BUG I ALMOST INTRODUCED! Reasoning should be obvious. Better solution would just be moving decrement above!
                    //     }
                    // }
                }
                // freq[s[l]-'A']--; //comments from the above if apply!
                l++;
                // replacements = windowLen - maxFreq;
                
                //IMPORTANT NOTE! wait it might change max freq too! so maybe better to store an idx? 
                //(well I did change it everywhere from maxFreq to maxFreqIdx already)
            }

            max = Math.Max(r-l+1, max); //I know we do this after shrinking in a loop, but before we extended r to include an extra duplicate, we have already calculated the max length of the last valid window here.
            r++;
        }

        return max; //solved in 50 minutes, mostly because of dumb mistakes on my part leading to errors on test cases.
    }
}


//Last Actual Solution:
// public class Solution {
//     public int CharacterReplacement(string s, int k) {
//         //Used NeetCode's video 
//         //to speed my progress because I was stuck without 
//         //much of an idea!:
//         // return attempt1OverEngineered(s,k); 
        

//         //More realistic and the MAIN way of doing it for me? Maybe?
//         return attempt2Realistic(s,k);
//     }
//     // public void fillArr(T[] arr, T val)
//     // {
//     //     for(int i=0;i<arr.Count();i++)
//     //     {
//     //         arr[i] = val;
//     //     }
//     // }


//     //OverEngineered variant
//     //TC: O(N)
//     //SC: O(1)
//     //NOTE: This only works because not decrementing maxFreq
//     // does not affect the result because in order to have
//     // a better max result (window length), we need more
//     // elements of the same kind (=> bigger maxFreq) to
//     // ensure we have bigger window while having same/constant
//     // replacements. 
//     public int attempt1OverEngineered(string s, int k) //based on second part of neetcode's solution
//     {
//         if(s.Length<2)
//             return s.Length;
        
//         int[] charFreq = new int[26]; //initialized to 0s by default.
//         //Used NeetCode's video (shortform) to speed my progress because I was stuck without much of an idea!
        
//         int maxLen = 0;
//         int l=0;
//         int maxFreq = 0;
//         for(int r=0;r<s.Length;r++)
//         {
//             charFreq[s[r]-'A']++;
//             if(charFreq[s[r]-'A']>maxFreq)
//                 maxFreq = charFreq[s[r]-'A'];
            
//             int lettersToChange = (r-l+1) - maxFreq;
//             if(lettersToChange>k)
//             {
//                 charFreq[s[l]-'A']--;
//                 l++;
//             }
//             if((r-l+1)>maxLen)//because of needing to include right
//                 maxLen=(r-l+1);
//         }
//         return maxLen;
//         ////My previous attempt:
//         // while(r<s.Length)
//         // {
//         //     if(++charFreq[s[r]-'A']>maxFreq)
//         //         maxFreq = charFreq[s[r]-'A'];
//         //     //r-l == window length (sliding window)
//         //     //window length - maxFreq == number of characters to replace.
//         //     while(r<s.Length&&k>r-l-maxFreq)
//         //     {
//         //         r++;
//         //         if(++charFreq[s[r]-'A']>maxFreq)
//         //             maxFreq = charFreq[s[r]-'A'];
//         //     }
//         //     if(r-l>maxLen)
//         //         maxLen = r-l;
//         //     while()
//         //     {
//         //         //
//         //         if
//         //         l++;
//         //     }
//         // }
//     }

//     //Realistic algorithm
//     //TC: O(N)  //O(26*N) but asymptotically bounded by O(N) //i.e. there exists another line (cuz linear) that is higher than it for all input sizes.
//     //SC: O(1)
//     public int attempt2Realistic(string s, int k) 
//     {
//         var freq = new int[26]; //initialized to 0 by default C# behavior.
//         int maxLen=0;
//         int l = 0;
//         for(int r=0;r<s.Length;r++) //NOTE freq.Max() = O(26) cuz it always contains 26 elements.
//         {
//             freq[s[r]-'A']++;
//             //freq.Max() is equivalent to the following: (this is what I was trying before)
//             // int maxFreq = 0;
//             // for(int i=0;i<26;i++)
//             // {
//             //     if(maxFreq<freq[i])
//             //         maxFreq=freq[i];
//             // }
            
//             int windowLength = r-l+1; //+1 for including r itself.
//             int lettersToReplace = windowLength - freq.Max(); // == number of occurences of any character other than the one with max freq.
//             while((r-l+1-freq.Max())>k) //while because there may be max frequent element at front, which would make an if statement ineffective at decreasing number of replacements.
//             {
//                 freq[s[l]-'A']--;
//                 l++;
//                 windowLength--; //obviously the same as recalculating via r-l+1; cuz we just incremented l so it'd be r-(l+1)+1==r-l-1+1
//                 lettersToReplace = windowLength-freq.Max();
//             }
            
//             if(windowLength>maxLen)
//                 maxLen=windowLength;
//         }
//         return maxLen;
//     }
// }