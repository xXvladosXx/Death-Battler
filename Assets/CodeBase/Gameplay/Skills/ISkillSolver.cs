using CodeBase.Gameplay.Battle;
using CodeBase.StaticData.Skills;

namespace CodeBase.Gameplay.Skills
{
  public interface ISkillSolver
  {
    void ProcessHeroAction(HeroAction heroAction);
    void SkillDelaysTick();
    float CalculateSkillValue(string skillCasterId, SkillTypeId skillTypeId, string targetId);
  }
}