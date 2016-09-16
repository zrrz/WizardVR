//----------------------------------------------------------------------------
// Created on Thu Oct 10 16:25:15 2013 Raphael Thoulouze
//
// This program is the property of Persistant Studios SARL.
//
// You may not redistribute it and/or modify it under any conditions
// without written permission from Persistant Studios SARL, unless
// otherwise stated in the latest Persistant Studios Code License.
//
// See the Persistant Studios Code License for further details.
//----------------------------------------------------------------------------

#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5
#define UNITY_5_2_UP
#endif
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//----------------------------------------------------------------------------

public class PKFxRenderingPlugin : PKFxCamera
{
#if !UNITY_5_2_UP
	private Camera					m_DistortionCamera = null;
#if !UNITY_5
	[Tooltip("Enables soft particles material")][HideInInspector]
	public bool						m_EnableSoftParticles = false;
	[Tooltip("Enables the distortion particles material, adding a postFX pass.")][HideInInspector]
	public bool						m_EnableDistortion = false;
	[Tooltip("Enables the distortion blur pass, adding another postFX pass.")][HideInInspector]
	public bool						m_EnableBlur = false;
	[Tooltip("Blur factor. Ajusts the blur's spread.")][HideInInspector]
	public float					m_BlurFactor = 0.2f;
	private RenderTexture			_FXRT = null;
	public RenderTexture			m_DepthRT = null;
	private	Camera					m_DepthCamera = null;
	private bool					m_IsDepthCopyEnabled = false;
	private RenderTexture			m_DistortionRT = null;
	private Material				m_MaterialDistortion;
	private Material				m_MaterialBlur;
#endif
#endif

	private PKFxDistortionEffect m_PostFx = null;

	// Exposed in "Advanced" Editor
	[Tooltip("Shows settings likely to impact performance.")][HideInInspector]
	public bool						m_ShowAdvancedSettings = false;
	[Tooltip("Loads a user-defined mesh to be used for particles world collisions.")][HideInInspector]
	public bool						m_UseSceneMesh = false;
	[Tooltip("Name of the scene mesh relative to the PackFx directory.")][HideInInspector]
	public string					m_SceneMeshPkmmPath = "Meshes/UnityScene.pkmm";

	public List<PkFxCustomShader>				m_BoundShaders;
	
	//----------------------------------------------------------------------------

	IEnumerator	Start()
	{
		base.BaseInitialize();
		yield return WaitForPack(true);

		if (Application.isEditor && QualitySettings.desiredColorSpace != ColorSpace.Linear)
			Debug.LogWarning("[PKFX] Current rendering not in linear space. " +
				"Colors may not be accurate.\n" +
				"To properly set the color space, go to \"Player Settings\">\"Other Settings\">\"Color Space\"");

		m_IsDepthCopyEnabled = (m_EnableSoftParticles || m_EnableDistortion);

		if (m_EnableDistortion && !SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning("[PKFX] Image effects not supported, distortions disabled.");
			m_EnableDistortion = false;
		}

#if UNITY_5_1
		// Native VR support started with 5.1 but 5.2 allows distortion stuff using the command buffers
		// Also, soft particles are available because 5.0's command buffers allow depth copy.
		if (Application.platform != RuntimePlatform.IPhonePlayer && UnityEngine.VR.VRSettings.enabled)
		{
			Debug.LogWarning("[PKFX] Native VR doesn't support distortions.");
			Debug.LogWarning("[PKFX] Distortion disabled.");
			m_EnableDistortion = false;
		}
#endif
		if (m_UseSceneMesh && m_SceneMeshPkmmPath.Length > 0)
		{
			if (PKFxManager.LoadPkmmAsSceneMesh(m_SceneMeshPkmmPath))
				Debug.Log ("[PKFX] Scene Mesh loaded");
			else
				Debug.LogError("[PKFX] Failed to load mesh " + m_SceneMeshPkmmPath + " as scene mesh");
		}

#if !UNITY_5
		if (m_EnableSoftParticles || m_EnableDistortion)
		{
			SetupDepthRenderTargets();
		}
#endif

#if UNITY_5
		if (m_EnableDistortion || m_EnableSoftParticles)
			SetupDepthGrab();
#	if UNITY_5_2_UP
		if (m_IsDepthCopyEnabled && m_EnableDistortion && m_Camera.actualRenderingPath == RenderingPath.Forward)
		{
			SetupDistortionPass();
#	else
		if (m_IsDepthCopyEnabled && m_EnableDistortion)
		{
			SetupDistortionPass();
			SetupDistortionRenderTarget();
#	endif
			if (m_EnableDistortion && m_Camera.actualRenderingPath == RenderingPath.Forward)
			{
				m_PostFx = gameObject.AddComponent<PKFxDistortionEffect>();
				m_PostFx.m_MaterialDistortion = m_DistortionMat;
				m_PostFx.m_MaterialBlur = m_DistBlurMat;
				m_PostFx.m_BlurFactor = m_BlurFactor;
				m_PostFx._DistortionRT = m_DistortionRT;
				m_PostFx.hideFlags = HideFlags.HideAndDontSave;
			}
		}
#else
		if (m_IsDepthCopyEnabled && m_EnableDistortion)
		{
			Shader shader = Shader.Find("Hidden/PKFx Distortion");
			if (!shader || !shader.isSupported)
			{
				Debug.LogError("[PKFX] Failed to load FxDistortionEffect shader...");
				this.m_EnableDistortion = false;
				yield break;
			}
			this.m_MaterialDistortion = new Material(shader);
			this.m_MaterialDistortion.hideFlags = HideFlags.HideAndDontSave;

			if (m_EnableBlur)
			{
				Shader shaderBlur = Shader.Find("Hidden/PKFx Blur Shader for Distortion Pass");
				if (!shaderBlur || !shaderBlur.isSupported)
				{
					Debug.LogWarning("[PKFX] Failed to load SeparableBlurPlus shader...");
					Debug.LogWarning("[PKFX] Distortion blur disabled.");
				}
				else
				{
					this.m_MaterialBlur = new Material(shaderBlur);
					this.m_MaterialBlur.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			SetupDistortionRenderTarget();
		}
#endif
		if (m_BoundShaders != null)
		{
			for (int iShader = 0; iShader < m_BoundShaders.Count; ++iShader)
			{
				if (m_BoundShaders[iShader] != null && !string.IsNullOrEmpty(m_BoundShaders[iShader].m_ShaderName))
				{
					if (!string.IsNullOrEmpty(m_BoundShaders[iShader].m_ShaderGroup))
					{
						m_BoundShaders[iShader].m_LoadedShaderId = PKFxManager.LoadShader(m_BoundShaders[iShader].GetDesc());
						m_BoundShaders[iShader].UpdateShaderConstants(true);
					}
					else
					{
						Debug.LogWarning("[PKFX] " + m_BoundShaders[iShader].m_ShaderName + " has no ShaderGroup, it will not be loaded");
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------
	// Pre 5.2 rendering setup
	//----------------------------------------------------------------------------

#if !UNITY_5_2_UP
	void SetupDistortionRenderTarget()
	{
#	if !UNITY_5 // Already created in PKFxCamera
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
		{
			m_DistortionRT = new RenderTexture((int)m_Camera.pixelWidth,
													(int)m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.sRGB);
		}
		else
			m_DistortionRT = new RenderTexture((int)m_Camera.pixelWidth,
													(int)m_Camera.pixelHeight, 0, RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.sRGB);
#	endif
		GameObject g = new GameObject("PKFX Offscreen Distortion Cam" + " for " + m_Camera.GetInstanceID());
		g.transform.parent = this.transform;
		g.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;

		m_DistortionCamera = g.AddComponent<Camera>();
		m_DistortionCamera.CopyFrom(m_Camera);
		m_DistortionCamera.backgroundColor = Color.black;
		m_DistortionCamera.renderingPath = RenderingPath.Forward;
		m_DistortionCamera.clearFlags = CameraClearFlags.Color;
		m_DistortionCamera.targetTexture = m_DistortionRT;
		m_DistortionCamera.cullingMask = 0;
		m_DistortionCamera.depth = m_Camera.depth - 1;
		m_DistortionCamera.rect = new Rect(0,0,1,1);
		m_DistortionCamera.enabled = true;
		
		PKFxCamera distCam = g.AddComponent<PKFxCamera>();
		distCam.RenderPass = 2;
		distCam.m_HasPostFx = true;
		distCam.DepthRT = m_DepthRT.GetNativeTexturePtr();

#	if !UNITY_5
		m_PostFx = gameObject.AddComponent<PKFxDistortionEffect>();
		m_PostFx.m_MaterialDistortion = m_MaterialDistortion;
		m_PostFx.m_MaterialBlur = m_MaterialBlur;
		m_PostFx.m_BlurFactor = m_BlurFactor;
		m_PostFx._DistortionRT = m_DistortionRT;
		m_PostFx.hideFlags = HideFlags.HideAndDontSave;
#	endif
	}

	//----------------------------------------------------------------------------

#	if !UNITY_5
	void SetupDepthRenderTargets()
	{
		// check for depth texture availability
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			Debug.LogWarning("[PKFX] This platform doesn't support depth textures.");
			Debug.LogWarning("[PKFX] Soft particles/distortion disabled.");
			m_IsDepthCopyEnabled = false;
			m_EnableDistortion = false;
			m_EnableSoftParticles = false;
			return;
		}

		m_DepthRT = new RenderTexture((int)this.m_Camera.pixelWidth,
											(int)this.m_Camera.pixelHeight, 16, RenderTextureFormat.Depth);
		if (!m_DepthRT.IsCreated())
			m_DepthRT.Create();
		this._FXRT = new RenderTexture((int)this.m_Camera.pixelWidth,
										(int)this.m_Camera.pixelHeight, 0, RenderTextureFormat.Default);
		if (!this._FXRT.IsCreated())
			this._FXRT.Create();

		GameObject g = new GameObject("OffscreenCamDepth id" + this.m_Camera.GetInstanceID() + " for " + this.m_Camera.GetInstanceID());
		g.transform.parent = this.transform;
		g.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
		
		m_DepthCamera = g.AddComponent<Camera>();
		m_DepthCamera.CopyFrom(this.m_Camera);
		m_DepthCamera.renderingPath = RenderingPath.Forward;
		m_DepthCamera.depth = this.m_Camera.depth - 2;
		m_DepthCamera.rect = new Rect(0,0,1,1);
		m_DepthCamera.enabled = true;

		m_DepthCamera.SetTargetBuffers(_FXRT.colorBuffer, m_DepthRT.depthBuffer);
		this.DepthRT = m_DepthRT.GetNativeTexturePtr();
	}
#	endif
#endif

	//----------------------------------------------------------------------------

	void Update()
	{
#if !UNITY_5_2_UP
		// update FOV and check for lost device textures.
#	if !UNITY_5
		if (m_DepthCamera != null)
		{
			m_DepthCamera.fieldOfView = m_Camera.fieldOfView;
			m_DepthCamera.aspect = m_Camera.aspect;
			if (m_DepthRT != null && !m_DepthRT.IsCreated())
			{
				this._FXRT = new RenderTexture((int)this.m_Camera.pixelWidth,
												(int)this.m_Camera.pixelHeight,
												0,
												RenderTextureFormat.Default);
				m_DepthRT = new RenderTexture((int)this.m_Camera.pixelWidth,
												(int)this.m_Camera.pixelHeight,
												16,
												RenderTextureFormat.Depth);
				m_DepthCamera.SetTargetBuffers(_FXRT.colorBuffer, m_DepthRT.depthBuffer);
				this.DepthRT = m_DepthRT.GetNativeTexturePtr();
				if (m_DistortionCamera != null)
					m_DistortionCamera.GetComponent<PKFxCamera>().DepthRT = m_DepthRT.GetNativeTexturePtr();
			}
		}
#	endif
		if (m_DistortionCamera != null)
		{
			m_DistortionCamera.fieldOfView = m_Camera.fieldOfView;
			m_DistortionCamera.aspect = m_Camera.aspect;
			if (m_DistortionRT != null && !m_DistortionRT.IsCreated())
			{
				if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
				{
					m_DistortionRT = new RenderTexture((int)m_Camera.pixelWidth,
															(int)m_Camera.pixelHeight,
															0,
															RenderTextureFormat.ARGBFloat,
															RenderTextureReadWrite.sRGB);
				}
				else
				{
					m_DistortionRT = new RenderTexture((int)m_Camera.pixelWidth,
															(int)m_Camera.pixelHeight,
															0,
															RenderTextureFormat.ARGBHalf,
															RenderTextureReadWrite.sRGB);
				}
				m_DistortionCamera.targetTexture = m_DistortionRT;
				m_PostFx._DistortionRT = m_DistortionRT;
			}
		}
#endif
		m_CurrentFrameID++;
	}

	//----------------------------------------------------------------------------

	void LateUpdate()
	{
		if (m_BoundShaders != null)
		{
			for (int iShader = 0; iShader < m_BoundShaders.Count; ++iShader)
			{
				if (m_BoundShaders[iShader] != null && !string.IsNullOrEmpty(m_BoundShaders[iShader].m_ShaderName))
				{
					if (!string.IsNullOrEmpty(m_BoundShaders[iShader].m_ShaderGroup))
					{
						m_BoundShaders[iShader].UpdateShaderConstants(false);
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------

#if !UNITY_5_2_UP
	void OnDestroy()
	{
		if (m_DistortionCamera != null)
			GameObject.DestroyImmediate(m_DistortionCamera);
#if !UNITY_5
		if (m_DepthCamera != null)
			GameObject.DestroyImmediate(m_DepthCamera);
#endif
		base.OnDestroy();
	}
#endif
}
