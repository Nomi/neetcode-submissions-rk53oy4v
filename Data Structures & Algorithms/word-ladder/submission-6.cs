// For interview: stick to single BFS from start.  (like my last actual solution? Watch NeetCode video?)
// At most will need to explain in follow-up this bi-directional BFS, but not write the code, most probably (unless you're a senior/L5+).
public class Solution {

    public const char Wildcard = '*';
    
    private const int NotFound = 0;

    public int LadderLength(string beginWord, string endWord, IList<string> wordList) {
        //of same length, lowercase, distinct

        // Create map of wildcards to their matching strings:
        Dictionary<string, List<string>> wildcardMap = new();
        // Dictionary<string, List<string>> wordToWildcard = new();
        var foundEnd = false;
        foreach(var word in wordList) {
            if(word == endWord) foundEnd = true;
            AddAllWildcards(wildcardMap, word);
        }

        if(!foundEnd)
            return 0;
        // if(!wildcardMap.ContainsKey(beginWord) || !wildcardMap.ContainsKey)

        Queue<string> beginQ = new(), endQ = new();
        beginQ.Enqueue(beginWord);
        endQ.Enqueue(endWord);        

        // return FindMinDistance(beginQ, endQ, wildcardMap);
        return FindMinDistance(beginQ, endQ, wildcardMap);
    }

    private void AddAllWildcards(Dictionary<string, List<string>> wildcardMap, string word) {
        
        var wildcards = GetAllWildcards(word);
        foreach(var wild in wildcards) {
            wildcardMap.TryAdd(wild, new());
            wildcardMap[wild].Add(word);
        }
    }

    private List<string> GetAllWildcards(string word)
    {
        List<string> wildcardsForWord = new();
        char[] wordArr = word.ToCharArray();

        for(int i = 0; i < word.Length; i++)
        {
            wordArr[i] = Wildcard;
            string wild = new string(wordArr);
            wordArr[i] = word[i];

            wildcardsForWord.Add(wild);
        }

        return wildcardsForWord;
    }

    private int FindMinDistance(Queue<string> beginQ, Queue<string> endQ, Dictionary<string, List<string>> wildcardMap) {
        Dictionary<string, int> distBegin = new();
        Dictionary<string, int> distEnd = new();

        var level = 0; // because we need to include the first and last word!

        while(beginQ.Count > 0 && endQ.Count > 0) //if either one of them ends without meeting the other, there's no path between them (otherwise one of them would at least overlap OR reach the other)
        {
            //Snapshot current level counts
            
            //beginQ:
            var res = ProcessCurrentLevelOrderBfsLevel(level, beginQ, distBegin, distEnd, wildcardMap);
            if(res != NotFound)
                return res;
            //endQ:
            res = ProcessCurrentLevelOrderBfsLevel(level, endQ, distEnd, distBegin, wildcardMap);
            if(res != NotFound)
                return res;

            level++;
        }

        return NotFound;
    }

    private int ProcessCurrentLevelOrderBfsLevel(int level, Queue<string> curQ, Dictionary<string, int> distCur, Dictionary<string, int> distOther, Dictionary<string, List<string>> wildcardMap) {
        var levelCount = curQ.Count;
        for(int i=0; i < levelCount; i++)
        {
            var cur = curQ.Dequeue();

            if(distOther.ContainsKey(cur))
                return 1 + level + distOther[cur];

            if (!distCur.TryAdd(cur, level)) continue; //if it fails, we already went there before, from a shorter OR same distance! Why reprocess then :)?

            // distCur.TryAdd(cur, level); //if it fails, we already went there before, from a shorter distance!
            foreach(var wild in GetAllWildcards(cur)) {
                if(!wildcardMap.ContainsKey(wild))
                    continue; //THIS IS FOR START AND END WORDS BECAUSE THEY ARE NOT IN LIST!
                foreach(var word in wildcardMap[wild]) {
                    if(!distCur.ContainsKey(word))
                        curQ.Enqueue(word);
                }
            }
        }

        return NotFound; //at this level
    }


    private int FindMinDistance_Unmodular_DEPRECATED(Queue<string> beginQ, Queue<string> endQ, Dictionary<string, List<string>> wildcardMap) {
        Dictionary<string, int> distBegin = new();
        Dictionary<string, int> distEnd = new();

        var level = 0; // because we need to include the first and last word!

        while(beginQ.Count > 0 && endQ.Count > 0) //if either one of them ends without meeting the other, there's no path between them (otherwise one of them would at least overlap OR reach the other)
        {
            //Snapshot current level counts
            var beginCount = beginQ.Count;
            var endCount = endQ.Count;
            for(int i=0; i < beginCount; i++)
            {
                var cur = beginQ.Dequeue();
                if(distEnd.ContainsKey(cur))
                    return 1 + level + distEnd[cur];
                distBegin.TryAdd(cur, level); //if it fails, we already went there before, from a shorter distance!
                foreach(var wild in GetAllWildcards(cur)) {
                    if(!wildcardMap.ContainsKey(wild))
                        continue; //THIS IS FOR START AND END WORDS BECAUSE THEY ARE NOT IN LIST!
                    foreach(var word in wildcardMap[wild]) {
                        if(!distBegin.ContainsKey(word))
                            beginQ.Enqueue(word);
                    }
                }
            }

            for(int i=0; i < endCount; i++)
            {
                var cur = endQ.Dequeue();
                if(distBegin.ContainsKey(cur))
                    return 1 + level + distBegin[cur];
                distEnd.TryAdd(cur, level); //if it fails, we already went there before, from a shorter distance!
                foreach(var wild in GetAllWildcards(cur)) {
                    if(!wildcardMap.ContainsKey(wild))
                        continue; //THIS IS FOR START AND END WORDS BECAUSE THEY ARE NOT IN LIST!
                    foreach(var word in wildcardMap[wild]) {
                        if(!distEnd.ContainsKey(word))
                            endQ.Enqueue(word);
                    }
                }
            }

            level++;
        }

        return NotFound;
    }
}



// # Last Actual Solution : (November 2024)

// public class Solution {
//     //ASYMPTOTICALLY BOUND MEANS WE TAKE THE FASTEST GROWING COMPLEXITY AS THE ACTUAL REPRESENTATIVE BECAUSE THAT'S WHAT MATTERS
//     public int LadderLength(string beginWord, string endWord, IList<string> wordList) {
//         // EASY BUT A VERY TRICKY (Complexity calculations confused me for a while because I was taking m as constant (deservedly, given it is <=10) but others weren't)

//         // I just noticed: 1<= word length <= 10 means that loops over words and methods like string.Substring() method can be treated as a constant time as it is at most O(10)
//         //NOT hard, just a little tricky to think how to do it (e.g. using the hashset, how to treat/traverse it as a graph, etc.) In fact, it MIGHT be hard to notice that this is a graph problem, here it was just given due to it being in the Graphs section.
        
//         //Since this problem boils down to shortest path in a graph, we should use BFS to be able to finish right at the level where we encounter the shortest rather than going through all branches with dfs and then checking the minimum.

//         //Had to watch neetcode video before doing it (and checked ChatGPT too). NeetCode's solution was a little better than ChatGPT's first solution (without pre-processing adjList) (using HashSet and looping to check possible neighors during bfs) unlike neetcode (using all places a wildcard character can be for any word as a key and all possible words for that key as a value (e.g. *ot would have dot, bot, etc.) and this would then serve as the adjacency list, where we would start from the beginWord(all of its possible wildcards)), which is what I will implement now (to be clear: which -> NeetCode soln)

//         return bfs1(beginWord, endWord, wordList); //SHIT!! IT TOOK SO LONG TO GET RIGHT!! (Well a lot of time was spent figuring out complexities as well!)
//     }

//     const char wildcard = '*';

//     public int bfs1(string beginWord, string endWord, IList<string> wordList) //Used NeetCode video
//     {
//         //For complexities, let n== wordList.Length and m== wordLength 
//         //NOTICE: here m<=10, so basically O(m) == O(10) === O(1) and we can actually treat it as constant time, but as a theoretical exercise, I'm calculating the complexity.
//         //As such, you can practically remove the m from all of the complexities here.

//         //SC of adjList: O(((n*m)*m)*26) == O(n*m^2) 
//         //because n*m is the number m is the number of wilcards per word and n is number 
//         //of words. Therefore, the total number of strings with 1 wildcard we can make
//         //(the ones we use as keys for the dict) is n*m. Now for the values, we store
//         // 26 strings (because any wild card can be replaced by any of 26 letters) 
//         // of length m. => SC = O(((n*m)*26)*m). (we multiply by 26 instead of adding it like we do for graphs usually because unlike there, a word can be present in the list for more than 1 key, unlike the graph problems where you have V+E)
//         Dictionary<string, List<string>> adjList = new();
        
//         int wordLen = beginWord.Length; //Assuming all words are of same length, this helps us avoid cache misses. //also assuming wordLen>1
        
//         bool foundEndWord = false; 
//         //I SAW FOR THE MILLIONTH TIME, READ THE QUESTION CAREFULLY!!! WAS WRONG ABOUT THIS ASSUNPTION AND IT WAS OBVIOUS FROM THE SAMPLE INPUTS/EXAMPLES: assuming begin word will be in the list
//         PopulatePossibleWildcardPlacements1(beginWord, wordLen, adjList); //O(m^2)
//         foreach(var word in wordList) //O(n*m^2) //Populate adjList (from all possible strings made from wordList with all possible placements of a single wildcard)
//         {
//             PopulatePossibleWildcardPlacements1(word, wordLen, adjList); //O(m^2)
//             if(word == endWord) //O(m)
//                 foundEndWord = true;
//         }

//         if(!foundEndWord)
//             return 0;

//         HashSet<string> visited = new(); //to avoid cycles. //SC: O(n*m) because n words of at most m length and each word will be here only once because cycle detection prevents otherwise

//         Queue<(string word, int step)> q = new(); //SC: O(n*m) because n words of at most m length and each word will be here only once because cycle detection prevents otherwise
//         q.Enqueue((beginWord, 1));
//         while(q.Count>0) //O(n*m^2)
//         {
//             var res = Bfs1Helper(endWord, q, visited, adjList); //O(m*26*m) == O(m^2)
//             if(res!=-1)
//                 return res;
//         }


//         return 0;
//     }

//     public int Bfs1Helper(string endWord, Queue<(string word, int step)> q, HashSet<string> visited, Dictionary<string, List<string>> adjList)
//     {
//         var cur = q.Dequeue();
//         if(visited.Contains(cur.word)) //this will NOT break the solution: because of how BFS works, the first time a node is added to visited is the shortest path we can take to get there from the BFS starting point.
//             return -1;
//         visited.Add(cur.word);
//         //We can treat these as constant time because it is O(m) where m is word length but word length is AT MOST 10 == O(10) <=> O(1)
//         var curWordArr = cur.word.ToCharArray();
//         for(int i=0; i < curWordArr.Length; i++) //O(m) where m is wordLength
//         {
//             char prevAtI = curWordArr[i];
//             curWordArr[i] = wildcard;
//             string wildcardAtI = new(curWordArr); //O(m)
//             Console.WriteLine(cur);
//             Console.WriteLine(wildcardAtI);
//             curWordArr[i] = prevAtI;
//             foreach(var word in adjList[wildcardAtI]) //O(26) because a single wildcard can only be replaced by at most 26 letters since there are only 26 letters
//             {
//                 if(visited.Contains(word))
//                     continue;
//                 if(word==endWord)//O(m)
//                     return cur.step+1;
//                 q.Enqueue((word, cur.step+1));
//             }
//         }
//        return -1;
//     }

//     public void PopulatePossibleWildcardPlacements1(string word, int wordLen, Dictionary<string, List<string>> adjList)
//     {
//         //We can treat these as constant time because it is O(m) where m is word length but word length is AT MOST 10 == O(10) <=> O(1)
//         var wordArr = word.ToCharArray(); //O(m)
//         for(int i=0; i < wordLen; i++) //O(m) where m is wordLength
//         {
//             char prevAtI = wordArr[i];
//             wordArr[i] = wildcard;
//             string wildcardAtI = new(wordArr); //O(m)
//             Console.WriteLine($"<<<{wildcardAtI}");
//             wordArr[i] = prevAtI;
//             adjList.TryAdd(wildcardAtI, new());
//             adjList[wildcardAtI].Add(word);
//         }
//     }
// }
