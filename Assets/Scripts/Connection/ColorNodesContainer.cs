using Utils.MonoBehaviourUtils;

namespace Connection
{
    public class ColorNodesContainer : GameMonoBehaviour
    {
        public ColorNode[] Nodes { get; private set; }
        public ColorNodeTarget[] NodeTargets { get; private set; }

        private void Awake()
        {
            Nodes = GetComponentsInChildren<ColorNode>();
            NodeTargets = GetComponentsInChildren<ColorNodeTarget>(true);
        }
    }
}