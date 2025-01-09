using System;
using TMPro;
using UnityEngine;

namespace UI.Views.Timer
{
    public class ViewTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;
        
        public IUpdateTimer CreateTimer(int seconds, Action onComplete) 
            => new SecondsTimer(seconds, UpdateTimerText, onComplete);
        
        private void UpdateTimerText(int totalSeconds)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            _timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }
}