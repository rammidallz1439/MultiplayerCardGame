using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vault
{
    public interface IFixedTick
    {
        /// <summary>
        /// Gets called at a fixed frame rate like FixedUpdate of monobehaviour
        /// </summary>
        public void OnFixedUpdate();// Acts as FixedUpdate
    }
}

