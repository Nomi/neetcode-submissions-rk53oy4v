/* (*IMPORTANT* also read the notes in the Search function of the main Solution class!)
[THESE ARE FOR "SEARCH TARGET IN ROTATED SORTED", BUT SIMILAR WOULD APPLY HERE, JUST SMALLEST INSTEAD OF SPECIFIC/TARGET!]

## > Potential Google Rephrasings/Maskings : 

1. The "Shifted Log" (Data Forensics)
"We have a server that logs events in chronological order. However, due to a bug in our log rotation script, the most recent events were accidentally moved to the beginning of the file. You have a huge array of these log timestamps. Find the timestamp of a specific error event."
The Translation: "Chronological order" = Sorted. "Moved to the beginning" = Rotated. "Find specific event" = Search Target.

2. The "Circular Buffer" (Systems Design)
"Imagine a circular data buffer used for a streaming service. The data is sorted by ID, but the 'start' pointer is at an arbitrary position in the buffer. How would you find if a specific ID exists in the buffer in O(log N) time?"
The Translation: This is the most direct systems-level application of a rotated array. They want to see if you realize that a circular buffer is a rotated array.

3. The "Missing Key" (Physical Security)
"A janitor has a ring of keys sorted by size. He drops the ring, and it snaps back together, but the 'smallest' key is no longer at the top. If the janitor knows the sizes but doesn't want to look at every key, how can he find the key that fits a specific lock size?"
The Translation: A "ring" is just a visual metaphor for rotation.

## > The Google "Twist": What they add to the problem
Once you identify it's a Rotated Sorted Array, Google interviewers often throw in a "Part 2" to test your depth:

- "What if there are duplicates?" (This is the Search in Rotated Sorted Array II). 
  This kills the O(log N) guarantee because if nums[l] == nums[mid] == nums[r], 
  you can't tell which side is sorted. You have to increment l++, making it O(N) 
  in the worst case.

- "Can you find the minimum element first?" (This is the Find Minimum in Rotated Sorted Array). 
  This is where your "Two-Pass" logic shines.
*/


public class Solution
{
    public int Search(int[] nums, int target)
    {
        //## Solution_TwoSeparteSearches vs. Solution_SingleLoop:
        // Solution_TwoSeparteSearches uses same TC O(log(n)) operations and SC!
        // and at least I think it's more readable and harder to mess up, so why not go with this?
        //
        // Benefits of Solution_TwoSeparteSearches include:
        // - Easily readable and clear!
        // - Easier to spot mistakes and harder to mess up!
        // - Same TC O(log(n)) and SC O(1) [total and aux.] where n is nums.Length
        // - Separation of Concerns
        // - Unit testing!
        // - Can make a more general Binary Search helper class! (and now you get that functionality separate!)
        //
        // BUT, Google might want to ask the single merged while loop for search as a follow up, so maybe just look at it and understand, so you can answer?
        // Or, even better, if I remember at the start, just let them that option exists and why you won't take it!
        //
        // If they ask me to implement that version, we will just inline our binary search helper(with modifications to use same l and r and reuse our existin code as much as possible),
        // BUT to do that REMEMBER:
        // for the SingleLoop version, you need the case of mid == target!

        // return (new Solution_TwoSeparteSearches()).Search(nums, target);
        return (new Solution_SingleLoop()).Search(nums, target); //Best version (clean, at least)
        // return (new Solution_TwoLoops()).Search(nums, target); //still find this more readable!
    }
}

//Pseudocoding SingleLoop: 
//Base approach is same as TwoSeparateSearches, 
// first finding the valid section where target can be,
// then bisecting that section until value found! [AND we use the same/existing if/else branches (with some additions and changes!)]
//
// -- while(l<=r) --
// mid = l + (r -l)/2;
// if(nums[mid] == target) //also plays part in l==r==m if the values exists!
//      return mid;
// if( nums at indices [l,mid] are sorted) //but like in TwoSeparteSearches, this handles r==m <=> l==r!
// {
//      if(target here)
//        //WRONG: //standard binary search, just move our existing l and r like in search function
//        // we want to bisect this half further! : 
//        r = mid - 1; //ALREADY CHECKED IN FIRST BRANCH AND NEEDED TO BREAK OUT OF LOOP!
//      else
//         l = mid+1; // discard the half we checked
// }
// else //( nums at indices [mid, r] are sorted)
// {
//      if(target here)
//        //WRONG: //standard binary search, just move our existing l and r like in search function
//        // we want to bisect this half further! : 
//        l = mid + 1; //ALREADY CHECKED IN FIRST BRANCH AND NEEDED TO BREAK OUT OF LOOP!
//         
//      else
//         r = mid-1; // discard the half we checked
// }
// --END WHILE--
// return -1;

public class Solution_TwoSeparteSearches { 
    public int Search(int[] nums, int target) {
        //dunno if I could mess this up if I tried, honestly.
        // Let's see if that's confidence or over confidence!

        int l = 0, r = nums.Length-1;

        while(l<=r) // This problem is deceptively similar to the minimum in rotated sorted array, but has some fundamental differences (i.e., we know value of what we're looking for!)
        {
            int mid = l + (r-l)/2; //floor

            if(nums[l] <= nums[mid]) 
            {
                //[l, mid] is sorted (or l==mid, which happens when l==r since we use floor)

                // We try to find target here,
                // or discard if it's not here!
                if(nums[l] <= target && target <= nums[mid]) //only had target < nums[mid] earlier because I did not account for rotation!
                {
                    return BinarySearch(nums, l, mid, target); //WAS PASSING L, R BEFORE BECAUSE I WAS STUPID
                }
                
                // target not here! Discard this half!
                l = mid+1;

                // r = mid;

                // oh wait, since target may or may not exist, it could be < nums[l] or > nums[r] !!!!
                // also, might have forgotten it was rotated midway :O
            }
            else //if(nums[mid] < nums[r])  {also, the case of mid ==r is handled above as that would mean l==r since we use floor}
            {
                // [mid, r] is sorted (and mid != r != l)
                
                if(nums[mid] <= target && target <= nums[r])
                {
                    return BinarySearch(nums, mid, r, target); //WAS PASSING L, R BEFORE BECAUSE I WAS STUPID
                }
                r = mid - 1;
            }            
        }

        return -1; // target was < min(nums) || > max(nums)
        //29 minutes but do I get 2:42am credits (with at least a slight headache)?
    }

    public int BinarySearch(int[] nums, int l, int r, int target) 
    // would be better to pass a struct to avoid passing values in wrong order or too much overhead!
    {
        while(l<=r)
        {
            int mid = l + (r-l)/2; //floor

            if(nums[mid] == target)
            {
                return mid;
            }
            else if(nums[mid] > target)
            { //need smaller!
                r = mid - 1;
            }
            else // >
            {
                l = mid + 1;
            }
        }

        return -1;
    }
}


public class Solution_SingleLoop {
    //Only got this version because "AI Interviewer" gem I created on Gemini suggested this. I personally still think the TwoLoops version is more readable.
    public int Search(int[] nums, int target) {
        if(nums.Length == 1 && nums[0] == target)
            return 0;

        int l = 0, r = nums.Length-1;
        while(l<=r) //so, this is where I tripped up 
        {
            int mid = l + (r-l)/2;
            if(nums[mid] == target)
                return mid;
            if(nums[l]<=nums[mid])//this half is sorted! //got <= when I asked AI interviewer about my concerns for l==mid
            {
                if(nums[l] <= target && target <= nums[mid]) //target must be in left half
                {
                    r = mid-1; //-1 because mid was already checked in if condition above
                }
                else { //target must be in right half!
                    l = mid+1; //+1 because [l,m] half was already checked!
                }
            }
            else //[mid, r] is sorted (because inflection point has to be in one of the places!)
            {
                if(nums[mid] <= target && target <= nums[r]) //target must be in right half
                {
                    l = mid+1; //+1 because mid was already checked in if condition above
                }
                else //target must be in right half
                {
                    r = mid-1; //-1 because [l,m] half was already checked!
                }
            }
        }

        return -1;
    }

}


public class Solution_TwoLoops {
    public int Search(int[] nums, int target) {
        if(nums.Length == 1 && nums[0] == target)
            return 0;

        int l = 0, r = nums.Length-1;
        while(l<=r) //so, this is where I tripped up 
        {
            int mid = l + (r-l)/2;
            if(nums[l]<=nums[mid])//this half is sorted! //got <= when I asked AI interviewer about my concerns for l==mid
            {
                if(nums[l] <= target && target <= nums[mid])
                {
                    return Search(nums, l, mid, target);
                }
                l = mid+1; //+1 because [l,m] half was already checked!
            }
            else //[mid, r] is sorted (because inflection point has to be in one of the places!)
            {
                if(nums[mid] <= target && target <= nums[r])
                {
                    return Search(nums, mid, r, target);
                }
                r = mid-1; //-1 because [l,m] half was already checked!
            }
        }

        return -1;
    }

    public int Search(int[] nums, int l, int r, int target)
    {
        while(l<=r)
        {
            int mid = l + (r-l)/2; //assuming no overflow
            if(nums[mid] == target)
            {
                return mid;
            }
            else if(nums[mid] > target)
            {
                //look left
                r = mid-1;
            }
            else //nums[mid] < target
            {
                //look right
                l = mid + 1;
            }
        }
        return -1;
    }
}



//Last Actual Solution:
// public class Solution {
//     public int Search(int[] nums, int target) {
//         return attempt1(nums,target);
//     }

//     public int attempt1HelperFindMinElemIdx(int[] nums)
//     {
//         //Based on solution of Minimum in Rotated Sorted Array
//         int l=0, r=nums.Length-1;
//         while(l<r)
//         {
//             int m = (l+r)/2; //== l + (r-l)/2 = (2l-l+r)/2 = (l+r)/2
//             if(nums[m]>nums[r])
//                 l=m+1;
//             else
//                 r = m;
//         }
//         return l; //l==r
//     }
//     public int attempt1(int[] nums, int target)
//     {
//         int minElemIdx = attempt1HelperFindMinElemIdx(nums);
//         int l = minElemIdx, r = minElemIdx+nums.Length-1;
//         //we use modulo to pretend we copied the array and pasted it right at the end of it,
//         //then we have the full array from l to the above r.
//         while(l<=r)
//         {
//             int m = (l+r)/2; //== l + (r-l)/2 = (2l-l+r)/2 = (l+r)/2
//             if(target<nums[m%nums.Length])
//                 r=m-1;
//             else if(target>nums[m%nums.Length])
//                 l=m+1;
//             else // ==
//                 return (m%nums.Length);
//         }
//         return -1;
//     }
// }
