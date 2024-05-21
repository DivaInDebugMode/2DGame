using UnityEngine;

namespace Save_Load
{
    public class GameIndex : MonoBehaviour
    {
        [SerializeField] private int index;
    
        public int Index
        {
            get => index;
            set => index = value;
        }
    }
}