using UnityEngine;

namespace Utils.Services
{
    public static class PhysicsService
    {
        public static bool RayCastFromCamera(Vector2 screenPoint, out RaycastHit hit)
        {
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            return Physics.Raycast(ray, out hit, 100f);
        }
        
        public static bool RayCastFromCamera(Vector2 screenPoint, LayerMask mask, out RaycastHit hit)
        {
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            return Physics.Raycast(ray, out hit, 100f, mask);
        }
    }
}