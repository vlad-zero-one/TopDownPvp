using ExitGames.Client.Photon;

namespace Game.Model
{
    public static class PhotonCustomTypesRegistrationManager
    {
        public static void Register()
        {
            PhotonPeer.RegisterType(typeof(Bullet), (byte)'B', BulletSerializer.Serialize, BulletSerializer.Deserialize);
        }
    }
}