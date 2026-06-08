public class Solution {
    // SINCE RESULTS ARRAY IS NOT COUNTED TOWARDS AUXILIARY SPACE, ALWAYS TRY TO USE THAT TO ACHIEVE BETTER SPACE COMPLEXITY!
    public int[] ProductExceptSelf(int[] nums) {
        // return (new ArrProductExceptSelfNoDivideSolver()).BestSolution(nums);
        return (new ArrProductExceptSelfNoDivideSolver_Retry()).Solve(nums);
    }
}

// 2026.2: Solution (review):

// Time taken: 8 minutes
// TC = O(N)
// Auxiliary SC = O(1) [total sc = o(n)]
internal class ArrProductExceptSelfNoDivideSolver_Retry {
    public int[] Solve(int[] nums) {
        GeneratePrefixProduct(nums, out var res);
        FillSuffixProduct(nums, res);
        return res;
    }

    public void GeneratePrefixProduct(int[] src, out int[] res) {
        res = new int[src.Length];
        res[0] = 1;
        for(int i=1; i<src.Length; i++) {
            res[i] = res[i-1]*src[i-1];
        }
    }

    public void FillSuffixProduct(int[] src, int[] res) {
        int rollingProduct = 1;
        for(int i = src.Length-2; i >= 0; i--) {
            rollingProduct *= src[i+1];
            res[i] *= rollingProduct;
        }
    }
}


//2026.1: Solution
internal class ArrProductExceptSelfNoDivideSolver
{

    public int[] BestSolution(int[] nums) 
    {
        // SC: O(1) Auxiliary (result array is usually excluded in this problem's constraints, since O(N) result storage is expected of us, it doesn't count as auxiliary storage!)
        // TC: O(N) because we make exactly two linear passes.
        var result = new int[nums.Length];
        
        // Step 1: Fill result with the product of everything to the LEFT of each index.
        PrefixProductPass(nums, result);
        
        // Step 2: Multiply those values by the product of everything to the RIGHT.
        SuffixProductPass(nums, result);
        
        return result;
    }

    internal void PrefixProductPass(int[] nums, int[] res) 
    {
        // Base Case: There is nothing to the left of the first element, so its prefix product is 1.
        res[0] = 1;

        for(int i = 1; i < nums.Length; i++)
        {
            // TRICK: To get everything to the left of 'i', take:
            // 1. Everything to the left of 'i-1' (which is already in res[i-1])
            // 2. Multiply it by the value AT 'i-1' (nums[i-1])
            res[i] = res[i - 1] * nums[i - 1];
        }
    }

    internal void SuffixProductPass(int[] nums, int[] res) 
    {
        // productSoFar keeps track of the "running total" from the right side.
        // We start at 1 because there is nothing to the right of the last element.
        var productSoFar = 1;

        // We start at Length-2 because the last element (Length-1) 
        // already has its correct prefix product and doesn't need to be multiplied by anything.
        for(int i = nums.Length - 2; i >= 0; i--)
        {
            // Update productSoFar to include the number we just passed (to the right of 'i').
            productSoFar *= nums[i + 1];

            // COMBINE: Multiply the existing Prefix (in res[i]) by the new Suffix (productSoFar).
            res[i] *= productSoFar;
        }
    }

    //BETTER SOLVED BY JUST CALCULATING SUFFIXES AND PREFIXES IN THE RESULT ARRAY!
    public int[] Solve_Deprecated(int[] nums) //Total SC: O(N) and Total TC: O(N)
    {
        if(nums.Length<=1)
            return nums;
        //My soln. should work for length>=2.

        var preProd = GetPrefixProduct(nums);
        var sufProd = GetSuffixProduct(nums);
        
        var result = new int[nums.Length];
        result[0]=sufProd[1];
        result[nums.Length-1]=preProd[nums.Length-2];

        for(int i=1; i<nums.Length+1; i++) //SC: O(N) and TC: O(N)
        {
            result[i] = sufProd[i+1] * preProd[i-1];
        }

        return result;
    }

    public int[] GetPrefixProduct(int[] nums) //SC: O(N) and TC: O(N)
    {
        var preProd = new int[nums.Length];
        preProd[0] = nums[0];
        for(int i=1; i<nums.Length; i++)
        {
            preProd[i]=preProd[i-1]*nums[i];
        }
        return preProd;
    }

    public int[] GetSuffixProduct(int[] nums) //SC: O(N) and TC: O(N) //AKA Postfix Prod
    {
        var sufProd = new int[nums.Length];
        sufProd[nums.Length-1] = nums[nums.Length-1];
        for(int i=nums.Length-2; i>=0; i--)
        {
            sufProd[i]=sufProd[i+1]*nums[i];
        }
        return sufProd;
    }
}



// Last Actual Solution:
// public class Solution {
//     // We are given Each product is guaranteed to fit in a 32-bit integer.
//     //BUT it might serve as a GREAT CLARIFYING QUESTION!!
//     public int[] ProductExceptSelf(int[] nums) {
//         // return attempt1(nums);
//         return attempt2(nums);
//     }

//     public int[] attempt1(int[] nums) {
//         int[] output = new int[nums.Count()];
//         int product = 1; //from right to here
//         for(int i=nums.Count()-2;i>=0;i--)//-2 cuz we don't need the product there.
//         {
//             product*=nums[i+1];
//             output[i]=product;
//         }
//         product = 1;//now from lhs
//         for(int i=0;i<nums.Count()-1;i++)
//         {
//             output[i]=output[i]*product;
//             product*=nums[i];
//         }
//         output[nums.Count()-1]=product;
//         return output;
//     }


//     public int[] attempt2(int[] nums)
//     {
//         var res = new int[nums.Count()];

//         res[0]=1;
//         for(int i=1;i<nums.Count();i++) //we fill result array with product of nums from L.H.S (except itself)
//         {
//             res[i]=res[i-1]*nums[i-1];
//         }

//         int product = 1;
//         for(int i=nums.Count()-1;i>=0;i--) //WE NEED PRODUCT CUZ WE AREN'T STORING THIS PRODUCT ANYWHERE (theoretically we could use nums array for it)
//         {
//             res[i]*=product;
//             product*=nums[i];
//         }
        
//         return res;
//     }
// }
