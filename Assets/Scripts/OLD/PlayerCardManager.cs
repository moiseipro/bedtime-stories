using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerCardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id = 0;
    public string playerName = "LOL";
    [SerializeField]private int horrorValue = 0;
    [SerializeField]private int maxReasonValue = 30;
    [SerializeField]private int reasonValue = 20;
    public int maxCardPlaced = 3;
    public List<Card> cardHero = new List<Card>();
    public List<Status> curStatuses = new List<Status>();

    public int curCardId = 0;
    float _timeLeft = 0.5f;
    public CardDrag placedCard;
    public PlayerHUD playerHud;
    //public List<int> cardIdsArray = new List<int>();
    public List<CardOptions3D> cardOptions3D = new List<CardOptions3D>();
    public GameObject prefab3DCard;

    private HandOptions handOpts;

    public bool enterCard = false;
    private float cardSize = 2.2f;
    private bool isCardOpening = false, timeStartStory = false;
    public Vector3 offsetCardSpawnPosition = Vector3.forward;
    public bool isNotPutCards = false;
    private bool isMove = false;
    private int curDanger = 0;

    [SerializeField]private Animator pl_Anim;
    
    void Awake()
    {
        handOpts = GameObject.Find("hand").GetComponent<HandOptions>();
        playerHud = GameObject.Find("PlayerHUD").GetComponent<PlayerHUD>();
        id = UnityEngine.Random.Range(0,10000);
        pl_Anim = GetComponent<Animator>();
        reasonValue = maxReasonValue;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse0)){
            if(placedCard){
                if(enterCard && cardOptions3D.Count < maxCardPlaced) {
                    PlaceCardOnPerson();
                } else {
                    placedCard.SetFocus(false);
                    placedCard = null;
                }
            }
        }
        
        // Движение персонажа
        if(isNotPutCards){
            if(pl_Anim.GetCurrentAnimatorStateInfo(0).IsTag("Move")){
                transform.Translate(Vector3.forward*Time.deltaTime, Space.World);
            }
        }

        // Таймер вызова активации карты
        if(isCardOpening){
            if (_timeLeft > 0){
                _timeLeft -= Time.deltaTime;
            } else {
                ActivateNextCard();
                isCardOpening = false;
            }
        }
        // Время перед стартом сюжета
        if(timeStartStory){
            if (_timeLeft > 0){
                _timeLeft -= Time.deltaTime;
            } else {
                StartNewStory();
            }
        }
    }

    // Активировать следующую неактивированную карту
    public void ActivateNextCard(){
        if(curCardId<cardOptions3D.Count){
            cardOptions3D[curCardId].ActivateCard(curDanger);
            _timeLeft = cardOptions3D[curCardId].timeCardActivated;
            curCardId++;
        } else isCardOpening = false;
    }

    // Инициализация времени активации карт
    public void ActivateCards(){
        //StartCoroutine(ActivateCardCoroutine());
        isCardOpening = true;
    }

    public bool CheckActivationCard(){
        return isCardOpening && curCardId<cardOptions3D.Count;
    }
    public bool CheckFinalActivationCard(){
        return !(curCardId<cardOptions3D.Count);
    }

    //Отчистить поле от использованных 3D карт
    public void ClearAll3DCard(){
        curCardId = 0;
        while(cardOptions3D.Count>0){
            CardOptions3D co3D = cardOptions3D[0];
            cardOptions3D.Remove(co3D);
            Destroy(co3D.gameObject);
        }
    }

    // Получить эффекты всех карт героя
    public void ActivateAllCardHero(){
        if(cardHero.Count>0){
            foreach (var item in cardHero)
            {
                
            }
        }
    }
    public void AddCardHero(Card newCardHero){
        cardHero.Add(newCardHero);
        ActivateAllCardHero();
    }

    // Изменение рассудка
    public void AddReason(int val){
        reasonValue += val;
    }
    // Изменение ужаса
    public void AddHorror(int val){
        horrorValue += val;
    }
    // Получение карт событий персонажем
    public void PlayerTakeCard(string type, int id){
        handOpts.AddCard(type, id);
    }
    public void PlayerTakeCard(int count, string type){
        handOpts.AddCard(count, type);
    }
    public void PlayerTakeCard(int count, string type, string tag){
        handOpts.AddCard(count, type, tag);
    }

    public void AddStatus(int id){
        curStatuses.Add(CardLoader.Instance.FindStatus(id));
        playerHud.UpdateAllStatuses();
    }
    
    public List<Status> GetAllStatuses(){
        return curStatuses;
    }

    // Активировать определенный typeAction
    public void ActivateAction(string actionId){
        string cardType = "";
        int pID;
        char typeShort = actionId[0];
        string objId = new string(actionId.Where((val, idx) => idx != 0).ToArray());
        bool isParced = int.TryParse(objId, out pID);
        if(isParced){
            Debug.Log("Проанализированный: "+pID);
            switch (typeShort){
                case 'e':
                    cardType = "events";
                    break;
                case 'a':
                    cardType = "actions";
                    break;
                case 'h':
                    cardType = "hero";
                    break;
                case 's':
                    cardType = "status";
                    break;
                default:
                    cardType = "actions";
                    break;
            }
            if(cardType != "status") PlayerTakeCard(cardType, pID);
            else AddStatus(pID);
        }
        
    }

    // Активация/деактивация движения игрока к следующему сюжету
    public void PlayerNextStory(bool isMove, int danger){
        pl_Anim.SetBool("MovePlayer", isMove);
        isNotPutCards = isMove;
        curDanger = danger;
    }

    public void StartNewStory(){
        pl_Anim.SetBool("MovePlayer", false);
        isNotPutCards = false;
        timeStartStory = false;
        _timeLeft = 0.5f;
    }

    public void SetNotPutCards(bool value){
        isNotPutCards = value;
    }

    // Активация/деактивация стадии построения героя
    public void PlayerToHeroStage(bool isHeroStage){
        pl_Anim.SetBool("HeroStage", isHeroStage);
    }

    public int GetPlayerReason(){
        return reasonValue;
    }
    public int GetPlayerHorror(){
        return horrorValue;
    }

    // Положить карту под персонажа
    public void PlaceCardOnPerson(){
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position-offsetCardSpawnPosition-Vector3.forward*cardSize*cardOptions3D.Count+Vector3.up, transform.TransformDirection(-Vector3.up), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position-offsetCardSpawnPosition-Vector3.forward*cardSize*cardOptions3D.Count, transform.TransformDirection(-Vector3.up) * hit.distance, Color.blue);
            GameObject new3DCard = Instantiate(prefab3DCard, hit.point, Quaternion.LookRotation(Vector3.forward, -hit.normal));
            CardOptions3D newCard3DObject = new3DCard.GetComponent<CardOptions3D>();
            newCard3DObject.card = placedCard.GetComponent<CardOptions>().card;
            newCard3DObject.typeCard = placedCard.GetComponent<CardOptions>().typeCard;
            newCard3DObject.SetTargetPlayer(gameObject.GetComponent<PlayerCardManager>());

            cardOptions3D.Add(new3DCard.GetComponent<CardOptions3D>());
            handOpts.PlaceCard(placedCard);
            
            //Destroy(placedCard.gameObject);
        }
        else
        {
            Debug.DrawRay(transform.position-offsetCardSpawnPosition-Vector3.forward*cardSize*cardOptions3D.Count+Vector3.up, transform.TransformDirection(-Vector3.up) * 1000, Color.red, 5f);
            Debug.Log("Did not Hit");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Story"){
            timeStartStory = true;
            _timeLeft = UnityEngine.Random.Range(0.5f, 1f);
            Debug.Log("Начало истории");
        }
    }

    public void OnPointerEnter(PointerEventData eventData){
        placedCard = handOpts.GetCurCard();
        //Debug.Log("test123");
        if(placedCard!=null && !isNotPutCards && cardOptions3D.Count < maxCardPlaced && (placedCard.GetComponent<CardOptions>().typeCard == "events" || placedCard.GetComponent<CardOptions>().typeCard == "actions" || placedCard.GetComponent<CardOptions>().typeCard == "hero")){
            enterCard = true;
            if(placedCard){
                placedCard.FocusedPlayer(Camera.main.WorldToScreenPoint(transform.position));
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        enterCard = false;
        if(placedCard){
            placedCard.SetFocus(false);
        }
    }

    private void OnMouseUp() {
        playerHud.SetTargetPlayer(transform);
    }

    private void OnMouseOver() {
        //playerHud.SetHUDText(playerName, reasonValue.ToString(), horrorValue.ToString());
    }
}
