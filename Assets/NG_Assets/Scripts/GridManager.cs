using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NG.Gameplay
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private int _unitGridSize;

        public int unitGridSize => _unitGridSize;

        private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();
        public Dictionary<Vector2Int, Node> grid => _grid;

        private void Awake()
        {
            CreateGrid();
        }

        public Node GetNode(Vector2Int coortinates)
        {
            if (grid.ContainsKey(coortinates))
            {
                return grid[coortinates];
            }
            return null;
        }

        public void BlockNode(Vector2Int coortinates)
        {
            if (grid.ContainsKey(coortinates))
            {
                grid[coortinates].walkable = false;
            }
        }

        public void ResetNodes()
        {
            foreach (KeyValuePair<Vector2Int, Node> entry in grid)
            {
                entry.Value.connectTo = null;
                entry.Value.explored = false;
                entry.Value.path = false;
            }
        }

        public Vector2Int GetCoordinatesFromPosition(Vector3 position)
        {
            Vector2Int coordinates = new Vector2Int();

            coordinates.x = Mathf.RoundToInt(position.x / unitGridSize);
            coordinates.y = Mathf.RoundToInt(position.z / unitGridSize);

            return coordinates;
        }

        public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
        {
            Vector3 position = new Vector3();

            position.x = coordinates.x * unitGridSize;
            position.z = coordinates.y * unitGridSize;

            return position;
        }

        private void CreateGrid()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector2Int cords = new Vector2Int(x, y);
                    _grid.Add(cords, new Node(cords, true));
                }
            }
        }
    }
}
