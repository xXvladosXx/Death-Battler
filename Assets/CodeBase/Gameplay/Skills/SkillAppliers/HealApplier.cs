﻿using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.UI;
using CodeBase.Infrastructure.StaticData;
using CodeBase.StaticData.Heroes;
using CodeBase.StaticData.Skills;
using UnityEngine;

namespace CodeBase.Gameplay.Skills.SkillAppliers
{
  public class HealApplier : ISkillApplier
  {
    private const string FXPrefabPath = "Fx/heal/healFx";
    
    private readonly IStaticDataService _staticDataService;
    private readonly IHeroRegistry _heroRegistry;
    private readonly IBattleTextPlayer _battleTextPlayer;
    private GameObject _fXPrefab;

    public SkillKind SkillKind => SkillKind.Heal;
    
    public HealApplier(IStaticDataService staticDataService, IHeroRegistry heroRegistry, IBattleTextPlayer battleTextPlayer)
    {
      _staticDataService = staticDataService;
      _heroRegistry = heroRegistry;
      _battleTextPlayer = battleTextPlayer;
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

        float healed = target.State.MaxHp * skill.Value;
        target.State.CurrentHp += healed;

        _battleTextPlayer.PlayText($"+{healed}", Color.green, target.transform.position);
        PlayFx(target.transform.position);
      }
    }
    
    public float CalculateSkillValue(string casterId, SkillTypeId skillTypeId, string targetId)
    {
      HeroBehaviour caster = _heroRegistry.GetHero(casterId);
      HeroBehaviour target = _heroRegistry.GetHero(targetId);
      HeroSkill skill = _staticDataService.HeroSkillFor(skillTypeId, caster.TypeId);

      return target.State.MaxHp * skill.Value;
    }

    public void WarmUp()
    {
      _fXPrefab = Resources.Load<GameObject>(FXPrefabPath);
    }

    private void PlayFx(Vector3 targetPosition) => 
      Object.Instantiate(_fXPrefab, targetPosition, Quaternion.identity);
  }
}