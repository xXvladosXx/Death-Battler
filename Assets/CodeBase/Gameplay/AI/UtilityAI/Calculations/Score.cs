using System;
using CodeBase.Gameplay.Heroes;

namespace CodeBase.Gameplay.AI.UtilityAI.Calculations
{
    public static class Score
    {
        public static float AsIs(float input, IHero hero) => input;
        public static Func<float, IHero, float> ScaleBy(float scale) => (input, _) => input * scale;
        public static Func<float, IHero, float> IfTrueThen(float ifTrue) => (input, _) => input * ifTrue;

        public static float FocusTargetBasedOnHp(float hpPercentage, IHero target) => (1 - hpPercentage) * 50;

        public static float CullByTargetHp(float healPercentage, IHero target)
        {
            if (target.State.HpPercentage >= .7f)
                return -30;

            return 100 * (healPercentage + 3 * (.7f - target.State.HpPercentage));
        }

        public static Func<float, IHero, float> CullByTargetInitiative(float scaleBy, float cullThreshold)
        {
            return (input, target) => target.State.InitiativePercentage > cullThreshold
                ? input * scaleBy
                : 0;
        }
    }
}