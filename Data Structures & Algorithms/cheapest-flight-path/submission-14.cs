// # Last Actual Solution (Late November 2024):

// public class Solution {
//     //[VERY IMPORTANT] USE THE FOLLOWING RESOURCES:
//     // - [V. IMP!!] READ MY SOLUTION FOR THIS PROBLEM
//     // - [IMP!!] WATCH NEETCODE VIDEO FOR THIS PROBLEM
//     // - ["Bellman Ford Algorithm | Shortest path & Negative cycles | Graph Theory" by "WilliamFiset"](https://www.youtube.com/watch?v=lyw4FaxrwHg)
//     //      * Comment on the video : We need to run V-1 times because we run edges in random order so we could run from the vertex has positive infinity cost to another vertex also has positive infinity cost. So we could reduce time complexity if we run edges in an order that assures unvisited vertex will be visited from visited vertex, right?
//     //          + Author's reply: Only in the worst case does it take V-1 iterations for the Bellman-Ford algorithm to complete. Another stopping condition is when we're unable to relax an edge, this means we have reached the optimal solution early. In practice, this optimization works very well from tests I've conducted in the past. See here for an example: https://github.com/williamfiset/Algorithms/blob/master/src/main/java/com/williamfiset/algorithms/graphtheory/BellmanFordEdgeList.java#L42
//     //      * Another comment: To elaborate on why we do "V-1" iterations, it comes from the following lemma: "if the shortest path from the source to a node v ends with the edge u->v, and we already know the correct distance to u, and then we relax the edge u->v, we will find the correct distance to v". It is a pretty obvious lemma, if you think about it, but the correctness of Bellman-Ford, Dijkstra, and topological sort are all based on it. The consequence of this lemma is that, in order to find the correct distance to a node v, we need to relax all the edges in the shortest path from the source to v *IN ORDER*. Dijkstra and topological sort are efficient because we only relax the out-going edges from each node after we found the correct distance for that node, so we only need to relax the edges once. Unfortunately, the combination of cycles and negative edges makes it impossible to find a "good" order to relax the edges. Thus, Bellman-Ford just relaxes all the edges in an arbitrary order (this is one iteration of Bellman-Ford). In the first iteration, we find the correct distance for all the nodes whose shortest paths from the source have 1 edge. In the next iteration, we find the correct distances for all the nodes whose shortest paths from the source have 2 edges, and so on. If the shortest path with the most edges has k edges, we need k iterations of Bellman Ford. Of course, we do not know what "k" is in advance, but, since shortest paths never repeat nodes (assuming there are no negative cycles), what we know for sure is that any shortest path will have at most V-1 edges (in the case where it goes through every node). This is why V-1 iterations is ALWAYS enough, but often not necessary. If in one iteration of Bellman-Ford no relaxation yields any improvement, it means that we already found all shortest paths and we can finish.
//     // - https://stackoverflow.com/questions/12782431/relaxation-of-an-edge-in-dijkstras-algorithm
//     // - ['Bellman-Ford in 5 minutes — Step by step example' by 'Michael Sambol'](https://www.youtube.com/watch?v=obWXjtg0L64)
//     // - ['Bellman-Ford in 4 minutes — Theory' by 'Michael Sambol'](https://www.youtube.com/watch?v=9PHkk0UavIM)
//     // - ['Graph Algorithms' article on University of Washington website](https://courses.cs.washington.edu/courses/cse373/23au/lessons/graph-algorithms/) Good for general cases.
//     // - Time complexity of BF is generally O(V*E), but since here we only go from any node at most k+1 nodes farther away, it is O(K*E) in an ideal world where copying arrays doesn't take O(V). Also, since the graph could technically be fully connected, E<N^2 => O(K*E) == O(K*V^2)
//     // - While Dijkstra is more efficient generally, we should use Bellman Ford's algorithm when: 
//     //      * It is more efficient and easier for problems like this (where you need at most k steps). 
//     //      * When there are NEGATIVE WEIGHTS
//     //      * Can detect negative cycles and determine where they exist. (especially useful for finance in arbitrage)
//     //      * Doesn't use visited node to keep track, in fact each vertex is considered multiple times ?
//     //      * It goes through every single edge in the graph
//     //      * Note that BF uses edge list.

//     //[IMPORTANT] THIS IS A SPECIAL CASE BECAUSE WE HAVE TO CONSTRAINTS: (compared to normal dijkstra problem)
//     //  - CHEAPEST FLIGHTS (meaning SPT algo)
//     //  - LESS THAN K STOPS ( this and above means we use Bellman Ford)
//     //  - We can't use `visited` HashSet to avoid infinite loop here because
//     //      we might want to visit a node multiple times through different paths.
//     //  - We should use Bellman Ford's algorithm for this. 
//     //      * It is more efficient and easier for problems like this (where you need at most k steps). 
//     //      * It even supports negative weights.
//     public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k) {
//         //Since it says AT MOST k steps, it means we want the Shortest Path (by weight), and not an MST.

//         //ACTUALLY, We should USE Bellman Ford's algorithm for this. 
//         //      * It is more efficient and easier for problems like this. 
//         //      * It even supports negative weights.
//         // Bellman Ford Algorithm does V-1 iterations (where V is number of nodes/vertices)
//         return BellmanFordAlgo1(n, flights, src, dst, k);

//         // //I DID THIS WRONG AT FIRST BECAUSE I JUST USED THE NORMAL DIJKSTRA WITH JUST THE STOPS BEING PASSED.
//         // //FOR THIS APPROACH, I READ THROUGH THE NEETCODEIO SOLN, BUT I FELT PRETTY SLEEPY TOWARDS THE END SO I'M NOT SURE I FULLY UNDERSTAND.
//         // //I WILL WATCH THE NEETCODE VIDEO NOW, JUST TO MAKE SURE I UNDERSTAND.
//         // return Dijkstra1(n, flights, src, dst, k);
//     }



//     int BellmanFordAlgo1(int n, int[][] flights, int src, int dst, int k) //TC: O(n+(m∗k)) //SC: O(n) Where n n is the number of flights, m m is the number of edges and k k is the number of stops.
//     {
//         //Could probably make it a little bit more efficient using hashmaps and stuff, but for interviews this is fine.

//         //SC: O(V)
//         //Should've named it `costAtPrevLevel` maybe????
//         int[] costAtCurLevel = new int[n]; //Minimum Cost to get to the index node at current level. Level is the length of the chain of nodes/edges we traverse (=> level = number of nodes we can cover travel from source ).
//         Array.Fill(costAtCurLevel, int.MaxValue); //When level is 0 (chain of 0 edges allowed between any two nodes), we cannot reach any node except staying at the source node.
//         costAtCurLevel[src] = 0; //Cost of staying where we are == 0
        
//         //TC: O(K*(V+E)) where K can be at most n-2.
//         // Now, let's find cost at each i-th level. Note that we go upto k+1-th level because we k is number of stops/nodes between (src, dst). So, k+1th node would be dst and src is the 0th level/node. 
//         for(int i = 1; i<k+2; i++) //I assume we start from 1 (turns out can also do [0,k+1), we only really need to ensure the loop runs K+1 times. We don't even use i.). Because we already manually filled the cost array for 0-th level (only src node visited with 0 nodes used between getting from src node to itself)
//         {   
//             //costAtCurLevel is the (i-1)thLevelCost here.
//             int[] ithLevelCost = costAtCurLevel.ToArray(); //O(V) //Clones array //Neetcodeio soln does: (int[])prices.Clone(); // In short: Used to ensure the updates do not affect the decision-making for the current iteration.
//             int changes = 0; //Could also break out earlier than i==k+2 keeping track of if there were no changes.
//             foreach(var flt in flights) //O(E)
//             {
//                 var fltSrc = flt[0];
//                 if(costAtCurLevel[fltSrc] == int.MaxValue) //Skip nodes we don't know the cost of getting to from the i-1'th level. This is how we limit our level, because we want to only i+1th level nodes, which would mean nodes that are connected to nodes we reached on the (i-1)-th level.
//                     continue;
                    
//                 var fltDst = flt[1];
//                 var fltCst = flt[2];
//                 var totalCst = costAtCurLevel[fltSrc] + fltCst;
                
//                 if(totalCst < ithLevelCost[fltDst]) //IMPORTANT!! : I HAD `if(totalCst < costAtCurLevel[fltDst])` earlier, but that's wrong because if we had found a smaller solution than the current totalCst in via a prior flt, we would end up overwriting it! 
//                 {
//                     ithLevelCost[fltDst] = totalCst;    // The following comment is what I was thinking when I made the above mistake. I was dumb : //[DEPRECATED / INCORRECT] This is (the rest of) how we limit our level to the i-th level. If we modify the costAtCurLevel for a node we encounter in the future (of the outer loop) as a source, then setting it would break the logic we use to skip if the we never visited the fltSrc at the (i-1)th level. Because then the array we use to check that would have the cost from i-th level, and the calculation then would give us the i+1th level instead of the i-th level.
//                     changes++;
//                 }
//             }

//             //Could also break out earlier than i==k+2 keeping track of if there were no changes:
//             if(changes==0) break;
//             costAtCurLevel = ithLevelCost; //Now costAtCurLevel has the cost at the K-th level. Keep in mind we need cost at K+1-th level because we need to get to dst which would be k+1th node/stop if there were k stops between it and src (and src would be 0th stop)
//         }

//         return costAtCurLevel[dst] != int.MaxValue ? costAtCurLevel[dst] : -1; //-1 => Not found
//     }


//     //[IMPORTANT] THIS IS A SPECIAL CASE BECAUSE WE HAVE TO CONSTRAINTS: 
//     //  - CHEAPEST FLIGHTS (meaning dijkstra shortest path over this as weight)
//     //  - LESS THAN K STOPS (meaning you have to check more than one / just the 
//     //      lowest cost path)
//     //  - We can't use `visited` HashSet to avoid infinite loop here because
//     //      we might want to visit a node multiple times through different paths.
//     //  - We still want to keep it greedy via dijkstra, so minheap over cost stays.
//     //  - We can use a cost[{dstNode}][{numberOfStops}] array where for each node
//     //      we keep track of if we have been there with this number of stops before,
//     //      if not, we do as if it wasn't in visited. BUT if it isn't, we check if 
//     //      the current cost we reached it with was lesser than the one we reached 
//     //      it with previously (compare cur.cost to value of the cost array with appropriate indexes),
//     //      and if it is lesser, then we do as if it wasn't in visited. But, if it
//     //      is greater, we continue as if it was in visited and skip adding any of 
//     //      its neighbors to the priority queue.
//     //[NOTE] I WROTE THIS STUFF JUST BASED ON NEETCODEIO SOLN. PLEASE WATCH HIS VIDEO
//     //too. I know I will right now.
//     int Dijkstra1(int n, int[][] flights, int src, int dst, int k) //TC: O((n+m)∗k) //SC: O(n∗k) Where n n is the number of flights, m m is the number of edges and k k is the number of stops.
//     {
//         // JUST WATCH THE NEETCODE VIDEO (and read the comment next to the ocst array)

//         //Conditions imply no cycles.
//         // The fact that we have another constraint (for k) means...
        
//         var cost = new int[n][]; //READ FULLY: JUST CHECKED THE NEETCODEIO SOLN AND WATCHED NEETCODE VIDEO AND REALIZED WE NEED THIS INSTEAD OF VISITED TO ENSURE THAT WE CAN CHECK IF EACH NODE HAS BEEN VISITED AFTER THIS MANY STOPS (and with what cost)
//         List<List<(int cost, int dst)>> neighbors = new(n);
//         for(int i=0; i<n;i++) //O(n*k)
//         {
//             cost[i] = new int[k+2]; //+2 because first step is src and last is node, the STOPS we want are inbetween.
//             Array.Fill(cost[i], int.MaxValue);
//             neighbors.Add(new());
//         }

//         foreach(var flight in flights)
//         {
//             neighbors[flight[0]].Add((flight[2], flight[1]));
//         }

//         // HashSet<int> visited = new();
//         cost[src][0] = 0; //<=> cost from src to src at 0 steps is 0.
//         PriorityQueue<(int cost, int dst, int stops), int> minHeap = new();
//         minHeap.Enqueue((0, src, 0), 0);
//         while(minHeap.Count>0)
//         {
//             var cur = minHeap.Dequeue();
//             if(cur.dst == dst) //ORDER MATTERS! I think...
//                 return cur.cost;
//             // if(visited.Contains(cur.dst))
//             //     continue;
//             // visited.Add(cur.dst);
            
//             if(cur.stops == k+1) //order of this matters because dst is allowed to be k+1th because technically only the stops inbetween src and dst count. //Basically, we put this after checking whether current node is destination.
//                 continue;

//             // if(!neighbors.ContainsKey(cur.dst)) 
//             //     continue;

//             foreach(var nei in neighbors[cur.dst])
//             {
//                 var newCost = cur.cost+nei.cost;
//                 var newStops = cur.stops+1;
//                 if(cost[nei.dst][newStops] > newCost)
//                 {
//                     //this condition acts somewhat like visited set due to initially being 
//                     //set to int.MaxValue:
//                     // - Ensures we calculate shortest path from the neighbor nei at least once 
//                     //      for all different number of steps (<=k) we get to the nei with. 
//                     //      (the <= k condition is maintained by one of the if conditions above).
//                     // - Ensures we ONLY calculate shortest path for same number of steps at 
//                     //      nei again if the new cost is lower than previous one.
//                     cost[nei.dst][newStops] = newCost;
//                     minHeap.Enqueue((newCost, nei.dst, newStops), newCost);
//                 }
//             }
//         }

//         return -1;
//     }
// }






// # Solution from 5-4(Apr)-2026:

public class Solution 
{
    const int NotFound = -1;

    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k) 
    {
        // Cost minimaztion is handled by PriorityQueue/MinHeap
        // Stop limits are enforced using minStops hashmap and storing stops in the PQ too!

        GetAdjList(flights, out var adj);

        if (src == dst) return 0;
        if (!adj.ContainsKey(src)) return NotFound;

        // minHeap stores: (target_node, total_cost, stops_taken)
        PriorityQueue<(int to, int cost, int airports), int> minHeap = new();
        
        minHeap.Enqueue((src, 0, 0), 0); //Source airport does NOT count as a stop AND it costs us nothing to get there!
        
        Dictionary<int, int> minStops = new();

        while (minHeap.Count > 0) 
        {
            var cur = minHeap.Dequeue();

            if (cur.to == dst) return cur.cost;

            if (minStops.ContainsKey(cur.to) && cur.airports >= minStops[cur.to]) 
                continue; //Cycle + Stop limit!
                
            minStops[cur.to] = cur.airports;

            if (!adj.ContainsKey(cur.to)) continue;

            foreach (var next in adj[cur.to]) 
            {
                var newAirports = cur.airports + 1;
                var newCost = cur.cost + next.cost;
                if (newAirports <= k+1) // Last node does NOT count as a stop!
                {
                    minHeap.Enqueue((next.to, newCost, newAirports), newCost);
                }
            }
        }

        return NotFound;
    }

    public void GetAdjList(int[][] flights, out Dictionary<int, List<(int to, int cost)>> adj) 
    {
        adj = new();
        foreach (var flight in flights) 
        {
            if (!adj.ContainsKey(flight[0])) 
            {
                adj[flight[0]] = new List<(int to, int cost)>();
            }
            adj[flight[0]].Add((flight[1], flight[2]));
        }
    }
}