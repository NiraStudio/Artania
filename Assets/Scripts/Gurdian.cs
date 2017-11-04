using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gurdian : MonoBehaviour {
    public enum GurdianModel
    {
        Normal,
        Double,
        Counter,
        DoubleCounter,
        DoubleCoin,
        DoubleEXP,
        TripleCoin,
        TripleEXP,
        GodLike
    }
    public string Name;
    public GurdianModel Kind;
	
}
