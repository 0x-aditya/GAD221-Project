using UnityEngine;
using UnityEngine.UIElements;

public class CameraPivotRotationTracking : MonoBehaviour
{
    [SerializeField] PlayerInputHandler inputTracker;
    [SerializeField] private float mouseSensitivity;

    private bool isDragging;
    private float pointerXLocation => inputTracker.MousePointerLocation.x;
    
    private void Start()
    {
        if (inputTracker == null)
        {
            Debug.LogError("Input Tracker is Null");
            return;
        }
        inputTracker.OnRightClickDown += () => {isDragging = true; lastPointerLocation=pointerXLocation;};
        inputTracker.OnRightClickUp += () => {isDragging = false;};
    }

    private float lastPointerLocation = 0;
    private void Update(){
        if (isDragging){
            float positionDelta = pointerXLocation - lastPointerLocation;
            transform.Rotate(0,positionDelta*mouseSensitivity,0);
            lastPointerLocation = pointerXLocation;
        }
    }
    // private void Update()
    // {
    //     if (isDragging)
    //     {
    //         float positionDelta = 0;
    //         if (pointerYLocation<0 && lastPointerLocation < 0){
    //             if (pointerYLocation<lastPointerLocation)
    //             {
    //                 positionDelta = pointerYLocation - lastPointerLocation;
    //             }
    //             else if (pointerYLocation>lastPointerLocation){
    //                 positionDelta = lastPointerLocation - pointerYLocation;
    //             }
    //         }
    //         else if (pointerYLocation>0 && lastPointerLocation>0){
    //             if (pointerYLocation<lastPointerLocation){
    //                 positionDelta = lastPointerLocation - pointerYLocation;
    //             }
    //             else if (pointerYLocation>lastPointerLocation){
    //                 positionDelta = pointerYLocation - lastPointerLocation;
    //             }
    //         }
    //         else
    //         {
    //             positionDelta = pointerYLocation - lastPointerLocation;
    //         }
    //         //transform.rotation = new Quaternion(0, pointerYLocation*mouseSensitivity, 0, 0);
    //         transform.Rotate(0,positionDelta*mouseSensitivity,0);
    //         lastPointerLocation = pointerYLocation;
    //     }
    // }
}
