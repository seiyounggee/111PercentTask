using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIComponent_InGameInput : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Vector2 prevPointerPosition;
    private Vector2 initialPointerPosition;
    private Vector2 currentSwipe;
    private float dragLength = 0f;
    public float swipeThreshold = 5f; // swipe로 판단할 거리

    public delegate void PointerUp(GestureType sd, float strength, float length);
    public PointerUp pointerUpCallback = null;

    public delegate void PointerDown(GestureType sd, float strength);
    public PointerDown pointerDownCallback = null;

    public delegate void DragDel(GestureType sd, float strength);
    public DragDel dragCallback = null;

    public enum GestureType { PointTouch, SwipeLeft, SwipeRight, SwipeUp, SwipeDown }

    public void OnPointerDown(PointerEventData eventData)
    {
        prevPointerPosition = eventData.position;
        initialPointerPosition = eventData.position;
        dragLength = 0f;

        pointerDownCallback?.Invoke(GestureType.PointTouch, 0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentSwipe = eventData.position - prevPointerPosition;
        prevPointerPosition = eventData.position;
        dragLength += currentSwipe.magnitude;

        GestureType swipeDirection;
        if (Mathf.Abs(currentSwipe.x) > Mathf.Abs(currentSwipe.y))
        {
            swipeDirection = currentSwipe.x < 0 ? GestureType.SwipeLeft : GestureType.SwipeRight;
        }
        else
        {
            swipeDirection = currentSwipe.y < 0 ? GestureType.SwipeDown : GestureType.SwipeUp;
        }

        dragCallback?.Invoke(swipeDirection, currentSwipe.magnitude);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var finalSwipe = eventData.position - initialPointerPosition;
        var strength = finalSwipe.magnitude;

        if (strength >= swipeThreshold) // Check if the swipe length is above the threshold
        {
            GestureType swipeDirection;
            if (Mathf.Abs(finalSwipe.x) > Mathf.Abs(finalSwipe.y))
            {
                swipeDirection = finalSwipe.x < 0 ? GestureType.SwipeLeft : GestureType.SwipeRight;
            }
            else
            {
                swipeDirection = finalSwipe.y < 0 ? GestureType.SwipeDown : GestureType.SwipeUp;
            }

            pointerUpCallback?.Invoke(swipeDirection, strength, dragLength);
        }
        else
        {
            pointerUpCallback?.Invoke(GestureType.PointTouch, 0f, 0f);
        }
    }
}
