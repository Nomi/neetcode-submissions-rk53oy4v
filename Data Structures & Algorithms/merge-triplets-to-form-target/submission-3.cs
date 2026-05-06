public class Solution {
    public bool MergeTriplets(int[][] triplets, int[] target) {
        bool[] found = new bool[target.Length];
        foreach(var t in triplets) {
            if(t[0] > target[0] || t[1] > target[1] || t[2] > target[2])
                continue;
            
            bool? isFound = null;
            for(int i = 0; i < target.Length; i++) {
                if(t[i] == target[i])
                    found[i] = true;
                isFound = (isFound ?? true) && found[i]; // IMPORTANT: Missing brackets earlier caused it to crash on a test case! (order precedence is a pain!) [without brackets, compiler was reading it as: `isFound = isFound ?? (true && found[i]);`]
            }
            if(isFound == true)
                return true;
        }

        return false;
    }
}
