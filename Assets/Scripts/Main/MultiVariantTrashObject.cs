using UnityEngine;

namespace Main
{
    public class MultiVariantTrashObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] _meshes;

        public void ChooseRandomMesh()
        {
            if (_meshes == null) return;
            
            var meshIndex = Random.Range(0, _meshes.Length);

            for (var i = 0; i < _meshes.Length; i++)
                _meshes[i].SetActive(i == meshIndex);
        }
    }
}