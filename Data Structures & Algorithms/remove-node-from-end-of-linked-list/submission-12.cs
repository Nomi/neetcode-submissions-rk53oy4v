//# Solution from March 2026

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
    public ListNode RemoveNthFromEnd(ListNode head, int n) {
        //establish: last node is the 1st node from end!
        if(n==0)
            return head; //deleting nothing!
        
        var dummy = new ListNode(-1, head);

        //Set distance n between them to be n, THIS MAKES SURE THAT WHEN ahead.next == null, THEN lag.next IS THE NODE TO REMOVE! (HAVING lag HELPS IN REMOVAL!)
        var lag = dummy; //nevermind, for removing first node, cleaner to have dummy! //for the math here, better to use head!
        var ahead = dummy;

        int gap = 0;
        while(ahead != null && gap < n) // GAP CAN AT MOST BE N! (so stop check at gap<n) [caught this only on my first run] //DEPRECATED FOR NOW: //distance LESS THAN < (NOT <=) n, because we want the element right before the element we remove!
        {                               // 11-06-26 Update: Must have n-1 nodes left after the current node (between node and the null reference of tail)
            ahead = ahead.next;
            gap++;
        }

        if(gap != n)
        {
            throw new Exception("Not enough elements in list!");
        }
        
        while(ahead.next!=null) //because we want (n+1)-th from end to remove n-th
        {
            ahead = ahead.next;
            lag = lag.next;
        }

        lag.next = lag.next.next; //lag.next is guaranteed to not be null due to our above loop and n==0 check!

        return dummy.next; //done in 15mins 41secs
    }
}


//# Solution from Feb 2026:

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
//     public ListNode RemoveNthFromEnd(ListNode head, int n) { 
//         //assuming length of linked list bigger than n
//         //and assuming n!=0
//         //oh wait, what indexed is the n? 0 indexed? Checked constraints and it's 1 and I assume n=1 means last node! //took 5-7 minutes to debug even with the help of the neetcode runner
        
//         //*ULTRA IMPORTANT* I FORGOT THE CASE OF DELETING FIRST ELEMENT!
//         ListNode dummy = new(-1, head);

//         var end = dummy;
//         while(n > 0) //> because dummy counts BUT Also, it is 1 indexed! //took 5-7 minutes to debug even with the help of the neetcode runner
//         {
//             end = end.next;
//             n--;
//         } 
//         //stale due to dummy: head = [1,2,3,4], n = 3 => e1n3, e2n2, e3,n1,e4n0 => end = 0
//         // head is n elements away (inclusive of head and end)
//         // so, we need end to be at null for head to be the n-th element from the end (not the variable)
        

//         var nPlus1ThFromEnd = dummy; //changed from nThFromEnd
//         while(end?.next!=null) //# *IMPORTANT NOTE* I FORGOT TO DO `end?.next` I KEEP FORGETTING TO ACCOUNT FOR THE LAST CHECK!
//         {//WAIT I almost did while(end!=null), but we need to know beforehand (n+1th from end) to remove!

//             //nThFromEnd = nThFromEnd.next;
//             nPlus1ThFromEnd = nPlus1ThFromEnd.next;
//             end = end.next;
//         }
//         nPlus1ThFromEnd.next = nPlus1ThFromEnd.next?.next;
        
//         return dummy.next;
        
//         //total time taken = 25m;
//     }
// }


//# Last Actual Solution: September 2024
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
//     public ListNode RemoveNthFromEnd(ListNode head, int n) {
//         return attempt1(head, n);
//     }



//     public ListNode attempt1(ListNode head, int n)
//     {

//         //[ ! ! ! IMPORTANT ! ! ! ] 
//         // ENABLES HANDLING OF THE EDGE CASE
//         // WHERE WE DELETE THE HEAD ITSELF!
//         ListNode dummy = new ListNode(-1, head); //Dummy node to store the result  
        
//         //First, let's get a pointer to the (n+1)th element FROM THE FRONT
//         var nPlus1ThElemFromFront = head;
//         int pos = 1;
//         while(pos<=n)
//         {
//             nPlus1ThElemFromFront = nPlus1ThElemFromFront.next;
//             pos++;
//         }
        
//         //Now, notice that the difference in position (distance) between head (1st element) and the (n+1)th element is n. So, let
//         ListNode l = dummy, r = nPlus1ThElemFromFront; //NOTICE THAT l=dummy HELPS US WITH THE CASE WHERE WE'RE DELETING THE HEAD.
//         //From earlier comment, we can see that the distance between the l.next and r is n.
//         //Therefore, we can see that if we keep incrementing l and r together at the same time,
//         //when r is at the last element, l.next will be n+1 elements before that.
//         //Which also means, that when you include the last element, 
//         //l.next is the n+1-th element from the end of the list (null).

//         //To make it handle the new l = dummy based approach for the edge case where we're deleting the head,
//         // we should just check r!=null, because when r=null (the end of the list), the distance of
//         // l.next from the end of the list is n and the distance of l from the end of the list is n+1.
//         // Which is what we need. 
//         while(r!=null)
//         {
//             l=l.next;
//             r=r.next;
//         }
//         // Console.WriteLine(l?.val);
//         // Console.WriteLine(r?.val);
//         // Console.WriteLine(l?.next?.val);
//         // Console.WriteLine(l?.next?.next?.val);

//         //as discussed above, l is the n+1-th element fromt he end of the list.
//         var lNextNext = l?.next?.next;//this is the n-1-th element from the end of the list.
//         l.next = lNextNext; //this removes the reference to the n-th node from the end and as such the garbage collector will eventually clean it up.
        
//         return dummy.next;
//     }
// }

