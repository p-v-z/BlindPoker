namespace BlindPoker;

/// <summary>
/// Static class to separate the logic a bit
/// </summary>
public static class Solver
{
	/// <summary>
	/// Main function that iterates over the cards and checks for matches and sequences
	/// This function assumes that the cards are sorted, starting with the lowest card
	/// </summary>
	public static PlayerHand IterateCards(List<int> hand, ICollection<MatchingCollection> collections, ref int longestSequence, ref int highestCard)
	{
		var idx = 0;
		var lastCard = -1;
		var sequenceLength = 1;
		var allCards = new List<Card>();
		var playerHand = new PlayerHand();

		foreach (var cardValue in hand)
		{
			// Create a new card object
			var currentCard = new Card(cardValue);
			allCards.Add(currentCard);
			
			// Only check for matches if the card is not the first card
			if (lastCard != -1)
			{
				// Check if the card matches the last card (belongs to collection)
				if (cardValue == lastCard)
				{
					AddCardToCollection(cardValue, collections);
				}
				// Check if the card is the next in a sequence
				else if (cardValue == lastCard + 1)
				{
					IncreaseSequenceLength(ref sequenceLength, ref longestSequence);
				}
				// Reset the sequence length
				else
				{
					sequenceLength = 1;
				}
			}
			
			// Check if this is the highest card
			if (idx == 4)
			{
				highestCard = cardValue;
			}
			
			lastCard = cardValue;
			idx++;
		}
		
		playerHand.Cards = allCards;
		playerHand.Collections = (List<MatchingCollection>) collections;
		return playerHand;
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
	/// Add a card to a collection or create a new collection if one doesn't exist yet
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
	/// This is the main function that determines the hand type based on the gathered collections and the longest sequence
	/// </summary>
	public static PokerHand GetHandType(IReadOnlyList<MatchingCollection> collections, int longestSequence)
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
	
	/// <summary>
	/// This function takes a sorted hand of cards, and moves the aces to the end to give them a higher value for high card comparison.
	/// </summary>
	public static void PutAcesOnTop(IList<Card> hand)
	{
		var cardsToMove = 5;
		while (hand[0].Value == 1 && cardsToMove > 0)
		{
			var ace = hand[0];
			hand.Remove(ace);
			ace.Value = 14;
			hand.Add(ace);
			cardsToMove--;
		}
	}
}

