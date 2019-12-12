using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    const float moveUp = 3.75f;     // How high each block is above the previous
    const float riseSpeed = 10.0f;  // The speed at which each block ascends when a hit is taken
    const float moveRight = 7.5f;

    [SerializeField] private Transform      healthBase;     // The space in the UI where the health bar begins
    [SerializeField] private RectTransform  lifeBase;       // The point in UI space where the first block will be laid
    [SerializeField] private Health         playerHealth;   // The object that stores and manages the player's health
    [SerializeField] private Lives          playerLives;    // The object that stores and manages the player's health
    [SerializeField] private Color[]        colors;         // The colors that each block can have
    [SerializeField] private GameObject     blockPrefab;    // The prefab representing a single unit of health
    [SerializeField] private GameObject     livesPrefab;    // The prefab representing a life

    private Stack<GameObject> health;
    private Stack<GameObject> lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = new Stack<GameObject>();

        if (livesPrefab != null)
        {
            for (int i = 0; i < playerLives.lives; i++)
            {
                lives.Push(Instantiate(livesPrefab, lifeBase));
                lives.Peek().transform.Translate(new Vector3(moveRight * i, 0, 0), Space.World);
            }
        }

        ResetHealthBar();
    }

    public void RemoveLife()
    {
        Destroy(lives.Pop());
    }

    public IEnumerator RemoveHealth()
    {
        GameObject currentBlock = health.Peek();
        Color currentColor = currentBlock.GetComponent<Renderer>().material.GetColor("_BaseColor");

        health.Pop();
        currentBlock.GetComponent<SpinInPlace>().rotSpeed = 0;

        do
        {
            currentBlock.transform.Translate(new Vector3(0, riseSpeed * Time.deltaTime, 0), Space.World);
            currentColor.a -= Time.deltaTime;
            currentBlock.GetComponent<Renderer>().material.SetColor("_BaseColor", currentColor);
            yield return null;
        } while (currentColor.a > 0);

        Destroy(currentBlock);
    }

    public void ResetHealthBar()
    {
        health = new Stack<GameObject>();

        if (blockPrefab != null)
        {
            GameObject newBlock;

            for (int i = 0; i < playerHealth.life; i++)
            {
                newBlock = Instantiate(blockPrefab, healthBase);
                newBlock.transform.Translate(new Vector3(0, moveUp * i, 0), Space.World);

                if (i >= colors.Length)
                    newBlock.GetComponent<Renderer>().material.SetColor("_BaseColor", colors[colors.Length - 1]);
                else
                    newBlock.GetComponent<Renderer>().material.SetColor("_BaseColor", colors[i]);

                health.Push(newBlock);
            }
        }
    }
}
