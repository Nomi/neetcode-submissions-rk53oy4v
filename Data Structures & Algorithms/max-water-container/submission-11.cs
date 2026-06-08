public class Solution {
    public int MaxArea(int[] heights) { //TC: O(N), SC: O(1).
        int maxArea = 0;
        int l = 0, r = heights.Length-1;
        while(l<r) //0 area at l==r
        {
            //Current rectangle formed details:
            var width = r-l; //we use each step as a unit, but this could be converted by multiplication to any measurment unit //also assuming steps are uniform which makes sense here, alongside countless other assumptions being made on lack of mention in problem statement
            var height = (int)Math.Min(heights[l], heights[r]); //Obviously, any container can only hold water upto the smallest wall it has.
            var area = width*height; //in our case, area is the amount of water a container can hold, cuz it's 2D.
            if(maxArea<area) maxArea=area;


            // ## Case 1:
            // As we said earlier, it is obvious that any container can only hold water upto the smallest wall it has.
            // No matter where we move the higher bar, the smaller bar will ensure that the height will be limited by it.
            // Moving the lower bar however, gives us a chance to increase our volume (even if we have to go through even lower values first).

            // Another thing about this is, as long as it was the smaller bar, it has contributed the maximum area it could ever contribute.
            // Shrinking higher bar to even higher (or lower) would always decrease the width and hence give worse results!
            // This area is the maximum the lower bar could ever hope to provide!
            
            // ## Case 2
            // What happens with equal heights though? (had to ask AI for hint but it gave me a sentence which was basically the answer)
            // Well, moving either bar works (and the next will be moved in future iteration), because we would need a higher bar on both sides anyway to get a higher area.
            // Or, you could move both (for the same reason!).
            // Why? Because:
            // 1. We decreased width
            // 2. The area can only be lower or at MAXIMUM it will be the same (when there is a higher bar on the other sides)
            // So, our best hope is just getting big enough bars (on both sides) to make up for the width loss from these 2.

            //So, yet again we do greedy shrinking of our "search space"!


            // =====================================================================================
            // THEORETICAL OVERVIEW: Why this is a "Search Space"
            // -------------------------------------------------------------------------------------
            // 1. MONOTONIC WIDTH: Even if heights are unsorted, Width is perfectly ordered.
            //    By starting at l = 0 and r = length - 1, we begin with the MAXIMUM possible width.
            //
            // 2. THE TRADE-OFF: Every pointer move strictly decreases the width:
            //    N-1 -> N-2 -> ... -> 1
            //
            // 3. THE LOGIC: We are systematically and greedily trading Width for the *possibility* 
            //    of better Height. Because Width only moves in one direction (down), we have a 
            //    "Monotonic" property that guarantees we exhaustively search all viable candidates.
            // =====================================================================================
            if(heights[l] < heights[r])
                l++;
            else if(heights[l] > heights[r])
                r--;
            else //heights[l] == heights[r]
            {
                l++;
                r--;
            }
        }
        return maxArea;
    }
}


// Last Actual Solution:
// //Check my physical notes for Max Water Container (on BWS notebook)
// //to see why this works! (and how)
// public class Solution {
//     public int MaxArea(int[] heights) {
//         //return attempt1(heights); //TC: O(N) //READ THE COMMENTS??

//         return attempt2(heights);
//     }

//     //TC: O(N)
//     //Since width has to decrease as we change heights, we want to make sure the bar we swap is the smaller one and make it bigger.
//     //almost like having pre-sorted width because we start from max width and decrease it by 1 with every change of bars.
//     public int attempt1(int[] heights)
//     {
//         int maxAr = -1;
//         int l=0, r=heights.Count()-1; //we start from max width and decrease it as a tradeoff for height from hereonforth.
//         while(l<r)//not == because that's just one bar.
//         {
//             int ar = (int)Math.Min(heights[l],heights[r])*(r-l);
//             maxAr = ar>maxAr ? ar : maxAr;
//             //It is better to make the smaller bar bigger than
//             //messing with the bigger of the two.
//             if(heights[l]>heights[r])
//             {
//                 r--;
//             }
//             else
//             {
//                 l++;
//             }
//         }
//         return maxAr;
//     }


//     public int attempt2(int[] heights)
//     {
//         int l=0,r=heights.Count()-1;
//         int maxArea = 0;
//         while(l<r)
//         {
//             int hl = heights[l];
//             int hr = heights[r];
//             int area = (r-l)*(int)Math.Min(hl,hr);
//             if(area>maxArea)
//                 maxArea = area;
//             if(hl<hr)
//                 l++;
//             else
//                 r--;
//         }
//         return maxArea;
//     }
// }

