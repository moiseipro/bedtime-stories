using System.Collections.Generic;
using System.Linq;
using Game;
using Unity.Netcode;
using UnityEngine;

namespace CardManagement
{
    public class HandView : MonoBehaviour
    {
        [SerializeField] private DraggableCard draggableCardPrefab;
        public float offsetCardX = 1f, offsetCardY = 1f;
        public float distanceFromCamera = 10f;
        public float distanceUpCamera = -3f;

        public List<DraggableCard> cardArray = new List<DraggableCard>();
        public int maxCardCount = 6;

        private Vector3 _handPosition;
        private Transform _cameraTransform;

        private Camera _camera;
        private PlayerStats _playerStats;
        
        void Start()
        {
            _camera = Camera.main;
            _cameraTransform = _camera.transform;

            cardArray = GetComponentsInChildren<DraggableCard>().ToList();
        }

        public void Initialize()
        {
            
        }
        
        public void TakeCard(GameCard gameCard)
        {
            DraggableCard newCard = Instantiate(draggableCardPrefab, transform);
            newCard.Initialize(this, gameCard);
            cardArray.Add(newCard);
        }
        
        // Выровнять карты относительно руки
        private void CardPositionUpdate(){
            //if(!IsOwner) return;
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
                    
                    newCardPos = _cameraTransform.position + 
                                 _cameraTransform.forward * distanceFromCamera +
                                 _cameraTransform.up * distanceUpCamera +
                                 _cameraTransform.up * cardYOffset * cardYDistance +
                                 _cameraTransform.right * cardXOffset * cardXDistance;
                    newCardRot = Quaternion.LookRotation(newCardPos - _handPosition, Vector3.up);
                }
                else
                {
                    newCardPos = cardArray[i].GetDragPosition(distanceFromCamera);
                    //Debug.Log(newCardPos);
                    newCardRot = Quaternion.LookRotation(newCardPos - _handPosition, Vector3.up);
                }
                
                cardArray[i].SetBasePosition(newCardPos, newCardRot);
            }
        }

        public void RemoveCard(DraggableCard draggableCard)
        {
            cardArray.Remove(draggableCard);
            Destroy(draggableCard.gameObject);
        }
        
        private void Update()
        {
            CardPositionUpdate();
        }
    }
}