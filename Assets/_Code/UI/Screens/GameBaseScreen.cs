using UnityEngine;

namespace UI.Screens
{
    public abstract class GameBaseScreen : MonoBehaviour
    {
        [SerializeField] 
        protected GameObject _root;

        protected virtual void Toogle(bool state) { _root.SetActive(state); }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_root == null) _root = transform.Find("Root").gameObject;
        }
#endif
    }
}