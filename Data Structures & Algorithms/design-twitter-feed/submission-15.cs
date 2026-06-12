
public class Twitter {
    ITwitter backend;
    public Twitter() {
        //CHECK ATTEMPT1 COMMENTS!!!
        //Watch neetcode video?
        //maybe compare with neetcodeio c# soln?
        backend = new 
            //attempt1();
            NuAttempt1();

        // # Textual Review/practice without writing code from 13-06-26 (a few months after NuAttempt1):
        //
        //There should be a dictionary of userId to HashSet of followees' IDs (sorted by newest posts first)
        //
        // Then a dictionary of userId to LinkedList of their posts
        //
        // Also, an int TimeStamp that is incremented on every operation.
        //
        // Then finally:
        //
        // PostTweet: -> if == 10 post length, remove last. Then add post first.
        //
        // Also, call follow on user to themselves here (I was thinking of doing it ine very function then accidentally saw my last solution from a few months ago and realized I only need to do it when a post actually exists)
        //
        // getNewsFeed: -> Use a PQ of LinkedListNodes to get top 10 newest feeds. (top K pattern), 
        //      then clone merge the linkedlist of feeds with the help of the same resulting PQ to get the 10 most recent posts. 
        //      (runs exactly for 10 iterations btw, obviously)
        //
        // void follow: add followee to follower's follow dict
        //
        // void unfollow: the other way around 
        //
        // ## Note:
        // I didn't remember that I need to make sure that a user CANNOT unfollow themselves!
        //
    }
    
    public void PostTweet(int userId, int tweetId) {
        backend.PostTweet(userId, tweetId);
    }
    
    public List<int> GetNewsFeed(int userId) {
        return backend.GetNewsFeed(userId);
    }
    
    public void Follow(int followerId, int followeeId) {
        backend.Follow(followerId, followeeId);
    }
    
    public void Unfollow(int followerId, int followeeId) {
        backend.Unfollow(followerId, followeeId);
    }
}

// # From 24-10(October)-2024 00:59 CET!
public interface ITwitter 
{
    public void PostTweet(int userId, int tweetId);
    
    public List<int> GetNewsFeed(int userId);
    
    public void Follow(int followerId, int followeeId);
    
    public void Unfollow(int followerId, int followeeId);
}

// # Last Actual Solution: From 24-10(October)-2024 00:59 CET!
public class attempt1 : ITwitter 
{
    Dictionary<int, HashSet<int>> followMap; //EDGE CASE: INITIALLY HAD List<int> for followed users because I didn't consider the possiblity of keeping followers unique (and constant time unfollow)
    Dictionary<int, List<(int content, int time)>> tweetMap;
    int time;
    const int MAX_TWEETS_IN_FEED = 10;

    public attempt1() {
        followMap = new();
        tweetMap = new();
        time = 0; 
    }
    
    public void PostTweet(int userId, int tweetId) {
        Follow(userId, userId);//this is to make sure a user sees tweets they made //HAD MISSED THIS EARLIER (didn't read description properly I guess?)
        time++;
        tweetMap.TryAdd(userId, new List<(int,int)>(){});
        tweetMap[userId].Add((tweetId, time));
    }
    
    public List<int> GetNewsFeed(int userId) {         
        if(!followMap.ContainsKey(userId))
            return new(){};
        
        PriorityQueue<(int content, int time),int> minHeap = new(MAX_TWEETS_IN_FEED);
        foreach(int followeeId in followMap[userId])
        {
            if(!tweetMap.ContainsKey(followeeId))
                continue;
            foreach(var tweet in tweetMap[followeeId])
            {
                if(minHeap.Count<MAX_TWEETS_IN_FEED)
                {
                    minHeap.Enqueue(tweet, tweet.time);
                }
                else if(minHeap.Peek().time<tweet.time)
                {
                    minHeap.Dequeue();
                    minHeap.Enqueue(tweet, tweet.time);
                }
            }
        }

        //IMPORTANT NOTE!!!!
        //MISSED THE LINE : "Tweets IDs should be ordered from most recent to least recent." earlier.
        //If the input wasn't so small, MAYBE? we should have used SortedSet (like in NeetCodeIo C# example) to avoid making a redundant int[] which gets copied to create a List<int> to return.
        int[] res = new int[minHeap.Count];
        while(minHeap.Count>0)
        {
            res[minHeap.Count-1] = minHeap.Dequeue().content;
        }
        return new List<int>(res);
    }
    
    public void Follow(int followerId, int followeeId) {
        followMap.TryAdd(followerId, new HashSet<int>());
        followMap[followerId].Add(followeeId);
    }
    
    public void Unfollow(int followerId, int followeeId) {
        if(followerId==followeeId) //FORGOT ABOUTT THIS EDGE CASE AS WELL UNTIL LATE INTO THE GAME!! (maybe because I added the functionality to follow the user themselves autmatically just recently / a minute ago as well)
            return;
        if(followMap.ContainsKey(followerId)) //forgot about checking for this edge case earlier
            followMap[followerId].Remove(followeeId);
    }
}

// # From 12-03(March)-2026
public class NuAttempt1 : ITwitter {

    // *IMPORTANT* Next time do with a timer counter in our class instead of assuming tweets ID'd chronologically!


    // I remember reading LinkedLists in the topic and I do remember merge k sorted linked lists 
    // (and here posts are given to be sorted chronologically, I assume)
    // So, we can use a PQ of LinkedLists (well, generally their nodes but here we have that luxury!!
    // I do wonder why I cant just use a normal list (and its last element as first)?
    // Ohh, maybe because that allows us to hold the same memory and not do things twice? BUT WAIT, THat's same memory

    // Dictionary<int, List<int>> feeds;
    
    //1. *Important* It would be faster in performance to have List<int> BUT linked list makes it convenient (otherwise you store the index of last processed too)
    //2(new). Nvm, saw AI do it (was my idea, only implementation from AI) and it's very easy: //2. though LLs would also allow culling of data to only store upto last 10 (Cap) recent posts from any user, but that's too complex. I ain't doing all that for an interview. Maybe mention the tradeoffs?
    Dictionary<int, LinkedList<int>> feeds; //only got why LL when I got to the `PriorityQueue<int, LinkedList<int>> sources = new();` part! (otherwise we'd have to store last accessed index!)
    Dictionary<int, HashSet<int>> follows;


    int Cap = 10;

    public NuAttempt1() {
        feeds = new();
        follows = new();
    }
    
    public void PostTweet(int userId, int tweetId) { //solution assumes ids grow chronologically ordered
        feeds.TryAdd(userId, new());
        follows.TryAdd(userId, new());
        follows[userId].Add(userId); //I meant to do this but forgot somewhere //add is tryadd for hashset anyway!
        // feeds[userId].Add(tweetId);
        if(feeds[userId].Count == Cap)
        {
            feeds[userId].RemoveLast();
        }
        feeds[userId].AddFirst(tweetId);
    }
    
    public List<int> GetNewsFeed(int userId) { //did this method last!
        if(!follows.ContainsKey(userId))
            return [];
        // == O(N), why? Because at any moment only 10 (Cap) feeds in PQ! O(N*log(Cap(10))) == O(N*1) == O(1) 
        PriorityQueue<LinkedListNode<int>, int> sources = new(); //switched to LL because otherwise we'd have to store last accessed index too!
        foreach(var followed in follows[userId])
        {
             //Missed feeds.First null check, had to have AI point it out!
            if(!feeds.TryGetValue(followed, out var feed) || feed.First == null) //Had to have AI point to me I was getting for userId feed everytime here!
                continue;
            if(sources.Count == Cap)
            {
                if(sources.Peek().Value>=feed.First.Value)
                    continue;
                //else
                sources.Dequeue(); //so we need a MIN HEAP! (so we can filter out smallest tweet, assumed to be earliest!)
            }

            // if(feed.First!=null) //Missed this, had to have AI point it out!
            // {
                sources.Enqueue(feed.First, feed.First.Value);
            // }
        }  
        //Turn it into a maxHeap
        PriorityQueue<LinkedListNode<int>, int> maxHeap = new();
        while(sources.Count > 0 && sources.Peek().Value >= 0) //assuming no 0 as tweetId
        {
            var curSrc = sources.Dequeue();
            maxHeap.Enqueue(curSrc, -curSrc.Value);
        }
        sources = maxHeap;

        //Continue
        List<int> result = new(); //NVM could do similar with Lists, but again, complicated //NEVERMIND, this was probably where the linked list comes in,

        while(sources.Count > 0 && result.Count < Cap)
        {
            var curSrc = sources.Dequeue();
            result.Add(curSrc.Value);
            if(curSrc.Next != null)
            {
                sources.Enqueue(curSrc.Next, -curSrc.Next.Value);
            }
        }

        // return result.Reverse(); 
        return result;
    }
    
    public void Follow(int followerId, int followeeId) {
        follows.TryAdd(followerId, new());
        follows[followerId].Add(followeeId);
    }
    
    public void Unfollow(int followerId, int followeeId) {
        // Forgot about followerId == followeeId and broke it :'(
        if(followerId == followeeId || !follows.ContainsKey(followerId) || !follows[followerId].Contains(followeeId))//switched follows values from List<int> to HashSet<int> after getting here!
            return;
        follows[followerId].Remove(followeeId); //hoping this is the correct method name :O
    }
}
