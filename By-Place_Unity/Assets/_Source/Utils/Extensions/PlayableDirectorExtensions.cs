using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace Utils.Extensions
{
    public static class PlayableDirectorExtensions
    {
        public static IEnumerable<T> GetBehaviours<T>(this PlayableDirector director) where T : class, IPlayableBehaviour, new()
        {
            var behaviours = new List<T>();
            
            var playableGraph = director.playableGraph;

            if (!playableGraph.IsValid())
                return behaviours;

            for (int i = 0; i < playableGraph.GetOutputCount(); i++)
            {
                var output = playableGraph.GetOutput(i);

                if (!output.IsOutputValid() || !output.IsPlayableOutputOfType<ScriptPlayableOutput>())
                    continue;

                GetBehaviours(playableGraph, behaviours);
            }

            return behaviours;
        }

        private static void GetBehaviours<T>(this PlayableGraph playableGraph, List<T> behaviours) where T : class, IPlayableBehaviour, new()
        {
            if (!playableGraph.IsValid())
                return;

            var numOutputs = playableGraph.GetOutputCount();
            for (var i = 0; i < numOutputs; i++)
            {
                var output = playableGraph.GetOutput(i);

                if (!output.IsOutputValid() || !output.IsPlayableOutputOfType<ScriptPlayableOutput>())
                    continue;

                var sourceOutputPort = output.GetSourceOutputPort();
                var playable = output.GetSourcePlayable().GetInput(sourceOutputPort);

                GetBehaviours(playable, behaviours);
            }
        }

        private static void GetBehaviours<T>(Playable playable, List<T> behaviours) where T : class, IPlayableBehaviour, new()
        {
            if (!playable.IsValid())
                return;

            var inputCount = playable.GetInputCount();
            for (var i = 0; i < inputCount; i++)
            {
                var behaviourType = typeof(T);
                var input = playable.GetInput(i);

                if (input.GetInputCount() > 0)
                    GetBehaviours(input, behaviours);
                else if (input.GetPlayableType() == behaviourType)
                {
                    var behaviour = ((ScriptPlayable<T>)input).GetBehaviour();

                    if (!behaviours.Contains(behaviour))
                        behaviours.Add(behaviour);
                }
            }
        }
    }
}