using System;
using UnityTemplateProjects;

namespace CardManagement
{
    [Serializable]
    public class ActionCard : GameCard
    {
        ActionCard(string name, string description, CardType[] cardTypeArray) : base(name, description, cardTypeArray)
        {
            
        }
    }
}