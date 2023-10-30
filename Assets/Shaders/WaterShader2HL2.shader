// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Romina/WaterShaderTransparentHL"
{
    Properties
    {
		_Color ("Color", Color) = (1,1,1,1)
        _TopColor ("Top Color", Color) = (1,1,1,1)
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
        _FillAmount ("Fill Amount", Range(0,1)) = 0.5
		_WaterNormal ("Water Normal", vector) = (0,1,0,0)
		[Enum(Off, 0, On, 1)] _ZWrite("Z Write", Float) = 0
		[Enum(Off, 0, On, 1)] _AlphaToMask("Alpha To Mask", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend", Float) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend", Float) = 0
    }
 
    SubShader
    {
		/***********************************
		Tags:
		https://docs.unity3d.com/Manual/SL-SubShaderTags.html
		you can access these in the c# file:
		   Renderer myRenderer = GetComponent<Renderer>();
           string tagValue = myRenderer.material.GetTag(ExampleTagName, true, "Tag not found");

		can have custom Tags

		about these tags:

		"Queue": the order of rendering
		https://docs.unity3d.com/ScriptReference/Rendering.RenderQueue.html

		"DisableBatching" is true because the shader uses object space operations
		************************************/

		Tags {"Queue" = "Transparent" }//"RenderType" = "Transparent" "DisableBatching" = "True" }

        Pass
        {
		 ZWrite [_ZWrite] // ? sets whether the depth buffer contents are updated during rendering normally ZWrite is enabled for opaque objects and disabled for semi-transparent ones
		 //Ztest LEqual //ztest does not run on the current object, so if culling is off and ztest is LEqual, the back facing tirangles are renderd on top(?)
		 Cull [_Cull] // we want the front and back faces, Culling is an optimization that does not render polygons facing away from the viewer. All polygons have a front and a back side.
		 AlphaToMask [_AlphaToMask] // transparency on top (?)
		 Blend [_SrcBlend] [_DstBlend]

         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"
 
         struct appdata
         {
           float4 vertex : POSITION;
		   float3 normal : NORMAL;
		   UNITY_VERTEX_INPUT_INSTANCE_ID
         };
 
         struct v2f
         {
            float4 vertex : SV_POSITION;
			float3 normal : NORMAL;
			float3 viewDir : TEXCOORD1;
			float fillEdge: TEXCOORD0;

			UNITY_VERTEX_INPUT_INSTANCE_ID
		    UNITY_VERTEX_OUTPUT_STEREO
		};
 
		 float _FillAmount;
         float4 _TopColor, _Color, _WaterNormal, _EdgeColor;
		

         v2f vert (appdata v)
         {
            v2f o;

			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_TRANSFER_INSTANCE_ID(v, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			
            o.vertex = UnityObjectToClipPos(v.vertex); //equals to o.vertex = mul(mul(mul(UNITY_MATRIX_P, UNITY_MATRIX_V), unity_ObjectToWorld),v.vertex); but use this because weird stuff happens in AR, prbably
			o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
			float4 normal4 = float4(v.normal, 0.0);
			o.normal = normalize(mul(normal4, unity_WorldToObject).xyz);
			//o.normal = v.normal;
			o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);

			float l = (_FillAmount*2-1)* length(unity_ObjectToWorld._m01_m11_m21);
			float3 localPointOnWaterPlane = mul(unity_ObjectToWorld,float4(0,l,0,1));//TODO
			float4 wn = normalize (_WaterNormal);
			//float3 wn = normalize( float3( cos(radians(_WaterNormalRotationX)),cos(radians(_WaterNormalRotationY)), cos(radians(_WaterNormalRotationZ)) ) );
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			o.fillEdge = dot((worldPos.xyz-localPointOnWaterPlane), wn.xyz);
            return o;
         }

		 //UNITY_DECLARE_SCREENSPACE_TEXTURE(_MainTex);

         fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target
         {
		   //UNITY_SETUP_INSTANCE_ID(i);
		   //fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.uv);
		   i.normal = facing > 0 ? i.normal : -i.normal;

		   // close to edge = closer to 0
		   // far from edge = closer to 1
		   float edgeFactor = abs(dot(i.viewDir, i.normal));
		   float oneMinusEdge = 1.0 - edgeFactor;

		   float result = step(i.fillEdge, 0);
		   return result *( (facing > 0 ?  _Color : _TopColor)- _EdgeColor*oneMinusEdge);
		   //return result * _Color;
		   //return i.fillEdge;
         }
         ENDCG
        }
    }
}