using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class PlayerSkinsData : ScriptableObject
    {
        [SerializeField] private string folderName;
        [SerializeField] private List<string> assetNames;

        public string FolderName => folderName;
        public List<string> AssetNames => assetNames;

        public string GetRandomSkinName()
        {
            return assetNames[Random.Range(0, assetNames.Count)];
        }

        public GameObject GetSkin(string name)
        {
            return Resources.Load($"{folderName}/{name}") as GameObject;
        }
    }
}