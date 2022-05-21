using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerHUD : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    TMP_Text nameText, reasonText, horrorText;
    private Transform targetPlayer;
    public StatusIcon[] statusIcons;
    private PlayerCardManager plManager;

    bool enteredHUD = false;

    private void Start() {
        nameText = GameObject.Find("NamePlayerText").GetComponent<TMP_Text>();
        reasonText = GameObject.Find("ReasonPlayerText").GetComponent<TMP_Text>();
        horrorText = GameObject.Find("HorrorPlayerText").GetComponent<TMP_Text>();
        statusIcons = GameObject.FindObjectsOfType<StatusIcon>();
    }

    private void FixedUpdate() {
        if(targetPlayer!=null) {
            SetHUDPosition();
        }
        if(plManager!=null) SetHUDText();
    }

    public void SetHUDText(){
        nameText.text = plManager.playerName;
        reasonText.text = plManager.GetPlayerReason().ToString();
        horrorText.text = plManager.GetPlayerHorror().ToString();
    }

    public void UpdateAllStatuses(){
        if(targetPlayer != null){
            List<Status> statuses = plManager.GetAllStatuses();
            int i = 0;
            for (; i < statusIcons.Length && i < statuses.Count; i++){
                statusIcons[i].SetNewIcon(statuses[i].id);
            }
            for (; i < statusIcons.Length; i++){
                statusIcons[i].SetNewIcon(-1);
            }
        }
    }

    public void SetTargetPlayer(Transform newTarget){
        if(newTarget == null || targetPlayer == newTarget) {
            gameObject.SetActive(false);
            targetPlayer = null; plManager = null;
        }
        else {
            targetPlayer = newTarget;
            plManager = newTarget.GetComponent<PlayerCardManager>();
            SetHUDPosition();
            SetHUDText();
            UpdateAllStatuses();
            gameObject.SetActive(true);
            
        }
        
        
    }

    public void SetHUDPosition(){
        Vector3 newHUDpos = Camera.main.WorldToScreenPoint(targetPlayer.position);
        newHUDpos.y -= GetComponent<RectTransform>().sizeDelta.y/3f;
        newHUDpos.x -= GetComponent<RectTransform>().sizeDelta.x;
        transform.position = newHUDpos;
        
    }

    public void OnPointerEnter(PointerEventData eventData){
        enteredHUD = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        enteredHUD = false;
    }
}
