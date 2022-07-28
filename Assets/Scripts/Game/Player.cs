using System;
using System.Collections.Generic;
using CardManagment;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.UI;

namespace UnityTemplateProjects.Game
{
    public interface IPlayer
    {
        
        void TakeDamage(int value);
        void TakeHorror(int value);
    }
    public class Player : MonoBehaviour, IPlayer, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]private FeaturesView _featuresView;
        
        private PlayerStats _playerStats = new PlayerStats();
        private PlayerCards _playerCards = new PlayerCards();
        private FeaturesHandler _featuresHandler = new FeaturesHandler();

        private void Awake()
        {
            Debug.Log(_playerStats.Reason);
        }

        public void TakeDamage(int value)
        {
            throw new NotImplementedException();
        }

        public void TakeHorror(int value)
        {
            throw new NotImplementedException();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                DraggableCard draggableCard = eventData.pointerDrag.GetComponent<DraggableCard>();
                _featuresHandler.AddFeatures(draggableCard.Card.Features);
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

    public class PlayerStats
    {
        private int _reason;
        public int Reason => _reason;
        private int _horror;
        public int Horror => _horror;

        private List<Features> _featuresPlayer = new List<Features>();

        public PlayerStats()
        {
            _reason = 5;
            _horror = 0;
        }
        
        public void AddHealth(int value)
        {
            _reason += value;
            Debug.Log("Получено здоровье: " + value);
        }

        public void ReduceHealth(int value)
        {
            _reason -= value;
            Debug.Log("Забрано здоровье: " + value);
        }

        public void AddHorror(int value)
        {
            _horror += value;
            Debug.Log("Получено ужаса: " + value);
        }
    }

    public class PlayerCards
    {
        private List<ActionCard> _actionCards = new List<ActionCard>();

        private List<Card> _cardInHand = new List<Card>();
    }
    
}