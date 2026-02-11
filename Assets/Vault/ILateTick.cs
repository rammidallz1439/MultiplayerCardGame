using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{
    public interface ILateTick
    {
        /// <summary>
        /// Gets called after onUpdate
        /// </summary>
        public void OnLateUpdate();// Acts as LateUpdate
    }
}

