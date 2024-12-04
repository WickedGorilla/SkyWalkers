using Infrastructure.Network.Request.Base.Player;
using SkyExtensions;
using UI.Core;

namespace UI.Views
{
    public class ReferralsUpdatePopupController : ViewController<ReferralsUpdatePopup>
    {
        public ReferralsUpdatePopupController(ReferralsUpdatePopup view) : base(view)
        {
        }

        public void SetInfo(ReferralInfo dataReferralInfo)
            => View.SetInfo(dataReferralInfo.CountReferrals, dataReferralInfo.NewAddedValue);
        
        protected override void OnHide() 
            => View.ClaimButton.AddClickAction(OnClickClaim);

        protected override void OnShow()
            => View.ClaimButton.RemoveClickAction(OnClickClaim);

        private void OnClickClaim() 
            => Hide();
    }
}