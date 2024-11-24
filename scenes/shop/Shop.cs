namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;

public partial class Shop : Control
{

    public static readonly PackedScene SHOP_CARD = GD.Load<PackedScene>("res://scenes/shop/shop_card.tscn");
    public static readonly PackedScene SHOP_RELIC = GD.Load<PackedScene>("res://scenes/shop/shop_relic.tscn");

    [Export] public Array<Relic> shopRelics = new();
    [Export] public CharacterStats charStats;
    [Export] public RunStats runStats;
    [Export] public RelicHandler relicHandler;

    public HBoxContainer cards;
    public HBoxContainer relics;
    public CardTooltipPopup cardTooltipPopup;
    public Button backButton;
    public Timer blinkTimer;
    public AnimationPlayer shopKeeperAnimation;
    public ModifierHandler modifierHandler;

    public override void _Ready()
    {
        cards = GetNode<HBoxContainer>("%Cards");
        relics = GetNode<HBoxContainer>("%Relics");
        cardTooltipPopup = GetNode<CardTooltipPopup>("%CardTooltipPopup");
        backButton = GetNode<Button>("%BackButton");
        blinkTimer = GetNode<Timer>("%BlinkTimer");
        shopKeeperAnimation = GetNode<AnimationPlayer>("%ShopkeeperAnimation");
        modifierHandler = GetNode<ModifierHandler>("ModifierHandler");

        backButton.Pressed += OnButtonPressed;

        foreach (Node shopCardNode in cards.GetChildren())
        {
            if (shopCardNode is not ShopCard shopCard) continue;
            shopCard.QueueFree();
        }

        foreach (Node shopRelicNode in relics.GetChildren())
        {
            if (shopRelicNode is not ShopRelic shopRelic) continue;
            shopRelic.QueueFree();
        }

        Events.Instance.ShopCardBought += OnShopCardBought;
        Events.Instance.ShopRelicBought += OnShopRelicBought;

        BlinkTimerSetup();
        blinkTimer.Timeout += OnBlinkTimerTimeout;
    }

    public void BlinkTimerSetup()
    {
        blinkTimer.WaitTime = GD.RandRange(1.0f, 5.0f);
        blinkTimer.Start();
    }

    public void OnBlinkTimerTimeout()
    {
        shopKeeperAnimation.Play("blink");
        BlinkTimerSetup();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel") && cardTooltipPopup.Visible)
        {
            cardTooltipPopup.HideTooltip();
        }
    }

    public void PopulateShop()
    {
        GenerateShopCards();
        GenerateShopRelics();
    }

    public void GenerateShopCards()
    {
        Array<Card> shopCardArray = new();
        Array<Card> availableCards = charStats.draftableCards.cards.Duplicate(true);
        RNG.ArrayShuffle(availableCards);
        shopCardArray = availableCards[..3];

        foreach (Card card in shopCardArray)
        {
            ShopCard newShopCard = SHOP_CARD.Instantiate<ShopCard>();
            cards.AddChild(newShopCard);
            newShopCard.card = card;
            newShopCard.currentCardUI.TooltipRequested += cardTooltipPopup.ShowTooltip;
            newShopCard.goldCost = GetUpdatedShopCost(newShopCard.goldCost);
            newShopCard.Update(runStats);
        }
    }

    public void GenerateShopRelics()
    {
        Array<Relic> shopRelicArray = new();
        Array<Relic> availableRelics = new(shopRelics.Where(relic => {
            bool canAppear = relic.CanAppearAsReward(charStats);
            bool alreadyHadIt = relicHandler.HasRelic(relic.id);
            return canAppear && !alreadyHadIt;
        }));
        RNG.ArrayShuffle(availableRelics);
        shopRelicArray = availableRelics[..3];

        foreach (Relic relic in shopRelicArray)
        {
            ShopRelic newShopRelic = SHOP_RELIC.Instantiate<ShopRelic>();
            relics.AddChild(newShopRelic);
            newShopRelic.relic = relic;
            newShopRelic.goldCost = GetUpdatedShopCost(newShopRelic.goldCost);
            newShopRelic.Update(runStats);
        }
    }

    public int GetUpdatedShopCost(int originalCost)
    {
        return modifierHandler.GetModifiedValue(originalCost, Modifier.Type.SHOP_COST);
    }

    public void UpdateItems()
    {
        foreach (Node shopCardNode in cards.GetChildren())
        {
            if (shopCardNode is not ShopCard shopCard) continue;
            shopCard.Update(runStats);
        }

        foreach (Node shopRelicNode in relics.GetChildren())
        {
            if (shopRelicNode is not ShopRelic shopRelic) continue;
            shopRelic.Update(runStats);
        }

        foreach (Node shopRelicNode in relics.GetChildren())
        {
            if (shopRelicNode is not ShopRelic shopRelic) continue;
            shopRelic.Update(runStats);
        }
    }

    public void UpdateItemCosts()
    {
        foreach (Node shopCardNode in cards.GetChildren())
        {
            if (shopCardNode is not ShopCard shopCard) continue;
            shopCard.goldCost = GetUpdatedShopCost(shopCard.goldCost);
            shopCard.Update(runStats);
        }

        foreach (Node shopRelicNode in relics.GetChildren())
        {
            if (shopRelicNode is not ShopRelic shopRelic) continue;
            shopRelic.goldCost = GetUpdatedShopCost(shopRelic.goldCost);
            shopRelic.Update(runStats);
        }
    }

    public void OnButtonPressed()
    {
        Events.Instance.EmitSignal(Events.SignalName.ShopExited);
    }

    public void OnShopCardBought(Card card, int goldCost)
    {
        charStats.deck.AddCard(card);
        runStats.gold -= goldCost;
        UpdateItems();
    }

    public void OnShopRelicBought(Relic relic, int goldCost)
    {
        relicHandler.AddRelic(relic);
        runStats.gold -= goldCost;

        if(relic is CouponsRelic couponsRelic)
        {
            couponsRelic.AddShopModifier(this);
            UpdateItemCosts();
        }
        
        UpdateItems();
    }

}
