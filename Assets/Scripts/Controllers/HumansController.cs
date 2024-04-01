using Entities;
using Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class HumansController : MonoBehaviour
    {
        private int _happyHumansCount = 8;
        private int _sadHumansCount = 1;
        public GameObject human;

        private Human[] _humans;

        private void Start()
        {
            _happyHumansCount = Random.Range(7, 14);
            _sadHumansCount = Random.Range(3, 6);

            _humans = new Human[_happyHumansCount + _sadHumansCount];

            var sad = _sadHumansCount;
            for (var i = 0; i < _happyHumansCount + _sadHumansCount; i++)
            {
                var h = Instantiate(human, 3 * Random.insideUnitSphere.XZ3(), Quaternion.identity, transform)
                    .GetComponent<Human>();
                h.SetMood(sad-- <= 0); // TODO: Distribution
                _humans[i] = h;
            }

            FindObjectOfType<Controls>().InitializeHumanCache(_humans);
        }
    }
}