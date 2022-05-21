using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLoader : MonoBehaviour
{
    public static CardLoader Instance { get; private set; }
    public GetCardsList cardTypesList = new GetCardsList();
    public GetStatusesList statusesList = new GetStatusesList();

    private int eventsCardCount, actionsCardCount, heroCardCount, storyCardCount;

    public void Awake()
	{
		Instance = this;
        DontDestroyOnLoad(this);
        LoadCardFromJson();
        LoadStatusFromJson();
	}

    //Выгрузить карты из файла json
    public void LoadCardFromJson(){
        string jsonCardValuesFile = Resources.Load<TextAsset>("Cards/baseCards").ToString();
        Debug.Log(jsonCardValuesFile);
        cardTypesList = JsonUtility.FromJson<GetCardsList>(jsonCardValuesFile);
        eventsCardCount = cardTypesList.events.Count;
        actionsCardCount = cardTypesList.actions.Count;
        heroCardCount = cardTypesList.hero.Count;
        storyCardCount = cardTypesList.story.Count;
    }

    //Выгрузить статусы из файла json
    public void LoadStatusFromJson(){
        string jsonStatusValuesFile = Resources.Load<TextAsset>("Cards/baseStatuses").ToString();
        Debug.Log(jsonStatusValuesFile);
        statusesList = JsonUtility.FromJson<GetStatusesList>(jsonStatusValuesFile);
    }

    public Status FindStatus(int id){
        return statusesList.statuses.Find(item => item.id==id);
    }
    public Status FindStatus(string tag){
        List<Status> allStatuses = new List<Status>();
        allStatuses = statusesList.statuses.FindAll(item => item.tag==tag);
        return allStatuses[Random.Range(0, allStatuses.Count)];
    }

    public int GetCountCards(string type){
        int returnCount = 0;
        switch (type){
            case "hero":
                returnCount = heroCardCount;
                break;
            case "story":
                returnCount = storyCardCount;
                break;
            case "actions":
                returnCount = actionsCardCount;
                break;
            case "events":
                returnCount = eventsCardCount;
                break;
            default:
                break;
        }
        return returnCount;
    }

    public Card FindCard(int id, int type){
        Card getCard = new Card();
        switch (type){ 
            case 1:
                getCard = cardTypesList.hero.Find(item => item.id==id);
                break;
            case 2:
                getCard = cardTypesList.story.Find(item => item.id==id);
                break;
            case 3:
                getCard = cardTypesList.actions.Find(item => item.id==id);
                break;
            case 4:
                getCard = cardTypesList.events.Find(item => item.id==id);
                break;
            default:
                break;
        }
        return getCard;
    }
    public Card FindCard(int id, string type){
        Card getCard = new Card();
        switch (type){
            case "hero":
                getCard = cardTypesList.hero.Find(item => item.id==id);
                break;
            case "story":
                getCard = cardTypesList.story.Find(item => item.id==id);
                break;
            case "actions":
                getCard = cardTypesList.actions.Find(item => item.id==id);
                break;
            case "events":
                getCard = cardTypesList.events.Find(item => item.id==id);
                break;
            default:
                break;
        }
        return getCard;
    }
    public Card FindCard(string tag, string type){
        List<Card> allCard = new List<Card>();
        Card getCard = new Card();
        switch (type){
            case "hero":
                allCard = cardTypesList.hero.FindAll(item => item.tag==tag || item.tag=="");
                break;
            case "story":
                allCard = cardTypesList.story.FindAll(item => item.tag==tag || item.tag=="");
                break;
            case "actions":
                allCard = cardTypesList.actions.FindAll(item => item.tag==tag || item.tag=="");
                break;
            case "events":
                allCard = cardTypesList.events.FindAll(item => item.tag==tag || item.tag=="");
                break;
            default:
                break;
        }
        getCard = allCard[Random.Range(0, allCard.Count)];
        return getCard;
    }
    
}
