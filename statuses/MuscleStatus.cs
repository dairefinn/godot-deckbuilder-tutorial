namespace DeckBuilder;

using Godot;


public partial class MuscleStatus : Status
{

	public static readonly float MODIFIER = 0.5f;

    public override void InitializeStatus(Node target)
    {
        StatusChanged += () => OnStatusChanged(target);
        OnStatusChanged(target);
    }

    public void OnStatusChanged(Node target)
    {
        if (target == null)
        {
            GD.PushError("Target is null");
            return;
        }

        ModifierHandler modifierHandler = (ModifierHandler)target.Get("modifierHandler");
        if (modifierHandler == null)
        {
            GD.PushError("No modifiers on " + target);
            return;
        }

        Modifier damageDealtModifier = modifierHandler.GetModifier(Modifier.Type.DMG_DEALT);
        if (damageDealtModifier == null)
        {
            GD.PrintErr("No damage dealt modifier on " + target);
            return;
        }

        ModifierValue muscleModifierValue = damageDealtModifier.GetValue("muscle");
        muscleModifierValue ??= ModifierValue.CreateNewModifier("muscle", ModifierValue.Type.FLAT);

        muscleModifierValue.flatValue = stacks;
        damageDealtModifier.AddNewValue(muscleModifierValue);
    }

}

