public class Solution {
    ICourseScheduleIISolver solver;
    public int[] FindOrder(int numCourses, int[][] prerequisites) {

        // # Solution from 13 April 2026:

        solver = new NuAttempt1_TopoSort_KahnAlgo();

        // # Last Actual Solution: (Late November 2024)
        
        //DID IT MYSELF RIGHT AFTER DOING THE FIRST/ORIGINAL VERSION OF THIS PROBLEM (Course Scheduler)
        //CHECK MY SOLUTIONS TO  THE ORIGINAL/FIRST_part Course Scheduler!!!
        //Also, check how other solutions on neetcodeio work???

        // solver = new KahnTopoSortSolver();
        
        
        
        return solver.FindOrder(numCourses, prerequisites);
    }
}

public interface ICourseScheduleIISolver
{
    int[] FindOrder(int numCourses, int[][] prerequisites);
}


// # Last Actual Solution: (Late November 2024)
public class KahnTopoSortSolver : ICourseScheduleIISolver
{
    //Check my solution to Course Schedule (the original) for explanations/comments.
    List<int> indegree; //indegree[i] == number of courses that depend on i (number of incoming edges to i)
    List<List<int>> adj;
    public int[] FindOrder(int numCourses, int[][] prerequisites)
    {
        adj = new(numCourses);
        indegree = new(numCourses);

        //Readying our collections, just C# things (could've used int[] for indegree but chose not to)
        for(int i=0; i<numCourses; i++)
        {
            adj.Add(new());
            indegree.Add(0);
        }

        //Build up adjacency list:
        foreach(var pre in prerequisites)
        {
            adj[pre[0]].Add(pre[1]);
            indegree[pre[1]]++;
        }

        //BFS using Topological Sort / Kahn's Algorithm [using indegrees]
        return bfs(numCourses);
    }

    public int[] bfs(int numCourses)
    {
        //BFS using Topological Sort / Kahn's Algorithm [using indegrees]
        Queue<int> q = new(); //we always store courses we can finish right now here (i.e. indegree = 0) as explained later on in the function
        int[] topoOrderedResult = new int[numCourses];
        int finishedCourses=0;
        
        for(int crs=0; crs<numCourses; crs++)
        {
            if(indegree[crs]==0)
                q.Enqueue(crs);
        }

        while(q.Count>0)
        {
            var cur = q.Dequeue();

            topoOrderedResult[numCourses-1 - finishedCourses] = cur;
            finishedCourses++;

            foreach(var prereq in adj[cur])
            {
                indegree[prereq]--;
                if(indegree[prereq] == 0) //If there are 0 other courses that this depends on(has an edge to), it is impossible for this to be part of a cycle, so we can safely say that as long as the rest of the courses before can be taken (which we will figure out later since we're doing this in reverse), this one can also be taken.
                    q.Enqueue(prereq);
            }
        }

        return numCourses == finishedCourses ? topoOrderedResult : new int[0];
    }
}

// # Solution from 13 April 2026:

// Time Taken: 22 minutes for code [I remembered the complexity from solving the problem yesterday, and getting the complexity wrong back then].

// Time and Space complexity: O(V+E). | BECAUSE, we only store and process each node once.  Explanation for both TC and SC:            |
//                                    | While in adjList, nodes can repeat as neighbors, there's a limit to them i.e. number of edges. |
//                                    | As such, the total number of occurences of ANY node as neighbors in adjList is E.              |

public class NuAttempt1_TopoSort_KahnAlgo : ICourseScheduleIISolver {
    static readonly int[] NotFound = []; //still mutable but what can we do

    public int[] FindOrder(int numCourses, int[][] prerequisites) {
        
        int[] indegrees = new int[numCourses]; //initialized with 0s by default! (index is the node, check explanation  for this in comments for adjList below!)
        //indegrees is how many dependencies it has!

        List<int>[] adjList = new List<int>[numCourses]; //can do this since values of prerequisite courses are: 0 to numCourses-1 [upto 1000], 
                                                         //so we can map them to indices of arrays instead of keys of dictionary/hashmap!
        //Adj list is in order of what we do first, its children are then next, and so on.

        foreach(var edge in prerequisites)
        {
            adjList[edge[1]] ??=  new();
            adjList[edge[1]].Add(edge[0]); //must take [a,b] => must take b before a => edge from b to a
            indegrees[edge[0]]++;
        }

        return GetCourseOrder(numCourses, indegrees, adjList);
    }

    // Topo Sort, Kahn's Algo:
    int[] GetCourseOrder(int numCourses, int[] indegrees, List<int>[] adjList)
    {        
        Queue<int> resolvedQ = new(); //resolved dependencies!
        for(int node = 0; node < indegrees.Length; node++)
        {
            if(indegrees[node] == 0)
                resolvedQ.Enqueue(node); //as explained above, index is the node here.
        }

        int coursesTaken = 0;
        int[] results = new int[numCourses]; //obviously!
        while(resolvedQ.Count > 0)
        {
            var curNode = resolvedQ.Dequeue();
            results[coursesTaken] = curNode; //*IMPORTANT* FORGOT THIS BEFORE FIRST NEETCODE RUN TESTS THEN REALIZED!!!
            coursesTaken++;
            var neighbors = adjList[curNode] ?? []; //dependants (dependED on by)
            // The above handles the caseof adjList[curNode] being null (it never had anything that depended on it!)

            foreach(var nei in neighbors)
            {
                indegrees[nei]--;
                if(indegrees[nei] == 0)
                    resolvedQ.Enqueue(nei);
            }

            adjList[curNode] = null;
        }

        if(coursesTaken != numCourses) 
            return NotFound;

        return results;
    }
}
