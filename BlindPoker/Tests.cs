namespace BlindPoker;

public class Tests
{
	private readonly HandSolver _handSolver = new HandSolver();
	[SetUp]	public void Setup() {}
	
	/// <summary>
	/// Tests that the correct error is thrown when the amount of cards in a hand is incorrect
	/// </summary>
	[Test]
	public void TestIncorrectAmount()
    {
        // Arrange
        var a = new[] { 1, 2, 3, 4, 5 };
		var b = new[] { 1, 2, 3, 4 };

		// Act
		var resultA = _handSolver.PokerHandSolver(a, b);
		var resultB = _handSolver.PokerHandSolver(b, a);
	    
		// Assert
        Assert.Multiple(() =>
        {
            Assert.That(resultA, Is.EqualTo(-1));
            Assert.That(resultB, Is.EqualTo(-1));
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
		
	}
}
