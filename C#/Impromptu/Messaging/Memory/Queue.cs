using System;
using ServiceStack.Redis;
using Impromptu.Utilities;
using System.Collections.Concurrent;

namespace Impromptu.Messaging.Memory
{
  public class Queue : IQueue
  {
    private ConcurrentDictionary<string, ConcurrentQueue<object>> _queueList;

    public Queue()
    {
      _queueList = new ConcurrentDictionary<string, ConcurrentQueue<object>>();
    }

    public void Flush(string list)
    {
      if(_queueList.ContainsKey(list))
      {
        var queue = _queueList[list];
        while(queue.Count > 0)
        {
          object temp;
          queue.TryDequeue(out temp);      
        }
      }
    }

    public long Length(string list)
    {
      if(_queueList.ContainsKey(list))
      {
        var queue = _queueList[list];
        return  queue.Count;
      }
      return -1;
    }

    public void Push<T>(string list, T value)
    {
      if(!_queueList.ContainsKey(list))
      if(_queueList.TryAdd(list, new ConcurrentQueue<object>()))
      {
        var queue = _queueList[list];
        queue.Enqueue(value);
      }
      else
        throw new Exception("List not created");
    }

    public T Pop<T>(string list)
    {
      if(_queueList.ContainsKey(list))
      {
        var queue = _queueList[list];
        object temp;
        if(queue.TryDequeue(out temp))
          return (T)temp;          
      }

      return default(T);
    }
  }
}

