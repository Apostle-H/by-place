using System;
using UnityEngine;
using UnityEngine.Playables;
using Utils.Services;
using VContainer;

namespace Timeline
{
    public class TimelineFinishedSceneLoad : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;

        private SceneService _sceneService;
        
        [Inject]
        private void Inject(SceneService sceneService) => _sceneService = sceneService;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => playableDirector.stopped += ToNextScene;

        private void Expose() => playableDirector.stopped -= ToNextScene;

        private void ToNextScene(PlayableDirector director) => _sceneService.LoadScene(_sceneService.GetActiveSceneBuildIndex() + 1);
    }
}