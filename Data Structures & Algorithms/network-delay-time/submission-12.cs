// # Solution from 24-04(April)-2026:

public class Solution {
    const int NotFound = -1;

    public int NetworkDelayTime(int[][] times, int nodeCount, int start) {
        var adj = GetAdjList(times);
        return GetMinTotalTime(adj, nodeCount, start);
    }

    public Dictionary<int, List<(int time, int dst)>> GetAdjList(int[][] times) {
        Dictionary<int, List<(int time, int dst)>> adj = new();
        foreach(var time in times) {
            adj.TryAdd(time[0], new());
            adj[time[0]].Add((time: time[2], dst: time[1]));
        }
        return adj;
    }

    public int GetMinTotalTime(Dictionary<int, List<(int time, int dst)>> adj, int nodeCount, int start) {
        // my earlier, worse and "breaking at cycle to first node attempt" took me 26 minutes for the code only, 
        // but I needed some hints, with only one being major, which is what led to this version (QUEUE START ONLY)!

        // # *IMPORTANT* Note:
        // I need to remember to treat Dijkstra as just a BFS with a PriorityQueue instead of a Queue, 
        // the PQ contains (int cost, int node/dst) where time is Total Cost to get to that node from start
        // and we maintain a minCost hashmap for cycle tracking AND the minimum cost for reaching each node from start!
        // THIS SIMILARITY SHOULD ALSO REMIND ME TO ONLY ENQUEUE START NODE AT FIRST, AND NOT ITS EDGES!! 
        // (for not breaking in cases where there's a cycle back to first node)
        
        // Also, please REMEMBER to do a minTime.ContainsKey(cur.dst) EVEN IF YOU DO OPTIONAL OPTIMIZATION FOR NEIGHBORS WITH THIS
        // WHY?? BECAUSE THESE DON'T GET ADDED UNTIL WE POP FROM PQ AND A PQ CAN HAVE MULTIPLE EDGES TO IT AT THE TIME!

        PriorityQueue<(int time, int dst), int> minHeap = new();
        minHeap.Enqueue((0, start), 0);
        
        Dictionary<int, int> nodeMinTime = new();
        
        var totalTime = 0;
        while(minHeap.Count > 0 && nodeMinTime.Count < nodeCount) {
            var cur = minHeap.Dequeue();

            if(nodeMinTime.ContainsKey(cur.dst)) // ABSOLUTELY NECCESSARY HERE!     [almost didn't have it until a neetcode.io test case failed with key already exists]
                    continue;

            totalTime = cur.time; //every time is greater than last
            nodeMinTime.Add(cur.dst, totalTime);
            
            if(!adj.ContainsKey(cur.dst)) //no neighbors
                continue;

            foreach(var nei in adj[cur.dst])
            {
                if(nodeMinTime.ContainsKey(nei.dst)) //WRONG: only need this here in new version. MISCONCEPTION! In fact, This one is just optional optimization!
                    continue;
                var minTimeToNei = nei.time + cur.time;
                minHeap.Enqueue((minTimeToNei, nei.dst), minTimeToNei);
            }
        }
        
        if(nodeMinTime.Count != nodeCount) //not adding n to it of course!
            totalTime = NotFound;

        return totalTime;
    }

    public int GetMinTotalTime__FirstTry_MostlyButNotFully_WithoutHints(Dictionary<int, List<(int time, int dst)>> adj, int nodeCount, int start) {
        if(!adj.ContainsKey(start))
            return NotFound;
        PriorityQueue<(int time, int dst), int> minHeap = new(adj[start].Select(tup => (tup, tup.time)));
        Dictionary<int, int> nodeMinTime = new();
        
        var totalTime = 0;
        while(minHeap.Count > 0 && nodeMinTime.Count < nodeCount) {
            var cur = minHeap.Dequeue();
            if(nodeMinTime.ContainsKey(cur.dst)) //already found a minimum! //need it here too for edges queued up in the heapify
                continue;
            totalTime = cur.time; //every time is greater than last
            nodeMinTime.Add(cur.dst, cur.time);
            
            if(!adj.ContainsKey(cur.dst)) //no neighbors
                continue;

            foreach(var nei in adj[cur.dst])
            {
                if(nodeMinTime.ContainsKey(nei.dst))
                    continue;
                var minTimeToNei = nei.time + cur.time;
                minHeap.Enqueue((minTimeToNei, nei.dst), minTimeToNei);
            }
        }
        
        if(nodeMinTime.Count < nodeCount-1) //not adding n to it of course!
            totalTime = NotFound;

        return totalTime;
    }
}



// # Last Actual Solution: [30-11(Nov)-2024]:

// //## BREAKDOWN:
// //### Given: 
// //A network of n nodes, labeled from 1 to n. You are also given times, a list of directed edges where `times[i] = (ui, vi, ti)`.
// // - ui is the source node (an integer from 1 to n)
// // - vi is the target node (an integer from 1 to n)
// // - ti is the time it takes for a signal to travel from the source to the target node (an integer greater than or equal to 0).
// // Finally, we have k, the label of the Node that sends the signal
// //### Input format: 
// //int[][] times, int n, int k
// //### Output format: int
// //### Result:
// //  minumum time it takes for all of the nodes to receive signal.
// //  -1 if it is impossible
// //### Example: ?
// //### Clarifications:
// //### Edge cases: ?
// //### Constraints:
// //### Observations: It is a directed, weighted graph.
// //### Approach: basically find the minimum spanning tree

// public class Solution {
    
//     public int NetworkDelayTime(int[][] times, int n, int k) {
//         //[IMPORTANT] THIS IS NOT MST PROBLEM, UNLIKE WHAT I INITIALLY THOUGHT!!
//         // ## Example:
//         //  Example of why MST does NOT necessarily give shortest path between 2 vertices.
//         //
//         // ## Graph:
//         // (A)----5---(B)----5---(C)
//         //  |                     |
//         //  |----------7----------| 
//         //
//         // - For MST, edges A-B and B-C will be on MST with total weight of 10. 
//         //      So cost of reaching A to C in MST is 10.
//         // - But in Shortest Path (SPT) case, shortest path between A to C is A-C, 
//         //      which is 7. So, A-C here is 7. But, A-C was never on MST.
//         // - So, we can see that SPT in our problem would act as we want, because for
//         //      this case it would make max time for the signal to reach all nodes = 7.
//         //      Because right from the start, the same signal would be sent through multiple lines at once/concurrently.
//         // - MST would NOT work because it would make the max time = 10 (to reach all nodes), 
//         //      it would have worked if we had some limitation stating a signal cannot 
//         //      be cloned/split.

//         // ## MST VS. SPT:
//         // ### SPT usecases: [Shortest Path from Source to Any Node]
//         //  - When you have want to go from a single point to another final point, 
//         //      without needing to visit any other nodes specifically.
//         //  - Or When you want to connect all points in a way that the distance of 
//         //      all of them to A is minimum. (We don't care about total length of path)
//         //  - Or when you need to visit all points (from a source node), but can traverse 
//         //      multiple branches at the same time (e.g. the same signal in a network is sent through all the wires) [same time == concurrently].
//         //  - Dijkstra can work on DIRECTED OR UNDIRECTED graphs.
//         // ### MST usecases: [Shortest Path with All Nodes Included]
//         //  - When you have to go to all nodes at least once, but you can only traverse 
//         //      one branch at a time.
//         //  - The goal of this is algorithm is to minimize the cost to connect all the points 
//         //      (e.g. minimizing length or cost of wire used for a telecommunications company trying to lay cable in a new neighborhood. If it is constrained to bury the cable only along certain paths (e.g. roads), then there would be a graph containing the points (e.g. houses) connected by those paths. The length or cost of buring wire along each road would be its weight.)
//         //  - Prim can work ONLY ON DIRECTED graphs.
//         // ### Observations: 
//         // - It does seem SPT prefers performance (connection from source to each 
//         //      node is minimized), 
//         // - whereas MST prefers efficiency (total cost is minimized).
//         // - In this problem especially, since we can just send the same signal through
//         //      multiple branches parallely/concurrently and we want to minimize 
//         //      the time it takes for all nodes to get the signal, we simply want to pick
//         //      paths such that all nodes receive the signal as fast as possible, even if
//         //      the overall cost (sum of all weights) is not minimzed.

//         // Example for MST vs Dijkstra:
//         // ```
//         // Nodes: 1, 2, 3, 4
//         // Edges:
//         // 1 -> 2 (weight = 1)
//         // 1 -> 3 (weight = 2)
//         // 2 -> 4 (weight = 1)
//         // 3 -> 4 (weight = 1)
//         //```
//         // Shortest Path Tree (SPT) from Node 1:
//         // Dijkstra's Algorithm:
//         // 1 -> 2 (cost = 1)
//         // 1 -> 3 (cost = 2)
//         // 2 -> 4 (cost = 2 via node 2)
//         // Total weight = 1+1+2 = 4
//         // SPT focuses on the shortest path from node 1 to all others.
//         // Prim's Algorithm from Node 1:
//         // 1 -> 2 (cost = 1)
//         // 1 -> 3 (cost = 2)
//         // 2 -> 4 (cost = 2 via node 2)
//         // Total weight = 1+1+2 = 4
//         // SPT focuses on the shortest path from node 1 to all others.


//         // MST (e.g. Prim's algorithm) is used to get the shortest path that connects all nodes, but not neccessarily the shortest distance to each node. Because at each
//         // Shortest Path (SPT) (e.g. Djikstra's Algorithm) is used to get shortest path FROM 1 node.
        
//         // MST ensures a global minimum (sum of all weights) across the entire graph.
//         // SPT ensures a local minimum (shortest path) from the source node to each individual node.
        
//         // MST may include edges that are not part of any shortest path but are necessary for minimizing the total connection cost.
//         // SPT only includes edges that lie along the shortest paths from the source.
        
//         // The SPT is rooted at the source node, and the structure is DIRECTED (if the graph is directed).
//         // MST is an UNDIRECTED structure, focusing solely on connection without regard to direction or specific paths. (because it's a tree)

//         //

//         //(From the following resource: https://courses.cs.washington.edu/courses/cse373/23au/lessons/graph-algorithms/#:~:text=Dijkstra%27s%20algorithm%20gradually%20builds%20a,cost%20of%20its%20shortest%20path.)
//         // Just as Prim’s algorithm built on the foundation of BFS, Dijkstra’s algorithm can be seen as a variation of Prim’s algorithm.
//         // Dijkstra’s algorithm gradually builds a shortest paths tree on each iteration of the while loop. 
//         // But whereas Prim’s algorithm selects the next unvisited vertex based on edge weight (of each edge)
//         // alone, Dijkstra’s algorithm selects the next unvisited vertex based on the 
//         // sum total cost of its shortest path.

//         //Dijkstra's focus is on minimizing the weight from 1 vertex to all others (getting shortest paths to them, or to just one index)
//         //Prim's focus is minimizing total weight. (e.g.)
        
//         return dijkstraAlgo1(times, n, k);
//     }

//     int dijkstraAlgo1(int[][] times, int n, int k)
//     {
//         HashSet<int> visited =  new(n);
//         Dictionary<int, List<(int time, int node)>> neighbors = new();
//         buildNeighborsDict1(neighbors, times);
        
//         int maxTime = 0; //WE NEED maxTime, BECAUSE MAX OF THE MIN PATHS WILL BE THE MINIMUM AMOUNT OF TIME TO FOR ALL TO RECEIVE SIGNALS. //WRONG: int minTime = int.MaxValue;
//         PriorityQueue<(int time, int node), int> pq = new();
//         //k is the source:
//         pq.Enqueue((0, k), 0);// DEPRECATED:: //DEPRECATED::since we're looking for minTime, we put first node to int.MaxValue because otherwise minTime b
//         while(visited.Count!=n && pq.Count>0)
//         {
//             var cur = pq.Dequeue();
//             if(visited.Contains(cur.node)) //I was doing `!neighbors.ContainsKey(cur.node) || visited.Contains(cur.node)` BUT that makes no sense and would break the program BECAUSE THAT NODE IS VISITABLE, just that there's no nodes to visit from it. If we do this, we won't get the correct cost and we won't stop early from the visited.Count condition either. Better to do it before the for loop.
//                 continue;
//             visited.Add(cur.node);

//             if(maxTime < cur.time)
//                 maxTime = cur.time;

//             if(!neighbors.ContainsKey(cur.node))
//                 continue;

//             foreach(var nei in neighbors[cur.node])
//             {
//                 var pathTime = cur.time + nei.time;
//                 pq.Enqueue((pathTime, nei.node), pathTime);
//             }
//         }

//         return visited.Count == n ? maxTime : -1; 
//     }

//     void buildNeighborsDict1(Dictionary<int, List<(int time, int node)>> neighbors, int[][] times)
//     {
//         foreach(var time in times)
//         {
//             neighbors.TryAdd(time[0], new());
//             neighbors[time[0]].Add((time[2], time[1]));
//         }
//     }
// }



// # Solution from 24-04(April)-2026 [NuAttempt1] -  KINDA SUCKS IN FACT, EVEN LAST ACTU

// /// THINGS I COULD'VE DONE BETTER:
// // // Queue stores: Element = NodeId, Priority = Cumulative Time
// // PriorityQueue<int, int> minHeap = new();

// // // ... inside your loop ...
// // minHeap.TryDequeue(out int currNode, out int currTime);

// // // Enqueueing neighbors is now just basic math, no array allocations!
// // minHeap.Enqueue(nei.dst, currTime + nei.cost);

// public class Solution {

//     public const int NotFound = -1;
//     const int SrcIdx = 0;
//     const int DstIdx = 1;
//     const int CostIdx = 2;

//     public int NetworkDelayTime(int[][] times, int nodeCount, int startNode) { 
//         //TC = O(Elog(V));
//         //Aux. SC = O(V^2 + E) and E <= V^2
//         //   = O(V^2)
//         //  [for fully connected graphs, adjList has V^2 or rather more accurately V+E (which is still V^2!]
//         // [For the V part, it comes from the minHeap. Normally I should have only stored vertices, which makes it obvious,
//         //  but here it is also guaranteed from my  if(minTime.ContainsKey(nei.dst)) continue; optimization!]

//         //Took me 36 minutes to finish but I spent 6 minutes in start just trying some wrong stuff since I wasn't as familiar with Dijkstra 
//         // (or that it requires edge list) AND it was my first time in 1.5 years doing a Dijkstra problem though unfortunately 
//         // I won't have the time to practice it much more.
//         // Also, there were some syntax bugs and typos and copy paste errors here and there.
//         // However, for all this, it is also true I was pretty sleepy 20 minutes in. (still am, could go and sleep right away prolly.)
//         // ALSO: *IMPORTANT* I ALMOST FORGOT TO DO THIS ADD CUMULATIVE COST (curMinTime + neiEdge[costIdx]) for priority (in minHeap priority queue inside the main Dijkstra loop, not init!)
//         // Though tbf that was mostly because 1. I am sleepy, 2. I KNEW and PLANNED to do this, but I took a break 8 minutes into the problem (was on wrong path a bit), so I forgot after coming back.
//         // And even might have forgotten afterwards, who knows.
//         // ALSO, uses weird things like storing edge in minheap instead of nodes, but that's also due to reasons described at the very start of this comment block.
//         // ALSO, a small logical bug in the way I calculated costs.
//         //times[i] = src_i, dest_i, ti         
//         if(times == null || times.Length == 0)
//             return NotFound;  
//         PriorityQueue<int[], int> minHeap = new();
//         Dictionary<int, List<(int dst, int cost)>> adjList = GetAdjacencyList(times, minHeap, startNode);

//         return Dijkstra(adjList, minHeap, nodeCount, startNode);
//     }

//     int Dijkstra(Dictionary<int, List<(int dst, int cost)>> adjList,  PriorityQueue<int[], int> minHeap, int nodeCount, int startNode) {

        
//         // PriorityQueue<int[], int> minHeap = new(times.Select(edge => (edge, edge[CostIdx]))); //could also check for individual length of edge == 3
//         ////zLinq better for this^!

//         Dictionary<int, int> minTime = new();
//         minTime[startNode] = 0;
//         int lastTime = int.MinValue;

//         // var reachedNodes = 0;
//         while(minHeap.Count > 0 && minTime.Count != nodeCount) { //reachedNodes != nodeCount) {
//             var cur = minHeap.Dequeue();
//             var curMinTime = cur[CostIdx] + minTime[cur[SrcIdx]]; //all sources should be already pre-filled, so no need to see if it contains!
//             // minTime[cur[DstIdx]] = curMinTime; //WRONG CUZ IT WOULD OVERWRITE!!!
//             if(!minTime.TryAdd(cur[DstIdx], curMinTime)) //we already found a shorter path to this node!!
//                 continue;
//             lastTime = curMinTime;
//             if(!adjList.ContainsKey(cur[DstIdx]))
//                 continue;
//             foreach(var nei in adjList[cur[DstIdx]]) {
//                 if(minTime.ContainsKey(nei.dst)) //THIS OPTIMIZATION MIGHT BE NIFTY!!!
//                     continue;
//                 var neiEdge = new int[3];
//                 neiEdge[SrcIdx] = cur[DstIdx];
//                 neiEdge[DstIdx] = nei.dst;
//                 neiEdge[CostIdx] = nei.cost; // Why not curMinTime + nei.cost ?
                
//                 minHeap.Enqueue(neiEdge, curMinTime + neiEdge[CostIdx]); //Why only here? Because of how we calculate `curMinTime`!
//                 // *IMPORTANT* I ALMOST FORGOT TO DO THIS ADD CUMULATIVE COST (curMinTime + neiEdge[costIdx]) for priority
//                 // Though tbf that was mostly because 1. I am sleepy, 2. I KNEW and PLANNED to do this, but I took a break 8 minutes into the problem (was on wrong path a bit), so I forgot after coming back.
//                 // And even might have forgotten afterwards, who knows.
//             }
//         }

//         if(minTime.Count != nodeCount)
//             return NotFound;
//         else
//             return lastTime;
//     }


//     Dictionary<int, List<(int dst, int cost)>> GetAdjacencyList(int[][] edgeList, PriorityQueue<int[], int> minHeap, int startNode) {
//         // var adjList = new List<int>[nodeCount];
//         Dictionary<int, List<(int dst, int cost)>> adjList = new();
//         foreach(var edge in edgeList) {
//             if(edge[SrcIdx] == startNode) {
//                 minHeap.Enqueue(edge, edge[CostIdx]);
//             }
//             adjList.TryAdd(edge[SrcIdx], []);
//             adjList[edge[SrcIdx]].Add((edge[DstIdx], edge[CostIdx]));
//             //Could do: if negative cost, throw
//         }
//         return adjList;
//     }
// }
