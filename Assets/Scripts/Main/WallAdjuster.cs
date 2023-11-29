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
        private static float HalfHeight;
        private static float HalfWidth;

        public static float[] SpawnBounds;
        private float _topWallHeight;
        private float _bottomWallHeight;
        private float _tempWallHeight;

        private const float DelayInSeconds = 2f;

        private void Awake()
        {
            _camera = Camera.main;
            var depth = _camera.transform.position.y;
            var halfFieldOfView = _camera.fieldOfView * 0.5f * Mathf.Deg2Rad;
            HalfHeight = depth * Mathf.Tan(halfFieldOfView);
            HalfWidth = _camera.aspect * HalfHeight;
            
            _topWallHeight = HalfHeight - 1.5f;
            _bottomWallHeight = -HalfHeight + 1.5f;
            _tempWallHeight = -HalfHeight + 3.5f;
            SpawnBounds = new[] {-HalfWidth, _topWallHeight, HalfWidth, _tempWallHeight};
        }

        private void Start()
        {
            AdjustAllWalls();

            LevelCreator.AllObjectSpawned += DisableTempWall;
        }

        private void OnDestroy()
        {
            LevelCreator.AllObjectSpawned -= DisableTempWall;
        }

        public void AdjustAllWalls()
        {
            AdjustBoundWall(_leftWall, Vector3.left, HalfWidth, HalfHeight);
            AdjustBoundWall(_rightWall, Vector3.right, HalfWidth, HalfHeight);
            AdjustBoundWall(_topWall, Vector3.forward, _topWallHeight, HalfWidth);
            AdjustBoundWall(_bottomWall, Vector3.forward, _bottomWallHeight, HalfWidth);
            AdjustTempWall();
        
            void AdjustBoundWall(BoxCollider wallCollider, Vector3 orientation, float value, float scale)
            {
                wallCollider.transform.position = orientation * value;
                wallCollider.size = new Vector3(wallCollider.size.x, wallCollider.size.y, scale * 2);
            }
            
            void AdjustTempWall()
            {
                _tempWall.gameObject.SetActive(true);
                AdjustBoundWall(_tempWall, Vector3.forward, _tempWallHeight, HalfWidth);
            }
        }

        private async void DisableTempWall()
        {
            await Task.Delay((int)(DelayInSeconds * 1000));
            //_tempWall.gameObject.SetActive(false);
        }
    }
}
