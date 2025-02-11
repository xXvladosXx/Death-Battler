using System.Collections.Generic;
using CodeBase.Gameplay.Heroes;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class HeroSet
    {
        public IEnumerable<IHero> Targets;

        public HeroSet(IHero target)
        {
            Targets = new[] {target};
        }

        public HeroSet(IEnumerable<IHero> targets)
        {
            Targets = targets;
        }
    }
}