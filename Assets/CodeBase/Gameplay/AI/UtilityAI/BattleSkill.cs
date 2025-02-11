using CodeBase.StaticData.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class BattleSkill
    {
        public string CasterId;
        public SkillTypeId TypeId;
        public SkillKind Kind;
        public TargetType TargetType;
        public float MaxCooldown;
        public bool IsSingleTarget => TargetType is TargetType.Ally or TargetType.Enemy or TargetType.Self;
    }
}