using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CalculatorScript : MonoBehaviour
{
    float buffer;
    [SerializeField] TextMeshProUGUI screen;
    [SerializeField] TextMeshProUGUI todoscreen;
    [SerializeField] TextMeshProUGUI bufferscreen;
    int todo = 0;
    // Start is called before the first frame update
    void Start()
    {
        Clear();
    }

    // Update is called once per frame

    public void Clear()
    {
        screen.text = "";
        bufferscreen.text = "";
        todoscreen.text = "";
        buffer = 0;
        todo = 0;
    }

    public void AddSymbol(string symbol)
    {
        screen.text += symbol;
    }
    public void ClearLastSymbol()
    {
        screen.text = screen.text.Remove(screen.text.Length - 1);
    }

    private void SaveToBuffer(int TodoNumber)
    {
        buffer = float.Parse(screen.text);
        screen.text = "";
        todo = TodoNumber;
        bufferscreen.text = buffer.ToString();
        switch (todo)
        {
            case 0:
                todoscreen.text = "";
                break;
            case 1:
                todoscreen.text = "+";
                break;
            case 2:
                todoscreen.text = "-";
                break;
            case 3:
                todoscreen.text = "*";
                break;
            case 4:
                todoscreen.text = "/";
                break;
        }
    }
    public void SaveAndClear()
    {
        try
        {
            switch (todo)
            {
                case 0:
                    screen.text = "¬ведите числа.";
                    break;
                case 1:
                    screen.text = (buffer + float.Parse(screen.text)).ToString();
                    break;
                case 2:
                    screen.text = (buffer - float.Parse(screen.text)).ToString();
                    break;
                case 3:
                    screen.text = (buffer * float.Parse(screen.text)).ToString();
                    break;
                case 4:
                    screen.text = (buffer / float.Parse(screen.text)).ToString();
                    break;
            }
        }
        catch(Exception ex)
        {
            screen.text = "ќшибка " + ex; 
        }
    }
    public void Divide()
    {
        SaveToBuffer(4);
    }
    public void Multiply()
    {
        SaveToBuffer(3);
    }

    public void Minus()
    {
        SaveToBuffer(2);
    }

    public void Plus()
    {
        SaveToBuffer(1);
    }
}
