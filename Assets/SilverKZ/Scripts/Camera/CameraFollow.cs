using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField] private Transform _target;

	public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
							   //public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
	public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

	// *** private Transform m_Player; // Reference to the player's transform.


	private void Awake()
	{
		// Setting up the reference.
		// *** m_Player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	private bool CheckXMargin()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return (transform.position.x - _target.position.x) < xMargin;
	}


	//private bool CheckYMargin()
	//{
	// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
	//return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
	//}


	private void Update()
	{
		TrackPlayer();
	}


	private void TrackPlayer()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		// If the player has moved beyond the x margin...
		if (CheckXMargin())
		{
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp(transform.position.x, _target.position.x, xSmooth * Time.deltaTime);
		}

		// If the player has moved beyond the y margin...
		//if (CheckYMargin())
		//{
		// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
		//targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth * Time.deltaTime);
		//}

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
		targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
		//targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
	}



	/*
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 0.15f;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector2 _minPos; 
    [SerializeField] private Vector2 _maxPos;
    [SerializeField] private float _lookAheadDistance = 2f;
    
    private float _currentLookAhead = 0f;

    private void LateUpdate()
    {
        if (_target == null) 
            return;

        float moveDir = Mathf.Sign(_target.localScale.x);
        _currentLookAhead = Mathf.Lerp(_currentLookAhead, moveDir * _lookAheadDistance, Time.deltaTime * 2f);

        Vector3 desiredPosition = _target.position + _offset + new Vector3(_currentLookAhead, 0, 0);

        float clampedX = Mathf.Clamp(desiredPosition.x, _minPos.x, _maxPos.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, _minPos.y, _maxPos.y);

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, _smoothSpeed);
    }
    */
}
