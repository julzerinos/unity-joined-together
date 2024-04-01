using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class Controls : MonoBehaviour
    {
        private Camera _camera;

        private Dictionary<Transform, Human> _transformHumanMap;
        private Human _currentHuman;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void InitializeHumanCache(IEnumerable<Human> humans)
        {
            _transformHumanMap = new Dictionary<Transform, Human>();
            foreach (var h in humans)
                _transformHumanMap.Add(h.transform, h);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            if (Input.GetKeyDown(KeyCode.Space) && _currentHuman.Chained)
            {
                _currentHuman.chain.DestroyChain();
                _currentHuman?.Deselect();
                _currentHuman = null;
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                _currentHuman?.Deselect();
                _currentHuman = null;
                return;
            }

            if (!Input.GetMouseButtonDown(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
                return;

            if (hit.collider.gameObject.CompareTag("Human"))
                HandleHuman(_transformHumanMap[hit.collider.transform]);

            if (hit.collider.gameObject.CompareTag("Walkable"))
                _currentHuman?.StartWalking(hit.point);
        }

        private void HandleHuman(Human human)
        {
            if (ReferenceEquals(_currentHuman, human))
            {
                human.Select();
                return;
            }

            if (ReferenceEquals(_currentHuman, null))
            {
                _currentHuman = human.Select();
                return;
            }

            _currentHuman.Deselect();
            _currentHuman = _currentHuman.TriggerPair(human) ? null : human.Select();
        }
    }
}