

// FOR PROBLEMS LIKE THESE, 
// WHERE IN THE MONOTONIC STACK YOU SIMPLY POP AND DO NOTHING WITH THE POPPED VALUE:
// JUST USE THE O(1) AUX SPACE APPROACH BELOW!



//Probably deprecated:
//My third submission without fixes was approximately 35 minutes in. With fixes let's say 40. BUT tbf,my biggest time waster was still sort!
//testing my third submission from this attempt to see if it works after just switching sort direction:
//STILL NEEDED THE FOLLOWING FIXES:
// -> changing loop direction, think that's the issue. At some point I switched the sorting and in my head I forgot about it and treated it like I was sorting it the other way?? Seems more likely than messing up the syntax since I seem to have made corresponding change to the `>=` -> `<=`. I also am not that familiar with sorting like this, that's why I prefer orderby :')
//      being sleep deprived, exhausted, after working whole day, I messed up in syncing these changes or remembering making them probably.
// -> [*EXTREMELY IMPORTANT*!] (double) divisoion instead of integer division. This is on me. I need to always be careful when dividing ints! Though lately I was considering them, need to drill this into my head even more!
// -> I do have a list of common edge cases so i should add the integer division to that if it isn't there already!
public class Solution {
    public int CarFleet(int target, int[] position, int[] speed)
    {
        return 
            CarFleet_REVIEW1_NuAttempt4Review1_BestVersion_NoAuxSpace_REVIEW1(target, position, speed)
            // CarFleet_NuAttempt4_BestVersion_NoAuxSpace(target, position, speed)
            // CarFleet_NuAttempt4_MonotonicStack(target, position, speed)
            // CarFleet_NuAttempt3_BestSolution(target, position, speed)
            // CarFleet_NuAttempt2_MonotonicStackVersion(target, position, speed)
            // CarFleet_NuAttempt1_HalfwayOptimized_Stack(target, position, speed)
            // CarFleet_LastActualSoln_Attempt1(target, position, speed)
            ;
    }

    public int CarFleet_REVIEW1_NuAttempt4Review1_BestVersion_NoAuxSpace_REVIEW1(int target, int[] position, int[] speed) {
        // For an actual explanation, refer to `CarFleet_NuAttempt4_BestVersion_NoAuxSpace`
        double nextLargestTimeToDest = int.MinValue; // Slowest fleet right after current
        int numFleets = 0;

        Array.Sort(position, speed);
        
        for(int i=position.Length-1; i>=0; i--) {
            var curTimeToDest = (target-position[i])/(double)speed[i];
            if(curTimeToDest <= nextLargestTimeToDest) {
                continue; //joins the next fleet.
            }

            //Else if curTimeToDest > nextLargestTimeToDest:
            numFleets++;
            nextLargestTimeToDest = curTimeToDest;
        }

        return numFleets;
    }

    public int CarFleet_NuAttempt4_BestVersion_NoAuxSpace(int target, int[] position, int[] speed) 
    { //TC: O(N + Nlog(N)) = O(Nlog(N)), AUX SC=O(1)
        
        // Sort
        Array.Sort(position, speed); //ascending order

        // State trackers
        double nextLargestTimeToDistance = int.MinValue; 
        int numFleets = 0;
        
        // Check, FROM REVERSE, WHICH CAR WILL BLOCK CURRENT CAR AT I?
        for(int i=position.Length-1; i >= 0; i--) //in my exhaustion/sleepiness, I had just i > 0 (though to be honest, it was because I was being stupid earlier and had <nums.Length but changed it right away, but only switched < to >)
        {
            double iTtd = (target - position[i])/(double)speed[i]; //whoops, forgot the speed earlier!
            if(iTtd <= nextLargestTimeToDistance)
                continue; //this joins up the fleet after this!
            nextLargestTimeToDistance = iTtd; //this is now the new bottleneck!
            numFleets++; //because it didn't join up with the one after this!
        }

        return numFleets; //7 mins overall
    }

    public int CarFleet_NuAttempt4_MonotonicStack(int target, int[] position, int[] speed) { //hmm kinda seems like I'm merging pre sorted intervals. 
        // Pattern: Next Greater (Time to Distance)
        //I already know this uses a monotonic stack :(, so not the best test for pattern finding?

        //TC: O(N + Nlog(N)) = O(Nlog(N)), AUX SC=O(N)
        Array.Sort(position, speed); //sorts ascending by default. 
        // I WAS PRETTY MUCH DONE AT 16 MINUTES BUT FOR REASONS IN THE COMMENTS AT THE END I FORGOT TO SORT THE ARRAYS </3

        Stack<double> fleets = new(); 
        //I just remember from last time we store the time to destination in the stack in this problem
        // for convenience, but I'm not sure I could recall that for other problems like this. (earlier I would be storing a tuple of speed and position!)
        //Also remembered that I need to store double, not int!

        for(int i=0; i<position.Length; i++)
        {
            // double iTtd = position[i]/(double)speed[i];//time to distance // verifying -> km/(km/h) => h
            // I AM STUPID BECAUSE I HAD `position[i]/(double)speed[i];`
            double iTtd = (target-position[i])/(double)speed[i];//time to distance // verifying -> km/(km/h) => h
            while(fleets.Count>0 && fleets.Peek()<=iTtd) //MISSED THE <= (had <) EARLIER ONLY CAUGHT DUE TO ADDING THE NOTE BELOW!
                                                          //almost did fleet.Peek() ALSO, had <=fleets[i] there because I'm stupid?                           
            {
                //I need to stop spacing out and just staring at the screen, damn!

                //fleets.Peek()<=fleets[i] implies the prior fleet should reach before, BUT
                //but the current car is the BOTTLENECK!
                fleets.Pop();
            }
            fleets.Push(iTtd);
        }

        return fleets.Count; //finished most of it in 15mins, but 2 runs failed, fixed part of it 
        //OKAY NEVERMIND, THIS WAS JUST A CONSEQUENCE OF ME TRYING TO SPEEDRUN THROUGH THIS BASED ON MY MEMORY OF WHAT THE PROBLEM EXACTLY WAS
        // I FORGOT TO ACCOUNT FOR SORTING BOTH ARRAYS BY POSITION ARRAY!

        // I WAS PRETTY MUCH DONE AT 16 MINUTES BUT FOR REASONS IN THE ABOVE COMMENT I FORGOT TO SORT THE ARRAYS </3
    }
    
    public int CarFleet_NuAttempt3_BestSolution(int target, int[] position, int[] speed) { //TC = O(N * log(N)), Aux SC = O(1)
    //## IMPORTANT ADDITION TO THE NOTE AFTER THIS:
    // It only works if either the input is pre-sorted, 
    // or the nature of the problem demands that we sort it regardless (which in car fleet, it does! Why? So we can pick the right order of bottlenecks!)

    //## NOTE: 
    //In this specific problems, it just so happens we don't need Monotonic stacks since at any given point from right to left (righ closer to target),
    //we only care about the most recent bottleneck, and once a new bottleneck is found, we do not care about the previous bottleneck ever.
    // There might be other problems where we aboslutely need a Monotonic stack for best solution, but this one just happens to be one where it is
    // a suboptimal approach.

        if(position.Length < 1)
            return 0;
        int N = position.Length;

        Array.Sort(position, speed); // TC: O(N*log2(N)) Aux SC: O(1), as long as we're allowed to modify input //For descending, could add argument Comparer<int>.Create((a, b) => b.CompareTo(a))
        
        //So, we only need these 2 variables:
        //I realized halfway through NuAttempt1 we don't need the stack or its monotonicity but I didn't change because I was in stacks section, but now that I have solved it both ways with stacks, time for the best solution! (but also didn't want to break anything)
        double bottleneck = CalculateTimeToTarget(target, position[N-1], speed[N-1]); //named bottleneck for now, but if I find a better name, i'll write it. But it's basically the SLOWEST car from the end we have processed that has highest time to distance (slowest speed), meaning which is the bottleneck!
        int fleets = 1;
        //WRONG: Silly mistake: idk why on the bottleneck part i had indices as i instead of 0 even though it's not part of the loop and I hadn't even written the loop yet, I only caught it when copying it over to the loop to change!
        //*MAJOR MISTAKE*: SHOULD BE LAST ELEMENTS! (I onlt realized because a test case failed on NC)
        for(int i = position.Length - 2; i >= 0; i--) //iterating from cars closest to target (based on sorting)!
        {
            var curTtt = CalculateTimeToTarget(target, position[i], speed[i]);
            //1. Car has same time to target as the bottleneck => BOTH fleets and bottleneck remain UNCHANGED
            //2. Car less time to target than bottleneck => bottlnecked, BOTH fleets and bottleneck remain UNCHANGED
            //3. Car has more time to target than current bottleneck => SLOWER CAR, this car is separate from old fleet and is the new bottleneck
            if(curTtt > bottleneck)
            {
                fleets++;
                bottleneck = curTtt;
            }
        }

        return fleets;
    }

    private double CalculateTimeToTarget(double target, double position, double speed) //putting arguments as double removes need to add more casts
     => (target-position)/speed;

    public int CarFleet_LastActualSoln_Attempt1(int target, int[] position, int[] speed) // READ `NuAttempt` SOLUTIONS (attempts) INSTEAD !!! //DO NOT USE THIS! 
    {
        var pairs = position.Zip(speed,(p,s)=>(p,s)).ToArray(); //tuple has .first/.second????
        pairs = pairs.OrderByDescending(x=>x.p).ToArray();
        //A LESSON ON TUPLES:
        //Could also use (int p, int s)[] pairs =...; 
        //OR
        //(a,b)=>(p: a, s: b);
        //READ UP ON https://www.bytehide.com/blog/tuple-csharp
        //For loops, an also use: 
        //foreach (var (p, s) in myList) and access them separately
        // OR EVEN var (p, s) = myList[0] to use them separately.
        // Array.Sort(pairs, x=> x.Item1);
        var fleet = new Stack<(int p,int s)>();
        
        foreach(var (p, s) in pairs)
        {
            if(fleet.Count==0)
            {
                // Console.WriteLine($"{target}: {p},{s}");
                fleet.Push((p,s));
                continue;
            }
            var (lp, ls) = fleet.Peek();
            //REMEMBER: YOU GOTTA CAST THEM AS DOUBLES TO GET ACTUAL TIMES!!!
            double lastFleetTimeToDist = (double)(target-lp)/ls;
            double timeToDistCurrCar = (double)(target-p)/s;
            // Console.WriteLine($"{target}: {p},{s}={timeToDistCurrCar} -- {lp},{ls}={lastFleetTimeToDist}");
            if(lastFleetTimeToDist<timeToDistCurrCar)
                fleet.Push((p,s));
        }
        return fleet.Count;
    }

    public int CarFleet_NuAttempt2_MonotonicStackVersion(int target, int[] position, int[] speed) { //TC = O(Nlog(N)), Aux SC = O(N)
        Array.Sort(position, speed); // Aux SC: O(1), as long as we're allowed to modify input
        var fleets = new Stack<double>(); // double due to lessons from my NuAttempt1

        for(int i=0; i<position.Length; i++)
        {
            // (int pos, int spd) car = (position[i], speed[i]); //Note: gotta make sure i have my order correct!
            double timeToTarget = (target - position[i])/((double)speed[i]); //almost missed the double again, but caught right after writing this line halfway!
            //lol I forgot the division sign above earlier haha

            //## What can happen here? 
            //1. fleet is empty, we can add the car here.
            //2. the current car after the previous car/fleet will reach the target before this one, and as such won't meet here or at the end
            // so, the next car just creates a new fleet.
            //3. current car reaches the target same time or later than earlier car would have if not bottlenecked by this,
            //  so, we merge the fleets and new car's time to target becomes its time to target. 
            //  (I almost forgot to mention: this will happen for all fleets faster than current one)
            //## WRONG: As such, we are clearly making a monotonic stack where speeds increase from bottom to top! (monotonic increasing!)
            //## FIXED: As such, we are clearly making a monotonic stack where TIME TO TARGET STRICTLY decreases from bottom to top! (monotonic decreasing!)

            while(fleets.Count>0 && fleets.Peek() <= timeToTarget)
            {
                fleets.Pop();
            }

            fleets.Push(timeToTarget); //wait, I could have used the cross multiplication trick for precision though it could make logic more complicated, I am not sure.
        }

        return fleets.Count; //NOTE: was fleets.Count(); but THIS IS O(N) BECAUSE IT IS ENUMERATING METHOD ON LINQ! USE THE PROPERTY FOR O(1)
    }

    public int CarFleet_NuAttempt1_HalfwayOptimized_Stack(int target, int[] position, int[] speed) { //TC = O(Nlog(N)), Aux SC = O(N+N) = O(N)
        // Input:
        // target = distance is at `target` miles
        // speed gives speed of ith car in miles per hour
        // position gives position of ith car in miles 
        // len of speed == position

        // Constraints:
        // - A car can not pass another car ahead of it. 
        //   It can only catch up to another car and then drive at the same speed as the car ahead of it.
        // -  (Wait does that mean it's on the same step as that car or 1 behind?)
        //     (assuming it''s on the same step because I remember this problem, in interview, would confirm)
        // - Nvm, answered here: A car fleet is a non-empty set of cars driving at the same position and same speed.
        // - If a car catches up to a car fleet the moment the fleet reaches the destination, then the car is considered to be part of the fleet.
        //      (this is one of the edge cases, or more like how we want to handle it. Not sure I'd remember it in interview!)

        // Return:
        // - Number of different car fleets that will arrive at the destination.

        // Assumptions: 
        //(assuming it starts from 0 miles)
        // no numerical overflow

        //Alright, 6 minutes to clarify the problem, but I will not write here about the examples, just go through them!


        // # Solution
        // Well, I knew it is solved with monotonic stacks before I even started, so that's kinda cheating, but I did realize why now:
        // - Each car is limited by the next SLOWER TO REACH DESTINATION than it, but only if we consider them in order of who is nearest to the finish in position!
        
        // IMPORTANT NOTE [at 20mins]: OKAY, I WAS DUMB, I don't know why I thought/assumed the array would be sorted, saw the failing case and that doesn't have sorted position (maybe the best case dry run wouldn't have caught it anyway, but hopefully interviewer would point it out without negative impression)
        //also, thought about sorting, then checked the recommended time complexity and lo and behold, it does have nlogn time complexity so I should be allowed!
        // Array.Sort(position) //oh wait if we only sort this we'll be in trouble! Thinking of creating a list of tuples but would require extra space? Lo and behold, SC O(N) in recommended time and space complexity!
        // [~5 min later] BUT WAIT, that could also have been just for the stack? Though this still seems the best solution! Wait, actually, we really only care about the last car and can maintain a counter otherwise. Only this list will take up time. My blind knowledge of this being monotnonic stack caused this, but at least I know how to fix it if needed

        List<(int pos, int spd)> cars = new(position.Length);
        for(int i=0; i < position.Length;i++) //actually, could've used a priorityqueue maybe?
        {
            cars.Add((position[i], speed[i]));
        }
        cars.Sort((x,y)=> x.pos.CompareTo(y.pos));
        // cars = cars.OrderBy(x => x.pos).ToList(); //I tried to do the above to be inplace, should've stuck with what I knew

        // int count = 0;
        var fleets = new Stack<double>();
        for(int i=cars.Count-1; i>=0; i--)// THIS WAS WRONG: foreach(var car in cars) //for(int i=position.Length-1; i>=0; i--)
        {
            //NOTE: Didn't think much of it myself but apparently important that this solution adds 1 more element to the stack than required (but also makes code simpler tbf)
            var car = cars[i];
            // var timeToTarget = (target - position[i])/speed[i]; //Late realization but, is it guaranteed speed >0 ?? //actualy yeah, 1 of the constraints is: 0 < speed[i] <= 100
            double timeToTarget = (target - car.pos)/(double)car.spd; //Late realization but, is it guaranteed speed >0 ?? //actualy yeah, 1 of the constraints is: 0 < speed[i] <= 100
            //while(fleets.Count > 0 && fleets.Peek()>=timeTo) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
            //if(fleets.Count > 0 && fleets.Peek()>=timeToTarget) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
            if(fleets.Count > 0 && fleets.Peek()>=timeToTarget) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
                continue; //the top "fleet" on stack acts as the representative, because this faster car/fleet is bottlenecked by them. We don't need to push it too! It joined the fleet ;).
            
            fleets.Push(timeToTarget);
        }
        return fleets.Count;
    }
}


//## FOR NuAttempt1: My conversation with an AI as an "interviewer" for interview-esque nudges/hints (wasn't that helpful as it wasted a lot of time without mentioning sorting direction and loop direction being off, but maybe that's just me and it did try to indirectly nudge me in an interviewer-esque way and I just missed?):

// public class Solution {

//     public int CarFleet(int target, int[] position, int[] speed) {

//         // Input:

//         // target = distance is at `target` miles

//         // speed gives speed of ith car in miles per hour

//         // position gives position of ith car in miles

//         // len of speed == position



//         // Constraints:

//         // - A car can not pass another car ahead of it.

//         //   It can only catch up to another car and then drive at the same speed as the car ahead of it.

//         // -  (Wait does that mean it's on the same step as that car or 1 behind?)

//         //     (assuming it''s on the same step because I remember this problem, in interview, would confirm)

//         // - Nvm, answered here: A car fleet is a non-empty set of cars driving at the same position and same speed.

//         // - If a car catches up to a car fleet the moment the fleet reaches the destination, then the car is considered to be part of the fleet.

//         //      (this is one of the edge cases, or more like how we want to handle it. Not sure I'd remember it in interview!)

//         // - A single car is also considered a car fleet. [added later but I read it or knew it beforehand]



//         // Return:

//         // - Number of different car fleets that will arrive at the destination.



//         // Assumptions:

//         //(assuming it starts from 0 miles)

//         // no numerical overflow



//         //Alright, 6 minutes to clarify the problem, but I will not write here about the examples, just go through them!





//         // # Solution

//         // Well, I knew it is solved with monotonic stacks before I even started, so that's kinda cheating, but I did realize why now:

//         // - Each car is limited by the next SLOWER TO REACH DESTINATION than it, but only if we consider them in order of who is nearest to the finish in position!

       

//         // IMPORTANT NOTE [at 20mins]: OKAY, I WAS DUMB, I don't know why I thought/assumed the array would be sorted, saw the failing case and that doesn't have sorted position (maybe the best case dry run wouldn't have caught it anyway, but hopefully interviewer would point it out without negative impression)

//         //also, thought about sorting, then checked the recommended time complexity and lo and behold, it does have nlogn time complexity so I should be allowed!

//         // Array.Sort(position) //oh wait if we only sort this we'll be in trouble! Thinking of creating a list of tuples but would require extra space? Lo and behold, SC O(N) in recommended time and space complexity!

//         // [~5 min later] BUT WAIT, that could also have been just for the stack? Though this still seems the best solution! Wait, actually, we really only care about the last car and can maintain a counter otherwise. Only this list will take up time. My blind knowledge of this being monotnonic stack caused this, but at least I know how to fix it if needed



//         List<(int pos, int spd)> cars = new(position.Length);

//         for(int i=0; i < position.Length;i++) //actually, could've used a priorityqueue maybe?

//         {

//             cars.Add((position[i], speed[i]));
//             // Console.WriteLine($"{cars[i]}"); //ohh this prints the pairs properly!
//         }

//         // cars.Sort((x,y)=> y.pos.CompareTo(x.pos)); //Nah, that wouldn't work because bottleneck!://maybe could sort by time to target?
//         cars.Sort((x,y)=> x.pos.CompareTo(y.pos)); //Nah, that wouldn't work because bottleneck!://maybe could sort by time to target?

//         // cars = cars.OrderBy(x => x.pos).ToList();


//         // int count = 0;
//         var fleets = new Stack<double>(); // *IMPORTANT NOTE* WAS <int>!! I NEED TO NOT FORGET THIS WHEN DIVIDING INTEGERS EVER!!! LIKE EVER!~!! Make notes to be ware??
//         for(int i=cars.Count-1; i>=0; i--)// THIS WAS WRONG: foreach(var car in cars) //for(int i=position.Length-1; i>=0; i--)
//         {
//             var car = cars[i];

//             // var timeToTarget = (target - position[i])/speed[i]; //Late realization but, is it guaranteed speed >0 ?? //actualy yeah, 1 of the constraints is: 0 < speed[i] <= 100
//             double timeToTarget = (target - car.pos)/(double)car.spd; //*Important Note*: Late realization but, is it guaranteed speed >0 ?? //actualy yeah, 1 of the constraints is: 0 < speed[i] <= 100

//             // Console.WriteLine($"{timeToTarget} : {cars[i]}"); //ohh this prints the pairs properly!

//             //while(fleets.Count > 0 && fleets.Peek()>=timeTo) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
//             //if(fleets.Count > 0 && fleets.Peek()>=timeToTarget) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
//             if(fleets.Count > 0 && fleets.Peek()>=timeToTarget) //`>=` and not `>` because of the condition of different fleets combining at the destination at the same time
//                 continue; //the top "fleet" on stack acts as the representative, because this faster car/fleet is bottlenecked by them. We don't need to push it too! It joined the fleet ;).
//             //I switched the above to <= timtToTarget somehow mid writing, which makes no sense!
//             // we keep the slowest there!


//             //while doing a dry run on the example (check comments below this loop), I realized the above condition is not the full picture.
//             // the cars only join up, if the time taken for newer car is lesser than or equal to current one!
//             fleets.Push(timeToTarget);

//             //note added pretty late into debugging: fleets needs to be monotnonic increasing in timeToTarget (top is smallest/fastest), we just need to check at every step whether there's a fleet slower or same finish time than this car AFTER this car
//         }

//         // for the failing test case on nc: target=10, position=[0,4,2],speed=[2,1,3]
//         // cars = [i0=(0,3), i1=(2,3), i2=(4,1)]
//         //i = 2 -> ttt = (10-4)/1 = 6 => fleets = [6]
//         //i = 1 -> ttt = (10-2)/3 = 2.9... => fleets =[6]
//         //i = 0 -> ttt = (10-0)/2 = 5 => fleets =[6]
//         // Console.WriteLine(fleets.Pop());
//         // Console.WriteLine(fleets.Pop()); //Somehow our stack for this example actually ends up being [1,2] which is correct in order, but nothing else??
//         return fleets.Count;

//     }



//     //target = 10,
// } //Final submissions t 1h:24m:32s but I don't think that's truly representative of how much it normally would've in interview setting. While I did try to use Gemini as an interviewer with nudges, it was uselss because it never pointed out the sorting!



// Here's a transcript of my "interview" with Gemini as the interviewer (basically there for realistic nudges and hints):
// Interviewer: I'm following. Let's take a look at what you've written so far. Why don't you walk me through your logic for the main loop? Specifically, I'd like to see how you're handling the termination condition.

// User: if(fleets.Count > 0 && fleets.Peek()>=timeToTarget)

// Interviewer: Okay, let's walk through that specific condition. If the timeToTarget for the current car is greater than the time of the fleet ahead of it, what does that mean for our fleet count?

// User: Then it means it forms a new fleet

// Interviewer: Right. And looking at your stack operation there—specifically fleets.Peek() >= timeToTarget—how does that align with your logic for forming a new fleet versus joining an existing one?

// User: oh I might have it the other way around

// Interviewer: Let's trace that with a quick example. If the car in front—the one already in your fleets stack—has a timeToTarget of 5, and the current car you're looking at has a timeToTarget of 10, what should happen there?

// User: Oh I think I get it, when I carelessly switched from the for loop to foreach, I lost the direction of the original intent, breaking the inner logic.

// Interviewer: That’s a good catch. If you change the direction of your iteration, how does that affect the relationship between the car you're currently looking at and the 'lead' car currently at the top of your stack?

// User: it just means we're letting different car fleets merge and ones that should be the same stay separate

// Interviewer: Okay, so if we're iterating in this new direction, does the stack still represent the car immediately in front of the current one? How are you ensuring that the 'lead' car is always the one you're comparing against?

// User: I will be reversing the loop direction, because that is the easiest fix. i do know how to fix it the other way around (basically, when we find a next car slower or equal to the current one, we pop current and add the new timeToTarget. (it is the same as monotonic stack))

// Interviewer: That sounds like a reasonable path forward. Reversing the iteration is a common way to simplify the dependency logic in this problem.

// User: 
// cars.Sort((x,y)=> y.pos.CompareTo(x.pos));
// // cars = cars.OrderBy(x => x.pos).ToList(); //I tried to do the above to be inplace, should've stuck with what I knew
// for(int i=cars.Count-1; i>=0; i--)
// {
//     var car = cars[i];
//     double timeToTarget = (target - car.pos)/(double)car.spd;
//     if(fleets.Count > 0 && fleets.Peek()>=timeToTarget)
//         continue;
//     fleets.Push(timeToTarget);
// }

// Interviewer: I see you're using int for your fleets stack and the timeToTarget calculation. If we have a target of 10, a car at position 8 with speed 3, and another car at position 0 with speed 1, how would those timeToTarget values be represented in your current code?

// User: okay, I am pretty much lost from being tired and this being midnight and my stupidity... [mentions sorting logic and bottleneck realizing]

// Interviewer: It’s definitely a long day, but you're hovering right over the solution. Let’s look at the sequence one more time. You’ve sorted the cars list so that the car closest to the target is at index 0. However, your loop starts at cars.Count - 1 and moves backwards.

// User: FUCK THE ONLY WRONG THING WAS I WAS SORTING WRONG DIRECTION

// Interviewer: It happens—especially when you're context-switching between the sort order and the loop direction. It’s easy to end up processing the cars 'away' from the target instead of 'toward' it.

// User: would an interviewer fail me if I missed this because in my mind I am ordering it correctly?

// Interviewer: Honestly? Take a breath. It’s midnight, and 'midnight brain' is a very real thing in interviews. To answer your question directly: No, a single mistake in sorting direction or loop indexing is rarely an automatic 'Fail.'

// User: Like, I intended to sort it correctly :')

// Interviewer: I get it. The gap between what's in your head and what hits the editor is where 90% of bugs live—especially at midnight.

// User: not but I fixed it already?


//-------------------------------------

// Last Actual Solution:
// public class Solution {
//     public int CarFleet(int target, int[] position, int[] speed) {
//         return attempt1(target, position, speed);
//     }

//     public int attempt1(int target, int[] position, int[] speed)
//     {
//         var pairs = position.Zip(speed,(p,s)=>(p,s)).ToArray(); //tuple has .first/.second????
//         pairs = pairs.OrderByDescending(x=>x.p).ToArray();
//         //A LESSON ON TUPLES:
//         //Could also use (int p, int s)[] pairs =...; 
//         //OR
//         //(a,b)=>(p: a, s: b);
//         //READ UP ON https://www.bytehide.com/blog/tuple-csharp
//         //For loops, an also use: 
//         //foreach (var (p, s) in myList) and access them separately
//         // OR EVEN var (p, s) = myList[0] to use them separately.
//         // Array.Sort(pairs, x=> x.Item1);
//         var fleet = new Stack<(int p,int s)>();
        
//         foreach(var (p, s) in pairs)
//         {
//             if(fleet.Count==0)
//             {
//                 // Console.WriteLine($"{target}: {p},{s}");
//                 fleet.Push((p,s));
//                 continue;
//             }
//             var (lp, ls) = fleet.Peek();
//             //REMEMBER: YOU GOTTA CAST THEM AS DOUBLES TO GET ACTUAL TIMES!!!
//             double lastFleetTimeToDist = (double)(target-lp)/ls;
//             double timeToDistCurrCar = (double)(target-p)/s;
//             // Console.WriteLine($"{target}: {p},{s}={timeToDistCurrCar} -- {lp},{ls}={lastFleetTimeToDist}");
//             if(lastFleetTimeToDist<timeToDistCurrCar)
//                 fleet.Push((p,s));
//         }
//         return fleet.Count;
//     }
// }

