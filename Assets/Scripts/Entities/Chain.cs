using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Vector3 = UnityEngine.Vector3;

namespace Entities
{
    public class Chain
    {
        private int ChainSize => _humans.Count;

        public Color chainColor;

        public Vector3 Center =>
            _humans.Aggregate(Vector3.zero, (current, h) => current + h.transform.position) / ChainSize;

        private bool _locked;

        private readonly LinkedList<Human> _humans;
        private readonly HashSet<Human> _uniqueHumans;

        private Chain(Human a, Human b)
        {
            _humans = new LinkedList<Human>();
            _uniqueHumans = new HashSet<Human>();
            chainColor = MoodColors.HappyColor();
            chainColor.a = .5f;
            AddToChain(a);
            AddToChain(b);
        }

        public static Chain CreateChain(Human a, Human b)
        {
            return new Chain(a, b);
        }

        public bool AddToChain(Human human)
        {
            if (_uniqueHumans.Contains(human) || _locked)
                return false;

            if (_humans.Count > 0)
                human.AddHingeJoint(_humans.Last.Value.rb);

            human.AddedToChain(this);
            _humans.AddLast(human);
            _uniqueHumans.Add(human);

            return true;
        }

        public Human Last => _humans.Last.Value;

        public bool FirstOrLast(Human human) =>
            ReferenceEquals(human, _humans.First.Value) || ReferenceEquals(human, _humans.Last.Value);

        public bool Together(Human other) => _uniqueHumans.Contains(other);

        public void DestroyChain()
        {
            foreach (var human in _humans)
                human.RemovedFromChain();
        }

        public Vector3 GetNextPosition => _humans.Last.Value.transform.position + Vector3.right;

        public void CloseCircle()
        {
            if (_humans.Count < 5)
                return;

            _humans.First.Value.AddHingeJoint(_humans.Last.Value.rb);
            _locked = true;

            foreach (var h in _humans)
            {
                if (Physics.Linecast(h.transform.position + Vector3.up, Center, out var hit) &&
                    hit.transform.CompareTag("Human"))
                    hit.transform.GetComponent<Human>().SwitchMood(true);
            }

            _humans.First.Value.StartCoroutine(TriggerDestroyChain());
        }

        private IEnumerator TriggerDestroyChain()
        {
            yield return new WaitForSeconds(2.0f);
            DestroyChain();
        }
    }
}