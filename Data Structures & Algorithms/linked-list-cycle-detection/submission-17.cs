/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int val=0, ListNode next=null) {
 *         this.val = val;
 *         this.next = next;
 *     }
 * }
 */

public class Solution {
    //# New Nifty Notes:
    // The Meeting Point: 
    // - In a real Google interview, they might ask:
    //      "If there is a cycle of length C and the distance to the cycle is D, how many steps until they meet?"
    // The Math: 
    // - They meet within O(N) steps because the distance between them 
    //   (within the cycle) decreases by 1 each iteration. 
    // More details:
    // - The relative difference in their speeds is just 1 (2 - 1 = 1) once they both enter the cycle.
    // - As such, the distance between them shrinks by exactly one node per iteration/step.
    //   This ensures the fast pointer cannot "jump over" the slow pointer without landing on the same node.
    // - Catch-up Guarantee: Once the slow pointer enters the cycle (at distance D), 
    //   the fast pointer is already in the cycle. Since the gap is at most C-1, 
    //   and the gap closes by 1 each step, they are guaranteed to meet in 
    //   at most C-1 additional steps. Total steps: D + (C-1), which is O(N)

    //# Nifty notes from my Last Actual Solution:
    //Here's why it works: https://github.com/Chanda-Abdul/Several-Coding-Patterns-for-Solving-Data-Structures-and-Algorithms-Problems-during-Interviews/blob/main/%E2%9C%85%20%20Pattern%2003:%20Fast%20%26%20Slow%20pointers.md
    //You can also check on paper for even and odd lengths.
   
    public bool HasCycle(ListNode head) {
        //TC: O(N), Aux SC = O(1)
        //Chosen Approach: Floyd's Tortoise and Hare Algorithm (slow and fast pointers)
        //Other approaches: HashSet of seen node pointers/references, but that would be bad for memory. (granted they're hashable, otherwise we'd have to implement hashing function or create a list which would reduce performance)
        var slow = head;
        var fast = head?.next; //while edition  // 11-06-26 Update : must be head?.next now due to new test case!
        //setting both at head does not work if head is the cycle start due to the fast==slow check even if you added != head (doesn't apply for do-while) (can add dummy at start, or just do the first step manually)
        //would work if instead of check we did a do while!
        // 11-06-26 Update 2 [IMP!]: The above is because w're doing the first step. It is basically same as starting both on a dummy node right before head.

        // var fast = head; //do-while edition
        while(fast != null) //while
        // do   //do-while edition
        {
            if(fast == slow) //while //does a reference check by default!
            {
                return true; 
                // This is the part that breaks if we do a while loop with head and fast both the same
                // and the condition were to be moved to the loop, it would still stop at first execution
                // and if we tried a `fast!=head` to avoid that, it breaks further if head is included in the cycle!
            }

            slow = slow.next;
            // slow = slow?.next; //do-while edition
            fast = fast.next?.next;
            // fast = fast?.next?.next; //do-while edition
        }
        // while(fast != slow && fast!=null); //do-while edition

        if(fast == slow && fast!=null) //do-while edition
        {
            return true;
        }
        return false; //no cycle!
    }
}


// Last Actual Solution:
// /**
//  * Definition for singly-linked list.
//  * public class ListNode {
//  *     public int val;
//  *     public ListNode next;
//  *     public ListNode(int val=0, ListNode next=null) {
//  *         this.val = val;
//  *         this.next = next;
//  *     }
//  * }
//  */

// public class Solution {
//     public bool HasCycle(ListNode head) {
//         //Here's why it works: https://github.com/Chanda-Abdul/Several-Coding-Patterns-for-Solving-Data-Structures-and-Algorithms-Problems-during-Interviews/blob/main/%E2%9C%85%20%20Pattern%2003:%20Fast%20%26%20Slow%20pointers.md
//         //You can also check on paper for even and odd lengths.
//         return attempt1(head);
//     }

//     public bool attempt1(ListNode head)
//     {
//         var fast = head?.next; //Hare
//         var slow = head; //Tortoise

//         while(fast!=null)
//         {
//             fast = fast.next?.next;
//             slow = slow.next;
//             if(fast==slow)
//                 return true;
//         }
//         return false;
//     }
// }
