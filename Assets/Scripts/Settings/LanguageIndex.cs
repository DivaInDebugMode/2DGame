using UnityEngine;

namespace Settings
{
    public class LanguageIndex : MonoBehaviour
    {
        [SerializeField] private int index;

        public int Index
        {
            get => index;
            set => index = value;
        }
    }
}