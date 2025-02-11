using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.Battle;
using CodeBase.Gameplay.Heroes;
using CodeBase.StaticData.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class ScoredAction : HeroAction
    {
        public float Score;

        public ScoredAction(IHero readyHero, BattleSkill readySkill,
            IEnumerable<IHero> targets, float score)
        {
            Caster = readyHero;
            Skill = readySkill.TypeId;
            SkillKind = readySkill.Kind;
            TargetIds = targets.Select(x => x.Id).ToList();
            Score = score;
        }

        public override string ToString()
        {
            var skillCategory = SkillKind switch
            {
                SkillKind.Damage => "dmg",
                SkillKind.Heal => "heal",
                SkillKind.InitiativeBurn => "initiative burn",
                SkillKind.Unknown => "unknown",
                _ => throw new ArgumentOutOfRangeException()
            };

            return $"{skillCategory}: {Skill} targets: {TargetIds.Count} score: {Score:0.00}";
        }
    }
}