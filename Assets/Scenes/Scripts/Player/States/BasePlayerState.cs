using UnityEngine;

public abstract class BasePlayerState : MonoBehaviour, IState
{
    protected IPlayer Player { get; private set; }
    protected IStateMachine<PlayerState> StateMachine { get; private set; }

    protected virtual void Awake()
    {
        Player = GetComponent<IPlayer>();
        StateMachine = GetComponent<IStateMachine<PlayerState>>();
        Exit();
    }

    public virtual void Enter() =>
        enabled = true;

    public virtual void Exit() =>
        enabled = false;

    protected void Move(Vector3 direction, float speed, Vector3 gravity)
    {
        Vector3 motion = direction * speed * Time.deltaTime;
        motion += gravity * Time.deltaTime;

        // Vector3 motion = (direction * speed + Physics.gravity) * Time.deltaTime;
        Player.Move(motion);
    }

    protected void LookAt(Vector3 direction, Vector3 upwards)
    {
        if (direction == Vector3.zero)
            return;

        if (direction.magnitude < Player.Data.MinMoveDistance)
            return;

        var d = direction.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(d, upwards); //mb smth with parent?

        Debug.Log($"{d.ToString("f4")} {direction.ToString("f4")} {direction.magnitude} {upwards.ToString("f4")} {upwards.magnitude}");

        transform.rotation = lookRotation; //Quaternion.Slerp(transform.rotation, lookRotation, Player.Data.RotationSpeed * Time.deltaTime);
    }

    protected bool IsDashing()
    {
        return Player.InputManager.ReadDashAction();
    }

    protected Vector3 GetMovementDirection()
    {
        Vector3 moveDirection = Player.InputManager.ReadMoveAction();

        if (moveDirection.magnitude < Player.Data.MinMoveDistance)
            return Vector3.zero;

        // Debug.Log(moveDirection);
        // if (moveDirection.x < Player.Data.MinMoveDistance)
        //     moveDirection.x = 0;
        //
        // if (moveDirection.y < Player.Data.MinMoveDistance)
        //     moveDirection.y = 0;
        //
        // if (moveDirection.z < Player.Data.MinMoveDistance)
        //     moveDirection.z = 0;

        return moveDirection;
    }

    protected bool TryGetGroundNormal(out Vector3 normal)
    {
        Vector3 down = -transform.up;

        if (Physics.Raycast(transform.position, down, out RaycastHit hit, 1.5f, ~(1 << global::Player.Layer)))
        {
            Debug.Log($"{hit.transform.name} down={down.ToString("f4")} {hit.normal.ToString("f4")}");

            normal = hit.normal;
            return true;
        }

        normal = Vector3.zero;
        return false;
    }

    protected static Vector3 ProjectOnGround(Vector3 direction, Vector3 normal)
    {
        return Vector3.ProjectOnPlane(direction, normal);
    }
}
