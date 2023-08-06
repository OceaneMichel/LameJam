using Pathfinding.Poly2Tri;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTerrain2d : MeshBool2d
{
	protected override void GenerateMesh(List<DelaunayTriangle> triangles)
	{
		base.GenerateMesh(triangles);
		GenerateColliders();
	}

	private void GenerateColliders()
    {
		var old = GetComponents<EdgeCollider2D>();
		for (int i = 0; i < old.Length; i++)
			Destroy(old[i]);

		for(int i = 0; i < m_polys.Count; i++)
        {
			var poly = m_polys[i];
			
			var points = new Vector2[poly.Count + 1];
			for (int j = 0; j < poly.Count; j++)
				points[j] = Convert(poly[j]);
			points[poly.Count] = points[0];

			var collider = gameObject.AddComponent<EdgeCollider2D>();
			collider.points = points;
		}
    }
}