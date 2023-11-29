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
        private Transform _transform;

        private Camera _camera;
        
        void Start()
        {
            _transform = transform;
            _camera = Camera.main;

            cardArray = GetComponentsInChildren<DraggableCard>().ToList();
            CardPositionInitialize();
        }

        // Выровнять карты относительно руки
        public void CardPositionInitialize(){
            float cardXOffset, cardYOffset;
            var maxOffset = cardArray.Count/2-0.5f;
            _handPosition = _camera.transform.position;
            float cardXDistance = (float)maxCardCount*offsetCardX;
            float cardYDistance = (float)maxCardCount*offsetCardY;
            //Debug.Log(cardXDistance);
            for (var i = 0; i < cardArray.Count; i++)
            {
                Vector3 newCardPos = Vector3.zero;
                Quaternion newCardRot = Quaternion.identity;
                if (cardArray[i].IsColliderActive())
                {
                    if(cardArray.Count%2==0) cardXOffset = -maxOffset + i;
                    else cardXOffset = -cardArray.Count/2 + i;

                    cardYOffset = -Mathf.Abs(cardXOffset)+maxOffset;
                    //Debug.Log(cardXOffset);

                    newCardPos = new Vector3(
                        _handPosition.x + cardXOffset * cardXDistance, 
                        _handPosition.y + cardYOffset * cardYDistance, 
                        _handPosition.z) + offsetHandPos * Screen.width / Screen.height;
                    //Quaternion newCardRot = Quaternion.LookRotation(cardArray[i].transform.forward, currentCorner);
                    //Quaternion newCardRot = Quaternion.AngleAxis(-cardXOffset*10f, Vector3.forward);
                    newCardRot = Quaternion.LookRotation(newCardPos - _handPosition, Vector3.up);
                }
                else
                {
                    newCardPos = cardArray[i].DragPosition;
                    //Debug.Log(newCardPos);
                    newCardRot = Quaternion.LookRotation(newCardPos - _handPosition, Vector3.up);
                }
                
                cardArray[i].SetBasePosition(newCardPos, newCardRot);
            }
        }

        private void FixedUpdate()
        {
            CardPositionInitialize();
        }
    }
}