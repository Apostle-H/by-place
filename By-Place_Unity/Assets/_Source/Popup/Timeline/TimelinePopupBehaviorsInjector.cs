using System;
using UnityEngine;
using UnityEngine.Playables;
using Utils.Extensions;
using VContainer;

namespace Popup.Timeline
{
    public class TimelinePopupBehaviorsInjector : MonoBehaviour
    {
        [SerializeField] private PlayableDirector playableDirector;

        private bool _played;
        
        [Inject]
        private void Inject(IObjectResolver container)
        {
            foreach (var behaviour in playableDirector.GetBehaviours<PopupMessageBehaviour>())
                container.Inject(behaviour);
            
            foreach (var behaviour in playableDirector.GetBehaviours<PopupEndBehaviour>())
                container.Inject(behaviour);
        }

        private void Awake()
        {
            playableDirector.Pause();
        }

        private void Update()
        {
            if (playableDirector.state == PlayState.Playing || _played) 
                return;
            
            playableDirector.Resume();
            _played = true;
        }
    }
}