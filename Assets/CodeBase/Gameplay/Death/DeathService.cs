﻿using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using UnityEngine;

namespace CodeBase.Gameplay.Death
{
  public class DeathService : IDeathService
  {
    private const float DefaultDestroyTime = 3;
    private readonly IHeroRegistry _heroRegistry;

    public DeathService(IHeroRegistry heroRegistry)
    {
      _heroRegistry = heroRegistry;
    }
    
    public void ProcessDeadHeroes()
    {
      foreach (string id in _heroRegistry.AllIds)
      {
        HeroBehaviour hero = _heroRegistry.GetHero(id);
        if (!hero.IsDead)
          continue;
        
        _heroRegistry.Unregister(hero.Id);
        
        hero.Animator.PlayDeath();
        Object.Destroy(hero.gameObject, DefaultDestroyTime);
      }
    }
  }
}