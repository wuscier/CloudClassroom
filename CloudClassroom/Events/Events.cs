using Prism.Events;

namespace CloudClassroom.Events
{

    public enum Target
    {
        LoginView,
        LoginViewModel,
        MainView,
        MainViewModel,
        MeetingView,
        MeetingViewModel,
        SharingOptionsView,
        SharingOptionsViewModel,
        WhiteboardView,
        WhiteboardViewModel,
        JoinMeetingView,
        JoinMeetingViewModel,
    }

    public class Argument
    {
        public Category Category { get; set; }
        public object Value { get; set; }
    }


    public enum Category
    {
        UserName,
        Password,
    }

    public class EventArgument
    {
        public Target Target { get; set; }
        public Argument Argument { get; set; }
    }

    public class UIGotFocusEvent : PubSubEvent<EventArgument> { }

    public class LoginSuccessEvent : PubSubEvent<EventArgument> { }
    public class StartOrJoinSuccessEvent : PubSubEvent<EventArgument> { }

    public class IntoMeetingSuccessEvent : PubSubEvent<EventArgument> { }
    public class SetVideoPositionEvent : PubSubEvent<EventArgument> { }

    public class LeaveMeetingEvent : PubSubEvent<EventArgument> { }


    public class ShowRecordPathEvent : PubSubEvent<EventArgument> { }
    public class ShowSharingOptionsEvent : PubSubEvent<EventArgument> { }
}