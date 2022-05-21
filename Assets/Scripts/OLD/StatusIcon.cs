using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    private Image icon;

    private void Awake() {
        icon = gameObject.GetComponent<Image>();
    }

    public void SetNewIcon(int idIcon){
        icon.sprite = Resources.Load<Sprite>("StatusesIcons/status_"+idIcon);
    }
}
