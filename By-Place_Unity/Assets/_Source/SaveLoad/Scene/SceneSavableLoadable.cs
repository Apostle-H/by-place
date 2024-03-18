using System;
using SaveLoad.Json.Data;
using SaveLoad.Scene.Data;
using UnityEngine;
using VContainer;

namespace SaveLoad.Scene
{
    public class SceneSavableLoadable : MonoBehaviour, ISavableLoadable<SceneSave>
    {
        [field: SerializeField] public string Path { get; private set; }
        
        private SceneSavableLoadableCollector _sceneSavableLoadableCollector;

        [Inject]
        public void Inject(SceneSavableLoadableCollector sceneSavableLoadableCollector) => 
            _sceneSavableLoadableCollector = sceneSavableLoadableCollector;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => _sceneSavableLoadableCollector.Register(this);

        private void Expose() => _sceneSavableLoadableCollector.Unregister(this);

        public SceneSave GetSaveData()
        {
            var position = transform.position;
            var rotation = transform.rotation;
            var jsonPosition = new JsonVector4(position.x, position.y, position.z, 0f);
            var jsonRotation = new JsonVector4(rotation.x, rotation.y, rotation.z, rotation.w);
            return new SceneSave(gameObject.activeInHierarchy, jsonPosition, jsonRotation);
        }

        public void LoadSaveData(SceneSave saveData)
        {
            gameObject.SetActive(saveData.Active);
            
            transform.position = saveData.Position.GetUnityVector();
            transform.rotation = saveData.Rotation.GetUnityQuaternion();
        }
    }
}