using System;
using SkyExtensions;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenView : View
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _backButton;
    
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private RectTransform _pageBar;
    [SerializeField] private RectTransform _sliderPage;
    
    private int _currentPageIndex;

    public event Action OnEnd;
    
    private void OnEnable()
    {
        _nextButton.AddClickAction(OnClickNext);
        _backButton.AddClickAction(OnClickBack);
    }

    private void OnDisable()
    {
        _nextButton.RemoveClickAction(OnClickNext);
        _backButton.RemoveClickAction(OnClickBack);
    }

    private void OnClickNext()
    {
        if (_currentPageIndex == _pages.Length - 1)
        {
            OnEnd?.Invoke();
            return;
        }
        
        if (_currentPageIndex == 0) 
            _backButton.gameObject.SetActive(true);
        
        _pages[_currentPageIndex].SetActive(false);
        _pages[++_currentPageIndex].SetActive(true);
        _sliderPage.SetSiblingIndex(_currentPageIndex);
    }

    private void OnClickBack()
    {
        if (_currentPageIndex == 0)
            return;
        
        _pages[_currentPageIndex].SetActive(false);
        _pages[--_currentPageIndex].SetActive(true);
        _sliderPage.SetSiblingIndex(_currentPageIndex);
        
        if (_currentPageIndex == 0) 
            _backButton.gameObject.SetActive(false);
    }
}
