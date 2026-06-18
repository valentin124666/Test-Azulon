using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Tools
{
    public class TouchUtility : MonoBehaviour
    {
        public static bool Enabled = true;

        public static int TouchCount
        {
            get
            {
#if UNITY_ANDROID || UNITY_IOS
                return Touch.activeTouches.Count;
#else
                if (Mouse.current != null && Mouse.current.leftButton.isPressed)
                    return 1;

                if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                    return 1;

                return 0;
#endif
            }
        }

        public static SimulatedTouch GetTouch(int index)
        {
#if UNITY_ANDROID || UNITY_IOS
            if (index >= Touch.activeTouches.Count)
                return default;

            var sourceTouch = Touch.activeTouches[index];

            return new SimulatedTouch
            {
                position = sourceTouch.screenPosition,
                phase = ConvertPhase(sourceTouch.phase),
                fingerId = sourceTouch.touchId
            };

#else
            // ===== UNIVERSAL DESKTOP / TOUCHSCREEN =====

            // TOUCHSCREEN FIRST (якщо є)
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                var t = Touchscreen.current.primaryTouch;

                return new SimulatedTouch
                {
                    position = t.position.ReadValue(),
                    phase = ConvertTouchPhase(t.phase.ReadValue()),
                    fingerId = 0
                };
            }

            // FALLBACK: MOUSE
            if (Mouse.current != null)
            {
                var touch = new SimulatedTouch();

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    touch.position = Mouse.current.position.ReadValue();
                    touch.phase = TouchPhase.Began;
                    touch.fingerId = 0;
                }
                else if (Mouse.current.leftButton.isPressed)
                {
                    touch.position = Mouse.current.position.ReadValue();
                    touch.phase = TouchPhase.Moved;
                    touch.fingerId = 0;
                }
                else if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    touch.position = Mouse.current.position.ReadValue();
                    touch.phase = TouchPhase.Ended;
                    touch.fingerId = 0;
                }

                return touch;
            }

            return default;
#endif
        }

        private static TouchPhase ConvertTouchPhase(UnityEngine.InputSystem.TouchPhase phase)
        {
            return phase switch
            {
                UnityEngine.InputSystem.TouchPhase.Began => TouchPhase.Began,
                UnityEngine.InputSystem.TouchPhase.Moved => TouchPhase.Moved,
                UnityEngine.InputSystem.TouchPhase.Stationary => TouchPhase.Stationary,
                UnityEngine.InputSystem.TouchPhase.Ended => TouchPhase.Ended,
                UnityEngine.InputSystem.TouchPhase.Canceled => TouchPhase.Canceled,
                _ => TouchPhase.Canceled
            };
        }
    }

    public struct SimulatedTouch
    {
        public Vector2 position;
        public TouchPhase phase;
        public int fingerId;
    }
}