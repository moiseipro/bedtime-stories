using CardManagement;
using UnityEngine;

namespace UnityTemplateProjects.CardScriptableObjects
{
    [CreateAssetMenu(fileName = "New Location Card", menuName = "Cards/Location Card", order = 0)]
    public class LocationCardScriptable : ScriptableObject
    {
        public LocationCard LocationCard;
    }
}