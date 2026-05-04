
// using AdjSet = Dictionary<char, HashSet<char>>;
// using Counter = Dictionary<char, int>;

public class Solution {
    const string InvalidInput = "";
    
    public string foreignDictionary(string[] words) {
        if(!GetAdjList(words, out var adj, out var indegrees)) //main logic, tbh,
            return InvalidInput;

        // Kahn's Topo Sort:
        
        Queue<char> resolvedQ = new();

        foreach(var (ch, indeg) in indegrees) {
            if(indeg == 0)
                resolvedQ.Enqueue(ch);
        }
        
        List<char> res = new();

        while(resolvedQ.Count > 0) {
            var cur = resolvedQ.Dequeue();
            res.Add(cur);
            foreach(var next in adj[cur]) {
                if(--indegrees[next] == 0) {
                    resolvedQ.Enqueue(next);
                }
            }
        }

        if(res.Count != indegrees.Count)
            return InvalidInput;
        
        return string.Concat(res);
    }

    //Leaving this here just for reference.
    bool OverComplicated_GetAdjList_OverComplicated(string[] words, out Dictionary<char, HashSet<char>> adj, out Dictionary<char, int> indegrees) {
        //could use empty validation //hashset cuz we don't need duplicate relationships CHECK INDEGREE ADDING LOGIC TO SEE WHY
        adj = new();
        indegrees = new();
        
        foreach(var ch in words[0]) {
            adj.TryAdd(ch, new());
            indegrees.TryAdd(ch, 0);
        }

        for(int i = 1; i < words.Length; i++) {
            string prev = words[i - 1];
            string cur = words[i];
            bool foundDiff = false;
            for(int j = 0; j < cur.Length; j++) {
                // *IMPORTANT* Need to have these 2 outside of the below if!
                adj.TryAdd(cur[j], []);
                indegrees.TryAdd(cur[j], 0);

                if(!foundDiff && j < prev.Length && prev[j] != cur[j]) {
                    foundDiff = true;
                    if(adj[prev[j]].Add(cur[j])) //NEED TO HAVE AT MOST ONE OF EACH TO AVOID INDEGREE DOUBING!
                    {
                        indegrees[cur[j]]++;
                    }
                }
            }
            if(!foundDiff && prev.Length > cur.Length)
                return false; //since they're the same word, but tbf that shouldn't be possible in language dictionaries.
        }

        return true;
    }


    // Got this one from Gemini AI, but kept the overcomplicated one I wrote there just for reference!
    bool GetAdjList(string[] words, out Dictionary<char, HashSet<char>> adj, out Dictionary<char, int> indegrees) {
        adj = new();
        indegrees = new();

        // PHASE 1: Node Setup (Initialize EVERYTHING first)
        // This guarantees we never hit a KeyNotFoundException later.
        foreach(var word in words) {
            foreach(var c in word) {
                adj.TryAdd(c, new HashSet<char>());
                indegrees.TryAdd(c, 0);
            }
        }

        // PHASE 2: Edge Building
        for(int i = 1; i < words.Length; i++) {
            string prev = words[i - 1];
            string cur = words[i];

            // Edge Case: Invalid Prefix (e.g., "abc" before "ab")
            if (prev.Length > cur.Length && prev.StartsWith(cur)) {
                return false; 
            }

            // Find the FIRST difference to build our edge
            int minLen = Math.Min(prev.Length, cur.Length);
            for(int j = 0; j < minLen; j++) {
                if(prev[j] != cur[j]) {
                    // If this is a new edge, update indegrees!
                    if(adj[prev[j]].Add(cur[j])) { 
                        indegrees[cur[j]]++;
                    }
                    
                    // IMPORTANT: Stop looking! Only the first difference matters.
                    break; 
                }
            }
        }

        return true;
    }

    // void GetAdjList(string[] words, out Dictionary<char, HashSet<char>> adj, out Dictionary<char, int> indegrees) {
    //     //could use empty validation
    //     adj = new();
    //     indegrees = new();

    //     var prev = words[0];
    //     foreach(var ch in words[0]) {
    //         adj.TryAdd(ch, new());
    //         indegrees.TryAdd(ch, 0);
    //     }

    //     for(int i = 0; i < words.Length; i++) {
    //         var cur = words[i];
    //         var curPrefixOfPrev = prev.Length > cur.Length; //completely calculated below:
    //         for(int j = 0; j < cur.Length; j++) {
    //             if(j < cur.Length && j < prev.Length)
    //             {
    //                 if(prev[j] != cur[j]) {
    //                     curPrefixOfPrev = false;
    //                     adj.TryAdd(prev[j], new());
    //                     if(adj[prev[j]].Add(cur[j])) { //if NOT added this relationship already!
    //                         indegrees.TryAdd(cur[j], 0);
    //                         indegrees[cur[j]]++;
    //                     }
    //                     break; //we can only make the call for the first disagreeing letters' position.
    //                 }
    //             }
    //             else if (j < cur.Length) {
    //                 indegrees.TryAdd(cur[j], 0);
    //             }
    //         }
    //         if(!curPrefixOfPrev)
    //             prev = cur;
    //     }
    // }
}
