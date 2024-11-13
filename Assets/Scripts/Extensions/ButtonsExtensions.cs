using System;
using Infrastructure.Disposables;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkyExtensions
{
    public static class ButtonsExtensions 
    {
        public static IDisposable SubscribeListener(this Button button, UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
            return DisposableContainer.Create(Remove);

            void Remove() 
                => button.onClick.RemoveListener(onClick);
        }
        
        public static void AddClickAction(this Button button, UnityAction onClick) 
            => button.onClick.AddListener(onClick);
        
        public static void RemoveClickAction(this Button button, UnityAction onClick) 
            => button.onClick.RemoveListener(onClick);
    }
}