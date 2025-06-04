using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Configuración del Objeto")]
    public string itemName = "Objeto Recolectable";
    public int itemID = 0;

    [Header("Efectos Visuales")]
    public bool rotateObject = true;
    public float rotationSpeed = 50f;

    void Start()
    {

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        else
        {
            Debug.LogWarning($"El objeto recolectable {itemName} no tiene Collider!");
        }
    }

    void Update()
    {
        if (rotateObject)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCollector collector = other.GetComponent<PlayerCollector>();
            if (collector != null)
            {
                collector.CollectItem(this);
            }
        }
    }
}