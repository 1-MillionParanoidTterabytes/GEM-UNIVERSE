using UnityEngine;

public static class NaturalMovement
{
	public static Vector3 MatchVelocityForce(Vector3 targetVelocity, Vector3 velocity, float acceleration, float maxAcceleration)
	{
		Vector3 dv = targetVelocity - velocity;
		return dv.normalized * Mathf.Min(dv.magnitude * acceleration, maxAcceleration);
	}
	public static Vector3 ApproachPositionForce(Vector3 targetPosition, Vector3 position, Vector3 velocity, float acceleration, float maxAcceleration, float maxVelocity, float timeToReach)
	{
		Vector3 targetVelocity = targetPosition - position;
		targetVelocity = targetVelocity.normalized * Mathf.Min(targetVelocity.magnitude / timeToReach, maxVelocity);
		return MatchVelocityForce(targetVelocity, velocity, acceleration, maxAcceleration);
	}
	public static Vector3 CapVelocity(Vector3 velocity, float maxVelocity)
	{
		return velocity.normalized * Mathf.Min(velocity.magnitude, maxVelocity);
	}
}