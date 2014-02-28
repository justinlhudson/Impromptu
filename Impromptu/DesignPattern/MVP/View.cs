using Impromptu.Utilities;

namespace Impromptu.DesignPattern.MVP
{
    internal class View
    {
        private readonly PublisherSubscriber m_ps = new PublisherSubscriber();

        public void Subscribe(PublisherSubscriber.EventDelegate listener)
        {
            m_ps.Subscribe(listener);
        }

        public void UnSubscribe(PublisherSubscriber.EventDelegate listener)
        {
            m_ps.UnSubscribe(listener);
        }

        private void Publish()
        {
            m_ps.Publish();
        }
    }
}
