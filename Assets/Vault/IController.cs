using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{
    public interface IController 
    {
        /// <summary>
        /// Gets called before everything
        /// </summary>
        public void OnInitialized();
        /// <summary>
        /// Gets called when object is alive
        /// </summary>
        public void OnVisible();
        /// <summary>
        /// Gets called called after object is alive this is the first thing to be called after object is availaible in hirarchey
        /// </summary>
        public void OnStarted();

        /// <summary>
        /// registers GameEvents
        /// </summary>
        public void OnRegisterListeners();
        /// <summary>
        /// deregisters Gamevent
        /// </summary>
        public void OnRemoveListeners();

        /// <summary>
        /// Gets called when Scene/object is about to be changed or removed or destroyed
        /// </summary>
        public void OnRelease();
    }
}

