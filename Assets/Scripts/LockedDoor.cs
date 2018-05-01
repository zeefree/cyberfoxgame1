using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour {

    HashSet<string> lookingFor;

    void Awake()
    {
        lookingFor = new HashSet<string>();
        lookingFor.Add("RedKeycard");
        lookingFor.Add("YellowKeycard");
    }
	
    public void OpenDoor(HashSet<string> itemSet)
    {
        bool unlock = true;
        foreach (string item in lookingFor)
        {
            if (!itemSet.Contains(item))
                unlock = false;
        }

        if (unlock)
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
