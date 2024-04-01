using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public static class Emojis
    {
        private static readonly Dictionary<string, Vector2Int> SadEmojis = new Dictionary<string, Vector2Int>
        {
            {"sad_face", new Vector2Int(0, 37)},
            {"face_palm", new Vector2Int(5, 40)},
            {"bear", new Vector2Int(1, 27)},
            {"alien", new Vector2Int(3, 31)},
            {"SOS", new Vector2Int(4, 7)},
            {"skull", new Vector2Int(5, 31)},
            {"crossed_arms", new Vector2Int(6, 37)},
            {"nerve", new Vector2Int(8, 32)},
            {"sleepy", new Vector2Int(10, 32)},
            {"alien_2", new Vector2Int(14, 35)},
            {"poo", new Vector2Int(15, 32)},
            {"volcano", new Vector2Int(18, 11)},
            {"thunder", new Vector2Int(19, 1)},
            {"poker_face", new Vector2Int(19, 36)},
            {"poker_face_2", new Vector2Int(20, 36)},
            {"sad_face_2", new Vector2Int(21, 36)},
            {"sad_face_3", new Vector2Int(21, 39)},
            {"sad_face_31", new Vector2Int(22, 36)},
            {"sad_face_4", new Vector2Int(22, 42)},
            {"not_like", new Vector2Int(23, 28)},
            {"sad_face_5", new Vector2Int(25, 36)},
            {"sad_face_6", new Vector2Int(26, 39)},
            {"broken_heart", new Vector2Int(30, 26)},
            {"sad_face_7", new Vector2Int(35, 22)},
            {"sad_face_8", new Vector2Int(38, 36)},
        };

        private static readonly Dictionary<string, Vector2Int> HappyEmojis = new Dictionary<string, Vector2Int>
        {
            {"ice_cream", new Vector2Int(0, 22)},
            {"rabbit", new Vector2Int(0, 26)},
            {"green_heart", new Vector2Int(0, 32)},
            {"pur_heart", new Vector2Int(2, 32)},
            {"happy_face", new Vector2Int(3, 36)},
            {"very_happy_face", new Vector2Int(4, 36)},
            {"wave", new Vector2Int(5, 28)},
            {"laughing", new Vector2Int(5, 36)},
            {"very_happy_face_2", new Vector2Int(6, 36)},
            {"very_happy_face_3", new Vector2Int(8, 36)},
            {"angel", new Vector2Int(10, 36)},
            {"wink", new Vector2Int(12, 36)},
            {"very_happy_face_4", new Vector2Int(13, 36)},
            {"very_happy_face_5", new Vector2Int(15, 36)},
            {"like", new Vector2Int(17, 28)},
            {"glasses", new Vector2Int(17, 36)},
            {"flower", new Vector2Int(19, 20)},
            {"biceps", new Vector2Int(21, 32)},
            {"king", new Vector2Int(24, 40)},
            {"clap", new Vector2Int(27, 3)},
            {"kiss", new Vector2Int(27, 36)},
            {"laughing2", new Vector2Int(31, 36)},
            {"rock", new Vector2Int(34, 39)},
            {"cowboy", new Vector2Int(38, 34)},
        };

        public static readonly Dictionary<string, Vector2Int> OtherEmojis = new Dictionary<string, Vector2Int>
        {
            {"check_mark", new Vector2Int(4, 42)},
            {"heart", new Vector2Int(42, 7)},
            {"cross", new Vector2Int(42, 1)},
        };

        public static void GetEmoji(bool happy, out int x, out int y)
        {
            var set = happy ? HappyEmojis : SadEmojis;
            var v2i = set.Values.ElementAt(Random.Range(0, set.Count / 2));
            x = v2i.y-1;
            y = v2i.x+1;
        }
    }
}