
// # Solution from 25-04(Apr)-2026 :

public class Solution {
    // Uses Hierholzer's Algorithm (w/ Recursive Post Order DFS)
    public List<string> FindItinerary(List<List<string>> tickets) {
        var startAp = "JFK";
        List<string> path = new();

        // Lexicographical order:
        tickets.Sort((lst1, lst2) => lst1[1].CompareTo(lst2[1])); //IMPORTANT!! Need to Sort by Destination! (Only spotted that when doing a dry-run as I had source based sorting previously!)
        
        GetAdjList(tickets, out var adj);

        GetEulerianPath_DfsPostOrder(adj, startAp, path);

        path.Reverse(); //NVM this was inplace! //DEPRECATED: could implement our own O(1) space swap-based reverse, but we'll stick with LINQ for time.
        return path;
    }

    // path is reversed!
    public void GetEulerianPath_DfsPostOrder(Dictionary<string, Queue<string>> adj, string curAp, List<string> path) {
        if(adj.ContainsKey(curAp)) //NOT already a deadend
        {
            var neiQ = adj[curAp];
        
            while(neiQ.Count > 0) {
                var neiAp = neiQ.Dequeue();
                GetEulerianPath_DfsPostOrder(adj, neiAp, path);
            }
            
        }

        // WE WANT DUPLICATES SINCE WE CAN VISIT THE SAME PLACE MORE THAN ONCE!
        // Was stuck here 13 minutes in to coding. Tried to think for 2 minutes (total 15 mins), 
        // then asked AI for vague hint then they said "think about how the duplicates is a property we want".
        // After that, by 20 minutes I was done with the implementation.
        // WRONG: When we get here, it has become a dead end!
        // WRONG: got stuck here for a few minutes because directly adding here would cause duplicates!

        // The node is now (or was already) a dead-end (all its neighbors are exhausted/deleted)
        path.Add(curAp);
    }

    public void GetAdjList(List<List<string>> tickets, out Dictionary<string, Queue<string>> adj) {
        // DEPRECATED (only for undirected): // could add validation here for indegrees and outdegrees to confirm (only 2 odd, rest even, forgot the exact specifics) [including whether start is start]
        
        // For directed graph, we could add validation here that:
        // - indegree == outdegree for all EXCEPT 2 (which are as below)
        // - Start (here we check specfically for JFK) has outdegre = 1 + indegree
        // - End has indegree = 1 + outdegree!

        adj = new();
        foreach(var ticket in tickets) {
            var src = ticket[0];
            var dst = ticket[1];
            adj.TryAdd(src, new());
            adj[src].Enqueue(dst);
        }
    }
}


// # Last Actual Solution: [from 27-11(Nov)-2024]

// public class Solution {
//     public List<string> FindItinerary(List<List<string>> tickets) {
//         //[IMPORTANT!!] PLEASE WATCH THE FOLLOWING FOR BETTER UNDESTANDING:
//         //  [William Fisset (YouTube Channel): "Eulerian Path/Circuit algorithm (Hierholzer's algorithm) | Graph Theory"](https://www.youtube.com/watch?v=8MpoO2zA2l4)
        
//         //[IMPORTANT!!] Also, read my comments for dfsHierholzersAlgorithmWrapper_1 and its dfs helper.


//         //My first thought is topological sort.
//         //NOW THAT I THINK ABOUT IT, THAT PROBABLY WON'T WORK BECAUSE WE WANT TO GET THE ONE WITH MINIMUM LEXOGICAL ORDER.
        
//         //Watched the Neetcode video of the solution (except the implementation/code part),
//         //and found out that we CAN visit the same node twice, BUT NOT THE SAME EDGE (ticket), so we remove from adjacencyList when done.
        
//         //READ COMMENTS IN ACTUAL SOLUTION (dfsHierholzersAlgorithm_1)!!
//         return dfsHierholzersAlgorithmWrapper_1(tickets);
//     }

//     Dictionary<string, List<string>> graph; //adjacency list //SC: O(V+E)
//     List<string> itnry;
//     const string startingPoint = "JFK";
    
//     List<string> dfsHierholzersAlgorithmWrapper_1(List<List<string>> tickets)
//     {
//         //[EXTRA IMPORTANT NOTE] Hierholzer's algorithm basically asks for any nodes with unused edges
//         //after first branch of DFS, what path should we have taken before to include those edges before going to the next node. Whenever all edges have been visited for a node, it means that we haven't left out anything, and as such it can be safely added to the result at the correct position (there's nothing left to visit before it)
//         // - Here, we are guaranteed an itenerary [here, Eulerian path but also works for cycles/circuits (Eulerian circuits/cycles)]
//         //  exists, so we do NOT need to check using `indegree-outdegree <= 1` ONLY AT 1 NODE
//         //  && `outdegree-indegree <= 1` ONLY AT 1 NODE (different from the previous one, obviously).
//         // - We also don't need to maintain a separate outdegree count when using adjacency list because we can just get the list's count.
//         // - PLEASE WATCH THE FOLLOWING FOR BETTER UNDESTANDING:
//         //  [William Fisset (YouTube Channel): "Eulerian Path/Circuit algorithm (Hierholzer's algorithm) | Graph Theory"](https://www.youtube.com/watch?v=8MpoO2zA2l4)
//         graph = new();
//         itnry = new();
        
//         //[EXREMELY_IMPORTANT_NOTE] Two things going on below:
//         // 1. [GREAT_TRICK] Ordering tickets here by destination means the adjacency list we create using it will be ordered correctly.
//         // 2. [GREAT_TRICK] Ordering Descending (in Lexicographical terms) means that we iterate from the end of the list, which not only
//         //      makes it O(1) to remove the element at each level of DFS so we don't consider it anymore in that branch, but also
//         //      makes it O(1) to add elements back at their index (at the end of the list at each level). //Credit to some random LeetCode C# solution I saw.
//         tickets = tickets.OrderByDescending(arr => arr[1]).ToList(); //O(E*log2(E))


//         foreach(var ticket in tickets) //O(E)
//         {
//             graph.TryAdd(ticket[0], new());

//             graph[ticket[0]].Add(ticket[1]);
//         }
        
//         dfsHierholzersAlgorithm_1(startingPoint); //O(E) because we go through all the edges once (and can visit a vertex multiple times)

//         itnry.Reverse(); //O(E) //Since we append airports post-recursion (post-order), the itinerary is built in reverse. Reverse the itinerary list before returning it.

//         return itnry;
//     }

//     void dfsHierholzersAlgorithm_1(string src) //DFS with Backtracking.
//     {
//         //[IMPORTANT NOTE] CHECK MY NOTES ABOUT Hierholzer's algorithm AT THE BEGINNING OF THE `dfsHierholzersAlgorithm_1` FUNCTION
//         // THIS SOLUTION USES THAT AND IT CAN ALSO BE CONFUSING TO JUST GET BY SEEING THE CODE
//         // IT IS THEREFORE CRITICAL TO ACTUALLY CHECK WHAT'S HAPPENING!

//         //Due to the sorted neighbor lists in adjList, the itinerary we get the 
//         //first time we exhaust all tickets(edges) is the lexicographically smallest and 
//         //as such can be returned without needing to compare it with others.
//         while(graph.ContainsKey(src) && graph[src].Count > 0) //We run this loop until all edges OUTGOING edges are used up, meaning we haven't missed ANY path OUTGOING FROM here (and due to recursion, the same is true for ALL of the nodes that we visited after it). //DEPRECATED(Maybe?) : also skips this and returns in case of base case (no tickets from here / Final destination? (case 1: there never were any(containskey) case 2: already used up in current branch(graph[cur].Count==0)))
//         {
//             var dest = graph[src][^1]; //the last element is the next lexicographically smallest destination from this airport
//             graph[src].RemoveAt(graph[src].Count-1); //Remove `dest` of this iteration from the adjacecny list
//             dfsHierholzersAlgorithm_1(dest);
//             //We don't add back dest to the adjacency list because of how Hierholzer's algorithm works (explained above). Might have been easier or better to just use an adjacency queue :P.
//         } //Sometimes the rest of the adjacency list will be cleared out (fully or partially) in a another/deeper level of the recursion because we'll get through this node from a different path while clearing out the remaining outgoing edges.
//         itnry.Add(src);
//     }
    
// }
