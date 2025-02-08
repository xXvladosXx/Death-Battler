using TMPro;
using UnityEngine;

namespace CodeBase.Gameplay.UI
{
  public class BattleText : MonoBehaviour
  {
    public TextMeshProUGUI Text;

    public void Initialize(string text, Color color)
    {
      Text.text = text;
      Text.color = color;
    }
  }
}