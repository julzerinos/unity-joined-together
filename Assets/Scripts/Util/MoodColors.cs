using UnityEngine;

namespace Util
{
    public static class MoodColors
    {
        public static Color HappyColor()
        {
            // var R = Random.Range(0.5f, 1f);
            // return new Color(R, Random.Range(0f, .5f), Random.Range(0f, R));

            return Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
        }

        public static Color SadColor()
        {
            // var B = Random.Range(.5f, 1f);
            // return new Color(Random.Range(0f, B), Random.Range(0f, 1f), B);
            
            return Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0f, .45f), Random.Range(0f, .45f));

        }
    }
}