using Impromptu.DesignPattern;

namespace Impromptu.DesignPattern.MVP
{
    /*
     * View:
     * Renders the model into a form suitable for interaction, typically a user
     * interface element. Multiple views can exist for a single model for
     * different purposes.
     */
    internal interface IView
    {
        void Subscribe(PublisherSubscriber.EventDelegate listener);

        void UnSubscribe(PublisherSubscriber.EventDelegate listener);
    }
}
