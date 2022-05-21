using UnityTemplateProjects.Game;

namespace UnityTemplateProjects
{
    public abstract class Card
    {
        private string _name;
        private string _description;
        private int _horror;
        private int _reason;
        
        private CardDanger[] _cardDangers;

        public void ActivateAllDangers(IPlayer player)
        {
            foreach (var cardDanger in _cardDangers)
            {
                cardDanger.ActivateAllEvents(player);
            }
        }

        public void ActivateDanger(IPlayer player, int number)
        {
            if(_cardDangers.Length < number && number < 0) return;
            _cardDangers[number].ActivateAllEvents(player);
        }
        
    }
}