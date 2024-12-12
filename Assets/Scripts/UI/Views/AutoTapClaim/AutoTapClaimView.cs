using TMPro;
using UI.Core;
using UI.Views;
using UnityEngine;
using UnityEngine.UI;

public class AutoTapClaimView : View
{
    [SerializeField] private Button _claimButton;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _coinsClaimText;
    [SerializeField] private string _firstDescription;
    [SerializeField] private string _secondDescription;
    
    public Button ClaimButton => _claimButton;
    
    public void FillWithParameter(int countHours, int coinsClaim)
    {
        _descriptionText.text = $"{_firstDescription} {GetParameterTextWithColor(countHours)} hours {_secondDescription}";
        _coinsClaimText.text = NumbersFormatter.GetCoinsCountVariant(coinsClaim);;
    }
    
    private string GetParameterTextWithColor(int parameter)
        => $"<color=#FF39FF>{parameter}</color>";
}