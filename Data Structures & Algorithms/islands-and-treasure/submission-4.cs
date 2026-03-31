public class Solution {
    public void islandsAndTreasure(int[][] grid) {
        IIslandsAndTreasureSolver soln = new
            // Attempt1
            NuAttempt1
        ();

        soln.islandsAndTreasure(grid);
        return;
    }
}

public interface IIslandsAndTreasureSolver {
    void islandsAndTreasure(int[][] grid);
}



// # Last Actual Solution (late November 2024):

public class Attempt1 : IIslandsAndTreasureSolver {
    const int INF = 2147483647;

    public void islandsAndTreasure(int[][] grid) {
        //GO THROUGH THIS!!! (multi-source BFS is kinda fkn nifty!!)
        //Greg Hogg's YouTube Short (58secs): https://www.youtube.com/watch?v=eObg8suJOBU
        //Maybe use this? https://medium.com/geekculture/multisource-bfs-for-your-faang-coding-interviews-d5177753f507
        
        multisourceBfs1(grid);
    }

    public void multisourceBfs1(int[][] grid)
    {
        Queue<(int r, int c)> q = new();
        
        //// Add the multiple sources from where we start our bfs:
        for(int r=0; r<grid.Length; r++)
        {
            for(int c=0; c<grid[0].Length; c++)
            {
                if(grid[r][c]==0)
                    q.Enqueue((r,c));
            }
        }

        //// Start BFS:
        // int curDistFromGates = 1; //Due to how bfs works and since we put all treasures in the queue already and are starting bfs from there, each neighbor is exactly the same distance away from each gate. //Each element in the queue is at the same level
        while(q.Count>0)
        {
            var rc = q.Dequeue();
            msBfsHelper(rc.r-1, rc.c, rc, grid, q);
            msBfsHelper(rc.r+1, rc.c, rc, grid, q);
            msBfsHelper(rc.r, rc.c-1, rc, grid, q);
            msBfsHelper(rc.r, rc.c+1, rc, grid, q);
        }
    }

    public void msBfsHelper(int r, int c, (int r, int c) prevRC, int[][] grid, Queue<(int r, int c)> q)
    {
        //prevRC is the node from which we got to current node/neighbor.
        if(r<0||c<0||r>=grid.Length||c>=grid[0].Length||grid[r][c]!=INF) //grid[0] to avoid cache misses?
            return;
        
        q.Enqueue((r,c));

        grid[r][c] = 1+grid[prevRC.r][prevRC.c]; //Each tile is 1 away from the previous one. (We can start from treasure chests because they are marked by 0)
    }
}



// # Soln. from 1st April 2024:

// Note: From the start it felt like it would be better to make BFS a function but I hadn't done or even studied about multi-source BFS in 1.5 years
//       so I wanted to avoid too much complication (e.g. what variables to pass, etc.)

// ## Time:
// Took 18 minutes for code with the first pass bugs I mentioned
// ~3 Minutes to fix them. (though NeetCode compiler and tests did help)
// ~2 Minutes for TC and SC

// ## Complexities:
// TC: O(M*N + M*N) =O(M*N)         // I was stupid and did N^2 earlir until I checked NeetCode complexity 
// SC: O(M*N) [worst case: when all cells are treasures]

// ## Code:

// using Point = (int row, int col);
public class NuAttempt1 : IIslandsAndTreasureSolver {
    const int Water = -1; //wall
    const int Treasure = 0;
    const int Land = 2147483647;
    readonly (int dr, int dc)[] dir = [(-1,0), (1,0), (0, -1), (0,1)]; //Up, Down, Left, Right
    //^ Was missing [] on type earlier :O
    
    public void islandsAndTreasure(int[][] grid) {
        // Multi Source BFS             // (I kinda just... knew that this was to be used???)

        Queue<(int row, int col)> q = new();

        // 1. Add all the multiple starting points:
        for(int r=0; r < grid.Length; r++)
        {
            for(int c=0; c<grid[0].Length; c++) //non jagged!
            {
                if(grid[r][c] != Treasure) //almost did Island but I remember from 1.5 years ago its better to start from destination, most of, if not all, times.
                    continue;
                q.Enqueue((r,c));
            }
        }

        // 2. Do Level Order BFS
        var dist = 1;
        while(q.Count > 0) //note that here !=Land acts as our "cycle detection"!!
        {
            int levelLength = q.Count;
            for(int i=0; i<levelLength; i++)
            {
                var (curR, curC) = q.Dequeue();
                // var curCell = grid[curR][curC];
                // if(curCell == Water) continue; //nvm, let's do it while adding to avoid problems!
                
                // if(grid[curR][curC] == Land) //don't want to overwrite treasure (which we add in base case!)
                //^ or we can also assume currently added is already changed (or doesn't need change) and do it that way below too!

                foreach(var (dr, dc) in dir)
                {
                    int nr = curR + dr;
                    int nc = curC + dc;
                    if(nr < 0 || nr >= grid.Length || 
                        nc < 0 || nc >= grid[0].Length || grid[nr][nc] != Land) //assuming no jagged arrays
                    {
                        continue;
                    }
                    grid[nr][nc] = dist;
                    q.Enqueue((nr,nc)); //I FORGOT TO ADD THIS! :'( Only caught this because NeetCode's automated test run failed :'(.
                }
            }

            dist++;
        }


        // return grid; //IDK why I was trying to return grid :O (caught by NeetCode run, of course. But tbf, not that big of a deal.)
    }
}
