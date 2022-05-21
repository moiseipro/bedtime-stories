using System;
using System.Diagnostics.Contracts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardOptions : MonoBehaviour
{
    //public int cardId = 0;
    public string typeCard = "";
    public TMP_Text horrorText, reasonText, nameText, danger1Text, danger2Text, description1Text, description2Text;

    //Информация о карте, описание, имя и другие параметры с переводом
    public Card card = new Card();

    // Инициализация случайной карты
    public void InisializationCard(string type){
        typeCard = type;
        card = CardLoader.Instance.FindCard(GetRamdomCardId(), type);
        SetCardInformationUI();
    }

    // Инициализация карты по id
    public void InisializationCard(int id,string type){
        typeCard = type;
        card = CardLoader.Instance.FindCard(id, type);
        SetCardInformationUI();
    }

    // Инициализация карты по тегу
    public void InisializationCard(string tag,string type){
        typeCard = type;
        card = CardLoader.Instance.FindCard(tag, type);
        SetCardInformationUI();
        //if(card.typeAction[0].Length>0) Debug.Log("Есть экшоны");
        //if(card.typeAction[1].Length>0) Debug.Log("Есть экшоны");
    }

    public void SetCardInformationUI(){
        horrorText.text = card.horrorValue.ToString();
        if(card.reasonValue == 0) reasonText.transform.parent.parent.gameObject.SetActive(false);
        else reasonText.text = card.reasonValue < 0 ? card.reasonValue.ToString() : "+"+card.reasonValue.ToString();
        if(card.dangerValue[0] == 0) danger1Text.transform.parent.parent.gameObject.SetActive(false);
        else {
            danger1Text.transform.parent.parent.gameObject.SetActive(true);
            danger1Text.text = card.dangerValue[0].ToString();
        }
        if(card.dangerValue[1] == 0) danger2Text.transform.parent.parent.gameObject.SetActive(false);
        else {
            danger2Text.transform.parent.parent.gameObject.SetActive(true);
            danger2Text.text = card.dangerValue[1].ToString();
        }
        nameText.text = card.name;
        description1Text.text = card.description[0].ToString();
        description2Text.text = card.description[1].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetCardId(){
        return card.id;
    }
    public int GetRamdomCardId(){
        return UnityEngine.Random.Range(0,CardLoader.Instance.GetCountCards(typeCard));
    }
    public void LoadCardValuesById(){

    }
}
