namespace BlindPoker;

public class HandSolver : IPokerSolver 
{
	/// <summary>
	///
	/// The function should output 0 in the case of a tie, 1 if the first player is the winner and 2 if the second player is the winner.
	/// </summary>
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand)
	{
		// Make sure that the hands are valid
		var handsValid = ValidateHand(player1Hand) && ValidateHand(player2Hand);
		if (!handsValid)
		{
			return -1;
		}
		
		// Get the ranking of the hands
		var player1Ranking = GetHandRanking(player1Hand.ToList());
		var player2Ranking = GetHandRanking(player2Hand.ToList());

		// If the rankings are the same, check the highest card
		if (player1Ranking == player2Ranking)
		{
			// TODO: Edge case - if the highest card is the same, check the second highest card, etc.
			// TODO: Ace (1) is the highest card
			return 1;
		}
		
		// Return the winner
		return player1Ranking > player2Ranking ? 1 : 2;
	}

	private static bool ValidateHand(int[] hand)
	{
		// Make sure that the hand contains 5 cards
		
		// Make sure that all cards are between 1 and 13
		
		// Make sure that there are no more than 4 cards of the same value

		return true;
	}

	private static int GetHandRanking(List<int> hand)
	{
		// Arrange
		hand.Sort();
		
		// Iterate over the cards
		var highestCard = 0;
		var longestSequence = 0;
		var collections = new List<MatchingCollection>();
		IterateCards(hand, collections, ref longestSequence, ref highestCard);
		
		return (int) GetHandType(collections, longestSequence);
	}
	
	/// <summary>
	/// Main function that iterates over the cards and checks for matches and sequences
	/// This function assumes that the cards are sorted, starting with the lowest card
	/// </summary>
	private static void IterateCards(List<int> hand, List<MatchingCollection> collections, ref int longestSequence, ref int highestCard)
	{
		var idx = 0;
		var sequenceLength = 0;
		var lastCard = -1;
		foreach (var card in hand)
		{
			// Only check for matches if the card is not the first card
			if (lastCard != -1)
			{
				// Check if the card matches the last card (belongs to collection)
				if (card == lastCard)
				{
					AddCardToCollection(card, collections);
				}
				// Check if the card is the next in a sequence
				else if (card == lastCard + 1)
				{
					IncreaseSequenceLength(ref sequenceLength, ref longestSequence);
				}
				// Reset the sequence length
				else
				{
					sequenceLength = 0;
				}
			}
			
			// Check if this is the highest card
			if (idx == 4)
			{
				highestCard = card;
			}
			
			lastCard = card;
			idx++;
		}
	}
	
	/// <summary>
	/// Increase the sequence length and update the longest sequence if needed
	/// </summary>
	private static void IncreaseSequenceLength(ref int sequenceLength, ref int longestSequence)
	{
		sequenceLength++;
		if (sequenceLength > longestSequence)
		{
			longestSequence = sequenceLength;
		}
	}
	
	/// <summary>
	/// Add a card to a collection or create a new collection if it doesn't exist yet
	/// </summary>
	private static void AddCardToCollection(int card, ICollection<MatchingCollection> collections)
	{
		// Check if the card is already in a collection
		var target = collections.FirstOrDefault(col => col.CardNumber == card);
		if (target != null)
		{
			target.Amount++;
		}
		else
		{
			collections.Add(new MatchingCollection(card, 2));
		}
	}

	/// <summary>
	/// This is the main function that determines the hand type based on the collections and the longest sequence
	/// </summary>
	private static PokerHand GetHandType(IReadOnlyList<MatchingCollection> collections, int longestSequence)
	{
		// Check for straight
		if (longestSequence == 5)
		{
			return PokerHand.Straight;
		}

		// Handle based on how many collections there are
		switch (collections.Count)
		{
			case 2:
				var total = collections[0].Amount + collections[1].Amount;
				return (total) switch
				{
					4 => PokerHand.TwoPair,
					5 => PokerHand.FullHouse,
					_ => 0
				};
			case 1:
				return (collections[0].Amount) switch
				{
					2 => PokerHand.Pair,
					3 => PokerHand.ThreeOfAKind,
					4 => PokerHand.FourOfAKind,
					_ => 0
				};
			default:
				return PokerHand.HighCard;
		}
	}
}
