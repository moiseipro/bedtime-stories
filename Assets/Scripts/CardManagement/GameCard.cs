using System;
using Game;
using UnityEngine;

namespace CardManagement
{
    public enum Features
    {
        Holy = 1,
        Demonic = 2,
        Physical = 3,
        Light = 4,
        Sound = 5,
        Smell = 6
    }

    public enum CardType
    {
        Nature = 1,
        Settlement = 2,
        Magic = 3,
    }
    
    [Serializable]
    public class GameCard
    {
        protected int _id;
        public int ID => _id;
        [SerializeField] protected string _name;
        public string Name => _name;
        [SerializeField] protected string _description;
        public string Description => _description;

        [SerializeField] protected CardType[] _cardTypes;
        public CardType[] CardTypes => _cardTypes;

        [SerializeField] protected CardEvent[] _cardEvents;

        public GameCard(string name, string description, CardType[] cardTypeArray)
        {
            _name = name;
            _description = description;
            if (cardTypeArray.Length > 3) _cardTypes = new CardType[3];
            else _cardTypes = cardTypeArray;
        }

        public void SetId(int id)
        {
            _id = id;
        }
        
        public void ActivateAllEvents(ICardTarget player)
        {
            foreach (var cardEvent in _cardEvents)
            {
                cardEvent.Activate(player);
            }
        }

        public void ActivateEvent(ICardTarget player, int number)
        {
            if(_cardEvents.Length < number || number < 0) return;
            _cardEvents[number].Activate(player);
        }
        
    }
}