using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Assets
{
    public class SelfResourceReleaser : MonoBehaviour
    {
        private void OnDestroy()
        {
            Addressables.Release(gameObject);
        }
    }
}
