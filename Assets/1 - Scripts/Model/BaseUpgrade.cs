using System.Collections.Generic;

namespace Game.Model.Upgrades
{
    public class BaseUpgrade
    {
        public readonly IReadOnlyList<float> Multipliers;
        public int Level { get; private set; } = 0;
        public float CurrentMultiplier => Multipliers[Level];

        public BaseUpgrade(IReadOnlyList<float> multipliers)
        {
            if (multipliers == null || multipliers.Count < 1)
            {
                throw new System.Exception($"Can't initialize {this}: multipliers in null or empty");
            }
            Multipliers = multipliers;
        }

        public void LevelUp()
        {
            if (Multipliers.Count - 1 > Level)
            {
                Level++;
            }
            else
            {
                throw new System.Exception($"Can't level up {this} due it's already maximum level");
            }
        }
    }
}