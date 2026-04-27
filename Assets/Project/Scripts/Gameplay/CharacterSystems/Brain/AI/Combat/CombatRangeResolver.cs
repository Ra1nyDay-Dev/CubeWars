using Project.Scripts.Gameplay.Data.Configs.AI;
using Project.Scripts.Gameplay.WeaponSystems;

namespace Project.Scripts.Gameplay.CharacterSystems.Brain.AI.Combat
{
    public static class CombatRangeResolver
    {
        public static float GetAttackRange(IWeapon weapon, BotConfig config)
        {
            return weapon switch
            {
                MeleeWeapon => config.MeleeAttackRange,
                RangeWeapon => config.RangedAttackRange,
                _ => 0f
            };
        }
 
        public static (float min, float max) GetApproachRadius(IWeapon weapon, BotConfig config)
        {
            return weapon switch
            {
                MeleeWeapon => (config.MeleeApproachMinRadius, config.MeleeApproachMaxRadius),
                RangeWeapon => (config.RangedApproachMinRadius, config.RangedApproachMaxRadius),
                _ => (0f, 0f)
            };
        }
    }
}