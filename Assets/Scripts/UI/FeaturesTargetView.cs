using UnityEngine;

namespace UnityTemplateProjects.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class FeaturesTargetView : FeaturesView
    {
        [SerializeField]private Transform target;
        [SerializeField]private Vector3 _offset;
        
        private RectTransform _rectTransform;
        private Camera _camera;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _camera = Camera.main;
            GenerateFeaturesItems();
        }
        
        private void FixedUpdate()
        {
            if (target)
            {
                _rectTransform.position = _camera.WorldToScreenPoint(target.transform.position+_offset);
            }
        }
    }
}