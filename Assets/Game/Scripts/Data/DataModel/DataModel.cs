using System.Collections;
using System.Collections.Generic;
using com.palash.immersed;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DataCreator", order = 1)] 
public class DataModel : ScriptableObject
{
    public FurnitureGroup furniture;
    public ElectronicsGroup electronic;

}

public class Data
{
    public FurnitureGroup furniture;
    public ElectronicsGroup electronic;
}
