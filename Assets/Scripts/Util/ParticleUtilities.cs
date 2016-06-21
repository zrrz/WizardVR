using UnityEngine;
using System.Collections;

public class ParticleUtilities : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  public static void ScaleParticleSystem(GameObject go, float scalingValue)
  {
      //Scale Shuriken Particles Values
      ParticleSystem[] systems;
      
      systems = go.GetComponentsInChildren<ParticleSystem>(true);

      foreach (ParticleSystem ps in systems)
      {
        ScaleParticleValues(ps, go, scalingValue);
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
  }

  static void ScaleParticleValues(ParticleSystem ps, GameObject parent, float scalingValue)
  {
    //Particle System
    ps.startSize *= scalingValue;
    ps.gravityModifier *= scalingValue;
    if (ps.startSpeed > 0.01f)
      ps.startSpeed *= scalingValue;
    if (ps.gameObject != parent)
    {
      ps.transform.localPosition *= scalingValue;
    }

    //SerializedObject psSerial = new SerializedObject(ps);


    var emission = ps.emission;
    if (emission.enabled)
    {
      var rate = emission.rate;
      rate.curveScalar /= scalingValue;
      emission.rate = rate;
    }
    ////Scale Emission Rate if set on Distance
    //if (psSerial.FindProperty("EmissionModule.enabled").boolValue && psSerial.FindProperty("EmissionModule.m_Type").intValue == 1)
    //{
    //  psSerial.FindProperty("EmissionModule.rate.scalar").floatValue /= ScalingValue;
    //}

    var sizeBySpeed = ps.sizeBySpeed;
    if (sizeBySpeed.enabled)
    {
      Vector2 range = sizeBySpeed.range;
      range /= scalingValue;
      sizeBySpeed.range = range;
    }
    ////Scale Size By Speed Module
    //if (psSerial.FindProperty("SizeBySpeedModule.enabled").boolValue)
    //{
    //  psSerial.FindProperty("SizeBySpeedModule.range.x").floatValue *= ScalingValue;
    //  psSerial.FindProperty("SizeBySpeedModule.range.y").floatValue *= ScalingValue;
    //}

    var inheritVelocity = ps.inheritVelocity;
    if (inheritVelocity.enabled)
    {
      var curve = inheritVelocity.curve;
      curve.curveScalar *= scalingValue;
      curve.curveMin.keys = IterateKeys(curve.curveMin.keys, scalingValue);
      curve.curveMax.keys = IterateKeys(curve.curveMax.keys, scalingValue);

      inheritVelocity.curve = curve;
    }
    ////Scale Velocity Module
    //if (psSerial.FindProperty("VelocityModule.enabled").boolValue)
    //{
    //  psSerial.FindProperty("VelocityModule.x.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("VelocityModule.x.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("VelocityModule.x.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("VelocityModule.y.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("VelocityModule.y.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("VelocityModule.y.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("VelocityModule.z.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("VelocityModule.z.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("VelocityModule.z.maxCurve").animationCurveValue);
    //}

    var velocityOverLifetime = ps.velocityOverLifetime;
    if (velocityOverLifetime.enabled)
    {
      var curveX = velocityOverLifetime.x;
      if(curveX.curveMin != null)
        curveX.curveMin.keys = IterateKeys(curveX.curveMin.keys, scalingValue);
      if (curveX.curveMax != null)
        curveX.curveMax.keys = IterateKeys(curveX.curveMin.keys, scalingValue);
      curveX.curveScalar *= scalingValue;
      velocityOverLifetime.x = curveX;

      var curveY = velocityOverLifetime.y;
      if (curveY.curveMin != null)
        curveY.curveMin.keys = IterateKeys(curveY.curveMin.keys, scalingValue);
      if (curveY.curveMax != null)
        curveY.curveMax.keys = IterateKeys(curveY.curveMin.keys, scalingValue);
      curveY.curveScalar *= scalingValue;
      velocityOverLifetime.y = curveY;

      var curveZ = velocityOverLifetime.z;
      if (curveZ.curveMin != null)
        curveZ.curveMin.keys = IterateKeys(curveZ.curveMin.keys, scalingValue);
      if (curveZ.curveMax != null)
        curveZ.curveMax.keys = IterateKeys(curveZ.curveMin.keys, scalingValue);
      curveZ.curveScalar *= scalingValue;
      velocityOverLifetime.z = curveZ;
    }
    //  //Scale Limit Velocity Module
    //  if (psSerial.FindProperty("ClampVelocityModule.enabled").boolValue)
    //{
    //  psSerial.FindProperty("ClampVelocityModule.x.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.x.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.x.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("ClampVelocityModule.y.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.y.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.y.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("ClampVelocityModule.z.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.z.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.z.maxCurve").animationCurveValue);

    //  psSerial.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.magnitude.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ClampVelocityModule.magnitude.maxCurve").animationCurveValue);
    //}

    var forceOverLifetime = ps.forceOverLifetime;
    if (forceOverLifetime.enabled)
    {
      var curveX = forceOverLifetime.x;
      curveX.curveMin.keys = IterateKeys(curveX.curveMin.keys, scalingValue);
      curveX.curveMax.keys = IterateKeys(curveX.curveMax.keys, scalingValue);
      curveX.curveScalar *= scalingValue;
      forceOverLifetime.x = curveX;

      var curveY = forceOverLifetime.y;
      curveY.curveMin.keys = IterateKeys(curveY.curveMin.keys, scalingValue);
      curveY.curveMax.keys = IterateKeys(curveY.curveMax.keys, scalingValue);
      curveY.curveScalar *= scalingValue;
      forceOverLifetime.y = curveY;

      var curveZ = forceOverLifetime.z;
      curveZ.curveMin.keys = IterateKeys(curveZ.curveMin.keys, scalingValue);
      curveZ.curveMax.keys = IterateKeys(curveZ.curveMax.keys, scalingValue);
      curveZ.curveScalar *= scalingValue;
      forceOverLifetime.z = curveZ;
    }

    ////Scale Force Module
    //if (psSerial.FindProperty("ForceModule.enabled").boolValue)
    //{
    //  psSerial.FindProperty("ForceModule.x.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ForceModule.x.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ForceModule.x.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("ForceModule.y.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ForceModule.y.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ForceModule.y.maxCurve").animationCurveValue);
    //  psSerial.FindProperty("ForceModule.z.scalar").floatValue *= ScalingValue;
    //  IterateKeys(psSerial.FindProperty("ForceModule.z.minCurve").animationCurveValue);
    //  IterateKeys(psSerial.FindProperty("ForceModule.z.maxCurve").animationCurveValue);
    //}

    var shape = ps.shape;
    if (shape.enabled)
    {
      shape.box *= scalingValue;
      shape.radius *= scalingValue;

      if(shape.shapeType == ParticleSystemShapeType.Mesh)
      {
        Mesh mesh = shape.mesh;
        if(mesh != null)
        {
          DuplicateAndScaleMesh(mesh, scalingValue);
          shape.mesh = mesh;
        }
      }
    }

      //  //Scale Shape Module
      //  if (psSerial.FindProperty("ShapeModule.enabled").boolValue)
      //  {
      //    psSerial.FindProperty("ShapeModule.boxX").floatValue *= ScalingValue;
      //    psSerial.FindProperty("ShapeModule.boxY").floatValue *= ScalingValue;
      //    psSerial.FindProperty("ShapeModule.boxZ").floatValue *= ScalingValue;
      //    psSerial.FindProperty("ShapeModule.radius").floatValue *= ScalingValue;

      //    //Create a new scaled Mesh if there is a Mesh reference
      //    //(ShapeModule.type 6 == Mesh)
      //    if (psSerial.FindProperty("ShapeModule.type").intValue == 6)
      //    {
      //      Object obj = psSerial.FindProperty("ShapeModule.m_Mesh").objectReferenceValue;
      //      if (obj != null)
      //      {
      //        Mesh mesh = (Mesh)obj;
      //        string assetPath = AssetDatabase.GetAssetPath(mesh);
      //        string name = assetPath.Substring(assetPath.LastIndexOf("/") + 1);

      //        //Mesh to use
      //        Mesh meshToUse = null;
      //        bool createScaledMesh = true;
      //        float meshScale = ScalingValue;

      //        //Mesh has already been scaled: extract scaling value and re-scale base effect
      //        if (name.Contains("(scaled)"))
      //        {
      //          string scaleStr = name.Substring(name.LastIndexOf("x") + 1);
      //          scaleStr = scaleStr.Remove(scaleStr.IndexOf(" (scaled).asset"));

      //          float oldScale = float.Parse(scaleStr);
      //          if (oldScale != 0)
      //          {
      //            meshScale = oldScale * ScalingValue;

      //            //Check if there's already a mesh with the correct scale
      //            string unscaledName = assetPath.Substring(0, assetPath.LastIndexOf(" x"));
      //            assetPath = unscaledName;
      //            string newPath = assetPath + " x" + meshScale + " (scaled).asset";
      //            Mesh alreadyScaledMesh = (Mesh)AssetDatabase.LoadAssetAtPath(newPath, typeof(Mesh));
      //            if (alreadyScaledMesh != null)
      //            {
      //              meshToUse = alreadyScaledMesh;
      //              createScaledMesh = false;
      //            }
      //            else
      //            //Load original unscaled mesh
      //            {
      //              Mesh orgMesh = (Mesh)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Mesh));
      //              if (orgMesh != null)
      //              {
      //                mesh = orgMesh;
      //              }
      //            }
      //          }
      //        }
      //        else
      //        //Verify if original mesh has already been scaled to that value
      //        {
      //          string newPath = assetPath + " x" + meshScale + " (scaled).asset";
      //          Mesh alreadyScaledMesh = (Mesh)AssetDatabase.LoadAssetAtPath(newPath, typeof(Mesh));
      //          if (alreadyScaledMesh != null)
      //          {
      //            meshToUse = alreadyScaledMesh;
      //            createScaledMesh = false;
      //          }
      //        }

      //        //Duplicate and scale mesh vertices if necessary
      //        if (createScaledMesh)
      //        {
      //          string newMeshPath = assetPath + " x" + meshScale + " (scaled).asset";
      //          meshToUse = (Mesh)AssetDatabase.LoadAssetAtPath(newMeshPath, typeof(Mesh));
      //          if (meshToUse == null)
      //          {
      //            meshToUse = DuplicateAndScaleMesh(mesh, meshScale);
      //            AssetDatabase.CreateAsset(meshToUse, newMeshPath);
      //          }
      //        }

      //        //Apply new Mesh
      //        psSerial.FindProperty("ShapeModule.m_Mesh").objectReferenceValue = meshToUse;
      //      }
      //    }
      //  }

      //  //Apply Modified Properties
      //  psSerial.ApplyModifiedProperties();
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
