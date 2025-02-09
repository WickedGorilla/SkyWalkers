using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class InviteView : View
    {
        [SerializeField] private TMP_Text _countReferralText;
        [SerializeField] private TMP_Text _scoreReferralText;
        [SerializeField] private TMP_Text _referralLinkText;

        [SerializeField] private Button _copyLinkButton;
        [SerializeField] private Button _shareLinkButton;

        public Button CopyLinkButton => _copyLinkButton;
        public Button ShareLinkButton => _shareLinkButton;

        public void Initialize(string link, int referralCount, int score)
        {
            _countReferralText.text = $"You referrals: <color=#FFFFFF>{referralCount}</color>";
            _scoreReferralText.text = $"Score: <color=#FFFFFF>{NumbersFormatter.GetCoinsCountVariant(score)}</color>";

            int maxLength = 20;

            string shortenedLink = link.Length > maxLength 
                ? link.Substring(0, maxLength) 
                : link;
            
            _referralLinkText.text = $"{shortenedLink}... <sprite name=copy>";
        }
        
    }
}