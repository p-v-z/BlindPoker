# Blind Poker ♠️🤖♥️

This repo contains C# code that can be used to compare the strengths of two poker hands to determine which is stronger. It uses integers to represent the value of the cards, and does not make use of suits. It uses a standard 52 pack with no jokers. 

## How to run the tests

The tests for this solution are implemented using [NUnit](https://nunit.org/) and can be found
[here (Tests.cs)](./src/Tests.cs)

1. [Clone the repo](https://docs.github.com/en/repositories/creating-and-managing-repositories/cloning-a-repository) to your local machine
2. Open the solution in Visual Studio / Rider
3. Run the tests using the aforementioned script and test runner of your choice

## Code
This repo implements the following interface:
```
public interface IPokerSolver {
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand);
}
```

The root can be found [here (HandSolver.cs)](./src/HandSolver.cs)


