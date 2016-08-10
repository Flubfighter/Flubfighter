using UnityEngine;
using System.Collections;

public static class GizmosHelper
{
	public static void DrawArrowHead(Vector3 position, Vector3 direction, float angle, float arrowWidth)
	{
		Gizmos.DrawRay(position, Quaternion.AngleAxis(angle, Vector3.forward) * -direction.normalized * arrowWidth);
		Gizmos.DrawRay(position, Quaternion.AngleAxis(-angle, Vector3.forward) * -direction.normalized * arrowWidth);
	}

	public static void DrawArrowLine(Vector3 start, Vector3 end, float angle=45f, float arrowWidth=0.5f)
	{
		Gizmos.DrawLine(start, end);
		DrawArrowHead(end, end-start, angle, arrowWidth);
	}

	public static void DrawArrowRay(Vector3 start, Vector3 direction, float angle=45f, float arrowWidth=0.5f)
	{
		Gizmos.DrawRay(start, direction);
		DrawArrowHead(start + direction, direction, angle, arrowWidth);
	}
}

