using System.Collections.Generic;
using System.Linq;
using CodeBase.Extensions;
using CodeBase.Gameplay.AI.Reporting;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.HeroRegistry;
using CodeBase.Gameplay.Skills;
using CodeBase.Gameplay.Skills.Targeting;
using CodeBase.Infrastructure.StaticData;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class UtilityAI : IArtificialIntelligence
    {
        private readonly IStaticDataService _staticDataService;
        private readonly ITargetPicker _targetPicker;
        private readonly IHeroRegistry _heroRegistry;
        private readonly ISkillSolver _skillSolver;
        private readonly IAIReporter _aiReporter;

        private readonly IEnumerable<IUtilityFunction> _utilityFunctions;

        public UtilityAI(IStaticDataService staticDataService,
            ITargetPicker targetPicker, IHeroRegistry heroRegistry,
            ISkillSolver skillSolver, IAIReporter aiReporter)
        {
            _staticDataService = staticDataService;
            _targetPicker = targetPicker;
            _heroRegistry = heroRegistry;
            _skillSolver = skillSolver;
            _aiReporter = aiReporter;

            _utilityFunctions = new Brains().GetUtilityFunctions();
        }

        public HeroAction MakeBestDecision(IHero readyHero)
        {
            var choices = GetScoredHeroActions(readyHero, ReadyBattleSkills(readyHero)).ToList();
            _aiReporter.ReportDecisionScores(readyHero, choices);
            return choices.FindMax(x => x.Score);
        }

        private IEnumerable<BattleSkill> ReadyBattleSkills(IHero readyHero)
        {
            return readyHero.State.SkillStates
                .Where(x => x.IsReady)
                .Select(x =>
                {
                    var heroSkillFor = _staticDataService.HeroSkillFor(x.TypeId, readyHero.TypeId);
                    
                    return new BattleSkill
                    {
                        CasterId = readyHero.Id,
                        TypeId = x.TypeId,
                        Kind = heroSkillFor.Kind,
                        TargetType = heroSkillFor.TargetType,
                        MaxCooldown = x.MaxCooldown
                    };
                });
        }

        private IEnumerable<ScoredAction> GetScoredHeroActions(IHero readyHero, IEnumerable<BattleSkill> readySkills)
        {
            foreach (var readySkill in readySkills)
            {
                foreach (var heroSet in HeroSetsForSkill(readySkill, readyHero))
                {
                    var score = CalculateScore(readySkill, heroSet);
                    if (!score.HasValue)
                        continue;

                    yield return new ScoredAction(readyHero, readySkill, heroSet.Targets, score.Value);
                }
            }
        }

        private float? CalculateScore(BattleSkill readySkill, HeroSet heroSet)
        {
            if (heroSet.Targets.IsNullOrEmpty())
                return null;
            
            return heroSet.Targets
                .Select(hero => CalculateScore(readySkill, hero))
                .Where(x => x != null)
                .Sum();
        }

        private float? CalculateScore(BattleSkill readySkill, IHero hero)
        {
            var scoreFactors = (from utilityFunction in _utilityFunctions
                where utilityFunction.AppliesTo(readySkill, hero)
                let input = utilityFunction.GetInput(readySkill, hero, _skillSolver)
                let score = utilityFunction.Score(input, hero)

                select new ScoreFactor(utilityFunction.Name, score)).ToList();

            _aiReporter.ReportDecisionDetails(readySkill, hero, scoreFactors);
            
            return scoreFactors.Select(x => x.Score).SumOrNull();
        }

        private IEnumerable<HeroSet> HeroSetsForSkill(BattleSkill readySkill, IHero readyHero)
        {
            var availableTargets = _targetPicker.AvailableTargetsFor(readyHero.Id, readySkill.TargetType);
            if (readySkill.IsSingleTarget)
            {
                foreach (var targetId in availableTargets)
                {
                    yield return new HeroSet(_heroRegistry.GetHero(targetId));
                }
                
                yield break;
            }

            yield return new HeroSet(availableTargets.Select(_heroRegistry.GetHero));
        }
    }
}