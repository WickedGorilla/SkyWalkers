using DG.Tweening;
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
    [SerializeField] private RectTransform _lightingImage;
    
    public Button ClaimButton => _claimButton;

    private void OnEnable()
    {
        _lightingImage.DORotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable() 
        => _lightingImage.DOKill();
    
    public void FillWithParameter(int countHours, int coinsClaim)
    {
        _descriptionText.text = $"{_firstDescription} {GetParameterTextWithColor(countHours)} hours {_secondDescription}";
        _coinsClaimText.text = $"{SpritesAtlasCode.Coin} {coinsClaim}";
    }
    
    private string GetParameterTextWithColor(int parameter)
        => $"<color=#FF39FF>{parameter}</color>";
    
    
}