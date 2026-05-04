
// # Solution from 5-4(Apr)-2026:

// using AdjSet = Dictionary<char, HashSet<char>>;
// using Counter = Dictionary<char, int>;

public class Solution {
    const string InvalidInput = "";
    
    public string foreignDictionary(string[] words) {
        // Read comments from 2024 solution (A.K.A. Last Actual Solution)?
        
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



// # LAST ACTUAL SOLUTION (Late 2024):

// public class Solution { 
//     public string foreignDictionary(string[] words) {//Tricky but not really that hard???.

//         //Did it pretty much while watching NC video :P (don't have enough time till interview) //Still took a while TBH.

//         //[IMPORTANT] ALL OF THE FOLLIWING COMMENTS ARE IMPORTANT:

//         //POST-ORDER DFS SEEMS TO BE EASIER FOR TOPOLOGICAL SORTING THAN BFS
//         //Watched Neetcode video first. (like in most, if not all, Advanced Graph problems from NC150. These are all because of limited time until the interview.)
        
//         //FROM NEETCODE VIDEO: ACCORDING TO PROBLEM DESCRIPTION ONE EDGE CASE IS WHEN STRINGS ARE OF DIFFERENT LENGTH BUT ONE OF THEM IS A PREFIX OF ANOTHER e.g [prefixedword, prefix], WHICH IS WRONG AND WE CAN RETURN []
        
//         //(NOTICING THAT YOU CAN AND ACTUALLY) CONVERTING IT TO GRAPH IS THE TRICKIER PART (using first differing character to form these relationships) [I can notice an EDGE CASE of a cyclc here i.e. a comes after b but b comes after a too]

//         //[IMP] AFTER CONVERTING IT GRAPH, WE CAN KNOW IT IS A TOPOLOGICAL SORT BECAUSE WE WANT A LIST(here, as a string) OF CHARACTERS WHERE EACH CHARACTER THAT COMES AFTER A CHARACTER WILL BE PLACED AFTER THAT CHARACTER.
//         //Also, multiple solutions means topological sort will work (because you can do topo sort in multiple ways).
//         //Now, in order to find where to sort, recall watching Neetcode's Topological Sort theory video from Advanced Algorithms course. You can use toposort from every node to build up the res. Watch the video again if needed, but here's a rough explanation anyway: E.g. if you start from the true first character, you will get all its children, otherwise if you start form its children you will get all of them and add them in correct order to result and mark it visited. Then call it from the parent which will try to call it for its children but we already have the result of this child and it is in visited, so we call for other children parallel to it and then append them (the order here doesn't matter because they're all before true root as needed and before their respective firsts), then we add the parent itself since all its children were added. 
//         //Also recall that, from the same video, that it will also work for multiple connected component e.g. r>t>f, w>e. By the same approach even.
        
//         // NOTE THAT TO AVOID THE CYCLE EDGE CASE I THOUGHT OF EARLIER, WE WILL EITHER NEED ANOTHER HASHSET FOR CURRENT PATH (so that if we visit a node in current path again, it means there's a cycle)
//         //  OR USE A DICTIONARY WITH A BOOL VALUE (false => Visited and Added to result once | true => Currently in the process of visiting (i.e. going to its children before adding it to result)). Meaning if we get true on a node we visit again, it was part of our path to get here and hence there's a cycle. 
        
//         //[IMP.] WE ONLY COMPARE A WORD TO THE ONE DIRECTLY NEXT TO IT!!! BECAUSE:
//         //  * The words are pre-sorted (according to aliens' standards)
//         //  * Only the first differing character is what we can make a judgement for.
//         //  * After that an inequality for that character has been established (e.g. a -> b). Then, IF we were to find another dependency like (b -> c), we don't need to add (a->c), we're okay with (a->b->c).
//         //  * If the inequality is at a different position, then we couldn't make a judgement about its relation with `a` anyway. 
        
//         return TopologicalSort_1(words);
//     }

//     Dictionary<char, HashSet<char>> CreateAdjacencyList_1(string[] words)
//     {
//         Dictionary<char, HashSet<char>> graph = new();
//         // Adding HashSet for every character to make things easier:
//         foreach(var wrd in words) //O(n*m)
//         {
//             foreach(var chr in wrd)
//             {
//                 graph[chr] = new();
//             }
//         }

//         // Create graphs:
//         //[IMP.] WE ONLY COMPARE A WORD TO THE ONE DIRECTLY NEXT TO IT!!! BECAUSE:
//         //  * The words are pre-sorted (according to aliens' standards)
//         //  * Only the first differing character is what we can make a judgement for.
//         //  * After that an inequality for that character has been established (e.g. a -> b). Then, IF we were to find another dependency like (b -> c), we don't need to add (a->c), we're okay with (a->b->c).
//         //  * If the inequality is at a different position, then we couldn't make a judgement about its relation with `a` anyway.
//         for(int i = 0; i < -1 + words.Length; i++) 
//         {
//             string w1 = words[i];
//             string w2 = words[i+1];
//             if(w1.Length > w2.Length && w1.StartsWith(w2)) //Edge case: Invalid ordering e.g. [prefix, pre] is wrong, it should be [pre, prefix]
//                 return null;
//             int minLen = (int) Math.Min(w1.Length, w2.Length);
//             for(int j=0; j<minLen; j++)
//             {
//                 if(w1[j]==w2[j])
//                     continue;
//                 //The following thing I tired to do is wrong because w1 comes before in list, so in alien lexicography, w1[j] is lexicologically smaller than w2[j] (or at least its supposed to be. We'll handle ones that actually don't later in the loop when detecting cycles) //[INCORRECT]if(w1[i]>w2[i]) {var temp = w2;w2 = w1;w1 = temp;} //this condition makes it so that variable w1 references the lexicographically smaller one and w2 references bigger one to make adjacency list creation less verbose

//                 graph[w1[j]].Add(w2[j]);          
//                 break; //only want first differing character
//             }
//         }

//         return graph;
//     }

//     string TopologicalSort_1(string[] words)
//     {
//         //Create graph/adjList
//         Dictionary<char, HashSet<char>> next = CreateAdjacencyList_1(words);
//         if(next == null)
//             return string.Empty;

//         //Build AdjList:
//         Dictionary<char, bool> inCurrentPath = new(); //The keys existing means either seen before or there's a cycle. value == true => in current path => cycle. value == false => seen before but already added to results and no need to consider any further.

//         List<char> res = new();

//         foreach(var chr in next.Keys)
//         {
//             if(TopoSortDfs(chr, inCurrentPath, next, res)) //Cycle detected.
//                 return string.Empty;
//         }

//         res.Reverse();
//         return new string(res.ToArray());
//     }

//     bool TopoSortDfs(char cur, Dictionary<char, bool> inCurrentPath,  Dictionary<char, HashSet<char>> next, List<char> res) //Post-Order DFS is the best way for Topological Sort
//     {
//         if(inCurrentPath.ContainsKey(cur))
//         {
//             if(inCurrentPath[cur])
//                 return true;    //Still visiting somewhere above in the recurision stack (i.e. there's a cycle cuz we got to this node while traversing its children)
//             else
//                 return false;   //visited and added to res already, but not in current path (no cycle)
//         }
        
//         inCurrentPath[cur] = true;

//         foreach(var neibr in next[cur])
//         {
//             if(TopoSortDfs(neibr, inCurrentPath, next, res))
//                 return true;
//         }

//         inCurrentPath[cur] = false;

//         res.Add(cur);

//         return false;
//     }
// }
