using Impromptu.DesignPattern;

namespace Impromptu.MVP
{
    internal class View
    {
        private readonly PublisherSubscriber m_ps = new PublisherSubscriber();

        public void Subscribe(PublisherSubscriber.UpdateDelegate listener)
        {
            m_ps.Subscribe(listener);
        }

        public void UnSubscribe(PublisherSubscriber.UpdateDelegate listener)
        {
            m_ps.UnSubscribe(listener);
        }

        private void Publish()
        {
            m_ps.Publish();
        }
    }
}
