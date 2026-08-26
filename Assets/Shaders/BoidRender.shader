Shader "Boids/BoidRender"
{
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert // runs on every vertex
			#pragma fragment frag // runs on every pixel

			#include "UnityCG.cginc"

			// Mesh
			struct appdata
			{
				float4 vertex : POSITION;
			};

			// A way to pass data from the vertex shader to the fragment shader
			struct v2f
			{
				float4 vertex : SV_POSITION
			};

			struct BoidModel
			{
				float3 position;
				float3 direction;
			};

			StructuredBuffer<BoidModel> boids;

			v2f vert(appdata v, uint id : SV_InstanceID)
			{
				v2f o; // o = output
				BoidModel boid = boids[id];

				float3 boidPosition = v.vertex.xyz + boid.position;
				o.vertex = UnityObjectToClipPos(boidPosition);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target // i = input
			{
				fixed4 col = 1;
				return col;
			}
			
			ENDCG
		}
	}
}