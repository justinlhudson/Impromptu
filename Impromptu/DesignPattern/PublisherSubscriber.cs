namespace Impromptu.DesignPattern
{
    /// <summary>
    ///   Publisher/Subscriber
    /// </summary>
    public class PublisherSubscriber
    {

        #region Fields

        public delegate void UpdateDelegate();

        public delegate void UpdateDelegateArg(object arg);

        public delegate void UpdateDelegateArgs(object[] args);

        private event UpdateDelegate m_event;
        private event UpdateDelegateArg m_eventArg;
        private event UpdateDelegateArgs m_eventArgs;

        #endregion

        #region Constructor

        public PublisherSubscriber()
        {
            // not needed, but shows purpose and gives functions to call via virtual methods.
            Subscribe(new UpdateDelegate(DelegatePointer));
            Subscribe(new UpdateDelegateArg(DelegatePointer));
            Subscribe(new UpdateDelegateArgs(DelegatePointer));
        }

        #endregion

        #region Public Methods

        public void Subscribe(UpdateDelegate listener)
        {
            if(listener != null)
                m_event += listener;
        }

        public void UnSubscribe(UpdateDelegate listener)
        {
            if(listener != null)
                m_event -= listener;
        }

        public void Subscribe(UpdateDelegateArg listener)
        {
            if(listener != null)
                m_eventArg += listener;
        }

        public void UnSubscribe(UpdateDelegateArg listener)
        {
            if(listener != null)
                m_eventArg -= listener;
        }

        public void Subscribe(UpdateDelegateArgs listener)
        {
            if(listener != null)
                m_eventArgs += listener;
        }

        public void UnSubscribe(UpdateDelegateArgs listener)
        {
            if(listener != null)
                m_eventArgs -= listener;
        }

        public void Publish()
        {
            var evtCopy = m_event;
            if(evtCopy != null)
                evtCopy();
        }

        public void Publish(object arg)
        {
            var evtCopy = m_eventArg;
            if(evtCopy != null)
                evtCopy(arg);
        }

        public void Publish(object[] args)
        {
            var evtCopy = m_eventArgs;
            if(evtCopy != null)
                evtCopy(args);
        }

        #endregion

        #region Protected Virtual Methods

        protected virtual void DelegatePointer()
        {
        }

        protected virtual void DelegatePointer(object arg)
        {
        }

        protected virtual void DelegatePointer(object[] args)
        {
        }

        #endregion

    }
}
