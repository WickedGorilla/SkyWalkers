using UnityEngine;

namespace Infrastructure
{
    public static class ScreenInput
    {
        public static bool GetTouchDown()
        {
            if (Input.touchCount == 0)
                return Input.GetMouseButtonDown(0); 
            
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Began;
        }
        
        public static bool GetTouchUp()
        {
            if (Input.touchCount == 0)
                return Input.GetMouseButtonUp(0);
            
            Touch touch = Input.GetTouch(0);
            return touch.phase == TouchPhase.Ended;
        }
        
        public static bool GetTouch(out Vector2 position)
        {
            if (Input.touchCount == 0)
            {
                var isMouseDown = Input.GetMouseButton(0);
                position = isMouseDown ? Input.mousePosition : Vector2.zero;
                return isMouseDown;
            }
            
            Touch touch = Input.GetTouch(0);

            var isTouch = touch.phase is TouchPhase.Moved or TouchPhase.Stationary;
            position = isTouch ? touch.position : Vector2.zero;
            return isTouch;
        }
    }
}