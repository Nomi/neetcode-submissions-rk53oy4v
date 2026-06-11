/* * GOOGLE FOLLOW-UP (OR VARIANT): Find K-th Smallest (or PERCENTILE) IN 2 SORTED STREAMS!
 * 0. Rename lHalfLen to leftLen = k
 * 1. Set 'totalLeftCount' to 'k' instead of '(n + m + 1) / 2'.
 * 2. Run the same partition logic on the smaller array.
 * 3. The k-th element is simply Math.Max(sLeft, bLeft).
 * * This generalized version is O(log(min(n, m))) and works even 
 * if k is very small or very large relative to the array sizes.
 */

// ### 🏛️ GOOGLE MASKS: THE "MEDIAN" REPHRASINGS

// #### MASK 1: THE METRIC AGGREGATOR (Sharded Statistics)
// > "We have two different databases storing latency metrics for the same service, 
// > both sorted by time. We want to find the 95th percentile (or median) of latency 
// > across both databases. How do you find it without downloading both datasets 
// > to a single machine?"

// * **The Translation:** "95th percentile" is just a `K-th` element problem where 
//     `K = 0.95 * (N + M)`.
// * **The "L5" Signal:** Explain that Binary Search minimizes data transfer (network calls) 
//     by only requesting specific indices (`L / R`) rather than the whole dataset.

// ---

// #### MASK 2: THE VERSION CONTROL (Branch History)
// > "You have two branches of a configuration file history. Each branch has 
// > a sorted list of timestamps when changes were made. Find the 'middle' 
// > change that occurred across the entire project history."

// * **The Translation:** This is the Median problem.
// * **The Test:** Can you handle `long`/timestamp values as the comparison 
//     criteria instead of simple integers?

// ---

// #### MASK 3: THE GLOBAL LEADERBOARD (Cross-Server K-th)
// > "Two different game servers (East Coast and West Coast) keep a sorted 
// > list of player scores. Find the 100th best player in the world."

// * **The Translation:** This is the `K-th` smallest/largest element. 
// * **The Test:** Does your binary search depend on the total size of the 
//     arrays, or only on the `K` you're looking for? (Optimization: `O(log(min(N, M)))`).

// ---

// ### 🛠️ THE "K-TH SMALLEST ELEMENT" FOLLOW-UP BOILERPLATE

// /* * 1. Set 'totalLeftCount' to 'k' instead of '(n + m + 1) / 2'.
//  * 2. Run the same partition logic on the smaller array.
//  * 3. The k-th element is simply Max(sLeft, bLeft).
//  * This generalized version is O(log(min(n, m))) and works even 
//  * if k is very small or very large relative to the array sizes.
//  */


// 04-03(March)-2026:
public class Solution {
    public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
        // *IMPORTANT* Update: 10 mins into my trash solution,
        // I realized it might be better to NOT binary search over indices,
        // BUT RATHER over how many elements to pick from smaller!
        // This realization happened when I was thinking about how to include case of taking none from smaller,
        // this would also help with taking all from smaller (that's why rs = smaller.Length)
        // Also helps with the case smaller is empty! (not sure about both being empty though, probably never should happen!)
        // Also, side note, we're trying to binary search for end of first half!

        // We use the smaller array and do a binary search on it to test each `mid`
        // can be the mid (or if not, where the mid could be!)

        int[] small = nums1, big = nums2;
        if(big.Length < small.Length)
        {
            (small, big) = (big, small);
        }

        int totalLen = small.Length + big.Length; //according to constraints, should not overflow!
        int lHalfLen = totalLen/2; //floor
        // ^floor chosen because then:
        // ODD => Median will be on the left. (e.g. [1,3], [2] -> left half)
        // EVEN => 1 median part is at end of left side, and 1 median part is on the beginning of right side!
        // i.e. it will be right heavy for odd cases!


        int sMin = 0, sMax = small.Length; //inclusive bounds on number of elements contributed to left side by small!
        
        while(sMin <= sMax)
        {
            int sShare = sMin + (sMax - sMin)/2; //floor
            // Doesn't matter whether we ceil or floor, 
            // since we will try to converge on the correct element by the end anyway 
            // e.g. in [1,2] assume solution needs 2, even if we search with upto 1 first, it doesn't matter because we will search upto 2 next!

            int bShare = lHalfLen - sShare; //e.g. [1,3] [2,4,5], if we take 1 from smaller, we get: 2-1 = 1, which is what we expect [right heavy]

            int sLeft = sShare-1 >= 0 ? small[sShare-1] : int.MinValue; //minvalue just makes this value like it were on far left of combined array
            int sRight = sShare < small.Length ? small[sShare] : int.MaxValue; //maxvalue just makes this value like it were on far right of combined array        
            int bLeft = bShare-1 >= 0 ? big[bShare-1] : int.MinValue;
            int bRight = bShare < big.Length ? big[bShare] : int.MaxValue;

            //Note that small and big values are already sorted within themselves, so no need to check for that!
            if(sLeft > bRight)
            {
                //we need a smaller small for smaller sLeft
                //and/or bigger big for bigger bRight

                //HAD THIS FLIPPED ON MY FIRST SUBMISSION BUT I WAS ALSO EXHAUSTED MIDNIGHT SOO :) // sMin = sShare + 1; // sShare already checked!
                sMax = sShare - 1; //sShare already checked!
            }
            else if(bLeft > sRight)
            {
                //we need a bigger small for bigger sRight
                //and/or smaller big for smaller bRight
                
                //HAD THIS FLIPPED ON MY FIRST SUBMISSION BUT I WAS ALSO EXHAUSTED MIDNIGHT SOO (also at least my comments were correct!) :) //sMax = sShare - 1; // sShare already checked!
                sMin = sShare + 1; //sShare already checked!
            }
            else //sLeft<=bRight && bLeft <= sRight (every left element is equal to or smaller than every element on the right, meaning we found our median!)
            {
                if ((totalLen & 1) == 0)//(totalLen%2 == 0) //even
                {
                    return (Math.Max(sLeft, bLeft) + Math.Min(sRight, bRight))/(double)2;
                }
                else //odd
                {
                    return Math.Min(sRight, bRight);
                }
            }
        }
        
        throw new Exception("Unreachable!");
        //(or both arrays empty? Wait for that we would probably return (int.MaxVal+int.MinVal)/(double)2 == 0/2 == 0! So truly unreachable!
    }
}

// Solution from February 2026
// public class Solution {

//     public double FindMedianSortedArrays(int[] nums1, int[] nums2) { //Here, I know we return double but in interview, I should always think about division results
//         //renamed all ShareLen suffixes to just Share

//         var small = nums1.Length > nums2.Length ? nums2 : nums1;
//         var big = nums1.Length > nums2.Length ? nums1 : nums2;

//         int totalCount = small.Length + big.Length; //assuming no overflow!
//         int halfCount = (small.Length + big.Length)/2; //For reasons I thought of earlier, this goes horribly if you include +1;//no it was better to just include the odd median and even's second median on the left! ://actuall might be better not to (after coming back from the loop): + 1; //we do +1 so that we get odd's median and even's 2nd median //assuming no overflow!

//         // int l = 0, r = small.Length-1; //we we will binary search over the smaller array to reduce our complexity!
//         //above was incorrect because it would not execute loop for small.Length=0 ALSO NEED TO MAKE OTHER CHANGES FOR THIS NOW, INCL. CHANGING MID CALCULATION OR HOW WE TREAT IT!
//         int l = 0, r = small.Length; //we we will binary search over the smaller array to reduce our complexity!

//         //*IMPORTANT NOTE*: after fighting with this problem a lot, I realized the best way to do this is have mid and l and r represent number of elements to take rather than indices!
//         // Which is why r = small.Length is valid in that case!
//         // And why we can work with length<=0 and more!
//         // Renaming mid to smallShare now!
//         while(l<=r)
//         {
//             int smallShare = l + (r-l)/2;
            
//             // we take values at indices [l, mid] from smaller, so big's share length:
//             // int bigShare = half - (mid - l + 1);
//             // *IMPORTANT* OHH THIS WAS WRONG, DIDN'T KNOW IT WAS UNTIL AI MOCK INTERVIEWER POINTED IT OUT!
//             // int bigShare = halfCount - (mid + 1); //-1, not + 1; //since we're searching for the inflection point, mid itself is the INDEX of the inflection point
//             int bigShare = halfCount - smallShare;

//             // *important note* I only know this because of my earlier attempt (in a mock interview with Gemini AI) and the hints I got from that!
//             // int sL = smaller[mid];
//             // Also from the same hints, if smaller is of length 0, we might go out of bounds, so we need to use values that would 
//             // cause our later comparisons to be work even in that case, so maybe let's write those out first?
            
//             // Let: small = [s1, ..., sL, sR, ..., sm] where sL is at our half!
//             // and: big  = [b1, ..., bL, bR, ..., bn] where bL is what we want to insert
//             // so, the actual combined array would look like one of:
//             // 1. [...., sL, sR,....] 
//             // 2. [...., bL, bR,....]
//             // 3. [...., bL, sR,....]
//             // 4. [...., sL, bR,....]
//             // Because the left half can end with sL [when sL >= bL] or bL [when bL>(=)sL] 
//             // and right half can only start with bR [when bR <= sR] or sR [when sR<(=)bR]
            
//             // int sL = small.Length > 0 ? small[smallShare-1] : int.MinValue; //if we handle 0 length case by setting int MinValue, this ensures we never treat it as the actual median (since it would be treated as a ghost value on the far left side of the combined array) 
//             // int sR = small.Length > 0 && smallShare < small.Length ? small[smallShare] : int.MaxValue; //handles empty array or case of mid being the last smaller by similar logic as above by making it a random ghost value on the far right side of the combined array)
//             // *IMPORTANT*: after switching to smallShare, it is better to switch to smallShare for checks 
//             // Also, even if we didn't, earlierwe were not allowing mid<0 and checking for that in the above part, so our earlier checks didn't work when our share from small was supposed to be 0 (it did work the other way around though!) 
//             int sL = smallShare > 0 ? small[smallShare-1] : int.MinValue; //if we handle 0 length case by setting int MinValue, this ensures we never treat it as the actual median (since it would be treated as a ghost value on the far left side of the combined array) 
//             // int sR = smallShare > 0 && smallShare < small.Length ? small[smallShare] : int.MaxValue; //handles empty array or case of mid being the last smaller by similar logic as above by making it a random ghost value on the far right side of the combined array)
//             //*Important*: for sL we use smallShare > 0 to perform 2 checks at a time : not empty AND there will be something from small on left!
//             //BUT, for sR we do NOT need to check if there will be something from it will be on the left! And for the > 0 check? That's incorporated in smallShare<small.Length!
//             int sR = smallShare < small.Length ? small[smallShare] : int.MaxValue; //handles empty array AND case of mid being the last smaller by similar logic as above by making it a random ghost value on the far right side of the combined array)
//             //also, remember we were guaranteed both would not be empty in the clarification questions with Gemini AI.
//             int bL = bigShare > 0 ? big[bigShare-1] : int.MinValue; //handles case of none being picked from big similarly to above
//             // int bR = bigShare > 0 && bigShare < big.Length ? big[bigShare]: int.MaxValue; //handles case of none being picked from big
//             //Same note form above (for sR) applies!
//             int bR = big.Length > 0 && bigShare < big.Length ? big[bigShare]: int.MaxValue; //handles case of none being picked from big
            
//             if(sL > bR) //s and b are internally sorted so we only need to cross compare!
//             {
//                 //our sL is too big and/or bR is too small!
//                 r = smallShare-1; //look for smaller sR and/or bigger bL.
//             }
//             else if(bL > sR)
//             {
//                 //our sR is too small, and/or bL is too big!
//                 l = smallShare+1; //look for bigger sR and/or smaller bL
//             }
//             else //this is the case where we actually do end up if the partitions are valid! Now we just calculate the median and return!
//             {
//                 //note that math.min returns decimal by default so no need to cast
//                 double median = Math.Min(sR, bR);
//                 if(totalCount%2 == 0)
//                 {
//                     median += Math.Max(sL, bL); //had this typo: sR);
//                     median /= 2.0; //WAS MISSING THIS EARLIER!
//                 }
//                 return median;
//             }
//         }

//         throw new Exception("Unreachable");
//     }

//     // public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
//     //     var small = nums1.Length > nums2.Length ? nums2 : nums1;
//     //     var big = nums1.Length > nums2.Length ? nums1 : nums2;

//     //     if(small.Length == 0)
//     //     {
//     //         big.Length % 2 != 0 ?
//     //             return (double)big[] //e.g. [1,2,3,4,5] => (4-1)/2 +1 = 2
//     //             : return (big[(big.Length-1)/2]+big[(big.Length-1)/2 + 1])/(double)2;
//     //             //took me a bit to get to these formulas but we did it still
//     //     }

//     //     bool isOdd = ((nums1.Length + nums2.Length) % 2 == 0);
//     //     int OddMedianIdx = (small.Length - 1 + big.Length - 1)/2 + 1; //assuming no overflow
//     //     int Even1stIdx = (small.Length - 1 + big.Length - 1)/2; //assuming no overflow
//     //     int Even2ndIdx = OddMedianIdx;

//     //     int l = 0, r = small.Length;
//     //     while(l<=r)
//     //     {
//     //         int mid = l + (r-l)/2; //floor
            
//     //         int bigIdx = isOdd ?
//     //                             OddMedianIdx - mid
//     //                             : Even1stIdx - mid;
//     //         // if(bigIdx<0)
//     //         // if(mid==small.Length)
            
//     //         if(small[mid]>big[bigIdx+1]) //small[mid] is too big, we need to take a smaller mid and/or a bigger bigIdx
//     //         {
//     //             r = mid-1;
//     //         }
//     //         else if (small[mid+1]<big[bigIdx]) //big is too big, small is too small
//     //         {
//     //             l = mid + 1;
//     //         }
//     //         // else //wait, are we sure it's not strictly increasing? That would complicate this a lot 
//     //     }
//     // }
// }


// Last Actual Solution: (from 2024)
// public class Solution {
//     public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
//         //EXTREMELY IMPORANT: Read comments from attempt1,
//         //at least for the edgecases.
//         //Watching NC's video only comes AFTER that :3.
//         //Though the LC solutions might be better!
        
//         //My intuition (bruteforce):
//         //the bruteforce solution would be O(m+n) with two pointers, one each on nums1 and nums2 which we can use to traverse these as if they were a single sorted array (by checking which pointer is currently at a smaller value and the using that for that iteration)
//         //^^This bruteforce might be enough in an interview when followed up with just an explanation of how you would do it in O(log(m+n)) time instead.
//         //^^^ ESPECIALLY GIVEN THE EDGECASE CODE (and the code as a whole)
        
//         // IMPORTANT NOTES: (try reading the ones with ^ suffix above first!)
//         // Just skim through the NeetCode video for a great explanation for the solution!
//         //CHECK THESE FOR STUDYING:
//         // Editorial part we need: https://leetcode.com/problems/median-of-two-sorted-arrays/editorial/#approach-3-a-better-binary-search
//         // C# solution by someone: https://leetcode.com/problems/median-of-two-sorted-arrays/solutions/5783216/median-of-two-sorted-arrays-binary-search-approach-in-c-beginner-friendly/
//         return attempt1(nums1, nums2);

        
//     }

//     public double attempt1(int[] nums1, int[] nums2) 
//     {
//         var A = nums1;
//         var B = nums2;
        
//         if (B.Length < A.Length) {
//             int[] temp = A;
//             A = B;
//             B = temp;
//         }

//         int totalCount = A.Length + B.Length;
//         int sizeOfHalves = totalCount/2; //Integer division => decimal is truncated.
        
//         //Time complexity: O(log(min(n,m))) //min because we are running binary search on the smaller of the two.
//         //We can binary search through B as we can calculate the remaining elements needed for each side after picking both the left and right subarrays for B.
//         int l=0, r=A.Length-1;
//         while(true) //We can use While True because There's guaranteed to be a median so we can just return from there when we find it!
//         {

//             //** EXTREMELY IMPORTANT!  [!! EDGE CASES !!] **
//             //We want Floor for when the A (the smaller array) is 
//             //of length 1 or 0, because that makes mA = -1 when
//             //((l+r)/2 == -1/2) (which happens in those cases).
//             //NeetCode's YouTube/Python solution uses "//",
//             //which, ONLY for positive numbers, is equivalent to int division for postiive numbers
//             //For both negative and positive, it is actually equivalent
//             //to Math.Floor().
//             int mA = (int)Math.Floor((l+r)/2.0); //int division => truncate

//             //IMPORTANT: sizeOfHalves - (mA+1) 
//             //is the remaining length (of the left subarray
//             // of B), then we -1 to get it as the index.
//             int idxB = (sizeOfHalves - (mA+1))-1; 
            

//             //[VERY IMPORTANT]: HANDLING EDGE CASES
//             var Aleft = mA>=0 ? A[mA] : double.MinValue; //right most element of left window of A
//             var Aright = mA+1<A.Length ? A[mA+1] : double.MaxValue; //left most element of right window of A
//             var Bleft = idxB>=0 ? B[idxB] : double.MinValue; //right most element of left window of B
//             var Bright = idxB+1<B.Length ? B[idxB+1] : double.MaxValue; //left most element of right window of B

//             //Partition is correct:
//             if(Aleft<=Bright && Bleft <= Aright)
//             {
//                 if(totalCount%2!=0) //odd:  //the one on right will be the one actual median because neither window should contain it for odd length, and it is ensured here by calculating half in int/truncating division.
//                     return Math.Min(Aright, Bright); //There will never be a case where both will be MaxValue obviously.
//                 //even:
//                 return (Math.Max(Aleft,Bleft) + Math.Min(Aright, Bright))/2.0; //Gotta divide by 2.0 (instead of simply 2) to get decimal division
//             }
//             else if(Aleft>Bright) //the right most element of left window of A is smaller than the leftmost element of right window of B (keep in mind A and B are sorted)! //Aleft is too big, meaning we have too many elements from B on the left side <=> not enough elements in A on the left side!
//                 r = mA - 1;
//             else //Bleft > Aright <=> Aright < Bleft //Aleft is too small, meaning we have at least 1 element smaller than the the last element of B in our A right window! (keep in mind A and B are sorted)
//                 l = mA + 1;
//         }
//         return -1;
//     }

// }



// 04-03(March)-2026:
// My first approach was the best!
// Never trust AI again when it says that doing this with a left heavy half is better (or more readable to humans)!
// Left heavy solution, just as I anticipated back then, is a mess!
// public class TRASH_APPROACH_Solution {
//     public double FindMedianSortedArrays(int[] nums1, int[] nums2) {
//         // *IMPORTANT* Update: 10 mins in I realized it might be better to NOT binary search over indices,
//         // BUT RATHER over how many elements to pick from smaller!
//         // This realization happened when I was thinking about how to include case of taking none from smaller,
//         // this would also help with taking all from smaller (that's why rs = smaller.Length)
//         // Also helps with the case smaller is empty! (not sure about both being empty though!)
//         // Also, side note, we're trying to binary search for end of first half!


//         // We use the smaller array and do a binary search on it to test each `mid`
//         // can be the mid (or if not, where the mid could be!)

//         // const int lastMedianIdx = (nums1.Length + nums2.Length)/2; //[1,2] + [3] => firstMedianIdx = 3/2 = 1!
//         //                                                             //[1] + [2] => firstMedianIdx = 1

//         //Just remembered it's better to treat it as half!
//         int halfIdx = (nums1.Length + nums2.Length)/2; //overflow doesn't seem possible given the constraints! 
//         //[1,2] + [3] => half = 3/2 = 1,
//         //[1] + [2] => half = 1,
//         // => we have a left heavy half (will include the median for odd, and 2 medians for even)!! (since we're trying to binary search for end of first half!)
//         // Median(s) will be only on the left side!


//         var small = nums1.Length <= nums2.Length ? nums1 : nums2; //started with smaller and bigger as names but changed later!
//         var big = nums1.Length < nums2.Length ? nums2 : nums1;

//         // WRONG: int ls = 0, rs = small.Length - 1;

//         int smin = 0, smax = small.Length; //**IMPORTANT** update: Let's treat rs as `sMaxShare` and ls as `sMinShare`

//         while(smin <= smax) // == case handling?
//         {
//             int sShare = smin + (smax-smin)/2; //floor //overflow is not possible by my mental math!
//             // doesn't matter if we ceil or floor, 
//             // since we will try to converge on the correct element by the end anyway 
//             // e.g. in [1,2] we want 2, even if we  search 1 first, it doesn't matter because we will search 2 next!

//             int bShare = halfIdx - sShare + 1; //==(halfIdx - (sShare-1)); //given constraints, no overflow possible!

//             int sleftleft = sShare-2 > 0 && sShare-2 < small.Length ? small[sShare-2] : int.MinValue; //minvalue just makes this value be treated as if it were on far left of the final array!
//             int sleft = sShare-1 > 0 && sShare-1 < small.Length ? small[sShare-1] : int.MinValue; //deprecated: //maxvalue just makes this value like it were on far right!
//             int bleftleft = bShare-2 > 0 && bShare-2 < big.Length ? big[bShare-2] : int.MinValue;
//             int bleft = bShare-1 > 0 && bShare-1 < big.Length ? big[bShare-1] : int.MinValue;
//             int bright = bShare > 0 && bShare < big.Length ? big[bShare] : int.MaxValue;
//             int sright = sShare > 0 && sShare < small.Length ? small[sShare] : int.MaxValue;
//             if(sleft > bright) //should I assume no duplicates (even cross arrays) for now?
//             {
//                 //we need a smaller small and bigger big
//                 smax = sShare-1; //we already tested sShare itself!
//             }
//             else if(bleft > sright)
//             {
//                 //we need a bigger small and smaller big
//                 smin = sShare + 1;
//             }
//             else //we are at our correct median!
//             {
//                 int[] sorted = [sleftleft,bleftleft,bleft,sleft]; 
//                 Array.Sort(sorted);//=O(1) //ascending

//                 if((nums1.Length + nums2.Length)%2 != 0) //odd!
//                 {
//                     return sorted[sorted.Length-1]/(double)2;
//                 }
//                 else //even
//                 {
//                     return (sorted[sorted.Length-1] + sorted[sorted.Length-2])/(double)2;
//                 }
//             }
//         }

//         throw new Exception("Unreachable");
//     }
// }
