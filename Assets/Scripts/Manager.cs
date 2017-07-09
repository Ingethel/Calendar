using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {

    public enum ViewState
    {
        MONTHLY,
        WEEKLY,
        DAILY,
        ILLEGAL
    };

    public DateTime currentDate {
        private set; get;
    }
    private DateTime lastGivenDate;

    public static Calendar calendar = CultureInfo.InvariantCulture.Calendar;
    public Dropdown view;
    private MonthManager monthViewManager;
    private DayManager dayViewManager;

    public Canvas MonthlyView, WeeklyView, DailyView;
    private Canvas currentView;
    private ViewState currentState;
    public GameObject NewEntryView, SearchView;
    private List<NewEntryClass> entries;

    void Start () {
        currentState = ViewState.ILLEGAL;
        currentDate = DateTime.Now;
        monthViewManager = FindObjectOfType<MonthManager>();
        dayViewManager = FindObjectOfType<DayManager>();
        NewEntryView.SetActive(false);
        SearchView.SetActive(false);

        MonthlyView.enabled = false;
        DailyView.enabled = false;

        SetView(currentDate);
    }

	void Update () {
		
	}

    public void ChangeView(int i)
    {
        if(i != 0)
            switch (view.value)
            {
                case 2:
                    lastGivenDate = calendar.AddDays(lastGivenDate, i);
                    break;
                case 1:
                    lastGivenDate = calendar.AddWeeks(lastGivenDate, i);
                    break;
                case 0:
                    lastGivenDate = calendar.AddMonths(lastGivenDate, i);
                    break;
            }
        SetView(lastGivenDate);
    }

    public void RequestView(DateTime date, ViewState state) {
        view.value = (int)state;
        SetView(date);
    }

    private void SetView(DateTime date)
    {
        lastGivenDate = date;
        ChangeState((ViewState)view.value);
        switch (view.value)
        {
            case 2:
                dayViewManager.SetView(lastGivenDate);
                break;
            case 1:
                //weekViewManager.SetView(lastGivenDate);
                break;
            case 0:
            default:
                monthViewManager.SetView(lastGivenDate);
                break;
        }
    }

    public void NewEntry()
    {

        NewEntryView.SetActive(!NewEntryView.activeSelf);
    }

    public void Search()
    {
        SearchView.SetActive(true);
    }

    public void CreateEntry()
    {

    }

    void ChangeState(ViewState state)
    {
        if (currentState != state) {
            if (currentView != null)
                currentView.enabled = false;

            currentState = state;

            switch (currentState)
            {
                case ViewState.MONTHLY:
                    currentView = MonthlyView;
                    break;
                case ViewState.WEEKLY:
                    currentView = WeeklyView;
                    break;
                case ViewState.DAILY:
                    currentView = DailyView;
                    break;
            }

            currentView.enabled = true;
        }
    }

}

public class NewEntryClass
{

    private string nameOfTeam, numberOfPeople, personInCharge, telephone, dateOfConfirmation, guide, notes;

    public NewEntryClass()
    {
        nameOfTeam = "";
        numberOfPeople = "";
        personInCharge = "";
        telephone = "";
        dateOfConfirmation = "";
        guide = "";
        notes = "";
    }

    public NewEntryClass(string i_nameOfTeam, string i_numberOfPeople, string i_personInCharge, string i_telephone, string i_dateOfConfirmation, string i_guide, string i_notes)
    {
        nameOfTeam = i_nameOfTeam;
        numberOfPeople = i_numberOfPeople;
        personInCharge = i_personInCharge;
        telephone = i_telephone;
        dateOfConfirmation = i_dateOfConfirmation;
        guide = i_guide;
        notes = i_notes;
    }

}