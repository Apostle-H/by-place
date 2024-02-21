using System;
using System.IO;
using DialogueSystem.Data;
using DialogueSystem.Utilities;
using DialogueSystem.Utils;
using DialogueSystem.Utils.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace DialogueSystem.Windows
{
    public class DGEditorWindow : EditorWindow
    {
        private const string DEFAULT_FILE_NAME = "DialoguesFileName";
        private const string DEFAULT_GRAPH_SAVE_PATH = "Assets/_Source/Editor/DialogueSystem/Graphs";
        
        private DSGraphView _graphView;

        private static DEContainerSO _targetGraph;

        private TextField _fileName;
        private Button _saveBtn;
        private Button _loadBtn;
        private Button _clearBtn;

        [MenuItem("Window/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DGEditorWindow>("Dialogue Graph");
            _targetGraph = default;
        }

        public static void OpenGraph(DEContainerSO graph)
        {
            GetWindow<DGEditorWindow>($"Dialogue Graph {graph.FileName}");
            _targetGraph = graph;
        }
        
        private void OnEnable()
        {
            DGSaveLoad.EditorWindow = this;
            
            AddGraphView();
            AddToolbar();

            AddStyles();
            
            Bind();
        }

        private void OnDisable() => Expose();

        private void AddGraphView()
        {
            _graphView = new DSGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);

            if (_targetGraph == default)
                return;
            
            DGSaveLoad.Load(_graphView, _targetGraph.FileName);
        }

        private void AddToolbar()
        {
            var toolbar = new Toolbar();
            
            _fileName = new TextField()
            {
                value = DEFAULT_FILE_NAME,
                label = "File Name:"
            };
            _saveBtn = new Button() { text = "Save" };
            _loadBtn = new Button() { text = "Load" };
            _clearBtn = new Button() { text = "Clear" };

            toolbar.Add(_fileName);
            toolbar.Add(_saveBtn);
            toolbar.Add(_loadBtn);
            toolbar.Add(_clearBtn);

            toolbar.AddStyleSheets("DialogueSystem/Styles//DSToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }

        private void AddStyles() => rootVisualElement.AddStyleSheets("DialogueSystem/Styles/DSVariables.uss");

        private void Bind()
        {
            _fileName.RegisterValueChangedCallback(UpdateFileName);
            _saveBtn.clicked += Save;
            _loadBtn.clicked += Load;
            _clearBtn.clicked += Clear;
        }

        private void Expose()
        {
            _fileName.UnregisterValueChangedCallback(UpdateFileName);
            _saveBtn.clicked -= Save;
            _loadBtn.clicked -= Load;
            _clearBtn.clicked -= Clear;
        }

        private void UpdateFileName(ChangeEvent<string> evt)
        {
            var newName = evt.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            _fileName.value = newName;
            titleContent.text = $"Dialogue Graph {newName}";
        }

        public void UpdateFileName(string newName)
        {
            _fileName.value = newName;
            titleContent.text = $"Dialogue Graph {newName}";
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileName.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Ok");
                return;
            }
            
            DGSaveLoad.Save(_graphView, _fileName.value);
        }

        private void Load()
        {
            var filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", DEFAULT_GRAPH_SAVE_PATH, "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();
            DGSaveLoad.Load(_graphView, Path.GetFileNameWithoutExtension(filePath));
        }

        private void Clear() => _graphView.ClearGraph();
    }
}