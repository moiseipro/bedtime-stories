using System;
using CardManagement;
using Source;
using UnityEngine;
using UnityTemplateProjects.CardScriptableObjects;

namespace Gameplay
{
    public class CardLoader : Singleton<CardLoader>
    {
        [SerializeField] private ActionCardScriptable[] actionCards;
        public int ActionCardsCount => actionCards.Length;
        [SerializeField] private LocationCardScriptable[] locationCards;
        public int LocationCardsCount => locationCards.Length;

        public LocationCard GetLocationCard(int index)
        {
            if(index >= locationCards.Length) return null;
            locationCards[index].LocationCard.SetId(index);
            return locationCards[index].LocationCard;
        }

        public ActionCard GetActionCard(int index)
        {
            if(index >= actionCards.Length) return null;
            actionCards[index].ActionCard.SetId(index);
            return actionCards[index].ActionCard;
        }
    }
}