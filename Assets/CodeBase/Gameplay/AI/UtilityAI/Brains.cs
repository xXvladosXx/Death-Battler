using System.Collections.Generic;
using CodeBase.Gameplay.AI.UtilityAI.Calculations;

namespace CodeBase.Gameplay.AI.UtilityAI
{
    public class Brains
    {
        private Convolutions _convolutions = new Convolutions()
        {
            {When.SkillIsDamage, GetInput.PercentageDamage, Score.ScaleBy(100), "Basic Damage"},
            {When.SkillIsDamage, GetInput.KillingBlow, Score.IfTrueThen(+150), "Killing Blow"},
            {When.SkillIsBasicAttack, GetInput.KillingBlow, Score.IfTrueThen(+30), "Killing Blow with Basic Attack"},
            {When.SkillIsDamage, GetInput.HpPercentage, Score.FocusTargetBasedOnHp, "Focus Damage"},
            
            {When.SkillIsHeal, GetInput.HealPercentage, Score.CullByTargetHp, "Heal"},
            
            {When.SkillIsInitiativeBurn, GetInput.InitiativeBurn, Score.CullByTargetInitiative(50, .25f), "Initiative Burn"},
            {When.SkillIsInitiativeBurn, GetInput.TargetUltimateIsReady, Score.IfTrueThen(+30), "Initiative Burn (Ultimate is ready)"},
        };
        
        public IEnumerable<IUtilityFunction> GetUtilityFunctions()
        {
            return _convolutions;
        }
    }
}