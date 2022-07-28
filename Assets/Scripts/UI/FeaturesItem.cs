using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityTemplateProjects.UI
{
    [RequireComponent(typeof(Image))]
    public class FeaturesItem : MonoBehaviour
    {
        private Image _image;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void ShowIconFeature(Features feature)
        {
            _image.sprite = Resources.Load<Sprite>("FeaturesIcon/" + feature);
            //Debug.Log(feature);
        }
    }
}