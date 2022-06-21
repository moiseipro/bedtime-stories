using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardManagment
{
    public class HandView : MonoBehaviour
    {
        public float offsetCardX = 1f, offsetCardY = 1f;
        public Vector3 offsetHandPos;

        public List<DraggableCard> cardArray = new List<DraggableCard>();
        public int maxCardCount = 6;
        public CardDrag curCardHand;

        private Vector3 _handPosition;
        private RectTransform _rectTransform;
        
        void Start()
        {
            _rectTransform = transform as RectTransform;

            cardArray = GetComponentsInChildren<DraggableCard>().ToList();
            CardPositionInitialize();
        }

        // Выровнять карты относительно руки
        public void CardPositionInitialize(){
            float cardXOffset, cardYOffset;
            var maxOffset = cardArray.Count/2-0.5f;
            float cardXDistance = _rectTransform.position.x/(float)maxCardCount*offsetCardX;
            float cardYDistance = Screen.height/(float)maxCardCount*offsetCardY;
            Debug.Log(cardXDistance);
            _handPosition = _rectTransform.position;
            for (var i = 0; i < cardArray.Count; i++)
            {
                if(cardArray.Count%2==0) cardXOffset = -maxOffset + i;
                else cardXOffset = -cardArray.Count/2 + i;

                cardYOffset = -Mathf.Abs(cardXOffset)+maxOffset;
                //Debug.Log(cardXOffset);

                Vector3 newCardPos = new Vector3(_handPosition.x + cardXOffset*cardXDistance, _handPosition.y +cardYOffset*cardYDistance, _handPosition.z)+ offsetHandPos*Screen.width/Screen.height;
                //Quaternion newCardRot = Quaternion.LookRotation(cardArray[i].transform.forward, currentCorner);
                Quaternion newCardRot = Quaternion.AngleAxis(-cardXOffset*10f, Vector3.forward);
                cardArray[i].SetCurPosition(newCardPos, newCardRot);
            }
        }

        private void FixedUpdate()
        {
            //CardPositionInitialize();
        }
    }
}