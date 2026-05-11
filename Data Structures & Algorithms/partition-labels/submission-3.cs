public class Solution {
    const int LowerCaseEnglishLetterCount = 26;
    public List<int> PartitionLabels(string s) { // Kind of like `intervals` :)
        List<int> partLengths = [];

        Dictionary<char, int> lastIdx = new(); //Last Appearance Idx (value) of each char (key)

        // for(int i = 0; i < s.Length; i++) {            lastIdx[s[i]] = i;        }
        for(int i = s.Length-1; i >= 0; i--) {
            lastIdx.TryAdd(s[i], i);
            if(lastIdx.Count == LowerCaseEnglishLetterCount) break; //Only works if all chars in string!
        }

        for(int l = 0; l < s.Length; ) {

            var curPartLast = lastIdx[s[l]]; //idx

            for(int r = l; r <= curPartLast; r++) { 
                curPartLast = Math.Max(curPartLast, lastIdx[s[r]]);
            }

            partLengths.Add(curPartLast + 1 - l); //*IMPORTANT* Since this is length, we must calculate it BEFORE updating l!
            l = curPartLast + 1; //next part's first idx
        }

        return partLengths;
    }
}