﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class NewEntryPanelHandler : Panel {

    public InputField StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes;
    public InputField[] fields;
    private EventSystem system;

    public GameObject editButtons, newEntryButtons;

    private NewEntry guide;

    void Start() {
        system = EventSystem.current;
        fields = new InputField[]{ StartTimeH, StartTimeM, EndTimeH, EndTimeM, Day, Month, Year, NameOfTeam, NumberOfPeople, PersonInCharge, Telephone, ConfirmationDate, Guide, Notes };
    }

    void Update()
    {
        KeybordInputHandler();
    }

    protected override void KeybordInputHandler()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Return))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                {
                    inputfield.OnPointerClick(new PointerEventData(system));
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Save()
    {
        if (CheckSaveEligibility()) {

            if (guide != null)
               if(guide.Date != Day.text + "." + Month.text + "." + Year.text)
                    dataManager.RequestDelete(guide);
             
            SaveInfo();
            dataManager.RequestWrite(guide);
            calendarController.RequestView(CalendarViewController.State.DAILY, new System.DateTime(guide.year, guide.month, guide.day));
            guide = null;
            Close();
        }
    }

    public void Delete()
    {
        dataManager.RequestDelete(guide);
        calendarController.RefreshView();
        Close();
    }

    private bool CheckSaveEligibility()
    {
        if (StartTimeH.text != "" && StartTimeM.text != "" && 
            EndTimeH.text != "" && EndTimeM.text != "" && 
            Day.text != "" && Month.text != "" && Year.text != "")
            return true;
        return false;
    }

    private string TryGetText(InputField a)
    {
        if (a.text == null)
            return "";
        return a.text;
    }

    public void PreviewEntry(NewEntry n)
    {
        foreach (InputField field in fields)
            field.interactable = false;

        newEntryButtons.SetActive(false);
        editButtons.SetActive(true);

        guide = n;
        DisplayInfo();
    }

    private void DisplayInfo()
    {
        string[] split = guide.attributes[0].Split(':');
        StartTimeH.text = split[0];
        StartTimeM.text = split[1];
        split = guide.attributes[1].Split(':');
        EndTimeH.text = split[0];
        EndTimeM.text = split[1];
        Day.text = guide.day.ToString();
        Month.text = guide.month.ToString();
        Year.text = guide.year.ToString();
        NameOfTeam.text = guide.attributes[2];
        NumberOfPeople.text = guide.attributes[3];
        PersonInCharge.text = guide.attributes[4];
        Telephone.text = guide.attributes[5];
        ConfirmationDate.text = guide.attributes[6];
        Guide.text = guide.attributes[7];
        Notes.text = guide.attributes[8];
    }

    private void SaveInfo()
    {
        string[] inputs = { StartTimeH.text + ":" + StartTimeM.text, EndTimeH.text + ":" + EndTimeM.text,
                TryGetText(NameOfTeam), TryGetText(NumberOfPeople), TryGetText(PersonInCharge), TryGetText(Telephone),
                TryGetText(ConfirmationDate), TryGetText(Guide), TryGetText(Notes)};
        guide = new NewEntry(inputs, Day.text + "." + Month.text + "." + Year.text);
    }

    public void EditEntry()
    {
        foreach (InputField field in fields)
            field.interactable = true;

        editButtons.SetActive(false);
        newEntryButtons.SetActive(true);
    }
    
}