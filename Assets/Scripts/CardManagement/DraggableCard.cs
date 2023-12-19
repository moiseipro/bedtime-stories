using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityTemplateProjects;
using UnityTemplateProjects.UI;

namespace CardManagement
{
    public enum DragState
    {
        Drag,
        Hand,
        Placed,
        Target
    }

    public interface ICardDrag
    {
        public GameCard Card { get; }
        public void DestroyCard();
    }
    
    public class DraggableCard : MonoBehaviour, ICardDrag, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        private Transform _transform;
        private Collider _collider;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _cardSize;

        private Camera _camera;

        private DragState _dragState = DragState.Hand;
        [SerializeField] private GameCard _card;
        public GameCard Card => _card;
        private HandView _handView;

        private CardTargetLine _cardTargetLine;

        private Vector3 _currentPosition, _oldPosition;
        private Quaternion _currentRotation, _oldRotation;
        private Vector3 _currentScale, _baseScale;
        private float _lerpSpeed = 5f;

        private Vector3 _dragPosition;

        private void Awake()
        {
            _camera = Camera.main;
            _collider = GetComponent<Collider>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _cardSize = _spriteRenderer.bounds.size;
            Debug.Log(_cardSize);
            _cardTargetLine = GetComponentInChildren<CardTargetLine>();
            _transform = transform;
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
        
        public void Initialize(HandView handView, GameCard card)
        {
            _handView = handView;
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

        public Vector3 GetDragPosition(float distanceFromCamera)
        {
            return _camera.ScreenToWorldPoint((Vector3)_dragPosition + _camera.transform.forward * distanceFromCamera) 
                   + (Vector3)_cardSize / 2f * _currentScale.magnitude;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            
            if (_dragState == DragState.Target)
            {
                _currentScale = _baseScale*1.2f;
                
            }
            else
            {
                //_currentRotation = Quaternion.LookRotation(_rectTransform.forward, _rectTransform.position - _oldPosition);
                _currentScale = _baseScale * 1.1f;
                //Debug.Log(eventData.position+"|||"+_dragPosition);
                //_currentPosition = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position + _cardSize / 2f * _currentScale);
            }
            _dragPosition = _dragPosition = eventData.position;
        }
        
        public void OnEndDrag(PointerEventData eventData){
            _collider.enabled = true;
            _currentPosition = _oldPosition;
            _currentRotation = _oldRotation;
            _currentScale = _baseScale;
            SetCardState(DragState.Hand);
        }
        
        public void DestroyCard()
        {
            _handView.RemoveCard(this);
        }
    }
}