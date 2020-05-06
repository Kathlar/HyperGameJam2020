using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    #region PlayerInputTypes
    public interface IPlayerInputType
    {
        void Update();
        void Clear();
    }

    public abstract class PlayerInputType<T> : IPlayerInputType
    {
        public T name { get; protected set; }
        public InputControlType controlType { get; protected set; } = InputControlType.None;

        public PlayerInputType<T> SetUp(T t)
        {
            name = t;
            return this;
        }

        public PlayerInputType<T> SetUp(InputControlType controlType)
        {
            this.controlType = controlType;
            return this;
        }

        public abstract void Clear();

        public abstract void Update();
    }

    [System.Serializable]
    public class PlayerInputAxis : PlayerInputType<string>
    {
        public float value;// { get; private set; }

        public PlayerInputAxis(string name)
        {
            SetUp(name);
        }

        public override void Update()
        {
            value = Input.GetAxis(name);
            if (controlType != InputControlType.None)
                value += InputManager.ActiveDevice.GetControl(controlType).Value;
        }

        public override void Clear()
        {
            value = 0;
        }
    }

    public abstract class PlayerInputButton<T> : PlayerInputType<T>
    {
        public bool wasPressed { get; protected set; }
        public bool isPressed { get; protected set; }
        public bool wasReleased { get; protected set; }

        public override void Update()
        {
            wasPressed = controlType != InputControlType.None &&
                InputManager.ActiveDevice.GetControl(controlType).WasPressed;
            isPressed = controlType != InputControlType.None &&
                InputManager.ActiveDevice.GetControl(controlType).IsPressed;
            wasReleased = controlType != InputControlType.None &&
                InputManager.ActiveDevice.GetControl(controlType).WasReleased;
        }

        public override void Clear()
        {
            wasPressed = isPressed = wasReleased = false;
        }
    }

    [System.Serializable]
    public class PlayerInputMouseButton : PlayerInputButton<int>
    {
        public PlayerInputMouseButton(int number)
        {
            SetUp(number);
        }

        public override void Update()
        {
            base.Update();
            wasPressed = wasPressed || Input.GetMouseButtonDown(name);
            isPressed = isPressed || Input.GetMouseButton(name);
            wasReleased = wasReleased || Input.GetMouseButtonUp(name);
        }
    }

    [System.Serializable]
    public class PlayerInputKeyCode : PlayerInputButton<KeyCode>
    {
        public override void Update()
        {
            base.Update();
            wasPressed = wasPressed || Input.GetKeyDown(name);
            isPressed = isPressed || Input.GetKey(name);
            wasReleased = wasReleased || Input.GetKeyUp(name);
        }
    }
    #endregion


    [System.Serializable]
    public class InputValues
    {
        public Vector2 mousePosition;

        public List<IPlayerInputType> inputs = new List<IPlayerInputType>();
        public PlayerInputMouseButton leftMouse, rightMouse;
        public PlayerInputAxis mouseX, mouseY;
        public PlayerInputAxis horizontal, vertical;
        public PlayerInputAxis mouseScrollWheel;

        public InputValues()
        {
            inputs.Add(leftMouse = new PlayerInputMouseButton(0));
            inputs.Add(rightMouse = new PlayerInputMouseButton(1));
            inputs.Add(mouseX = new PlayerInputAxis("Mouse X"));
            inputs.Add(mouseY = new PlayerInputAxis("Mouse Y"));
            inputs.Add(mouseScrollWheel = new PlayerInputAxis("Mouse ScrollWheel"));

            inputs.Add(horizontal = new PlayerInputAxis("Horizontal"));
            inputs.Add(vertical = new PlayerInputAxis("Vertical"));

            leftMouse.SetUp(InputControlType.RightTrigger);
            rightMouse.SetUp(InputControlType.LeftTrigger);
            mouseX.SetUp(InputControlType.LeftStickX);
            mouseY.SetUp(InputControlType.LeftStickY);
            mouseScrollWheel.SetUp(InputControlType.RightStickX);
        }

        public void Update()
        {
            if (GameManager.GameStatus != GameStatus.Gameplay) return;
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            foreach (IPlayerInputType input in inputs)
                input.Update();
        }

        public void ClearValues()
        {
            foreach (IPlayerInputType input in inputs)
                input.Clear();
        }
    }
    [SerializeField] private InputValues values;
    public static InputValues Values { get { return Instance.values; } }

    protected override void Awake()
    {
        base.Awake();

        values = new InputValues();

        SetCursor(false);
    }

    private void Update()
    {
        values.Update();
    }

    public static void SetCursor(bool on)
    {
        Cursor.lockState = on ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = on;
    }

    public static void ClearValues()
    {
        Values.ClearValues();
    }
}
