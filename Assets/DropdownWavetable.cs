using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownWavetable : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject Violin;
    public GameObject Original;
    public GameObject Guitarra_Datos;
    public GameObject SynthPolifonico;
    public GameObject DropViolinTipoO;
    public GameObject DropOriginalTipoO;
    public GameObject DropGuitarDatTipoO;

    void Start()
    {
            // Activa el objeto ADSRoriginal al inicio
        Original.SetActive(false);
        DropOriginalTipoO.SetActive(false);
    }

    public void OnDropdownValueChanged(int index)
    {
        // Desactiva todos los objetos primero
        Violin.SetActive(false);
        Original.SetActive(false);
        Guitarra_Datos.SetActive(false);
        SynthPolifonico.SetActive(false);
        DropViolinTipoO.SetActive(false);
        DropOriginalTipoO.SetActive(false);
        DropGuitarDatTipoO.SetActive(false);
        // Desactiva más objetos aquí

        // Activa el objeto correspondiente según la selección del Dropdown
        switch (index)
        {
            case 1:
                Original.SetActive(true);
                DropOriginalTipoO.SetActive(true);
                break;
            case 2:
                Violin.SetActive(true);
                DropViolinTipoO.SetActive(true);
                break;
            case 3:
                Guitarra_Datos.SetActive(true);
                DropGuitarDatTipoO.SetActive(true);
                break;
            case 4:
                SynthPolifonico.SetActive(true);
                break;                
            // Agrega más casos según las opciones del Dropdown
        }
    }
}
