using UnityEngine;

public class PLayerStateContext : MonoBehaviour
{
    public IPlayerState currentstate { get; set; }

    private readonly PlayerController _Playercontroller;

    public PLayerStateContext(PlayerController pLayerController)
    {
        _Playercontroller = pLayerController;
    }
    public void Transition()
    {
        currentstate.Handle(_Playercontroller);
    }
    public void Transition(IPlayerState state)
    {
        currentstate = state;
        currentstate.Handle(_Playercontroller);
    }
}

