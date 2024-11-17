using Game.Perks;
using Player;
using SkyExtensions;
using UI.Core;

public class AutoTapClaimViewController : ViewController<AutoTapClaimView>
{
    private readonly PerksService _perksService;
    private readonly WalletService _walletService;
    private readonly ViewService _viewService;

    private int _coinsCount;

    public AutoTapClaimViewController(AutoTapClaimView view, 
        PerksService perksService, WalletService walletService,
        ViewService viewService) : base(view)
    {
        _perksService = perksService;
        _walletService = walletService;
        _viewService = viewService;
    }

    public void SetInfo(int claimCount)
    {
        _coinsCount = claimCount;
        View.FillWithParameter(_perksService.AutoTap.CurrentValue, _coinsCount);
        View.ClaimButton.AddClickAction(OnClickClaim);
    }

    protected override void OnHide()
    {
        View.ClaimButton.RemoveClickAction(OnClickClaim);
    }

    private void OnClickClaim()
    {
        _walletService.Coins.Add(_coinsCount);
        _viewService.HidePermanent<AutoTapClaimViewController>();
    }
}
