using System;
using System.Collections;
using Extensions;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Entities
{
    public class Human : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public GameObject jointTemplate;

        public static Action<bool, Transform, string> Spoke;

        public Rigidbody rb;

        private bool _walking;
        private Vector3 _walkTarget;
        private Human _pairTarget;
        private bool _pairing;

        private Material[] _mats;
        private GameObject _selCircle;
        private Renderer _chainCircle;
        private Transform _model;
        private Vector3 _rightTarget;

        private HingeJoint _hj;

        public Chain chain;
        public bool Chained => chain != null;

        private bool _happy = true;
        private Color _baseColor;

        private Animator _anm;
        private static readonly int Happy = Animator.StringToHash("Happy");
        private static readonly int Holding = Animator.StringToHash("Holding");
        private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");

        private void Awake()
        {
            _selCircle = transform.Find("SelectedCircle").gameObject;
            _selCircle.SetActive(false);
            rb = GetComponent<Rigidbody>();

            _chainCircle = transform.Find("ChainCircle").gameObject.GetComponent<Renderer>();
            _chainCircle.gameObject.SetActive(false);

            _anm = GetComponentInChildren<Animator>();
            _anm.speed = Random.Range(.9f, 1.1f);

            _model = transform.Find("Model");
            _mats = new[]
            {
                _model.transform.Find("Body").GetComponent<Renderer>().material,
                _model.transform.Find("LLeg").GetComponent<Renderer>().material,
                _model.transform.Find("RLeg").GetComponent<Renderer>().material,
            };
        }

        private void Start()
        {
            StartCoroutine(Banter());
        }

        private void OnDisable()
        {
            Spoke = null;
        }

        private void SetClothingColor(Color color)
        {
            foreach (var m in _mats)
                m.color = color;
        }

        public void SetMood(bool happy)
        {
            _happy = happy;
            SetClothingColor(_baseColor = _happy ? MoodColors.HappyColor() : MoodColors.SadColor());
            _selCircle.GetComponent<Renderer>().material.color = _baseColor;
            _anm.SetBool(Happy, happy);
        }

        public void StartWalking(Vector3 to)
        {
            _walkTarget = to.XZ3(transform.position.y);
            _walking = true;
        }

        private void StopWalking(bool arrived = false)
        {
            _walkTarget = transform.position;
            _walking = false;

            if (!_pairing) return;

            if (arrived) TriggerPair(_pairTarget);

            _pairing = false;
            _pairTarget = null;
        }

        private void FixedUpdate()
        {
            _anm.SetFloat(WalkSpeed, rb.velocity.sqrMagnitude);

            if (!_walking)
                return;

            if ((transform.position - _walkTarget).sqrMagnitude <= 0.4f)
                StopWalking(true);

            if (rb.velocity.sqrMagnitude <= walkSpeed)
                rb.AddForce((_walkTarget - transform.position).normalized, ForceMode.VelocityChange);
        }

        private void Update()
        {
            if (Chained)
                _rightTarget = chain.Center - transform.position;

            if (rb.velocity.sqrMagnitude > 0.2f)
                _rightTarget = rb.velocity;

            if ((_model.right - _rightTarget).sqrMagnitude > 0.1f)
                _model.right = Vector3.Lerp(_model.right, _rightTarget, 0.15f); // TODO: Rotate speed
        }

        public Human Select()
        {
            if (!_happy)
            {
                Speak("cross");
                return null;
            }

            StopWalking();

            _selCircle.SetActive(true);
            return this;
        }

        private void UpdateVisual()
        {
            // _mat.color = Chained ? Color.blue : _baseColor; // todo selectioncircle
        }

        public void Deselect()
        {
            UpdateVisual();
            _selCircle.SetActive(false);
        }

        public bool TriggerPair(Human other)
        {
            if (!other._happy)
                return false;

            if (Chained && !other.Chained)
            {
                other.TriggerPair(this);
                return true;
            }

            if ((other.transform.position - transform.position)
                .sqrMagnitude > 1f)
            {
                _pairing = true;
                _pairTarget = other.Chained ? other.chain.Last : other;
                StartWalking(_pairTarget.transform.position);
                return true;
            }

            StopWalking();
            other.StopWalking();

            if (Chained)
            {
                if (chain.Together(other))
                {
                    if (chain.FirstOrLast(this) && chain.FirstOrLast(other))
                        chain.CloseCircle();
                    return true;
                }

                if (!other.Chained)
                {
                    chain.AddToChain(other);
                    return true;
                }

                if (other.Chained)
                    return false;

                // TODO: What happens if other is chained?
            }

            if (other.Chained)
            {
                other.chain.AddToChain(this);
                return true;
            }

            Chain.CreateChain(this, other);
            return true;
        }

        public void AddedToChain(Chain newChain)
        {
            chain = newChain;
            // _mat.color = Color.blue; // TODO: Fill circle under
            _chainCircle.gameObject.SetActive(true);
            _chainCircle.material.color = chain.chainColor;
            _anm.SetBool(Holding, true);
        }

        public void RemovedFromChain()
        {
            chain = null;
            BreakHingeJoint();
            UpdateVisual();
            _anm.SetBool(Holding, false);
            _chainCircle.gameObject.SetActive(false);
        }

        public void AddHingeJoint(Rigidbody connectedBody)
        {
            _hj = gameObject.AddComponent<HingeJoint>();
            _hj.autoConfigureConnectedAnchor = false;
            _hj.anchor = Vector3.zero;
            _hj.connectedAnchor = Vector3.left * 1.1f;
            _hj.axis = Vector3.up;
            _hj.enablePreprocessing = false;
            _hj.connectedBody = connectedBody;
            _hj.connectedAnchor = 1.1f * Vector3.left;
        }

        private void BreakHingeJoint()
        {
            Destroy(_hj);
        }

        private IEnumerator Banter()
        {
            yield return new WaitForSeconds(Random.Range(5f, 15f));
            var delay = new WaitForSeconds(Random.Range(10f, 25f));
            while (true)
            {
                yield return delay;
                Speak();
            }
        }

        private void Speak(string emoji = "")
        {
            Spoke?.Invoke(_happy, transform, emoji);
        }

        public void SwitchMood(bool happy)
        {
            Speak("heart");
            SetMood(happy);
        }
    }
}