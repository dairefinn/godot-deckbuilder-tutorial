namespace DeckBuilder;

using System.Linq;
using Godot;
using Godot.Collections;

public partial class BattleReward : Control
{

    private static readonly PackedScene CARD_REWARDS = GD.Load<PackedScene>("res://scenes/ui/card_rewards.tscn");
    private static readonly PackedScene REWARD_BUTTON = GD.Load<PackedScene>("res://scenes/ui/reward_button.tscn");
    private static readonly Texture2D GOLD_ICON = GD.Load<Texture2D>("res://art/gold.png");
    private static readonly string GOLD_TEXT = "{0} gold";
    private static readonly Texture2D CARD_ICON = GD.Load<Texture2D>("res://art/rarity.png");
    private static readonly string CARD_TEXT = "Add New Card";

    [Export] public RunStats runStats;
    [Export] public CharacterStats characterStats;
    [Export] public RelicHandler relicHandler;

    public Button backButton;
    public VBoxContainer rewards;

    public float cardRewardTotalWeight = 0.0f;
    public Godot.Collections.Dictionary<Card.Rarity, float> cardRewardWeights = new()
    {
        { Card.Rarity.COMMON, 0.0f },
        { Card.Rarity.UNCOMMON, 0.0f },
        { Card.Rarity.RARE, 0.0f },
    };

    public override void _Ready()
    {
        backButton = GetNode<Button>("%BackButton");
        rewards = GetNode<VBoxContainer>("%Rewards");

        backButton.Pressed += OnBackButtonPressed;

        foreach(Node node in rewards.GetChildren())
        {
            node.QueueFree();
        }
    }

    public void AddGoldReward(int amount)
    {
        RewardButton rewardButton = REWARD_BUTTON.Instantiate<RewardButton>();
        rewardButton.rewardIcon = GOLD_ICON;
        rewardButton.rewardText = string.Format(GOLD_TEXT, amount);
        rewardButton.Pressed += () => {
            OnGoldRewardTaken(amount);
        };
        rewards.CallDeferred(VBoxContainer.MethodName.AddChild, rewardButton);
    }

    public void AddCardReward()
    {
        RewardButton cardReward = REWARD_BUTTON.Instantiate<RewardButton>();
        cardReward.rewardIcon = CARD_ICON;
        cardReward.rewardText = CARD_TEXT;
        cardReward.Pressed += () => {
            ShowCardRewards();
        };
        rewards.CallDeferred(VBoxContainer.MethodName.AddChild, cardReward);
    }

    public void AddRelicReward(Relic relic)
    {
        RewardButton relicReward = REWARD_BUTTON.Instantiate<RewardButton>();
        relicReward.rewardIcon = relic.icon;
        relicReward.rewardText = relic.relicName;
        relicReward.Pressed += () => {
            OnRelicRewardTaken(relic);
        };
        rewards.CallDeferred(VBoxContainer.MethodName.AddChild, relicReward);
    }

    public void OnGoldRewardTaken(int amount)
    {
        if (runStats == null) return;
        runStats.gold += amount;
    }

    public void ShowCardRewards()
    {
        if (runStats == null) return;
        if (characterStats == null) return;

        CardRewards cardRewards = CARD_REWARDS.Instantiate<CardRewards>();
        AddChild(cardRewards);
        cardRewards.CardRewardSelected += OnCardRewardTaken;

        Array<Card> cardRewardArray = new();
        Array<Card> availableCards = characterStats.draftableCards.cards.Duplicate();

        for (int i = 0; i < runStats.cardRewards; i++)
        {
            SetupCardChances();
            float roll = (float)GD.RandRange(0.0f, cardRewardTotalWeight);

            foreach (Card.Rarity rarity in cardRewardWeights.Keys)
            {
                if (cardRewardWeights[rarity] > roll)
                {
                    ModifyWeights(rarity);
                    Card card = GetRandomAvailableCard(availableCards, rarity);
                    cardRewardArray.Add(card);
                    availableCards.Remove(card);
                    break;
                }
            }
        }

        cardRewards.rewards = cardRewardArray;
        cardRewards.Show();
    }

    public void OnRelicRewardTaken(Relic relic)
    {
        if (relic == null) return;
        if (relicHandler == null) return;

        relicHandler.AddRelic(relic);
    }

    public void OnCardRewardTaken(Card card)
    {
        if (runStats == null) return;
        if (characterStats == null) return;

        characterStats.deck.AddCard(card);
    }

    public void SetupCardChances()
    {
        cardRewardTotalWeight = runStats.commonWeight + runStats.uncommonWeight + runStats.rareWeight;
        cardRewardWeights[Card.Rarity.COMMON] = runStats.commonWeight;
        cardRewardWeights[Card.Rarity.UNCOMMON] = runStats.uncommonWeight;
        cardRewardWeights[Card.Rarity.RARE] = runStats.rareWeight;
    }

    public void ModifyWeights(Card.Rarity rarityRolled)
    {
        if (rarityRolled == Card.Rarity.RARE)
        {
            runStats.rareWeight = RunStats.BASE_RARE_WEIGHT;
        }
        else {
            runStats.rareWeight = Mathf.Clamp(runStats.rareWeight + 0.3f, RunStats.BASE_RARE_WEIGHT, 5.0f);
        }
    }

    public Card GetRandomAvailableCard(Array<Card> availableCards, Card.Rarity withRarity)
    {
        // .Filter doesn't exist in the C# version of Godot.Collections.Array so we fall back to LINQ
        Card[] allPossibleCards = availableCards.Where(card => card.rarity == withRarity).ToArray();
        Array<Card> allPossibleCardsArray = new(allPossibleCards);
        return allPossibleCardsArray.PickRandom();
    }

    public void OnBackButtonPressed()
    {
        Events.Instance.EmitSignal(Events.SignalName.BattleRewardExited);
    }

}
