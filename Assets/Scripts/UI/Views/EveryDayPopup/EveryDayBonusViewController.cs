using SkyExtensions;
using UI.Core;

namespace UI.Views.EveryDayPopup
{
    public class EveryDayBonusViewController : ViewController<EveryDayBonusView>
    {
        public EveryDayBonusViewController(EveryDayBonusView view) : base(view)
        {
        }

        protected override void OnShow() 
            => View.ClaimButton.AddClickAction(OnClickClaim);

        protected override void OnHide() 
            => View.ClaimButton.RemoveClickAction(OnClickClaim);

        private void OnClickClaim()
            => Hide();
    }
}