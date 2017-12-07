using UnityEngine;

/// <summary>
/// Simple animations for gameobjects
/// </summary>
public class Animation : MonoBehaviour
{
    public AnimationProperties properties;

    public bool startOnEnable = false;

    private bool animationEnabled = false;

    private float _internalTimer = 0;
    private Quaternion _startRotation;
    private Vector3 _startPosition;

    public void Start()
    {
        if (!startOnEnable) return;

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        StartAnimation();
    }

    public void SetProperties(AnimationProperties properties)
    {
        this.properties = properties;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void Update()
    {
        //  Dont run it if not started
        if (!this.animationEnabled) return;

        //  Get the reference
        AnimationType type = properties.animationType;

        //  Rotation
        Vector3 nextRotation = transform.rotation.eulerAngles;
        if ((type & AnimationType.ROTATE_X) != 0) nextRotation.x += properties.rotationSpeed * Time.deltaTime;
        if ((type & AnimationType.ROTATE_Y) != 0) nextRotation.y += properties.rotationSpeed * Time.deltaTime;
        if ((type & AnimationType.ROTATE_Z) != 0) nextRotation.z += properties.rotationSpeed * Time.deltaTime;

        //  Position
        Vector3 nextPosition = transform.position;
        if ((type & AnimationType.FLOAT_UPDOWN) != 0)
            nextPosition.y = _startPosition.y + Mathf.Sin(properties.floatingSpeed * _internalTimer) * properties.floatingHeight;

        _internalTimer += Time.deltaTime;

        //  Set the rotation and position
        transform.rotation = Quaternion.Euler(nextRotation);
        transform.position = nextPosition;
    }

    /// <summary>
    /// Resets the animation to its starting rotations and position.
    /// </summary>
    public void ResetAnimation()
    {
        _internalTimer = 0;
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        StopAnimation();
    }

    public void StartAnimation()
    {
        this.animationEnabled = true;
    }

    public void StopAnimation()
    {
        this.animationEnabled = false;
    }

    public static void Example()
    {
        GameObject myGameObject = new GameObject();
        Animation animation = myGameObject.AddComponent<Animation>();
        animation.SetProperties(new AnimationProperties()
        {
            animationType = AnimationType.FLOAT_UPDOWN,
            floatingSpeed = 2,
            floatingHeight = 1
        });

        animation.StartAnimation();
        animation.ResetAnimation();
        animation.StopAnimation();
    }
}

[System.Serializable()]
public struct AnimationProperties
{
    public AnimationType animationType;
    public float rotationSpeed;
    public float floatingSpeed;
    public float floatingHeight;
}

public enum AnimationType
{
    NONE = 0,
    FLOAT_UPDOWN = 1,
    ROTATE_X = 2,
    ROTATE_Y = 4,
    ROTATE_Z = 8,
    FLOAT_UPDOWN_AND_ROTATE_X = FLOAT_UPDOWN | ROTATE_X,
    FLOAT_UPDOWN_AND_ROTATE_Y = FLOAT_UPDOWN | ROTATE_Y,
    FLOAT_UPDOWN_AND_ROTATE_Z = FLOAT_UPDOWN | ROTATE_Z
}
