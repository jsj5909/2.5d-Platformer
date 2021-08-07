using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _coinText, _livesText;

    [SerializeField] GameObject _instructionPanel, _startCutScene;


    public void UpdateCoinDisplay(int coins)
    {
        _coinText.text = "Power Cells: " + coins.ToString();
    }

    public void UpdateLivesDisplay(int lives)
    {
        _livesText.text = "Lives: " + lives.ToString();
    }
   
    public void StartButtonPressed()
    {
        _instructionPanel.SetActive(false);
        _startCutScene.SetActive(true);
    }
}
