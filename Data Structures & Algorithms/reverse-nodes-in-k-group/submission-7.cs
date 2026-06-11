//# Solution from 6th March 2026

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
    public ListNode ReverseKGroup(ListNode head, int k) {
        ListNode dummy = new(-1, head);
        var prevEnd = dummy;
        while(prevEnd != null)
        {
            // Get group
            var curEnd = GetKth(prevEnd,k); //prev end because our implementation of it is exclusive of given node!
            if(curEnd == null)
                break;
            var curStart = prevEnd.next;
            var nextGroup = curEnd.next;
            //we have our current group now (and the new head of the next group and end of previous group!)

            //Reverse 
            //since I did this problem a few days ago, I know not to get burned by not having snapshots of some of the variables 
            // and ending up changing them when we still need them! 
            //Also, that I need prev and cur to reverse!
            var prev = nextGroup; //20 minutes in, realized I had the right idea but wrong execution! with help of AI (asked for google level nudge vague and short) //curEnd; 
                                  // I knew it should be nextGroup but left it as curEnd somehow :'(
            var cur = curStart;
            // ^ we need to make sure the first element now points to the next group!
            while(cur!=nextGroup) //prevs are handled in prior iteration, in case of first iteration, it's already correct!@
            {
                var curNext = cur.next;
                cur.next = prev;
                prev = cur;
                cur = curNext;
            }

            prevEnd.next = prev; //curEnd; //the earlier end of this group needs to be pointed to by the previous group's end now! 
            prevEnd = curStart;  //new end!
        }
        return dummy.next; //solved in 24mins30secs (with hint from AI for one typo (prev = curEnd instead of nextGroup. Think I wanted to do curEnd.next but then decided to add nextGroup but forgot to switch this back :'())
    }

    public ListNode GetKth(ListNode start, int k) //exclusive of start (start is kinda treated like 0, so kth element is the element k steps from start (0+k))
    {
        while(start != null && k>0)
        {
            start = start.next;
            k--;
        }
        return start;
        
        //for start=A->B->C, K = 2:
        // i1: start = B, k = 1
        // i2: start = C, K = 0
        // return C;
    }
}


//# Solution from 1 March 2026: (might have been the AI mock interview!)
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

// public class Solution {
//     public ListNode ReverseKGroup(ListNode head, int k) {
//         var dummy = new ListNode(-1); 
//         dummy.next = head;
//         //we have to ensure that dummy stores the pointer to the last element IN A K LENGTH PARTITION of a list, so we can return new list head!
//         // > WE HAVE DUMMY FOR THAT!
//         //(if a block at the end has length <k, we just set it to the next of first element of our original list)

//         //Now, for the next of each partition, we need its end, which 
//         // > we'll get by AdvanceNode!
        
//         //Finally, for each partition, we need to know the end of the previous partition, so we can point it's next to the new first (prev end)!
//         // Obviously, this is easy by storing previous end in each loop!
//         // > We do this by prevPartEnd!

//         //BUT, for the original first element, if we point it to dummy, we have cycle!
//         // So, let's think about what it means, okay?
        
//         //15 mins in: WAIT, I was thinking in wrong direction!
//         //prevPartEnd is what the end of the current list will be stored as the next of!

//         //What about where original head should point? 
//         // > endPart.next! The next element after current group! (so we can continue!)   //(it should be at the after end of its k group)
        
//         //20 mins in and we can move on to the code (I did write the advance function written already (included in these 20 mins!)
//         // var start = dummy.next; //REALIZED AT THE END WE DO NEED START!
//         var endPart = dummy;
//         var prevGroupEnd = dummy;
//         while(AdvanceNode(ref endPart, k))
//         {
//             //*IMPORTANT NOTE*:  FOR STANDARD LINKED LIST REVERSAL:       (*IMPORTANT*: fumbled with this yesterday!)
//             // WE NEED `prev` AND `cur`
//             var start = prevGroupEnd.next; //need this to track what the next iteration's endPart should be (because group first is the new group last)!
//             var groupNext = endPart.next; //WE NEED THIS SNAPSHOT LIKE WE NEED START! (CHECK LOOP CONDITION COMMENTS)
//             var prev = endPart.next; //AS DISCUSSED ABOVE, NEXT OF ORIGINAL FIRST IN A GROUP SHOULD POINT TO START OF NEXT GROUP!
//             var cur = prevGroupEnd.next; //start;
//             while(cur!=groupNext) //** IMPORTANT NOTE **: THIS WAS THE OTHER PROBLEM I WAS MISSING 50 minutes IN //(cur!=endPart.next)
//                                   // -> WE OVERWRITE endPart.next LAST ITERATION OF A GROUP (when cur==endPart) SO USING 
//                                   // endPart.Next CAUSES THE CONDITION NEVER TO GO FALSE BECAUSE endPart now points to the prev in last loop
//                                   // and cur points to the start of the next group and we just keep going!
//             {// loop condition covers null too, also, endPart is never null due to implementation of AdvanceNode function!
//                 var curNext = cur.next; //since we'll be overwriting this! //this is where we move cur to after processing!
//                 cur.next = prev;
//                 prev = cur;
//                 cur = curNext; //had prev=curNext, cuz I'm silly, caught it due to the dry run-esque comments afterwards!
//                 //these make sense I thinkt?
//             } 
//             //Assuming we're had k=2, 
//             // - ITR 1: dummy(prevGroupEnd)->A(cur)->B(endPart, curNext)->C(prev)->D 
//             //          and end up [dummy(prevGroupEnd)>A(prev)>C>D & B(cur)>C] (dummy still points to A)
//             // - ITR 2: we end up at: dummy(prevGroupEnd)>A && B>A>C>D 
             
//             // Deprecated://NOW AT THE END OF EACH GROUP, s

//             //Now with the above dry run esque comments, we see we need to set the previous group's 
//             //end's (here dummy's) NEXT to the new first element of this group, which was previously the last element,
//             // > endPart
//             prevGroupEnd.next = endPart; //** IMPORTANT NOTE **: 
//                                         //AT 50 MIN MARK, WAS MISSING THIS!! Even though I had it in my comments :'( I just forgot to add it here
//             prevGroupEnd = start;
//             endPart = start; //(we already set new first element's next to IT'S NEXT, AND DUE TO ADVANCENODE IMPLEMENTATION, IT HANDLES THE TAIL TOO! :D)
//         }

//         return dummy.next;
//     }
    
//     public bool AdvanceNode(ref ListNode node, int k) //IMPORTANT, HAS TO BE REF!!!
//     {
//         while(node!=null && k>0)
//         {
//             node = node.next;
//             k--;
//         }
//         return (node!=null); // `node!=null` IS EQUIVALENT TO `k==0` //Had the condition inverted earlier!
//     }
// }



//# Solution from late September 2024
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
//     public ListNode ReverseKGroup(ListNode head, int k) {
//         // Console.WriteLine(attempt1.tupleTest().a);

//         //WATCHED NEETCODE VIDEO WHILE DOING THIS!!
//         //SO, USE THAT FOR REFERENCE!!! (Easy but remember the merge sort trick, and how it could apply to problems(like this one))
//         //I ALSO HAVE A FEELING THAT DUE TO USING THE NEETCODE SOLUTION WHILE WRITING THIS, I MIGHT NOT BE ABLE TO SOLVE THIS OR A SIMILAR PROBLEM BY MYSELF IN AN INTERVIEW SETTING??
//         return attempt1.ReverseKGroup(head, k);
//     }
// }

// //WATCHED NEETCODE VIDEO WHILE DOING THIS!!
// public static class attempt1
// {
//     public static (int a, int b) tupleTest()
//     {//THESE ALL WORK!!!
//         //Honestly, tested for no reason. (they still work tho!!)
//         //Case 1:
//         return (0,1);
//         //Case 2:
//         // return (a: 0,b: 1);
//     }
//     public static ListNode ReverseKGroup(ListNode head, int k) 
//     {
//         ListNode dummy = new(-1);
//         dummy.next = head;
//         ListNode prevGrpTail = dummy;

//         //GENERAL TRICK: REMEMBER TO USE WHILE(TRUE) IN YOUR SOLUTIONS UNTIL YOU FIND THE BREAK CONDITION [If it exists and you don't know it already, or just use while(true) if that's more convenient].
//         while(true)
//         {
//             var curTail = getKPlus1thFromNow(prevGrpTail, k);
//             if(curTail==null)
//                 break; //not enough elements for a group of k elements, end of list.
//             var nextGrpHead = curTail.next;

//             var cur = prevGrpTail.next;
//             var prev = nextGrpHead;
//             while(cur != nextGrpHead) //IDK why, but this condition, even though extremely easy to come up with, didn't come to me immediately so I just checked the NC .io solution
//             {
//                 var curNext = cur.next;
//                 cur.next = prev;
//                 prev = cur;
//                 cur = curNext;
//             }

//             //THE ABOVE DOESN'T CHANGE THE NEXT POINTER OF THE TAIL OF THE PREVIOUS GROUP!!!
//             //SO WE HANDLE THAT HERE:
//             var prevCurHeadNowCurTail = prevGrpTail.next;
//             prevGrpTail.next = curTail; //curTail is now curHead (because of above reversing loop)
//             prevGrpTail = prevCurHeadNowCurTail;
//         }

//         return dummy.next;
//     }
//     // public static void reverseK(ListNode lastElemOfPrevList, int k)
//     // {
//     //     var head = lastElemOfPrevList.next;
//     //     var tail = getKth(curr, k);
//     //     lastElemOf
//     // }
//     public static ListNode getKPlus1thFromNow(ListNode curr, int k)
//     {
//         while(curr != null && k>0)
//         {
//             curr = curr.next;
//             k--;
//         }
//         return curr;
//     }
// }
