using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Vault
{
    public interface IPausable
    {
        /// <summary>
        /// gets called when application went to background
        /// </summary>
        public void OnApplicationPaused();
    }

}
