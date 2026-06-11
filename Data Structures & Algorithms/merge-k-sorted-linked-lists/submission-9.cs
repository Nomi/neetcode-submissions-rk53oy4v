// [ IMPORTANT ]:
// The best algorithm is actually https://www.youtube.com/watch?v=DvnxDGkjMDM
// TC = O(Nlog(K)) [same as  the PQ version]
// SC = O(1)
// Basically:
    // for (int i = 0; i < lists.Length - interval; i += interval * 2) { //O(logK * N) // Note: "interval * 2" replaces the Queue's dequeue/enqueue cycle
    //     lists[i] = MergeTwoLists(lists[i], lists[i + interval]); //O(N)
    // }
    // interval *= 2;

// ^^^ READ THIS ^^^

//# Solution from March 2026

// 2026.2 - Solution (review):
public class Solution {
    // TC = O(N*log(K))     [Same as PQ and Queue solutions]
    // Aux. SC = O(1)
    public ListNode MergeKLists(ListNode[] lists) { //O(1) Aux. SC Non-PQ and Non-Queue solution
        if (lists == null || lists.Length == 0) return null;

        int interval = 1; // The gap between lists we are merging
        
        // Note: "interval * 2" replaces the Queue's dequeue/enqueue cycle
        while (interval < lists.Length) { //O(logK * N)
            // Jump by interval * 2 to get the next distinct pair
            for (int i = 0; i < lists.Length - interval; i += interval * 2) {
                // Merge the pair and overwrite the left index with the result
                lists[i] = MergeTwoLists(lists[i], lists[i + interval]); //O(N)
            }
            interval *= 2; // Double the gap for the next pass
        }

        return lists[0]; // The fully merged list ends up at index 0
    }

    ListNode MergeTwoLists(ListNode list1, ListNode list2) { //Copy pasted my solution from the relevant problem
        //TC:O(N1+N2) and Aux SC = O(1) [at the cost of overwriting next values in the existing lists]

        ListNode dummy = new(-1);//assuming overwrites are fine
        ListNode prev = dummy;
        while(list1 != null && list2 != null) //*IMPORTANT* WE CAN KEEP THIS AS AND DO A NIFTY TRICK AT THE END!
        {            
            if(list2.val <= list1.val) //both will never be null because of the loop condition
            {
                prev.next = list2;
                list2 = list2.next;
            }
            else //Only happens when list1 !=null && list2.Val > list1.val, so no need for this check: // if(list2 == null || list1.val <= list2.val) //both will never be null because of the loop condition
            { 
                prev.next = list1;
                list1 = list1.next;
            }
            prev = prev.next; //forgot this earlier!
        }

        //*IMPORTANT* Nifty trick! Was doing it inside the while loop before seeing AI solution have it outside and keep the && as is! :(
        //*IMPORTANT* Did not think of this :( ! Had to get AI to tell me to do this, though I guess if I was nudged I would end up realizing :(
        prev.next = list1 ?? list2; //just attach the remaining tail since it is already in order!

        return dummy.next;
    }
}



// # 2026.1 - Solution(s):

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

//Better to store just the node in the list instead of the index in lists, that way you can always get next from the list you popped from!
// public class Solution { //Read DRY RUN comments from  [AI Mock Interview 1] commented out solution below!
//     //Figured it out on my first attempt since 1.5 yearrs ago in 47 minutes 
//     // (including  coming up with bruteforce and this approach (on the fly!), discussing each, their time and space complexities, and dry runs!)
//     // And without any help (except from AI suggesting I not use a dictionary to keep track of idx in lists/streams array which I also partly wanted to wipe the array to null (and get next)!
//     // But still, HUGE W! Let's goooo!!!
//     //
//     //Also, I was 3 minutes late to the end (48 instead of 45) :(! 
//     //BUT I did code it up in pretty much a text editor (had to manually add spaces even because Tab key wouldn't work + no syntax highlighting!)
//     //Surely Google interview editor has at least functional Tab key and syntax highlighting??? (I do remember them having that at least!) 

//     public ListNode MergeKLists(ListNode[] lists) { //TC = O(Nlog2(M)), SC = O(M)
//         PriorityQueue<ListNode, int> minHeap = new();
//         for(int i=0; i<lists.Length; i++) //safe from empty lists array
//         {
//             if(lists[i]==null) continue;
//             minHeap.Enqueue(lists[i], lists[i].val); //pq sorts lowest priorities first!
//             //don't need a dictionary or tuple to track indices, basically don't need to track indices or nodes
//             //as long as we store nodes in the min heap!
//         }
       
//        ListNode dummy = new(-1);
//        var prevEntry = dummy;
//        while(minHeap.Count > 0) //safe from empty lists array, because minHeap remains empty!
//        {
//             var curMin = minHeap.Dequeue();
//             var newHead = curMin.next;
//             if(newHead != null) //was missing until dry run!
//                 minHeap.Enqueue(newHead, newHead.val);
//             prevEntry.next = curMin;
//             prevEntry = prevEntry.next;
//         }
        
//         return dummy.next;
//     }
//     //oh didn't identify it in the beginning, but midway I remembered, using minheap also makes it better equipped to handle real time log streams!!
// }


// # [AI Mock Interview 1] (NO HELP FROM AI {except the suggestion to not use dictionary to track index in streams, and just put node in minHeap})
//
// /**
//  * Definition for a log entry in a linked list.
//  * public class LogEntry {
//  * public int val;
//  * public LogEntry next;
//  * public LogEntry(int val=0, LogEntry next=null) {
//  * this.val = val;
//  * this.next = next;
//  * }
//  * }
//  */
// // DR Trace Table: (sorry it's so confusing)
// // 0: streams = [(1), (1,2)]
// // 1: (the first loop): minHeap = (1,0), (1,1) //honestly, I think I didn't even need index!
// // 2: dummy = -1, prevEntry -> dummy
// // 3. minHeap.Count == 1 > 0 (second loop start:)
// // 4. minHeap = [(1,1)], cur = (1,0) => newHead = null; //oh I did not account for that! => prevEntry.next = 1, then prevEntry goes from -1 to 1, 
// // 5. minHeap.Count == 1 > 0 => continue:
// // 6. minHeap = [], cur  = (1,1) => newHead = 2 => minHeap = [(2,1))] =>> prevEntry(1).next = 1, then prevEntry is prevEntry.next (the 1 from second list)
// // 5. minHeap.Count == 1 > 0 => continue:
// // 7. minheap = [], cur = (2,1) => newHead = null => prevEntry(1 from 1 idx).next = 2, prevEntry = prevEntry.next)
// // 8. break out of loop!
// // 9. return dummy.next = 1(->1->2)
// public class Solution {
//     public LogEntry MergeKLogStreams(LogEntry[] streams) { //from clarifications: streams array could contain null entries, or be empty itself. (So since I asked if it could be null and got told it can be empty, assuming it cant!)

//        PriorityQueue<(LogEntry entry, int streamIdx), int> minHeap = new();
//        for(int i=0; i<streams.Length; i++) //safe from empty streams array
//        {
//           if(streams[i]==null) continue;
//           minHeap.Enqueue((streams[i], i), streams[i].val); //pq sorts lowest priorities first!
//        }
       
//        LogEntry dummy = new(-1);
//        var prevEntry = dummy;
//        while(minHeap.Count > 0) //safe from empty streams array, because minHeap remains empty!
//        {
//          var cur = minHeap.Dequeue();
//          var newHead = cur.Entry.next;
//          if(newHead != null) //was missing until dry run!
//             minHeap.Enqueue((newHead, cur.streamIdx), newHead.val);
//          prevEntry.next = cur.Entry;
//          prevEntry = prevEntry.next;
//        }
       
//        return dummy.next;
//     }
// }
// //oh didn't identify it in the beginning, but midway I remembered, using minheap also makes it better equipped to handle real time log streams!!




// Last Actual Solution (2024)
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
//     public ListNode MergeKLists(ListNode[] lists) {
//         return attempt1.mergeK(lists);
//     }
// }

// public static class attempt1{
//     public static ListNode merge2(ListNode head1, ListNode head2) //same as LC easy we did earlier.
//     {
//         var sortedDummy = new ListNode(-1);
//         var n = sortedDummy;
//         var n1 = head1;
//         var n2 = head2;
//         while(n1!=null&&n2!=null)
//         {
//             if(n1.val<n2.val)
//             {
//                 n.next = n1;
//                 n1 = n1.next;
//             }
//             else
//             {
//                 n.next = n2;
//                 n2 = n2.next;
//             }
//             n = n.next;
//         }
//         if(n1!=null)
//             n.next = n1;
//         else if (n2!=null)
//             n.next = n2;
//         return sortedDummy.next;
//     }

//     public static ListNode mergeK(ListNode[] lists)
//     {
//         //EDGE CASES:
//         if(lists==null||lists.Length==0)
//             return null;
        
        
//         /// DON'T NEED A CASE FOR list.length==1 because of how we do things below!
//         // int count = 20; //for debugging!
//         while(lists.Length>1)// && count>0)
//         {
//             // count--;
//             // Console.WriteLine($"===== {20-count} =====");
//             var mergedLists = new ListNode[(lists.Length+1)/2]; //+1 INSIDE THE ROUND BRACKETS for the odd element out at the end of the list.
//             //+1 MUST BE INSIDE THE BRACKETS BECAUSE WE NEED lists COUNT TO BE CORRECT TO MAKE A JUDGEMENT OF WHEN TO LEAVE!! Wrong way: (list.Length/2 +1) IS THE WRONG WAY! 
//             //CAUSES INFINITE LOOP: when only 1 element is left, we still get an array of length 2 here, where the second element is null.
//             //THEREFORE, we use the formula (lists.Length+1)/2, because it helps us get: (1+1)/2 = 1, (2+1)/2 = 2, (3+1)/2 = 2 [NOTE / IS INTEGER DIVISION, SO IT TURNCATES EVERYTHING AFTER DECIMAL POINT]
//             for(int i=0;i<lists.Length;i+=2)
//             {
//                 // Console.WriteLine($"{lists.Length}:{i},{i+1}");
//                 // string d2 = (i+1)<lists.Length && lists[i+1]!=null? lists[i+1].val.ToString() : "null_BecauseOutOfBound";
//                 // Console.WriteLine($"(vals: {lists[i].val},{d2})");
//                 var l1 = lists[i];
//                 ListNode l2 = (i+1) < lists.Length ? lists[i+1] : null; //null happens when the number of lists is null. We can see that setting it to null doesn't break our merge2 function, which ends up returning l1 itself in this case.
//                 mergedLists[i/2] = merge2(l1,l2); //i/2 is integer division, meaning it truncates, but that is fine because the array is 0 indexed.
//             }
//             lists = mergedLists;
//         }
//         return lists[0];
//     }
// }
