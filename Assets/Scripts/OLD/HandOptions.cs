using Mirror;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOptions : MonoBehaviour
{
    public float offsetCardX = 1f, offsetCardY = 1f;
    public Vector3 offsetHandPos;

    public List<CardDrag> cardArray = new List<CardDrag>();
    public int maxCardCount = 6;
    public CardDrag curCardHand;

    private Vector3 handPosition;
    private RectTransform rect_tr;
    
    void Start()
    {
        rect_tr = transform as RectTransform;

        cardArray = GetComponentsInChildren<CardDrag>().ToList();
        CardPositionInitialize();
    }

    // Выровнять карты относительно руки
    public void CardPositionInitialize(){
        float cardXOffset, cardYOffset;
        var maxOffset = cardArray.Count/2-0.5f;
        float cardXDistance = rect_tr.position.x/(float)maxCardCount*offsetCardX;
        float cardYDistance = Screen.height/(float)maxCardCount*offsetCardY;
        Debug.Log(cardXDistance);
        handPosition = rect_tr.position;
        for (var i = 0; i < cardArray.Count; i++)
        {
            if(cardArray.Count%2==0) cardXOffset = -maxOffset + i;
            else cardXOffset = -cardArray.Count/2 + i;
            for (float j = cardXOffset; j < cardXOffset*-1f; j++)
            {
                
            }
            cardYOffset = -Mathf.Abs(cardXOffset)+maxOffset;
            //Debug.Log(cardXOffset);

            Vector3 newCardPos = new Vector3(handPosition.x + cardXOffset*cardXDistance, handPosition.y +cardYOffset*cardYDistance, handPosition.z)+offsetHandPos;
            //Quaternion newCardRot = Quaternion.LookRotation(cardArray[i].transform.forward, currentCorner);
            Quaternion newCardRot = Quaternion.AngleAxis(-cardXOffset*10f, Vector3.forward);
            cardArray[i].SetCurPosition(newCardPos, newCardRot);
        }
    }

    public void AddCard(string typeCard, int id){
        if(cardArray.Count > maxCardCount){
            CardPositionInitialize();
            return;
        }
        GameObject instance = Instantiate(Resources.Load("Prefabs/Card", typeof(GameObject)), rect_tr) as GameObject;
        CardOptions instCardOptions = instance.GetComponent<CardOptions>();
        instCardOptions.InisializationCard(id, typeCard);
        cardArray.Add(instance.GetComponent<CardDrag>());
        CardPositionInitialize();
    }

    // Добавить карты в руку с меткой типа карты
    public void AddCard(int cardCount, string typeCard){
        for (int i = 0; i < cardCount; i++){
            if(cardArray.Count > maxCardCount){
                CardPositionInitialize();
                return;
            }
            GameObject instance = Instantiate(Resources.Load("Prefabs/Card", typeof(GameObject)), rect_tr) as GameObject;
            //instance.transform.SetParent(rect_tr);
            CardOptions instCardOptions = instance.GetComponent<CardOptions>();
            instCardOptions.InisializationCard(typeCard);
            cardArray.Add(instance.GetComponent<CardDrag>());
            CardPositionInitialize();
        }
        
    }
    public void AddCard(int cardCount, string typeCard, string tag){
        for (int i = 0; i < cardCount; i++){
            if(cardArray.Count > maxCardCount){
                CardPositionInitialize();
                return;
            }
            GameObject instance = Instantiate(Resources.Load("Prefabs/Card", typeof(GameObject)), rect_tr) as GameObject;
            //instance.transform.SetParent(rect_tr);
            CardOptions instCardOptions = instance.GetComponent<CardOptions>();
            instCardOptions.InisializationCard(tag, typeCard);
            cardArray.Add(instance.GetComponent<CardDrag>());
            CardPositionInitialize();
        }
        
    }
    
    //Убрать все карты из руки
    public void CardsToStack(){
        while(cardArray.Count>0){
            CardDrag cd = cardArray[0];
            cardArray.Remove(cd);
            Destroy(cd.gameObject);
        }
        CardPositionInitialize();
    }

    // Убрать карту из руки и уничтожить
    public void PlaceCard(CardDrag card){
        if(card) cardArray.Remove(card);
        CardPositionInitialize();
        Destroy(card.gameObject);
    }

    public CardDrag GetCurCard(){
        return curCardHand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
