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
        
        public static float HalfHeight;
        public static float HalfWidth;
        public static float TempWallHeight = -2f;
        
        private const float Delay = 1f;

        private void Awake()
        {
            _camera = Camera.main;

            float depth = _camera.transform.position.y;
            float halfFieldOfView = _camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            HalfHeight = depth * Mathf.Tan(halfFieldOfView);
            HalfWidth = _camera.aspect * HalfHeight;

            AdjustAllWalls();

            // todo: replace with callback on every spawner initialized
            StartCoroutine(DisableTempWall());
        }

        private IEnumerator DisableTempWall()
        {
            yield return new WaitForSeconds(Delay);
            _tempWall.gameObject.SetActive(false);
        }

        private void AdjustAllWalls()
        {
            AdjustBoundWall(_leftWall, Vector3.left, HalfWidth, HalfHeight);
            AdjustBoundWall(_rightWall, Vector3.right, HalfWidth, HalfHeight);
            AdjustBoundWall(_topWall, Vector3.forward, HalfHeight, HalfWidth);
            AdjustBoundWall(_bottomWall, Vector3.back, HalfHeight, HalfWidth);

            AdjustTempWall();

            _tempWall.size = new Vector3(_tempWall.size.x, _tempWall.size.y, HalfWidth * 2);
        
            void AdjustBoundWall(BoxCollider wallCollider, Vector3 orientation, float value, float scale)
            {
                wallCollider.transform.position = orientation * value;
                wallCollider.size = new Vector3(wallCollider.size.x, wallCollider.size.y, scale * 2);
            }
            
            void AdjustTempWall() => 
                AdjustBoundWall(_tempWall, Vector3.forward, TempWallHeight, HalfWidth);
        }
    }
}
