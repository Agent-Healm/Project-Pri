using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHealth;
    [SerializeField] private TextMeshProUGUI textArmor;
    [SerializeField] private TextMeshProUGUI textMana;

    [SerializeField] private PlayerHitpoint playerHitpoint;
    [SerializeField] private PlayerMana playerMana;

    // private bool _isBattleMode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // _isBattleMode = true;
        textHealth.text = "" + playerHitpoint.GetCurrentHealthPoint;
        textArmor.text = "" + playerHitpoint.GetCurrentArmorPoint;
        textMana.text = "" + playerMana.GetEnergyPoint();
    }
}
