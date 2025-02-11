using System.Linq;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI.Calculations
{
    public static class GetInput
    {
        private const int TRUE = 1;
        private const int FALSE = 0;

        public static float PercentageDamage(BattleSkill skill, IHero target, ISkillSolver skillSolver)
        {
            var damage = PotentialDamage(skill, target, skillSolver);
            return damage / target.State.MaxHp;
        }

        public static float KillingBlow(BattleSkill skill, IHero target, ISkillSolver skillSolver)
        {
            var damage = PercentageDamage(skill, target, skillSolver);
            return damage > target.State.CurrentHp
                ? TRUE
                : FALSE;
        }

        public static float HealPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
            skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);

        public static float HpPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) =>
            target.State.CurrentHp / target.State.MaxHp;

        public static float InitiativeBurn(BattleSkill skill, IHero target, ISkillSolver skillSolver)
        {
            float burn = skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);
            return burn / target.State.MaxInitiative;
        }

        public static float TargetUltimateIsReady(BattleSkill skill, IHero target, ISkillSolver skillSolver) =>
            target.State.SkillStates.Last().IsReady
                ? TRUE
                : FALSE;

        private static float PotentialDamage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
            skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);
    }
}