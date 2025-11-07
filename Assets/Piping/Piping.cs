using UnityEngine;
using UnityEngine.UI;

public abstract class Piping : MonoBehaviour
{
    public ShapeType selectedShape; // Use the shared ShapeType enum
    public ShapeAndTypeHandler shapeAndTypeHandler; // Reference to ShapeAndTypeHandler

    public GameObject mainCakeObject;
    public Slider pipingPositionSlider;
    public Slider radiusSlider;
    public Slider depthSlider; // Only used if the shape requires it
    private ShapeType selectedShapeLocalDrop;



    protected float pipingPosition;
    protected float radius;
    protected float depth; // Only relevant for some shapes

    protected virtual void Start()
    {
        // Initialize values from sliders
        pipingPosition = pipingPositionSlider.value;
        radius = radiusSlider.value;

        // Add listeners to sliders
        pipingPositionSlider.onValueChanged.AddListener(OnPipingPositionChanged);
        radiusSlider.onValueChanged.AddListener(OnRadiusChanged);


    }
    protected virtual void Update()
    {
        // Update selectedShapeLocalDrop from ShapeAndTypeHandler

        selectedShapeLocalDrop = shapeAndTypeHandler.selectedShape;

        // Show or hide depth slider based on selected shape
        if (selectedShapeLocalDrop == ShapeType.Circle)
        {
            depthSlider.gameObject.SetActive(false); // Hide the depth slider
            depthSlider.onValueChanged.RemoveListener(OnDepthChanged); // Remove listener
        }
        else if (selectedShapeLocalDrop == ShapeType.Star)
        {
            depthSlider.gameObject.SetActive(true); // Show the depth slider
            depthSlider.onValueChanged.AddListener(OnDepthChanged); // Add listener
            depth = depthSlider.value; // Initialize depth
        }
    }
    protected virtual void OnPipingPositionChanged(float value)
    {
        pipingPosition = value;
        Debug.Log(pipingPosition + "position changed");

        Generate();
    }

    protected virtual void OnRadiusChanged(float value)
    {
        radius = value;
        Generate();
    }

    protected virtual void OnDepthChanged(float value)
    {
        if (selectedShapeLocalDrop == ShapeType.Star)
        {
            Debug.Log("depth changed");
            depth = value;
            Generate();
        }
    }

    public abstract void Generate();
}