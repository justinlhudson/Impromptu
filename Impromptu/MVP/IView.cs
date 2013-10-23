using Impromptu.DesignPattern;

namespace Impromptu.MVP
{
    /*
     * View:
     * Renders the model into a form suitable for interaction, typically a user
     * interface element. Multiple views can exist for a single model for
     * different purposes.
     */
    internal interface IView
    {
        void Subscribe(PublisherSubscriber.UpdateDelegate listener);

        void UnSubscribe(PublisherSubscriber.UpdateDelegate listener);
    }
}
