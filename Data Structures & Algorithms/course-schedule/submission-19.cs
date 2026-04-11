public class Solution 
{
    ICourseSchedule soln;
    public bool CanFinish(int numCourses, int[][] prerequisites) 
    {
        // # Solution from 12th April 2026 [12-04(April)-2026]: 
        soln = new NuAttempt1_TopoSort_KahnAlgo();

        // # Last Actual Solution: (late 2024)

        // //I SHOULD HAVE REALIZED THAT numCourses was to get all the course names because there was no other relaible way to do it (AS ALWAYS, LEARN TO CAREFULLY READ THE FKIN PROBLEM!!!)
        
        // //[!!!IMPORTANT!!!] //About Time and Space Complexities
        // //ACCORDING TO TakeUForward's video on Kahn (also applies to other graph algorithms with similar time complexities):
        // // SC: O(V+E), we store each vertex and its outgoing edges (and since it is a directed graph, any edge will only belong to/stored for the vertex it is going out from!)
        // // TC: O(V+E), similarly, each Vertex AND Edge is considered only ONCE.

        // //[!!!IMPORTANT!!!] 
        // //NEED TO PRACTICE!!! 
        // //I had done this approximately 3 months ago but forgot.
        // //I started by trying an overcomplicated method (using Nodes and building adjacency lists, etc.),
        // //but when it got too complicated I checked the solution on neetcodeio and remembered we could do it the way I finally ended up doing it in DfsAttempt1.
        // //I think I have heard of Kahn's algorithm but forgot.
        
        // //[!!!IMPORTANT!!!] 
        // //CHECK THE [OPTIMIZATION] comment IN DfsAttempt1.dfsCycleDetector
        // //I DID NOT REALIZE UNTIL THE END WHEN I WAS CALCULATING TIMECOMPLEXITY THAT WE COULD OPTIMIZE THIS WAY!!!
        // //I did refer to (sneak a peak at) neetcodeio soln to see how they did it for inspiration.

        // //[!!!IMPORTANT!!!] 
        // // I didn't know how to do it with Topological sort / Kahn's algorithm before. BUT:
        // // Watched TakeUForward's YouTube video about Kahn's algorithm/Topological sort for it (Title: "G-22. Kahn's Algorithm | Topological Sort Algorithm | BFS")
        // // It was extremely helpful and intuitive!!
        // // Read the [CAUTION!] message in the bfs function!
        // // WATCH THE TakeUForward video AGAIN (Title: "G-22. Kahn's Algorithm | Topological Sort Algorithm | BFS")
        
        // /////// SOLUTION:
        // // soln = new DfsAttempt1(); //TC: O(V+E) SC: O(V+E) [BUT WHY??? Rewatch GregHogg's video about graphs?? maybe that'll help??]
        // soln = new TopologicalSortKahnsAlgorithmAttempt1(); //TC: O(V+E) SC: O(V+E) [BUT WHY??? Rewatch GregHogg's video about graphs?? maybe that'll help??]
        
        
        return soln.CanFinish(numCourses, prerequisites);
    }
}

public interface ICourseSchedule
{
    bool CanFinish(int numCourses, int[][] prerequisites);
}

// # One of the Last Actual Solution(s): (late 2024)
public class DfsAttempt1 : ICourseSchedule
{
    //DEPRECATED_DfsAttempt1 to see how convoluted I was making this, even though I could do it so simply!
    Dictionary<int, List<int>> prereqMap;
    HashSet<int> visiting;

    public bool CanFinish(int numCourses, int[][] prerequisites) 
    {
        prereqMap = new(numCourses);
        visiting = new();
        
        //Fill prereqMap:
        for(int i=0; i<prerequisites.Length;  i++) //O(N) where N is numCourses
        {
            prereqMap.TryAdd(prerequisites[i][0], new());
            prereqMap[prerequisites[i][0]].Add(prerequisites[i][1]);
            // prereqMap.TryAdd(prerequisites[i][1], new()); //Uncomment this if you either: 1. don't initialize hashmap for every course already in a loop above this one 2. in dfs, return true if prereqMap does not contain the course as a Key (because otherwise the program breaks/throws_exception because of the key not being present)
        }

        //DFS to find cycles (meaning you can't take courses in that cycle):
        //O(N*())
        for(int course=0; course<numCourses; course++) //Given that that the courses will be numbered 0 to numCourses-1, we can manually use a normal for loop to do that instead of looping over prereqMap keys.
        {
            if(false == dfsCycleDetector(course)) //CHECK THE [OPTIMIZATION] comment IN dfsCycleDetector
                return false;
        }

        return true;
    }

    bool dfsCycleDetector(int course)
    {
        if(visiting.Contains(course))
            return false;

        if(false == prereqMap.ContainsKey(course)) //Has no prerequisites.
            return true;
        
        visiting.Add(course);

        foreach(var prereq in prereqMap[course])
        {
            if( false == dfsCycleDetector(prereq))
                return false;
        }
        
        visiting.Remove(course);
        prereqMap.Remove(course); //[OPTIMIZATION] DAMN! I WAS CALCULATING TIME COMPLEXITY AND ONLY THEN DID I REALIZE OF THIS IMPROVEMENT!!! (since we already found it doesn't need prerequisites), this way we will never go through it again, saving us time.
        return true;
    }
}

// # One of the Last Actual Solution(s): (late 2024)
public class TopologicalSortKahnsAlgorithmAttempt1 : ICourseSchedule //topoSort
{
    //[IMPORTANT] 
    // Watched TakeUForward's YouTube video about Kahn's algorithm/Topological sort for this (Title: "G-22. Kahn's Algorithm | Topological Sort Algorithm | BFS")
    // It was extremely helpful and intuitive!!

    List<List<int>> adjList;
    //Number of incoming edges (here, indegree[i] number of courses that depend on the course `i`):
    List<int> indegree;
    // int[] indegree; 

    public bool CanFinish(int numCourses, int[][] prerequisites) 
    {
        Queue<int> q = new(); //SC: O(V)/O(E)? (worst case when all nodes independent (?or when all nodes depend on 0)
        //Enumerable.Repeat(new List<int>(), numCourses).ToList(); //WRONG! THIS BREAKS THE SOLUTION! (probably because for reference types, Enumerable.Repeat uses the same reference everywhere!)
        adjList = new(); //SC: O(V+E)
        indegree = new List<int>(numCourses); //O(V)
        for (int i = 0; i < numCourses; i++) {
            adjList.Add(new List<int>());
            indegree.Add(0);
        }
        // indegree = Enumerable.Repeat(0, numCourses).ToList();
        // indegree = new int[numCourses]; //For arrays, values are initialized to their default value (here 0) by default.
        
        //Fill adjacencyList:
        for(int i=0; i<prerequisites.Length;  i++) //TC: O(E)
        {
            adjList[prerequisites[i][0]].Add(prerequisites[i][1]);
            indegree[prerequisites[i][1]]++;
        }

        //1. Add to queue all courses with indegree == 0
        for(int i=0;i<numCourses ; i++)//[CAUTION!] I WAS USING `i<prerequisites.Length` for the loop condition WHEN IT SHOULD HAVE BEEN `i<numCourses` (spent ~10 mins debugging!)
        {
            if(indegree[i]==0) //We set this up here to eventually basically multi-source bfs from these
                q.Enqueue(i);
        }

        //2. bfs:
        int coursesFinished = bfs(q); //O(V+E) (but why?)
        return (coursesFinished == numCourses);
    }

    int bfs(Queue<int> q) //TC: O(V) since all edges are visited only once
    {
        int coursesFinished = 0;
        while(q.Count>0) //Each loop continues bfs over all nodes that have 0 indegree at that time (indegree == number of courses that depend on it) //courses that are not dependencies of anything, but may depend on other courses.
        {
            //If there are 0 courses that depend on a course, there is NO chance of it being a part of a cycle (doesn't mean that there are no cycle outside of it).
            var curCourse = q.Dequeue();
            foreach(var prereq in adjList[curCourse])
            {
                indegree[prereq]--;
                if(indegree[prereq]==0)
                    q.Enqueue(prereq);
            }
            coursesFinished++;
        }
        return coursesFinished;
    }
}

// # One of the Last Actual Solution(s): (late 2024)
// public class DEPRECATED_DfsAttempt1 : ICourseSchedule //I WAS OVERCOMPLICATING IT BECAUSE I DIDN'T EVEN NEED A NODE CLASS!! CHECK THE NEW `DfsAtt1emp1`!!!
// {
//     const UNVISITED = 1;
//     const VISITING = 1; //If you encounter a node with this state again, there's a cycle.
//     const VISITED = 2; //Confirmed no cycles.
//     internal class Node
//     {
//         public int course;
//         public int state;
//         public List<int> prereqs;
//         public Node(int _course)
//         {
//             course = _course;
//             dependancyOf = new();
//             state = UNVISITED;
//         } 
//     }
//     public bool CanFinish(int numCourses, int[][] prerequisites)
//     {
//         //1. Make an adjacency list
//         Dictionary<int, Node> adjList = new(n);
//         for(int r = 0; r<prerequisites.Length; r++) //O(numCourses^2)
//         {
//             //READ THE QUESTION PROPERLY!!! I WAS DOING IT THE OTHER WAY AROUND!!!
//             adjList.TryAdd(prerequisites[r][0], new(prerequisites[r][0]));
//             adjList.TryAdd(prerequisites[r][1], new(prerequisites[r][1]));
//             adjlist[prerequisites[r][0]].dependancyOf.Add(prerequisites[r][1]);
//         }

//         //2. DFS to find any cycles
//         foreach((int key, Node node) in adjList)
//         {
//             if(node.state != VISITED)

//         }
//     }

//     bool dfs(node)
//     {
//         if(node==VISITING) //CYCLE
//             return false;
//         foreach(int )
//     }
// }


// # Solution from 12th April 2026 [12-04(April)-2026]: 
public class NuAttempt1_TopoSort_KahnAlgo : ICourseSchedule {
    // I only straight away knew how to use Topological Sort (Kahn's Algo) here. 
    // I did see that DFS was a possible solution but I just don't know and not really that interested since I talked to an AI and 
    // they said this was the optimal solution even for this variant where you don't need to make the order itself!

    // ## Time Taken:
    // NOTE: I did it with a pretty bad headache, while I took some medicine and it is better, still not peak brain performance OR feeling. Sucks.
    // Took 16 minutes for the code
    // Took 4 minutes to fix the bugs (the 2 major adjList ones and the smaller ones).
    // Took 5 minutes for complexity analysis (though i was kinda being leisurely).
    // Total = 25  minutes (excluding dry run or approach discussion in the beginning).
    // WAIT, I HAD 2 BUGS: 
    // 1. Using foreach to enqueue value of indegrees instead of the course ID (index). [switched to for now!]
    // 2. I had misunderstood edge directions in the preqrequisites array so my adjList building logic was slightly reversed,
    // Noticed only on failure of NC.io automated tests.
    // Then I asked Gemini AI to help me out with a hint (it basically pointed out the above 2 problems in a hint-ish way, though for the first one it just directly said index vs type problem).
    // Took 3 minutes to fix that.
    // So I guess that brings total to 28 minutes (excluding dry run or approach discussion in the beginning).
    // BUT it isn't very accurate since it doesn't account for time saved from NC.io compiler/tests and Gemini 
    // AND time lost from my headache (and the silly mistakes made henceforth).
    

    
    // ## Complexities:
    // Time: 
    //  TC = O(E + V + V*E_out_max) where E_out_max is the maximum edges going OUT of any vertex, which theoretically is (E-1)
    //  => TC = O(V*E);
    // Space:
    //  SC= O(V*E (adjList)+ V(indegrees) + V(queue)) 
    //  => SC = O(V*E)
    public bool CanFinish(int numCourses, int[][] prerequisites) {
        // prerequisites is clearly an edge-list here.
        
        int[] indegrees = new int[numCourses]; //To track number of remaining unfulfilled dependencies!
        Dictionary<int, List<int>> adjList = new();
        foreach(var edge in prerequisites)
        {
            indegrees[edge[0]]++;
            adjList.TryAdd(edge[1], new());
            adjList[edge[1]].Add(edge[0]);
        }

        return TopoSort(adjList, numCourses, indegrees);
    }
    
    // Kahn's Algo for Topologial Sort: (kinda just a modified BFS. Let's call it modded BFS.)
    bool TopoSort(Dictionary<int, List<int>> adjList, int numCourses, int[] indegrees)
    {
        Queue<int> fulfilledQ = new();
        EnqueueNodesWithNoMissingDependencies(fulfilledQ, indegrees);
        
        uint coursesTaken = 0; 

        while(fulfilledQ.Count > 0)
        {
            var curNode = fulfilledQ.Dequeue();
            coursesTaken++;
            
            var neighbors = adjList.GetValueOrDefault(curNode, []); // *IMPORTANT* DIDN'T CONSIDER NODES WITH NO EDGES BEFORE! //alt .name: dependedOnBy // var neighbors = adjList[curNode]
                                                                    // Took my first neetcode run to realize it! (I mean, in my head I knew this scenario exists, but I forgot to account for it here. in fact, probably using an int[] adjList would be better here for hashing performance and safety from this!)
            // adjList.Remove(curNode); // Was doing the same here but then on first neetcode run realized it was wrong, so, while I could've fixed it with a contains check, it is not a vital part of the algorithm so I'm cutting it out for now!
            
            foreach(var nei in neighbors)
            {
                indegrees[nei]--;
                if(indegrees[nei] == 0) //the last dependency enqueues nodes that aren't 0 indegrees here.
                {
                    fulfilledQ.Enqueue(nei);
                }
            }
        }
        return coursesTaken == numCourses;
    }

    void EnqueueNodesWithNoMissingDependencies(Queue<int> fulfilledQ, int[] indegrees)
    {
        //foreach(var node in indegrees)
        for(int node = 0; node< indegrees.Length; node++)
        {
            //could validate bounds, but constraints don't specify
            if(indegrees[node] == 0)
                fulfilledQ.Enqueue(node);
        }
    }
}
