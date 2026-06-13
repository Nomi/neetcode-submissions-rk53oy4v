
public class PrefixTree {
    IPrefixTree soln;
    public PrefixTree() {
        // In google interview: must clarify how to treat upper and lower case (and perhaps same letters different codes (normalizing))
        // Also, ask: "How should we treat an empty string?"


        // # 2024 Solution:
        // //WATCH THE HACKERRANK VIDEO ABOUT TRIE FROM THE LADY WHO WROTE CRACKING THE CODING INTERVIEW.
        
        // //Dictionary/HashMap would've been less verbose, thus easier to implement, while Array is faster in performance.

        // soln = new Attempt1();


        // # 2026 (March) Solutions:

        // ## NuAttempt 1:
        // This time I basically did the basic design myself and mentioned to the AI, got only a few hints.  (And I remembered someone telling me a couple weeks back that Tries use dictionaries!)
        // Most of it was mine, but the few hints I ended up needing were not too complex but stii shouldn't have missed it. 
        // This session probably lasted like 4-5 minutes,
        // Well, a massive improvement from 
        // (also in 2024 I said arrays are faster in performance true, but they can allocate unneccessary space too IF number of unique chars is big or even if the words are extremely long.)

        // soln = new NuAttempt1();

        // # NuAttempt 2: Best attempt *AND* WRITTEN 1 HOUR AFTER NuAttempt1!
        // Dictionary is actually better as it helps with more realistic input (not limited to ASCII lowercase letters e.g. unicode, etc.)!
        soln = new NuAttempt2();
    }
    
    public void Insert(string word) {
        soln.Insert(word);
    }
    
    public bool Search(string word) {
        return soln.Search(word);
    }
    
    public bool StartsWith(string prefix) {
        return soln.StartsWith(prefix);
    }
}

// From 2024, but NOT SOLUTION (or a part of it):
public interface IPrefixTree
{
    public void Insert(string word);
    
    public bool Search(string word);
    
    public bool StartsWith(string prefix);
}





// # March 2026 Solutions:


// ## NuAttempt 2: Best attempt *AND* WRITTEN 1 HOUR AFTER NuAttempt1!          //DEPRECATED BECAUSE DICTIONARY IS BETTER FOR CASES WHERE THERE ARE OTHER CHARACTERS THAN JUST ASCII lowercase letters: Best attempt (except for not using char[])

// - Took 11 mins 17 seconds for CODE.
// - Took 2 mins 44 seconds to write down the complexities and their explanations.

// Complexities:
// - TC: O(N) [Insert, Search, and StartsWith function are O(N) where N is current input word length!]
// - SC: O(M*N) where N is number of words and W is length of LONGEST word. [worst case is when no prefixes are shared between any words, best is O(M) when all words share a prefix!]
// *IMPORTANT* note: (pointed out by AI) I HAD made mistake in calculating SC (had M^2) this time because of my slight headache and slight exhaustion, even though I calculated it correctly just ~2 hours ago!
public class NuAttempt2 : IPrefixTree {     // Written an hour or so after `NuAttempt1`
    internal class Node
    {
        public Node() {
            childs = new();
        }

        // POST-FINISH DISCOVERED BUG (via NC.io compiler): Had the following members private by mistake! (forgot to add the public modifier!)
        public bool isEnd; //false by defauly
        public Dictionary<char, Node> childs; //POST-FINISH DISCOVERED BUG (via NC.io compiler): found I was using `node` instead of `Node` for `TValue`! (on my first attempt at running)
    }

    Node trie;

    public NuAttempt2() {
        trie = new();
    }
    
    public void Insert(string word) {
        if(word.Length == 0)
            return;

        var prev = trie;
        foreach(char c in word)
        {
            if(!prev.childs.TryGetValue(c, out var node))
            {
                node = new();
                prev.childs.Add(c, node); //POST-FINISH DISCOVERED BUG (via NC.io compiler): In my rush was doing just `Add(node)`
            }
            prev = node;
        }
        prev.isEnd = true; //last element is end!
    }
    
    public bool Search(string word) {
        var (exists, isEnd) = SearchHelper(word.AsSpan());
        return exists && isEnd;
    }
    
    public bool StartsWith(string prefix) {
        var (exists, _) = SearchHelper(prefix.AsSpan());
        return exists;
    }

    // 13-06-26 Update: Might be cleaner and more standard to just return the TrieNode, as that can also serve the same purpose without the tuple!
    public (bool exists, bool isEnd) SearchHelper(ReadOnlySpan<char> str)
    {
        if(str.Length == 0) //POST-FINISH DISCOVERED BUG (via NC.io compiler): Was `str.Length = 0`
            return (true, false); //Well, didn't have to here since N>=1 is given!

        var prev = trie;
        foreach(char c in str)
        {
            if(!prev.childs.TryGetValue(c, out var node))
            {
                return (false, false); //if it doesn't exist, it can't be the end!
            }
            prev = node;
        }
        
        return (true, prev.isEnd);
    }
}




// # NuAttempt 1:
public class NuAttempt1 : IPrefixTree { //Total 43 mins. [including my earlier disaster DFS attempts (consecutively for 2 different functions!)]

    //  *IMPORTANT* at the end I realized that:
    // in google interview must clarify how to treat upper and lower case (and perhaps same letters different codes (normalizing))


    // TC: all function TC are O(W) where W = input word or prefix length
    // SC: O(M*N) where N is number of words and W is length of LONGEST word. [this is the worst case when nothing is common. Best case is O[M] if all are prefixes!]
    // Took 2 mins to get these complexities and write the notes about them^
    
    internal class TrieNode 
    {  
        public TrieNode(char val, bool isEnd)
        {
            this.val = val;
            this.isEnd = isEnd;
            childs = new(); //had forgotten to update this only found it at the end!
        }
        public char val;
        public bool isEnd; //false by default
        public Dictionary<char, TrieNode> childs;
    } //4 minutes to build this class first //updated from isComplete to isEnd later  and next to childs and cur to value to val

    TrieNode root;
    public NuAttempt1() {
        root = new(' ', false); //dummy! //*IMP* WAS '' before BUT APPARENTLY YOU CANT USE EMPTY CHAR IN C# (FOUND BY NEETCODE COMPILER WHEN I TRIED TO RUN FIRST TIME!)
    }
    
    public void Insert(string word) {
        // InsertRec(word.AsSpan(), root);

        if(word.Length == 0)//discarding empty "words"
            return;

        var prevNode = root;
        for(int i=0; i<word.Length; i++)
        {
            prevNode.childs.TryAdd(word[i], new(word[i], false));
            var curNode = prevNode.childs[word[i]]; //had words instead of word
            prevNode = curNode;
        }
        prevNode.isEnd = true;
    }
    
    // bool InsertRec(ReadOnlySpan<char> word, TreeNode parent) //14 minutes in I realized I was forgetting a lot of things!
    // {

    //     if(word.Length == 0) //base case, but also for empty strings inserts nothing!
    //         return true;
    //     var newNode = new(word[0], false);
    //     parent.next.Add(word[0], newNode);
    //     var isEnd = InsertRec(word[1..], cur);
    //     newNode.isEnd = isEnd;
    //     return false;
    // }
    public bool Search(string word) {
        // return SearchDfsPre(word.AsSpan(), root);
        var prevNode = root;
        for(int i=0; i< word.Length; i++)
        {
            var curChr = word[i];
            if(!prevNode.childs.TryGetValue(curChr, out var node)) //was doing TryGet instead of TryGetValue! Found on neetcode run (compiler message)
                return false;
            prevNode = node; //had == somehow
        }
        return prevNode.isEnd; //LAST CHAR HAS TO BE AN END IF THE WORD EXIST!
    } //finished at 34m:37s

    // void bool SearchDfsPre(ReadOnlySpan<char> expr, TrieNode cur) //At 29:40 I realized I was stupid and I only needed to do a walk (just like in my conversation with the AI earlier!)
    // {
        

    //     // // //expr == 0?
    //     // // // if(expr.Length == 0 && )
    //     // // if(expr.Length == 0) //asuming word length > 0 always!
    //     // // {
    //     // //     return false;
    //     // // }
        
    //     // if(expr.Length > 1 && cur.val == expr[0] && cur.isEnd) //asuming word length > 0 always!
    //     //     return true;
    //     // else
    //     //     return false;

    //     // foreach(var node in cur.childs.val)
    //     // {
    //     //     if(cur.)
    //     // }
    // }
    
    public bool StartsWith(string prefix) {
        var prev = root;
        for(int i=0; i<prefix.Length; i++) //assuming word is prefix of itself
        {
            var curChr = prefix[i]; //forgot to change it back ro prefix from word since i copied from above, found on NC run!
            if(!prev.childs.TryGetValue(curChr, out var node)) //was doing TryGet instead of TryGetValue! Found on neetcode run (compiler message)
                return false; //forgot to change the above usage to prevNode before first run
            //AT THE VERY END, SPENT 4 minutes TO REALIZE I HAD FORGOTTEN THIS: (even though I had it in my search function all time and even here right after copying. Think i removed it when changing prevNode to prev!)
            prev = node;
        }
        return true; //here, it doesn't have to be end
    } //finished this at 36:00m + at 40 mins recognized the bug here and fixed it (from failed NeetCode auto test cases!)
}





// # 2024 Solution:            (Last submission: 19-11(November)-2024, 12:01 AM, CET !)
public class Attempt1 : IPrefixTree
{
    private class TrieNode
    {
        // public string currChar;
        public TrieNode[] children = new TrieNode[26]; //could've used `Node[] children = new[26];` and it would have been faster and more time efficient. 
        public bool canBeEndOfWord = false;
    }
    
    TrieNode trieRoot;
    
    public Attempt1()
    {
        trieRoot = new();
    }
    
    public void Insert(string word) {

        var parent = trieRoot;

        for(int i=0;i<word.Length;i++)
        {
            // parent.children.TryAdd(curSubStr, new TrieNode());
            int targetChildIdx = word[i] - 'a';
            parent.children[targetChildIdx] ??= new TrieNode();
            parent = parent.children[targetChildIdx];
        }

        parent.canBeEndOfWord = true;
    }
    
    public bool Search(string word) {

        var parent = trieRoot;

        for(int i=0;i<word.Length;i++)
        {
            int targetChildIdx = word[i] - 'a';
            // if(parent.ContainsKey(word[i]) is false)
            //     return false;
            if(parent.children[targetChildIdx] is null)
                return false;
            parent = parent.children[targetChildIdx];
        }

        return parent.canBeEndOfWord;
    }
    
    public bool StartsWith(string prefix) {
        var parent = trieRoot;

        for(int i=0;i<prefix.Length;i++)
        {
            int targetChildIdx = prefix[i] - 'a';
            // if(parent.children.ContainsKey(word[i]) is false)
            //     return false;
            if(parent.children[targetChildIdx] is null)
                return false;
            parent = parent.children[targetChildIdx];
        }

        return true;
    }
}