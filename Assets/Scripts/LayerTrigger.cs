using UnityEngine;

public class LayerTrigger : MonoBehaviour
{
    public int layerNumber = 1;
    private float stayTimer = 0f;
    private bool fruitInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Fruit")) return;

        FruitSelector.instance.FruitEnteredLayer(layerNumber);
        stayTimer = 0f;
        fruitInside = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Fruit")) return;

        stayTimer += Time.deltaTime;

        if (!fruitInside && stayTimer >= 0.5f)
        {
            FruitSelector.instance.SetLayerFromTrigger(layerNumber);
            fruitInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Fruit")) return;

        FruitSelector.instance.FruitExitedLayer(layerNumber);
        stayTimer = 0f;
        fruitInside = false;
    }
}
