using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.EveryDayPopup
{
    public class EveryDayBonusView : View
    {
        [SerializeField] private Button _claimButton;
        
        public Button ClaimButton => _claimButton;
    }
}