using CodeBase.StaticData.Heroes;
using CodeBase.StaticData.Skills;

namespace CodeBase.Infrastructure.StaticData
{
  public interface IStaticDataService
  {
    void LoadHeroConfigs();
    HeroConfig HeroConfigFor(HeroTypeId typeId);
    HeroSkill HeroSkillFor(SkillTypeId typeId, HeroTypeId heroTypeId);
  }
}