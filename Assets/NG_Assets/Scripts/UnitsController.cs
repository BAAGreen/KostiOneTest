using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NG.Gameplay
{
    public class UnitsController : MonoBehaviour
    {
        private GridManager _gridManager;
        private Pathfinding _pathFinder;

        private Unit _selectedUnit;
        private bool _unitSelected = false;
        private Camera _mainCamera;

        void Start()
        {
            _gridManager = FindObjectOfType<GridManager>();
            _pathFinder = FindObjectOfType<Pathfinding>();
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                bool hasHit = Physics.Raycast(ray, out hit);

                if (hasHit)
                {
                    if (hit.transform.tag == "Tile")
                    {
                        if (_unitSelected)
                        {
                            Vector2Int targetCords = _gridManager.GetCoordinatesFromPosition(hit.point);
                            if (!_gridManager.GetNode(targetCords).walkable) return;
                            Vector2Int startCords = _gridManager.GetCoordinatesFromPosition(_selectedUnit.transform.position);
                            _pathFinder.SetNewDestination(startCords, targetCords);
                            RecalculatePath(true);

                            //_selectedUnit.transform.position = new Vector3(targetCords.x, _selectedUnit.transform.position.y, targetCords.y);
                        }
                    }

                    if (hit.transform.tag == "Unit")
                    {
                        _selectedUnit = hit.transform.GetComponent<Unit>();
                        _unitSelected = true;
                    }
                }
            }
        }

        private void RecalculatePath(bool resetPath)
        {
            Vector2Int coordinates = new Vector2Int();
            if (resetPath)
            {
                coordinates = _pathFinder.startCords;
            }
            else 
            {
                coordinates = _gridManager.GetCoordinatesFromPosition(_selectedUnit.transform.position);
            }

            _selectedUnit.StopFollowing();
            _selectedUnit.path.Clear();
            _selectedUnit.path = _pathFinder.GetNewPath(coordinates);
            _selectedUnit.FollowPath();
        }
    }
}
