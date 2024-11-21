namespace DeckBuilder;

using Godot;

public partial class CouponsRelic : Relic
{

    [Export(PropertyHint.Range, "1, 100")] public int discount = 50;

    public RelicUI relicUI;

    public override void InitializeRelic(RelicUI owner)
    {
        Events.Instance.ShopEntered += AddShopModifier;
        relicUI = owner;
    }

    public override void DeactivateRelic(RelicUI owner)
    {
        Events.Instance.ShopEntered -= AddShopModifier;
    }

    public void AddShopModifier(Shop shop)
    {
        relicUI.Flash();

        Modifier shopCostModifier = shop.modifierHandler.GetModifier(Modifier.Type.SHOP_COST);
        if (shopCostModifier == null)
        {
            GD.PrintErr("Shop cost modifier not found");
            return;
        }

        ModifierValue couponsModifierValue = shopCostModifier.GetValue("coupons");

        if (couponsModifierValue == null)
        {
            couponsModifierValue = ModifierValue.CreateNewModifier("coupons", ModifierValue.Type.PERCENT_BASED);
            couponsModifierValue.percentValue = -1 * discount / 100.0f;
            shopCostModifier.AddNewValue(couponsModifierValue);
        }
    }

}
