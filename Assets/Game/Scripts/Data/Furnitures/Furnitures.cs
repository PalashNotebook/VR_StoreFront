using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace com.palash.immersed
{
    [System.Serializable]
    public class FurnitureGroup : Group
    {
        public Furnitures[] furnitures;
    }
    [System.Serializable]
    public class Furnitures : Items, IFurnitures
    {
        public Color color;
        public float size;

    }
}
