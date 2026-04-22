using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.Windows.Gameplay
{
    public class WeaponAmmoView : MonoBehaviour
    {
        [SerializeField] TMP_Text _ammoText;
        
        private string _infiniteAmmoReserveText = "<size=70%> /</size><size=110%>∞</size>";

        public void UpdateText(int currentAmmo, int reservedAmmo, bool infiniteAmmo)
        {
            string ammoText = currentAmmo.ToString();
            ammoText += infiniteAmmo ? _infiniteAmmoReserveText : $"<size=70%> /{reservedAmmo}</size>";
            _ammoText.text = ammoText;
        }
    }
}