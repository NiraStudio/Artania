using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;
    public bool Tutorial;
    public Slider EnergyBar;
    public PlayerDataBase Data;
    public string CharacterName;

    Player Character;

    ButtonMover Mover;
    PlayerAttack attack;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if(!Tutorial)
             Character = Data.GetPlayerById(GameManager.Instance.characterData.charcaterId);
        else
            Character = Data.GetPlayerById(1);

        CharacterName = Character.Name;
        GameObject p = Instantiate(Character.Shape.transform.GetChild(0).gameObject, transform);
        p.transform.localPosition = Vector2.zero;
        Mover = GetComponent<ButtonMover>();
        Mover.player = p;
    }
	
}
