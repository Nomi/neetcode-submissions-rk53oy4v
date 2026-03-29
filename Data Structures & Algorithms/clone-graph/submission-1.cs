public class Solution {
    public Node CloneGraph(Node node) {
        IGraphCloner soln = new
            // Attempt1
            NuAttempt1
        ();
        return soln.CloneGraph(node);
    }
}

// Interface:
public interface IGraphCloner {
    public Node CloneGraph(Node node);
}

// Last Actual Solution (November 2024):
public class Attempt1 : IGraphCloner {
    public Node CloneGraph(Node node) {
        //GO THROUGH MY bfs1 ATTEMPT (AND MAYBE WATCH NEETCODE VIDEO???)
        //KINDA EASY BUUUT TRICKYYYY
        return bfs1(node);
    }

    //TC: O(V+E)
    //SC: O(V)
    public Node bfs1(Node start)
    {
        if(start == null) return null; //I DID NOT EVEN THINK ABOUT IT!!! BE CAREFUL!!!!!!!
        
        var oldToNew = new Dictionary<Node, Node>();

        var q = new Queue<Node>();
        
        oldToNew[start] = new Node(start.val); //So, before we even process an unseen node, as soon as we encounter it, we add its clone here.
        q.Enqueue(start);

        while(q.Count>0)
        {
            var curOld = q.Dequeue();
            foreach(var nei in curOld.neighbors)
            {
                if(oldToNew.TryAdd(nei, new Node(nei.val))) //If it doesn't exist already, it means that it hasn't had its neighbors array filled yet. 
                {
                   q.Enqueue(nei); //queue it up to fill neighbors
                }
                oldToNew[curOld].neighbors.Add(oldToNew[nei]); //fill curOld's current neighbor nei
            }
        }
        return oldToNew[start];
    }
    // public Node bfs1(Node node) //I WAS TRYING TO DO IT IN SUCH A CONVOLUTED WAY!!
    // {
    //     if(node == null) return null; //I DID NOT EVEN THINK ABOUT IT!
    //     HashSet<int> seen = new();

    //     Node dummy = new(-1, new List<int>());
    //     Node parent = dummy;
        
    //     Queue<Node> q = new();
    //     q.Enqueue(node);
    //     while(q.Count>0)
    //     {
    //         var curToClone = q.Dequeue();
    //         var curNew = new Node(curToClone.val, new());
    //         parent.neighbours.Add(curNew);

    //     }
    // }
}





// # Soln. from 30 March 2025 (1:20 AM):

// Took 20 minutes. Why did it take me so long :O?
// I was definitely confused by some graph concepts. 
// I really need to go for a theoretical refresher! (e.g. even complexity analysis would benefit from that!)

public class NuAttempt1 : IGraphCloner {
    public Node CloneGraph(Node node) {
        if(node == null)
            return node;
        return Dfs(node, new());
    }
    
    public Node Dfs(Node cur, Dictionary<Node, Node> cloneCache) //Nevermind, 2-way connection implies we use it still: //Assuming acyclic, otherwise we'd need to keep track of all the already created nodes.
    {
        if(cur == null)
            return null;
            
        if(cloneCache.TryGetValue(cur, out var curCln))
        {
            return curCln;
        }
        
        curCln = new();
        curCln.val = cur.val;
        cloneCache[cur] = curCln;
        //children we'll get in actual recursion itself! (kinda like post order DFS)
        foreach(var neighbor in cur.neighbors)
        {
            curCln.neighbors.Add(Dfs(neighbor, cloneCache));
        }

        //True post order part is that curCln isn't complete until all its children complete (because we're adding neighbors)

        return curCln;
    }
}
