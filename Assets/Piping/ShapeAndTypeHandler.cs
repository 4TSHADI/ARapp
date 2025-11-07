using UnityEngine;
using UnityEngine.UI;

public class ShapeAndTypeHandler : MonoBehaviour
{

    public ShapeType shapeType; // Use the shared ShapeType enum

    public enum PipingType {None, Straight, Twisted, Swirl, Drop }

    public PipingType selectedPipingType = PipingType.None; // Default to None
    //public ShapeType selectedShapeLocalDrop = ShapeType.None; // Default to None
    public ShapeType selectedShape;
    public void OnStarButtonClicked()
    {
        selectedShape = ShapeType.Star;
        Debug.Log("Selected Shape: Star");
        Debug.Log(selectedShape);

    }

    public void OnFrenchStarButtonClicked()
    {
        selectedShape = ShapeType.FrenchStar;
        Debug.Log("Selected Shape: French Star");
        Debug.Log(selectedShape);

    }
    public void OnPetalButtonClicked()
    {
        selectedShape = ShapeType.Petal;
        Debug.Log("Selected Shape: Petal");
        Debug.Log(selectedShape);

    }
    public void OnCircleButtonClicked()
    {
        selectedShape = ShapeType.Circle;
        Debug.Log("Selected Shape: Circle");
        Debug.Log(selectedShape);

    }

    public void OnStraightButtonClicked()
    {
        selectedPipingType = PipingType.Straight;
        Debug.Log("Selected Piping Type: Straight");
    }
    public void OnTwistedButtonClicked()
    {
        selectedPipingType = PipingType.Twisted;
        Debug.Log("Selected Piping Type: Twisted");
    }

    public void OnSwirlButtonClicked()
    {
        selectedPipingType = PipingType.Swirl;
        Debug.Log("Selected Piping Type: Swirl");
    }

    public void OnDropButtonClicked()
    {
        selectedPipingType = PipingType.Drop;
        Debug.Log("Selected Piping Type: Drop");
    }

}