namespace BlindPoker;

public class Tests
{
	private readonly IPokerSolver _pokerSolver = new HandSolver();
	
	[SetUp] public void Setup() {}
	
	/// <summary>
	/// Tests that the correct error is thrown when the amount of cards in a hand is incorrect
	/// </summary>
	[Test]
	public void TestIncorrectAmount()
    {
        // Arrange
        var a = new[] { 1, 2, 3, 4, 5 };
		var b = new[] { 1, 2, 3, 4 }; // 4 cards instead of 5
		var c = new[] { 1, 2, 3, 4, 5, 6 }; // Too many

		// Act
		var resultA = _pokerSolver.PokerHandSolver(a, b);
		var resultB = _pokerSolver.PokerHandSolver(b, a);
		var resultC = _pokerSolver.PokerHandSolver(a, c);
	    
		// Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultA, Is.EqualTo(-1));
            Assert.That(resultB, Is.EqualTo(-1));
            Assert.That(resultC, Is.EqualTo(-1));
        });
    }

	/// <summary>
	/// Test to make sure that the input is valid:
	/// - All cards are between 1 and 13
	/// - Max 4 cards of the same value
	/// </summary>
	[Test]
	public void TestInvalidInput()
	{
		var c = new[] { 1, 2, 3, 1, 7 };
		var b = new[] { 1, 2, 3, 0, 7 }; // 0 is not a valid card
		var a = new[] { 1, 15, 3, 4, 5 }; // 15 is not a valid card
		var d = new[] { 1, 1, 1, 1, 1 }; // 5 cards of the same value
		
		// Act
		var resultA = _pokerSolver.PokerHandSolver(a, b);
		var resultB = _pokerSolver.PokerHandSolver(a, c);
		var resultC = _pokerSolver.PokerHandSolver(b, c);
		var resultD = _pokerSolver.PokerHandSolver(a, d);
		
		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(resultA, Is.EqualTo(-1));
			Assert.That(resultB, Is.EqualTo(-1));
			Assert.That(resultC, Is.EqualTo(-1));
			Assert.That(resultD, Is.EqualTo(-1));
		});
	}

	private readonly int[] _high = { 1, 3, 5, 7, 9 }; // High card 9
	private readonly int[] _pair = { 1, 3, 5, 7, 7 }; // Pair
	private readonly int[] _pairB = { 1, 1, 5, 7, 9 }; // Different pair
	[Test] public void PairBeatsHigh() => Assert.That(_pokerSolver.PokerHandSolver(_high, _pair), Is.EqualTo(2));
	[Test] public void HighLosesPair() => Assert.That(_pokerSolver.PokerHandSolver(_pair, _high), Is.EqualTo(1));
	[Test] public void DifferentPairs() => Assert.That(_pokerSolver.PokerHandSolver(_pair, _pairB), Is.EqualTo(1));
	[Test] public void PairTie() => Assert.That(_pokerSolver.PokerHandSolver(_pair, _pair), Is.EqualTo(0));
	
	
	private readonly int[] _twoPair = { 1, 3, 3, 7, 7 }; // Two pair
	private readonly int[] _twoPairB = { 1, 2, 2, 10, 10 }; // Higher two pair
	[Test] public void TwoBeatsPair() => Assert.That(_pokerSolver.PokerHandSolver(_pair, _twoPair), Is.EqualTo(2));
	[Test] public void PairLosesTwo() => Assert.That(_pokerSolver.PokerHandSolver(_twoPair, _pair), Is.EqualTo(1));
	[Test] public void DifferentTwoPairs() => Assert.That(_pokerSolver.PokerHandSolver(_twoPair, _twoPairB), Is.EqualTo(2));
	[Test] public void TwoPairTie() => Assert.That(_pokerSolver.PokerHandSolver(_twoPair, _twoPair), Is.EqualTo(0));
	
	
	private readonly int[] _threeOfAKind = { 1, 3, 7, 7, 7 }; // Three of a kind
	private readonly int[] _threeOfAKindB = { 3, 3, 3, 7, 8 }; // Lower threes
	[Test] public void ThreeBeatsTwo() => Assert.That(_pokerSolver.PokerHandSolver(_twoPair, _threeOfAKind), Is.EqualTo(2));
	[Test] public void TwoLosesThree() => Assert.That(_pokerSolver.PokerHandSolver(_threeOfAKind, _twoPair), Is.EqualTo(1));
	[Test] public void DifferentThrees() => Assert.That(_pokerSolver.PokerHandSolver(_threeOfAKind, _threeOfAKindB), Is.EqualTo(1));
	[Test]public void ThreeTie() => Assert.That(_pokerSolver.PokerHandSolver(_threeOfAKind, _threeOfAKind), Is.EqualTo(0));
	
	
	private readonly int[] _straight = { 1, 2, 3, 4, 5 }; // Straight 
	private readonly int[] _straightB = { 3, 4, 5, 6, 7 }; // Higher straight 
	[Test] public void StraightBeatsThree() => Assert.That(_pokerSolver.PokerHandSolver(_threeOfAKind, _straight), Is.EqualTo(2));
	[Test] public void ThreeLosesStraight() => Assert.That(_pokerSolver.PokerHandSolver(_straight, _threeOfAKind), Is.EqualTo(1));
	[Test] public void DifferentStraights() => Assert.That(_pokerSolver.PokerHandSolver(_straight, _straightB), Is.EqualTo(1));
	[Test] public void StraightTie() => Assert.That(_pokerSolver.PokerHandSolver(_straight, _straight), Is.EqualTo(0));

	
	private readonly int[] _fullHouse = { 1, 1, 9, 9, 9 }; // Full house
	private readonly int[] _fullHouseB = { 10, 10, 10, 9, 9 }; // Higher house
	private readonly int[] _fullHouseC = { 10, 10, 1, 1, 1 }; // Higher house
	[Test] public void FullBeatsStraight() => Assert.That(_pokerSolver.PokerHandSolver(_straight, _fullHouse), Is.EqualTo(2));
	[Test] public void StraightLosesFull() => Assert.That(_pokerSolver.PokerHandSolver(_fullHouse, _straight), Is.EqualTo(1));
	[Test] public void DifferentHouses() => Assert.That(_pokerSolver.PokerHandSolver(_fullHouse, _fullHouseB), Is.EqualTo(2));
	[Test] public void UsesBiggestPack() => Assert.That(_pokerSolver.PokerHandSolver(_fullHouse, _fullHouseC), Is.EqualTo(1));
	[Test] public void HouseTie() => Assert.That(_pokerSolver.PokerHandSolver(_fullHouse, _fullHouse), Is.EqualTo(0));

	
	private readonly int[] _fourOfAKind = { 1, 1, 1, 1, 9 }; // Four of a kind
	private readonly int[] _fourOfAKindB = { 5, 2, 2, 2, 2 }; // Different four of a kind
	[Test] public void FourBeatsFull() => Assert.That(_pokerSolver.PokerHandSolver(_fullHouse, _fourOfAKind), Is.EqualTo(2));
	[Test] public void FullLosesFour() => Assert.That(_pokerSolver.PokerHandSolver(_fourOfAKind, _fullHouse), Is.EqualTo(1));
	[Test] public void DifferentFours() => Assert.That(_pokerSolver.PokerHandSolver(_fourOfAKind, _fourOfAKindB), Is.EqualTo(2));
	[Test] public void FoursTie() => Assert.That(_pokerSolver.PokerHandSolver(_fourOfAKind, _fourOfAKind), Is.EqualTo(0));

	
	private readonly int[] _highB = { 1, 3, 5, 8, 9 }; // _high with 2nd highest card changed
	private readonly int[] _highC = { 2, 3, 5, 7, 9 }; // Lower high than _high
	private readonly int[] _highD = { 3, 4, 5, 7, 9 }; // Higher than _highC on smallest card
	[Test] public void HighCardAce() => Assert.That(_pokerSolver.PokerHandSolver(_high, _highC), Is.EqualTo(1)); // A high is ace
	[Test] public void HighCardSmall() => Assert.That(_pokerSolver.PokerHandSolver(_highC, _highD), Is.EqualTo(2)); // Smaller card is higher
	[Test] public void HighCardTieSame() => Assert.That(_pokerSolver.PokerHandSolver(_high, _high), Is.EqualTo(0));
	[Test] public void HighCardTie() => Assert.That(_pokerSolver.PokerHandSolver(_high, _highB), Is.EqualTo(2)); // B 2nd highest card is higher
}
