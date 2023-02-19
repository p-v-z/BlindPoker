namespace BlindPoker;

/// <summary>
/// Contains all the details of a player's current hand
/// </summary>
public class PlayerHand
{
	public List<Card> Cards = new();
	public List<MatchingCollection> Collections = new();
}
