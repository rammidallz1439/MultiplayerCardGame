using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{
    public class EventManager : IController, ITick
    {
        public bool LimitQueueProcess = false;
        public int QueueProcessTime = 20;

        private static EventManager instance = null;
        public static bool isDestroyed = false;

        public delegate void EventDelegate<T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        private readonly Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
        private readonly Dictionary<System.Delegate, EventDelegate> delegatesLookUp = new Dictionary<System.Delegate, EventDelegate>();
        private readonly Dictionary<System.Delegate, bool> lookUpOnce = new Dictionary<System.Delegate, bool>();
        public readonly Queue m_EventQueue = new Queue();

        public static EventManager Instance
        {
            get
            {
                if (!isDestroyed && instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }

        public bool IsDestroyed
        {
            get
            {
                return isDestroyed;
            }
            set
            {
                isDestroyed = value;
            }
        }

        private EventDelegate AddDelegate<T>(EventDelegate<T> @delegate) where T : GameEvent
        {
            if (delegatesLookUp.ContainsKey(@delegate))
            {
                return null;
            }

            EventDelegate internalDelegate = (e) => @delegate((T)e);
            delegatesLookUp[@delegate] = internalDelegate;

            EventDelegate tempDelegate = null;
            if (delegates.TryGetValue(typeof(T), out tempDelegate))
            {
                delegates[typeof(T)] = tempDelegate += internalDelegate;
            }
            else
            {
                delegates[typeof(T)] = internalDelegate;
            }
            return internalDelegate;
        }

        public void AddListener<T>(EventDelegate<T> @delegate) where T : GameEvent
        {
            AddDelegate(@delegate);
        }

        public void RemoveListener<T>(EventDelegate<T> @delegate) where T : GameEvent
        {
            if (delegatesLookUp.TryGetValue(@delegate, out EventDelegate internalDelegate))
            {
                if (delegates.TryGetValue(typeof(T), out EventDelegate tempDelegate))
                {
                    tempDelegate -= internalDelegate;
                    if (tempDelegate == null)
                    {
                        delegates.Remove(typeof(T));
                    }
                    else
                    {
                        delegates[typeof(T)] = tempDelegate;
                    }
                }
                delegatesLookUp.Remove(@delegate);
            }
        }

        private void RemoveAll()
        {
            delegates.Clear();
            delegatesLookUp.Clear();
            lookUpOnce.Clear();
        }

        public bool HasListener<T>(EventDelegate<T> @delegate) where T : GameEvent
        {
            return delegatesLookUp.ContainsKey(@delegate);
        }


        public void TriggerEvent(GameEvent e)
        {
            if (delegates.TryGetValue(e.GetType(), out EventDelegate @delegate))
            {
                @delegate.Invoke(e);
            }
            else
            {
                Debug.Log("Event:" + e.GetType() + " has no listener");
            }
        }

        public bool QueueEvent(GameEvent e)
        {
            if (delegates.ContainsKey(e.GetType()))
            {
                return false;
            }

            m_EventQueue.Enqueue(e);
            return true;
        }


        #region Contract Methods
        void IController.OnInitialized()
        {

        }

        void IController.OnRegisterListeners()
        {
        }

        void IController.OnRelease()
        {
            RemoveAll();
            m_EventQueue.Clear();
            IsDestroyed = true;
        }

        void IController.OnRemoveListeners()
        {
        }

        void IController.OnStarted()
        {
        }

        void ITick.OnUpdate()
        {
            DateTime startTime = DateTime.Now;

            while (m_EventQueue.Count > 0)
            {
                if (LimitQueueProcess)
                {
                    if((DateTime.Now-startTime).Milliseconds > QueueProcessTime)
                    {
                        return;
                    }
                }
                GameEvent e = m_EventQueue.Dequeue() as GameEvent;
                TriggerEvent(e);
            }
        }

        void IController.OnVisible()
        {
        }

        #endregion
    }
}
