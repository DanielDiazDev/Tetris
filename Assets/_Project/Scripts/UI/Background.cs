using System;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        FitToScreen();
    }

    private void OnEnable()
    {
        FitToScreen();
    }

    private void FitToScreen()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        float worldScreenHeight = mainCamera.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        float scale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(scale, scale, 1f);

        transform.position = new Vector3(
            mainCamera.transform.position.x,
            mainCamera.transform.position.y,
            transform.position.z
        );
    }
}
