# Death Battler

**Death Battler** is a Unity demo project that showcases a cutting-edge Utility AI system designed to be easily modified and extended. This project demonstrates how to integrate a flexible, utility-based AI into games, allowing for dynamic and adaptive enemy behavior.

![DeathBattler](https://github.com/user-attachments/assets/9c278162-1734-45b1-87d2-6e4620ca9cf6)

## Key Features

- **Modular Utility AI:**  
  The project’s standout feature is its flexible Utility AI framework. It evaluates actions based on weighted criteria, making it simple to adjust and extend the decision-making process to suit different gameplay scenarios.

- **Dynamic Enemy Behavior:**  
  Watch as AI-driven opponents adapt their strategies in real-time based on the game state, providing a challenging and engaging combat experience.

- **Ease of Customization:**  
  With a design focused on modifiability, developers can quickly tweak the AI without needing to overhaul complex systems. This makes it an ideal starting point for both beginners and experienced developers interested in advanced AI concepts.

## Utility AI Code Overview

At the heart of **Death Battler** is a modular AI system composed of several interconnected components:

- **Brains Class:**  
  Aggregates a collection of utility functions (stored in a custom `Convolutions` list). Each utility function consists of:
  - **Condition (`appliesTo`):** Determines if a skill meets a specific criterion.
  - **Input Function (`getInput`):** Computes a value based on the battle skill and target.
  - **Scoring Function (`score`):** Transforms the computed value into a final utility score.
  - **Name:** A descriptive label for clarity and debugging.

```csharp
public class Brains
{
    private readonly Convolutions _convolutions = new Convolutions()
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
```

- **Condition Functions:**  
  Defined in the `When` static class, these methods check if a battle skill meets certain criteria:
  - **`SkillIsDamage`:** Confirms the skill is of the damage type.
  - **`SkillIsBasicAttack`:** Determines if the skill is a basic attack (damage with zero cooldown).
  - **`SkillIsHeal`:** Verifies if the skill is intended for healing.
  - **`SkillIsInitiativeBurn`:** Checks if the skill is meant to burn initiative.

```csharp
public static class When
{
    public static bool SkillIsDamage(BattleSkill skill, IHero hero) => 
        skill.Kind == SkillKind.Damage;
    
    public static bool SkillIsBasicAttack(BattleSkill skill, IHero hero) => 
        skill.Kind == SkillKind.Damage && skill.MaxCooldown == 0;
    
    public static bool SkillIsHeal(BattleSkill skill, IHero hero) => 
        skill.Kind == SkillKind.Heal;
    
    public static bool SkillIsInitiativeBurn(BattleSkill skill, IHero hero) => 
        skill.Kind == SkillKind.InitiativeBurn;
}
```

- **Input Calculators:**  
  The `GetInput` static class computes various values that help determine the priority of each action:
  - **`PercentageDamage`:** Damage as a percentage of the target’s maximum HP.
  - **`KillingBlow`:** Checks if the calculated damage is sufficient to defeat the target.
  - **`HealPercentage`:** Assesses the effectiveness of a healing skill.
  - **`HpPercentage`:** Computes the current HP percentage of the target.
  - **`InitiativeBurn`:** Calculates the initiative burn as a fraction of the target's max initiative.
  - **`TargetUltimateIsReady`:** Determines if the target's ultimate ability is ready.

```csharp
public static class GetInput
{
    private const int TRUE = 1;
    private const int FALSE = 0;

    public static float PercentageDamage(BattleSkill skill, IHero target, ISkillSolver skillSolver)
    {
        var damage = PotentialDamage(skill, target, skillSolver);
        return damage / target.State.MaxHp;
    }

    public static float KillingBlow(BattleSkill skill, IHero target, ISkillSolver skillSolver)
    {
        var damage = PercentageDamage(skill, target, skillSolver);
        return damage > target.State.CurrentHp ? TRUE : FALSE;
    }

    public static float HealPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
        skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);

    public static float HpPercentage(BattleSkill skill, IHero target, ISkillSolver skillSolver) =>
        target.State.CurrentHp / target.State.MaxHp;

    public static float InitiativeBurn(BattleSkill skill, IHero target, ISkillSolver skillSolver)
    {
        float burn = skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);
        return burn / target.State.MaxInitiative;
    }

    public static float TargetUltimateIsReady(BattleSkill skill, IHero target, ISkillSolver skillSolver) =>
        target.State.SkillStates.Last().IsReady ? TRUE : FALSE;

    private static float PotentialDamage(BattleSkill skill, IHero target, ISkillSolver skillSolver) => 
        skillSolver.CalculateSkillValue(skill.CasterId, skill.TypeId, target.Id);
}
```

- **Scoring Functions:**  
  The `Score` static class transforms the raw inputs into final utility scores through:
  - **`AsIs`:** Returns the input value unmodified.
  - **`ScaleBy`:** Multiplies the input by a constant factor.
  - **`IfTrueThen`:** Applies a bonus multiplier if a condition is met.
  - **`FocusTargetBasedOnHp`:** Adjusts the score based on the target’s HP percentage.
  - **`CullByTargetHp`:** Penalizes healing when the target's HP is high.
  - **`CullByTargetInitiative`:** Modifies the score based on the target’s initiative percentage.

```csharp
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
```

- **Utility Function Aggregation:**  
  The `Convolutions` class is a custom list that streamlines adding new utility functions by encapsulating:
  - The condition function.
  - The input calculation function.
  - The scoring function.
  - A descriptive name.

```csharp
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
```

