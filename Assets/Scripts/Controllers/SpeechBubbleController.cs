using System;
using System.Collections;
using Entities;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Controllers
{
    public class SpeechBubbleController : MonoBehaviour
    {
        public int speechBubblesCount = 10;
        public GameObject speechBubble;

        private RectTransform _bubblesRect;

        private const float ImageSize = 0.023256f;

        private Tuple<RectTransform, RawImage>[] _speechBubbles;

        private Camera _cm;

        private void OnEnable()
        {
            Human.Spoke += StartSpeaking;
        }

        private void Awake()
        {
            _bubblesRect = GetComponent<RectTransform>();
            _cm = Camera.main;

            _speechBubbles = new Tuple<RectTransform, RawImage>[speechBubblesCount];

            for (var i = 0; i < speechBubblesCount; i++)
            {
                var sb = Instantiate(speechBubble, transform);
                _speechBubbles[i] =
                    new Tuple<RectTransform, RawImage>(sb.GetComponent<RectTransform>(),
                        sb.transform.Find("Speech").GetComponent<RawImage>());
                sb.SetActive(false);
            }
        }

        private void StartSpeaking(bool happy, Transform speaker, string emoji)
        {
            if (!GetActive(out var sb))
                return;

            var bubble = sb.Item1;
            var speech = sb.Item2;

            bubble.gameObject.SetActive(true);

            speech.uvRect = Emoji(happy, emoji);

            StartCoroutine(Speak(bubble, speaker));
        }

        private static Rect Emoji(bool happy, string emoji)
        {
            int x, y;
            if (emoji.Length == 0)
                Emojis.GetEmoji(happy, out x, out y);
            else
            {
                var v2i = Emojis.OtherEmojis[emoji];
                x = v2i.y -1;
                y = v2i.x + 1;
            }

            return new Rect(x * ImageSize, 1 - y * ImageSize, ImageSize, ImageSize);
        }

        private IEnumerator Speak(RectTransform bubble, Transform speaker)
        {
            bubble.anchoredPosition = (Vector2) _cm.WorldToScreenPoint(speaker.position + 2.5f * Vector3.up) -
                                      _bubblesRect.sizeDelta / 2f;

            yield return new WaitForSeconds(3.0f);
            bubble.gameObject.SetActive(false);
        }

        private bool GetActive(out Tuple<RectTransform, RawImage> sb)
        {
            sb = null;

            foreach (var t in _speechBubbles)
            {
                if (t.Item1.gameObject.activeSelf)
                    continue;

                sb = t;
                return true;
            }

            return false;
        }
    }
}