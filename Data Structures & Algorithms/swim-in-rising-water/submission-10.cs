public class Solution {
    public int SwimInWater(int[][] grid) {

        // # Solution from 27-04(Apr)-2026:

        return (new NuAttempt1_Dijkstra()).SwimInWater(grid); 


        // # Last Actual Solution: [from 30-11(Nov)-2024]

        //[IMPORTANT!!!] JUST WATCHED THE NEETCODE VIDEO AND:
        //  - We can ignore the t in the question from the perspective of using it to calculate anything, it is just something used to explain the question.
        //  - INSTEAD, WE WILL SOLVE `WHAT IS THE MINIMUM AMOUNT OF TIME TO IT WOULD TAKE TO GET FROM START TO END`
        //  - ALLOWED TRAVEL: UP DOWN LEFT RIGHT (each of these could be considered edges with the weight being the elevation of the destination node) [FACT_0]
        //  - FROM [FACT_0], HERE WE WON'T USE ADJACENCY LIST LIKE WE USUALLY DO. IT IS EASY TO JUST GO THROUGH ALL THE EDGES FOR EACH NODE BECAUSE IT IS JUST 1 NODE UP OR DOWN OR LEFT OR RIGHT
        //  - WE WANT TO GO FROM GIVEN SOURCE (0,0) TO GIVEN DESTINATION (n-1, n-1). [FACT_1]
        //  - IN ORDER TO SWIM/TRAVEL FROM ONE NODE TO ANOTHER, WE NEED TO WAIT UNTIL t IS >= THAN ELEVATION/VALUE AT BOTH NODES (so there's water to swim),
        //  - SINCE WE CAN SWIM THROUGH ANY PATH IN 0 SECONDS, AS LONG AS ALL THE NODES IN THE PATH ARE <=t, WE CAN TRAVEL THROUGH IT IN 0 SECONDS
        //  - AS SUCH, THE BOTTLENECK IS THE MAXIMUM ELEVATION/VALUE OF ANY NODE IN THE PATH RATHER THAN THE SUM_OF_VALUES/DISTANCE OF WHOLE PATH. [FACT_2] 
        //      (This is because we can just wait at starting node for t = Elevation/Value of maximum node in the path, and then you can swim through the whole path in 0 seconds.)
        //  - From [FACT_1] IT FOLLOWS THAT WE DON'T WANT TO A MST (specific src and dest, and it doesn't matter if we travel all nodes (which could in fact only hurt)).
        //      SEEMS WE WANT SPT FROM SRC TO DEST. [ALPHA_1]
        //  - FROM [FACT_2] WE CAN NOTICE THAT WE DON'T USE THE NORMAL DIJKSTRA, BUT INSTEAD OF USING THE SUM OF THE WEIGHTS/ELEVATIONS IN PATH AS COST,
        //          WE USE THE MAXIMUM_ELEVATION OF THE DEST NODE FOR EACH EDGE IN THE PATH AS THE COST. [FACT_3]
        //  - [ALPHA_2 (might not be 100% applicable here??)]-> From [FACT_2] WE CAN CONFRIM WE WANT SHORTEST PATH BECAUSE: Since we want to minimize waiting time, we want to minimize the largest elevation in the path locally. 
        //          We don't care about the globally smallest sum of values/elevations (like we would in MST), but we care about minimizng the (local) elevation FOR EVERY 
        //          node in the path. Even if the sum of their elevations ends up being bigger (because we only wait for the time of the maximum elevation, then swim through that path in 0 seconds).
        //  - For more, read up on my Dijkstra vs MST related comments in `Network Delay Time`
        //  - ACTUALLY, about [ALPHA_2], it might not be fully relevant here because we of the modification made in [FACT_3] AND only needing the path from SRC to DST without needing every node ([FACT_1] & [ALPHA_1]). For [ALPHA_2] I seemed to have been thinking more about using Dijkstra to find cost to go to every node from src.



        //  TL;DR / Quick Summary (not comprehensive):
        //  WE ARE FINDING A PATH WITH MINIMIZED MAX_HEIGHT_OF_ANY_OF_ITS_NODES 
        //  (=>MAX_HEIGHT_OF_ANY_OF_NODES_ON_PATH_TO_THIS_NODE AS COST) 
        //  (We use SPT/Dijkstra because: 1. WE GO FROM SRC TO DEST AND 2. DON'T NEED TO COVER ALL THE NODES) //NOTE THAT 1. OR 2. BY THEMSELVES ARE ENOUGH TO KNOW WE DON'T NEED MST. //1. IS ENOUGH TO KNOW WE NEED SPT.
        //  FROM [FACT_0], HERE WE WON'T USE ADJACENCY LIST LIKE WE USUALLY DO. IT IS EASY TO JUST GO THROUGH ALL THE EDGES FOR EACH NODE BECAUSE IT IS JUST 1 NODE UP OR DOWN OR LEFT OR RIGHT
        
        //  From NeetCode video: we will put in maxheap the following: (MAX_HEIGHT_OF_ANY_OF_NODES_ON_PATH_TO_THIS_NODE, NODE_ROW_IDX, NODE_COLUMN_IDX)
        


        // return Dijkstra1(grid);


    }

    int Dijkstra1(int[][] grid)
    {
        int N = grid.Length; // we know grid is NxN from question
        HashSet<(int r, int c)> visited = new();

        PriorityQueue<(int maxTimeNeededOnPath, int r, int c), int> pq = new();

        pq.Enqueue((grid[0][0], 0, 0), grid[0][0]); //Deprecated (because I switched my approach a little to match neetcodeio soln.): //First node we'll go to is the source node before which the max elevation is 0.
        while(pq.Count>0) //IN WORST CASE: Goes over all possible edges => TC: O(4*V*log2(V)) == OC(Vlog2(V))
        {
            var cur = pq.Dequeue(); //O(1)
            
            if(cur.r==N-1 && cur.c==N-1)
                return cur.maxTimeNeededOnPath;
            
            visited.Add((cur.r, cur.c));
            
            AddToPq(pq, cur, cur.r-1, cur.c, grid, visited);
            AddToPq(pq, cur, cur.r+1, cur.c, grid, visited);
            AddToPq(pq, cur, cur.r, cur.c-1, grid, visited);
            AddToPq(pq, cur, cur.r, cur.c+1, grid, visited);
        }
        
        throw new Exception("Couldn't find a solution, but should always have a solution by definition.");
    }

    void AddToPq(PriorityQueue<(int maxTimeNeededOnPath, int r, int c), int> pq, (int maxTimeNeededOnPath, int r, int c) cur, int nR, int nC, int[][] grid, HashSet<(int r, int c)> visited)
    {
        if(visited.Contains((nR, nC)) || nR <  0 || nR >= grid.Length || nC <  0 || nC >= grid.Length) //[Read full comment] ONLY THOUGHT ABOUT PUTTING THIS HERE INSTEAD OF ABOVE AND PASSING THE ACTUAL CURRENT MAX WEIGHT AT ALL NODES ONLY AFTER PEEKING AT NEETCODEIO SOLN ONCE AFTER MINE DIDN'T WORK
                return;
        int maxTimeHere = cur.maxTimeNeededOnPath > grid[nR][nC] ? cur.maxTimeNeededOnPath : grid[nR][nC];
        pq.Enqueue((maxTimeHere, nR, nC), maxTimeHere);
    }

    // void AddToPq(PriorityQueue<(int maxTimeNeededOnPath, int r, int c), int> pq, (int maxTimeNeededOnPath, int r, int c) cur, int nR, int nC, int[][] grid)
    // {
    //     int maxTimeHere = cur.maxTimeNeededOnPath > grid[cur.r][cur.c] ? cur.maxTimeNeededOnPath : grid[cur.r][cur.c];
    //     pq.Enqueue((maxTimeHere, nR, nC), cur.maxTimeNeededOnPath);
    // }
}


public class NuAttempt1_Dijkstra {
    //Finished in 27 minutes except for the r,c > 0 error [should've been >= 0 for both] I found due to failing nc.io test case!
    //Complexity analysis (TC and SC) took like 1-2 minutes.

    public int SwimInWater(int[][] grid) {
        return Dijkstra(grid);
    }

    record Cell(int r, int c);

    //TC = O(E*log(V))
    //Aux. SC = O(E)     [worst case == O(V^2) cuz E <= V*(V-1)]
    public int Dijkstra(int[][] grid) { //assuming no negative weights!
        int rLen = grid.Length, cLen = grid[0].Length;
        Cell dest = new(rLen-1, cLen-1); //assuming no single length array!

        HashSet<Cell> seen = new();
        PriorityQueue<(Cell cell, int timeSoFar), int> minHeap = new();

        minHeap.Enqueue((new(0,0), grid[0][0]), grid[0][0]);
        
        while(minHeap.Count > 0) {
            var cur = minHeap.Dequeue();
            
            if(!seen.Add(cur.cell)) //MUST ALWAYS BE HERE!
                continue;
            if(cur.cell == dest)
                return cur.timeSoFar;

            AddIfValid(minHeap, grid, cur.timeSoFar, cur.cell.r, cur.cell.c - 1, seen);            
            AddIfValid(minHeap, grid, cur.timeSoFar, cur.cell.r, cur.cell.c + 1, seen);            
            AddIfValid(minHeap, grid, cur.timeSoFar, cur.cell.r - 1, cur.cell.c, seen);            
            AddIfValid(minHeap, grid, cur.timeSoFar, cur.cell.r + 1, cur.cell.c, seen);            
        }

        throw new Exception("Unreachable");
    }

    void AddIfValid(PriorityQueue<(Cell cell, int timeSoFar), int> minHeap, int[][] grid, int timeSoFar, int r, int c, HashSet<Cell> seen) {
        if(r >= 0 && r < grid.Length && c >= 0 && c < grid[0].Length) { //*IMPORTANT*: MESSED UP EARLIER: < 0 INSTEAD OF >= 0 !!!
            Cell toAdd = new(r,c);
            if(seen.Contains(toAdd)) //we need the seen check above, this one is more of a micro optimization 
                return;
            var newTimeSoFar = Math.Max(timeSoFar, grid[r][c]);
            minHeap.Enqueue((toAdd, newTimeSoFar), newTimeSoFar); //you only ever need to wait as long as the bottleneck! (assuming speed of light XD)
        }
    }
}
