// NeetCode notes on common pitfalls:

// ## Common Pitfalls
// ### Not Restoring the Cell After Backtracking
// When marking a cell as visited by changing its value (e.g., to '#'), forgetting to restore the original character after exploring all neighbors will permanently modify the board. This causes other paths starting from different cells to incorrectly treat valid cells as already visited.

// ### Checking Visited Before Matching Character
// Placing the visited check before the character match check can cause subtle bugs. If a cell is marked visited with a special character like '#', comparing board[r][c] != word[i] will correctly fail. However, if using a separate visited structure, the order of checks matters for correctness and clarity.

// ### Forgetting to Match the First Character Before DFS
// Starting DFS from every cell without first checking if board[r][c] == word[0] wastes time exploring paths that cannot possibly match the word. While not incorrect, this optimization significantly improves performance on large boards.




public class Solution {
    public bool Exist(char[][] board, string word) {
        
        //# 2024
        //DID IT ALL ON MY OWN, LET'S GOOOO!!!
        return backtrack1Wrapper(board, word); //Read the ONE comment I left on it :P

        //# 2026
        // Even though I was exhausted and sleepy right now, 
        // my soln. from 2024 was better than `NuAttempt1` (except span word and tuple input I guess and TC and SC analysis I guess)

        // return NuAttempt1.Exist(board, word); 
    }

    // # My solution from 11-05-2024
    public bool backtrack1Wrapper(char[][] board, string word)
    {
        for(int i=0;i<board.Length;i++)
        {
            for(int j=0; j<board[i].Length;j++)
            {
                if(word[0]==board[i][j] && backtrack1(board, word, 0, i, j))
                    return true;
            }
        }
        return false;
    }
    public bool backtrack1(char[][]board, string word, int idx, int x, int y) // # Part of my solution from 11-05-2024
    {
        if(idx==word.Length)
            return true;
        if(x==-1||y==-1||x==board.Length||y==board[x].Length||word[idx]!=board[x][y])
            return false;
        
        char curr = board[x][y];
        board[x][y] = '*'; //COULD'VE USED A SET OF TUPLES IF MODIFYING ARRAY WAS NOT ALLOWED OR THERE WAS NO UNALLOWED CHARACTER.

        bool isFound = 
            backtrack1(board, word, idx+1, x+1, y) ||
            backtrack1(board, word, idx+1, x-1, y) ||
            backtrack1(board, word, idx+1, x, y+1) ||
            backtrack1(board, word, idx+1, x, y-1);

        board[x][y] = curr;

        return isFound;
    }
}


// # From 19-03(March)-2026 : 
//(My 2024 solution, at first attempt, was waaay better! 
// This time I even forgot to think if I can modify given array in place instead of used AND MORE!)
public static class NuAttempt1 { //DAMN MY FIRST ATTEMPT WAS WAAAY BETTER (tbf, I am doing this exhausted at 2 AM)

    //Finished in 35 minutes (with a lot of help from NC auto test cases and one AI hint at the end, but I am also exhausted!)

    // Let R = board.Length and C = board[0].Length and W = word.Length
    // TC = O(R * C * 3^W) //4^W)
    // Aux. SC = O(W)
    public static bool Exist(char[][] board, string word) {
        if(board.Length == 0 && word.Length > 0)
            return false;
        if(word.Length == 0)
            return true;

        for(int r = 0; r<board.Length; r++)
        {
            for(int c = 0; c < board[0].Length; c++)
            {
                if(Backtracking(board, (r,c), word.AsSpan(), new())) //forgot about AsSpan until ran on NeetCode
                    return true;
            }
        }

        return false;
    }

    //W length path AT MOST
    //4^W [W places AT MOST and 4 (actually, 3 after first) choices AT MOST at each place]
    public static bool Backtracking(char[][] board, (int row, int col) start, ReadOnlySpan<char> word, HashSet<(int row, int col)> used) //forgot ReadOnly but caught it right when changing to AsSpan above!
    {
        // damn, I forgot about the visited already part! I remembered it at the start but lost track of it somewhere along the way!
        // Remembered because of neetcode auto test cases.
        // Also had a fair number of tuple syntax problems initially!
        // On second NC auto test run, failed test case for single element matrix! 
        Console.WriteLine($"{word.ToString()} : {start}");
        if(word.Length == 0)
            return true;

        //WRONG POSITION FOR THIS: (had to ask AI what was wrong until i realized!)
        // if(!used.Add(start)) //Value Tuple Hashability FTW!
        //     return false;

        (var curRow, var curCol) = start;
        if(word[0] != board[curRow][curCol])
            return false;

        if(!used.Add(start)) //Value Tuple Hashability FTW!
            return false;

        if(word.Length == 1) //used check above already! // This check also above: && word[0] == board[row][col]
            return true;
        // if(word.Length > board[row].Length - col) //if we don't have as many elements left in this row as we need, we can just backtrack
        //     return false;

        //Decision: Which element do we use next for given start
        var up = (row: curRow - 1, col: curCol);
        var down = (row: curRow + 1, col: curCol);
        var left = (row: curRow, col: curCol - 1);
        var right = (row: curRow, col: curCol + 1);

        var found = false;
        //UP:
        if(up.row >= 0)
            found = Backtracking(board, up, word[1..],used);
        //Down:
        if(!found && down.row < board.Length)
            found = Backtracking(board, down, word[1..], used);
        //Left:
        if(!found && left.col >= 0)
            found = Backtracking(board, left, word[1..], used);
        //UP:
        if(!found && right.col < board[0].Length)
            found = Backtracking(board, right, word[1..], used);

        //forgot to add removal of used too!
        used.Remove(start);
        
        return found;
    }
}
