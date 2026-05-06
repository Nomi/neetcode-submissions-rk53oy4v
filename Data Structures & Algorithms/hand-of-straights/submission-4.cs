public class Solution {
    public bool IsNStraightHand(int[] hand, int groupSize) {
        if(hand.Length % groupSize != 0) //not divisible, meaning some cards leftover and not in group
            return false;

        Dictionary<int, int> cardCount = new();
        foreach(var card in hand) {
            cardCount.TryAdd(card, 0);
            cardCount[card]++;
        }

        foreach(var card in hand) {
            if(cardCount[card] == 0) //amortized O(N) because of this!      [ 3 Reasons Why: 1. Each unique card is used as a startOfGroup candidate only once. 2. Each individual card in the input is decremented in cardCount exactly once. 3. Once a card's count hits 0, the if(cardCount[card] == 0) continue; line ensures we never do heavy lifting for that card again.]
                continue;

            int startOfGroup = card;
            while(cardCount.ContainsKey(startOfGroup-1) && cardCount[startOfGroup - 1] > 0) {
                startOfGroup--;
            }

            while(startOfGroup <= card) { //This handles duplicates. If you have two 1s, two 2s, and two 3s, you need to form the group (1, 2, 3) twice
                while(cardCount.ContainsKey(startOfGroup) && cardCount[startOfGroup] > 0) { // Once you've exhausted the 1s, you move to the 2s. If there are any 2s left over (because they were part of a different overlapping group), you start forming groups from there.
                    // ^ IMPORTANT! (I spotted the problem [e.g. there were two consecutive groups and the current `card` appeared only once] 
                    // but didn't think of the solution (2 surrounding while loop), so I had to check neetcode solution :'( )
                    for(int i = 0; i < groupSize; i++) {
                        if(!cardCount.ContainsKey(startOfGroup + i) || cardCount[startOfGroup + i] == 0)
                            return false;
                        
                        cardCount[startOfGroup + i]--;
                    }
                }
                startOfGroup++;
            }
        }

        return true;
    }
}
