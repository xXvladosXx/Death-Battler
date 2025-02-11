using System;
using System.Collections.Generic;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class Convolutions : List<UtilityFunction>
    {
        public void Add(Func<BattleSkill, IHero, bool> appliesTo,
            Func<BattleSkill, IHero, ISkillSolver, float> getInput,
            Func<float, IHero, float> score,
            string name)
        {
            Add(new UtilityFunction(appliesTo, getInput, score, name));
        }
    }
}