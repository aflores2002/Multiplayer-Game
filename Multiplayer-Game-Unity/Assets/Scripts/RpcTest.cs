     using Unity.Netcode;
     using UnityEngine;

     // RPC test script from Unity guide
     public class RpcTest : NetworkBehaviour
     {
         public override void OnNetworkSpawn()
         {
             if (!IsServer && IsOwner)
             {
                 TestServerRpc(0, NetworkObjectId);
             }
         }

         [Rpc(SendTo.ClientsAndHost)]
         void TestClientRpc(int value, ulong sourceNetworkObjectId)
         {
             Debug.Log($"Client Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
             if (IsOwner)
             {
                 TestServerRpc(value + 1, sourceNetworkObjectId);
             }
         }

         [Rpc(SendTo.Server)]
         void TestServerRpc(int value, ulong sourceNetworkObjectId)
         {
             Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
             TestClientRpc(value, sourceNetworkObjectId);
         }
     }