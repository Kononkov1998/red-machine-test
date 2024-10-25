using Connection;
using Events;
using UnityEngine;
using Utils.MonoBehaviourUtils;

namespace Camera
{
    public class CameraLevelBounds : GameMonoBehaviour
    {
        [SerializeField] public float extraBorderSpace = 1f;
        [SerializeField] private ColorNodesContainer colorNodesContainer;

        private void Start()
        {
            CalculateBounds(CameraHolder.Instance.MainCamera);
        }

        private void CalculateBounds(UnityEngine.Camera cam)
        {
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = cameraHeight * cam.aspect;

            Vector3 screenBottomLeft = new Vector3(-cameraWidth * 0.5f, -cameraHeight * 0.5f, 0) + new Vector3(extraBorderSpace, extraBorderSpace, 0);
            Vector3 screenTopRight = new Vector3(cameraWidth * 0.5f, cameraHeight * 0.5f, 0) - new Vector3(extraBorderSpace, extraBorderSpace, 0);

            Vector3 bottomLeft = Vector3.zero;
            Vector3 bottomRight = Vector3.zero;

            var nodesBounds = new Bounds();
            foreach (ColorNode node in colorNodesContainer.Nodes)
                nodesBounds.Encapsulate(node.Bounds);

            if (nodesBounds.min.x < screenBottomLeft.x)
            {
                float offset = screenBottomLeft.x - nodesBounds.min.x;
                bottomLeft.x -= offset;
            }

            if (nodesBounds.max.x > screenTopRight.x)
            {
                float offset = nodesBounds.max.x - screenTopRight.x;
                bottomRight.x += offset;
            }

            if (nodesBounds.min.y < screenBottomLeft.y)
            {
                float offset = screenBottomLeft.y - nodesBounds.min.y;
                bottomLeft.y -= offset;
            }

            if (nodesBounds.max.y > screenTopRight.y)
            {
                float offset = nodesBounds.max.y - screenTopRight.y;
                bottomRight.y += offset;
            }

            var levelBounds = new Bounds();
            levelBounds.SetMinMax(bottomLeft, bottomRight);
            EventsController.Fire(new EventModels.Game.CameraBoundsRecalculated { Bounds = levelBounds });
        }
    }
}