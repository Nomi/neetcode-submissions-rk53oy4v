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