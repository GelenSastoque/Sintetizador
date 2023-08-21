using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{
    public Dropdown dropdown;
    public GameObject Violin;
    public GameObject ADSRoriginal;
    public GameObject Violin_Datos;
    public GameObject Guitarra_Datos;
    public GameObject DropViolinTipoO;
    public GameObject DropADSRTipoO;
    public GameObject DropViolDatTipoO;
    public GameObject DropGuitarDatTipoO;
    //Sliders ADS Piano
    public GameObject SAttackP;
    public GameObject SDecayP;
    public GameObject SSustainP;
    //Slider ADS violin
    public GameObject SAttackV;
    public GameObject SDecayV;
    public GameObject SSustainV;
    
    //Slider armonicos piano
    public GameObject ArmoniP;

    public GameObject ArmoniV;

    public GameObject ArmoniVD;
    void Start()
    {
            // Activa el objeto ADSRoriginal al inicio
        ADSRoriginal.SetActive(true);
        DropADSRTipoO.SetActive(true);
        SAttackP.SetActive(true);
        SDecayP.SetActive(true);
        SSustainP.SetActive(true);
        ArmoniP.SetActive(true);
    }
     public void OnDropdownValueChanged(int index)
    {
        // Desactiva todos los objetos primero
        Violin.SetActive(false);
        ADSRoriginal.SetActive(false);
        Violin_Datos.SetActive(false);
        Guitarra_Datos.SetActive(false);
        DropViolinTipoO.SetActive(false);
        DropADSRTipoO.SetActive(false);
        DropViolDatTipoO.SetActive(false);
        DropGuitarDatTipoO.SetActive(false);
        SAttackP.SetActive(false);
        SDecayP.SetActive(false);
        SSustainP.SetActive(false);
        SAttackV.SetActive(false);
        SDecayV.SetActive(false);
        SSustainV.SetActive(false);
       
        ArmoniP.SetActive(false);
        ArmoniV.SetActive(false);
        ArmoniVD.SetActive(false);
        // Desactiva más objetos aquí

        // Activa el objeto correspondiente según la selección del Dropdown
        switch (index)
        {
            case 0:
                ADSRoriginal.SetActive(true);
                DropADSRTipoO.SetActive(true);
                SAttackP.SetActive(true);
                SDecayP.SetActive(true);
                SSustainP.SetActive(true);
                ArmoniP.SetActive(true);
                break;
            case 1:
                Violin.SetActive(true);
                DropViolinTipoO.SetActive(true);
                SAttackV.SetActive(true);
                SDecayV.SetActive(true);
                SSustainV.SetActive(true);
                ArmoniV.SetActive(true);
                break;
            case 2:
                Violin_Datos.SetActive(true);
                DropViolDatTipoO.SetActive(true);
                ArmoniVD.SetActive(true);
                break;
            case 3:
                Guitarra_Datos.SetActive(true);
                DropGuitarDatTipoO.SetActive(true);
                
                break;
                
            // Agrega más casos según las opciones del Dropdown
        }
    }
}
