using CodeBase.Gameplay.AI;
using CodeBase.Gameplay.AI.Reporting;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Cooldowns;
using CodeBase.Gameplay.Death;
using CodeBase.Gameplay.Factory;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.Initiative;
using CodeBase.Gameplay.Skills;
using CodeBase.Gameplay.Skills.Targeting;
using CodeBase.Gameplay.UI;
using CodeBase.Infrastructure.StaticData;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure
{
  public class BootstrapInstaller : MonoInstaller, IInitializable
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();

      Container.BindInterfacesTo<CooldownService>().AsSingle();
      Container.BindInterfacesTo<BattleTextPlayer>().AsSingle();
      Container.BindInterfacesTo<BattleConductor>().AsSingle();
      Container.BindInterfacesTo<SkillSolver>().AsSingle();
      Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
      Container.Bind<IHeroFactory>().To<HeroFactory>().AsSingle();
      Container.Bind<IHeroRegistry>().To<HeroRegistry>().AsSingle();
      Container.Bind<IAIReporter>().To<AIReporter>().AsSingle();

     
      Container.Bind<IDeathService>().To<DeathService>().AsSingle();
      Container.Bind<IInitiativeService>().To<InitiativeService>().AsSingle();
      Container.Bind<ITargetPicker>().To<TargetPicker>().AsSingle();
      Container.Bind<IArtificialIntelligence>().To<StupidAI>().AsSingle();
    }

    public void Initialize()
    {
      Container.Resolve<IStaticDataService>().LoadHeroConfigs();
      
      SceneManager.LoadScene("Game");
    }
  }
}