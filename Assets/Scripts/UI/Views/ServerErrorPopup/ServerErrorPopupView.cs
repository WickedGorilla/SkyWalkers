using System;
using SkyExtensions;
using TMPro;
using UI.Core;
using UI.Loading;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.ServerErrorPopup
{
    public class ServerErrorPopupView : View
    {
        [SerializeField] private TMP_Text _errorText;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private CircleLoader _circleLoaderButton;
        [SerializeField] private GameObject _textButton;
        
        private const string ErrorText = "Something went wrong.\n Code error:";
        
        public IDisposable Initialize(int errorCode, Action refreshAction)
        {
            _errorText.text = $"{ErrorText} #{errorCode}";
            return _refreshButton.SubscribeListener(refreshAction);
        }

        public void ShowLoader() 
            => ShowLoaderButton(true);

        public void HideLoader() 
            => ShowLoaderButton(false);

        private void ShowLoaderButton(bool value)
        {
            _refreshButton.interactable = !value;
            _textButton.SetActive(!value);
            _circleLoaderButton.gameObject.SetActive(value);
        }
    }
}