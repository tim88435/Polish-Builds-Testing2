using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ArcadeKeyboard : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerInstance;
    [SerializeField] private Text inputField;
    private char[] newName = new char[12] { '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_', '_' };
    private string InputText
    {
        get
        {
            string text = "";
            text += "New Name: \n";
            text += 0 == selectedCharacter ? $"<b>{newName[0]}</b>" : newName[0].ToString();
            for (int i = 1; i < newName.Length - 1; i++)
            {
                text += " ";
                text += i == selectedCharacter ? $"<b>{newName[i]}</b>" : newName[i].ToString();
            }
            return text;
        }
    }
    private int selectedCharacter = 0;
    private char[] letterOptions = new char[27] { '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private Vector2 LastLetterChange = Vector2.zero;
    private float heldDownTime;
    private float heldDownRateTime;
    private Vector2 rawJoystickInputs
    {
        get
        {
#if UNITY_EDITOR
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#else
            return new Vector2(Input.GetAxisRaw("P1 Hori"), Input.GetAxisRaw("P1 Verti"));
#endif
        }
    }
    private Vector2 JoyStickInputs { get; set; }

    private void Start()
    {
        inputField = transform.GetChild(0).GetComponent<Text>();
    }
    private void Update()
    {
        UpdateJoystickInputs();
        selectedCharacter = (int)Mathf.Clamp(selectedCharacter + JoyStickInputs.x, 0, newName.Length - 2);
        newName[selectedCharacter] = ChangeLetter((int)JoyStickInputs.y);
        inputField.text = InputText;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Return))
#else
        if (Input.GetButtonDown("P1 Start") || Input.GetButtonDown("P2 Start"))
#endif
        {
            if (TryName(out string newName))
            {
                gameManagerInstance.NewHighScore(newName);
                gameManagerInstance.ChangeScene(0);
            }
        }
    }
    private void UpdateJoystickInputs()
    {
        if (LastLetterChange != rawJoystickInputs)
        {
            heldDownTime = 0;
            LastLetterChange = rawJoystickInputs;
            JoyStickInputs = rawJoystickInputs;
            return;
        }
        heldDownTime += Time.deltaTime;
        heldDownRateTime += Time.deltaTime;
        if (heldDownTime > 0.4f && heldDownRateTime > 0.1f)
        {
            heldDownRateTime = 0;
            JoyStickInputs = rawJoystickInputs;
            return;
        }
        JoyStickInputs = Vector2.zero;
    }
    private char ChangeLetter(int direction)
    {
        int index = Array.IndexOf(letterOptions, newName[selectedCharacter]);
        index += direction;
        if (index >= letterOptions.Length)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = letterOptions.Length - 1;
        }
        return letterOptions[index];
    }
    private bool TryName(out string name)
    {
        name = (new string(newName)).Trim('_');
        name = name.Replace('_', ' ');
        if (name.Length <= 0)
        {
            return false;
        }
        return true;
    }
}
