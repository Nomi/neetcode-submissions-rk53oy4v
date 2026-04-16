public class Solution {
    IGraphValidTree solver;
    public bool ValidTree(int n, int[][] edges) {
        // # Last Actual Solution from November 2024:

        // //Initial thoughts:
        // //Trees are simply Connected and Acyclic graphs.
        
        // // CANNOT use Kahn's/Topological_Sort algorithm here, even though it might be deceptively similar to Course Scheduler in terms of how the input is given.


        // //WATCHED NEETCODE VIDEO!!!!
        // //Also, BFS seems impossible (or unneccessarily complex) for this due to the need to pass parent/previous node


        // //SOLUTION:
        // solver = new DfsAttempt1(); //HAD TO WATCH NEETCOOE VIDEO.

        // # Solution from 16 April 2026:
        solver = new NuAttempt1_Dsu_BySize_PathCompression();

        return solver.ValidTree(n, edges);
    }
}


public interface IGraphValidTree
{
    bool ValidTree(int n, int[][] edges);
}

// # Last Actual Solution from November 2024:
public class DfsAttempt1 : IGraphValidTree
{
    HashSet<int> visited;

    public bool ValidTree(int n, int[][] edges)
    {
        //As commented previously, I just need to check if graph is CONNECTED AND ACYCLIC!

        visited = new();

        var nodeToNeighbor = new List<List<int>>(); //SC: O(V+E) where E is the edge count! (because we have a list edges for each vertex and each edge obviously appears only once (twice here because undirected but it doesn't really matter here))

        for(int i=0; i<n;i++) //O(V)
        {
            nodeToNeighbor.Add(new());
        }

        for(int i=0; i<edges.Length; i++) //TC: O(E) where E is the edge count!
        {
            nodeToNeighbor[edges[i][0]].Add(edges[i][1]);
            nodeToNeighbor[edges[i][1]].Add(edges[i][0]);
        }

        bool hasCycle = !checkCyclesAndBuildSetofNodesReachableFromCurDfs(0, -1, nodeToNeighbor); //TC: O(V) because we go through every node once
        // Console.WriteLine(hasCycle);
        if(hasCycle)
            return false;
    
        Console.WriteLine(visited.Count);

        return visited.Count == n;
    }

    public bool checkCyclesAndBuildSetofNodesReachableFromCurDfs(int cur, int parent, List<List<int>> nodeToNeighbor)
    {

        //CAUTION! keeping the visited set lines (checking and adding) inside the loop right after `if child==parent` condition by mistake initially, but it broke the whole thing BECAUSE for nodes with no other connection than its parent(because undirected), it would never get added to the visited set.
        if(visited.Contains(cur))
            return false;
        visited.Add(cur);
        foreach(var child in nodeToNeighbor[cur])
        {
            if(child==parent) //since it is undirected, we use this to prevent using the edge we got here from again so that we don't get a false-positive cycle.
                continue;
            if(!checkCyclesAndBuildSetofNodesReachableFromCurDfs(child, cur, nodeToNeighbor))
                return false;
        }
        return true;
    }

}


// # Solution from 16 April 2026:

// ## Time Taken:
// - 17 minutes to write `UnionFind` (DSU, i.e. Disjoin Set Union).
// - 3 Minutes to complete the provided `ValidTree` function
// - 2 minutes to add the missing instantiation of the UnionFind lol (I was calling Union like a function on this class :P) and fixing some other minor syntax stuff [found it by NeetCode first compile]
// - Forgot how long it took me to calculate complexities, but let's say 6 minutes?
// = 28-ish minutes
//
// ## Complexities:
// Let V =  number of nodes/vertices, E == number of edges, BUT we discard all cases where E != V-1.
// ### Time:
//      - Find(): TC = O(InverseAckerman(V)) ~= O(1) for all realistically possible V values (inverse ackerman for these is <5).
//      - Union(): Same as above ^. [union calls Find twice]
//      - ValidTree(): TC = O((V-1)*O(InvAck(V))) ~= O(V);
// ### Space:
//      - UnionFind: Aux. SC = O(2 * V) = O(V);
//      - ValidTree: Aux. SC = O(V) [because it uses UnionFind]

public class NuAttempt1_Dsu_BySize_PathCompression : IGraphValidTree{
    // a tree is a CONNECTED DAG (connected directed acyclic graph) with each vertex/node having at most 2 children.
    // A tree with `n` nodes MUST HAVE `n-1` edges EXACTLY, 
    // (*IMPORTANT* I didn't think of the connectedness AND edge count until checking NC.io solution mentioning those!!!)
    // Here, we are given an undirected graph, so I assume we're supposed to treat each edge as a child.

    public class UnionFind { // Path Compression (Lazy i.e. in Find) + Union by Size!
        public int[] parent; //representative of the connected component of a node / root / parent
        public int[] size; //number of nodes in connected component of a representative/root

        public UnionFind(int nodeCount)
        {
            parent = new int[nodeCount];
            size = new int[nodeCount];
            for(int i=0; i<nodeCount; i++)
            {
                parent[i] = i; //initially, the representative of each node is the node itself!
                size[i] = 1;
            }
        }

        public int Find(int node) //recursive, with path compression
        {
            if(parent[node] == node) //the root/representative of this connected component (in our UnionFind)
                return node;
            
            //Path compression: (Optimization!)
            parent[node] = Find(parent[node]);

            return parent[node];
        }

        public bool Union(int a, int b) //Union by size //returns False if already part of the same connected component!
        {
            var repA = Find(parent[a]);
            var repB = Find(parent[b]);

            if(repA == repB) //already in the same connected components!
                return false;
            
            //By size:
            int sizeRepA = size[repA], sizeRepB = size[repB];
            if(sizeRepA < sizeRepB)
            {
                parent[repA] = parent[repB];
                size[repB] += sizeRepA;
            }                       
            else
            {
                parent[repB] = parent[repA];
                size[repA] += sizeRepB;
            }

            return true;
        }
        
    }

    public bool ValidTree(int n, int[][] edges) {
        if(edges?.Length != n-1) //this checks for 1. < n-1: all edges COULD be connected (unless there's a cycle!) 2. > n-1: due to pigeon hole principle, there has to be extra edge(s). [there can only EXACTLY n-1 edges in a tree!]
            return false;

        // All we need to check now is that there is NO cycle! (i.e. all the nodes are connected with these n-1 edges!)
        UnionFind dsu = new(n);
        foreach(var edge in edges)
        {
            //could validate edge nullability and count here!
            if(!dsu.Union(edge[0],edge[1])) //i.e. there's a cycle!
                return false;
        }

        return true;
    }
}
