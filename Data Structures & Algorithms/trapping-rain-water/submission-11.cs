public class Solution {
    public int Trap(int[] height) //NC solution turned out to be pretty much the same except they use while(l<r) by doing some tricks beforehand, but they're not much different tbh
    {
        if (height == null || height.Length == 0) {
            return 0;
        }
        
        int l=0, lMaxH = 0;
        int r=height.Length-1, rMaxH = 0;
        int totalWater = 0;
        while(l <= r) 
        {
            // The diagram is hugely helpful and a big part of the reason I was able to do this much tbh. What do I do in the interview???
            // Also, cuz I saw need for tracking lMax and rMax elsewhere (which might be one of the biggest hints I could have gotten)!
            // And the fact that I got AI to give me some light nudges (but it might have done juuust a bit more than a nudge XD).

            // NOTE THAT HEIGHT IS ALSO VOLUME KINDA (because width of each index is 1)

            // We handle (lMaxH <= rMaxH) together. (explained in comments inside the block)
            if(lMaxH <= rMaxH) 
            {
                // ### Logic [for lMaxH < rMax]:
                // We know for sure we can put exactly lMaxH amount of water at the current square BECAUSE:
                // 1. left side bottle neck is lMaxH and it is smaller than rMaxH, that means no matter 
                //    EVEN IF every other height is 0, we've got lMaxH to support it from the left side,
                //    and rMaxH to support it from the right side. 
                // 2. And EVEN IF every height between l and r were infinity (we know whats biggest to the left and right of them already),
                //    the bottleneck is what's the smaller of the 2. In this case, lMaxH.
                // 3. And also, we greedily try to find a bigger left bound (one that can hopefully bring more water holding capacity at r by being closer to, equal to, or bigger than r)
                // 4. Since maxH'es are NOT equal, we have to calculate for the point where r and l cross over (l==r) 
                //     by the side with lower max to get water. (unlike the "max area" problem since a single height by itself has a 
                //     width of 0 in that problem and 1 in this problem and we didn't even need to include it in the previous 
                //     problem becasue of width being 0)
                
                // The rest is the pretty much same as the "max area" problem right before this.
                
                // ### Logic [for lMaxH == rMaxH && l != r)]:
                // Similar base logic to the above. 
                // Also, kinda similar to how it's done in the "max area" problem right before this.
                
                // ### Logic [for lMaxH == rMaxH && l == r)]:
                // When l==r (l and r cross over), similar to lMaxH<rMaxH logic, we still need to calculate for crossover point!
                //
                // As such, if we created a separate else for lMaxH==rMax, we would have to handle it separately because in this problem
                // we do care about NOT DOUBLE COUNTING at the crossover point (since we have to include it for calculations)
                // (i.e. only calculating for either r or l in this specific case, doesn't matter which)
                // This would clutter the code a lot because of an additional else block containing repetition of contents of the other 2 
                // condition branches like in previous problem, but also one of them would need to be in an if condition (so one side does not
                // calculate when the other is the same as it)

                if(lMaxH < height[l]) 
                    lMaxH = height[l]; // doing this before calculating water handles the case of leaks (to left)
                
                totalWater += lMaxH - height[l]; 
                l++;
            }
            else // lMaxH > rMax
            {
                // Logic: Same logic as "Logic [for lMaxH < rMax]", just l and r switched around! (and crossover point not handled!)
                // We know lMaxH is strictly taller, so rMaxH is the bottleneck.

                if(rMaxH < height[r]) 
                    rMaxH = height[r]; // doing this before calculating water handles the case of leaks (to right)
                
                totalWater += rMaxH - height[r];
                r--;
            }
        }

        return totalWater;
    }

    public int Trap_New1stAmpt_Long(int[] height) { //This current version is apparently the global bottleneck version, meanwhile the typical approach is apparently the local bottleneck approach (you move the one with the smaller height instead of the smaller max)?
        int l=0, lMaxH = 0;
        int r=height.Length-1, rMaxH = 0;
        int totalWater = 0;
        

        while(l<=r) ///I wouldn't have thought of l<=r if I hadn't seen the diagram on neetcode description.
        {
            //Still greedy shrinking!

            //The diagram is hugely helpful and a big part of the reason I can do this much tbh
            
            //Now the actual logic (I think?):
            if(lMaxH < rMaxH) //NOTE THAT HEIGHTS ARE ALSO VOLUME KINDA (because width of each index is 1)
            {
                // Logic:
                //we know for sure we can put exactly lMaxH amount of water at the current square BECAUSE:
                // 1. left side bottle neck is lMaxH and it is smaller than rMaxH, that means no matter 
                //    EVEN IF every other height is 0, we've got lMaxH to support it from the left side,
                //    and rMaxH to support it from the right side. 
                // 2. And EVEN IF every height between l and r were infinity (we know whats biggest to the left and right of them already),
                //    the bottleneck is what's the smaller of the 2. In this case, lMaxH.
                // The rest is pretty much based on max area problem (the one before this)

                if(lMaxH < height[l]) lMaxH = height[l]; //doing this before calculating water handles the case of leaks (to left)
                totalWater += lMaxH - height[l]; 

                l++;
            }
            else if(lMaxH > rMaxH)
            {
                //Logic: Same logic as previous branch of the if, just l and r switched around!

                if(rMaxH < height[r]) 
                    rMaxH = height[r]; //doing this before calculating water handles the case of leaks (to right)
                totalWater += rMaxH - height[r];

                r--;
            }
            else // lMaxH == rMaxH
            {
                // Same logic as the first branch of the if, but this time, we know definitely BOTH SIDES can support each other!

                if(lMaxH < height[l]) 
                    lMaxH = height[l]; //doing this before calculating water handles the case of leaks (to left)
                totalWater += lMaxH - height[l];

                if(l != r) //wouldn't need this if I just combined this parent else with the above and removed this if condition's contents and just let the next step take care of the other side in equal cases (as happens in other people's solutions for this and the max area problem just before to this)
                {
                    if(rMaxH < height[r]) 
                        rMaxH = height[r]; //doing this before calculating water handles the case of leaks (to right)
                    totalWater += (int)Math.Max(0, rMaxH - height[r]);
                }

                l++;
                r--;
            }
            
        }
        return totalWater;
    }
}




// # Last Actual Solution:

// //Check my physical notes for Max Water Container (on BWS notebook)
// //to see why this works! (and how)
// public class Solution {
//     //## TWO POINTER APPROACH:
//     //# TC = O(N)
//     //# SC = O(1)
//     public int Trap(int[] height) {
//         // return attempt1(height); //PREFER LOOP STYLE FROM HERE!

//         return attempt2(height); //USE ATTEMPT1 LOOP STILE RATHER THAN THIS!!
//     }

//     //## TWO POINTER APPROACH:
//     //# TC = O(N)
//     //# SC = O(1)
//     public int attempt1(int[] height)
//     {
//         if(height==null||height.Count()==0)
//             return 0;

//         // // NeetCode Video Assisted:
//         int water = 0;
//         int l=0, r=height.Count()-1;
//         int lMax=height[l], rMax = height[r];
//         while(l<r)
//         {
//             if(lMax<rMax)
//             {
//                 l++;
//                 lMax= (int) Math.Max(lMax,height[l]);
//                 water += lMax-height[l]; //never negative because of the statement directly above!
//             }
//             else
//             {
//                 r--;
//                 rMax= (int) Math.Max(rMax,height[r]);
//                 water += rMax-height[r]; //never negative because of the statement directly above!
//             }
//         }
//         return water;


//         ////My old attempt:
//         // for(int l=1;l<-2-1+height.Count();l++)
//         // {
//         //     int r=l+1;
//         //     // if(height[l]<height[r])
//         //     //     continue;
//         //     while(r+1<height.Count()&&height[r]<height[r+1])
//         //         height++;
//         //     int minHeight
//         // }
//     }

//     public int attempt2(int[] height)
//     {
//         if(height==null||height.Count()==0)
//             return 0;
        
//         int l = 0, r = height.Count()-1;
//         int lMax = 0, rMax = 0;
//         int water=0;
//         while(l<=r) //the way I have written this, it wouldn't work with l<r
//         {
//             //it doesn't work with l<r because we would not
//             //calculate the water on the middle block (for odd numbered blocks).
//             Console.Write($"{l},{r} : {lMax},{rMax} :");
//             if(lMax<rMax)
//             {
//                 int temp = (int)Math.Min(lMax,rMax)-height[l];
//                 if(temp>0) //The current bar is not higher than water level (i.e. not negative ot zero)
//                     water+=temp;
//                 lMax = (int)Math.Max(lMax,height[l]);
//                 l++;
//                 Console.WriteLine($"l - {temp} : total_water - {water}");
//             }
//             else
//             {
//                 int temp = (int)Math.Min(lMax,rMax)-height[r];
//                 if(temp>0) //The current bar is not higher than water level (i.e. not negative ot zero)
//                     water+=temp;
//                 rMax = (int)Math.Max(rMax,height[r]);
//                 r--;
//                 Console.WriteLine($"r - {temp} : total_water - {water}");
//             }
//         }
//         return water;
//     }

// }
