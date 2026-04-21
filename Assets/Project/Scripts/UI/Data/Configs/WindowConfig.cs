using System;
using Project.Scripts.UI.Windows;
using UnityEngine;

namespace Project.Scripts.UI.Data.Configs
{
  [Serializable]
  public class WindowConfig
  {
    public WindowId Id;
    public GameObject Prefab;
  }
}