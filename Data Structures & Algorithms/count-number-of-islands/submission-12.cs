public class Solution {
    public int NumIslands(char[][] grid) {
        ICountIslands soln;

        soln = new
        // Attempt1
        NuAttempt1
        ();

        return soln.NumIslands(grid);
    }
}


// Interface for Solutions:
public interface ICountIslands {
    public int NumIslands(char[][] grid);
}



// # Last Actual Attempt (Soln. from Late 2024):
public class Attempt1 : ICountIslands {
    public int NumIslands(char[][] grid) {
        
        // return dfs1Wrapper(grid);
        return bfs1_MarkIslandVisited(grid);
        
    }

    ////////// DFS (+overwriting original array):
    public int dfs1Wrapper(char[][] grid)
    {
        //TC: O(ROWS*COLUMNS) SC: O(ROWS*COLUMNS)
        int numIslands = 0;
        for(int r = 0; r < grid.Length; r++)
        {
            for(int c = 0; c< grid[0].Length; c++)
            {
                if(grid[r][c] == '1')
                {
                    numIslands += 1;
                    dfs1_MarkIslandVisited(grid, r, c);
                }
            }
        }
        return numIslands;
    }
    char visited = '*'; //if not allowed to overwrite input array, we could use a seen (HashSet of tuples r,c),
    public void dfs1_MarkIslandVisited(char[][] grid, int r, int c) //marks connected parts of islands as visited.
    {
        if(r<0 || r>= grid.Length || c<0 || c>=grid[r].Length || grid[r][c] != '1')
            return;
        
        grid[r][c]=visited;

        dfs1_MarkIslandVisited(grid, r+1, c); //down
        dfs1_MarkIslandVisited(grid, r-1, c); //up
        dfs1_MarkIslandVisited(grid, r, c+1); //right
        dfs1_MarkIslandVisited(grid, r, c-1); //left
        return;
    }


    ////////// BFS (+ WITHOUT overwriting input array)
    HashSet<(int r, int c)> seen;
    public int bfs1_MarkIslandVisited(char[][] grid) //marks connected parts of islands as visited.
    {
        int numIslands = 0;
        seen = new();
        for(int r=0; r<grid.Length;r++)
        {
            for(int c=0; c<grid[0].Length;c++)
            {
                (int r, int c) rc = (r,c);
                // Console.WriteLine(seen.Contains(rc));
                if(grid[rc.r][rc.c]=='1' && !seen.Contains(rc))
                {
                    // Console.WriteLine(seen.Count);
                    numIslands++;
                    bfs1_Helper(grid, rc);
                }
            }
        }
        return numIslands;
    }

    public void bfs1_Helper(char[][] grid, (int r, int c) _rc)
    {
        Queue<(int r, int c)> q = new();
        //Earlier, I spent a while trying to fix it when i had simply just added the _rc to seen here and so it never did anything except add that to seen (check if condition inside loop)
        q.Enqueue(_rc);
        while(q.Count>0)
        {
            var rc = q.Dequeue();
            if(rc.r<0 || rc.r>= grid.Length || rc.c<0 || rc.c>=grid[rc.r].Length || grid[rc.r][rc.c] != '1' || seen.Contains(rc))
                continue;
            (int r, int c) = rc;
            // Console.WriteLine($"{_rc.r},{_rc.c} : {r},{c}");
            seen.Add((r, c));
            q.Enqueue((r-1, c));
            q.Enqueue((r+1, c));
            q.Enqueue((r, c-1));
            q.Enqueue((r, c+1));
        }
    }
}




// # Soln. from 29 March 2026:
// Took 10 minutes except the "small"-ish mistakes I had made.
public class NuAttempt1 : ICountIslands {
    const char Seen = '\0';
    const char Isle = '1'; //unused part of island

    public int NumIslands(char[][] grid) {
        //Assuming in place modifications are okay!! (permanent!)
        int islands = 0;
        for(int r = 0; r < grid.Length; r++)
        {
            for(int c = 0; c < grid[r].Length; c++)
            {
                if(grid[r][c] != Isle)
                    continue; //had return here instead of continue earlier, only caught on first compile run on NeetCode!
                islands++;
                // MarkIslandVisitedDfs(grid, r, c);
                BfsMarkIslandVisited(grid, r, c);
            }
        }
        
        return islands; //was still return 0 from my earlier test :O
    }

    public void MarkIslandVisitedDfs(char[][] grid, int curR, int curC) //Preorder DFS over the connected component (and its immediate neighbors)
    {
        if(curR < 0 || curR >= grid.Length 
            || curC < 0 || curC >=  grid[curR].Length || grid[curR][curC] != Isle) //Had curC >=  grid[r].Length (should've been curR not r), only caught on to first compile run on NC 
            return; // *IMPORTANT* CHECKED curR/C > ....Length INSTEAD OF >= YET AGAIN!! REALLY HOPING PRACTICE FIXES IT!
                    // Only caught on second compile on leetcode.
        
        grid[curR][curC] = Seen; //mark visited!

        MarkIslandVisitedDfs(grid, curR-1, curC); //Up
        MarkIslandVisitedDfs(grid, curR+1, curC); //Down
        MarkIslandVisitedDfs(grid, curR, curC-1); //Left
        MarkIslandVisitedDfs(grid, curR, curC+1); //Right

        return;
    }

    public void BfsMarkIslandVisited(char[][] grid, int r, int c)
    {
        Queue<(int r, int c)> queue = new();
        queue.Enqueue((r,c));
        while(queue.Count > 0) //only mistake was I was doing q.Count here, caught it with the first NeetCode compile of the BFS version
        {
            var cur = queue.Dequeue();
            
            if(grid[cur.r][cur.c] != Isle) //either already visited/seen OR water!
                continue;

            //Mark Visited:
            grid[cur.r][cur.c] = Seen;

            //Moved conditions here to save space:
            //Up:
            if(cur.r - 1 >= 0)        
                queue.Enqueue((cur.r-1, cur.c));
            //Down:
            if(cur.r+1 < grid.Length)
                queue.Enqueue((cur.r+1, cur.c)); 
            //Left:
            if(cur.c-1 >= 0)
                queue.Enqueue((cur.r, cur.c-1)); 
            //Right:
            if(cur.c+1 < grid[0].Length) //for jagged grid, we would use [cur.r] for cur.c check instead of 0!
                queue.Enqueue((cur.r, cur.c+1)); 
        }

        return;
    }
}

