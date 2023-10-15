using System.Collections;
using UnityEngine;

namespace Main
{
    public class WallAdjuster : MonoBehaviour
    {
        [SerializeField] private BoxCollider _leftWall;
        [SerializeField] private BoxCollider _rightWall;
        [SerializeField] private BoxCollider _topWall;
        [SerializeField] private BoxCollider _bottomWall;
        [Space] 
        [SerializeField] private BoxCollider _tempWall;
    
        private Camera _camera;
        private static float _halfHeight;
        private static float _halfWidth;
        private const float Delay = 1f;

        private void Awake()
        {
            _camera = Camera.main;

            float depth = _camera.transform.position.y;
            float halfFieldOfView = _camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            _halfHeight = depth * Mathf.Tan(halfFieldOfView);
            _halfWidth = _camera.aspect * _halfHeight;

            AdjustWalls();

            StartCoroutine(DisableTempWall());
        }

        private IEnumerator DisableTempWall()
        {
            yield return new WaitForSeconds(Delay);
            _tempWall.gameObject.SetActive(false);
        }

        private void AdjustWalls()
        {
            AdjustWall(_leftWall, Vector3.left, _halfWidth, _halfHeight);
            AdjustWall(_rightWall, Vector3.right, _halfWidth, _halfHeight);
            AdjustWall(_topWall, Vector3.forward, _halfHeight, _halfWidth);
            AdjustWall(_bottomWall, Vector3.back, _halfHeight, _halfWidth);
        
            _tempWall.size = new Vector3(_tempWall.size.x, _tempWall.size.y, _halfWidth * 2);
        
            void AdjustWall(BoxCollider wallCollider, Vector3 orientation, float value, float scale)
            {
                wallCollider.transform.position = orientation * value;
                wallCollider.size = new Vector3(wallCollider.size.x, wallCollider.size.y, scale * 2);
            }
        }

    }
}
