using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.UI;

namespace UnityTemplateProjects.Game
{
    public class Enemy: MonoBehaviour, IDropHandler, IPointerEnterHandler
    {

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                Debug.Log ("Dropped object was: "  + eventData.pointerDrag);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                FindObjectOfType<CardTargetLine>().ShowLineTarget(eventData.pointerDrag.transform, transform);
                Debug.Log ("Enter object was: "  + eventData.pointerDrag);
            }
        }
    }
}