// 2026.1 - Solution (Review, Rushed):


public class Solution {
    static readonly List<int> EmptyList = [];

    public int[] TopKFrequent(int[] nums, int k) {
        // For MinHeap solution, refer to the previous solutions (though MinHeap section has better problems for practice, even similar ones).
        return BucketSort(nums, k);
    }

    // SC == O(N + K) = O(N) [exactly, as we always store all.]
    // TC == O(N)
    public int[] BucketSort(int[] nums, int k) {

        // Count freqs:
        Dictionary<int, int> numFreq = new();
        foreach(var num in nums) {
            numFreq.TryAdd(num, 0);
            numFreq[num]++;
        }

        // Assign each number to bucket with its frequency as idx:
        var freqBuckets = new List<int>[nums.Length+1]; //max freq possible is same number repeating everywhere
        
        foreach(var (num, freq) in numFreq) {
            freqBuckets[freq] ??= new();
            freqBuckets[freq].Add(num);
        }
        
        // Get top K most frequent elements from buckets
        int[] res = new int[k];
        int count = 0;
        for(int i = freqBuckets.Length-1; i>= 0; i--) {
            if(count == k)
                    break;
            var bucket = freqBuckets[i] ?? EmptyList;
            foreach(var num in bucket) {
                if(count == k)
                    break;
                res[count] = num;
                count++;
            }
        }

        return res;
    }
}



// 2026.1 - Solution:

// public class Solution {
//     public int[] TopKFrequent(int[] nums, int k) 
//     {
//         // Watch YT Shorts soln. for both approaches (preferably Greg Hogg or NeetCode)

//         // Bucket Sort is bad for large inputs space scales with input size (Fixed Case O(N)), 
//         // creating N+1 buckets regardless of how few unique numbers exist. 
//         // Min-Heap space scales with unique elements (U); 
//         // O(N) is just its theoretical upper bound if every number is unique.

//         //Note that N >= U (U = number of unique numbers e.g. [1,1,2] => U=2)
//         //and U>= k (you can't get top k frequent numbers if there aren't enough actual unique numbers) 

//         return MinHeapV3(nums, k);
//         // return BucketSort3(nums, k);
//     }

//     public int[] MinHeapV3(int[] nums, int k) //TC=O(N + U*log2(k) + k*log2(k))= O(N+U*log2(k))=O(N+N*log2(k)) = O(N*log2(k)),
//                                               //SC= O(k+U) = O(U) = O(N);
//     {
//         if (k == nums.Length) return nums;
        
//         PriorityQueue<int, int> minHeap = new(k+1); //SC: o(k+1)=O(k)
//         Dictionary<int, int> numToFreq = GetFrequencyMap(nums); //TC=O(N), SC = O(U) where U is number of non-repeating/unique elements (so U<=N) -> O(U) -> O(N);

//         foreach(var (num, freq) in numToFreq) //TC= O(U)
//         {
//             minHeap.Enqueue(num, freq);
//             if(minHeap.Count > k)
//             {
//                 minHeap.Dequeue();
//             }
//         }

//         var topKFrequentNums = new int[k]; //SC: O(k)
//         while(minHeap.Count > 0) // TC: O(k*log2(k))
//         {
//             topKFrequentNums[k-minHeap.Count] = minHeap.Dequeue(); //TC: O(log2(k))
//         }
//         return topKFrequentNums;
//     }

//     private Dictionary<int, int> GetFrequencyMap(int[] nums) //TC=O(N), SC = O(U) where U is number of non-repeating/unique elements (so U<=N) -> O(U) -> O(N);
//     {
//         Dictionary<int, int> numToFreq = new();
//         foreach(var num in nums)
//         {
//             if(!numToFreq.TryAdd(num, 1))
//                 numToFreq[num]++;    
//         }
//         return numToFreq;
//     }
    
    
//     public int[] BucketSort3(int[] nums, int k) //TC=O(N + U + N) = O(N), SC = O(N + U + U) = O(N)
//     {
//         //Initialize bucket storage:
//         List<int>[] freqToNums = new List<int>[nums.Length + 1]; //SC: O(N+1) = O(N) (fixed case) because frequencies range from 0 to N  //we don't care about 0, just here it acts as a sentinel/guardian index.
        
//         //Get numbers and their frequencies:
//         var numsToFreq = GetFrequencyMap(nums); //TC=O(N), SC = O(U) where U is number of non-repeating/unique elements (so U<=N) -> O(U) -> O(N);
        
//         //Fill buckets:
//         foreach(var (num, freq) in numsToFreq) //TC = O(U)
//         {
//             freqToNums[freq] ??= new();
//             freqToNums[freq].Add(num);
//         }

//         // Get top K:
//         int count = 0;
//         int[] result = new int[k];
//         for(var i = freqToNums.Length-1; i>=0; i--) //TC: O(N), SC: O(K) //TC=O(N) (directly, not O(k)-> O(N)) because we might have to go all the way down to the first bucket if all elements are unique!
//         {
//             if(freqToNums[i] == null) // IMPORTANT NOTE: Totally forgot freqToNums == null earlier!
//                 continue;
//             if(count==k)
//                 break;
//             foreach(var num in freqToNums[i]) //if more than k match, we just send any k.
//             {
//                 if(count==k)
//                     break;
//                 result[count]=num;
//                 count++;
//             }
//         }
//         return result;
//     }
// }




// Last Actual Submission:

// public class Solution {
//     //WHERE MY BUCKET SORT (frequency based buckets) AT????

//     //Bruteforce: TC = O(nlog(n))
//     //
//     //MaxHeap: TC = O(n+klog(n)) (at the end you just pop k times after heapifying in linear time using heapify)
//     //         SC = O(n)=O(n)
//     //
//     //MinHeap(best SC): 
//     //         TC = O(nlog(k)), where n is the number of elements in nums 
//     //         SC = O(k)
//     //BucketSort(best TC):  
//     //             TC= O(n)
//     //             SC = O(n) 
//     //FUCK QUICKSELECT, BUCKETSORT BETTER
//     public int[] TopKFrequent(int[] nums, int k) {
//         //INSTEAD OF MINHEAP WE USE BUCKETSORT (not even LeetCode's stupid quickselect can beat it)

//         //return BucketSort(nums, k);
//         // return MinHeapAttempt(nums, k);
//         ///* can also do maxheap, which will decrease time complexity when used with heapify (heapify is O(N) when we the data is pre-existing) so we can just pop k elements. */
//         ///* minheap decreases the spacecomplexity by making sure there's only at most k elements in the heap at any given time. */


//         // ===== PRACTICE FROM HERE: =====
//         // return bs2(nums,k);
//         return mh2(nums,k); //THIS one doesnt need tuples (as compared to other minheap version!) //but might be a tiny bit slower because of adding 1 more element than the other version?

//     }

//     // public int[] BucketSort(int[] nums, int k)
//     // {
//     //     List<int>[] freqToNums = new List<int>[nums.Count()+1]; //+1 as the biggest frequency is nums.Count() itself (because possible frequencies are IN [0,nums.Count()])!
//     //     var freqMap = new Dictionary<int,int>();
//     //     foreach(var num in nums)
//     //     {
//     //         freqMap.TryAdd(num,0);
//     //         freqMap[num]++;
//     //     }
//     //     foreach(var key in freqMap.Keys) //key is the value
//     //     {
//     //         if(freqToNums[freqMap[key]]==null)
//     //             freqToNums[freqMap[key]]=new();
//     //         freqToNums[freqMap[key]].Add(key);
//     //     }
//     //     int countAdded = 0;
//     //     int[] res = new int[k];
//     //     for(int i=freqToNums.Count()-1;i>=0&&countAdded<k;i--)
//     //     {
//     //         for(int j=0;freqToNums[i]!=null&&j<freqToNums[i].Count&&countAdded<k;j++)
//     //         {
//     //             res[countAdded]=(freqToNums[i][j]);
//     //             countAdded++;
//     //         }
//     //     }
//     //     return res;
//     // }
//     // public int[] QuickSelect(int[] nums, int k)
//     // {

//     // }

//     // public int[] MinHeapAttempt(int[] nums, int k)
//     // {
//     //     // var minHeap = new PriorityQueue<Tuple<int,int>>(Comparer<Tuple<int,int>>.Create((a,b)=>b.Item2-a.Item2)); 
//     //     var minHeap = new PriorityQueue<Tuple<int,int>,int>(Comparer<int>.Create((a,b)=>a-b));
//     //     var freqMap = new Dictionary<int,int>();
//     //     foreach(var num in nums)
//     //     {
//     //         freqMap.TryAdd(num,0);
//     //         freqMap[num]++;
//     //     }
//     //     foreach(var key in freqMap.Keys)
//     //     {
//     //         if(minHeap.Count==k)
//     //         {
//     //             if(minHeap.Peek().Item2<freqMap[key])
//     //                 minHeap.Dequeue();
//     //             else
//     //                 continue;
//     //         }
//     //         minHeap.Enqueue(Tuple.Create(key,freqMap[key]), freqMap[key]);
//     //     }
//     //     var res = new int[minHeap.Count];
//     //     int i=0;
//     //     while(minHeap.Count>0)
//     //     {
//     //         res[i]=minHeap.Dequeue().Item1;
//     //         i++;
//     //     }
//     //     return res;
//     // }


//     public int[] bs2(int[] nums, int k) 
//     {
//         int maxFreq = nums.Count(); //max possible frequency (when array only has 1 element)
//         List<int>[] freqBuckets = new List<int>[maxFreq+1]; //+1 is for 0 frequency, which technically isn't needed, but makes indexing easier.
//         Dictionary<int, int> numToFreq = new();
//         foreach(var num in nums)
//         {
//             numToFreq.TryAdd(num,0);
//             numToFreq[num]++;
//         }
//         foreach(var num in numToFreq.Keys)
//         {
//             if(freqBuckets[numToFreq[num]]==null)
//                 freqBuckets[numToFreq[num]]=new();
//             freqBuckets[numToFreq[num]].Add(num);
//         }
//         var result = new int[k];
//         int count = 0;
//         for(int i=freqBuckets.Count()-1;i>=0&&count<k;i--)
//         {
//             if(freqBuckets[i]==null)
//                 continue;
//             foreach(var num in freqBuckets[i])
//             {
//                 result[count]=num;
//                 count++;
//                 if(count==k)
//                     break;
//             }
//         }
//         return result;
//     }

//     public int[] mh2(int[] nums, int k) //This one doesn't need tuples!! //but might be a tiny bit slower because of adding 1 more element than the other version?
//     {
//         var minHeap = new PriorityQueue<int,int>(Comparer<int>.Create((a,b)=>a-b));
       
//         var freqMap = new Dictionary<int,int>();
//         for(int i = 0; i<nums.Count();i++)
//         {
//             freqMap.TryAdd(nums[i],0);
//             freqMap[nums[i]]++;
//         }
        
//         foreach(var elem in freqMap)
//         {
//             minHeap.Enqueue(elem.Key,elem.Value);
//             if(minHeap.Count>k)
//                 minHeap.Dequeue();
//         }

//         var result = new int[k];
//         int count = 0;
//         while(minHeap.Count>0)
//         {
//             result[count] = minHeap.Dequeue();
//             count++;
//         }
//         return result;
//     }
// }

