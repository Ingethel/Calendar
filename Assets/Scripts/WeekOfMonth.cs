using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekOfMonth : MonoBehaviour {

    public DayOfMonth[] days;
    
	void Awake () {
        days = GetComponentsInChildren<DayOfMonth>();
	}
	
}
