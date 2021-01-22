using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config param
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockVFX;
    int maxHits;
    [SerializeField] Sprite[] hitSprites;

    // Cached Reference
    Level level;
    GameStatus gameStatus;

    // State Variables
    [SerializeField] int hitCount;

    private void Start()
    {
        CountBreakableBlocks();

        gameStatus = FindObjectOfType<GameStatus>();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();

        if (tag == "Breakable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            maxHits = hitSprites.Length + 1;
            hitCount++;
            if (hitCount >= maxHits)
            {
                DestroyBlock();
            }
            else
            {
                ShowNextHitSprite();
                gameStatus.AddToScore();
            }
            
        }
    }

    private void ShowNextHitSprite()
    {
        GetComponent<SpriteRenderer>().sprite = hitSprites[hitCount - 1];
    }

    private void DestroyBlock()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
        Destroy(gameObject);
        level.CountBrokenBlocks();
        gameStatus.AddToScore();
        TriggerVFX();
    }

    private void TriggerVFX()
    {
        GameObject sparkles = Instantiate(blockVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
