// *IMPORTANT NOTE*: Check "Depth First Search (Optimal)" on NeetCode using a limit based approach.

// It's also easier intuitively speaking TBH!

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
    public TreeNode BuildTree(int[] preorder, int[] inorder) {
        //READ REC2 (and comments from it), and maybe after that read rec1/
        //Also, TakeUForward's video was better!

        //Since we are not making changes to the values of the sub-arrays, we can use the ArraySegment wrapper (otherwise it would change the original array as well).
        ReadOnlySpan<int> preo = preorder;
        ReadOnlySpan<int> ino = inorder;
        //ARRAY SLICING IN C#: https://code-maze.com/csharp-array-slicing/
        //SHOULD HAVE USED ReadOnlySpan!!
        // return rec1(preo, ino);
        // return rec2(preo, ino);
        
        //NuAttempt1 TC O(N) upgrade (all apporaches until now were TC: O(N^2), including previous version of `BuildTree_NuAttempt1_Recursive`)
        //Note this upgrade is frankenstein to save time.
        //*IMPORTANT* Better to just pass indices instead of spans and then deal with actual indices for this approach!
        indices = new(); //Only works if values are unique!
        for(int i=0;i<ino.Length;i++){indices[inorder[i]] = i;}; 
        return BuildTree_NuAttempt1_Recursive(preo, ino, 0);  

        //DO IT WITH INDICES NEXT TIME (AND EVERY TIME TBF!)
    }

    // # Solution from 07-03-2026
    // Better explained in `rec1`
    Dictionary<int, int> indices;
    public TreeNode BuildTree_NuAttempt1_Recursive(ReadOnlySpan<int> preo, ReadOnlySpan<int> ino, int inoFirstActualIdx)
            //`inoFirstActualIdx` is the actual index of first element of the span in its OG array form!! (so we get 0'th for first element!)
    {
        //Mostly copying with some learning, will watch a video later and attempt again!

        //well I did try writing it before and could come up with nothing but comments in 20 minutes
        // until i had AI verify them and turned out partially wrong.
        //So, now I have my own partial correct notion and some nudges from the AI. Let's see if i can get it done!

        // Had to look at my old solution `rec2` A LOT (actually mostly just that) to do this! Will watch TakeUForward video after this and try to do it again later!
        if(preo.IsEmpty || ino.IsEmpty)  //copied this from my old solution, but it makes sense I guess?
            return null;

        TreeNode cur = new(preo[0]);

        var curIdxInOrder = indices[cur.val] - inoFirstActualIdx; //ino.IndexOf(cur.val); 
        
        //Note that: 
        //preo[0] is the root of the current (sub)tree and 
        //ino[curIdxInOrder] is the the root of the current (sub)tree
        // And we don't include the current node in any of the subtree traversals (as expected!)


        // LST (Left SubTree):
        var lstLength = curIdxInOrder; //Why not -1 to cancel out the root? because idx is 0-indexed!

        var lstPreo = preo[1 .. (1+lstLength)]; // +1 => INCLUSIVE of lstLength
        var lstIno = ino[ .. curIdxInOrder]; //EXCLUSIVE of curIdxInOrder

        cur.left = BuildTree_NuAttempt1_Recursive(lstPreo, lstIno, inoFirstActualIdx); //inoFirstActualIdx doesn't change

        // RST (Right SubTree): 
        var rstPreoStartIdx = 1 + lstLength; //we skip root and all lst elements (root is 0th idx, lstlength is last lst elem), so +1 gives first rst idx!
        var rstInoStartIdx = 1 + curIdxInOrder;
        
        var rstPreo = preo[rstPreoStartIdx .. ]; //Exclusive of whole left side and root!
        var rstIno = ino[rstInoStartIdx ..]; //Exclusive of curIdxInOrder

        cur.right = BuildTree_NuAttempt1_Recursive(rstPreo, rstIno, indices[cur.val] + 1); // OR: inoFirstActualIdx + rstInoStartIdx); 
        //since curIdxInOrder = indices[cur.val] - inoFirstActualIdx
        // rstInoStartIdx = 1 + curIdxInOrder
        // the rst inoFirstActualIdx we pass is = inoFirstActualIdx +  1 +  `indices[cur.val] - inoFirstActualIdx`
        //                              = indices[cur.val] + 1;
        // WHICH IS INDEED WHAT WE HAVE DEFINED inoFirstActualIdx as! 
        
        
        // THE FOLLOWING IDEAD IS HORRIBLY WRONG! DO NOT DO: (in fact, we could probably just also pass parent element and then use its index to calculate offsets maybe?)
        
        //Return
        return cur;
    }


    // # Solutions from late 2024:
    public TreeNode rec2(ReadOnlySpan<int> preorder, ReadOnlySpan<int> inorder)
    {
        //Done on my own! //Used TakeUForward's YouTube video to re-learn/confirm the theory! Explains better than NeetCode, honestly. (more like, his visualization of the arrays is better or at least my headspace now is better than it was back then! (don't have as much of a headache!))
        if(preorder.IsEmpty || inorder.IsEmpty)
            return null;
        var currRoot = new TreeNode(preorder[0]);
        int currRootPosInorder = inorder.IndexOf(preorder[0]);

        //[!! IMPORTANT !!] (some of the theory explained!)
        //Notice that currRootPosInorder is also the number of nodes to the left of the root because InOrder is recursively left>root>right, 
        //and as such, all the nodes to in the left subtree of the node are to its left in the inorder array and are stored contiguosly.
        //Furthermore, preorder is root>left>right, all the left element will be contiguously right after it, and we have the count of 
        //the nodes left already sub tree already so we can split the arrays into subproblems for each left and right sub treee.
        //For right sub tree, we use similar logic.
        //ALSO, each of the subarrays will obviously preserve their nature (i.e. being the preorder or inorder tree of the subtree.)
        
        //Recursively build sub-trees:
        //Remember the second parameter in the range operator is EXCLUDED! 
        //(NOTE: second parameter is the first index to exclude, NOT the length of the subarray as I had initally thought)
        currRoot.left = rec2(preorder[1 .. (1+currRootPosInorder)], inorder[..currRootPosInorder]);
        
        //Recursively build the right sub-tree:
        currRoot.right = rec2(preorder[(1+currRootPosInorder)..], inorder[(1+currRootPosInorder)..]);


        //Also notice the sub inorder and sub preorder subarrays being passed contain the next root, but not the current root. Obvious, but just wanted to point it out.
        return currRoot;
    }


    public TreeNode rec1(ReadOnlySpan<int> preorder, ReadOnlySpan<int> inorder)
    {
        //ACCORDING TO THE NEETCODE VIDEO, WE NEED THE FOLLOWING:   (WATCH THE NEETCODE VIDEO!!!)
        //Note that it's given every value in tree is unique.
        //Fact 1: First value in preorder traversal is always the root. 
        //        This can be done for each subtree after that (as long as we disregard/remove the prevcious root).
        //Fact 2: Every value to the left of an elemen in the in-order traversal array is on the left sub tree 
        //        and every element after that goes in right subtree. (we got through the tree inorder (left to right.))
        
        if(preorder.IsEmpty || inorder.IsEmpty)
            return null;

        var curRoot = new TreeNode(preorder[0]);
        // Console.WriteLine(preorder[0]);
        int curPosInInorder = inorder.IndexOf(curRoot.val);

        curRoot.left = rec1(preorder[1..(1+curPosInInorder)], inorder[..curPosInInorder]); //rec1(preorder.Slice(1,1+curPosInOrder), inorder.Slice(0, curPosInOrder));
        //NOTES FOR THE LINE DIRECTLY ABOVE THIS COMMENT:
        //DEPRECATED!! READ COMMENT BELOW INSTEAD OF THE ONE TO THE RIGH!! /??? (1+curPosInOrder) because the second parameter is length and we want to include the same number of elements in inorder as we do in preorder (i.) We start from 1 because we don't want it as we're done with creating it.
        //ACTUALLY: the second parameter in the range operator is EXCLUDED! (also, the second parameter is the first index to exclude, NOT the length of the subarray as I had initally thought)
        curRoot.right = rec1(preorder[(1+curPosInInorder)..], inorder[(1+curPosInInorder)..]); //rec1(preorder.Slice(1+curPosInOrder));
        
        return curRoot;
     }

    public static int GetIndexOf(ArraySegment<int> arraySegment, int target)
    {
        int i=0;
        while(true)
        {
            if(arraySegment[i]==target)
                return i;
            i++;
        }
    }
}


//## Thinking out loud: 
//           1
//          /
//         2
//        / \
//       3   5
//      /
//     4
// Preorder: 1, 2, 3, 4, 5
// Inorder:  4, 3, 2, 5, 1
//
// Clearly, when we reach the end of the list, in preorder, the element at the left-most end will be in the beginning.
// That is to say, we can know when a tree ends when we reach the element at the beginning.
// Also, any unseen nodes from then on, are on the right node.
