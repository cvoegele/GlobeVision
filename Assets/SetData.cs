using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SetData : MonoBehaviour
{
    public Champion data;
    public ChampionInformationSetter ChampionInformationSetter;
    
    public void Set()
    {
        if (data != null)
        {
            ChampionInformationSetter.SetInformation(data);
        }
       
    }
}
