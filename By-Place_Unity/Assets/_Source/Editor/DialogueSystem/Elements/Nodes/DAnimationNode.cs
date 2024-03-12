using Animate.Data;
using Dialogue.Data.NodeParams;
using Dialogue.Data.Save.Nodes;
using DialogueSystem.Windows;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DialogueSystem.Elements.Nodes
{
    public class DAnimationNode : DNode
    {
        public AAnimatableLink AnimatableLink { get; set; }
        public AnimationClip Animation { get; set; }

        private ObjectField _animatableLinkField;
        private ObjectField _animationField;
        
        public DAnimationNode(Vector2 position, int guid = -1) : base(position, guid)
        {
            title = "Animation";
            NextGuids.Add(new DOutputData());
        }
        
        public override DNode NewAt(Vector2 position) => new DAnimationNode(position);

        public override void Draw()
        {
            base.Draw();
            
            // MAIN CONTAINER

            _animatableLinkField = new ObjectField()
            {
                objectType = typeof(AAnimatableLink),
                value = AnimatableLink
            };
            _animatableLinkField.RegisterValueChangedCallback(evt => 
                UpdateAnimatableLink((AAnimatableLink)evt.newValue));

            _animationField = new ObjectField()
            {
                objectType = typeof(AnimationClip),
                value = Animation
            };
            _animationField.RegisterValueChangedCallback(evt => UpdateAnimation((AnimationClip)evt.newValue));
            
            mainContainer.Insert(1, _animatableLinkField);
            mainContainer.Insert(2, _animationField);
            
            /* OUTPUT CONTAINER */
            
            var outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            outputPort.portName = DEFAULT_OUTPUT_PORT_NAME;
            outputPort.userData = NextGuids[^1];
            
            outputContainer.Add(outputPort);
            
            RefreshExpandedState();
        }

        private void UpdateAnimatableLink(AAnimatableLink value) => AnimatableLink = value;

        private void UpdateAnimation(AnimationClip value) => Animation = value;
    }
}