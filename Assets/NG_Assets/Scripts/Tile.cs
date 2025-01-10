using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NG.Gameplay
{
    public class Tile : MonoBehaviour
    {
        [HideInInspector] public Vector2Int cords;

        private GridManager _gridManager;

        void Start()
        {
            _gridManager = FindObjectOfType<GridManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

