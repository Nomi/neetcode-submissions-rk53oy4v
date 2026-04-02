public class Solution {
    public int OrangesRotting(int[][] grid) {
        IRottingOranges soln = new
            // Attempt1
            NuAttempt1
        ();

        return soln.OrangesRotting(grid);
    }
}

public interface IRottingOranges {
    int OrangesRotting(int[][] grid);
}


// # Last Actual Solution (Late November 2024):
public class Attempt1 : IRottingOranges {
    public int OrangesRotting(int[][] grid) {
        //READ COMMENTS!!
        //very optional bonus note: only if you want to, check the BFS (No Queue) solution BUT keep in mind its Time Complexity is HORRENDOUS (O((r*c)^2)) even though its space complexity is O(1) since it just uses the original array. //Given our constraints, it either doesn't matter or the normal queue based bfs might be a better, balanced approach.
        return msbfs1(grid);
    }

    const int empty = 0;
    const int fresh = 1;
    const int rotten = 2;

    //TC: O(r*c)
    //SC: O(r*c) //worst case being when every cell is a rotten fruit.
    public int msbfs1(int[][]grid)
    {
        //IMPOTTANT NOTES:
        // - Coming up with the freshFruitCount was(or seems like) a happy accident.
        // - (Edge Case:??) I FORGOT TO READ THE QUESTION AGAIN AND DIDN'T REALIZE I NEEDED TO RETURN A -1
        // - I NEED TO MAKE SURE TO NOT JUST ASSUME STUFF IN THE INTERVIEWS (WHERE THIS KIND OF STUFF ISN'T EVEN LISTED, BUT YOU'RE REQUIRED TO ASK OR STATE YOUR ASSUMPTIONS)
        // - EDGE CASE: FORGOT ABOUT THE EDGE CASE WHERE THERE ARE ALREADY NO FRESH FRUITS UNTIL FAILED A TEST CASE!!! NEED TO HANDLE THAT (OR SHOULD JUST MOVE THE -2 FROM THE FINAL RETURN TO WHEN ASSIGNING maxSeconds EACH TIME AND INITIALIZE maxSeconds TO 0)
        // - (EDGE CASE:??) TURNS OUT THE GRID DOESN'T NEED TO BE n*n IN SIZE (not neccessarily a square grid). (found this out after exception in a test case)

        int maxSeconds = 0;
        int freshFruitCount = 0;
        Queue<(int r, int c)> q = new();

        //Add the starting points (sources)
        for(int r=0; r<grid.Length; r++) //TC: O(r*c)
        {
            for(int c=0; c<grid[0].Length; c++)
            {
                if(grid[r][c]==rotten)
                    q.Enqueue((r,c));
                else if(grid[r][c]==fresh)
                    freshFruitCount++;
            }
        }
        if(freshFruitCount==0)
            return 0;

        //BFS
        while(q.Count>0) //TC: O(r*c)
        {
            (int r, int c) = q.Dequeue();
            handleCurrent1(r-1, c, r, c, ref maxSeconds, ref freshFruitCount, grid, q);
            handleCurrent1(r+1, c, r, c, ref maxSeconds, ref freshFruitCount, grid, q);
            handleCurrent1(r, c-1, r, c, ref maxSeconds, ref freshFruitCount, grid, q);
            handleCurrent1(r, c+1, r, c, ref maxSeconds, ref freshFruitCount, grid, q);
        }
        if(freshFruitCount!=0) //check handleCurrent1 comments.
            return -1;
        return maxSeconds-2; //-2 to remove the initial 2 we add from the rotten fruit start.
    }
    public void handleCurrent1(int r, int c, int prevR, int prevC, ref int maxSeconds, ref int freshFruitCount, int[][] grid, Queue<(int r, int c)> q)
    {
        if(r<0 || c<0 || r>=grid.Length || c>=grid[0].Length || grid[r][c]!=fresh)
            return;
        freshFruitCount--; //due to the condition, each fresh fruit will be processed only once and that too by its nearest rotten fruit (or whichever ends up in being first in the queue if there are multiple same distance away).
        grid[r][c] += grid[prevR][prevC]; //think about it. it works out to place number of minutes required for the rot to reach current place.
        if(grid[r][c]>maxSeconds) maxSeconds = grid[r][c];
        q.Enqueue((r,c));
    }
}


// # Solution from 02-04(April)-2024:

// using Point = (int r, int c); //Would normally create a class, but in interest of time, a named tuple serves better.
                              // Also, if we needed hashing, a value tuple is directly hashable (instead of reference in classes!) and easier to write than records!
// using doesn't work in neetcode, I'll switch all `Point`s to plain `(int r, int c)` I guess.

public class NuAttempt1 : IRottingOranges { //TC = O(M*N + M*N) = O(2*M*N) =  O(M*N) | Aux. SC = O(M*N)

    // SOME POTENTIAL DRY RUN STRATEGIES: 
    // Focus on minimal matrices (1x1, 1x2, 2x2) to validate 
    // loop termination, empty/fresh starts, and basic propagation.
    //
    // 1. Single Cell (Edge Cases):
    //    - [[0]]: Empty grid -> Expect 0
    //    - [[1]]: Fresh only -> Expect -1
    //    - [[2]]: Rotten only -> Expect 0 (Catches the +1 off-by-one bug)
    //
    // 2. Minimal Interaction:
    //    - [[2, 1]]: Simple propagation -> Expect 1
    //    - [[2, 0]]: Blocked path -> Expect 0
    //
    // 3. Simple Chain/Branching:
    //    - [[2, 1], [1, 0]]: L-shape propagation -> Expect 2
    //    - [[1, 2], [0, 1]]: Disconnected fresh orange -> Expect -1
    
    /* *IMPORTANT* [AI generated note tho]
    * RECOMMENDED MINIMAL TEST SUITE: 
    * Prioritize these three cases to catch nearly all common BFS logic bugs:
    *
    * 1. [[2]] (The Single Rotten)
    * - Why: The ultimate "off-by-one" killer. If the code returns 1 instead of 0 
    * due to an extra "ghost" BFS cycle with an empty queue, this catches it immediately.
    *
    * 2. [[1, 2]] (The Successful Step)
    * - Why: Validates that the 'dirs' array and bounds checking work, and that 
    * fresh/infected counts are updating correctly during a standard transition.
    *
    * 3. [[1, 2], [0, 1]] (The Obstacle/Failure)
    * - Why: 
    * - Pathfinding: Does the BFS respect the '0' (wall/empty) boundary?
    * - Integrity: Does the final return logic correctly identify that 
    * at least one orange remained uninfected?
    */
    
    const int Empty = 1; //blocker, wall, firewall, etc. for similar problems!
    const int Uninfected = 1; //here, fresh
    const int Infected = 2; //here, rotten
    const int FailedToInfectAll = -1;
    readonly (int r, int c)[] dirs = [(-1,0), (1,0), (0,-1), (0,1)];

    public int OrangesRotting(int[][] grid) {
        //Assuming in place modification allowed

        Queue<(int r, int c)> q = new(); //same pronounciation, easier typing XD
        int uninfectedCount = 0;
        for(int r = 0; r < grid.Length; r++)
        {
            for(int c = 0; c<grid[0].Length; c++) //Due to [0], no jagged array support from this point on! (won't comment it again anywhere else to save time)
            {
                if(grid[r][c] == Infected)
                    q.Enqueue((r,c));
                else if (grid[r][c] == Uninfected)
                    uninfectedCount++;
            }
        }

        return SpreadInfectionBfs(q, uninfectedCount, grid);
    }

    public int SpreadInfectionBfs(Queue<(int r, int c)> q, int uninfectedCount, int[][] grid) // returns Time taken to infect all, OR `FailedToInfectAll` if cannot infect all!
    {
        //here at 10 minutes in!

        // Multi-Source BFS (level-order):
        int infectedCount = 0; //Post-submission notes (not included in time): only the new uninfected that were previously uninfected. Maybe `NewInfections` would be better name-wise? Or just track `remaining = uninfectedCount` or just modify the `uninfectedCount` itself to track remaining
        int time = 0; //assuming no overflow
        while(q.Count > 0 && infectedCount != uninfectedCount)
        {
            // So, it took a long while to fix the issue with failing test case on NeetCode automatic run. Not included in time completed!

            // *IMPORTANT*:
            // BUG: Off-by-one error in time calculation (+1)
            //
            // Root Cause: Since we mark children as infected upon enqueueing, the final BFS level
            // only dequeues the last infected batch to check for the children's uninfected neighbors, finding none.
            // If we increment time unconditionally, we overcount this final empty traversal by 1.
            // 
            // FIX: Add `infectedCount < uninfectedCount` to the while loop condition 
            // to exit early once all infectable nodes are processed. Also acts as optimization!
            //
            // DRY RUN SUGGESTION: Even the simplest case of [[2]] (only one element and it's infected) would have helped catch it!

            time++;
            var levelLength = q.Count;
            for(int i=0; i < levelLength; i++)
            {
                var (prevR, prevC) = q.Dequeue();
                
                foreach(var (dr,dc) in dirs)
                {
                    int nr = prevR + dr; //assuming no overflow
                    int nc = prevC + dc; //assuming no overflow


                    // Check Bounds, Validity, and Cycle:
                    if(nr < 0 || nr >= grid.Length 
                        || nc < 0 || nc >= grid[0].Length || grid[nr][nc] != Uninfected)
                        continue;
                    
                    grid[nr][nc] = Infected; //had == Infected :O
                    q.Enqueue((nr, nc));
                    infectedCount++;    //Forgot to do this initially until a NeetCode automatic test run failed and I checked this!
                }
            }
        }

        return uninfectedCount == infectedCount ? time : FailedToInfectAll;
         // Here at 20mins 9 secs in except for the part about forgetting infectedCount++ 
         // and the failing test case (1 extra in result), dry run attempt for it (I gave up), asking AI for hint, and its fix.
         // The problem was an off-by-one (+1) in time output, and is described in detail above,
    }
    // # Dry Running because of failing Example:
    // ## Starting state: (for SpreadInfectionBfs)
    //  - grid=[
    //                        [1,1,0],
    //                        [0,1,2]]
    // - uninfectedCount = 3 (dry run just quickly running through it for this part)
    // -  q = [(1,1)];
    //
    // - time = 0
    // - infectedCount = 0
    // ## S1: (While)
    //  time 0 -> 1
    //  levelLength = 1
    // prevR =1, prevC= 1.
    // inner for loop state changes: nr = -1

    // # Took 7 minutes so far (for dry run) but I give up, this is too complicated. I started with the actual failing example first ([[1,1,0],[0,1,1],[0,1,2]]),
    // but that was too big. Then, cut row, while keeping it still somewhat complex (the 2 L bends), but this is too many nested loops to dry run while state tracking so much!

    // # WOWWW I JUST TOOK A LOOK AT MY FUNCTION AGAIN AND REALIZED WHAT THE PROBLEM WAS:
    // EVEN A SIMPLE DRY RUN CASE OF NO UNINFECTED WOULD HELP FIND IT!
    // -> The problem is that I INCREMENT TIME EVEN IF WE HAVE NOTHING LEFT TO INCREMENT!
    // SINCE WE MARK AS VISITED/INFECTED CHILDREN BEFORE WE GET TO A THEM TO SEE IF THEIR CHILDREN SHOULD BE INFECTED, 
    // OUR LAST LEVEL (while loop iteration) IS JUST WHEN WE FIND THAT ALL CHILDREN ARE UNINFECTABLE (already infected or empty)
    // BUT WE INCREMENT TIME ON THAT LEVEL ALREADY, YET IT WAS ALREADY COMPLETELY INFECTED!
    // SO, THE SIMPLEST FIX IS: add infectedCount != uninfectedCount to while loop condition!
    // WE INCREMENT IT POINTLESSLY (ALWAYS BY 1)!!!
}
