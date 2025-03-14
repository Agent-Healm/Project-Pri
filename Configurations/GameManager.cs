using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textHealth;
    public TextMeshProUGUI textArmor;
    public TextMeshProUGUI textMana;

    public PlayerHitpoint playerHitpoint;
    public PlayerMana playerMana;

    // private bool _isBattleMode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // _isBattleMode = true;
        textHealth.text = "" + playerHitpoint.getPlayerStats(1);
        textArmor.text = "" + playerHitpoint.getPlayerStats(2);
        textMana.text = "" + playerMana._energyPoint;
    }
}
