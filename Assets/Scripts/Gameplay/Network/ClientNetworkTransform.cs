using Unity.Netcode.Components;
using UnityEngine;

namespace Gameplay.Network
{
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}