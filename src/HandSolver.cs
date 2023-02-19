namespace BlindPoker;

public interface IPokerSolver {
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand);
}

/// <summary>
/// This class is responsible for comparing two poker hands and determining the winner.
/// </summary>
public class HandSolver : IPokerSolver 
{
	private List<Card> _p1Hand = new();
	private List<Card> _p2Hand = new();
	
	/// <summary>
	/// Main function to compare two poker hands and determine the winner.
	/// The function should output 0 in the case of a tie, 1 if the first player is the winner and 2 if the second player is the winner.
	/// </summary>
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand)
	{
		// Get the ranking of the hands
		var player1Ranking = GetHandRanking(true, player1Hand.ToList());
		var player2Ranking = GetHandRanking(false, player2Hand.ToList());
		
		// If the ranking is invalid, return -1
		if (player1Ranking == -1 || player2Ranking == -1) return -1;
		
		// If the ranking is the same, check the highest card or tie
		if (player1Ranking == player2Ranking)
		{
			// Players do not have winning hands, check the highest card
			if (player1Ranking == 0 && player2Ranking == 0)
			{
				// If the first card is a 1 (Ace), move it to the end of the list as it has the highest value
				PutAcesOnTop(_p1Hand);
				PutAcesOnTop(_p2Hand);

				// Compare the last cards in the list until they are not equal
				var maxRemove = 4;
				while (_p1Hand.Last().Value == _p2Hand.Last().Value && maxRemove > 0)
				{
					_p1Hand.Remove(_p1Hand.Last());
					_p2Hand.Remove(_p2Hand.Last());
					maxRemove--;
				}

				var p1Last = _p1Hand.Last().Value;
				var p2Last = _p2Hand.Last().Value;
				return p1Last == p2Last ? 0 : p1Last > p2Last ? 1 : 2;
			}
			
			// Both players have the same winning hand (e.g. both have a pair of twos), it is a tie.
			return 0;
		}
		
		// // Return the winner
		return player1Ranking > player2Ranking ? 1 : 2;
	}
	
	/// <summary>
	/// This function takes a single hand of cards, and returns the ranking of the hand.
	/// </summary>
	/// <param name="handA"></param>
	/// <param name="hand"></param>
	/// <returns></returns>
	private int GetHandRanking(bool handA, List<int> hand)
	{
		// Make sure that the hands are valid
		var hasFullHand = hand.Count == 5;
		var hasRightValues = hand.All(card => card is >= 1 and <= 13);
		if (!hasFullHand || !hasRightValues)
		{
			return -1;
		}
		
		// Arrange
		hand.Sort();
		
		// Iterate over the cards
		var highestCard = 0;
		var longestSequence = 0;
		var collections = new List<MatchingCollection>();
		if (handA)
		{
			_p1Hand = (List<Card>)IterateCards(hand, collections, ref longestSequence, ref highestCard);
		}
		else
		{
			_p2Hand = (List<Card>)IterateCards(hand, collections, ref longestSequence, ref highestCard);
		}

		return (int) GetHandType(collections, longestSequence);
	}
	
	/// <summary>
	/// Main function that iterates over the cards and checks for matches and sequences
	/// This function assumes that the cards are sorted, starting with the lowest card
	/// </summary>
	private static IEnumerable<Card> IterateCards(List<int> hand, ICollection<MatchingCollection> collections, ref int longestSequence, ref int highestCard)
	{
		var idx = 0;
		var sequenceLength = 1;
		var lastCard = -1;
		var allCards = new List<Card>();

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
		return allCards;
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
	
	/// <summary>
	/// This function takes a sorted hand of cards, and moves the aces to the end to give them a higher value for high card comparison.
	/// </summary>
	private static void PutAcesOnTop(IList<Card> hand)
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
