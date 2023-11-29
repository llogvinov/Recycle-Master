using System.Threading.Tasks;
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

        private void Awake()
        {
            _camera = Camera.main;
            var depth = _camera!.transform.position.y;
            var halfFieldOfView = _camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            _halfHeight = depth * Mathf.Tan(halfFieldOfView);
            _halfWidth = _camera.aspect * _halfHeight;
        }

        private void Start()
        {
            SetSpawnBounds();
            AdjustAllWalls();
        }

        private static void SetSpawnBounds()
        {
            const float topWallOffset = -2f;
            const float bottomWallOffset = 1.5f;
            const float tempWallOffset = 3.5f;
            
            SpawnBounds.LeftBound = -_halfWidth;
            SpawnBounds.RightBound = _halfWidth;
            SpawnBounds.TopBound = _halfHeight + topWallOffset;
            SpawnBounds.BottomBound = -_halfHeight + bottomWallOffset;
            SpawnBounds.TempBound = -_halfHeight + tempWallOffset;
        }

        public void AdjustAllWalls()
        {
            AdjustBoundWall(_leftWall, Vector3.left, _halfWidth, _halfHeight);
            AdjustBoundWall(_rightWall, Vector3.right, _halfWidth, _halfHeight);
            AdjustBoundWall(_topWall, Vector3.forward, SpawnBounds.TopBound, _halfWidth);
            AdjustBoundWall(_bottomWall, Vector3.forward, SpawnBounds.BottomBound, _halfWidth);
            AdjustTempWall();
        
            void AdjustBoundWall(BoxCollider wallCollider, Vector3 orientation, float value, float scale)
            {
                wallCollider.transform.position = orientation * value;
                wallCollider.size = new Vector3(wallCollider.size.x, wallCollider.size.y, scale * 2);
            }
            
            void AdjustTempWall()
            {
                _tempWall.gameObject.SetActive(true);
                AdjustBoundWall(_tempWall, Vector3.forward, SpawnBounds.TempBound, _halfWidth);
            }
        }
    }
}
