using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GameHUD : MonoBehaviour
{
    TMP_Text stroyDangerText;

    // Start is called before the first frame update
    void Start(){
        stroyDangerText = GameObject.Find("StoryDangerText").GetComponent<TMP_Text>();
    }

    public void SetStoryDangerText(string newText){
        stroyDangerText.text = newText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
