using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UnityTemplateProjects.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FeaturesView : MonoBehaviour
    {
        [SerializeField]private int _featuresCount = 9;
        [SerializeField]private FeaturesItem _featuresItem;

        private FeaturesItem[] _featuresItems;
        

        private void Awake()
        {
            GenerateFeaturesItems();
        }

        public void GenerateFeaturesItems()
        {
            _featuresItems = new FeaturesItem[_featuresCount];
            for (int i = 0; i < _featuresCount; i++)
            {
                _featuresItems[i] = Instantiate(_featuresItem, transform);
            }

        }

        public void ShowFeatures(Features[] features)
        {
            for (int i = 0; i < features.Length; i++)
            {
                _featuresItems[i].ShowIconFeature(features[i]);
            }
        }
    }
}