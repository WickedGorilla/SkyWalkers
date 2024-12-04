using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ReferralsUpdatePopup : View
    {
        [SerializeField] private TMP_Text _referralsCountText;
        [SerializeField] private TMP_Text _referralsCoinsText;
        [SerializeField] private Button _claimButton;

        public Button ClaimButton => _claimButton;

        public void SetInfo(int referralsCount, int coinsCount)
        {
            _referralsCountText.text = $"Referrals: {referralsCount.ToString().ApplyWhiteColor()}" ;
            _referralsCoinsText.text = $"Coins: {NumbersFormatter.GetCoinsCountVariant(coinsCount)}" ;
        }
    }
}