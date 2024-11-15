using Game.Perks;
using UI.Core;

public class AutoTapClaimViewController : ViewController<AutoTapClaimView>
{
    private readonly PerksService _perksService;

    public AutoTapClaimViewController(AutoTapClaimView view, PerksService perksService) : base(view)
    {
        _perksService = perksService;
    }

    protected override void OnShow()
    {

    }

    public void SetInfo(int claimCount)
    {
        View.FillWithParameter(_perksService.AutoTap.CurrentValue, claimCount);
    }
    
    protected override void OnHide()
    {
        
    }
}
