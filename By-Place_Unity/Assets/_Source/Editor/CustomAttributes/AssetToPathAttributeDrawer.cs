using Journal.Quest.View;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Utils.CustomAttributes;
using Utils.Extensions;
using Utils.Services;

namespace CustomAttributes
{
    [CustomPropertyDrawer(typeof(AssetToPathAttribute))]
    public class AssetToPathAttributeDrawer : PropertyDrawer
    {
        private AssetToPathAttribute _castedAttribute;

        private SerializedProperty _property;
        private Object _targetObject;
        
        private ObjectField _assetField;
        
        private bool _isBind;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (fieldInfo.FieldType != typeof(string))
                return base.CreatePropertyGUI(property);

            _property = property;
            _castedAttribute = (AssetToPathAttribute)attribute;
            _targetObject = property.serializedObject.targetObject;

            _assetField = new ObjectField()
            {
                label = fieldInfo.Name[1..^">k__BackingField".Length].CamelCaseToUpperCaseWithSpaces(),
                objectType = _castedAttribute.TargetType
            };
            
            Bind();
            return _assetField;
        }

        private void Bind()
        {
            if (_isBind)
                return;
            
            _assetField.value = Resources.Load(_property.stringValue);
            
            _assetField.RegisterValueChangedCallback(SetAssetPath);
            Selection.selectionChanged += Expose;

            _isBind = true; 
        }

        private void Expose()
        {
            if (!_isBind || (Selection.objects.Length == 1 && Selection.objects[0] == _targetObject))
                return;
            
            _assetField.UnregisterValueChangedCallback(SetAssetPath);
            Selection.selectionChanged -= Expose;

            _isBind = false;
        }

        private void SetAssetPath(ChangeEvent<Object> evt)
        {
            if (!AssetsService.PathToResources(evt.newValue.GetInstanceID(), out var path))
            {
                Debug.LogWarning("Chosen Asset is not a Resource");
                _assetField.value = evt.previousValue;
                return;
            }
            
            _property.stringValue = path;
            _property.serializedObject.ApplyModifiedProperties();
        }
    }
}