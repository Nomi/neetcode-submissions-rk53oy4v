// 2026.2 - Solution (review): 
public class Solution {
    const int NUM_POSSIBLE_CHARS = 26; 

    public bool IsAnagram(string s, string t) {
        // IMPORTANT NOTE: This is an OVER-ENGINEERED solution.
        // The .All() method from my prior attempts was better, and since number of possible chars is 26, it'd be O(26) == O(1) regardless.

        if(s.Length != t.Length)
            return false;

        Span<int> diff = stackalloc int[NUM_POSSIBLE_CHARS];
        int zeros = NUM_POSSIBLE_CHARS;

        for(int i = 0; i < s.Length; i++) {
            if(diff[s[i]-'a'] == 0)
                zeros--;
            if(s[i] != t[i] && diff[t[i]-'a'] == 0)
                zeros--;
                
            diff[s[i]-'a']++;
            diff[t[i]-'a']--;

            if(diff[s[i]-'a'] == 0)
                zeros++;
            if(s[i] != t[i] && diff[t[i]-'a'] == 0)
                zeros++;
        }

        return zeros == NUM_POSSIBLE_CHARS;
    }
}


// 2026.1 - Solution: 

// public class Solution {
//     private uint CHARS_IN_ALPHABET = 26;
//     public bool IsAnagram(string s, string t) {
//         if(s.Length != t.Length)
//             return false;
//         int[] balanceArr = new int[CHARS_IN_ALPHABET];
//         for(int i=0; i<s.Length; i++)//O(n)
//         {
//             balanceArr[s[i]-'a']++;
//             balanceArr[t[i]-'a']--;
//         }
//         return balanceArr.All(x => x == 0); //O(26) == O(1) ==constant time
//     }
// }


// Last Actual Solution:

// public class Solution {
//     public bool IsAnagram(string s, string t) {
//         if(s.Length!=t.Length)
//             return false;
//         Dictionary<char, int> sMap = new();
//         Dictionary<char, int> tMap = new();
//         foreach(char c in s)
//         {
//             if(!sMap.TryAdd(c,1))
//                 sMap[c]++;
//         }
//         foreach(char c in t)
//         {
//             if(!tMap.TryAdd(c,1))
//                 tMap[c]++;
//         }
//         if(tMap.Count!=sMap.Count)
//             return false;
//         foreach(char key in tMap.Keys)
//         {
//             if(!sMap.ContainsKey(key)||sMap[key]!=tMap[key])
//                 return false;
//         }
//         return true;
//     }
// }
