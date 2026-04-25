public class Solution {
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
        // indegree == outdegree for all EXCEPT 2 (which are as below)
        // 1 has outdegre = 1 + indegree
        // 

        adj = new();
        foreach(var ticket in tickets) {
            var src = ticket[0];
            var dst = ticket[1];
            adj.TryAdd(src, new());
            adj[src].Enqueue(dst);
        }
    }
}
