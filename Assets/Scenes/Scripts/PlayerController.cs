using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;

    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public CharacterController characterController;

        private void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            characterController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<NetworkTransform>().clientAuthority = true;
        }

        public override void OnStartLocalPlayer()
        {
            characterController.enabled = true;
        }

        [Header("Movement Settings")]
        public float moveSpeed = 8f;
        public float turnSensitivity = 5f;
        public float maxTurnSpeed = 100f;

        [Header("Diagnostics")]
        public float horizontal;
        public float vertical;
        public float turn;
        public float jumpSpeed;
        public bool isGrounded = true;
        public bool isFalling;
        public Vector3 velocity;

        private bool _isDash;
        private bool _isColorChanged = false;
        [SerializeField]
        private int _waitColorChangedSeconds = 3; // it should be DotWeen duration too

        [SerializeField]
        private TextMeshPro _hitCountText;
        
        private int _hitCount = 0;

        private void Update()
        {
            if(_isColorChanged)
                return;
            
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            _isDash = Input.GetKey(KeyCode.Z);

            // Q and E cancel each other out, reducing the turn to zero
            if (Input.GetKey(KeyCode.Q))
                turn = Mathf.MoveTowards(turn, -maxTurnSpeed, turnSensitivity);
            if (Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, maxTurnSpeed, turnSensitivity);
            if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, 0, turnSensitivity);
            if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                turn = Mathf.MoveTowards(turn, 0, turnSensitivity);

            if (isGrounded)
                isFalling = false;

            if ((isGrounded || !isFalling) && jumpSpeed < 1f && Input.GetKey(KeyCode.Space))
            {
                jumpSpeed = Mathf.Lerp(jumpSpeed, 1f, 0.5f);
            }
            else if (!isGrounded)
            {
                isFalling = true;
                jumpSpeed = 0;
            }
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            transform.Rotate(0f, turn * Time.deltaTime, 0f);

            Vector3 direction = new Vector3(horizontal, jumpSpeed, vertical);
            direction = Vector3.ClampMagnitude(direction, 1f);
            direction = transform.TransformDirection(direction);
            direction *= moveSpeed;

            if (_isDash)
            {
                characterController.Move(transform.forward * 50 * Time.deltaTime);
            }
            else if (jumpSpeed > 0)
                characterController.Move(direction * Time.deltaTime);
            else
                characterController.SimpleMove(direction);

            isGrounded = characterController.isGrounded;
            velocity = characterController.velocity;
        }

        [ServerCallback]
        private void OnCollisionEnter(Collision collision)
        {
            return; //tood:
            Debug.Log("SERRRRVER");
            if (collision.gameObject.TryGetComponent(out PlayerController another)) //reaplce to TAG
            {
                if (another._isDash)
                {
                    characterController.Move(collision.impulse * 10);
                    StartCoroutine(R());
                }
            }
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            StopAllCoroutines();
            Debug.Log("AAAA");

            // if (_isDash)
            {
                _hitCountText.SetText((++_hitCount).ToString());

                if (_hitCount == 4)
                {
                    Debug.Log("CHANGEEE");
                    var aa = (NetworkRoomManagerExt)NetworkManager.singleton;
                    aa.AAA();
                    // aa.OnRoomServerSceneChanged(gameObject.scene.name);
                    // aa.OnRoomServerPlayersReady();
                }
                
                // characterController.Move(-velocity);
                StartCoroutine(R());
            }
        }

        [ServerCallback]
        IEnumerator R()
        {
            _isColorChanged = true;
            var rc = GetComponent<RandomColor>();
            var originalColor = rc.color;
            rc.color = Color.green;
            yield return new WaitForSeconds(_waitColorChangedSeconds);
            rc.color = originalColor;
            Debug.Log("SERRRRVER");
            _isColorChanged = false;
        }
    }
