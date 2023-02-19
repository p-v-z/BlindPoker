namespace BlindPoker;

/// <summary>
/// Describes a collection of cards that match
/// </summary>
public class MatchingCollection
{
	public readonly int CardNumber;
	public int Amount;
	
	public MatchingCollection(int cardNumber, int amount)
	{
		CardNumber = cardNumber;
		Amount = amount;
	}
}