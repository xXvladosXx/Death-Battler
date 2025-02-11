using CodeBase.Gameplay.Heroes;
using CodeBase.Gameplay.Skills;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public interface IUtilityFunction
    {
        string Name { get; set; }
        bool AppliesTo(BattleSkill readySkill, IHero hero);
        float GetInput(BattleSkill readySkill, IHero hero, ISkillSolver skillSolver);
        float Score(float input, IHero hero);
    }
}