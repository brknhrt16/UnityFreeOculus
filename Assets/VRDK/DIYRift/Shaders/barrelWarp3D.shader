
Shader "VRcade/Stereo 3D Barrel Warp" {
Properties {
   _LeftTex ("Left (RGB)", RECT) = "white" {}
   _RightTex ("Right (RGB)", RECT) = "white" {}
   
   _BarrelFactor ("Barrel Factor", Float) = -8.1
   _WarpFactor ("Warping Factor", Float) = 2.0
}

SubShader {
	
	// **** PASS 0 LEFT ****
    Pass {
      ZTest Always Cull Off ZWrite Off
      Fog { Mode off }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma fragmentoption ARB_precision_hint_fastest
      #include "UnityCG.cginc"
      
      uniform sampler2D _LeftTex;
      
      uniform float _BarrelFactor;
      uniform float _WarpFactor;
      
      struct v2f {
         float4 pos : POSITION;
         float2 uv : TEXCOORD0;
      };
      v2f vert( appdata_img v ) {
         v2f o;
         o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
         float2 uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
         o.uv = uv;
         return o;
      }
      half4 frag (v2f i) : COLOR {
         float undoWarp = 1.0 / _WarpFactor;
         
         float2 v = float2(i.uv);
         v *= _WarpFactor; //[0,1-based -> [-1,1]
		 v -= 1.0;
		 float2 warped = float2(
		 _BarrelFactor*v.x/(v.y*v.y + _BarrelFactor),
		 _BarrelFactor*v.y/(v.x*v.x + _BarrelFactor));
		 warped += 1.0; //[-1,1] back into [0,1]
		 warped *= undoWarp;
			 
		 return tex2D(_LeftTex, warped);
      }
      ENDCG
   }
   
   // **** PASS 1 RIGHT ****
   Pass {
      ZTest Always Cull Off ZWrite Off
      Fog { Mode off }

      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma fragmentoption ARB_precision_hint_fastest
      #include "UnityCG.cginc"
      
      uniform sampler2D _RightTex;

      uniform float _BarrelFactor;
      uniform float _WarpFactor;
      
      struct v2f {
         float4 pos : POSITION;
         float2 uv : TEXCOORD0;
      };
      v2f vert( appdata_img v ){
         v2f o;
         o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
         float2 uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
         o.uv = uv;
         return o;
      }
      half4 frag (v2f i) : COLOR{
         float undoWarp = 1.0 / _WarpFactor;
         
         float2 v = float2(i.uv);
         v *= _WarpFactor; //[0,1-based -> [-1,1]
		 v -= 1.0;
		 float2 warped = float2(
		 _BarrelFactor*v.x/(v.y*v.y + _BarrelFactor),
		 _BarrelFactor*v.y/(v.x*v.x + _BarrelFactor));
		 warped += 1.0; //[-1,1] back into [0,1]
		 warped *= undoWarp;
		 
         return tex2D(_RightTex, warped);
      }
      ENDCG
  	}
}   
   Fallback off
} 

