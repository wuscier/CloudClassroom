using Prism.Events;

namespace CloudClassroom.Events
{

    public enum Target
    {
        FreeSDKAuthView,
        FreeSDKAuthViewModel,
        FreeLoginView,
        FreeLoginViewModel,
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
        Show,
        Hide,
        Enter,
        Exit
    }

    public class EventArgument
    {
        public Target Target { get; set; }
        public Argument Argument { get; set; }
    }

    public class UIGotFocusEvent : PubSubEvent<EventArgument> { }
    public class ShowLessonDetailEvent : PubSubEvent<EventArgument> { }

    public class SDKAuthSuccessEvent : PubSubEvent<EventArgument> { }

    public class LoginSuccessEvent : PubSubEvent<EventArgument> { }
    public class StartOrJoinSuccessEvent : PubSubEvent<EventArgument> { }

    public class IntoMeetingSuccessEvent : PubSubEvent<EventArgument> { }

    public class ResetVideoUiEvent : PubSubEvent<EventArgument> { }

    public class MeetingViewVisibleEvent : PubSubEvent<EventArgument> { }

    public class FullScreenStatusEvent : PubSubEvent<EventArgument> { }

    public class ShowRecordPathEvent : PubSubEvent<EventArgument> { }
    public class ShowSharingOptionsEvent : PubSubEvent<EventArgument> { }




}