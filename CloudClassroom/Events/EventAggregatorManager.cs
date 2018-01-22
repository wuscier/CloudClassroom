using Prism.Events;

namespace CloudClassroom.Events
{
    public class EventAggregatorManager
    {
        private EventAggregatorManager()
        {
            EventAggregator = new EventAggregator();
        }
        public static readonly EventAggregatorManager Instance = new EventAggregatorManager();

        public EventAggregator EventAggregator { get; }
    }
}
