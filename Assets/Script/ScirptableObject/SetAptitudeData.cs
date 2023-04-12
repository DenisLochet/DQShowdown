using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SetAptitudeData", menuName = "set Aptitude", order = 1)]
public class SetAptitudeData : ScriptableObject
{
    public string setName;
    public List<AptitudeData> aptitudeDatas;
}
