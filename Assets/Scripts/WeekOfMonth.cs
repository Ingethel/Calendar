using UnityEngine;

public class WeekOfMonth : MonoBehaviour {

    public DayOfMonth[] days;
    
	void Awake () {
        days = GetComponentsInChildren<DayOfMonth>();
	}
	
}
