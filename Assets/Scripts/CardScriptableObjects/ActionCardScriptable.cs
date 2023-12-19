using CardManagement;
using UnityEngine;

namespace UnityTemplateProjects.CardScriptableObjects
{
    [CreateAssetMenu(fileName = "New Action Card", menuName = "Cards/Action Card", order = 0)]
    public class ActionCardScriptable : ScriptableObject
    {
        public ActionCard ActionCard;
    }
}