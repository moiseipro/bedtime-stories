using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.UI;
using UnityTemplateProjects;

namespace CardManagment
{
    public enum DragState
    {
        Drag,
        Hand,
        Placed,
        Target
    }
    public class DraggableCard : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private RectTransform _rectTransform, _handRectTransform;
        private CanvasGroup _canvasGroup;

        private DragState _dragState = DragState.Hand;
        private GameCard _card;
        public GameCard Card => _card;
        private CardTargetLine _cardTargetLine;
        private FeaturesView _featuresView;

        private Vector3 _currentPosition, _oldPosition;
        private Quaternion _currentRotation, _oldRotation;
        private Vector3 _currentScale, _baseScale;
        private float _lerpSpeed = 5f;

        private void Awake()
        {
            _featuresView = GetComponentInChildren<FeaturesView>();
            SetCard(new GameCard("Test" + Random.Range(0, 100), "Test descr" + Random.Range(0, 100),
                new[]{Features.Demonic,Features.Holy}));
            _canvasGroup = GetComponent<CanvasGroup>();
            _cardTargetLine = GetComponentInChildren<CardTargetLine>();
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

        public void SetCard(GameCard card)
        {
            _card = card;
            _featuresView.ShowFeatures(_card.Features);
        }

        public void SetTargetLineCard(Transform endTarget)
        {
            _cardTargetLine.ShowLineTarget(transform, endTarget);
        }

        public void HideLineTarget()
        {
            _cardTargetLine.HideLineTarget();
        }
        
        public void SetBasePosition(Vector3 pos, Quaternion rot){
            _currentPosition = _oldPosition = pos;
            _currentRotation = _oldRotation = rot;
        }

        public void SetCardState(DragState newDragState)
        {
            _dragState = newDragState;
        }

        public void OnBeginDrag(PointerEventData eventData){
            _canvasGroup.blocksRaycasts = false;
            SetCardState(DragState.Drag);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
            if (_dragState == DragState.Target)
            {
                _currentPosition = new Vector2(Screen.width * 0.9f, Screen.height * 0.5f);
                _currentRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                _currentScale = _baseScale/1.2f;
            }
            else
            {
                //_currentRotation = Quaternion.LookRotation(_rectTransform.forward, _rectTransform.position - _oldPosition);
                _currentRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                _currentScale = _baseScale/1.5f;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position+_rectTransform.sizeDelta/2f*_currentScale, eventData.pressEventCamera, out _currentPosition);
            }
            /*RectTransformUtility.ScreenPointToWorldPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _currentPosition);
            //Debug.Log(hand.GetComponent<RectTransform>().anchoredPosition.y);
            if(Mathf.Abs(_rectTransform.localPosition.y) < _handRectTransform.sizeDelta.y){
                _currentRotation = Quaternion.AngleAxis(0f, Vector3.forward);
                _currentScale = _baseScale;
            } else { 
                _currentRotation = Quaternion.LookRotation(_rectTransform.forward, _rectTransform.position - _oldPosition);
                _currentScale = _baseScale/2f;
            }*/
            
        }
        
        public void OnEndDrag(PointerEventData eventData){
            _canvasGroup.blocksRaycasts = true;
            _currentPosition = _oldPosition;
            _currentRotation = _oldRotation;
            _currentScale = _baseScale;
            SetCardState(DragState.Hand);
        }
    }
}