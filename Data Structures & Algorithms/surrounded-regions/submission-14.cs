public class Solution {

    ISurroundedRegionsSolver solver;
    public void Solve(char[][] board) {
        // sol

        // # Last Actual Solution: (Late 2024)

        // ## IMPORTANT NOTES:
        // * COULDN'T COME UP WITH THE SOLUTION ON MY OWN (did think about trying to invert the problem but I did not invert it correctly enough)
        // * HAD TO WATCH NEETCODE'S VIDEO TO WRITE THIS!!!!
        // * READ THE COMMENTS FOR Dfs1
        // solver = new Dfs1();

        // # Solution from 7 April 2026:
        
        // ## NOTES: 
        // * The above DFS solution is cleaner!
        
        // * Clearly, yet again after ~1.5 years, I couldn't come up with the approach initially and had to discuss with an AI about 2 days ago about it 
        //   to actually discover how to do it. It did give some substantial hints, but not the solution directly.
        solver = new NuAttempt1_MsBfs();
        

        
        solver.Solve(board);
    }
}

public interface ISurroundedRegionsSolver
{
    void Solve(char[][] board);
}


// # Last Actual Solution: (from Late 2024)

public class Dfs1 : ISurroundedRegionsSolver
{
    // * COULDN'T COME UP WITH THE SOLUTION ON MY OWN (did think about trying to invert the problem but I did not invert it correctly enough)
    // * HAD TO WATCH NEETCODE'S VIDEO TO WRITE THIS!!!!

    // I had initially CONCIOUSLY assumed that ROWS==COLS but I did comment next to the loop that I was assuming this. That assumption turned out to be wrong.

    const char O = 'O';
    const char X = 'X';
    const char NOT_SURROUNDED = '#';
    // int ROWS;
    // int COLS;
    public void Solve(char[][] board) 
    {
        // ROWS = board.Length;
        // COLS = board[0].Length;

        //Clearly, for an edge to be NOT SURROUNDED, it needs to connect directly OR indirectly to the outer boundaries of the array.
        // [!!!IMPORTANT!!!] DOING IT ALL TOGETHER IN ONE LOOP BREAKS IT!!! //[ANSWER] TURNS OUT 

        //1. MARK SURROUNDED TILES

        //If NUM ROWS == NUM COLS
        // for(int rc=0; rc<board.Length; rc++) //Assumes NUM ROWS == NUM COLS ////technically we can ignore the top left, top right, bottom left, and bottom right cells because they can't encase or un-encase anything.
        // {
        //     if(board[rc][0] == O) markNotSurroundedIfVistedDfs(rc, 0, board);//LEFT EDGE
        //     markNotSurroundedIfVistedDfs(0, rc, board);//TOP EDGE
        //     markNotSurroundedIfVistedDfs(rc, board.Length-1, board);//RIGHT EDGE
        //     markNotSurroundedIfVistedDfs(board[0].Length-1, rc, board);//BOTTOM EDGE
        // }
        
        //If NUM ROWS != NUM COLS
        for(int r=0; r<board.Length; r++) //technically we can ignore the top left, top right, bottom left, and bottom right cells because they can't encase or un-encase anything.
        {
            if(board[r][0] == O) markNotSurroundedIfVistedDfs(r, 0, board);//LEFT EDGE
            if(board[r][board[0].Length-1] == O) markNotSurroundedIfVistedDfs(r, board[0].Length-1, board);//RIGHT EDGE
        }
        for(int c=0; c<board[0].Length; c++)
        {
            if(board[0][c] == O) markNotSurroundedIfVistedDfs(0, c, board);//TOP EDGE
            if(board[board.Length-1][c] == O) markNotSurroundedIfVistedDfs(board.Length-1, c, board);//BOTTOM EDGE      //I SPENT HOURS DEBUGGING THIS WHOLE PROGRAM WITH NO IDEA WHAT WAS WRONG BUT I WAS USING [0] IN THE INDEDX INSTEAD OF [c]!!
        }


        //2. TURN NOT_SURROUNDED TILES BACK TO `O`s AND SURROUNDED TO `X`s.
        for(int r=0; r<board.Length; r++)
        {
            for(int c=0; c<board[0].Length; c++) //assumes every row is of same length
            {
                if(board[r][c] == O)
                    board[r][c] = X;
                else if(board[r][c] == NOT_SURROUNDED)
                    board[r][c] = O;
            }
        }
    }

    public void markNotSurroundedIfVistedDfs(int r, int c, char[][] board)
    {
        // Console.WriteLine($"{r}!={board.Length},{c}!={board[0].Length}");
        if( r<0 || c<0 || r == board.Length || c == board[0].Length || board[r][c] != O ) //Additional note: anything already visited would have visited its neighbors too, so no need to repeat (also if we allowed NOT_VISITED there will be cycles.)
            return;
        // Console.WriteLine($">> TRUE");

        board[r][c] = NOT_SURROUNDED;

        markNotSurroundedIfVistedDfs(r-1, c, board);//up
        markNotSurroundedIfVistedDfs(r+1, c, board);//down
        markNotSurroundedIfVistedDfs(r, c-1, board);//left
        markNotSurroundedIfVistedDfs(r, c+1, board);//right
    }
}




// # Solution from 7 April 2026:

// DFS clearly better/smaller/cleaner. Though, it's not too stack friendly.

// ## NOTES:
// 1. Finished code in 19 minutes with 1 single error.
// 2. BUT, I didn't know the approach initially and had to discuss with AI a 2 days ago about it to actually discover how to do it.
// It did give some substantial hints, but not the solution directly.
// And that session took like 6-8 minutes until we got there (I was mistakenly believing it was flood fill intitally!)
// 3. Complexity analysis took 3 minutes.
// 4. The error was accessing cell with board[r][board.Length-1] in first loop (over the vertical edges) AND 
//    board[board[0].Length-1][c] in the second loop (over the horizontal edges)
//    Which obviously wasn't correct. I was supsicious of it when writing (maybe would've caught it if I was feeling well, but I wasn't somehow?).
//    Anyhow, took 3 minutes to find and fix it. (didn't dry run, just looked over the code to see what it could be)
// 5. Total Time Taken: ~33 minutes (approx)

public class NuAttempt1_MsBfs : ISurroundedRegionsSolver {
    const char O = 'O';
    const char X = 'X';
    const char UnflippableO = '\0'; // Better name would be NotSurrounded like in my earlier approach! (added this note post submission!)
    readonly (int r, int c)[] Dirs = [(-1,0),(1, 0),(0,-1),(0,1)]; //up down left right

    // Complexities: where R = Rows count, C = Cols count.
    // TC = O(3*R*C) = O(R*C)       [in any section of our solution, we visit the cells to do actual work no more than once]
    // Aux. SC = O(R*C)
    public void Solve(char[][] board) {
        if(board == null || board.Length == 0 || board[0] == null || board[0].Length == 0)
            return;
        
        Queue<(int r, int c)> unsurroundedQ = new();

        for(int r = 0; r<board.Length; r++)
        {
            if(board[r][0] == O)
            {
                unsurroundedQ.Enqueue((r,0));
                board[r][0] = UnflippableO;
            }
            if(board[r][board[0].Length-1] == O)
            {
                unsurroundedQ.Enqueue((r,board.Length-1));
                board[r][board[0].Length-1] = UnflippableO;
            }
        }

        for(int c = 0; c<board[0].Length; c++)
        {
            if(board[0][c] == O)
            {
                unsurroundedQ.Enqueue((0,c));
                board[0][c] = UnflippableO;
            }
            if(board[board.Length-1][c] == O)
            {
                unsurroundedQ.Enqueue((board[0].Length-1,c));
                board[board.Length-1][c] = UnflippableO;
            }
        }

        MarkUnsurrounded(unsurroundedQ, board);

        for(int r = 0; r<board.Length; r++)
        {
            for(int c = 0; c<board[0].Length; c++)
            {
                var curChar = board[r][c];
                if(curChar == O)
                    board[r][c] = X;
                else if(curChar == UnflippableO)
                    board[r][c] = O;
            }
        }

        return;
    }


    private void MarkUnsurrounded(Queue<(int r, int c)> unsurroundedQ, char[][] board)
    { //This function gets the connected components of the Os at the edges. 
     // Only edges that are directly or indirectly (only through other Os) connected to the ones on edges
     // are ones that are not fully surrounded by X (clearly). So, we just have to avoid flipping connected components of the cells on the edges! 
     // (this also happens to be the most efficient way instead of going through all connected components of Os to see if they reach an edge!) 

        //null checks
        while(unsurroundedQ.Count > 0)
        {
            var (curR, curC) = unsurroundedQ.Dequeue();
            foreach(var (dr, dc) in Dirs)
            {
                int nr = curR+dr, nc = curC+dc;
                //Check for bounds, cycles, and validity (only Os)
                if(nr < 0 || nr >= board.Length || nc < 0 || nc >= board[0].Length || 
                    board[nr][nc] != O)
                    continue;
                unsurroundedQ.Enqueue((nr, nc));
                board[nr][nc] = UnflippableO;
            }
        }

        return;
    }
}
