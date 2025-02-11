using CodeBase.StaticData.Skills;

namespace CodeBase.Gameplay.Skills.SkillAppliers
{
  public interface ISkillApplier
  {
    void WarmUp();
    void ApplySkill(ActiveSkill activeSkill);
    SkillKind SkillKind { get; }
    float CalculateSkillValue(string casterId, SkillTypeId skillTypeId, string targetId);
  }
}