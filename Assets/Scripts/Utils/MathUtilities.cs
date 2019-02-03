using UnityEngine;

public static class MathUtilities
{
	public static int Modulo(int a, int n)
	{
		return (a % n + n) % n;
	}

	public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
	{
		var mid = Vector3.Lerp(start, end, t);

		return new Vector3(mid.x, GetParabolaHeight(height, t) + Mathf.Lerp(start.y, end.y, t), mid.z);
	}

	private static float GetParabolaHeight(float peak, float t)
	{
		return -4 * peak * t * t + 4 * peak * t;
	}
}
