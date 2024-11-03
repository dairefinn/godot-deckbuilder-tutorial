using Godot;
using System.Collections.Generic;

namespace DeckBuilder;

public partial class CardStateMachine : Node
{

	[Export]
	public CardState initialState;

	private CardState currentState;
	protected Dictionary<CardState.State, CardState> states = new();
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Init(CardUI card)
	{
		foreach (Node child in GetChildren())
		{
			if (child is CardState state)
			{
				states[state.state] = state;
				state.TransitionRequested += OnTransitionRequested;
				state.cardUI = card;
			}
		}

		if (initialState != null)
		{
			initialState.Enter();
			currentState = initialState;
		}
	}

	public void OnInput(InputEvent _event)
	{
		currentState?.OnInput(_event);
	}

	public void OnGuiInput(InputEvent _event)
	{
		currentState?.OnGuiInput(_event);
	}

	public void OnMouseEntered()
	{
		currentState?.OnMouseEntered();
	}

	public void OnMouseExited()
	{
		currentState?.OnMouseExited();
	}

	public void OnTransitionRequested(CardState from, CardState.State to)
	{
		if (from != currentState) return; // Something went wrong

		CardState newState = states[to];
		if (newState == null) return; // Something went wrong

		if (currentState != null)
		{
			currentState.Exit();
		}

		newState.Enter();
		currentState = newState;
	}

}
