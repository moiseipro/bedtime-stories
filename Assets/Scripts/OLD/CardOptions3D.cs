using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardOptions3D : CardOptions
{

    public bool cardShirt = true;
    public int speedRotationShirt = 10;
    public float timeCardActivated = 1f;
    public PlayerCardManager targetPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Установить персонажа, к которому относиться карта
    public void SetTargetPlayer(PlayerCardManager po){
        targetPlayer = po;
    }

    // Активировать базовый эффект карты
    public void ActivateCard(int danger){
        int cardChallenge = 0;
        if(card.dangerValue[1]<=danger) cardChallenge = 1;
        else if(card.dangerValue[0]<=danger) cardChallenge = 0;
        
        string[] actions = new string[0];

        if(card.typeAction[cardChallenge] != ""){
            //int[] actions = card.typeAction[cardChallenge].Split( new[]{","}, StringSplitOptions.RemoveEmptyEntries ).Select(x => int.Parse(x)).ToArray();
            actions = card.typeAction[cardChallenge].Split( new[]{","}, StringSplitOptions.RemoveEmptyEntries );
            foreach (var item in actions)
            {
                Debug.Log(item+"||");
            }
            
        }
        if(cardShirt){
            cardShirt = false;
            
            switch (typeCard){
                case "actions":
                    if(actions.Length>0) targetPlayer.ActivateAction(actions[UnityEngine.Random.Range(0, actions.Length)]);
                    targetPlayer.AddHorror(card.horrorValue);
                    break;
                case "events":
                    targetPlayer.AddReason(card.reasonValue);
                    targetPlayer.AddHorror(card.horrorValue);
                    if(actions.Length>0){
                        foreach (var action in actions)
                        {
                            targetPlayer.ActivateAction(action);
                        }
                    }
                    break;
                case "story":
                    break;
                case "hero":
                    targetPlayer.AddCardHero(card);
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!cardShirt){
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(0 , Vector3.forward), Time.deltaTime * speedRotationShirt);
        }
    }
}
