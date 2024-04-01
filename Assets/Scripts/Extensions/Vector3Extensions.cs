using UnityEngine;

namespace Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 XZ3(this Vector3 v, float y = 0) => new Vector3(v.x, y, v.z);
    }
}