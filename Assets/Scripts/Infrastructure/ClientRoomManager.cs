using Mirror;

namespace Infrastructure
{
    public sealed class ClientRoomManager : NetworkBehaviour
    {
        public readonly SyncList<PlayerData> PlayerDatas = new SyncList<PlayerData>();

        [Server]
        public void AddPlayerData(Player player)
        {
            PlayerDatas.Add(player.Data);
        }

        [Server]
        public void RemovePlayerData(Player player)
        {
            PlayerDatas.Remove(player.Data);
        }
    }
}