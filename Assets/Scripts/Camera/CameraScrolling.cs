using Events;
using Player;
using Player.ActionHandlers;
using UnityEngine;

namespace Camera
{
    public class CameraScrolling : MonoBehaviour
    {
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float scrollSpeed = 10f;

        private UnityEngine.Camera _mainCamera;
        private ClickHandler _clickHandler;

        private bool _scrolling;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _lastMousePosition;
        private Bounds _bounds;

        private void Start()
        {
            _mainCamera = CameraHolder.Instance.MainCamera;
            _clickHandler = ClickHandler.Instance;
            _clickHandler.DragStartEvent += OnDragStart;
            _clickHandler.DragEndEvent += OnDragEnd;
            EventsController.Subscribe<EventModels.Game.CameraBoundsRecalculated>(this, OnNodesBoundsRecalculated);
        }

        private void OnDestroy()
        {
            _clickHandler.DragStartEvent -= OnDragStart;
            _clickHandler.DragEndEvent -= OnDragEnd;
            EventsController.Unsubscribe<EventModels.Game.CameraBoundsRecalculated>(OnNodesBoundsRecalculated);
        }

        private void Update()
        {
            if (!_scrolling)
                return;

            Vector3 currentMousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = _lastMousePosition - currentMousePosition;

            Vector3 mainCameraPosition = _mainCamera.transform.position;
            Vector3 targetPosition = mainCameraPosition + new Vector3(delta.x * scrollSpeed, delta.y * scrollSpeed, 0);
            targetPosition.x = Mathf.Clamp(targetPosition.x, _bounds.min.x, _bounds.max.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, _bounds.min.y, _bounds.max.y);
            _mainCamera.transform.position = Vector3.SmoothDamp(mainCameraPosition, targetPosition, ref _velocity, smoothTime);

            _lastMousePosition = currentMousePosition;
        }

        private void OnDragStart(Vector3 startPosition)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;

            _scrolling = true;
            _lastMousePosition = startPosition;
        }
        
        private void OnDragEnd(Vector3 endPosition)
        {
            if (PlayerController.PlayerState != PlayerState.Scrolling)
                return;

            _scrolling = false;
        }

        private void OnNodesBoundsRecalculated(EventModels.Game.CameraBoundsRecalculated boundsRecalculated)
        {
            _bounds = boundsRecalculated.Bounds;
            ResetPosition();
        }

        private void ResetPosition()
        {
            transform.position = new Vector3(0f, 0f, transform.position.z);
        }
    }
}