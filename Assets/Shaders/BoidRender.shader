Shader "Boids/BoidRender"
{
	// Material properties adapted from SoftSurface.shader
    Properties
    {
        _Color ("Color", Color) = (0.4,0.4,0.4,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Emission ("Emission", Range (0,1)) = 0.5
    }
	SubShader
	{
        Tags { "RenderType"="Opaque" }
        LOD 100

		Pass
		{
			CGPROGRAM

			#pragma vertex vert // runs on every vertex
			#pragma fragment frag // runs on every pixel
			#pragma target 3.0

			#include "UnityCG.cginc"

			// Boid data from the compute shader
			struct BoidModel
			{
				float3 position;
				float3 direction;
			};
			StructuredBuffer<BoidModel> boids;

			// Material properties adapted from SoftSurface.shader
			sampler2D _MainTex;
			float4 _MainTex_ST; // a voir
			fixed4 _Color;
			fixed _Emission;

			// Mesh
			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			// A way to pass data from the vertex shader to the fragment shader
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD1;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v, uint id : SV_InstanceID)
			{
				v2f o; // o = output
				BoidModel boid = boids[id];

				// Calculate the right, up, and forward vectors based on the boid's direction
				float3 forward = normalize(boid.direction);
				float3 up = float3(0, 1, 0); // Assuming Y-up world
				float3 right = cross(up, forward);
				up = normalize(cross(forward, right)); 

				// Transform the mesh vertices based on the boid's position
				float3 boidPosition = v.vertex.xyz + boid.position;
				o.vertex = UnityObjectToClipPos(boidPosition);

				// uv
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				// normal
				o.normal = right * v.normal.x +
					up * v.normal.y +
					forward * v.normal.z;	
				
				return o;
			}

			fixed4 frag (v2f i) : SV_Target // i = input
			{
				/*fixed4 col = 1;
				return col;*/
				return tex2D(_MainTex, i.uv) * _Color * _Emission;
			}
			ENDCG
		}
	}
}