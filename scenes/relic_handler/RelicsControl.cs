namespace DeckBuilder;

using Godot;

public partial class RelicsControl : Control
{

    public static readonly int RELICS_PER_PAGE = 5;
    public static readonly float TWEEN_SCROLL_DURATION = 0.2f;

    [Export] public TextureButton leftButton;
    [Export] public TextureButton rightButton;

    public HBoxContainer relics;
    public float pageWidth;

    public int numberOfRelics = 0;
    public int currentPage = 1;
    public int maxPage = 0;
    public Tween tween;

    public override void _Ready()
    {
        relics = GetNode<HBoxContainer>("%Relics");
        pageWidth = CustomMinimumSize.X;

        leftButton.Pressed += OnLeftButtonPressed;
        rightButton.Pressed += OnRightButtonPressed;

        foreach (Node relicNode in relics.GetChildren())
        {
            if (relicNode is not RelicUI relicUI) continue;
            relicUI.Free();
        }

        relics.ChildOrderChanged += OnRelicsChildOrderChanged;
        OnRelicsChildOrderChanged();
    }
    
    public void Update()
    {
        if (!IsInstanceValid(leftButton)) return;
        if (!IsInstanceValid(rightButton)) return;

        numberOfRelics = relics.GetChildCount();
        maxPage = Mathf.CeilToInt(numberOfRelics / (float)RELICS_PER_PAGE);

        leftButton.Disabled = currentPage <= 1;
        rightButton.Disabled = currentPage >= maxPage;
    }

    public void TweenTo(float xPosition)
    {
        if (tween != null)
        {
            tween.Kill();
        }

        tween = CreateTween().SetTrans(Tween.TransitionType.Back).SetEase(Tween.EaseType.Out);
        tween.TweenProperty(relics, "position:x", xPosition, TWEEN_SCROLL_DURATION);
    }

    public void OnLeftButtonPressed()
    {
        if (currentPage > 1)
        {
            currentPage -= 1;
            Update();
            TweenTo(relics.Position.X + pageWidth);
        }
    }

    public void OnRightButtonPressed()
    {
        if (currentPage < maxPage)
        {
            currentPage += 1;
            Update();
            TweenTo(relics.Position.X - pageWidth);
        }
    }

    public void OnRelicsChildOrderChanged()
    {
        Update();
    }

}
