using UnityEngine;

public class GameState : MonoBehaviour
{
    public double[] GetObservation()
    {
        //get the enemy direction in front, get nearest cover(any GameObject present in front of us that can be used as cover) direction in front, get the distance from enemy and cover
        return null;
    }

    public void TakeAction(int action)
    {
        //translate action into actual action
        switch (action)
        {
            case 0:
                //call action 1 function
                break;
            case 1:
                //call action 2 function
                break;
        }
    }

    public float reward(int state)
    {
        float reward = 0f;
        return reward;
    }
}