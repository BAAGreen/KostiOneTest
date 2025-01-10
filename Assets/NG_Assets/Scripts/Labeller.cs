using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NG.Gameplay
{

    [ExecuteAlways]
    public class Labeller : MonoBehaviour
    {
        private GridManager _gridManager;
        private TextMeshPro _label;
        private Vector2Int _cords = new Vector2Int();

        [SerializeField] private Color _defaultColor = Color.white;
        [SerializeField] private Color _blockedColor = Color.red;
        [SerializeField] private Color _exploredColor = Color.yellow;
        [SerializeField] private Color _pathColor = Color.blue;

        private void Awake()
        {
            _gridManager = FindObjectOfType<GridManager>();
            _label = GetComponentInChildren<TextMeshPro>();
            _label.enabled = false;
            DisplayCords();
        }

        private void Update()
        {
            if (!Application.isPlaying) _label.enabled = true;

            DisplayCords();
            transform.name = _cords.ToString();

            SetLableColor();
            ToggleLables();
        }

        private void SetLableColor()
        {
            if (_gridManager == null) return;

            Node node = _gridManager.GetNode(_cords);

            if (node == null) return;

            if (!node.walkable) _label.color = _blockedColor;
            else if (node.path) _label.color = _pathColor;
            else if (node.explored) _label.color = _exploredColor;
            else _label.color = _defaultColor;
        }

        private void ToggleLables()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                _label.enabled = !_label.enabled;
            }
        }

        private void DisplayCords()
        {
            if (!_gridManager) return;

            _cords.x = Mathf.RoundToInt(transform.position.x / _gridManager.unitGridSize);
            _cords.y = Mathf.RoundToInt(transform.position.z / _gridManager.unitGridSize);
            _label.text = $"{_cords.x},{_cords.y}";
        }
    }
}
