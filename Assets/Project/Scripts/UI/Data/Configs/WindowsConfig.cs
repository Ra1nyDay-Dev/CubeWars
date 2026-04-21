using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.UI.Data.Configs
{
  [CreateAssetMenu(fileName = "WindowsConfig", menuName = "Configs/UI/Windows Config")]
  public class WindowsConfig : ScriptableObject
  {
    public List<WindowConfig> WindowConfigs;
  }
}