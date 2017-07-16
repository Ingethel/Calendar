using UnityEngine;
using UnityEngine.UI;

public class DayGuideView : MonoBehaviour {

    public Text Time, Details;
    public int id;

    public void SetTime(string s) {
        Time.text = s;
    }

    public void SetDetails(string s) {
        Details.text = s;
    }

}
