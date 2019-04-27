using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Fade : MonoBehaviour {

	public Material[] materials;
	public float waitTime = 0;
	public float fadeTime = 4;
	public bool replaceShaders = true;
	
	static Dictionary<Shader, Shader> replacementShaders = new Dictionary<Shader, Shader>();

	static string[] standardShaderNames = new string[] { "Standard", "Standard (Specular setup)" };

	static bool IsStandardShader(Shader shader) {
		return System.Array.IndexOf(standardShaderNames, shader.name) != -1;
	}
	
	static string[] lwrpStandardShaderNames = new string[] {
		"LightweightPipeline/Standard (Physically Based)",
		"LightweightPipeline/Standard (Simple Lighting)",
		"LightweightPipeline/Standard Unlit",
		"LightweightPipeline/Particles/Standard (Physically Based)",
		"LightweightPipeline/Particles/Standard Unlit",
		// These are the new names from at least version 4.1.0-preview of the LWRP package (as included with Unity 2018.3):
		"Lightweight Render Pipeline/Lit",
		"Lightweight Render Pipeline/Simple Lit",
		"Lightweight Render Pipeline/Unlit",
		"Lightweight Render Pipeline/Particles/Lit",
		"Lightweight Render Pipeline/Particles/Simple Lit",
		"Lightweight Render Pipeline/Particles/Unlit",
	};
	
	static bool IsLWRPStandardShader(Shader shader) {
		return System.Array.IndexOf(lwrpStandardShaderNames, shader.name) != -1;
	}

	static string[] hdrpStandardShaderNames = new string[] {
		"HDRenderPipeline/LayeredLit",
		"HDRenderPipeline/LayeredLitTessellation",
		"HDRenderPipeline/Lit",
		"HDRenderPipeline/LitTessellation",
		"HDRenderPipeline/Unlit",
	};
	
	static bool IsHDRPStandardShader(Shader shader) {
		return System.Array.IndexOf(hdrpStandardShaderNames, shader.name) != -1;
	}
	
	public static Shader GetReplacementFor(Shader original) {
		Shader replacement;
		if (replacementShaders.TryGetValue(original, out replacement)) return replacement;

		const string legacyShadersPrefix = "Legacy Shaders/";
		const string transparentPrefix = "Transparent/";
		const string mobilePrefix = "Mobile/";

		if (IsStandardShader(original) || IsLWRPStandardShader(original) || IsHDRPStandardShader(original)) {
			replacement = original;
		} else {
			var name = original.name;

			var originalIsLegacy = name.StartsWith(legacyShadersPrefix);
			if (originalIsLegacy) {
				name = name.Substring(legacyShadersPrefix.Length);
			}

			if (name.StartsWith(mobilePrefix)) {
				name = name.Substring(mobilePrefix.Length);
			}
			
			if (name.StartsWith(transparentPrefix)) {
				replacement = original;
			} else {
				name = transparentPrefix + name;
				if (originalIsLegacy) name = legacyShadersPrefix + name;
				replacement = Shader.Find(name);
			}
		}

		replacementShaders[original] = replacement;
		return replacement;
	}
	
	static string[] colorPropertyNameCandidates = new string[] { "_Color", "_TintColor", "_BaseColor", "_BaseColor0", "_BaseColor1", "_BaseColor2", "_BaseColor3", "_UnlitColor" };
	
	IEnumerator Start() {
		var mat = materials;
		if (mat == null || mat.Length == 0) materials = mat = GetComponent<Renderer>().materials;
		
		if (waitTime > 0) yield return new WaitForSeconds(waitTime);
		
		if (replaceShaders) {
			foreach (var i in mat) {
				var replacement = GetReplacementFor(i.shader);
				if (replacement != null) i.shader = replacement;
			}
		}
		
		var materialCount = mat.Length;
		var colorPropertyNamesForAllMaterials = new List<List<string>>(materialCount);
		
		foreach (var m in mat) {
			var colorPropertyNamesForThisMaterial = new List<string>(1);
			colorPropertyNamesForAllMaterials.Add(colorPropertyNamesForThisMaterial);
			
			var shader = m.shader;
			
			if (IsStandardShader(shader)) {
				// Set it to 'Fade' mode:
				m.SetFloat("_Mode", 2);
				
				var material = m;
				
				// From StandardShaderGUI.SetupMaterialWithBlendMode() in Unity's 'Built-in Shaders' package:
				material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
				material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.EnableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
			}
#if UNITY_2018_1_OR_NEWER
			else if (IsLWRPStandardShader(shader)) {
				// These settings are common to the particle shaders and the regular ones:
				m.renderQueue = 3000;
				m.SetOverrideTag("RenderType", "Transparent");
				m.SetFloat("_DstBlend", 10);
				m.SetFloat("_SrcBlend", 5);
				m.SetFloat("_ZWrite", 0);
				m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				
				if (shader.name.Contains("/Particles/")) {
					// Set the 'Rendering Mode' setting to 'Fade':
					m.EnableKeyword("_ALPHABLEND_ON");
					m.DisableKeyword("_ALPHAMODULATE_ON");
					m.DisableKeyword("_ALPHATEST_ON");
					m.SetFloat("_Mode", 2);
					m.SetFloat("_BlendOp", 0);
				} else {
					// Set the 'Surface Type' setting to 'Transparent' and the 'Blending Mode' to 'Alpha':
					m.SetShaderPassEnabled("SHADOWCASTER", false);
					m.SetFloat("_Surface", 1);
				}
			} else if (IsHDRPStandardShader(shader)) {
				// Set the 'Surface Type' setting to 'Transparent' and the 'Blend Mode' to 'Alpha':
				
				var isUnlit = shader.name.Contains("Unlit");
				
				m.renderQueue = 3000;
				m.SetOverrideTag("RenderType", "Transparent");
				m.EnableKeyword("_BLENDMODE_ALPHA");
				if (!isUnlit) m.EnableKeyword("_BLENDMODE_PRESERVE_SPECULAR_LIGHTING");
				m.EnableKeyword("_ENABLE_FOG_ON_TRANSPARENT");
				m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
				m.SetFloat("_DstBlend", 10);
				m.SetFloat("_SurfaceType", 1);
				if (!isUnlit) m.SetFloat("_ZTestDepthEqualForOpaque", 4);
				m.SetFloat("_ZWrite", 0);
			}
#endif
			
			foreach (var candidate in colorPropertyNameCandidates) {
				if (m.HasProperty(candidate)) {
					colorPropertyNamesForThisMaterial.Add(candidate);
				}
			}
		}
		
		for (float t = 0; t < fadeTime; t += Time.deltaTime) {
			var newAlpha = 1 - (t / fadeTime);
			
			for (var materialIndex = 0; materialIndex < materialCount; ++materialIndex) {
				var material = mat[materialIndex];
				var colorPropertyNamesForThisMaterial = colorPropertyNamesForAllMaterials[materialIndex];
				var colorPropertyNamesForThisMaterialCount = colorPropertyNamesForThisMaterial.Count;
				
				for (var colorPropertyNamesForThisMaterialIndex = 0; colorPropertyNamesForThisMaterialIndex < colorPropertyNamesForThisMaterialCount; ++colorPropertyNamesForThisMaterialIndex) {
					var colorPropertyName = colorPropertyNamesForThisMaterial[colorPropertyNamesForThisMaterialIndex];
					var c = material.GetColor(colorPropertyName);
					c.a = newAlpha;
					material.SetColor(colorPropertyName, c);
				}
			}
			yield return null;
		}
		
		SendMessage("FadeCompleted", SendMessageOptions.DontRequireReceiver);
	}

}
