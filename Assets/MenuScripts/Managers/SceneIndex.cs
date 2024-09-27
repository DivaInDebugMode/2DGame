using UnityEngine;

namespace Managers
{
    public class SceneIndex : MonoBehaviour
    {
        [SerializeField] private int index;

        public int Index
        {
            get => index;
            set => index = value;
        }
    }
}
