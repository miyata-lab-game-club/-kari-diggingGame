using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer middleGroundSpriteRenderer;
    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private TextMeshProUGUI gemText;
    private int gemNumber;
    private Texture2D originalTexture;
    private Color[] initialPixels;
    private Texture2D originalMiddleTexture;
    private Color[] middleInitialPixels;

    private Texture2D groundTexture;
    private Texture2D middleGroundTexture;

    private bool sarch;// �T�[�`�����ǂ���

    private void Start()
    {
        originalTexture = groundSpriteRenderer.sprite.texture;
        initialPixels = originalTexture.GetPixels();
        originalMiddleTexture = middleGroundSpriteRenderer.sprite.texture;
        middleInitialPixels = originalMiddleTexture.GetPixels();
        groundTexture = (Texture2D)groundSpriteRenderer.sprite.texture;
        middleGroundTexture = (Texture2D)middleGroundSpriteRenderer.sprite.texture;
        DigHole(middleGroundTexture, player.transform.position, 20);
        DigHole(groundTexture, player.transform.position, 30);
        gemNumber = 0;
    }

    private void Update()
    {
        if (sarch == false)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += new Vector3(0, -1, 0) * Time.deltaTime;
                DigHole(middleGroundTexture, player.transform.position, 20);
                DigHole(groundTexture, player.transform.position, 30);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
                DigHole(middleGroundTexture, player.transform.position, 20);
                DigHole(groundTexture, player.transform.position, 30);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
                DigHole(middleGroundTexture, player.transform.position, 20);
                DigHole(groundTexture, player.transform.position, 30);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime;
                DigHole(middleGroundTexture, player.transform.position, 20);
                DigHole(groundTexture, player.transform.position, 30);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("���s");
                // �T�[�`
                StartCoroutine(
                SarchAround(player.transform.position, 200));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("���s");
        if (collision.gameObject.tag == "gem")
        {
            gemNumber++;
            gemText.text = "x " + gemNumber.ToString();
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator SarchAround(Vector2 position, int radius)
    {
        sarch = true;
        Color[] tmpGroundPixels = groundTexture.GetPixels();
        Color[] tmpMiddleGroundPixels = middleGroundTexture.GetPixels();
        //groundTexture
        //middleGroundTexture
        // �e�N�X�`�����W�ɕϊ�
        Vector2Int pixelPos = WorldToTextureCoord(position);
        // ���̔��a�ɂ��ǂ��ăe�N�X�`���𑀍�
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (!IsPixelInsideTexture(pixelPos, groundTexture))
                {
                    continue;
                }
                // �s�N�Z�����W���v�Z
                Vector2Int pixelOffset = new Vector2Int(x, y);
                Vector2Int pixel = pixelPos + pixelOffset;

                if (!IsPixelInsideTexture(pixel, groundTexture))
                {
                    continue;
                }
                if (pixelOffset.magnitude <= radius)
                {
                    Color transparentColor = groundTexture.GetPixel(pixel.x, pixel.y);
                    transparentColor.a = 0.3f;
                    groundTexture.SetPixel(pixel.x, pixel.y, transparentColor);
                }
            }
        }

        groundTexture.Apply();
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (!IsPixelInsideTexture(pixelPos, middleGroundTexture))
                {
                    continue;
                }
                // �s�N�Z�����W���v�Z
                Vector2Int pixelOffset = new Vector2Int(x, y);
                Vector2Int pixel = pixelPos + pixelOffset;

                if (!IsPixelInsideTexture(pixel, middleGroundTexture))
                {
                    continue;
                }
                if (pixelOffset.magnitude <= radius)
                {
                    Color transparentColor = middleGroundTexture.GetPixel(pixel.x, pixel.y);
                    transparentColor.a = 0.3f;
                    middleGroundTexture.SetPixel(pixel.x, pixel.y, transparentColor);
                }
            }
        }
        middleGroundTexture.Apply();
        yield return new WaitForSeconds(1);
        groundTexture.SetPixels(tmpGroundPixels);
        middleGroundTexture.SetPixels(tmpMiddleGroundPixels);
        groundTexture.Apply();
        middleGroundTexture.Apply();
        sarch = false;
    }

    private void DigHole(Texture2D texture, Vector2 position, int radius)
    {
        // �e�N�X�`�����W�ɕϊ�
        Vector2Int pixelPos = WorldToTextureCoord(position);
        // ���̔��a�ɂ��ǂ��ăe�N�X�`���𑀍�

        // ���̔��a�ɂ��ǂ��ăe�N�X�`���𑀍�
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (!IsPixelInsideTexture(pixelPos, texture))
                {
                    continue;
                }
                // �s�N�Z�����W���v�Z
                Vector2Int pixelOffset = new Vector2Int(x, y);
                Vector2Int pixel = pixelPos + pixelOffset;

                if (!IsPixelInsideTexture(pixel, texture))
                {
                    continue;
                }
                if (pixelOffset.magnitude <= radius)
                {
                    Color transparentColor = new Color(0, 0, 0, 0);
                    texture.SetPixel(pixel.x, pixel.y, transparentColor);
                }
            }
        }
        texture.Apply();
    }

    private bool IsPixelInsideTexture(Vector2Int pixel, Texture2D texture)
    {
        int width = texture.width;
        int height = texture.height;
        return pixel.x >= 0 && pixel.x < width && pixel.y >= 0 && pixel.y < height;
    }

    private Vector2Int WorldToTextureCoord(Vector2 worldPos)
    {
        Vector2 textureSize = new Vector2(groundSpriteRenderer.sprite.texture.width, groundSpriteRenderer.sprite.texture.height);
        float pixelPerUnit = groundSpriteRenderer.sprite.pixelsPerUnit;

        // ���[���h���W���X�v���C�g�̒��S�����_�Ƃ������΍��W�ɕϊ�
        Vector2 localPos = worldPos - (Vector2)groundSpriteRenderer.transform.position;
        int texturX = (int)(textureSize.x / 2 + localPos.x * pixelPerUnit);
        int textureY = (int)(textureSize.y / 2 + localPos.y * pixelPerUnit);

        Vector2Int pixelCoord = new Vector2Int(
            texturX, textureY
        );

        return pixelCoord;
    }

    public void ResetTexture()
    {
        Texture2D texture = (Texture2D)groundSpriteRenderer.sprite.texture;
        texture.SetPixels(initialPixels);
        texture.Apply();
        texture = (Texture2D)middleGroundSpriteRenderer.sprite.texture;
        texture.SetPixels(middleInitialPixels);
        texture.Apply();
        //groundSpriteRenderer.sprite = Sprite.Create(originalTexture, groundSpriteRenderer.sprite.rect, groundSpriteRenderer.sprite.pivot);
    }
}