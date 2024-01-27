
using UnityEngine;

namespace Navigation.Location
{
    public class BasicLocation : ALocation
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform cameraPosition;

        public override void Enter()
        {
            targetCamera.transform.position = cameraPosition.position;
            targetCamera.transform.rotation = cameraPosition.rotation;
        }

        public override void Exit()
        {
            
        }
    }
}