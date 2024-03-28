using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Rigidbody _rbPlayerMove;

    public Player player;
    Vector3 cacheJoystick = Vector3.zero;
    public float speedMove;
    public float speedRotate;
    private void Update()
    {
        MovePlayer();
    }
    public void MovePlayer()
    {
        cacheJoystick.x = joystick.Horizontal;
        cacheJoystick.z = joystick.Vertical ;
        cacheJoystick = cacheJoystick.normalized;
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 direction = Vector3.RotateTowards(player.transform.forward, cacheJoystick * speedMove * Time.deltaTime, speedRotate * Time.deltaTime, 0);
            player.transform.rotation = Quaternion.LookRotation(direction);
            player.OnMoing();
        }
        else
        {
            player.OnIdle();
        }
        _rbPlayerMove.MovePosition(_rbPlayerMove.position + cacheJoystick * speedMove * Time.deltaTime);
    }
}
