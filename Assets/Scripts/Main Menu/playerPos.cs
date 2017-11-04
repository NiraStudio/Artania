using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPos : MonoBehaviour {

	// Use this for initialization
    public void GoToScene(string Name)
    {
        loadingScreen.Instance.Show(Name);
    }
}
