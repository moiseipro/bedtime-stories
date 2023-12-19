using System;
using UnityEngine;

namespace CardManagement
{
    [Serializable]
    public class LocationCard : GameCard
    {
        [SerializeField] protected int _turnPrice;
        public int TurnPrice => _turnPrice;

        public LocationCard(string name, string description, int turnPrice, CardType[] cardTypeArray) : base(name, description, cardTypeArray)
        {
            _name = name;
            _description = description;
            _turnPrice = turnPrice;
            if (cardTypeArray.Length > 3) _cardTypes = new CardType[3];
            else _cardTypes = cardTypeArray;
        }
    }
}