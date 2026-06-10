
//# *IMPORTANT*: FOR NOTES, check my yesterday's attempt right after this class!!

public class Solution {

    public int LargestRectangleArea(int[] heights)  
    {
        // return LargestRectangleArea_Review(heights);

        //I already know this problem specifically is a monotonic stack problem, not sure if I could spot it.
        // What is the pattern anyway?
        // I do know we keep adding bigger heights (until we can't expand) because that's how long we can keep extending!

        //Each height having width 1 seems like an important detail because even one bar has its own height to contribute!

        int maxArea = -1;
        
        Stack<int> toExtend = new(); //I just rememebred I need to to store indices for how far left we can extend from!
        //Wait I don't need a tuple, I can get height from index! :D

        toExtend.Push(-1); //IMPORTANT: I FORGOT I COULD PUT THIS SENTINEL VALUE HERE TO MAKE SURE ALL ELEMENTS AT THE END ARE PROCESSED PROPERLY FOR SINGLE ELEMENT ARRAYS!!

        for(int i=0; i <= heights.Length; i++) //need <= for single element arrays and the final one! 
        {
            //OH WAIT, NOW IT DOESN'T APPLY ANYMORE DUE TO THE `i==heights.Length` PART: //if heights could be negative, we'd have to check toExtend.Count > 1 (since we now have our sentinel value there!), but we can keep it >0 for now
            while(toExtend.Count > 1 && (i==heights.Length || heights[toExtend.Peek()]>heights[i])) //heights[toExtend.Peek()]>heights[i] => can't extend current top of toExtend any further(even at i)!
            {//since i <=, we have to do the heights check!
                var topIdx = toExtend.Pop();

                // Bounds (Exclusive):
                
                // var leftBoundIdx = toExtend.Peek(); //note that even if leftBound == cur, we will still end up calculating the correct area for their common height when we try to extend that one!
                //Oh almost forgot that for the last element in stack, we might get a failed peek! So, we set leftBound to topIdx-1 (-1 for first ever iteration for i=0)
                // var leftBoundIdx = toExtend.Count > 0 ? toExtend.Peek() : topIdx-1; //note that even if leftBound == cur, we will still end up calculating the correct area for their common height when we try to extend that one!
                

                //WAIT, I'm messing up. How far back can we extend? Until the last element smaller than current one (exclusive)! 
                // Which just is always on top, and that makes sense!
                // And as long as we also insert 0s into the stack (AND DO NOT POP THEM EVEN IF ANOTHER 0 COMES ALONG, i.e. keep > symbol as is),
                // we can be sure that elements only extend as far back as they can!
                // Now what happens if there are no elements in the stack from before? What does it mean, it means all elements that were ever in it
                // were smaller than the current one, so we extend back to 0 (inclusive)! Why? because this is a montonic increasing (not strictly)! 
                // And the stack wouldn't be empty unless all elements before current were smaller than it! [idk why I ever let topIdx-1 get there just now!]
                
                // var leftBoundIdx = toExtend.Count > 0 ? toExtend.Peek() : -1; //note that even if leftBound == cur, we will still end up calculating the correct area for their common height when we try to extend that one!
                var leftBoundIdx = toExtend.Peek(); //don't need this after the -1 sentinel in stack!// : -1; //note that even if leftBound == cur, we will still end up calculating the correct area for their common height when we try to extend that one!
                var rightBoundIdx = i; //obvious but makes it easier to track!
                //the above is 

                var width = rightBoundIdx - leftBoundIdx - 1; //since both are indices, we donot need to do a +/- 1 on that account
                                                        // AND, since rightbound and leftbound are both exclusive, we need to subtract the

                //e.g. for [0,5,4] when adding 4 (and removing 5), we get width = 2-(0)-1= 1 and for [2], we now get here with i=1 width = 1 - (-1) -1 = 1
                // damn, I don't remember the formula and am struggling with this since 20 mins (it's 23mins now)
                maxArea = Math.Max(width*heights[topIdx], maxArea);
                //damn, finished at 7 mins, but should've been 20 if my maths wasn't off in the example (tbf I'm sleepy and mentally exhausted)
            }
            toExtend.Push(i); //DAMN AT 40 MINUTES I REALIZED I FORGOT TO ADD THIS IN THE MIDDLE OF EVERYTHING!
        }

        //To be fair, my solution at 20mins was mostly finished and correct, I just miscalculated the [2] example (perhaps because I'm mentally exhausted and sleepy)!
        return maxArea;
    }

    public int LargestRectangleArea_Review(int[] heights) { //2026.2 :  Solution (Review):
        // FOR EXPLANATION, READ THE NON REVIEW VERSION!
        int maxArea = -1;

        Stack<int> toExtend = new();
        toExtend.Push(-1); //Sentinel to make inner while loop easier for last case. (for leftBottleNeckIdx for curToExtendIdx == 0)

        for(int i = 0; i <= heights.Length; i++) { // We want Monotonically NON-Decreasing Stack because that's as long as we can extend prior bars' height.
            while(toExtend.Count > 1  && (i == heights.Length || heights[toExtend.Peek()] > heights[i])) {
                var curToExtendIdx = toExtend.Pop();
                var leftBottleneckIdx = toExtend.Peek();
                var rightBottleneckIdx = i;

                var width = rightBottleneckIdx - leftBottleneckIdx - 1;

                var curArea = width * heights[curToExtendIdx];

                if(curArea > maxArea) maxArea = curArea;
            }
            toExtend.Push(i);
        }

        return maxArea;
    }
}



// public class Solution { //This is my second attempt after yesterday! 
//     public int LargestRectangleArea(int[] heights) //TC = O(N), Aux SC = O(N) [worst case -> when all heights are montonically increasing (not sorted) anyway so stack is filled fully by the last iteration where we pop em all]
//     {
//         int maxArea = 0;

//         Stack<int> extendable = new();
//         // while(extendable.Count>0) //Almost ended up using this but I knew I had to use `for` from yesterday, but I did figure out why, because we need to iterate through heights mainly so it's more convenient
//         for(int i = 0; i <= heights.Length; i++)
//         {
//             //`ALPHA 1`: curHeight was added here instead of just using index in the while loop condition to account for the last pass!
//             //i.e. when elements can be extended to the end of the array! (i will be heights.Length anyway!)
//             var curHeight = i==heights.Length ? 0 //only 0s' idx (incl. one for curHeigths) will be left in the stack at the end this way, which doesn't matter! (unless memory concerned)
//                                             : heights[i]; 
//             while(extendable.Count > 0  &&  curHeight < heights[extendable.Peek()]) // just rememebered stack needs to store indices to calculate distances 
//             { 
//                 //also just remembered since we use these indices to calculate how far a height at a certain INDEX CAN be extended, we need to push even bars same width, so not strictly decreasing
//                 //we're also storing indices in monotnonic increasing order of heights at that index because previous bars can only extend upto this point if all bars until that point are bigger or equal
//                 // that's why heights[i] < heights[extendable.Peek() is in condition
//                 var toExtendIdx = extendable.Pop();
//                 //ALSO JUST REMMEBERED WE NEED TO PROCESS LEFTOVER INDICES IN STACK, WHETHER HERE OR A SUBSEQUENT LOOP before returning!

//                 var leftFirstSmallerIdx = extendable.Count == 0 ? -1 : extendable.Peek(); //can only extend upto here because it is smaller or equal to current index, even if it is equal, we will calculate the proper height when we process that element in next iteration!
//                 // -1 ABOVE BECAUSE WE CAN EXTEND UP TO THE START OF HEIGHTS ARRAY => INCLUDING 0 IDX BUT SUBSEQUENT CALCULATIONS EXCLUDE 0

//                 //ALSO, CAN ONLY EXTEND ALL UPTO `i` ON RIGHT BECAUSE THAT IS THE FIRST ELEMENT SMALLER THAN CURRENT HEIGHT AT toExtendIdx 
//                 int width = i - leftFirstSmallerIdx - 1; // For range (l, r) {exclusive of both ends!}, number of integral elements = r - l - 1; //got this wrong earlier, need to memorize this!
                
//                 maxArea = Math.Max(width*heights[toExtendIdx], maxArea);

//                 //`ALPHA 1`: also, chagned outer loop condition to `i <= heights.Length` from `<` because I will integrate the last pass into this loop!
//             }

//             //lol forgot to push before I got here
//             // extendable.Push(curHeight);
//             //wow I was pushing height, I'm stupid. Only compiler helped me find out!
//             extendable.Push(i);
//         }

//         return maxArea;
//     }
// }

//# THE BEST NOTE I WILL EVER MAKE:
// In discrete math, the number of elements between two indices `L` and `R` (EXCLUDING BOTH) is: `R - L - 1`
//
// More precisely, in discrete mathematics, for any two integers L and R:
// 
// 1. The number of integers in the closed interval [L, R] is:
//    R - L + 1
// 
// 2. The number of integers in the open interval (L, R) is:
//    R - L - 1
//
//#END OF NOTE


//Okay this problem sucks!

// # NEETCODE:
// 4. Stack (One Pass)
// Intuition:
// We want, for each bar, the widest area where it can act as the shortest bar.
// With a single pass and a stack, we can do this on the fly:
// - We keep a stack of bars in increasing height order, each stored with the earliest index where that height can start.
// - When we see a new bar that is shorter than the top of the stack, it means the taller bar on top can’t extend further to the right.
// -    - So we pop it and compute the area it could cover.
// - The new shorter bar can start from as far left as the popped bar’s start index, so we reuse that index.
// - After the pass, we compute areas for any bars still in the stack, extending them to the end.
// - Each bar is pushed and popped at most once, giving an efficient, one-pass solution.

/* # Gemini:
*
* ## APPROACH & PATTERN: Monotonic Increasing Stack (Indices)
*
* ### CORE CONCEPT: 
* A stack of indices implicitly encodes the left boundaries of the histogram bars.
*
* * ---------------------------------------------------------
* 1. THE "RIGHT BOUNDARY" LOGIC:
* ---------------------------------------------------------
* - When do you pop an element from the stack?
* - You pop stack.Peek() ONLY when the current bar you are looking at (heights[i]) 
* is shorter than it.
* * - Therefore: The current index `i` is the Right Boundary (exclusive). 
* The popped bar cannot extend to `i` or beyond because heights[i] is too short. 
* It stopped the streak.
* * ---------------------------------------------------------
* 2. THE "LEFT BOUNDARY" LOGIC:
* ---------------------------------------------------------
* - Once you pop the top element, look at the NEW top of the stack.
* * - Therefore: The new stack.Peek() is the Left Boundary (exclusive). 
* Why? Because if there was a bar in between newTop and poppedBar that was shorter, 
* poppedBar would have already been popped earlier. If there was one taller, 
* it would still be there.
* * ---------------------------------------------------------
* 3. THE "REMAINING ELEMENTS" LOGIC:
* ---------------------------------------------------------
* - What about the elements left in the stack at the very end?
* - They never met a bar shorter than them on the right.
* * - Therefore: Their Right Boundary is the end of the array (n).
* * ---------------------------------------------------------
* 4. THE UNIFIED FORMULA:
* ---------------------------------------------------------
* This unifies both the "Extending Right" (during the loop) and the 
* "End of Array" (after the loop) logic.
* * Width = RightLimit - LeftLimit - 1
* * - RightLimit: The index `i` that caused the pop (or `n` if at the end).
* - LeftLimit:  The index currently at the top of the stack (or -1 if stack is empty).
*/

// public class Solution {
//     public int LargestRectangleArea(int[] heights)
//     {
//         // 1. THIS might be the holy grail for this problem: "We want, for each bar, the widest area where it can act as the shortest bar."
//         //    (Side note: This is because we can cull the search space as we should always only consider rectangles of heights that are present in the input array,
//         //    because otherwise we would be wasting space. The only reasons to lower heights should be to fit under the another height, so it makes no sense to include values in between as they will only reduce the area from what has already been calculated.)
//         // 2. ALSO, that we need to store indices in stack! (to calculate distance!)
//         // 3. I REALLY NEED TO START THINKING ABOUT EDGE CASES!!! (e.g. what to do when stack still has elements at the end (extendable to the end!))
//         // 4. Read comments about `width` in the solution!

//         int maxArea = 0;

//         Stack<int> extendableBars = new Stack<int>(); //STACK DOES NOT SUPPORT [] or (){}??
//         extendableBars.Push(0); //I'M STUPID I HAD THIS EARLIER: //extendableBars.Push(heights[0]);

//         // We run the loop one extra time (i == n) to act as a '0' height bar.
//         // This 'flushes' all remaining bars out of the stack at the end.
//         for(int i=1; i <= heights.Length; i++)
//         {
//             var curHeight = (i==heights.Length) ? -1 : heights[i]; //To flush out all values out of stack: if we are past the end of the array, the height is -1 or 0  (anything >=0 works tbf).

//             //if we encounter a height smaller than current one, remove all the heights that cannot be extended from this point on.
//             //This means our stack will be monotonic increasing at all times! (strictly, because for same value repeating, we just keep the old one because that can extend farther to the right)
            
//             while(extendableBars.Count > 0 && curHeight<heights[extendableBars.Peek()]) //had <height (missing `s`) on the right and It keeps happening :(
//             {                  
//                 //# For `width`: 
//                 //
//                 // ## On the right: 
//                 // - All elements in the stack can be extended UP TO `i` (EXCLUSIVE). If you look at the loop condition, `i` is the first index with height bigger than at `idx`
//                 //
//                 // ## On the left: We can extend this bar to the next element in the stack (it is next smaller). 
//                 //  Let's look at ALL possible cases to prove this: 
//                 // - Case 1: There were any values between this and the element at top of stack at some point and they were removed by this element, those elements were bigger than this and that's why they were removed.
//                 //           The last element before this would always be the last one on the left this bar cannot be extended to.
//                 // - Case 2: There were no values between this element and the one at the top of the stack, still holds because the top to it is still smaller.
//                 // - Case 3: Only element in the list, but the guard clause in the loop makes sure we never have that here.
                
//                 var idx = extendableBars.Pop(); 
//                 int lastBiggerIdx = (extendableBars.Count == 0) ? -1 : extendableBars.Peek(); //-1 because width is calculated excluding lastBiggerIdx
//                 var width = i - lastBiggerIdx - 1; // In discrete math, the number of elements between two indices `L` and `R` (EXCLUDING BOTH) is: `R - L - 1`
//                 maxArea = Math.Max(heights[idx]*width, maxArea);
//             }

//             // IMPORTANT NOTE: THIS WAS WRONG! DO NOT DO THIS: if(extendableBars.Count == 0 || heights[i]!=heights[extendableBars.Peek()]) //DEPRECATED :mneant to have this earlier and even wrote it above. Just missed it :().
//             //Why? If kept, we track indices so if we start with heights = [0,1,0,1], we end up with stack with indices [0,3], meaning we would think our last 1 height bar extends from 3rd index to 1st index (inclusive), which makes no sense.
            
//             extendableBars.Push(i);
//         }
//         extendableBars.TryPop(out _); //Not really needed, cuz GC would work anyway, but just to remove the last fake index that got added here from the flushing logic

//         return maxArea;
//     }





//     public int LargestRectangleArea_SeparateLeftoverProcessing(int[] heights)
//     {
//         // 1. THIS might be the holy grail for this problem: "We want, for each bar, the widest area where it can act as the shortest bar."
//         //    (Side note: This is because we can cull the search space as we should always only consider rectangles of heights that are present in the input array,
//         //    because otherwise we would be wasting space. The only reasons to lower heights should be to fit under the another height, so it makes no sense to include values in between as they will only reduce the area from what has already been calculated.)
//         // 2. ALSO, that we need to store indices in stack! (to calculate distance!)
//         // 3. I REALLY NEED TO START THINKING ABOUT EDGE CASES!!! (e.g. what to do when stack still has elements at the end (extendable to the end!))
//         // 4. Read comments about `width` in the solution!

//         int maxArea = 0;

//         Stack<int> extendableBars = new Stack<int>(); //STACK DOES NOT SUPPORT [] or (){}??
//         extendableBars.Push(0); //I'M STUPID I HAD THIS EARLIER: //extendableBars.Push(heights[0]);
//         for(int i=1; i < heights.Length; i++)
//         {
//             //if we encounter a height smaller than current one, remove all the heights that cannot be extended from this point on.
//             //This means our stack will be monotonic increasing at all times! (strictly, because for same value repeating, we just keep the old one because that can extend farther to the right)
            
//             while(extendableBars.Count > 0 && heights[i]<heights[extendableBars.Peek()]) //had <height (missing `s`) on the right and It keeps happening :(
//             {                  
//                 //# For `width`: 
//                 //
//                 // ## On the right: 
//                 // - All elements in the stack can be extended UP TO `i` (EXCLUSIVE). If you look at the loop condition, `i` is the first index with height bigger than at `idx`
//                 //
//                 // ## On the left: We can extend this bar to the next element in the stack (it is next smaller). 
//                 //  Let's look at ALL possible cases to prove this: 
//                 // - Case 1: There were any values between this and the element at top of stack at some point and they were removed by this element, those elements were bigger than this and that's why they were removed.
//                 //           The last element before this would always be the last one on the left this bar cannot be extended to.
//                 // - Case 2: There were no values between this element and the one at the top of the stack, still holds because the top to it is still smaller.
//                 // - Case 3: Only element in the list, but the guard clause in the loop makes sure we never have that here.
                
//                 var idx = extendableBars.Pop(); 
//                 int lastBiggerIdx = (extendableBars.Count == 0) ? -1 : extendableBars.Peek(); //-1 because width is calculated excluding lastBiggerIdx
//                 var width = i - lastBiggerIdx - 1; // In discrete math, the number of elements between two indices `L` and `R` (EXCLUDING BOTH) is: `R - L - 1`
//                 maxArea = Math.Max(heights[idx]*width, maxArea);
//             }

//             // IMPORTANT NOTE: THIS WAS WRONG! DO NOT DO THIS: if(extendableBars.Count == 0 || heights[i]!=heights[extendableBars.Peek()]) //DEPRECATED :mneant to have this earlier and even wrote it above. Just missed it :().
//             //Why? If kept, we track indices so if we start with heights = [0,1,0,1], we end up with stack with indices [0,3], meaning we would think our last 1 height bar extends from 3rd index to 1st index (inclusive), which makes no sense.
            
//             extendableBars.Push(i);
//         }

//         while(extendableBars.Count > 0) 
//         { //Same notes as the popping inner loop in the above loop!
//             var idx = extendableBars.Pop();
//             int lastBiggerIdx = (extendableBars.Count == 0) ? -1 : extendableBars.Peek();
//             var width = heights.Length - lastBiggerIdx - 1; 
//             maxArea = Math.Max(heights[idx]*width, maxArea); 
//         }

//         return maxArea;
//     }
// }



// Last Actual Solution (WITH TIPS!!!)
// public class Solution {
//     public int LargestRectangleArea(int[] heights) {
//         //READ THE COMMENTS ON MY attempt1!! 
//         //check neetcode video!
//         //compare my solution (in terms of style) with what neetcode wrote?
//         return attempt1(heights);
//     }
    
//     public int attempt1(int[] heights) 
//     {
//         int maxArea = 0;

//         //solution brief description:
//         //WE NEED TO KEEP HEIGHTS IN INCREASING ORDER FOR THE ALGORITHM! (because we can't extend the prior rectangle to the right anymore).
//         //This means that for any element in the stack, we can extend its rectangle to the end of the stack (while keeping the height of the rectangle equal to it)
//         //But, we can't extend towards the older/lower elements in the stack.
//         //If it is not increasing, we pop (and calculate)
//         //This is a monotonic increasing stack (because we go from low on bottom to high on top)

//         //[IMPORTANT] SOLUTION STEPS:
//         //While we move to right and add elements to stack until we reach an element smaller than the current one (can't extend the rectangle to the right).
//         //Then we pop elements from the stack (while the top element is bigger than current one) and calculate the area of the bar on top of the stack.
//         //Then if the one before that is the same height, we calculate the area of that bar and the prior one (or until the last bar with same height) (won't have lesser height cuz we maintain monotonic increasing stack).
//         //If we meet a smaller one, we just calculate the rectangle from it (with its height) until now.
//         //If element on top of stack is smaller than the newly encountered element at any point, we stop and add that to the stack BUT we change the index to insert isntead of its index to the index of the last popped element because we could extend this one to the left until that point (because that's the last element >= this) and continue as we had before.
//         //If we reach end of input, we can do the same thing we did for encountering a smaller element because that is our trigger for calculating rectangles thus far.
//         //Since we are popping from the top (most recent) element, we will use stack. [the sentences above's references to 'stack' could just be replaced with 'from the storage']
//         //Watch the neetcode video for better visualization.
//         Stack<(int h,int i)>stack = new();
//         for(int i=0; i<heights.Length;)
//         {
//             if(i==0)//there will always be at least one element in the stack except on first iteration.
//             {
//                 maxArea = heights[i]*1;
//                 stack.Push((heights[i],i));
//                 i++;
//                 continue;
//             }
//             //stack.Count will always be >0 here.
//             while(i<heights.Length && stack.Peek().h<=heights[i])//< ???
//             {
//                 stack.Push((heights[i], i));
//                 i++;
//             }
//             //maxArea = stack.Peek().h*1; //last bar can not be extended and cannot be more than its width (1).
//             int lastUsableI = i;
//             while(stack.Count>0 && (i==heights.Length||heights[i]<stack.Peek().h))//<= ???
//             {
//                 var (th, ti) = stack.Pop(); //th will be smaller than heights[i] or any bars on top of it, as it is in a monotonic increasing stack (from bottom to top)
//                 maxArea = (int)Math.Max((i-ti)*th, maxArea);
//                 lastUsableI = ti;
//             }
//             if(i<heights.Length)
//             {
//                 stack.Push((heights[i],lastUsableI));
//             }
//             //LOGGING TUPLES:
//             //var tup = (h: heights[i], i: i);
//             // Console.WriteLine(tup); WOW YOU CAN PRINT TUPLES EASILY!!!
//         }
//         return maxArea;
//     }
// }
