using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HintText : MonoBehaviour
{
    Transform target;
    RectTransform rect;

    HintText SetText(Transform target, string keyName, string description, TextDirection dir = TextDirection.Right)
    {
        this.target = target;
        rect = GetComponent<RectTransform>();

        var text = GetComponent<TextMeshProUGUI>();

        text.text = "[" + keyName + "] " + description;

        if (dir == TextDirection.Left)
        {
            text.horizontalAlignment = HorizontalAlignmentOptions.Right;
            text.margin = new Vector4(-Mathf.Abs(text.margin.x), text.margin.y, text.margin.z, text.margin.w);
        }

        return this;
    }

    public static HintText CreateText(Transform target, string keyName, string description, TextDirection dir = TextDirection.Right)
    {
        HintTextContainer container = FindObjectOfType<HintTextContainer>();
        return Instantiate(container.prefab, container.transform).SetText(target, keyName, description, dir);
    }

    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        if (!target) return;
        rect.position = Camera.main.WorldToScreenPoint(target.position);
    }


    public void Remove()
    {
        Destroy(gameObject);
    }



    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public enum TextDirection
    {
        Left,
        Right
    }
}
