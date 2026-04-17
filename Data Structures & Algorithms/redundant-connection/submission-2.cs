public class Solution {
    IRedundantConnectionFinder soln;
    public int[] FindRedundantConnection(int[][] edges) {
        // # Last Actual Solution from November 2024:

        // //[WRONG] My first thoughts: Naieve solution would be doing a loop starting from the end of the array and for each edge we see if removing it removes the cycle (by doing a DFS from 0). 
        // //Checked NeetCode's Naieve optimal DFS solution, and its very different from the above idea. //Note that the solution is not practically efficient since new int[n] used for `visited` is an O(n) operation since it sets all values to default value of the thing. But then again, we do another O(x) 
        
        // //Actually, the first edge that adds the cycle is the last edge that removes the cycle (because there were no cycles before it).  [That's why my idea was not good]
        // //Adding one edge to a tree will always create exactly one cycle. Thus, the redundant edge is the one that closes this cycle.
        // //Watched the neetcode video (except the implementation part) and now I know the best solution would Union Findl

        // //READ THE QUESTION PROPERLY LMAO!! I DIDN'T READ n == numVertices == edges.Length, I kinda ended up doing it without understanding/paying_attention anyway though. 

        // //Union Find by Size using Disjoint Union Sets:
        // // !!! [~~IMPORTANT~~] HAD TO WATCH NeetCode's VIDEO TO SEE HOW TO SOLVE THIS PROBLEM because I thought UnionFind wouldn't work here but the union operator can also help find cycles like we do here !!!
        // soln = new UnionFind_BySize_WithPathCompression_1();

        // [Note] Watch


        // # Solution from 17 April 2026:
        soln = new NuAttempt1_UnionFind_BySize_WithPathCompression();
        


        return soln.FindRedundantConnection(edges);
    }
}


public interface IRedundantConnectionFinder
{
    int[] FindRedundantConnection(int[][] edges);
}

// # Last Actual Solution from November 2024:

public class UnionFind_BySize_WithPathCompression_1 : IRedundantConnectionFinder
{
    public int[] FindRedundantConnection(int[][] edges) 
    {
        DisjointUnionSets dsu = new(edges.Length); //TC: O(V), SC:O(V)

        foreach(var edge in edges) //O(E*ackerman(V))~=O(E)
        {
            if(!dsu.Union(edge[0], edge[1]))
                return edge;
        }

        throw new Exception("No cycle found.");
    }

    private class DisjointUnionSets
    {
        int[] parent;
        int[] size; //number of children + 1(for itself)

        public DisjointUnionSets(int numNodes)
        {
            //we use numNodes+1 so that we can have a 1-1 mapping from node to its size or parent. The 0th index is just an extra that's never used. (we even start the loops from 1)
            parent = new int[numNodes+1];
            size = new int[numNodes+1];
            parent[0] = size[0] = int.MinValue; //Never used, so I could've kept them as is, but I just decided to keep it a negative number (int.MinValue) for no reason.
            for(int i=1; i<numNodes+1; i++)
            {
                parent[i] = i;
                size[i] = 1;
            }
        }

        public bool Union(int u, int v) //Returns false if the members are already connected, which means the last node to induce a cycle will return false right away.
        {
            int rootToAttach = Find(u);
            int rootTarget = Find(v);

            if(rootToAttach == rootTarget)
                return false; //Already in the connected set.

            if(size[rootToAttach] > size[rootTarget])
            {
                int temp = rootTarget;
                rootTarget = rootToAttach;
                rootToAttach = temp;
            }

            parent[rootToAttach] = rootTarget;

            return true;
        }

        public int Find(int child) //Due to the nature of Path Compression (and since find is also called in union), the maximum height/depth of any tree representing a set is 2 (as explained in my solution to `Number of Connected Components In An Undirected Graph`)
        {
            if(child != parent[child]) //Root's parent is itself.
                parent[child] = Find(parent[child]); //Path compression/
            return parent[child];
        }

    }
}





// # Solution from 17 April 2026:

// ## Note:
// - If you wanted to have all candidates for removal, you'd collect them!
// - For just number of removable edges, keep count in UnionFind as {get; private set;} initialize with 0, increment on every failed union (before returning false!)
//
// ## Time Taken
// Approximately 14.5 minutes! (including complexity calculations!)
public class NuAttempt1_UnionFind_BySize_WithPathCompression : IRedundantConnectionFinder {
    public class UnionFind {
        //Aux. SC: O(V + V) = O(V)
        int[] parent;
        int[] size;

        public UnionFind(int vertexCount) // TC: O(V) where v is total vertices!
        {            
            // *IMPORTANT* Note:
            // NEED TO ACTUALLY READ THE PROBLEM BEFORE STARTING!
            // HERE, I MISSED THAT LABELS WENT FROM 1 to n INSTEAD OF 0 to n-1 LIKE USUAL!
            // FOUND OUT AND FIXED AFTER FINISHING THE PROBLEM!
            parent = new int[vertexCount+1];
            size = new int[vertexCount+1];

            for(int v = 1; v<=vertexCount; v++)
            {
                parent[v] = v;
                size[v] = 1;
            }
        }

        public int Find(int vertex) { //TC: O(InvAck(V)) where V == vertex
            if(parent[vertex] == vertex)
                return vertex;
            parent[vertex] = Find(parent[vertex]);
            return parent[vertex];
        }

        public bool Union(int v1, int v2) { //TC: O(InvAck(V)) where V == vertex [because it calls Find]
            var root1 = Find(v1);
            var root2 = Find(v2);

            if(root1 == root2)
                return false;
            
            var size1 = size[root1];
            var size2 = size[root2];

            if(size1 < size2) {
                parent[root1] = root2;
                size[root2] += size1;
            }
            else {
                parent[root2] = root1;
                size[root1] += size2;
            }

            return true;
        }
    } 
    // ^ Took 8.5 minutes to write UnionFind and its complexities (inside the class itself!) 
    // + ~2 minutes fixing the issue I created from not reading that labels went from 1 to n instead of 0 to n like they did until this problem! (more details in the comment about it!)


    //Aux. SC = O(V)
    public int[] FindRedundantConnection(int[][] edges) {
        //given that n (vertex count) == edges.Length (but wouldn't that)
        int vertexCount = edges.Length;
        
        int[] lastRemoveableEdge = null;

        var dsu = new UnionFind(vertexCount); //TC = O(V)
        foreach(var edge in edges) { //TC=O(E) E<= V^2
            if(!dsu.Union(edge[0],edge[1]))
                lastRemoveableEdge = edge;
        }

        return lastRemoveableEdge;
    } 
    // ^ Took 4 minutes for writing this. 
}
