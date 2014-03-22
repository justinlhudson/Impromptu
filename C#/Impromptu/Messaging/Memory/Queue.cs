using System;
using Impromptu.Utilities;
using System.Collections.Concurrent;

namespace Impromptu.Messaging.Memory
{
    public class Queue : IQueue
    {
        private static ConcurrentDictionary<string, ConcurrentQueue<object>> _queueList;

        public Queue()
        {
            _queueList = new ConcurrentDictionary<string, ConcurrentQueue<object>>();
        }

        public void Flush(string list)
        {
            if(_queueList.ContainsKey(list))
            {
                while(_queueList[list].Count > 0)
                {
                    object temp;
                    _queueList[list].TryDequeue(out temp);      
                }
            }
        }

        public long Length(string list)
        {
            if(_queueList.ContainsKey(list))
                return  _queueList[list].Count;
            return -1;
        }

        public void Push<T>(string list, T value)
        {
            if(!_queueList.ContainsKey(list))
                _queueList.TryAdd(list, new ConcurrentQueue<object>());

            if(_queueList.ContainsKey(list))
                _queueList[list].Enqueue(value);
            else
                throw new Exception("List not created");
        }

        public T Pop<T>(string list)
        {
            if(_queueList.ContainsKey(list))
            {
                object temp;
                if(_queueList[list].TryDequeue(out temp))
                    return (T)temp;          
            }

            return default(T);
        }
    }
}

