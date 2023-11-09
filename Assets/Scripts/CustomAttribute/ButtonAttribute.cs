using UnityEngine;

// Reference: https://note.com/dobunezumi06/n/n47c3ee7ac7f8
public class ButtonAttribute : PropertyAttribute
{
    public string methodName;

    public string buttonName;

    public ButtonAttribute(string methodName, string buttonName = null)
    {
        this.methodName = methodName;

        if (buttonName == null)
        {
            this.buttonName = methodName;
        }
        else
        {
            this.buttonName = buttonName;
        }

    }

}