using System.Collections.Generic;
using UnityEngine;


namespace Vault
{
    public abstract class Registerer : MonoBehaviour
    {

        public List<IController> Controllers = new List<IController>();
        public List<ITick> Ticks = new List<ITick>();
        public List<IFixedTick> FixedTicks = new List<IFixedTick>();
        public List<ILateTick> LateTicks = new List<ILateTick>();
        public List<IPausable> pausables = new List<IPausable>();

        public void Awake()
        {
            OnAwake();
            InitializeListeners();
            NotifyControllers();
        }
        public void OnEnable()
        {
            Enable();
            NotifyOnActivated();
        }

        public void Start()
        {
            OnStart();
            NotifyOnStarted();

        }

        public void FixedUpdate()
        {
            NotifyFixedUpdates();
        }

        public void Update()
        {
            NotifyUpdates();

        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            NotifyOnDisabled();
        }

        private void OnApplicationPause(bool pause)
        {
            NotifyPausables();
        }

        #region public methods

        public abstract void OnAwake();
        public abstract void Enable();
        public abstract void OnStart();


        //call this to add a Controller or any tick
        public void AddController(IController controller)
        {
            if (!Controllers.Contains(controller))
            {
                Controllers.Add(controller);
                AddRespectiveTicks(controller);
            }
            else
            {
                Debug.LogError("Observer not added or not the right type of observer");
            }
        }

        public void AddRespectiveTicks(IController controller)
        {
            if (controller is IFixedTick)
            {
                FixedTicks.Add((IFixedTick)controller);
            }

            if (controller is ITick)
            {
                Ticks.Add((ITick)controller);
            }

            if (controller is ILateTick)
            {
                LateTicks.Add((ILateTick)controller);
            }

            if (controller is IPausable)
            {
                pausables.Add((IPausable)controller);
            }
        }
        #endregion

        #region private methods


        private void InitializeListeners()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRegisterListeners();
            }
        }

        private void RemoveListeners()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRemoveListeners();
            }
        }

        private void NotifyControllers()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnInitialized();
            }
        }

        private void NotifyOnActivated()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnVisible();
            }
        }

        private void NotifyOnStarted()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnStarted();
            }
        }

        private void NotifyOnDisabled()
        {
            foreach (IController controller in Controllers)
            {
                controller.OnRelease();
            }
            RemoveListeners();
            RevomeControllers();
        }

        private void NotifyUpdates()
        {
            foreach (ITick tick in Ticks)
            {
                tick.OnUpdate();
            }
        }

        private void NotifyFixedUpdates()
        {
            foreach (IFixedTick tick in FixedTicks)
            {
                tick.OnFixedUpdate();
            }
        }

        private void NotifyPausables()
        {
            foreach (IPausable pausable in pausables)
            {
                pausable.OnApplicationPaused();
            }
        }

        private void RevomeControllers()
        {
            Controllers.Clear();
            Ticks.Clear();
            FixedTicks.Clear();
            LateTicks.Clear();
        }
        #endregion
    }

}
