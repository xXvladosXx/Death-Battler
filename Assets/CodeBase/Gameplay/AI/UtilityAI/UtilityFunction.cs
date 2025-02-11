using System;
using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class UtilityFunction : IUtilityFunction
    {
        private readonly Func<BattleSkill, IHero, bool> _appliesTo;
        private readonly Func<BattleSkill, IHero, ISkillSolver, float> _getInput;
        private readonly Func<float, IHero, float> _score;
        public string Name { get; set; }

        public UtilityFunction(Func<BattleSkill, IHero, bool> appliesTo,
            Func<BattleSkill, IHero, ISkillSolver, float> getInput,
            Func<float, IHero, float> score,
            string name)
        {
            _appliesTo = appliesTo;
            _getInput = getInput;
            _score = score;
            Name = name;
        }
        
        public bool AppliesTo(BattleSkill readySkill, IHero hero) => _appliesTo(readySkill, hero);

        public float GetInput(BattleSkill readySkill, IHero hero, ISkillSolver skillSolver) => _getInput(readySkill, hero, skillSolver);

        public float Score(float input, IHero hero) => _score(input, hero);
    }
}