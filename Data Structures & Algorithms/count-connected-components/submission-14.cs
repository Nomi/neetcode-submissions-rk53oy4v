public class Solution {
    public int CountComponents(int n, int[][] edges) {
        IComponentsCounter soln;

        // # Last Actual Solutions from November 2024:

        //Did it by myself since I had done a similar problem Graph Valid Tree an hour or two ago. (though that one took me a long while, this one was done in ~20 minutes)
        //Refer to Graph Valid Tree if needed.
        
        //soln = new Dfs1();

        // DFS is actually perfectly fine for this, but if you wanted to, 
        // for Union Find using Disjoin Union Set, watch Union Find in 5 minutes by Potato Coders on YouTube
        // According to a comment on the video: He does forget to mention that you have to union the shorter tree into the longer tree, otherwise the trees is in a set of n elements can be up to n elements long (just a straight chain, instead of branching) and you don't get log(n) complexity.
        // To get O(log2(n)) complexity, you need to use union by rank or size.
        // There's also Path Compression to further optimize if we don't need direct parents
        // ALSO, MUST WATCH if you want to do Union Find: William Fisset's videos on Union Find `Union Find - Union and Find Operations` and `Union Find Path Compression`
        // ULTRA MUST WATCH: TakeUForward's VIDEO ON UnionFind (G-46)
        // Also, watch Neetcode's video on this problem to see how it applies here. I plan to watch it before solving.
        
        //soln = new UnionFind_DisjointUnionSet_UnionBySize_WithPathCompression_1();


        // # Solution from 17 April 2024:
        soln = new NuAttempt1_UnionFindDSU_UnionBySize_WithPathCompression();
                

        return soln.CountComponents(n, edges);
    }
}


public interface IComponentsCounter
{
    int CountComponents(int n, int[][] edges);
}

// # Last Actual Solutions from November 2024:
public class Dfs1 : IComponentsCounter
{
    public int CountComponents(int n, int[][] edges)
    {
        //1. Make adjacency list:
        List<List<int>> nodeEdges = new(n); //SC: O(V+E) [V==n]  because there are V List<ints>, and the sum of all the nodes in those lists is E (because the lists contain the vertex on the other end of the edges from the V indices)
        for(int i=0; i<n; i++) //O(n)==O(V)
        {
            nodeEdges.Add(new());
        }

        foreach(var edge in edges)
        {
            nodeEdges[edge[0]].Add(edge[1]);
            nodeEdges[edge[1]].Add(edge[0]);
        }

        //2. Graph traversal to mark connected components:

        int numConnectedComponents = 0;
        HashSet<int> visited = new(n); //To track nodes already visited (helps with cycle detection and also checking if a node is a part of a connected component discovered before)

        for(int node = 0; node<n; node++) //TC (including DFS): O(V+E) where V==n, because we only visit each node once and the number of other nodes we visit from each node sums up to the total number of edges because we only traverse each edge only once using the hashset's power (obvious / by definition).
        {
            if(visited.Contains(node))
                continue;
            numConnectedComponents++;
            dfsMarkThisAndConnectedNodesAsVisited(node, -1, visited, nodeEdges); //assuming we can use -1 as a dummy parent node for first level of recursion.
        }
        return numConnectedComponents;
    }

    void dfsMarkThisAndConnectedNodesAsVisited(int curNode, int parentNode, HashSet<int> visited, List<List<int>> nodeEdges) //O(V+E) because we go through each node at most once and the total sum of all paths from one node to another (edges) will sum up to E because 1 edge only occurs between 1 pair of vertices. (I think I phrased it shitty, but give me a break. I have a headache.)
    {
        if(visited.Contains(curNode))
            return; //We don't need to process this branch anymore as it is a cycle (it can't be something we visited for a different connected component exactly because then that and this connected component would be connected and would actually be only one connected component, but that's not true)
        visited.Add(curNode);
        foreach(int childNode in nodeEdges[curNode])
        {
            if(childNode == parentNode) //Since we came from there, we don't need to process it again. In fact that would lead to a false-positive detection of a cycle.
                continue;
            dfsMarkThisAndConnectedNodesAsVisited(childNode, curNode, visited, nodeEdges);
        }
    }
}

public class UnionFind_DisjointUnionSet_UnionBySize_WithPathCompression_1 : IComponentsCounter
{
    //Very useful when the graph is changing (e.g. executing Find somewhere in between of Union calls)
    //TakeUForward helped a lot! And for some of how to code it (especially doing the find function iteratively instead of recursively, though now that I think about it, it wouldn't really affect the performance much in this case), I took a look at the soln on NeetCodeIo even though it is by rank.
    public int CountComponents(int n, int[][] edges) //TC: O(V + E*α(n)) where α(n) is the inverse Ackerman function, which is less than 5 for any practical input size n. //See also: https://tarunjain07.medium.com/union-find-disjoint-set-union-dsu-notes-24f3e228858d#30a0 AND https://stackoverflow.com/questions/6342967/why-is-the-ackermann-function-related-to-the-amortized-complexity-of-union-find
    {
        DSU dsu = new(n); //TC: O(V)
        
        int numRoots = n; //at the start of DSU, everything is a root.

        foreach(var edge in edges) //O(E*α(V)) where α is <= 5 for any 'reasonable' input
        {
            if(dsu.Union(edge[0], edge[1]))
                numRoots--;
        }
        return numRoots;
        // I DIDN'T THINK OF THE ABOVE SOLUTION! WAS GOING TO DO THE FOLLOWING CONVOLUTED SOLUTION UNTIL I SAW THIS PART OF NEETCODEIO SOLN!!
        //WELL, technically I did think of keeping count like the above approach, but in the DSU class itself and being the idiot I am, I decided against it somehow. 

        // HashSet<int> connectedComponentRoots = new();

        // for(int i=0; i<n; i++ )//~=O(V) where α is <= 5 for any 'reasonable' input)
        // {
                //I had planned to do find here lol
        // }

        // for(int )
    }
    private class DSU
    {
        //SC: O(V)
        int[] nodeToParent;
        int[] numChildren; //Usually called `Size` in DSU

        public DSU(int numNodes)//TC: O(V) (in fact, it is amortized(exactly) v, so α(v))
        {
            numChildren = new int[numNodes]; //C# initializes them all to 0 and that's the value we want because all nodes are ROOT nodes (no parents) with no children before edges are added
            nodeToParent = new int[numNodes];
            for(int node=0; node<numNodes; node++)//TC: O(V) //The parent of each ROOT node is the node itself, and at the beginning everything is a root (no parents) since no union has been called
            {
                nodeToParent[node] = node;
            }
        }

        //TC: α(V)
        public bool Union(int u, int v) //returns false if already in same component, otherwise returns true after uniting them
        {
            int uRoot = Find(u); //usually labeled `pu` in DSU
            int vRoot = Find(v); //usually labeled `pv` in DSU
            if( uRoot == vRoot )
                return false; //already the same set/connected_component/Union
            
            int parent = uRoot, child = vRoot; //I WAS USING u and v HERE EARLIER AND THAT'S WRONG!! (and breaks tings!!)
            if(numChildren[vRoot]>numChildren[uRoot])
            {
                parent = vRoot;
                child = uRoot;
            }
            nodeToParent[child] = parent;
            numChildren[parent] += 1+numChildren[child];

            return true;
        }

        //TC: ~O(1)
        public int Find(int curNode) //returns the root node, which acts as the representative for its current set/connected_component
        {
            //Notice that due to path compression, we will NEVER have a tree with depth bigger than 3 (because all nodes are attached to root making length 2, and if we union that with another node which has bigger `size` (numChildren), then the resulting graph/tree will have depth 3)
            //Due to the above comment, I could've done it via recursion and it would be fine performance wise and from any other aspect. In fact, it would be easier to come up with the iterative approach (had to check neetcodeio soln to see how to do it, even though they were doing by rank, some parts (including this) remained same or similar)
            while(curNode != nodeToParent[curNode]) //Parent of a node being itself means it is a root
            {
                nodeToParent[curNode] = nodeToParent[nodeToParent[curNode]]; //This doesn't break because the parent of the root is the root itself. //Also, works due to the depth being <= 3.
                curNode = nodeToParent[curNode];
            }

            return curNode; //due to while loop condition, it is the root.
        }
    }
}


// # Solution from 17 April 2024:

// ## Notes:
// - For more comments/details on UnionFind/DSU check the `NuAttempt1_...` from the previous problem (Graph Valid Tree).
//
// ## Time Taken:
// - 10 minutes to write UnionFind
// - 2 minutes to write Count Components
// - ~1 minute to fix some small syntax errors and typos (pointed out by first NeetCode run/compile)
// = 13 minutes (for code)
// BUT WAIT, this does NOT INCLUDE the extra minutes it took me to fix the bug where I was using a and b on the left side of swaps instead of rootA and rootb!
// WHICH, was detected because a NeetCode automated test failed AND then I asked Gemini AI to help me find the exact problem!
//
// Also: Not including complexity calculation, but it actually took a while since I was chilling, but let's say 5 min (because I was writing explanations)
//
// ## Complexities:
// Note that E is at most V^2 (edge from every node to another, unless duplicate edges are allowed)
// ### Time:
//  Construction: O(V)
//  Find: TC = O(InverseAckerman(V)), which for all realistically possible graphs is <5. => TC ~=O(1)
//  Union: Same as Find [i.e. O(InverseAckerman(V))] since the components of its code NOT calling Find are pretty much linear.
//  CountComponents: O(V + E*O(InvAck(V))) ~= O(V + E)
// ### Space:
// Aux. SC = O(V)

public class NuAttempt1_UnionFindDSU_UnionBySize_WithPathCompression : IComponentsCounter {
    public class UnionFind { //A.K.A. Disjoint Set Union (DSU)
        int[] parent;
        int[] size;
        
        public int ConnectedComponents {get; private set;}
        
        public UnionFind(int nodeCount) {
            
            ConnectedComponents = nodeCount;
            
            parent = new int[nodeCount];
            size = new int[nodeCount];

            for(int i=0; i<nodeCount; i++) {
                parent[i] = i;
                size[i] = 1;
            }
        }

        public int Find(int node) {
            //could validate bounds
            if(node == parent[node])
                return node;
            
            var root = Find(parent[node]);
            parent[node] = root;

            return root;
        }

        public bool Union(int a, int b) {
            int rootA = Find(a), rootB = Find(b);
            if(rootA == rootB) //already in same connected component! (Redundant Edge A.K.A. Cycle!)
                return false;
            
            int sizeRootA = size[rootA], sizeRootB = size[rootB];
            
            if(sizeRootA < sizeRootB) {
                parent[rootA] = rootB; //WAS WRONG: parent[a] = rootB;
                size[rootB] += sizeRootA; //WAS WRONG: size[b] += sizeRootA
            }
            else {
                parent[rootB] = rootA; //WAS WRONG: parent[b] = rootA;
                size[rootA] += sizeRootB; //WAS WRONG: size[a] += sizeRootB; 
            }

            ConnectedComponents--;

            return true;
        }
    }

    public int CountComponents(int n, int[][] edges) { 
        UnionFind dsu = new(n);

        foreach(var edge in edges) {
            dsu.Union(edge[0], edge[1]);
        }

        return dsu.ConnectedComponents;
    }
}
