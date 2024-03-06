using UnityEngine;
using UnityEngine.UIElements;
using Utils.CustomAttributes;

namespace Dialogue.Resolve.Data
{
    [CreateAssetMenu(menuName = "SO/DS/View/Config", fileName = "NewDialogueViewConfigSO")]
    public class DialogueViewConfigSO : ScriptableObject
    {
        [field: SerializeField] public VisualTreeAsset ChoiceBtnVisualTree { get; private set; }
    }
}