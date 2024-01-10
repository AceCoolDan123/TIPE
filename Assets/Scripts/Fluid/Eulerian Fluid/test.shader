Shader "Custom/test"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
    }
    FallBack "Diffuse"
    float4 PS_ADVECT_MACCORMACK(GS_OUTPUT_FLUIDSIM in, float timestep) : SV_Target {
    // Trace back along the initial characteristic - we'll use
    // values near this semi-Lagrangian "particle" to clamp our final advected value.
    float3 cellVelocity = velocity.Sample(samPointClamp, in.CENTERCELL).xyz;
    float3 npos = in.cellIndex - timestep * cellVelocity;

    // Find the cell corner closest to the "particle" and compute the
    // texture coordinate corresponding to that location.
    npos = floor(npos + float3(0.5f, 0.5f, 0.5f));
    npos = cellIndex2TexCoord(npos);

    // Get the values of nodes that contribute to the interpolated value.
    // Texel centers will be a half-texel away from the cell corner.
    float3 ht = float3(0.5f / textureWidth, 0.5f / textureHeight, 0.5f / textureDepth);
    float4 nodeValues[8];
    nodeValues[0] = phi_n.Sample(samPointClamp, npos + float3(-ht.x, -ht.y, -ht.z));
    nodeValues[1] = phi_n.Sample(samPointClamp, npos + float3(-ht.x, -ht.y, ht.z));
    nodeValues[2] = phi_n.Sample(samPointClamp, npos + float3(-ht.x, ht.y, -ht.z));
    nodeValues[3] = phi_n.Sample(samPointClamp, npos + float3(-ht.x, ht.y, ht.z));
    nodeValues[4] = phi_n.Sample(samPointClamp, npos + float3(ht.x, -ht.y, -ht.z));
    nodeValues[5] = phi_n.Sample(samPointClamp, npos + float3(ht.x, -ht.y, ht.z));
    nodeValues[6] = phi_n.Sample(samPointClamp, npos + float3(ht.x, ht.y, -ht.z));
    nodeValues[7] = phi_n.Sample(samPointClamp, npos + float3(ht.x, ht.y, ht.z));

    // Determine a valid range for the result.
    float4 phiMin = min(min(min(min(min(min(min(
        nodeValues[0], nodeValues[1]), nodeValues[2]), nodeValues[3]),
        nodeValues[4]), nodeValues[5]), nodeValues[6]), nodeValues[7]);

    float4 phiMax = max(max(max(max(max(max(max(
        nodeValues[0], nodeValues[1]), nodeValues[2]), nodeValues[3]),
        nodeValues[4]), nodeValues[5]), nodeValues[6]), nodeValues[7]);

    // Perform final advection, combining values from intermediate
    // advection steps.
    float4 r = phi_n_1_hat.Sample(samLinear, nposTC) +
        0.5 * (phi_n.Sample(samPointClamp, in.CENTERCELL) -
               phi_n_hat.Sample(samPointClamp, in.CENTERCELL));

    // Clamp result to the desired range.
    r = max(min(r, phiMax), phiMin);
    return r;
}

}

