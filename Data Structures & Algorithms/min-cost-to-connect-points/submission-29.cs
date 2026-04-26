
public class Solution {
    IMinCostToConnectPointsCalculator soln;
    public int MinCostConnectPoints(int[][] points) { //Easy once you know how to do it.
        soln = new 
        
        // # Solution from 26-04(April)-2024:
            NuAttempt1_Prim


        // # LAST ACTUAL SOLUTION: [From 30 - 11(November) - 2024]

        /*
        //[DEPRECATED] Basically shortest path in an undirected AND weighted graph problem, so I'm thinking Djikstra's algorithm (I remember a little bit about it from a video I watched a long time ago)
        //OKAY, IT MIGHT NOT BE THE CORRECT APPROACH. AT LEAST Neetcodeio solns don't use it.
        
        //OKAY, I WAS DUMB, DJIKSTRA IS LOWEST_COST PATH FROM A NODE TO ANOTHER NODE (or all nodes) IN DIRECTED and WEIGHTED GRAPHS 
        //BUT IT DOESN'T ACCOUNT FOR HAVING TO GO THROUGH ALL NODES!!
        
        // This problem is ACTUALLY more like finding the Minimum Spanning Tree (MST), 

        // Watched the NeetCode Advanced Algorithms course's video about Prim's and Kruskal's (only a little for this one, for now) algorithms for finding MST (without the code examples)
        
        // NOTE: Djikstra's, Kruskal's, and Prim's algorithms are greedy algorithms. (I somehow hadn't made the connection yet)

        // What is an MST?
        //      - Recall that Trees are Acyclical, Connected, & Undirected graphs (well, technically they are directed from parent to child, but that's besides the point).
        //      - Recall that since a tree are connected and acyclical, for a tree of N nodes, there are N-1 edges (because to connect N points, you need N-1 lines. If you use N lines, you end up with a cycle (e.g. a square has 4 vertices and 4 edges, or two points with two edges between them, or one point with one edge to itself, etc.)).
        //      - MST is the smallest subset of edges from a graph that still connects all of its nodes but also forms a Tree (Acyclical, Connected, & Undirected Graph as discussed in the point above).
        //      - If the edges are weighted, then we minimize the total cost by taking a subset of the edges such that the cost is minimized while still satisfying the other conditions of the MST.
        //          For unweighted, we just assume the weights to be 1 for all edges.
        //      - There CAN be multiple valid solutions/MSTs (with same cost). We just return 1 of them, like in shortest path algorithms.
        //      - The result will be one of the valid MSTs but in the form of an edge list.
        //      - Unlike for Shortest path (where we start from source node), for MST it doesn't matter which node we start from (because all node need to be included anyway).
        //      - For some Trees (like binary trees) we usually ignore the fact that they're directed (only parent has pointers to its children), but here it is more strict than that. 
        //          Meaning the edges really will not have any direction.

        // Since we want the MST, we can use the following algorithms (read the MST section above first):
        // NOTE: Djikstra and Prim's are both based on BFS but use PriorityQueue/MinHeap instead of Queue.
        // NOTE2: Read the note about Djikstra vs Prim in my solution for the 'Network Delay Time' problem.
        //  * Prim's Algorithm: [for Undirected & Connected Graphs] 
        //      - Prim's algorithm fails for directed graphs.
        //      - We use a `visited` HashSet because we don't want to visit a node more than once,
        //          because that would lead to a cycle.
        //      - We also use a MinHeap<weight, n1, n2>, which sorts based on weight/cost of the edges
        //          because we want to pop/dequeue the minimum weight/cost edges first. (where n1 and n2 are the nodes for the edges)
        //      - We start at ANY node and add its edges to the MinHeap AND add the node to the Visited set.
        //      - Then keep iterating and for the current edge for each iteration, use the n2 as the node
        //          and take all neighbors of the current node and add its edges them to the MinHeap (alongside their total weights (combined with how much it took get there))
        //      - { At this point, the algorithm works almost exactly like Djikstra's algorithm. (Side note: I watched the video about the algorithm from NeetCode's Advanced Graphs course but the problem for it on NC150 comes after this one (without the code example).) }
        //      - { Also, we could run the algorithm to keep the loop running until the MinHeap is empty, but we could also add a different condition to continue the algorithm
        //          to avoid extra processing once we're done finding it. For example:
        //              1. Run until MinHeap empty (as we discussed above, this is a little inefficient but it works)
        //              2. Run while Visited.Count < numOfNodes (if we know what the numOfNodes is),
        //              3. While number/count of edges in list of edges in our results is < n-1 (as we discussed above, it being n-1 means we're done).
        //          Just remember that visited.Count is number of nodes processed thus far, and result.Count is number of edges thus far.
        //          To keep it simple, we will run until MinHeap is empty. }
        //      - By the way, while the edges are undirected, the order we put n1 and n2 in the MinHeap
        //          matters because that's the direction of our traversal (not the edge) [where n1 is where we are at, and n2 is where we will go] 
        //          and reversing it suddenly would just lead to going back to previous nodes.
        //      - If there are equal weights at some point, we can pop any of those and still get a valid MST (no cycle and minimum cost) 
        //          because as discussed earlier, there can be many valid solution, and this will be one of them.
        //      - Since the algorithm is so similar to Dikstra's alogirthm, the complexities are the same:
        //          * TC: O(E*log2(E)) where E <= V^2 (E==V^2 when every node is connected to every other node)
        //              [Also, O(E*log2(v^2)) == O(E*2*log2(v)) == O(E*log2(v))]
        //              (Note: E*log2(E) comes from adding AT MOST E edges to the MinHeap)
        //          * SC: O(E) where E<=V^2 (E==V^2 when every node is connected to every other node)
        //              [because we only store Edges]
        //      - Watch the video about this algorithm from the NeetCode Advanced Algorithms 
        //          course to find WHY this algorithm works and get more details about it.
        //      - Note, MinHeap is also called `Frontier`? Also, in some rare cases, due to our input's properties (and further processing), we might not end up needing the MinHeap.
        //
        //              
        //  * Kruskal's Algorithm:
        //      - Just another way to find the MST of a connected graph.
        //      - Maybe if Prim's algorithm is too confusing for you, you might prefer Kruskal's algorithm.
        //      - In fact, in NeetCode's opinion, it is conceptually it is a much easier algorithm to understand.
        //      - BUT, coding it might actually be more difficult/complicated because it uses the Union Find datastructure (which you might also need to implement yourself) and the implementation even if Union Find were to be given might be at least somewhat difficult/complicated.
        //      - Due to above reason, I stopped learning this, at least for now, because I have limited time and clearly this doesn't seem to be worth it given the tight schedule for my upcoming interview.
        //      - Before fully stopping, I did watch the video at least upto the point where the basic logic is and why it works is explained and makes sense (without code) [~6:45 mark] (e.g. how to avoid introducing cycles by using Find to check if the edge we're using connects nodes in the same component), but not worth typing it out if I am not fully commited to practicing/using it yet.
        //      - Oh, and Neecode's video about this problem also mentions that Prim's algo is also usualy more efficient.
        */

        // PrimsAlgo_1



        ();
        return soln.MinCostConnectPoints(points);
    }

}


public interface IMinCostToConnectPointsCalculator
{
    int MinCostConnectPoints(int[][] points);
}

// # LAST ACTUAL SOLUTION: [From 30 - 11(November) - 2024]

//## Problem statement Breakdown:
//GIVEN: An array of DISTINCT points on a 2D graph
//RESULT: Minimum cost to connect points
//INPUT: int[][] points, representing the graph, where points[i] == [x,y]
//OUTPUT: int
//TO DO: 
//  - Calculate ways to connect all the points such that:
//      - There's EXACTLY 1 path between 2 points.
//      - While keeping the manhattan distance (cost) minimum.
//FACTS: 
//  - cost == |x1 - x2| + |y1 - y2| (maybe this would need to be asked as a clarification in an interview?)
//CLARIFICATIONS:
// - Should've asked if it is guaranteed there will be a solution, but as from what I can see, there must be a solution (since all nodes are connected to every other node)
//CONSTRAINTS:
// * 1 <= points.length <= 1000
// * -1000 <= xi, yi <= 1000
//EXAMPLES:
// - Input: points = [[0,0],[2,2],[3,3],[2,4],[4,2]]
// - Output: 10


public class PrimsAlgo_1 : IMinCostToConnectPointsCalculator
{
    int Abs(int num) => (int) Math.Abs(num);
    int Cost(int x1, int y1, int x2, int y2) => Abs(x1-x2) + Abs(y1-y2); //returns ManhattanCost

    public int MinCostConnectPoints(int[][] points) //Note: Watched NC video for this and somehow I ended up doing this while sneaking peeks at the NC150 solution.
    {
        //[IMPORTANT OBSERVATION!] Since any point can be conneccted to any other point, we can consider it as a FULLY CONNECTED graph.  //ALSO IT IS UNDIRECTED!!!
        //      => We will have N^2 edges in total
        //We want to find a MST of this graph because that will be (one of) our solution.

        //Step 1- Build Adjacency List        
        var neighbors = new Dictionary<int, List<(int cost, int ptIdx)>>(points.Length);

        for(int i=0; i<points.Length; i++)
        {
            int x1 = points[i][0], y1 = points[i][1];
            neighbors.TryAdd(i, new());
            for(int j = i+1 ; j < points.Length; j++) //[IMPORTANT!!] [ALMOST_FORGOT!!] `j=i+1` because we would have paired the current i with all the nodes before it (and we don't pair it with itself). Obviously!
            {
                int cost = Cost(x1, y1, points[j][0], points[j][1]);
                neighbors[i].Add((cost, j));
                
                neighbors.TryAdd(j, new());
                neighbors[j].Add((cost, i));
                //We add both because it is undirected (or technically one undirected edge is two directed edges just in opposite direction)
            }

        }

        
        //Step 2- Use Prim's algorithm (standard) to get MST (apparently there's an optimal version which is more efficient. Check Neetcodeio solns. I'm skipping them for now.)
        int minCost = 0;
        HashSet<int> visited = new(points.Length); //For avoiding nodes we already visited..

        PriorityQueue<(int cost, int ptIdx), int> pq = new(); //For Prim's algo.
        pq.Enqueue((0, 0), 0); //since we can start at any node in Prim's algo, let's start at the first point.

        while(visited.Count < points.Length && pq.Count>0)
        {
            var cur = pq.Dequeue();

            if(visited.Contains(cur.ptIdx))
                continue; //skip
            visited.Add(cur.ptIdx);

            minCost += cur.cost;
            //
            foreach(var neighbor in neighbors[cur.ptIdx])  //If it were not a fully connected graph, it might have helped to have this loop inside an `if (adj.ContainsKey(i))` condition to avoid sorting and going through empty nodes.
            {
                pq.Enqueue(neighbor, neighbor.cost); //if this were djikstra, we would add total cost added to get to here (except last edge) and then current cost (last edge).
            }
        }

        return visited.Count == points.Length ? minCost : -1;
    }
}





// # Solution from 26-04(April)-2024:

public class NuAttempt1_Prim : IMinCostToConnectPointsCalculator {
    record Point(int x, int y);

    int MhDist(int[] p1, int[]p2) 
        => Math.Abs(p1[0]-p2[0]) + Math.Abs(p1[1]-p2[1]);


    //*IMPORTANT* : 
    // I still don't understand this fully, I only did the `MinCostConnectPoints_NonOptimal` myself, 
    // and I think I'd stick with that for interview and knowing the concept of this optimization for follow-up explanation!
    public int MinCostConnectPoints(int[][] points) { 
        //Prim's Algo
        int V = points.Length;
        int E = V-1; 
        // For Graphs without cycles [i.e. Forest], E <= V-1
        // For Connected Graphs withouy Cycles [i.e. Trees], E == V-1 



        bool[] visit = new bool[V];
        int[] dist = new int[V];
        Array.Fill(dist, int.MaxValue);
        

        int edgesAddedToMst = 0;
        int totalDist = 0;
        int cur = 0;
        while(edgesAddedToMst < E) {
            visit[cur] = true;
            int nxt = -1;

            //In fully connected graph, a full loop is better than minheap-with-adj (this is O(V^2), that is O(V^2 * log2(V)))
            for(int i = 0; i < V; i++) {
                if(visit[i]) //already visited / found a min edge for this and connected it!
                    continue;

                dist[i] = Math.Min(MhDist(points[cur], points[i]), dist[i]); //*IMPORTANT* part (need to store min of either cuz the shortest MST variant can have branches like Y instead of just being a linear path!)

                if(nxt == -1 || dist[i] < dist[nxt]) {
                    nxt = i;
                }
            }

            totalDist += dist[nxt];
            cur = nxt;
            edgesAddedToMst++;
        }

        return totalDist;
    }

    public int MinCostConnectPoints_NonOptimal(int[][] points) {
        //Prim's Algo

        GetAdj(points, out var adj);

        Dictionary<Point, int> minCosts = new();
        PriorityQueue<(Point pt, int cost), int> mh = new();
        mh.Enqueue((adj.Keys.First(), 0), 0); //start from any node in MST
        // minCosts.Add(adj.Keys.First(), 0);

        var totalCost = 0;
        while(mh.Count > 0 && minCosts.Count != adj.Count) {
            var cur = mh.Dequeue();

            if(!minCosts.TryAdd(cur.pt, cur.cost)) { //already found a smaller cost for this node
                continue;
            }
            totalCost += cur.cost;

            if(!adj.ContainsKey(cur.pt)) { //optional optimization
                continue;
            }

            foreach(var nei in adj[cur.pt]) {
                mh.Enqueue(nei, nei.cost);
            }
        }

        return totalCost;
    }

    void GetAdj(int[][] points, out Dictionary<Point, List<(Point pt, int cost)>> adj) {
        adj = new();

        for(int i = 0; i < points.Length; i++) {
            var src = new Point(points[i][0], points[i][1]);
            for(int j = 0; j < points.Length; j++) {
                // *IMPORTANT* THE FOLLOWING BREAKS FOR INPUT OF LENGTH 1 (points = [[0,0]])        [Only caught due to failing test case!]
                // if(i==j)
                //     continue;
                
                var dest = new Point(points[j][0], points[j][1]);
                var cost = MhDist(points[i], points[j]);
                
                adj.TryAdd(src, new());
                adj[src].Add((dest, cost));
            }
        }
    }
}