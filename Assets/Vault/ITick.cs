using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vault
{
    public interface ITick
    {
        /// <summary>
        /// Gets Called for every frame used for physics calculation and stuff just like Update in monobehaviour
        /// </summary>
        public void OnUpdate();//works as Update method or gets called for every frame
    }
}

