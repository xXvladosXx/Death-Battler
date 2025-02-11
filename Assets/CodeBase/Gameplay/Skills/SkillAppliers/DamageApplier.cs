﻿using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.UI;
using CodeBase.Infrastructure.StaticData;
using CodeBase.StaticData.Heroes;
using CodeBase.StaticData.Skills;
using UnityEngine;

namespace CodeBase.Gameplay.Skills.SkillAppliers
{
  public class DamageApplier : ISkillApplier
  {
    private readonly IStaticDataService _staticDataService;
    private readonly IHeroRegistry _heroRegistry;
    private readonly IBattleTextPlayer _battleTextPlayer;
    public SkillKind SkillKind => SkillKind.Damage;

    public DamageApplier(IStaticDataService staticDataService, IHeroRegistry heroRegistry, IBattleTextPlayer battleTextPlayer)
    {
      _staticDataService = staticDataService;
      _heroRegistry = heroRegistry;
      _battleTextPlayer = battleTextPlayer;
    }
    
    public void WarmUp()
    {
      
    }

    public void ApplySkill(ActiveSkill activeSkill)
    {
      foreach (string targetId in activeSkill.TargetIds)
      {
        if (!_heroRegistry.IsAlive(targetId))
          continue;
        
        HeroBehaviour caster = _heroRegistry.GetHero(activeSkill.CasterId);
        HeroSkill skill = _staticDataService.HeroSkillFor(activeSkill.Skill, caster.TypeId);

        HeroBehaviour target = _heroRegistry.GetHero(targetId);
        target.State.CurrentHp -= skill.Value;

        _battleTextPlayer.PlayText($"{skill.Value}", Color.red, target.transform.position);
        PlayFx(skill.CustomTargetFx, target.transform.position);
      }
    }
    
    public float CalculateSkillValue(string casterId, SkillTypeId skillTypeId, string _)
    {
      HeroBehaviour caster = _heroRegistry.GetHero(casterId);
      HeroSkill skill = _staticDataService.HeroSkillFor(skillTypeId, caster.TypeId);
      return skill.Value;
    }

    private void PlayFx(GameObject fxPrefab, Vector3 position)
    {
      if (fxPrefab)
        Object.Instantiate(fxPrefab, position, Quaternion.identity);
    }
  }
}