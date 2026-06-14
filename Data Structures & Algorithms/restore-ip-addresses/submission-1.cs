public class Solution {
    // Did it in mock interview with friend (Yusuf) [14-06-26]
    // Finished approach discussion and explanation in 23 minutes.
    // TC analysis took a bit tho.
    // Dry run wasnt perfect either and took time. 


    public List<string> RestoreIpAddresses(string s) {
        List<string> res = new();
        Backtrack(s.AsMemory(), res, []);
        return res;
    }

    const string sep = ".";

    void Backtrack(ReadOnlyMemory<char> input, List<String> res, List<ReadOnlyMemory<char>> cur) {
        if(cur.Count == 4 && input.Length == 0) {
            // res.Add(string.Join('.', cur)); //we assume this syntax works even tho its ROS
            var sepSpan = sep.AsSpan();
            // We combine the first 4 pieces into a temporary string...
            string part1 = string.Concat(cur[0].Span, sepSpan, cur[1].Span, sepSpan);
            // ...then combine that temporary string with the last 3 pieces.
            res.Add(string.Concat(part1, cur[2].Span, sepSpan, cur[3].Span));
            return;
        }
        if(cur.Count == 4) {// && input.Length > 0) {
            return;
        }
            
        //Potential optimization: remainingSpaces = 4 - cur.Lenght; maxDigitsInSingleSpace = 3; if(remainingSpace < input.Length/(double)maxDigitsInSingleSpace) return;
        // <- if more spaces needed in future than we have right now, just backtrack
        // ALSO, could do it if input.Length < remainingSpaces
        // Case 2: curLen =0, remSpaces = 4, maxDig= 3, so -> 15/3 = 5, 5> 4, return. (on first CALL of this function)
        int remainingSpaces = 4 - cur.Count;
        const double maxDigitsInSingleSpace = 3;
        if(input.Length < remainingSpaces || remainingSpaces < input.Length/(double)maxDigitsInSingleSpace)
            return;

        var bound = input.Length;
        if(input.Span[0] == '0') {
            bound = 1;
        }

        for(int i = 0; i < 3 && i<bound; i++) { // bound = 3
            cur.Add(input[..(i+1)]); //1.1.1.1
            if(i <  2 ||  int.Parse(cur[^1].Span) <= 255) //added this only at 29 mins
                Backtrack(input[(i+1)..], res, cur); 
            cur.RemoveAt(cur.Count-1); //
        }

        return;  
    }

}