using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace com.palash.immersed
{
    [System.Serializable]
    public class ElectronicsGroup : Group
    {
        public Electronics[] electronics;
    }
    
    [System.Serializable]
    public class Electronics : Items, IElectronics
    {
        public Color color;
        public float size;
        public string brandName;
    }
}
