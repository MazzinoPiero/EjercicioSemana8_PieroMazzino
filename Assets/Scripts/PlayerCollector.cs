using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerCollector : MonoBehaviour
{
    [Header("UI de Recolección")]
    public TextMeshProUGUI collectedItemsText;
    
    [Header("Panel de Objeto Especial")]
    public GameObject specialObjectPanel;
    public Button activateSpecialButton;
    public TextMeshProUGUI specialPanelText;
    
    [Header("Objeto Especial")]
    public GameObject specialObject;
    
    [Header("Configuración")]
    public int totalItemsNeeded = 3;
    

    private List<int> collectedItemIDs = new List<int>();
    private List<string> collectedItemNames = new List<string>();

    void Start()
    {
        UpdateCollectedItemsUI();

        if (specialObjectPanel != null)
            specialObjectPanel.SetActive(false);

        if (activateSpecialButton != null)
            activateSpecialButton.onClick.AddListener(ActivateSpecialObject);
 
        if (specialObject != null)
            specialObject.SetActive(false);
    }

    public void CollectItem(CollectibleItem item)
    {

        if (collectedItemIDs.Contains(item.itemID))
        {
            Debug.Log($"Ya tienes el objeto: {item.itemName}");
            return;
        }

        collectedItemIDs.Add(item.itemID);
        collectedItemNames.Add(item.itemName);
        
        Debug.Log($"Objeto recolectado: {item.itemName} (ID: {item.itemID})");

        UpdateCollectedItemsUI();

        Destroy(item.gameObject);

        if (collectedItemIDs.Count >= totalItemsNeeded)
        {
            ShowSpecialObjectPanel();
        }
    }

    void UpdateCollectedItemsUI()
    {
        if (collectedItemsText != null)
        {
            collectedItemsText.text = $"Objetos recolectados: {collectedItemIDs.Count}/{totalItemsNeeded}";
        }
    }

    void ShowSpecialObjectPanel()
    {
        if (specialObjectPanel != null)
        {
            specialObjectPanel.SetActive(true);
            
            if (specialPanelText != null)
            {
                specialPanelText.text = "HAS CONSEGUIDO LOS 3 OBJETOS, FABRICA TU SOMBRERO";
            }
            
            Debug.Log("¡Todos los objetos recolectados! Panel especial mostrado.");
        }
    }

    public void ActivateSpecialObject()
    {
        if (specialObject != null)
        {
            specialObject.SetActive(true);
            Debug.Log("¡Objeto especial activado!");
        }

        if (specialObjectPanel != null)
        {
            specialObjectPanel.SetActive(false);
        }
    }

    public int GetCollectedItemsCount()
    {
        return collectedItemIDs.Count;
    }
    
    public bool HasAllItems()
    {
        return collectedItemIDs.Count >= totalItemsNeeded;
    }
    
    public List<string> GetCollectedItemNames()
    {
        return new List<string>(collectedItemNames);
    }

    public void ResetCollection()
    {
        collectedItemIDs.Clear();
        collectedItemNames.Clear();
        UpdateCollectedItemsUI();
        
        if (specialObjectPanel != null)
            specialObjectPanel.SetActive(false);
            
        if (specialObject != null)
            specialObject.SetActive(false);
    }
}