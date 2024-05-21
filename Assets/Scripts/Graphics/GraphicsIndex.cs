using UnityEngine;

namespace Graphics
{
    public class GraphicsIndex : MonoBehaviour
    {
        [SerializeField] private int index;

        public int Index
        {
            get => index;
            set => index = value;
        }
    }
}