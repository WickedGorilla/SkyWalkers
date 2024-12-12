using Game.Perks;
using Player;
using SkyExtensions;
using UI.Core;

public class AutoTapClaimViewController : ViewController<AutoTapClaimView>
{
    private readonly PerksService _perksService;
    private readonly WalletService _walletService;

    private int _coinsCount;

    public AutoTapClaimViewController(AutoTapClaimView view,
        PerksService perksService, WalletService walletService) : base(view)
    {
        _perksService = perksService;
        _walletService = walletService;
    }

    public void SetInfo(int claimCount)
    {
        _coinsCount = claimCount;
        View.FillWithParameter(_perksService.AutoTap.CurrentValue, _coinsCount);
    }

    protected override void OnShow()
        => View.ClaimButton.AddClickAction(OnClickClaim);

    protected override void OnHide()
        => View.ClaimButton.RemoveClickAction(OnClickClaim);

    private void OnClickClaim()
    {
        _walletService.Coins.Add(_coinsCount);
        Hide();
    }
}