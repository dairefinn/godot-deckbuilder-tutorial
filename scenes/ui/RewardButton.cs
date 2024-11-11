namespace DeckBuilder;

using Godot;

public partial class RewardButton : Button
{

    [Export] public Texture2D rewardIcon {
        get => _rewardIcon;
        set => SetRewardIcon(value);
    }
    private Texture2D _rewardIcon;
    [Export] public string rewardText {
        get => _rewardText;
        set => SetRewardText(value);
    }
    private string _rewardText;

    public TextureRect customIcon;
    public Label customText;

    public override void _Ready()
    {
        customIcon = GetNode<TextureRect>("%CustomIcon");
        customText = GetNode<Label>("%CustomText");

        Pressed += OnPressed;
    }

    public void OnPressed()
    {
        QueueFree();
    }

    public async void SetRewardIcon(Texture2D value)
    {
        _rewardIcon = value;

        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        customIcon.Texture = _rewardIcon;
    }

    public async void SetRewardText(string value)
    {
        _rewardText = value;

        if (!IsNodeReady())
        {
            await ToSignal(this, "ready");
        }

        customText.Text = _rewardText;
    }

}
