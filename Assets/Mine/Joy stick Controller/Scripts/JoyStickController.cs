using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler
{

    public static JoyStickController Instance;

    public Vector3 joyStickVector;
    public RectTransform backgroundJoystick, handleJoyStick;
    public float handleRange;

    public bool Touched = false;


    private void Awake()
    {
        Instance = this;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Touched = false;
        joyStickVector = Vector3.zero;
        handleJoyStick.anchoredPosition = Vector2.zero;

        backgroundJoystick.gameObject.SetActive(false);

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Touched = true;
        backgroundJoystick.gameObject.SetActive(true);
        handleJoyStick.anchoredPosition = Vector2.zero;
        backgroundJoystick.transform.position = Input.mousePosition;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundJoystick,eventData.position,
            eventData.pressEventCamera,out Vector2 localPont))
        {
            var sizeBackgroundJoystick =backgroundJoystick.sizeDelta;
            localPont /= sizeBackgroundJoystick;

            joyStickVector = new Vector3(localPont.x * 2 - 1, localPont.y * 2 - 1, 0);
            joyStickVector = joyStickVector.magnitude > 1f ? joyStickVector.normalized : joyStickVector;

            float handlePos = sizeBackgroundJoystick.x / 2 * handleRange;
            handleJoyStick.anchoredPosition = Vector2.zero + new Vector2(joyStickVector.x * handlePos, joyStickVector.y * handlePos);

        }
    }
}
