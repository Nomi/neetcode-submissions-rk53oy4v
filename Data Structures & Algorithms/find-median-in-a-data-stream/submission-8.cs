public class MedianFinder {
    IMedianFinder solver;
    public MedianFinder() {
        //READ THE COMMENTS OF THE attempt1 CLASS!!!
        // solver = new attempt1();
        // solver = new NuAttempt1();
        solver = new NuAttempt2_Review__BEST_for_Inteview();
    }
    
    public void AddNum(int num) {
        solver.AddNum(num);
    }
    
    public double FindMedian() {
        return solver.FindMedian();
    }
}


public interface IMedianFinder { //From 25-10(October)-2024 02:57 CET
    public void AddNum(int num);
    
    public double FindMedian();
}

// # Review from 13-06-26:
// The BEST Approach for Interview, even though it has a bit of performance tax!

public class NuAttempt2_Review__BEST_for_Inteview : IMedianFinder {
    // Max-Heap (Left side). We use negative priority to simulate Max-Heap.
    private PriorityQueue<int, int> smaller;
    
    // Min-Heap (Right side).
    private PriorityQueue<int, int> bigger;

    public NuAttempt2_Review__BEST_for_Inteview() {
        smaller = new PriorityQueue<int, int>();
        bigger = new PriorityQueue<int, int>();
    }

    public void AddNum(int num) {
        // Step 1: Add to Left (smaller) first
        smaller.Enqueue(num, -num);

        // Step 2: Unconditional bounce largest `smaller` value to Right (bigger)
        int maxFromSmaller = smaller.Dequeue();
        bigger.Enqueue(maxFromSmaller, maxFromSmaller);

        // Step 3: Your Asymmetrical Balance.
        // We WANT 'bigger' to hold the extra element (Right-Heavy).
        // If 'bigger' gets strictly MORE than 1 element ahead of 'smaller', pull it back.
        if (bigger.Count > smaller.Count + 1) {
            int minFromBigger = bigger.Dequeue();
            smaller.Enqueue(minFromBigger, -minFromBigger);
        }
    }

    public double FindMedian() {
        // Because of the `+ 1` balance logic above, if the count is odd,
        // 'bigger' is mathematically guaranteed to hold exactly one more element.
        if (bigger.Count > smaller.Count) {
            return bigger.Peek(); 
        }
        
        // Even total elements
        return (smaller.Peek() + bigger.Peek()) / 2.0;
    }
}



// # Last Actual Solution: from 25-10(October)-2024 02:57 CET

public class attempt1 : IMedianFinder 
{   
    //Space Complexity: O(N)
    PriorityQueue<int,int> leftHalf; //maxheap
    PriorityQueue<int,int> rightHalf; //minheap
    
    public attempt1()
    {
        leftHalf = new();
        rightHalf = new();
        // //Filling with dummy values to make our checks while adding easier (not checking if either are empty)
        // leftHalf.Enqueue(int.MinValue, int.MaxValue); //priority = int.MaxValue cuz priority queue has smalles priorty first (and -int.MinValue overflows)
        // rightHalf.Enqueue(int.MaxValue, int.MaxValue); //priority = int.MaxValue cuz priority queue has smalles priorty first
    }

    //IMPORTANT!!! TAKE A LOOK AT THIS FUNCTION (and how it's done)!!! (I had to take a peek at the NeetCodeIo solution initally a little to get how its done)
    //Time Complexity: O(nlog(n))
    public void AddNum(int num) //THIS FUNCTION REQUIRED ME TO TAKE A BRIEF LOOK AT THE NEETCODEIO SOLN!
    {
        if(rightHalf.Count != 0 && num > rightHalf.Peek()) // !=0 because we try to insert to left first, arbitrarily chosen. Could do right too.
            rightHalf.Enqueue(num, num); //minheap so we use num as prio
        else
            leftHalf.Enqueue(num, -num); //maxheap so we use -num as prio

        //Make sure the 'halves' are balanced:
        if(leftHalf.Count > rightHalf.Count+1) //+1 for when queue has odd number of elements
        {
            int largestOnLeft = leftHalf.Dequeue();
            rightHalf.Enqueue(largestOnLeft, largestOnLeft); //minheap so we keep smallestOnRight as priority
        }
        else if(rightHalf.Count > leftHalf.Count+1) //+1 for when queue has odd number of elements
        {
            int smallestOnRight = rightHalf.Dequeue();
            leftHalf.Enqueue(smallestOnRight, -smallestOnRight); //maxheap so we use (-largestOnLeft) as priority
        }
    }

    //Time Complexity: O(1)
    public double FindMedian()
    {
        //odd number of numbers:
        if(leftHalf.Count!=rightHalf.Count) 
            return leftHalf.Count>rightHalf.Count ? leftHalf.Peek() : rightHalf.Peek();
        
        //even number of numbers:
        return (((double)leftHalf.Peek()+rightHalf.Peek())/2.0);
    }
}



// # Solution from 12-03(March)-2026
public class NuAttempt1 : IMedianFinder {
    //So, anyhow, why is a Hard problem easier than all Medium Heap ones?? :O
    // Nvm, it was plenty hard.
    PriorityQueue<int, int> leftMax;
    PriorityQueue<int, int> rightMin;

    // PriorityQueue<int, int> smallMax;
    // PriorityQueue<int, int> largeMin;

    public NuAttempt1() {
        leftMax = new();
        rightMin = new();
        // smallMax = new PriorityQueue<int, int>();
        // largeMin = new PriorityQueue<int, int>();
    }
    
    public void AddNum(int num) {
        //we want a right heavy approach, Why?
        // So Top of right min gives median for Odd. Or the right middle value for even.
        // Left gives left middle value!


        // Where does current element LIKELY belong?
        if(rightMin.Count == 0 || rightMin.Peek() <= num) //in case of absolute <, it definitely belongs there :D
        {
            rightMin.Enqueue(num, num);
        }
        else //rightMin.Count > 0 && rightMin.Peek() > num 
        {
            leftMax.Enqueue(num, -num);
        }


        // Balancing in case of wrong size
        if(rightMin.Count > 1 + leftMax.Count)
        {
            var smallestRight = rightMin.Dequeue();
            leftMax.Enqueue(smallestRight, -smallestRight);
        }
        else if (leftMax.Count > rightMin.Count)
        {
            var biggestLeft = leftMax.Dequeue();
            rightMin.Enqueue(biggestLeft, biggestLeft);
        }

        // I had to ask a lot of questions from Gemini AI to get it. 
        // (I mean, my mind was trying to go to a solution where I don't place things into a minheap until certain of what to do. I know it's possible, I just can't prove it XD.)
        // So, I really need to practice it at least once again. So I have this down.
    }
    
    public double FindMedian() {
        if(rightMin.Count + leftMax.Count == 0)
            throw new Exception();

        if((leftMax.Count + rightMin.Count)%2 != 0)
            return rightMin.Peek();
        //else
        return (leftMax.Peek()+rightMin.Peek())/(double)2;
    }
}