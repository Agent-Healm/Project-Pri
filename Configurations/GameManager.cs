using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI textHealth;
    public TextMeshProUGUI textArmor;

    public PlayerHitpoint playerHitpoint;
    private bool _isBattleMode = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textHealth.text = "" + playerHitpoint.getPlayerStats(1);
        textArmor.text = "" + playerHitpoint.getPlayerStats(2);
    }
}
