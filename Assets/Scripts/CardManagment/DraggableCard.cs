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
        private Transform _transform, _handTransform;
        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _cardSize;

        private DragState _dragState = DragState.Hand;
        private GameCard _card;
        public GameCard Card => _card;
        private CardTargetLine _cardTargetLine;
        //private FeaturesView _featuresView;

        private Vector3 _currentPosition, _oldPosition;
        private Quaternion _currentRotation, _oldRotation;
        private Vector3 _currentScale, _baseScale;
        private float _lerpSpeed = 5f;

        private Vector3 _dragPosition;
        public Vector3 DragPosition => _dragPosition;

        private void Awake()
        {
            //_featuresView = GetComponentInChildren<FeaturesView>();
            SetCard(new GameCard("Test" + Random.Range(0, 100), "Test descr" + Random.Range(0, 100),
                new[]{Features.Demonic,Features.Holy}));
            _collider = GetComponent<Collider>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _cardSize = _spriteRenderer.bounds.size;
            Debug.Log(_cardSize);
            _cardTargetLine = GetComponentInChildren<CardTargetLine>();
            _transform = transform;
            _handTransform = _transform.parent.transform;
            _currentScale = _baseScale = _transform.localScale;
        }

        private void FixedUpdate()
        {
            _transform.position = Vector3.Lerp(_transform.position, _currentPosition, Time.deltaTime * _lerpSpeed);
            _transform.rotation = Quaternion.Lerp(_transform.rotation, _currentRotation, Time.deltaTime * _lerpSpeed);
            _transform.localScale = Vector3.Lerp(_transform.localScale, _currentScale, Time.deltaTime * _lerpSpeed);
        }

        public bool IsColliderActive()
        {
            return _collider.enabled;
        }
        
        public void SetCard(GameCard card)
        {
            _card = card;
            //_featuresView.ShowFeatures(_card.Features);
        }

        public void SetTargetLineCard(Transform endTarget)
        {
            _cardTargetLine.ShowLineTarget(_transform, endTarget);
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
            _collider.enabled = false;
            SetCardState(DragState.Drag);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
            if (_dragState == DragState.Target)
            {
                _currentScale = _baseScale/1.2f;
                _dragPosition =
                    eventData.pressEventCamera.ScreenToWorldPoint((Vector3)eventData.position + eventData.pressEventCamera.transform.forward*15f) 
                    + (Vector3)_cardSize / 2f * _currentScale.magnitude;
            }
            else
            {
                //_currentRotation = Quaternion.LookRotation(_rectTransform.forward, _rectTransform.position - _oldPosition);
                _currentScale = _baseScale/1.5f;
                _dragPosition =
                    eventData.pressEventCamera.ScreenToWorldPoint((Vector3)eventData.position + eventData.pressEventCamera.transform.forward*15f) 
                    + (Vector3)_cardSize / 2f * _currentScale.magnitude;
                //Debug.Log(eventData.position+"|||"+_dragPosition);
                //_currentPosition = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position + _cardSize / 2f * _currentScale);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData){
            _collider.enabled = true;
            _currentPosition = _oldPosition;
            _currentRotation = _oldRotation;
            _currentScale = _baseScale;
            SetCardState(DragState.Hand);
        }
    }
}