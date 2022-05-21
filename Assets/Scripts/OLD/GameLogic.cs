using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{

    public List<PlayerCardManager> playerOptions = new List<PlayerCardManager>();
    public List<Card> storyCard = new List<Card>();
    public Card curStoryCard = null;
    private CardOptions cardOptions;
    private GameHUD gameHUD;
    public HandOptions handOptions;
    public Vector3 offsetCameraPosition;
    public Vector3 offsetCameraHeroPosition;
    [SerializeField]private float cameraSpeed = 5f, cameraRotationSpeed = 10f;
    public int allStorySpawned = 0;
    [SerializeField]private int stepCount = 0;
    [SerializeField]private int storyDanger = 5;
    [SerializeField]private int chCardGive = 2;
    public string stage = "hero";
    public bool isStartTurn = false, isNextStageTimer = false, isMoveNextStory = false;
    float _timeLeft = 0.5f;

    //Поиск всех персонажей, которые не добавлены в массив
    public void SearchAllPlayers(){
        PlayerCardManager[] searchedPlayerOptions = GameObject.FindObjectsOfType<PlayerCardManager>();
        foreach (var po in searchedPlayerOptions){
            PlayerCardManager newPO = playerOptions.Find(item => item.id==po.id);
            if(newPO == null){
                playerOptions.Add(po);
            }
        }
        Debug.Log("Кол-во персонажей: "+playerOptions.Count);
    }

    public void CompleteTurn(){
        isStartTurn = true;
        if(playerOptions.Count>0){
            foreach (var item in playerOptions)
            {
                item.SetNotPutCards(true);
            }
        }
    }

    //Следующий ход вскрытия карт
    public void NextCardsStep(){
        if(playerOptions.Count>0){
            if(!playerOptions[stepCount].CheckFinalActivationCard()){
                playerOptions[stepCount].ActivateCards();
            }
            if(playerOptions.Count-1<=stepCount) stepCount = 0;
            else stepCount++;
        }
    }

    public bool CheckPlayersActivationCard(){
        bool isActive = false;
        int finalActivation = 0;
        if(playerOptions.Count>0){
            foreach (var po in playerOptions){
                if(po.CheckActivationCard()) isActive = true;
                if(po.CheckFinalActivationCard()) finalActivation++;
            }
            if(finalActivation==playerOptions.Count) {
                isStartTurn = false;
                // Включить таймер следующей стадии
                _timeLeft = 2f;
                isNextStageTimer = true;
                Debug.Log("Ходы завершены");
            }
        }
        return isActive;
    }

    //Получить в количестве случайных карт истории
    public void GetRandomStoryCard(int cardCount){
        for (int i = 0; i < cardCount; i++)
        {
            storyCard.Add(CardLoader.Instance.FindCard(UnityEngine.Random.Range(0,CardLoader.Instance.GetCountCards("story")), "story"));
        }
    }

    // Переключить на следующую карту истории
    public void SelectNextStoryCard(float time){
        if(!isMoveNextStory && storyCard.Count > 0){
            curStoryCard = storyCard[0];
            storyCard.Remove(curStoryCard);
            storyDanger = curStoryCard.dangerValue[0];

            allStorySpawned++;
            SpawnNextStoryPrefab();

            _timeLeft = time;
            isMoveNextStory = true;
            AllPlayersMoveNextStory(isMoveNextStory);
            gameHUD.SetStoryDangerText(storyDanger.ToString());
        }
    }

    public void SpawnNextStoryPrefab(){
        Instantiate(Resources.Load<GameObject>("Prefabs/Story/story_"+curStoryCard.id), new Vector3(0f,0f,30f*allStorySpawned), Quaternion.identity);

    }

    // Переключить всех персонажей в движение
    public void AllPlayersMoveNextStory(bool isMove){
        if(playerOptions.Count>0){
            foreach (var po in playerOptions){
                po.PlayerNextStory(isMove, storyDanger);
            }
        }
    }

    // Установить новую стадию игры по названию
    public void SetStage(string newStage){
        Debug.Log("Стадия: "+newStage);
        stage = newStage;
        if(playerOptions.Count>0){
            foreach (var item in playerOptions)
            {
                item.SetNotPutCards(false);
                item.ClearAll3DCard();
                if(newStage == "hero") {
                    item.PlayerToHeroStage(true);
                    item.PlayerTakeCard(chCardGive, newStage);
                } else item.PlayerToHeroStage(false);
                if(newStage == "actions"){
                    item.PlayerTakeCard(chCardGive, newStage, curStoryCard.tag);
                }
            }
            if(newStage == "move"){
                SelectNextStoryCard(UnityEngine.Random.Range(5,8));
            }
        }
    }

    // Взять 1 новую карту
    public void TakeNewCard(){
        handOptions.AddCard(1, stage, curStoryCard.tag);
    }

    public void StartGame(){
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        handOptions = GameObject.FindObjectOfType<HandOptions>();
        cardOptions = GameObject.FindWithTag("CardGUI").GetComponent<CardOptions>();
        //handOptions.AddCard(3, stage);
        GetRandomStoryCard(5);
        SearchAllPlayers();
        SetStage("hero");
        //SelectNextStoryCard();
    }

    public void StartNewStage(){
        switch (stage){
            case "hero":
                SetStage("move");
                break;
            case "actions":
                SetStage("events");
                break;
            case "events":
                SetStage("move");
                break;
            default:
                SetStage("actions");
                break;
        }
        
        
    }

    // Start is called before the first frame update
    void Start(){
        StartGame();
    }

    // Update is called once per frame
    void Update(){
        if(playerOptions.Count>0){
            float posX = 0, posZ = 0, posfX = 0, posfZ = 0;
            foreach (var po in playerOptions){
                posX += po.transform.position.x;
                posZ += po.transform.position.z;
                posfX += po.transform.position.x+po.transform.forward.x*offsetCameraHeroPosition.x;
                posfZ += po.transform.position.z+po.transform.forward.z*offsetCameraHeroPosition.z;
            }
            posX/=playerOptions.Count; posZ/=playerOptions.Count;
            posfX/=playerOptions.Count; posfZ/=playerOptions.Count;
            Vector3 newCameraPos = new Vector3(posX, 0f, posZ);
            Vector3 newCameraPosForward = new Vector3(posfX, offsetCameraHeroPosition.y, posfZ);
            Quaternion rotation = Quaternion.LookRotation(newCameraPos - Camera.main.transform.position);
			Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, rotation, Time.deltaTime * cameraRotationSpeed);
            if(stage=="hero"){
                //Camera.main.transform.LookAt(newCameraPos);
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPosForward, Time.deltaTime * cameraSpeed);
            } else {
                //Camera.main.transform.LookAt(newCameraPos);
                newCameraPos += offsetCameraPosition;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newCameraPos, Time.deltaTime * cameraSpeed);
            }

            // Таймер вызова следующего сюжета
            if(isMoveNextStory){
                isMoveNextStory = false;
                foreach (var po in playerOptions){
                    if(po.isNotPutCards) isMoveNextStory = true;
                }
                if(isMoveNextStory){
                    cardOptions.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, -500, 0);
                } else {
                    cardOptions.InisializationCard(curStoryCard.id, "story");
                    cardOptions.GetComponent<RectTransform>().anchoredPosition = new Vector3(230, -360, 0);
                    StartNewStage();
                }
                /*if (_timeLeft > 0){
                    _timeLeft -= Time.deltaTime;
                    cardOptions.GetComponent<RectTransform>().anchoredPosition = new Vector3(-500, -500, 0);
                } else {
                    isMoveNextStory = false;
                    AllPlayersMoveNextStory(isMoveNextStory);
                    cardOptions.InisializationCard(curStoryCard.id, "story");
                    cardOptions.GetComponent<RectTransform>().anchoredPosition = new Vector3(230, -360, 0);
                    StartNewStage();
                }*/
            }
            if(isNextStageTimer){
                if (_timeLeft > 0){
                    _timeLeft -= Time.deltaTime;
                } else {
                    isNextStageTimer = false;
                    StartNewStage();
                }
            }

            
        }

        // Проверка для вызова следующего хода встрытия карт
        if(isStartTurn && !CheckPlayersActivationCard()){
            NextCardsStep();
        }
    }
}
