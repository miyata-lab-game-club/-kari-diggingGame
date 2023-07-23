using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundColliderManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer middleGroundSpriteRenderer;
    private Texture2D middleGroundTexture;
    [SerializeField] private PolygonCollider2D groundCollider;

    // Start is called before the first frame update
    private void Start()
    {
        middleGroundTexture = (Texture2D)middleGroundSpriteRenderer.sprite.texture;
        //UpdateGroundCollider();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateGroundCollider()
    {
        middleGroundTexture = (Texture2D)middleGroundSpriteRenderer.sprite.texture;
        int width = middleGroundTexture.width;
        int height = middleGroundTexture.height;
        Color[] pixels = middleGroundTexture.GetPixels();
        // コライダーの二番目以降があったら破棄する
        if (groundCollider.pathCount > 1)
        {
            groundCollider.pathCount = 1;
        }
        // コライダーの座標
        // 境界線

        int hasBorder = SarchBorder(pixels);

        if (hasBorder == 1)
        {
            // 輪郭線からパスの頂点を求める
            List<Vector2> vertexs = SarchPass();
            Debug.Log(vertexs.Count);
            // コライダーを追加する
            groundCollider.pathCount++;
            Debug.Log(groundCollider.pathCount);
            groundCollider.SetPath(groundCollider.pathCount - 1, vertexs);
        }
        else
        {
            // なにもしない
        }
    }

    private struct sarch8SidesResult
    {
        public int sarchIndex;
        public Vector2Int sarchPos;
    }

    private List<Vector2Int> borders = new List<Vector2Int>();

    private int SarchBorder(Color[] pixels)
    {
        Debug.Log("実行");
        int width = middleGroundTexture.width;
        int height = middleGroundTexture.height;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (pixels[x + y * width].a < 0.5f)
                {
                    sarch8SidesResult firstPoint;
                    firstPoint.sarchPos = new Vector2Int(x, y);
                    firstPoint.sarchIndex = 0;
                    borders.Add(firstPoint.sarchPos);
                    sarch8SidesResult secondPoint = Sarch8Sides(firstPoint, pixels); Debug.Log(x + "," + y);
                    Debug.Log(secondPoint.sarchPos);
                    // 輪郭線が途切れた
                    if (secondPoint.sarchIndex == -1)
                    {
                        return 0;
                    }
                    else
                    {
                        borders.Add(secondPoint.sarchPos);
                    }
                    int k = 1;
                    //borders[borders.Count - 1] != borders[0]
                    sarch8SidesResult previousResult = secondPoint;
                    while (borders[borders.Count - 1] != firstPoint.sarchPos)
                    {
                        sarch8SidesResult nextPoint = Sarch8Sides(previousResult, pixels);
                        if (nextPoint.sarchIndex == -1)
                        {
                            return 0;
                        }
                        else
                        {
                            borders.Add(nextPoint.sarchPos);
                            previousResult = nextPoint;
                            k++;
                        }
                    }
                    //borders.RemoveAt(borders.Count - 1);
                    borders.Add(borders[1]);
                    borders.Add(borders[2]);
                    return 1;
                }
            }
        }

        return 1;
    }

    private List<Vector2> SarchPass()
    {
        List<Vector2> vertexs = new List<Vector2>();
        float previousDiffX, previousDiffY, currentDiffX, currentDiffY;
        float previousDiff, currentDiff;
        for (int i = 0; i < borders.Count - 3; i++)
        {
            previousDiffX = borders[i + 1].x - borders[i].x;
            previousDiffY = borders[i + 1].y - borders[i].y;
            currentDiffX = borders[i + 2].x - borders[i + 1].x;
            currentDiffY = borders[i + 2].y - borders[i + 1].y;
            if (previousDiffY == 0)
            {
                previousDiffY = 0.0001f;
            }
            if (currentDiffY == 0)
            {
                currentDiffY = 0.0001f;
            }
            previousDiff = previousDiffX / previousDiffY;
            currentDiff = currentDiffX / currentDiffY;
            Debug.Log("previousDiff:" + previousDiff + " currentDiff:" + currentDiff);
            if (previousDiff != currentDiff)
            {
                // 座標変換
                Vector2 zahyo = TextureCoordToWorld(borders[i]);
                Debug.Log("座標" + zahyo);
                vertexs.Add(zahyo);
            }
        }
        return vertexs;
    }

    // 8近傍を探索する
    private sarch8SidesResult Sarch8Sides(sarch8SidesResult previousResult, Color[] pixels)
    {
        int width = middleGroundTexture.width;
        int height = middleGroundTexture.height;
        sarch8SidesResult result;
        result.sarchPos = new Vector2Int(width + 1, height + 1);
        result.sarchIndex = -1;
        //左上、上、右上、右、右下、下、左下、左
        Vector2Int[] sarchDiff = { new Vector2Int(-1, 1) , new Vector2Int(0, 1) ,
                new Vector2Int(1, 1) ,new Vector2Int(1,0),new Vector2Int(1,-1),new Vector2Int(0,-1),new Vector2Int(-1, -1) , new Vector2Int(-1, 0) };
        int sarchIndex = (previousResult.sarchIndex + 6) % 8;
        for (int i = 0; i < sarchDiff.Length; i++)
        {
            Vector2Int sarchPos = previousResult.sarchPos + sarchDiff[sarchIndex];
            if (sarchPos.x >= 0 && sarchPos.x <= width && sarchPos.y >= 0 && sarchPos.y <= height)
            {
                if (pixels[sarchPos.x + sarchPos.y * middleGroundTexture.width].a < 0.5f)
                {
                    result.sarchPos = sarchPos;
                    result.sarchIndex = sarchIndex;
                    return result;
                }
            }

            sarchIndex += 1;
            sarchIndex %= 8;
        }
        return result;
    }

    private Vector2 TextureCoordToWorld(Vector2 texturePos)
    {
        int width = middleGroundTexture.width;
        int height = middleGroundTexture.height;
        Vector2 textureSize = new Vector2(width, height);
        float pixelPerUnit = 100;

        Vector2 worldPosition;
        float worldPosX = (texturePos.x - textureSize.x / 2) / pixelPerUnit;
        float worldPosY = (texturePos.y - textureSize.y / 2) / pixelPerUnit;
        //int texturX = (int)(textureSize.x / 2 + localPos.x * pixelPerUnit);
        //int textureY = (int)(textureSize.y / 2 + localPos.y * pixelPerUnit);
        worldPosition = new Vector2(worldPosX, worldPosY);

        return worldPosition;
    }
}