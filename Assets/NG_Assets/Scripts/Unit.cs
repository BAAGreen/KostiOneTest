using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NG.Gameplay
{
    public class Unit : MonoBehaviour
    {
        private GridManager _gridManager;

        [SerializeField] private string _unitID;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _movementSpeed = 1;
        [HideInInspector] public List<Node> path = new List<Node>();

        void Start()
        {
            _gridManager = FindObjectOfType<GridManager>();
            InitPositionInGrid();
        }

        private void InitPositionInGrid()
        {
            transform.position = Data.DataManager.GetUnitPosition(_unitID, transform.position);

            Node initialNode = _gridManager.GetNode(_gridManager.GetCoordinatesFromPosition(transform.position));
            Vector3 initialPosition = _gridManager.GetPositionFromCoordinates(initialNode.cords);
            initialPosition.y = transform.position.y;
            transform.position = initialPosition;
            initialNode.walkable = false;
        }

        public void FollowPath()
        {
            StartCoroutine(FollowPathCoroutine());
        }

        public void StopFollowing()
        {
            StopAllCoroutines();
            _animator.SetBool("IsRunning", false);
        }

        private IEnumerator FollowPathCoroutine()
        {
            _animator.SetBool("IsRunning", true);

            for (int i = 1; i < path.Count; i++)
            {
                Vector3 startPosition = transform.position;
                Vector3 endPosition = _gridManager.GetPositionFromCoordinates(path[i].cords);
                endPosition.y = transform.position.y;

                float travelPercent = 0;

                transform.LookAt(endPosition);

                while (travelPercent < 1)
                {
                    travelPercent += Time.deltaTime * _movementSpeed;
                    transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                    yield return new WaitForEndOfFrame();
                }
            }

            _animator.SetBool("IsRunning", false);

            Data.DataManager.SetUnitPosition(_unitID, _gridManager.GetPositionFromCoordinates(path[path.Count - 1].cords));
        }
    }
}
