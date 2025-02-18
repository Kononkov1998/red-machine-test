using Events;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviour
    {
        private const string LevelNamePattern = "Level{0}";

        private int _currentLevelIndex;


        private void Start()
        {
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));

            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        public void GoToLevel(int index)
        {
            _currentLevelIndex = index;
            ScenesChanger.GotoScene(string.Format(LevelNamePattern, index));
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            GoToLevel(_currentLevelIndex + 1);
        }
    }
}