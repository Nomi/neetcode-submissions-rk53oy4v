public class Solution {
    IEncodeAndDecodeStrings _sp = new Review_StringProcessor(); //StringProcessor();

    public string Encode(IList<string> strs) {
        return _sp.Encode(strs);
    }

    public List<string> Decode(string s) {
        return _sp.Decode(s);
   }
}

public interface IEncodeAndDecodeStrings {
    public string Encode(IList<string> strs);
    public List<string> Decode(string s);
}

// 2026.2 - Solution (Review):
public class Review_StringProcessor : IEncodeAndDecodeStrings{
    const char Separator = '#';

    public string Encode(IList<string> strs) {
        var sb = new StringBuilder();
        foreach(var str in strs) {
            sb.Append($"{str.Length}{Separator}{str}");
        }
        return sb.ToString();
    }

    public List<string> Decode(string s) {
        var res = new List<string>();
        for(int i = 0; i<s.Length; i++) {
            int j = i+1;
            for(; j < s.Length && s[j] != Separator; j++) {} //j is current separator or out of bounds!
            int curLen = int.Parse(s[i..j]);
            res.Add(s[(j+1)..(j+1+curLen)]);

            i = j+curLen;
        }
        return res;
   }
}



// 2026.1 - Solution:


internal class StringProcessor() : IEncodeAndDecodeStrings
{
    char seperator='#';

    //M = total number of characters (sum of lengths of all strings)
    //TC: O(M) [ignoring concatenation] because we process each character exactly once.
    //SC: O(N+M)  [obvious; n for ptrs to list and m for total number of characters!]
    public string Encode(IList<string> strs) 
    {
        //Should use string builder for this, but for time's sake, will go with simple string:
        var result = "";
        foreach(string str in strs) //TC: O(M) + O(M^2) without StringBuilder (using Concat), O(M) with it! For simplicity's sake, I am removing this overhead! 
                                    //SC: O(N+M)
        {
            result += $"{str.Length}{seperator}{str}"; //Concat itself is O(str1.Len+str2.Len), so at every step we reprocess str1 (which is result for us), creating O(M^2) total complexity by itself
        }
        return result; 
    }

    //M = total number of characters (sum of lengths of all strings)
    //TC: O(M) because we process each character exactly once. 
    //SC: O(N+M) [obvious; n for ptrs to list and m for total number of characters!]
    public List<string> Decode(string s) //Assuming always passed a valid string!
    {
        
        var result = new List<string>(); //SC:O(N+M) 
        for(int i=0;i<s.Length;) //TC: O(M) because process each character only once.
        {
            var j=i+1;
            while(s[j]!=seperator)
            {
                j++; //j is at separator by the end
            }

            var curLen = int.Parse(s[i..j]); //range is end exclusive in C#
            j = j+1; //j is at first character of the to-be curStr
            i = j+curLen; //get i to beginning of the next length number after this string;
            var curStr = s[j..i];  // i+curLen is at the start of the next string's number, but that's okay because range is end exclusive. 
            result.Add(curStr);
        }
        return result;
    }
}

// LAST ACTUAL SOLUTION:
// public class Solution {
//     //This encoding is interesting! (also, use while loops like they did?)

//     public string Encode(IList<string> strs) {
//         // return enc1(strs);
//         return enc2(strs);
//     }

//     public List<string> Decode(string s) {
//         // return dec1(s);
//         return dec2(s);
//     }

// /////////////////////////--ATTEMPT_1--//////////////////////////////////
//     // public string enc1(IList<string> strs)
//     // {
//     //     StringBuilder res = new();

//     //     foreach(var s in strs)
//     //     {
//     //         res.Append($"{s.Length}#{s}");
//     //     }
//     //     return res.ToString();
//     // }

//     // public string dec1(IList<string> strs)
//     // {
//     //     if(s.Length<2)
//     //         return new();
//     //     List<string> res = new();
//     //     int numStart = -1;
//     //     int shrp = -1;
//     //     for(int i=0;i< s.Length;i++) //s.Substring has a complexity of O(N) WHERE N is length of the substr???
//     //     {
//     //         if(numStart==-1&&char.IsNumber(s[i]))
//     //             numStart = i;
//     //         else if(s[i]=='#'&&numStart!=-1)
//     //             shrp = i;
//     //         if(shrp==-1)
//     //             continue;
//     //         // int wLen;
//     //         int.TryParse(s.Substring(numStart, shrp-numStart),out var wLen); //shrp-numStart gives the length from first digit of num to the last digit 
//     //         i=shrp+wLen;
//     //         res.Add(s.Substring(shrp+1,wLen));
//     //         numStart=-1;
//     //         shrp=-1;
//     //     }
//     //     return res;
//     // }

// /////////////////////////--ATTEMPT_2--//////////////////////////////////
//     public string enc2(IList<string> strs) {
//         StringBuilder res = new();
//         foreach(var str in strs)
//         {
//             res.Append($"{str.Length}#{str}");
//         }
//         return res.ToString();
//     }

//     public List<string> dec2(string s) {
//         List<string> res = new();
//         for(int i=0; i<s.Length;)
//         {
//             // if(!s[i].IsDigit)
//             //     throw new Exception("Encoded string is invalid.");
//             int numLen=1;
//             while(s[i+numLen]!='#')
//                 numLen++;
//             int wordLen = int.Parse(s.Substring(i,numLen));
//             i+=1+numLen; //we add 1 to skip the # right after the number.
//             res.Add(s.Substring(i,wordLen));
//             i+=wordLen;
//         }
//         return res;
//     }

// /////////////////////////--ATTEMPT_--//////////////////////////////////
// }