using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    bool isDown;
    Vector3 mouseStartPos;
    Vector2 contentSize;
    Vector2 contentOffset;
    BoxCollider2D cameraCollider;
    Camera mainCamera;
    float initialOrthographicSize;
    float minZoomSize; // ズームインの最小サイズ
    float maxZoomSize; // ズームアウトの最大サイズ

    [SerializeField] GameObject mapSprite;  // インスペクターから設定するためのゲームオブジェクト
    [SerializeField] float zoomSpeed = 1.0f;  // ズーム速度

    private Coroutine zoomCoroutine; // ズームアニメーションのコルーチン

    void Awake()
    {
        mainCamera = Camera.main;

        // カメラのコリジョンをCameraの表示範囲に合わせてサイズ設定
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        cameraCollider = GetComponent<BoxCollider2D>();
        cameraCollider.size = new Vector2(cameraWidth, cameraHeight);

        // コンテンツ（マップ）のサイズとオフセットを取得
        var sr = mapSprite.GetComponent<SpriteRenderer>();
        contentSize = new Vector2(sr.bounds.size.x, sr.bounds.size.y);
        contentOffset = new Vector2(mapSprite.transform.position.x, mapSprite.transform.position.y);

        // 初期の正射投影サイズを記録
        initialOrthographicSize = mainCamera.orthographicSize;

        // ズームインの最小サイズを設定
        minZoomSize = initialOrthographicSize * 0.9f;
        // ズームアウトの最大サイズを計算（縦か横の小さい方を基準に）
        float cameraAspect = mainCamera.aspect;
        float maxZoomSizeBasedOnSprite = Mathf.Min(contentSize.x / (2 * cameraAspect), contentSize.y / 2);
        maxZoomSize = maxZoomSizeBasedOnSprite;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTouchDown();
        }
        if (Input.GetMouseButton(0))
        {
            if (isDown)
            {
                OnTouchMove();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDown = false;
        }

        // マウスホイールのズーム処理
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            ZoomCamera(scroll);
        }

        // カメラがcontent外に出てしまった場合、カメラをcontent内に戻す
        Vector3 currentPosition = transform.position;
        float cameraHalfWidth = cameraCollider.size.x / 2;
        float cameraHalfHeight = cameraCollider.size.y / 2;

        float contentHalfWidth = contentSize.x / 2;
        float contentHalfHeight = contentSize.y / 2;

        currentPosition.x = Mathf.Clamp(currentPosition.x, contentOffset.x - contentHalfWidth + cameraHalfWidth, contentOffset.x + contentHalfWidth - cameraHalfWidth);
        currentPosition.y = Mathf.Clamp(currentPosition.y, contentOffset.y - contentHalfHeight + cameraHalfHeight, contentOffset.y + contentHalfHeight - cameraHalfHeight);

        transform.position = currentPosition;
    }

    private void OnTouchDown()
    {
        isDown = true;
        mouseStartPos = Input.mousePosition;
    }

    private void OnTouchMove()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 mouseStartWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseStartPos.x, mouseStartPos.y, 0));
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

        Vector3 movingDistance = mouseStartWorld - mouseWorld;
        transform.position += new Vector3(movingDistance.x, movingDistance.y, 0);
        mouseStartPos = mousePos;

        Vector2 colliderOffset = cameraCollider.offset;
        float limitR = (contentSize.x - cameraCollider.size.x) / 2 - colliderOffset.x + contentOffset.x;
        float limitL = (contentSize.x - cameraCollider.size.x) / -2 - colliderOffset.x + contentOffset.x;
        float limitT = (contentSize.y - cameraCollider.size.y) / 2 - colliderOffset.y + contentOffset.y;
        float limitB = (contentSize.y - cameraCollider.size.y) / -2 - colliderOffset.y + contentOffset.y;

        Vector3 currentPosition = transform.position;

        if (contentSize.x > cameraCollider.size.x)
        {
            currentPosition.x = Mathf.Clamp(currentPosition.x, limitL, limitR);
        }

        if (contentSize.y > cameraCollider.size.y)
        {
            currentPosition.y = Mathf.Clamp(currentPosition.y, limitB, limitT);
        }
        transform.position = currentPosition;
    }

    void ZoomCamera(float zoomDelta)
    {
        float newOrthographicSize = mainCamera.orthographicSize - zoomDelta * zoomSpeed;
        // ズームサイズを制限
        newOrthographicSize = Mathf.Clamp(newOrthographicSize, minZoomSize, maxZoomSize);

        mainCamera.orthographicSize = newOrthographicSize;

        // ズーム後にカメラのコリジョンを更新
        float cameraHeight = 2f * mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        cameraCollider.size = new Vector2(cameraWidth, cameraHeight);
    }

    public void ZoomIn()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomToSize(minZoomSize));
    }

    public void ZoomOut()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomToSize(maxZoomSize));
    }

    private IEnumerator ZoomToSize(float targetSize)
    {
        float initialSize = mainCamera.orthographicSize;
        float animationDuration = 0.5f;
        float timeElapsed = 0f;

        while (timeElapsed < animationDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / animationDuration;

            // 初速が早く、最後に速度を落とすようにイージングを使用
            float easeT = 1f - Mathf.Pow(1f - t, 3); // カスタムイージング関数（3次のイージング）

            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, easeT);

            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            cameraCollider.size = new Vector2(cameraWidth, cameraHeight);

            yield return null;
        }

        mainCamera.orthographicSize = targetSize;

        float finalCameraHeight = 2f * mainCamera.orthographicSize;
        float finalCameraWidth = finalCameraHeight * mainCamera.aspect;
        cameraCollider.size = new Vector2(finalCameraWidth, finalCameraHeight);

        zoomCoroutine = null;
    }
}
