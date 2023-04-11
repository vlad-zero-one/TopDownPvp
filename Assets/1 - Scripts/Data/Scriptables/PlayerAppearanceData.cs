using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu]
    public class PlayerAppearanceData : ScriptableObject
    {
        [Header("UI DATA")]
        [Tooltip("Name that appears above local player in game")]
        [SerializeField] private string playerNameReplacement;
        [SerializeField] private Color playerNameColor;
        [SerializeField] private Color enemyNameColor;

        [Header("SKINS DATA")]
        [SerializeField] private string folderName;
        [SerializeField] private List<string> assetNames;

        public string PlayerNameReplacement => playerNameReplacement;
        public Color PlayerNameColor => playerNameColor;
        public Color EnemyNameColor => enemyNameColor;

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