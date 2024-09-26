using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCurtain : MonoBehaviour
{
    [SerializeField] private Image _loadingFiller;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _lenghtAnimation;
    [SerializeField] private GameObject _loaderGroup;
    [SerializeField] private Button _startButton;
    
    private void Awake() 
        => DontDestroyOnLoad(gameObject);

    public void Show()
    {
        gameObject.SetActive(true);
        _startButton.onClick.AddListener(OnClickStart);
        StartCoroutine(AnimateLoading());
    }

    public void ShowStartButton()
    {
        _loaderGroup.SetActive(false);
        _startButton.gameObject.SetActive(true);
    }

    private void OnClickStart()
    {
        gameObject.SetActive(false);
        _startButton.onClick.RemoveAllListeners();
    }
    
    private IEnumerator AnimateLoading()
    {
        float time = 0;
        _loadingFiller.fillAmount = 0;
        
        while (time < _lenghtAnimation)
        {
            float value = _animationCurve.Evaluate(time);
            _loadingFiller.fillAmount = value;
            time += Time.deltaTime;
            yield return null;
        }
    }
    
}