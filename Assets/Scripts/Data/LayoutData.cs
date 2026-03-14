using UnityEngine;

namespace CardMatch.Data
{
    [System.Serializable]
    public struct LayoutData
    {
        [Min(1)] public uint x;
        [Min(1)] public uint y;

        public LayoutData(uint x, uint y)
        {
            this.x = (uint) Mathf.Max(1, x);
            this.y = (uint) Mathf.Max(1, y);
        }

        public float Aspect => (float) x / y;
    }
}