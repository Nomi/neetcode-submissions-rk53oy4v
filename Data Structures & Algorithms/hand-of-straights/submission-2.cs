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
            if(cardCount[card] == 0) //O(N) because of this!
                continue;

            int startOfGroup = card;
            while(cardCount.ContainsKey(startOfGroup-1) && cardCount[startOfGroup - 1] > 0) {
                startOfGroup--;
            }

            while(startOfGroup <= card) { 
                while(cardCount.ContainsKey(startOfGroup) && cardCount[startOfGroup] > 0) {
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
