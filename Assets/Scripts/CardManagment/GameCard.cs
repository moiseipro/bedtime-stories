using UnityTemplateProjects.Game;

namespace UnityTemplateProjects
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
    
    public class GameCard
    {
        public string Name { get; }
        public string Description { get; }
        public Features[] Features { get; }
        
        private CardEvent[] _cardEvents;

        public GameCard(string name, string description, Features[] featuresArray)
        {
            Name = name;
            Description = description;
            if (featuresArray.Length > 3) Features = new Features[3];
            else Features = featuresArray;
        }

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