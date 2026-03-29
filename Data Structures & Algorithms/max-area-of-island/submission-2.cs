public class Solution {
    public int MaxAreaOfIsland(int[][] grid) {
        return (new NuAttempt1()).MaxAreaOfIsland(grid);
        // return dfs1(grid);
    }

    // # Last Actual Solution (Late 2024):

    /////// DFS (NO INPUT ARRAY OVERWRITE)
    HashSet<(int r, int c)> seen;
    int dfs1(int[][] grid)
    {
        seen=new();

        int maxArea = 0;
        for(int r=0; r<grid.Length; r++)
        {
            for(int c=0; c<grid[0].Length; c++)
            {
                var rc = (r,c);
                if(grid[r][c]==1 && !seen.Contains(rc))
                {
                    var curArea = dfs1Helper(grid, rc);
                    if(maxArea < curArea)
                        maxArea = curArea;
                }
            }
        }
        return maxArea;
    }

    int dfs1Helper(int[][] grid, (int r, int c) rc)
    {
        if(rc.r<0||rc.r>=grid.Length||rc.c<0||rc.c>=grid[rc.r].Length||grid[rc.r][rc.c]==0 || seen.Contains(rc)) //keep forgetting to add 'rc.' before r and c here. Could try (int r, int c) = rc above this line for easy fix.
            return 0;

        int sum = 1;
        seen.Add(rc);

        sum += dfs1Helper(grid, (rc.r-1, rc.c)); //Up
        sum += dfs1Helper(grid, (rc.r+1, rc.c)); //Down
        sum += dfs1Helper(grid, (rc.r, rc.c-1)); //Left
        sum += dfs1Helper(grid, (rc.r, rc.c+1)); //Right

        return sum;
    }
}



// # Soln. from 29 March 2026 (11:18PM):
// Took me 18 minutes to get the base down with the bugs (while talking through my solution)
// Took me 4 minutes to try and fix it
// Then I asked AI and fixed the last 2 remaining errors.
// => So, a total of 25-26 minutes?
// I am really hoping I only made these errors because 1. I am fatigued & 2. I used BFS here just to practice even though my intuition was telling me DFS would be simpler.
// Also, 3. the nr, nc approach is a bit new to me since I've been manually doing it for each side so far (in my 2026 practice).

public class NuAttempt1 {
    const int Isle = 1; //unvisited part of an island
    const int Seen = int.MinValue; // readonly int Seen = int.MinValue; //not sure if MinValue is a const, so using readonly! 

    public int MaxAreaOfIsland(int[][] grid) {
        int maxArea = 0; //makes sense?

        for(int r = 0; r < grid.Length; r++)
        {
            for(int c = 0; c < grid[r].Length; c++) //was doing c < grid.Length until AI caught it.
            {
                if(grid[r][c] != Isle)
                    continue;

                maxArea = Math.Max(maxArea, 
                                GetCurIslandAreaBFS(grid, r, c));
            }
        }

        return maxArea;
    }

    //Directions: up, down, left, right.
    readonly (int r, int c)[] dirDif = [(-1, 0), (1, 0), (0, -1), (0,1)];

    public int GetCurIslandAreaBFS(int[][] grid, int r, int c) //assumess r,c is a valid island
    {
        
        Queue<(int r, int c)> queue = new(); // Was doing [(r,c)]; but compiler caught this by saying it doesn't have Add method!
        
        // STARTING CELL:   //assuming r,c is a valid island (grid[r][c]==1)
        queue.Enqueue((r,c)); //stopped midway through fix earlier thinking I finsihed it :O (only had empty enqueue call)

        var curArea = 1; //*IMPORTANT*: HAD FORGOTTEN TO SET THIS TO 1 (was 0) BEFORE MY SUBMISSION 
                            //and even though AI pointed it out to me, I thought it was correct as is (that my soloution below would handle it :/ ..)
                            // maybe because the default test case worked. Though that is broken now that I fixed this (sends 1 more than needed)) :O
                            // okay fixed that by:
        
        grid[r][c] = Seen; //*IMPORTANT* MISSED EARLIER THIS FOR EARLY MARKING!!! Had to get an AI to point out the error.

        while(queue.Count > 0)
        {
            var cur = queue.Dequeue();
            // if(grid[cur.r][cur.c] != Isle) // *IMPORTANT* CANNOT HAVE THIS HERE FOR EARLY MARKING!
            //     continue;
            for(int i = 0; i < dirDif.Length; i++)
            {
                var (dr, dc) = dirDif[i];
                int nr = cur.r + dr;
                int nc = cur.c + dc; //was doin nc+dc, but compiler caught it thank god!
                // ^ OKAY: FOUND THE BUG!! I WAS DOING r + dr and c + dr BUT I SHOULD HAVE BEEN USING cur!!!
                // Had to have an AI help me spot it.

                if(nr >= 0 && nr < grid.Length && nc >= 0 && nc < grid[nr].Length && grid[nr][nc] == Isle)
                {
                    grid[nr][nc] = Seen; //Early Marking!
                    curArea++;
                    queue.Enqueue((nr, nc));
                }
            }
        }

        return curArea;
    }
}
