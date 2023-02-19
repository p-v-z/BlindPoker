# Blind Poker ♠️🤖

This repo contains C# code that can be used to compare the strengths of two poker hands to determine which is stronger. It uses integers to represent the value of the cards, and does not make use of suits. 

It implements the following interface:
```
public interface IPokerSolver {
	public int PokerHandSolver(int[] player1Hand, int[] player2Hand);
}
```

The main implementation can be found 
[here (HandSolver.cs)](./src/HandSolver.cs)

The tests for this solution are implemented using [NUnit](https://nunit.org/) and can be found 
[here (Tests.cs)](./src/Tests.cs)