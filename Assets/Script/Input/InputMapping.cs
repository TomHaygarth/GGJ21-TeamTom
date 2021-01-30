using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "player_controls", menuName = "Data/Input/Input Mapping")]
public class InputMapping : ScriptableObject
{
    public KeyCode UpKey = KeyCode.W;
    public KeyCode DownKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;

    public KeyCode ScanKey = KeyCode.Space;
    public KeyCode DigKey = KeyCode.E;

    public float DirectionDeadzone = 0.05f;
}
