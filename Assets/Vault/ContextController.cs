

namespace Vault
{
    public class ContextController : Registerer
    {
        public override void Enable()
        {
        }

        public override void OnAwake()
        {
            AddController(EventManager.Instance);
            AddController(ObjectPoolManager.Instance);
            AddController(GenericEventsController.Instance);
            AddController(DataManager.Instance);
        }

        public override void OnStart()
        {
        }
    }
}

