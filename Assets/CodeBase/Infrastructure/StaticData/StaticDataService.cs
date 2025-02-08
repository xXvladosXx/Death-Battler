using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Heroes;
using CodeBase.StaticData.Skills;
using UnityEngine;

namespace CodeBase.Infrastructure.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string HeroConfigFolderPath = "Configs/Heroes/";
    
    private Dictionary<HeroTypeId, HeroConfig> _heroConfigs = new();

    public HeroConfig HeroConfigFor(HeroTypeId typeId)
    {
      if (_heroConfigs.TryGetValue(typeId, out HeroConfig config))
        return config;
      
      throw new KeyNotFoundException($"No config found for {typeId}");
    }

    public HeroSkill HeroSkillFor(SkillTypeId typeId, HeroTypeId heroTypeId)
    {
      HeroConfig heroConfig = HeroConfigFor(heroTypeId);
      HeroSkill heroSkill = heroConfig.Skills.Find(x => x.TypeId == typeId);
      if (heroSkill != null)
        return heroSkill;
      
      throw new KeyNotFoundException($"No hero skill config found for {typeId} on {heroTypeId}");
    }
    
    public void LoadHeroConfigs()
    {
      _heroConfigs = Resources
        .LoadAll<HeroConfig>(HeroConfigFolderPath)
        .ToDictionary(x => x.TypeId, x => x);
    }
  }
}