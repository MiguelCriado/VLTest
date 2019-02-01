public static class MathUtilities
{
	public static int Modulo(int a, int n)
	{
		return (a % n + n) % n;
	}
}
