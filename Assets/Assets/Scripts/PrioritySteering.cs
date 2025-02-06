using UnityEngine;

public class PrioritySteering
{
    public float epsilon = 0.1f;
    public BlendSteering[] groups;

    public SteeringOutput getSteering()
    {
        SteeringOutput steering = new SteeringOutput();
        foreach(BlendSteering group in groups)
        {
            steering = group.getSteering();
            if(steering.linear.magnitude > epsilon || Mathf.Abs(steering.angular) > epsilon)
            {
                return steering;
            }
        }
        return steering;
    }
}