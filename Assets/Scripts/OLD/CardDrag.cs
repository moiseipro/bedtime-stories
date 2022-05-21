using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private GameObject hand, canvas;
	private RectTransform m_DraggingPlane;
    private Vector3 oldCardPosition, curCardPosition, curScale;
    private Vector3 baseScale = new Vector3(1,1,1);
    private Quaternion oldCardRotation, curCardRotation;
    private bool isDrag = false, isEnter = false, isFlying = false, isFocused = false;

    void Start()
    {
        hand = GameObject.Find("hand");
        canvas = gameObject.GetComponentInParent<Canvas>().gameObject;
        m_DraggingPlane = transform as RectTransform;
        curScale = baseScale;
    }

    public void SetCurPosition(Vector3 pos, Quaternion rot){
        curCardPosition = oldCardPosition = pos;
        curCardRotation = oldCardRotation = rot;
        isFlying = true;
    }
    public void SetFocus(bool focus){
        isFocused = focus;
    }
    public void FocusedPlayer(Vector3 pos){
        SetFocus(true);
        curCardPosition = pos + Vector3.left * Screen.width/8f;
        curCardRotation = Quaternion.Euler(Vector3.zero);
        curScale = baseScale*1.2f;
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!isDrag && eventData.pointerDrag.tag == "Card"){
            if(!isFlying){
                oldCardPosition = m_DraggingPlane.transform.position;
                oldCardRotation = m_DraggingPlane.rotation;
                isFlying = true;
            }
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            isDrag = true;
            hand.GetComponent<HandOptions>().curCardHand = gameObject.GetComponent<CardDrag>();
            //Debug.Log(m_DraggingPlane);
        }
    }

    public void OnDrag(PointerEventData eventData){
        //if (eventData.pointerEnter != null && eventData.pointerEnter.transform as RectTransform != null)
			//m_DraggingPlane = eventData.pointerEnter.transform as RectTransform;
        if(m_DraggingPlane && isDrag && isFlying && !isFocused){
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventData.position, eventData.pressEventCamera, out curCardPosition);
            //Debug.Log(hand.GetComponent<RectTransform>().anchoredPosition.y);
            if(Mathf.Abs(m_DraggingPlane.localPosition.y) < hand.GetComponent<RectTransform>().sizeDelta.y){
                curCardRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                curScale = baseScale*2f;
            } else { 
                curCardRotation = Quaternion.LookRotation(m_DraggingPlane.transform.forward, m_DraggingPlane.transform.position - oldCardPosition);
                curScale = baseScale/2f;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if(isDrag && eventData.pointerDrag.tag == "Card"){
            isDrag = false;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            curCardPosition = oldCardPosition;
            curCardRotation = oldCardRotation;
            curScale = baseScale;
            hand.GetComponent<HandOptions>().curCardHand = null;
        }
    }

    void Update()
    {   
        if(isFlying){
            m_DraggingPlane.position = Vector3.Lerp(m_DraggingPlane.position, curCardPosition, Time.deltaTime * 10f);
            m_DraggingPlane.rotation = Quaternion.Lerp(m_DraggingPlane.rotation, curCardRotation, Time.deltaTime * 10f);
            if((oldCardPosition - m_DraggingPlane.position).magnitude < 0.5f){
                m_DraggingPlane.position = oldCardPosition;
                isFlying = false;
            }
        }
        m_DraggingPlane.localScale = Vector3.Lerp(m_DraggingPlane.localScale, curScale, Time.deltaTime * 10f);
            //m_DraggingPlane.rotation = Quaternion.AngleAxis(-m_DraggingPlane.transform.position.x/10f, Vector3.forward);
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        //Debug.Log(eventData);
        if(!isDrag && !isEnter && eventData.pointerEnter.tag == "Card"){
            isEnter = true;
            curScale = baseScale*1.1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!isDrag){
            curScale = baseScale;
            isEnter = false;
            curCardPosition = oldCardPosition;
            curCardRotation = oldCardRotation;
        }
    }

    public void OnPointerClick(PointerEventData eventData){
        
    }

    public void OnPointerDown(PointerEventData eventData){

    }

    public void OnPointerUp(PointerEventData eventData){
        
    }

}
