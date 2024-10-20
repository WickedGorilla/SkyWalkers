using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public class OnGameFocusEvent : MonoBehaviour
    {
        private readonly LinkedList<Action> _onFocusEvents = new(); 
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
            {
                foreach (var focusEvent in _onFocusEvents)
                    focusEvent();
            }
               
            _onFocusEvents.Clear();
        }

        public void AddOnFocusEvent(Action onFocus)
        {
            _onFocusEvents.AddLast(onFocus);
        }
    }
}