using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class UIFrameClickedEventArgs : EventArgs
{
    public UnitFrame Frame { get; set; }
}

public class UIRaycaster : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public event EventHandler<UIFrameClickedEventArgs> UIFrameClicked;

    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                UnitFrame frame = result.gameObject.GetComponent<UnitFrame>();
                if (frame)
                {
                    OnUIFrameClicked(frame);
                    break;
                }
            }
        }
    }

    protected virtual void OnUIFrameClicked(UnitFrame frame)
    {
        EventHandler<UIFrameClickedEventArgs> raiseEvent = UIFrameClicked;
        if (raiseEvent != null)
        {
            raiseEvent(this, new UIFrameClickedEventArgs { Frame = frame });
        }
    }
}
