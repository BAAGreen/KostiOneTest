using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NG.Gameplay
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Vector2Int _startCords;
        [SerializeField] private Vector2Int _targetCords;
        [SerializeField] private bool _canMoveDiagonally = true;

        public Vector2Int startCords => _startCords;
        public Vector2Int targetCords => _targetCords;

        private Node _startNode;
        private Node _targetNode;
        private Node _currentNode;

        private Queue<Node> _frontier = new Queue<Node>();
        private Dictionary<Vector2Int, Node> _reached = new Dictionary<Vector2Int, Node>();

        private GridManager _gridManager;
        private Dictionary<Vector2Int, Node> _grid = new Dictionary<Vector2Int, Node>();

        private Vector2Int[] _searchOrderWithDiagonal = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down, Vector2Int.right + Vector2Int.up, Vector2Int.left + Vector2Int.up, Vector2Int.down + Vector2Int.right, Vector2Int.down + Vector2Int.left };
        private Vector2Int[] _searchOrder = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();

            if (_gridManager)
            {
                _grid = _gridManager.grid;
            }
        }

        public List<Node> GetNewPath()
        {
            return GetNewPath(_startCords);
        }

        public List<Node> GetNewPath(Vector2Int coordinates)
        {
            _gridManager.ResetNodes();
            BreadthFirstSearch(coordinates);
            return BuildPath();
        }

        private void BreadthFirstSearch(Vector2Int coordinates)
        {
            _startNode.walkable = true;
            _targetNode.walkable = true;

            _frontier.Clear();
            _reached.Clear();

            bool isRunning = true;

            _frontier.Enqueue(_grid[coordinates]);
            _reached.Add(coordinates, _grid[coordinates]);

            while (_frontier.Count > 0 && isRunning)
            {
                _currentNode = _frontier.Dequeue();
                _currentNode.explored = true;
                ExploreNeighbors();
                if (_currentNode.cords == targetCords)
                {
                    isRunning = false;
                    _currentNode.walkable = false;
                }
            }
        }

        private void ExploreNeighbors()
        {
            List<Node> neighbors = new List<Node>();
            foreach (Vector2Int direction in _canMoveDiagonally ? _searchOrderWithDiagonal : _searchOrder)
            {
                Vector2Int neighborCords = _currentNode.cords + direction;

                if (_grid.ContainsKey(neighborCords))
                {
                    neighbors.Add(_grid[neighborCords]);
                }
            }

            foreach (Node neighbor in neighbors)
            {
                if (!_reached.ContainsKey(neighbor.cords) && neighbor.walkable)
                {
                    neighbor.connectTo = _currentNode;
                    _reached.Add(neighbor.cords, neighbor);
                    _frontier.Enqueue(neighbor);
                }
            }
        }

        private List<Node> BuildPath()
        {
            List<Node> path = new List<Node>();
            Node currentNode = _targetNode;

            path.Add(currentNode);
            currentNode.path = true;

            while (currentNode.connectTo != null)
            {
                currentNode = currentNode.connectTo;
                path.Add(currentNode);
                currentNode.path = true;
            }

            path.Reverse();
            return path;
        }

        public void NotifyReceivers()
        {
            BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
        }

        public void SetNewDestination(Vector2Int startCoordinates, Vector2Int targetCoordinates)
        {
            _startCords = startCoordinates;
            _targetCords = targetCoordinates;
            _startNode = _grid[_startCords];
            _targetNode = _grid[_targetCords];
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                _canMoveDiagonally = !_canMoveDiagonally;
            }
        }
    }
}

