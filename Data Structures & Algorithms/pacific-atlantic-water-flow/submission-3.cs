public class Solution {
    IPacificAtlanticWaterflowSolver solver;
    public List<List<int>> PacificAtlantic(int[][] heights) {
        solver = new


        // # Solution from 4th April 2026, 4:00 AM CET:
        NuAttempt1_Bfs 

        // # Last Actual Solution: (late 2024)
        
        //IMPORTANT:
        // - Read comments.
        // - Had to watch GregHogg's short about this
        // - Also skimmed through some (NOT-implementation-related) parts of its Neetcode video
        // - Sneaked a few peeks at the solution on neetcodeio for a general idea on how to implement
        // - [EXTRA IMPORTANT] the idea of one dfs function for both of the oceans did not come to me until I saw it in the given solutions (on neetcodeio), but I might have come up with it? Maybe?
        // - could've done BFS (in fact, check BFS solution on Neetcodeio (the Python version cuz C# version doesn't use HashSet but a weird bool m*n matrix, but sets might be marginally better in space because they only have what can be reached from the ocean that set belongs to))
        // - [EXTRA IMPORTANT] TAKE A LOOK AT THIS AGAIN AND SKIM THROUGH IT AT LEAST
        // - [EXTRA IMPORTANT] The bool[] m*n solution on neetcode works because we can mark every node we reach as visited and so it can serve purposes of both tracking what was visited and all places we can reach
        //      + [EXTRA IMPORTANT] We can't discard nodes we visit from current node but water can't flow from there because a path without current node/cell might also exist.
        // - Had to check soln on neetcodeio to see previous value was provided as an int from the argument
        // - Took me like 40 minutes even with all the above help (was my first time), so might need to practice? (or maybe I can already do it faster?)
        // - Maybe you should watch the full neetcode video now?
        // old: // solver = new DfsAttempt1();

        // DfsAttempt1


        ();
        return solver.Solve(heights);
    }
}

public interface IPacificAtlanticWaterflowSolver
{
    List<List<int>> Solve(int[][] heights);
}

// # Last Actual Solution: (late 2024)

public class DfsAttempt1 : IPacificAtlanticWaterflowSolver
{
    HashSet<(int r, int c)> pacificSet;
    HashSet<(int r, int c)> atlanticSet;

    public List<List<int>> Solve(int[][] heights)
    {
        pacificSet = new();
        atlanticSet = new();

        //1. DFS from the each tile in the horizontal rows with direct connection to Pacific ocean OR Atlantic ocean to get all tiles FROM which water can get here (the tile where we run dfs from)
        for(int c = 0; c < heights[0].Length; c++)
        {
            dfs(0, c, heights, pacificSet, int.MinValue);
            dfs(heights.Length-1, c, heights, atlanticSet, int.MinValue);
        }

        //2. DFS from the each tile in the Vertical columns with direct connection to Pacific ocean OR Atlantic ocean to get all tiles FROM which water can get here (the tile where we run dfs from)
        for(int r=0; r<heights.Length;r++)
        {
            dfs(r, 0, heights, pacificSet, int.MinValue);
            dfs(r, heights[0].Length-1, heights, atlanticSet, int.MinValue);
        }

        //3. Find all the ones that are common for both and return them as result
        List<List<int>> result = new();
        foreach(var rc in pacificSet)
        {
            if(atlanticSet.Contains(rc))
                result.Add(new List<int>{rc.r, rc.c});
        }

        return result;
    }

    private void dfs(int r, int c, int[][]heights, HashSet<(int r, int c)> oceanSet, int prevVal) //the idea of one dfs function for both of the oceans did not come to me until I saw it in the given solutions (on neetcodeio), but I might have come up with it? Maybe?
    {
        var rc = (r,c);
        if( r<0 || c<0 || r >= heights.Length || c >= heights[0].Length || oceanSet.Contains(rc))
            return;

        if(heights[r][c] >= prevVal) //Water can flow!!
        {
            oceanSet.Add(rc); //we only add it here because the water might be able to get to others from a different path without going through the node with prevVal
            dfs(r-1, c, heights, oceanSet, heights[r][c]);
            dfs(r+1, c, heights, oceanSet, heights[r][c]);
            dfs(r, c-1, heights, oceanSet, heights[r][c]);
            dfs(r, c+1, heights, oceanSet, heights[r][c]);
        }
    }
}




// # Solution from 4th April 2026, 4:00 AM CET!


// THIS IS KIND OF A LOG OF MY THOUGHT PROCESS. HOW WOULD I ADD IT TO MY COMMENTS IN MY SOLUTION? [conversation with AI to come up with this, but AI was under strict instructions to not spoil the problem! Just validatign my approach or the smalles of nudges (told it to be realistically Goolge interview esque hints-wise, and to be on more conservative side)!]

// Took me 10 minutes to come up with the approach, and it is 1.5 years after doing it once or twice back then with and that was after watching a few shorts on how to actually solve it back then. Today, I know why I do things and didn't need the short for much more than verifying what I am suggesting is correct.



// wait nevermind, I hadn't read the question properly. In most cases like these, it's better to start from destination to reduce timecomplexity (instead of checking where each cell can be filled from, we find where water can flow to each edge from. Notice that if we start from each cell, we would be doing a bfs that would go through a lot of adjacent cells, meaning we're doing repeated work when we can just do the simpler version.)

// --

// //I TRIED SOME WRONG SUGGESTIONS APPROACH HERE (look below)

// --

// actually, the simplest solution is to do 2 bfs, 1 each for pacific and atlantic

// --

// Time complexity would be 2*O(M*N) space complexity would be O(M*N) (because we need to store the cell coordinates that would reach one of them first, then do a "union" of it with the other, so we need a hashset)

// --

// we're storing coordinates. A cell should be added as long as height is greater than or equal to the prior cell

// if I store coordinates from pacific in hashset, then all I need to do is to put coordinates we can reach in atlantic and are in hashset to our output list

// I will also be using value tuples (int r, int c) to facilitate easier handling and hashing!

// ----------- WRONG SUGGESTIONS: -------
// Also, since we don't allow diagonal movement, we can ignore the top-left, top-right, bottom-left, and bottom-right corner cell
// --
// No, you are right about that, BUT, we don't have to add them to the queue initially (since it is a multi-source BFS that I'm looking to do). Actually, perhaps it is better to add them just to simplify some of our internal logic!
// --
// Or perhaps, would it be better to start from the diagonal and see where water can flow from there (and any subsequent cell ) to?// --
// actually, the simplest solution is to do 2 bfs, 1 each for pacific and atlantic

public class NuAttempt1_Bfs : IPacificAtlanticWaterflowSolver {
    // ### Time taken:
    // IMPORTANT NOTE: 
    //      I was very sleepy and tired when I coded this. It's a miracle I coded this up. 
    //      Thankfully, I had designed the algorithm exactly yesterday already (my conversation with AI as not a help but as a pseudo interviewer, instructed to be vague and conservative with any hints, and to not give me the solution)
    //      AND IT PASSED ALL TESTS ON FIRST TRY (after fixing compile/syntax errors)
    // Code: Like around 32 minutes, but no outside help for code, except for some syntax stuff (and some dumb mistakes)
    // Design (yesterday): (The AI chat mentioned above) around 8 minutes max, I think.
    // Complexities: 2.5 minutes.

    // *IMPORTANT* THIS TIME TOO, YET AGAIN, I DIDN'T REMEMBER THAT WE NEED TO DO 2 SEPARATE BFS's FOR EACH OCEAN UNTIL I WAS BOUNCING IDEAS WITH THE 
    // AI PSEUDO INTERVIEWER AND GOT NUDGES FROM THERE (knowing what I was thinking wasn't quite right and and so on, the conversation is listed above, 
    // though I moved the wrong assumption part to the last part of that "log".)

    // PRACTICE AGAIN???

    readonly (int dr, int dc)[] dirs = [(-1,0), (1,0), (0, -1), (0, 1)];

    // Complexities:
    // TC: O(R*C)
    // Aux. SC: O(R*C)
    public List<List<int>> Solve(int[][] heights) {
        if(heights == null ||  heights.Length == 0 || heights[0] == null || heights[0].Length == 0)
            return [];

        Queue<(int r, int c)> pacificQ = new();
        Queue<(int r, int c)> atlanticQ = new();
        for(int c = 0; c < heights[0].Length; c++) //TC: O(C)
        {
            pacificQ.Enqueue((0, c));
            atlanticQ.Enqueue((heights.Length-1, c));
        }

        for(int r = 0; r < heights.Length; r++) //TC: O(R)
        {
            //Now that I wrote the BFS functions, I guess these if conditions weren't needed because the hashsets for visited would take care of it!
            if(r != 0)
                pacificQ.Enqueue((r, 0));
            if(r != heights.Length-1)
                atlanticQ.Enqueue((r,heights[0].Length-1));
        }

        //TC: O(R*C + R*C) = O(R*C)
        HashSet<(int r, int c)> pacificSet = GetReachableCells(heights, pacificQ);
        HashSet<(int r, int c)> atlanticSet = GetReachableCells(heights, atlanticQ); //tbf we could do it without a second set (or requiring separate intersection loop), but this approach is just.. cleaner?

        List<List<int>> intersection = new();
        var smaller = pacificSet;
        var bigger = atlanticSet;
        if(smaller.Count > bigger.Count)
            (smaller, bigger) = (bigger, smaller);

        //TC: O(R*C)
        foreach(var cell in smaller)
        {
            if(bigger.Contains(cell))
                intersection.Add([cell.r, cell.c]);
        }        
        return intersection;
    }

    // HashSet<(int r, int c)> GetPacificReachingCellsBfs(int[][] heights, Queue<(int r, int c)> pacificQ)
    HashSet<(int r, int c)> GetReachableCells(int[][] heights, Queue<(int r, int c)> oceanQ)
    {
        //multisource BFS
        HashSet<(int r, int c)> set = new();
        while(oceanQ.Count > 0)
        {
            var cur = oceanQ.Dequeue();
            set.Add(cur); //Has to be here for cycle checks! :)

            foreach(var (dr, dc) in dirs)
            {
                int nr = cur.r - dr;
                int nc = cur.c - dc;
                var next = (nr, nc);
                // Check: Bounds, Validity, and Cycles!
                if(nr < 0 || nr >= heights.Length || nc < 0 || nc >= heights[0].Length ||
                     heights[cur.r][cur.c] > heights[nr][nc] || set.Contains(next))
                    continue;

                oceanQ.Enqueue(next);
            }
        }

        return set;
    }
}
