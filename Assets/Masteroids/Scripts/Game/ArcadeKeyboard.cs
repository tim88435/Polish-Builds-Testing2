using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcadeKeyboard : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerInstance;
    [SerializeField] private InputField inputField;
    private string name;
    private char[] letters = new char[12];
    private char[] letterOptions = new char[27] { '_', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
    private Vector2 LastLetterChange = Vector2.zero;
    private float heldDownTime;
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
    private Vector2 JoyStickInputs
    {
        get
        {
            if (LastLetterChange != rawJoystickInputs)
            {
                heldDownTime = 0;
                return rawJoystickInputs;
            }
            heldDownTime += Time.deltaTime;
            if (heldDownTime > 1)
            {
                return rawJoystickInputs;
            }
            return Vector2.zero;
            
        }
    }
    private void Update()
    {
        inputField.selectionAnchorPosition += (int)JoyStickInputs.x;
        Mathf.Clamp((int)JoyStickInputs.x, 0, inputField.text.Length - 1);
        inputField.selectionFocusPosition = inputField.selectionAnchorPosition + 1;
        //if (Input.GetButtonDown())
        {

        }
    }
}
