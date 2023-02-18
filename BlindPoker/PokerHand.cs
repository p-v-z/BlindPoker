namespace BlindPoker;

public enum PokerHand
{
	HighCard = 0,
	Pair = 1,
	TwoPair = 2,
	ThreeOfAKind = 3,
	Straight = 4,
	FullHouse = 5,
	FourOfAKind = 6
}

[Obsolete]
public enum HandTypes
{
	None, // high card
	Collection, // doubles, triples, quads, full house
	Sequence, // straight
}