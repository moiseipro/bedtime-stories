using CardManagement;
using CardManagement.CardFeatures;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.UI;

namespace Game
{
    public class Enemy: MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        private FeaturesHandler _featuresHandler = new FeaturesHandler();
        [SerializeField]private FeaturesView _featuresView;

        private void Awake()
        {
            
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                DraggableCard draggableCard = eventData.pointerDrag.GetComponent<DraggableCard>();
                //_featuresHandler.AddFeatures(draggableCard.Card.Features);
                Debug.Log ("Dropped object was: "  + eventData.pointerDrag);
                Destroy(eventData.pointerDrag);
                _featuresView.ShowFeatures(_featuresHandler.GetFeatures());
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var dragCard = eventData.pointerDrag.GetComponent<DraggableCard>();
                dragCard.SetCardState(DragState.Target);
                dragCard.SetTargetLineCard(transform);
                //FindObjectOfType<CardTargetLine>().ShowLineTarget(eventData.pointerDrag.transform, transform);
                Debug.Log ("Enter object was: "  + eventData.pointerDrag);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var dragCard = eventData.pointerDrag.GetComponent<DraggableCard>();
                dragCard.SetCardState(DragState.Drag);
                dragCard.HideLineTarget();
                //FindObjectOfType<CardTargetLine>().HideLineTarget();
                Debug.Log ("Exit object was: "  + eventData.pointerDrag);
            }
        }
    }
}