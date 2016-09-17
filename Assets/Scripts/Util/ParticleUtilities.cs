using UnityEngine;
using System.Collections;

public class ParticleUtilities : MonoBehaviour {

	class ParticleSnapshot {

	}

  public static void ScaleParticleSystem(GameObject go, float scalingValue, bool scaleParticleSize = true)
  {
      //Scale Shuriken Particles Values
      ParticleSystem[] systems;
      
      systems = go.GetComponentsInChildren<ParticleSystem>(true);

      foreach (ParticleSystem ps in systems)
      {
		ScaleParticleValues(ps, go, scalingValue, scaleParticleSize);
      }

      //Scale Lights' range
      Light[] lights = go.GetComponentsInChildren<Light>();
      foreach (Light light in lights)
      {
        light.range *= scalingValue;
        if (light.gameObject != go)
        {
          light.transform.localPosition *= scalingValue;
        }
      }

	  //Scale wind zone
	  WindZone[] windZones = go.GetComponentsInChildren<WindZone>();
	  foreach (WindZone windZone in windZones)
	  {
		windZone.radius *= scalingValue;
		if (windZone.gameObject != go)
		{
		  windZone.transform.localPosition *= scalingValue;
		}
	  }
  }

	static void ScaleParticleValues(ParticleSystem ps, GameObject parent, float scalingValue, bool scaleParticleSize = true)
	{
		ScaleSizeSpeed (ps, parent, scalingValue, scaleParticleSize);

		ScaleEmission (ps, scalingValue);

		ScaleSizeBySpeed (ps, scalingValue);

		ScaleVelocity (ps, scalingValue);

		ScaleVelocityOverLife (ps, scalingValue);

		ScaleForceOverLife (ps, scalingValue);

//		ScaleShape (ps, scalingValue);
	}

	static void ScaleSizeSpeed (ParticleSystem ps, GameObject parent, float scalingValue, bool scaleParticleSize)
	{
		//Particle System
		if (scaleParticleSize)
			ps.startSize *= scalingValue;
		ps.gravityModifier *= scalingValue;
		if (ps.startSpeed > 0.01f)
			ps.startSpeed *= scalingValue;
		if (ps.gameObject != parent) {
			ps.transform.localPosition *= scalingValue;
		}
	}

	static void ScaleEmission (ParticleSystem ps, float scalingValue)
	{
		var emission = ps.emission;
		if (emission.enabled) {
			var rate = emission.rate;
//			rate.constant *= scalingValue;
			rate.curveScalar /= scalingValue;
			emission.rate = rate;
		}
	}

	static void ScaleSizeBySpeed (ParticleSystem ps, float scalingValue)
	{
		var sizeBySpeed = ps.sizeBySpeed;
		if (sizeBySpeed.enabled) {
			Vector2 range = sizeBySpeed.range;
			range /= scalingValue;
			sizeBySpeed.range = range;
		}
	}

	static void ScaleVelocity (ParticleSystem ps, float scalingValue)
	{
		var inheritVelocity = ps.inheritVelocity;
		if (inheritVelocity.enabled) {
			var curve = inheritVelocity.curve;
			curve.curveScalar *= scalingValue;
			if (curve.curveMin != null)
				curve.curveMin.keys = IterateKeys (curve.curveMin.keys, scalingValue);
			if (curve.curveMax != null)
				curve.curveMax.keys = IterateKeys (curve.curveMax.keys, scalingValue);
			inheritVelocity.curve = curve;
		}
	}

	static void ScaleVelocityOverLife (ParticleSystem ps, float scalingValue)
	{
		var velocityOverLifetime = ps.velocityOverLifetime;
		if (velocityOverLifetime.enabled) {
			var curveX = velocityOverLifetime.x;
			if (curveX.curveMin != null)
				curveX.curveMin.keys = IterateKeys (curveX.curveMin.keys, scalingValue);
			if (curveX.curveMax != null)
				curveX.curveMax.keys = IterateKeys (curveX.curveMin.keys, scalingValue);
			curveX.curveScalar *= scalingValue;
			velocityOverLifetime.x = curveX;
			var curveY = velocityOverLifetime.y;
			if (curveY.curveMin != null)
				curveY.curveMin.keys = IterateKeys (curveY.curveMin.keys, scalingValue);
			if (curveY.curveMax != null)
				curveY.curveMax.keys = IterateKeys (curveY.curveMin.keys, scalingValue);
			curveY.curveScalar *= scalingValue;
			velocityOverLifetime.y = curveY;
			var curveZ = velocityOverLifetime.z;
			if (curveZ.curveMin != null)
				curveZ.curveMin.keys = IterateKeys (curveZ.curveMin.keys, scalingValue);
			if (curveZ.curveMax != null)
				curveZ.curveMax.keys = IterateKeys (curveZ.curveMin.keys, scalingValue);
			curveZ.curveScalar *= scalingValue;
			velocityOverLifetime.z = curveZ;
		}
	}

	static void ScaleForceOverLife (ParticleSystem ps, float scalingValue)
	{
		var forceOverLifetime = ps.forceOverLifetime;
		if (forceOverLifetime.enabled) {
			var curveX = forceOverLifetime.x;
			if (curveX.curveMin != null)
				curveX.curveMin.keys = IterateKeys (curveX.curveMin.keys, scalingValue);
			if (curveX.curveMax != null)
				curveX.curveMax.keys = IterateKeys (curveX.curveMax.keys, scalingValue);
			curveX.curveScalar *= scalingValue;
			forceOverLifetime.x = curveX;
			var curveY = forceOverLifetime.y;
			if (curveY.curveMin != null)
				curveY.curveMin.keys = IterateKeys (curveY.curveMin.keys, scalingValue);
			if (curveY.curveMax != null)
				curveY.curveMax.keys = IterateKeys (curveY.curveMax.keys, scalingValue);
			curveY.curveScalar *= scalingValue;
			forceOverLifetime.y = curveY;
			var curveZ = forceOverLifetime.z;
			if (curveZ.curveMin != null)
				curveZ.curveMin.keys = IterateKeys (curveZ.curveMin.keys, scalingValue);
			if (curveZ.curveMax != null)
				curveZ.curveMax.keys = IterateKeys (curveZ.curveMax.keys, scalingValue);
			curveZ.curveScalar *= scalingValue;
			forceOverLifetime.z = curveZ;
		}
	}

	static void ScaleShape (ParticleSystem ps, float scalingValue)
	{
		var shape = ps.shape;
		if (shape.enabled) {
			shape.box *= scalingValue;
			shape.radius *= scalingValue;
			if (shape.shapeType == ParticleSystemShapeType.Mesh) {
				Mesh mesh = shape.mesh;
				if (mesh != null) {
					DuplicateAndScaleMesh (mesh, scalingValue);
					shape.mesh = mesh;
				}
			}
		}
	}

  public static Keyframe[] IterateKeys(Keyframe[] keys, float scalingValue)
  {
    for (int i = 0; i < keys.Length; i++)
    {
      keys[i].value *= scalingValue;
    }
    return keys;
  }

  //Create Scaled Mesh
  static Mesh DuplicateAndScaleMesh(Mesh mesh, float Scale)
  {
    //Mesh scaledMesh = new Mesh();

    Vector3[] scaledVertices = new Vector3[mesh.vertices.Length];
    for (int i = 0; i < scaledVertices.Length; i++)
    {
      scaledVertices[i] = mesh.vertices[i] * Scale;
    }
    mesh.vertices = scaledVertices;

    //scaledMesh.normals = mesh.normals;
    //scaledMesh.tangents = mesh.tangents;
    //scaledMesh.triangles = mesh.triangles;
    //scaledMesh.uv = mesh.uv;
    //scaledMesh.uv2 = mesh.uv2;
    //scaledMesh.colors = mesh.colors;

    return mesh;
  }
}
