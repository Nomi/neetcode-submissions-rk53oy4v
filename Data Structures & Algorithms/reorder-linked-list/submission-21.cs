//Solution from 5th March 2026:

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
    public void ReorderList(ListNode head) {
        //## Part 0: The Ultimate Edge Case Guard
        if (head == null || head.next == null) return; //*IMPORTANT* FORGOT THESE EARLIER!
            
        //forgot to reverse in the previous solution!
        
        ListNode dummy = new(-1);
        dummy.next = head;

        //## Part 1: Get Half 

        // NEED TO REMEMBER THIS IS EXACTLY HOW TO START:
        // DRILL IT IN YOUR HEAD! (Why tho? For the math probably!)
        // var slow = head;
        // var fast = head.next;
        // OR (both on the starting line, no distance covered!):
        var slow = dummy;
        var fast = dummy;

        while(fast?.next != null) //got this from AI, but why?
        {
            slow = slow.next;
            fast = fast.next.next;
        }
        //[1,2,3]: fast=null and slow = 2

        var half = slow;

        //## Part 2: Split the lists!
        var first = head;
        var second = half.next;
        half.next = null;

        //## Part 3: Reverse
        var cur = second;
        ListNode prev = null;
        while(cur!=null)
        {
            var oldNext = cur.next;
            cur.next=prev;
            prev=cur; //the prev was once a cur, so its next has been handled (for first element, done above, outside the loop!)
            cur=oldNext;
        }
        second = prev;

        //## Part 4: Interleave
        while(first!=null && second!=null)
        {
            //Snapshot nexts
            var firstNext = first.next;
            var secondNext = second.next;

            //insert current second right after first and before the next element before first!
            first.next = second;
            second.next = firstNext;

            //set to next unprocessed from each list:
            first = firstNext;
            second = secondNext;
        }

        // //## Return (of the king)
        // dummy.next;
    }
}



//Solution from February 2026:

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
//     public void ReorderList(ListNode head) {
//         if(head==null || head.next == null)
//             return;

//         ListNode mid = head; //will be the end of the resulting LL
//         ListNode end = head; //will be 2nd.
//         //Basically, we need to "merge" these two parts of the linked list such that the even positions (ODD INDEXES) 
//         //are filled with latter part, while the rest are filled with the first part!
//         //We will calculate these below:
//         while(end?.next!=null) //had end.next!=null until AI pointed it out for the null reference exception
//         {
//             mid = mid.next;
//             end = end.next?.next; //had this until AI pointed it out: //head.next?.next ?? head.next;
//         }
//         // Since it's a bit confusing to me as well:
//         // To make sure everything is at the right place, let's visualize some examples:
//         // For 1->2->3 we get end at 3 and mid at 2, which are correct.
//         // For 1->2->3->4 we get end at 4 and mid at 3, which is the correct mid according to the examples provided on the problem!

//         // if(mid==head) not needed since we added and kept `|| head.next == null)` in our initial guard clause!
//         //     return;
        
//         //DEPRECATED: //# *Important*: Actually, we need mid-1 so we can break the list into 2. 
//         //DEPRECATED (continued): //For that, we change `ListNode end = head` to go to `head.next` and add `|| head.next == null)` to our initial guard condition to avoid breaking logic!
        
//         //# *Important*: 
//         //OHH the above are useless because the mid needs to be at the end anyway, so we can just break it there and insert the rest inbetween!

//         var cur  = Reverse(mid.next); //mid.next;
//         mid.next = null;
//         var dest = head;
//         while(cur!=null) 
//         {
//             //in our case, for both even and odd lengths, the left side will be larger! (for even by 2 for odd by 1, because of the way we handle mid!)
//             //So we only care about righ (here, cur)
//             // // var oldDestNext = dest.next;
//             // var oldCurNext = cur.next;
//             // cur.next = dest.next;
//             // dest.next = cur;
//             // cur = oldCurNext;
//             // dest = dest.next.next; //forgot to add this earlier until AI pointed it out!   

//             //Got this from AI and it's better, should always do this! (in fact I started that way!)

//             // 1. Anchor the future
//             var nextDest = dest.next; 
//             var nextCur = cur.next;

//             // 2. The Stitch
//             dest.next = cur;
//             cur.next = nextDest;

//             // 3. The Leap
//             dest = nextDest;
//             cur = nextCur;
//         } 
//         //just realized I forgot to reverse it!
//         // I knew it initially but then I took a break or 2 to eat food and stuff, and lost track of it, 
//         // but hey, I remembered it myself before trying to submit!
//         // I'll write reversal in the middle now!~
//     }

//     public ListNode Reverse(ListNode head)
//     {
//         var cur = head;
//         ListNode prev = null; //had var prev=null before because I changed it from var prev=head but forgot to change that!
//         while(cur!=null)
//         {
//             var oldNext = cur.next;
//             cur.next=prev;
//             prev=cur;
//             cur=oldNext;
//         }
//         return prev;
//     }
// }



// Last Actual Solution: from September 2024
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
//     public void ReorderList(ListNode head) {
//         //READ MY COMMENTS IN attempt1!!! 
//         //Easy but a bit tricky. Just need to know what to do (big picture) and some minor implementation tips/tricks.
//         //Also, only figured out the edge cases after facing the error.
//         //But I do think I thought of them when I was starting but back then I had bigger fish to fry.
//         // attempt1NotThatEfficient(head);

//         ///ACTUALLY: USE THE FOLLOWING SOLUTION!!
//         //Also take a look at my prior "attempt1NotThatEfficient"'s comments! 
//         attempt1Efficient(head);
//     }

//     public void attempt1Efficient(ListNode head)
//     {
//         if(head.next==null) //Could modify the following solution to avoid this condition!
//             return;
//         var slow = head;
//         var fast = head.next; //because we want to move slow right after the center element in odd list.
//         while(fast!=null && fast.next!=null)
//         {
//             slow = slow.next;
//             fast = fast.next.next;
//         }
//         //Here slow, will be at mid point because fast is 2x faster than slow.
//         //E.g. for [1,2,3,4,5,6,7{,8}] if starting pos xs = 1, and starting pos xf = 2:
//         //then, each of the iterations will be:
//         //  (slow, fast)
//         //  (1, 2) //starting (they have to start from double position (2=2*1) right from the start to keep it even.)
//         //  (2, 4) 
//         //  (3, 6) //for odd length, slow is at the last element of the left (unchanged sublist).
//         //  {(4, 8)} //for even length, slow is at the last element of the left (unchanged sublist).
//         //Therefore, we can see that for both even and odd length, slow is at the last element of the left (unchanged sublist).
//         //So, we go next to get the point after which we pick the elements.

//         var rightHead = slow.next;
//         slow.next = null; //break left and right sublists.

//         //Reverse:
//         var prev = rightHead;
//         var cur = prev.next;
//         prev.next = null;
//         while(cur!=null)
//         {
//             var tmp = cur.next;
//             cur.next = prev;
//             prev = cur;
//             cur = tmp;
//         }
//         rightHead = prev;

//         //Merge:
//         var node = head;
//         while(rightHead!=null) //we only check right because it might end before the left (because we can have the extra middle element for odd lengths there) and we can just keep the rest of the linked list as is.
//         {
//             var nodeOldNext = node.next;
//             node.next = rightHead;
//             rightHead = rightHead.next;
//             node.next.next = nodeOldNext;
//             node = nodeOldNext; //because we the current next is just the one we inserted.
//         }
//     }   

//     public ListNode ReverseList(ListNode head) {
//         ListNode prev = null;
//         ListNode current = head;
//         while (current != null) {
//             var next = current.next;  // Store next node
//             current.next = prev;  // Reverse current node's pointer
//             prev = current;       // Move prev to current node
//             current = next;       // Move to next node
//         }

//         return prev;  // New head of the reversed list
//     }


//     public void attempt1NotThatEfficient(ListNode head)
//     {
//         //We can reverse the linkedlist from halfway through.
//         //And for odd length, the element in the middle
//         //is put at the end, i.e. it remains where it is
//         //because only every even element is replaced clearly
//         //We could also figure this out by a look at the example and seeing the 3 at the end.
        
//         //Step 1: calculate length
//         int len = 0;
//         var node = head;
//         while(node!=null) //The length here is correct because for each non-null node that exists, we add 1 (starting from node=head). It is different from what happens below!
//         {
//             len++;
//             node=node.next;
//         }

//         //[IMPORTANT] EDGE CASE:
//         //I think I thought of this edge case before but decided not to care until I had something?
//         if(len<=2)
//             return;
        
        
//         //Step 2: Go halfway (include the middle element for odd length lists)
//         var rightSubHead = head;
//         int currLeftLen = 1; //IMP!! WE START AT 1 BECAUSE WE HAVE one ELEMENT ALREADY (head)! Then we increase it as we add more. //Different from the above loop where we calculate len because there we are adding 1 for each element in the loop, but here we consider one of these already finished.
//         //currLeftLen being 1 when rightSubHead is 
//         //at the 1st position (head), this makes sure that
//         //at n, rightSubHead will be at the n-th node!
//         int targetLeftLen = (len+1)/2; //e.g. int div of (2+1)/2=1 but (3+1)/2 = 2, and this helps us include exactly the elements we want for both even and odd length
//         while(currLeftLen<targetLeftLen) //we go the last element of the target left list before we try getting the right sub-list that we want to reorder and have its elements appended.
//         {
//             rightSubHead = rightSubHead.next;
//             currLeftLen++;
//         }
//         node = rightSubHead.next; //This is the beginning of the second sub-list. We store it here before we cut the list here.
//         rightSubHead.next = null; //we have broken off the list at the middle.
//         rightSubHead = node;
        
//         //Step 3: Reverse right sublist
//         var prevNext = rightSubHead.next;
//         rightSubHead.next = null;
//         while(prevNext!=null)
//         {
//             var temp = rightSubHead; //we store the value of the previous node, so we can link it from the current element later.
//             rightSubHead = prevNext; //we move rightSubHead to what was the previous next, which is our current element.
//             prevNext = rightSubHead.next;//prevNext now contains next of the current element (which is our previousNext)
//             rightSubHead.next = temp; //we set the next of our current node to the one that was before it (set above).
//         }

//         //Step 4: Merge the two sublists in the intended order.
//         ListNode l = head, r = rightSubHead;
//         while(r!=null) //since we didn't change the left side of the list, we only need to keep going until all the r are inserted (generally, they would only end separately if the original list had odd count so the left list had one extra node, but the left list has its original connections intact until we change it so it is fine to just insert the rs we have)
//         {
//             var temp = r;
//             r=r.next;
//             var lOldNext = l.next;
//             l.next = temp;
//             temp.next = lOldNext;
//             l = lOldNext;//obviously.
//         }
//     }
// }
