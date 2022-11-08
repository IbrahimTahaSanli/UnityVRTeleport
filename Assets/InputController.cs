using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR;

public class InputController: MonoBehaviour
{
    public static InputController Instance { get; private set; }

    public InputDevice leftController;
    public InputDevice rightController;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InputDevices.deviceConnected += InputDevices_deviceConnected;
    }

    private void InputDevices_deviceConnected(InputDevice obj)
    {
        if (obj.characteristics == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice |  InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left))
        {
           leftController = obj;
        }
        else if (obj.characteristics == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right))
        {
            rightController = obj;
        }
        else if(obj.characteristics == (InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice))
            Application.targetFrameRate = (int)XRDevice.refreshRate;

        
    }

    // Update is called once per frame
    void Update()
    {

        if (leftController.isValid)
        {
            Vector2 val;

            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out val);

            if (val.magnitude > 0)
                Teleport.Instance.ThumbStickEvent(val);
        }
    }

    public void GetInput(Controller cont, Inputs input, out float value)
    {
        switch (cont) {
            case Controller.Left:
                switch (input)
                {
                    case (Inputs.Grip):
                        float val = 0;
                        leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out val);
                        value = val;
                        return;
                }
                break;
            case Controller.Right:
                switch (input)
                {
                    case (Inputs.Grip):
                        float val = 0;
                        rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out val);
                        value = val;
                        return;
                }
                break;
        }

        value = 0.0f;
    }

    public enum Controller
    {
        Left,
        Right,
        None
    }

    public enum Inputs
    {
        Grip,
        Trigger
    }

    public void TryToGetDevices()
    {
        var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);

        if (leftHandDevices.Count == 1)
        {
            leftController = leftHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", leftController.name, leftController.role.ToString()));
        }
        else if (leftHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }

        var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, rightHandDevices);

        if (rightHandDevices.Count == 1)
        {
            rightController = rightHandDevices[0];
            Debug.Log(string.Format("Device name '{0}' with role '{1}'", rightController.name, rightController.role.ToString()));
        }
        else if (rightHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }

    }

}
