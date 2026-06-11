/*
 * === BST LOWEST COMMON ANCESTOR (LCA) PATTERN NOTES ===
 *
 * 🕵️‍♂️ THE "DE-WRAPPER" CHEAT SHEET
 * In your pattern notes, when you are trying to spot a hidden BST LCA problem, 
 * look for these three clues in the interviewer's prompt:
 * * 1. Hierarchy: They mention a tree, a chart, a directory, or nested containers.
 * 2. Sorted Constraint: They explicitly tell you a rule about how things are ordered 
 * (e.g., "Left is always older, right is always newer," or "IDs are strictly partitioned"). 
 * This is your signal that you can search in O(log N) time.
 * 3. Intersection: They ask for the "closest shared," "first common," "smallest enclosing," 
 * or "lowest mutual" item between two distinct targets.
 *
 * ---🕵️‍♂️ THREE COMMON GOOGLE WRAPPERS ---
 * * 1. The "Org Chart / Access Control" Wrapper (Most Common)
 * - The Story: "We have a massive corporate directory. Every employee has a unique 
 * SeniorityRank (or EmployeeID). The directory is structured so that a manager's 
 * left-hand reports always have a lower rank, and right-hand reports have a higher 
 * rank. If two employees need to escalate a conflict, they must go to their 
 * lowest common manager."
 * - The Trap: They give you an Employee object. You have to realize that 
 * SeniorityRank = BST.val, and "lowest common manager" = LCA.
 * - The Follow-up: "What if the employees don't know who their manager is? 
 * (No parent pointers)."
 *
 * 2. The "Network Router / IP Address" Wrapper (Very Google Cloud)
 * - The Story: "We are routing traffic through a hierarchy of network switches. 
 * Subnets are divided dynamically: a switch routes all IP addresses smaller 
 * than its own subnet ID to its left connection, and larger ones to its right 
 * connection. Machine A (IP: 192.168.1.5) needs to send a packet to Machine B 
 * (IP: 192.168.1.250). What is the first common switch they will both hit?"
 * - The Trap: IPs look like strings, but under the hood, they are just 32-bit 
 * integers. Once you convert them to ints, it is literally BST_SEARCH.
 *
 * 3. The "Video Game Rendering / Spatial Tree" Wrapper (Graphics/Maps)
 * - The Story: "You are rendering a 1D landscape. The map is divided using a 
 * Binary Space Partitioning (BSP) tree, where an x-coordinate splits the world. 
 * Left is smaller x, right is larger x. Two players are standing at x1 and x2. 
 * We want to draw a weather effect (like a rain cloud) that covers both players, 
 * but is attached to the smallest possible map sector that contains them both."
 * - The Trap: The phrasing "smallest possible map sector that contains them both" 
 * is the exact definition of a Lowest Common Ancestor.
 */



/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */

public class Solution {
    public TreeNode LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q) {

        //IMPORTANT! USE NEETCODEIO ITERATIVE SOLUTION 
        // BECAUSE IT IS BETTER DUE TO SAME TIME COMPLEXITY BUT O(1) SPACE COMPLEXITY 
        // (recursive has O(log(n) space complexity)!!!!
        //READ THE COMMENTS FOR rec1 (and rec1Helper) 
        // return rec1(root, p, q);
        // return itr1_NeetCodeBasedSoln(root, p, q);
        return BST_SEARCH(root, p, q);
    }

    //12-06-26 Update: Could've just done (TreeNode root, int pVal, int qVal)
    public TreeNode BST_SEARCH(TreeNode root, TreeNode p, TreeNode q) //*IMPORTANT NOTE*: UNDERSTAND QUESTION THOROUGHLY //SHOULD'VE READ THE QUESTION PROPERLY AND THOUGHT ABOUT IT (forgot it was BST and forgot how to take best advantage of it!)
    {
        //alaready know values are unique so don't need checks for ==
        var small = p.val < q.val ? p : q;
        var big = p.val > q.val ? p : q;

        return BST_SEARCH_helper_dfs_pre_order_NuAttempt1(root, small, big);
    }

    private TreeNode BST_SEARCH_helper_dfs_pre_order_NuAttempt1(TreeNode root, TreeNode small, TreeNode big) //theoretically, pre order dfs takes less space iteratively
                                                                                                             //and even other orders are safer out of call stack (for huge data)
                                                                                                             //BUT, it can be easier to write recursively.
                        //Time Complexity: O(log2(N)) [search in binary seacrch tree]
                        //SC: O(H) where at worst H = N (skewed to LL), at best O(log2(N)) for balanced
                        //      since this is preorder traversal, and iteravtive solution would have SC O(1)
    {
        //break through at 13 mins:
        // OHH SINCE p and q ARE GUARANTEED TO BE IN THERE
        // AND IT IS A BINARY SEARCH TREE, WE HAVE AN EASIER PATH!

        //Given: node values are unique, and two nodes from the tree p and q
        // assuming that means p and q are different?
        //oh yes: p != q && p and q will both exist in the BST.
        // I did think of these clarifications but a bit too late

        if(root == null) //assuming p and q are not null (and cannot be null)
            return null;


        if(big.val < root.val) //the bigger of the 2 is smaller than current root value
        {
            //look left (they're both on left)
            return BST_SEARCH_helper_dfs_pre_order_NuAttempt1(root.left, small, big);
        }
        else if (small.val > root.val) //the bigger of the 2 is bigger than current root value
        {
            //look right (they're both on right)
            return BST_SEARCH_helper_dfs_pre_order_NuAttempt1(root.right, small, big);
        }
        else //if(small.val <= root.val && big.val >= root.val)
        {
            // Given that p!=q

            // Case 1:
            //-> small is either current or in left branch
            //-> big is in the right branch
            // => this is THE LCA.

            // Case 2:
            //-> big is either current or in right branch
            //-> small is in the left branch
            // => this is THE LCA.
            return root;
        }
        // # Deprecated:
        // ELSE:
        // return helper_dfs_post_order_NuAttempt1(root.left, p, q) ?? helper_dfs_post_order_NuAttempt1(root.right, p, q); //THIS IS A FAILURE TO USE PROPER BST STRATS!

        //Finished in 25 minutes but AI had to point out that I was doing a root.right instead in the second call at the end!
    }



    // ATTEMPT WHERE I WAS DOING EVERYTHING WRONG AND EVERYTHING WENT WRONG BECAUSE I FAILED TO READ AND UNDERSTAND QUESTION!
    // public TreeNode dfs_post_order_NuAttempt1(TreeNode root, TreeNode p, TreeNode q) //I wasn't even trying to use BST property! //gave up after 10 minutes!
    // {
    //     if(root == null) //assuming p and q are not null (and cannot be null)
    //         return null;
    //     // if(root == p) //wait, no this kills the search if p is root of q or vice versa!
    //     //     return root;
    //     // if(root == q)
    //     //     return root;
        
    //     var leftPorQ = dfs_post_order_NuAttempt1(root, p, q);
    //     var rightPorQ = dfs_post_order_NuAttempt1(root, p, q);
        
    //     if(leftPorQ != null && rightPorQ !=null)
    //     {
    //         return root;
    //     } 
    // }

    public TreeNode itr1_NeetCodeBasedSoln(TreeNode root, TreeNode p, TreeNode q) //Should modify to bigger and smaller precalculated like in rec1 to simplify conditions!
    {
        while(true)
        {
            if(root.val < p.val && root.val < q.val) //Checking both here because we don't know which one is bigger or smaller, but we could modify it that way (like in the recursive solution!)
                root = root.right;
            else if (root.val > p.val && root.val > q.val)
                root = root.left;
            else
                return root; // Check comments in rec1Helper to check why this works!
        }
    }
    //TC: O(log(n))
    //SC: O(log(n))
    public TreeNode rec1(TreeNode root, TreeNode p, TreeNode q)
    {
        var bigger = p.val>=q.val ? p : q;
        var smaller = p.val>=q.val ? q : p;
        return rec1Helper(root, smaller, bigger);
    }
    public TreeNode rec1Helper(TreeNode root, TreeNode smaller, TreeNode bigger)
    {
        // Console.WriteLine($"{root?.val} : {smaller.val}, {bigger.val}");

        if(root.val>bigger.val)
            return rec1Helper(root.left, smaller, bigger);
        if(root.val<smaller.val)
            return rec1Helper(root.right, smaller, bigger);

        //IF root.val >= smaller.val && root.val <= bigger.val (because of above if conditions) 
        
        //Notice how because of the conditions above we will only ever get here when in the following cases:
        //
        //1. When `root`'s value is between smaller and bigger and as such,
        //this is where the paths diverge (smaller on left and bigger on right).
        //Therefore, this point would be the LOWEST common ancestor.
        //
        //2. If the above condition is untrue, this means that the `root` is either the `smaller` or `bigger` node.
        // and since we haven't found the other one yet and it is guaranteed [clarification question???] that
        // both the values exist in there, we can be sure that this is the lowest common ancestor because this element won't be anywhere after here.
        return root;
    }
}
