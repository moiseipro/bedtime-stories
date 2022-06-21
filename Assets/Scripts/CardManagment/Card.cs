using UnityTemplateProjects.Game;

namespace UnityTemplateProjects
{
    public enum Features
    {
        Holy = 1,
        Demonic = 2,
        Physical = 3,
        EmitsLight = 4,
        MakesSound = 5,
        HasSmell = 6
    }
    
    public abstract class Card
    {
        private string _name;
        private string _description;
        private Features[] _cardFeatures;
        
        private CardEvent[] _cardEvents;

        public void ActivateAllEvents(IPlayer player)
        {
            foreach (var cardEvent in _cardEvents)
            {
                cardEvent.Activate(player);
            }
        }

        public void ActivateEvent(IPlayer player, int number)
        {
            if(_cardEvents.Length < number && number < 0) return;
            _cardEvents[number].Activate(player);
        }
        
    }
}