using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace UnityTemplateProjects.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FeaturesView : MonoBehaviour
    {
        private const int _featuresCount = 9;
        
        [SerializeField]private Transform target;
        [SerializeField]private FeaturesItem _featuresItem;
        [SerializeField]private Vector3 _offset;

        private FeaturesItem[] _featuresItems = new FeaturesItem[_featuresCount];
        private RectTransform _rectTransform;
        private Camera _camera;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _camera = Camera.main;
            for (int i = 0; i < _featuresCount; i++)
            {
                _featuresItems[i] = Instantiate(_featuresItem, transform);
            }
            
        }

        private void FixedUpdate()
        {
            if (target)
            {
                _rectTransform.position = _camera.WorldToScreenPoint(target.transform.position+_offset);
            }
        }

        private void ShowFeatures(Features[] features)
        {
            
        }
    }
}