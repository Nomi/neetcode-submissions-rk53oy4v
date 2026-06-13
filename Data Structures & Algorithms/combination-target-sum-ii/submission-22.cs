public class Solution {
    public List<List<int>> CombinationSum2(int[] candidates, int target) {
        // *IMPORTANT 1* CHECK MY NOTEBOOK NOTES FOR BACKTRACKING! THEY'RE THE BEST RESOURCE FOR BACKTRACKING OUT THERE!
        // *IMPORTANT 2* CHECK MY COMPLEXITY NOTES ON `Backtracking_Optimal_Retry1`!

        List<List<int>> results = new();
        Array.Sort(candidates);
        // Backtracking(candidates, target, 0, [], results);
        // Backtracking_Optimal(candidates, target, 0, [], results);
        Backtracking_Optimal_Retry1(candidates, target, 0, [], results);
        return results;
    }

    //Time Complexity:
    // - Let N be candidates.Length.
    // - (1) For each place, the number of ways we can put any specific element (unique idx, but not neccesarily unique value) 
    // we can put there is 2^(N+1) //+1 is for the "Empty" option.
    // - Also, the longest possible valid or first invalid state is when we consider every element,
    // and there are N elements. (e.g. no including any & including every which may or may not sum to target)
    // - (2) Therefore we can assume we have N places.
    // - From (1) and (2), we can see that:
    // - (3) To fill all places, final TC = N * 2^(N+1) 
    // [worst branch is when we don't pick any element until the end and have to make decision for N elements at every step] 

    // TC = O(N*2^N)

    // Aux. SC = O(N) (at any point we only store 1 single path, which at max can be == num elements, e.g. when we pick everything)

    // Total SC = O(N*2^N) (since number of valid paths is never bigger than all paths, which is O(N*2^N))

    public void Backtracking_Optimal_Retry1(Span<int> candidates, int target, int curSum, List<int> cur, List<List<int>> results)
    {
        // *IMPORTANT* CHECK MY NOTEBOOK NOTES FOR BACKTRACKING! THEY'RE THE BEST RESOURCE FOR BACKTRACKING OUT THERE!

        // Backtracking is traversing a "Decision Tree" (or State-Space Tree) to find valid configurations.
        // Here, decision is:
        // Which UNUSED, UNIQUE elements can we place in the current place?
        if(curSum == target)
        {
            results.Add(new(cur));
            return; //only valid solution on this path, so backtrack (even 0s being allowed is handled by us having sorted ascending).
        }

        if(curSum > target || candidates.Length == 0)
        {
            return; //backtrack
        }

        //Copied the above 2 ifs, so time will only be for my next code:
        for(int i=0; i<candidates.Length; i++)
        {
            if(i > 0 && candidates[i] == candidates[i-1]) //Enforces UNIQUE constraint (per position)
            {
                continue; //we can only try unique values at 1 place! (and note candidates is sorted!)
                // 13-06-26 Update: 
                // If we are looking at a duplicate number, AND it's not the first time 
                // we are picking a number in this specific recursive loop, skip it!
                // The case of none of the duplicates being picked is handled by just picking the next number not the same as this one.
            }

            if(curSum + candidates[i] > target)
                return; //since we sorted candidates, any other non-duplicate OR duplicate values would cause us to overshoot. 
            
            cur.Add(candidates[i]);
            Backtracking_Optimal_Retry1(candidates[(i+1)..], target, candidates[i]+curSum, cur, results);
            // (i+1) => Enforces UNUSED constraint.
            
            // Why do we generally do (i+1) in such backtracking?  
            // 1. Like we avoid duplicates like in 3 Sum!
            //(we have tried all combinations of things that had all letters before (i+1) at this place thus far)
            // 2. Also, we have already tried all elements containing the candidates before current Span's start.
            cur.RemoveAt(cur.Count-1);
        }
        //Finished in 10 minutes! (with the notes!)
        // But also, like, I did it like 4 hours ago too 
        // though back then I didn't have the Eureka moment of the decision! 
    }

    // TC: n*2^N (WHY?)
    // Aux. SC (cost of algorithm): O(N)
    // Total SC (cost of the solution): O(N*2^N) //all possible ways to pick anything from there in results. with max length of each individual set being N
    public void Backtracking_Optimal(Span<int> candidates, int target, int curSum, List<int> cur, List<List<int>> results)
    {
        //Had to peek at NeetCode Optimal solution sometimes to get to this quickly and to understand it. 
        // Took 14 minutes overall with TC and SC analysis.

        if(curSum == target)
        {
            results.Add(new(cur));
            return;
        }

        if(curSum > target || candidates.Length == 0)
        {
            return;
        }

        for(int i = 0; i<candidates.Length; i++)
        {
            if(i > 0 && candidates[i] == candidates[i-1])
                continue; //Only pick first non-added duplicate (the ones before must have been added already)!
            if(curSum + candidates[i] > target)
                return;
            cur.Add(candidates[i]);
            Backtracking_Optimal(candidates[(i+1)..], target, curSum + candidates[i], cur, results);
            cur.RemoveAt(cur.Count-1);
        }

        return;
    }

    public void Backtracking(Span<int> candidates, int target, int curSum, List<int> cur, List<List<int>> results)
    {
        //Unique combinations => same sequence cannot repeat! 
        // candidate at SAME index may NOT repeat either!
        // duplicate value at DIFFERENT index CAN be included
        
        if(curSum == target)
        {
            results.Add(new(cur));
            return;
        }
        if(curSum > target || candidates.Length == 0) //not really needed since the loop condition handles it, but keeping anyway!
            return;
        
        //Which to pick next?
        for(int i=0; i<candidates.Length; i++)
        {
            //For situations where we don't include one duplicate, we must not include any other duplicate!
            
            // How many of current duplicates should we include?
            var nextNewIdx = i;
            while(nextNewIdx < candidates.Length && candidates[nextNewIdx] == candidates[i])
            {
                // if(curSum >= target) //we need the next new idx, can just add the cur and cursum logic in ifs to do what I wanted, but that requires other changes in below loop, so I'll do that later!
                //     break;
                cur.Add(candidates[i]);
                curSum += candidates[i];
                nextNewIdx++;
            }

            for(int j = i; j < nextNewIdx; j++)
            {
                if(curSum <= target) //included == so our base cases can handle adding solution!
                {
                    Backtracking(candidates[nextNewIdx..], target, curSum, cur, results);
                }
                cur.RemoveAt(cur.Count-1);
                curSum-=candidates[i];
            }
            
            i = nextNewIdx-1; // loop will increment it back!
        }
    }
}


// Last Actual Solution: From 5th November 2024 3:37 AM CET
// public class Solution {
//     public List<List<int>> CombinationSum2(int[] candidates, int target) {
//         //https://algo.monster/flowchart

//         //CHECK THE HASHMAP AND OPTIMAL VERSIONS ON NEETCODEIO (written ones)?????
//         //WATCH THE NEETCODE VIDEO !!!!
//         return backtrack1_PickOrNot(candidates, target);
//     }


//     //TC: O(n*2^n) (length of subsets *  number of subsets) [we remove duplicates so it is not exactly 2^n subsets if there are duplicates]
//     //SC: O(n) //subset
//     public List<List<int>> backtrack1_PickOrNot(int[] candidates, int target) 
//     {
//         // FOR [2, 2] WE CAN HAVE SUBSETS [2, 2] (where each 2 is from different index) and [2]. Notice there's only ONE [2]. 
//         //i.e. A subset may contain the duplicate values more than once, but each subset only appears once so ([2] and [2] are considered the same even if they're 2 from different indices).       
        
//         //General note: 
//         //We CAN (not must) use the binary decision tree when there's NO possibility of NOT picking for each element.
//         //The n-nary decision tree (via iteration) HELPS when the elements MUST be picked (e.g. Permutations) (or we need to stop as soon as we can't pick up an element from all or unused-only options(like in "Combination Sum").

//         List<List<int>> res = new();
//         Array.Sort(candidates);

//         backtrack1_PickOrNotHelper(
//             candidates,
//             idx: 0,
//             target,
//             curSum: 0,
//             subset: new(candidates.Length),
//             res
//         );
//         return res;
//     }

//     public void backtrack1_PickOrNotHelper(int[] candidates, int idx,int target, int curSum, List<int> subset, List<List<int>> res)
//     {
//         if(target == curSum)
//         {
//             res.Add(subset.ToList());
//             return;
//         }
//         if(curSum>target || idx == candidates.Length) //curSum>target => we can't get desired target because all elements >=1 according to constraints.
//             return;

//         //Case 1: Pick this
//         subset.Add(candidates[idx]);
//         backtrack1_PickOrNotHelper(candidates, idx+1, target, curSum+candidates[idx], subset, res);
//         subset.RemoveAt(subset.Count-1);

//         //**IMPORTANT PART:** SKIP ALL FUTURE INSTANCES OF THIS DIGIT (because the backtrack1Helper recursive call above will deal with those, 
//         // we simply use the next case to when NONE of this digit (from ANY index after current index) is used)
//         //This loop in conjunction with recursion helps do the following:
//         //* Assume we start with array [1, 2, 2, 3]
//         //* First, above case considers L: [1], LL: [1,2], LLL: [1,2,2], LLLL: [1,2,2,3]
//         //* The loop below makes it so that for each of the above levels/nodes, respectively, we consider the following cases after not including current element:
//         //* i.e. 
//         // R: [2], LR: [1, 3], LRL: [1,3], LRR: [1], .......
//         // LLR:[1, 2, 3], LLRL: ..., LLRR: ...
//         // LLLR: [1, 1, 1, 2], ......
//         //(watch Neetcode video for full chart)
//         // RRRR: [] //Here, idx+1 == nums.Length (because last element), so the loop is skipped, and upon further recursion, we get our empty set back.
//         while((idx+1)<candidates.Length && candidates[idx]==candidates[(idx+1)])
//             idx++;

//         //Case 2: Don't pick this (i.e. try the idx after this element finishes repeating)
//         backtrack1_PickOrNotHelper(candidates, idx+1, target, curSum, subset, res);
        
//         return;
//     }
// }

