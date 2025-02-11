using System;
using System.Collections.Generic;
using CodeBase.Gameplay.AI.UtilityAI;
using CodeBase.Gameplay.Heroes;

namespace CodeBase.Gameplay.AI.Reporting
{
  public interface IAIReporter
  {
    event Action<DecisionDetails> DecisionDetailsReported;
    event Action<DecisionScores> DecisionScoresReported;
    void ReportDecisionDetails(BattleSkill skill, IHero target, List<ScoreFactor> scoreFactors);
    void ReportDecisionScores(IHero readyHero, List<ScoredAction> choices);
  }
}