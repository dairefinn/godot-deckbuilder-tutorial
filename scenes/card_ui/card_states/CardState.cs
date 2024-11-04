using Godot;

namespace DeckBuilder;

public partial class CardState : Node
{
    public enum State
    {
        BASE = 0,
        CLICKED = 1,
        DRAGGING = 2,
        AIMING = 3,
        RELEASED = 4,
    }

    [Signal]
    public delegate void TransitionRequestedEventHandler(CardState from, State to);

    [Export]
    public State state;

    public CardUI cardUI;

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
        // Replace with function body
    }

    public virtual void OnInput(InputEvent @event)
    {
        // Replace with function body
    }

    public virtual void OnGuiInput(InputEvent @event)
    {
        // Replace with function body
    }

    public virtual void OnMouseEntered()
    {
        // Replace with function body
    }

    public virtual void OnMouseExited()
    {
        // Replace with function body
    }
}
