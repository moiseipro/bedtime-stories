using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UIElements;

namespace CardManagment
{
    public enum DragState
    {
        Drag,
        Hand,
        Placed
    }
    public class DraggableCard : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private RectTransform _rectTransform, _handRectTransform;
        private CanvasGroup _canvasGroup;

        private DragState _dragState = DragState.Hand;
        private Card _card;

        private Vector3 _currentPosition, _oldPosition;
        private Quaternion _currentRotation, _oldRotation;
        private Vector3 _currentScale, _baseScale;
        private float _lerpSpeed = 5f;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = transform as RectTransform;
            _handRectTransform = _rectTransform.parent.transform as RectTransform;
            _currentScale = _baseScale = _rectTransform.localScale;
        }

        private void FixedUpdate()
        {
            _rectTransform.position = Vector3.Lerp(_rectTransform.position, _currentPosition, Time.deltaTime * _lerpSpeed);
            _rectTransform.rotation = Quaternion.Lerp(_rectTransform.rotation, _currentRotation, Time.deltaTime * _lerpSpeed);
            _rectTransform.localScale = Vector3.Lerp(_rectTransform.localScale, _currentScale, Time.deltaTime * _lerpSpeed);
        }
        
        public void SetCurPosition(Vector3 pos, Quaternion rot){
            _currentPosition = _oldPosition = pos;
            _currentRotation = _oldRotation = rot;
        }
    
        public void OnBeginDrag(PointerEventData eventData){
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _currentPosition);
            //Debug.Log(hand.GetComponent<RectTransform>().anchoredPosition.y);
            if(Mathf.Abs(_rectTransform.localPosition.y) < _handRectTransform.sizeDelta.y){
                _currentRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                _currentScale = _baseScale;
            } else { 
                _currentRotation = Quaternion.LookRotation(_rectTransform.forward, _rectTransform.position - _oldPosition);
                _currentScale = _baseScale/2f;
            }
        }
        
        public void OnEndDrag(PointerEventData eventData){
            _canvasGroup.blocksRaycasts = true;
            _currentPosition = _oldPosition;
            _currentRotation = _oldRotation;
            _currentScale = _baseScale;
        }
    }
}