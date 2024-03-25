using UnityEngine;
using UnityEngine.UIElements;

namespace Popup.Data
{
    [CreateAssetMenu(menuName = "SO/Popup/ConfigSO", fileName = "NewPopupConfigSO")]
    public class PopupConfigSO : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset PopupBoxTreeAsset { get; private set; }
    }
}