namespace BlindPoker;

public interface IPokerSolver {
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand);
}

/// <summary>
/// This class is responsible for comparing two poker hands and determining the winner.
/// </summary>
public class HandSolver : IPokerSolver 
{
	private PlayerHand _p1Hand = new();
	private PlayerHand _p2Hand = new();
	
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
			return (player1Ranking & player2Ranking) switch
			{
				// Check the highest card for (high card / straight)
				0 or 4 => DetermineHighestCard(),
				// Check the highest collection (pair / three of a kind / four of a kind)
				1 or 2 or 3 or 6 => DetermineHighestCollection(),
				// Full house
				5 => DetermineHighestFullHouse(),
				// Handle invalid selection
				_ => throw new ArgumentOutOfRangeException($"{nameof(player1Hand)}")
			};
		}
		
		// Return the winner
		return player1Ranking > player2Ranking ? 1 : 2;
	}

	/// <summary>
	/// Determine the highest card in the hand that is not the same as the opponent
	/// </summary>
	private int DetermineHighestCard()
	{
		// If the first card is a 1 (Ace), move it to the end of the list as it has the highest value
		Solver.PutAcesOnTop(_p1Hand.Cards);
		Solver.PutAcesOnTop(_p2Hand.Cards);

		// Compare the last cards in the list until they are not equal
		var maxRemove = 4;
		while (_p1Hand.Cards.Last().Value == _p2Hand.Cards.Last().Value && maxRemove > 0)
		{
			_p1Hand.Cards.Remove(_p1Hand.Cards.Last());
			_p2Hand.Cards.Remove(_p2Hand.Cards.Last());
			maxRemove--;
		}

		var p1Last = _p1Hand.Cards.Last().Value;
		var p2Last = _p2Hand.Cards.Last().Value;
		return p1Last == p2Last ? 0 : p1Last > p2Last ? 1 : 2;
	}

	/// <summary>
	/// Determine the highest collection
	/// </summary>
	private int DetermineHighestCollection()
	{
		var p1Ordered = _p1Hand.Collections.OrderByDescending(c => c.CardNumber).ToList();
		var p2Ordered = _p2Hand.Collections.OrderByDescending(c => c.CardNumber).ToList();
		var p1Highest = p1Ordered.First().CardNumber;
		var p2Highest = p2Ordered.First().CardNumber;
		if (p1Highest != p2Highest)
		{
			return p1Highest > p2Highest ? 1 : 2;
		}
		
		if (p1Ordered.Count > 1)
		{
			var p1Second = p1Ordered.Last().CardNumber;
			var p2Second = p2Ordered.Last().CardNumber;
			return p1Second == p2Second ? 0 : p1Second > p2Second ? 1 : 2;
		}

		return 0;
	}
	
	/// <summary>
	/// Determine the highest full house
	/// </summary>
	/// <returns></returns>
	private int DetermineHighestFullHouse()
	{
		var p1Ordered = _p1Hand.Collections.OrderByDescending(c => c.Amount).ToList();
		var p2Ordered = _p2Hand.Collections.OrderByDescending(c => c.Amount).ToList();
		var p1Highest = p1Ordered.First().CardNumber;
		var p2Highest = p2Ordered.First().CardNumber;
		return p1Highest == p2Highest ? 0 : p1Highest > p2Highest ? 1 : 2;
	}

	/// <summary>
	/// This function takes a single hand of cards, and returns the ranking of the hand.
	/// </summary>
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
		ref var targetHand = ref handA ? ref _p1Hand : ref _p2Hand;
		targetHand = Solver.IterateCards(hand, collections, ref longestSequence, ref highestCard);

		return (int) Solver.GetHandType(collections, longestSequence);
	}
}
