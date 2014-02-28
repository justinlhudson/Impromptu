namespace Impromptu.DesignPattern
{
    /// <summary>
    ///   Publisher/Subscriber
    /// </summary>
    public class PublisherSubscriber
    {
        #region Fields

        public delegate void EventDelegate();

        public delegate void EventDelegateArg(object arg);

        public delegate void EventDelegateArgs(object[] args);

        private event EventDelegate _event;
        private event EventDelegateArg _eventArg;
        private event EventDelegateArgs _eventArgs;

        #endregion

        #region Constructor

        public PublisherSubscriber()
        {
            // not needed, but shows purpose and gives functions to call via virtual methods.
            Subscribe(new EventDelegate(DelegatePointer));
            Subscribe(new EventDelegateArg(DelegatePointer));
            Subscribe(new EventDelegateArgs(DelegatePointer));
        }

        #endregion

        #region Public Methods

        public void Subscribe(EventDelegate listener)
        {
            if(listener != null)
                _event += listener;
        }

        public void UnSubscribe(EventDelegate listener)
        {
            if(listener != null)
                _event -= listener;
        }

        public void Subscribe(EventDelegateArg listener)
        {
            if(listener != null)
                _eventArg += listener;
        }

        public void UnSubscribe(EventDelegateArg listener)
        {
            if(listener != null)
                _eventArg -= listener;
        }

        public void Subscribe(EventDelegateArgs listener)
        {
            if(listener != null)
                _eventArgs += listener;
        }

        public void UnSubscribe(EventDelegateArgs listener)
        {
            if(listener != null)
                _eventArgs -= listener;
        }

        public void Publish()
        {
            var evtCopy = _event;
            if(evtCopy != null)
                evtCopy();
        }

        public void Publish(object arg)
        {
            var evtCopy = _eventArg;
            if(evtCopy != null)
                evtCopy(arg);
        }

        public void Publish(object[] args)
        {
            var evtCopy = _eventArgs;
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
