namespace DeckBuilder;

using Godot;

public partial class ExposedStatus : Status
{

	public static readonly float MODIFIER = 0.5f;

    public override void InitializeStatus(Node target)
    {
		if (target == null)
		{
			GD.PrintErr("ExposedStatus: target is null");
			return;
		}

		ModifierHandler modifierHandler = (ModifierHandler)target.Get("modifierHandler");
		if (modifierHandler == null)
		{
			GD.PrintErr("ExposedStatus: modifierHandler is null");
			return;
		}

		Modifier damageTakenModifier = modifierHandler.GetModifier(Modifier.Type.DMG_TAKEN);
		if (damageTakenModifier == null)
		{
			GD.PrintErr("ExposedStatus: damageTakenModifier is null");
			return;
		}

		ModifierValue exposedModifierValue = damageTakenModifier.GetValue("exposed");

		if (exposedModifierValue == null)
		{
			exposedModifierValue = ModifierValue.CreateNewModifier("exposed", ModifierValue.Type.PERCENT_BASED);
			exposedModifierValue.percentValue = MODIFIER;
			damageTakenModifier.AddNewValue(exposedModifierValue);
		}

		if (!IsConnected(SignalName.StatusChanged, new Callable(this, MethodName.OnStatusChanged)))
		{
			StatusChanged += () => OnStatusChanged(damageTakenModifier);
		}
    }

    public void OnStatusChanged(Modifier damageTakenModifier)
	{
		if (duration <= 0 && damageTakenModifier != null)
		{
			damageTakenModifier.RemoveValue("exposed");
		}
	}

    public override string GetTooltip()
    {
        return string.Format(tooltip, duration);
    }

}

