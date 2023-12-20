using System.IO;
using DialogueSystem.Utilities;
using DialogueSystem.Utils;
using DialogueSystem.Utils.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        private const string DEFAULT_FILE_NAME = "DialoguesFileName";
        private const string DEFAULT_GRAPH_SAVE_PATH = "Assets/_Source/Editor/DialogueSystem/Graphs";
        
        private DSGraphView _graphView;

        private static TextField _fileNameTextField;
        private Button _saveButton;

        [MenuItem("Window/DS/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();

            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new DSGraphView(this);

            _graphView.StretchToParentSize();

            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            var toolbar = new Toolbar();

            _fileNameTextField = DSElementUtility.CreateTextField(DEFAULT_FILE_NAME, "File Name:", evt => 
                    _fileNameTextField.value = evt.newValue.RemoveWhitespaces().RemoveSpecialCharacters());

            _saveButton = DSElementUtility.CreateButton("Save", Save);

            var loadButton = DSElementUtility.CreateButton("Load", Load);
            var clearButton = DSElementUtility.CreateButton("Clear", Clear);

            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);

            toolbar.AddStyleSheets("DialogueSystem/Styles//DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);
        }

        private void AddStyles() => rootVisualElement.AddStyleSheets("DialogueSystem/Styles/DSVariables.uss");

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            DSIOUtility.Save(_graphView, _fileNameTextField.value);
        }

        private void Load()
        {
            var filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", DEFAULT_GRAPH_SAVE_PATH, "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            DSIOUtility.Load(_graphView, Path.GetFileNameWithoutExtension(filePath));
        }

        private void Clear() => _graphView.ClearGraph();

        public static void UpdateFileName(string newFileName) => _fileNameTextField.value = newFileName;
    }
}