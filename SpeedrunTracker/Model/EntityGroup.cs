using SpeedrunTracker.Model.Enum;
using System.Collections.ObjectModel;

namespace SpeedrunTracker.Model
{
    public class EntityGroup : ObservableCollection<Entity>
    {
        public EntityType EntityType { get; }

        public string ImageSource => EntityType switch
        {
            EntityType.Games => "game.svg",
            EntityType.Users => "user.svg",
            _ => string.Empty,
        };

        public EntityGroup(EntityType searchType, List<Entity> items) : base(items)
        {
            EntityType = searchType;
        }
    }
}
