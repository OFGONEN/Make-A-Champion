Shader "Unlit/FakeLiquid"
{
    Properties
    {
        _Tint           ( "Tint",            Color              ) = ( 1, 1, 1, 1 )
        _MainTex        ( "Texture",         2D                 ) = "white" {}
        // Range here is arbitrary; Actual range depends on height of object.
        _FillAmount     ( "Fill Amount",     Range( -3, 3 )     ) = 0 
		[HideInInspector]_WobbleX ( "WobbleX", Range( -1, 1 ) ) = 0.0
		[HideInInspector]_WobbleZ ( "WobbleZ", Range( -1, 1 ) ) = 0.0
        _TopColor       ( "Top Color",       Color              ) = ( 1, 1, 1, 1 )
		_FoamColor      ( "Foam Line Color", Color              ) = ( 1, 1, 1, 1 )
        _FoamThickness  ( "Foam Thickness",  Range( 0.0, 0.1 )  ) = 0.0    
		_RimColor       ( "Rim Color",       Color              ) = ( 1, 1, 1, 1 )
	    _RimPower       ( "Rim Power",       Range( 0,10 )      ) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Geometry" }
        LOD 100
        
        Pass
        {
            ZWrite On
            Cull Off // Need both faces.
            AlphaToMask On // For transparency.
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag

            // Make fog work
            //#pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct VertexOutput
            {
                float2 uv       : TEXCOORD0;
                //UNITY_FOG_COORDS( 1 )
                float4 vertex   : SV_POSITION;
                float3 viewDir  : COLOR0;
                float3 normal   : COLOR1;
                float  fillEdge : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            
            float4    _Tint;
            float     _RimPower;
            float4    _RimColor;
            float     _FoamThickness;
            float     _FillAmount;
            float4    _TopColor;
            float4    _FoamColor;

            VertexOutput vert( VertexInput input )
            {
                VertexOutput output;
                
                output.vertex   = UnityObjectToClipPos( input.vertex );
                output.uv       = TRANSFORM_TEX( input.uv, _MainTex );
                //UNITY_TRANSFER_FOG( output, output.vertex );
                
                float3 worldPos = mul( unity_ObjectToWorld, input.vertex.xyz );
                output.fillEdge = worldPos.y + ( -1 * _FillAmount );
                
                output.viewDir = normalize( ObjSpaceViewDir( input.vertex ) );
                output.normal  = input.normal;

                return output;
            }

            //fixed4 frag( VertexOutput input, bool facingFront : SV_ISFRONTFACE ) : SV_Target
            fixed4 frag( VertexOutput input, fixed facing : VFACE ) : SV_Target
            {
                fixed4 color = tex2D( _MainTex, input.uv ) * _Tint;
                
                //UNITY_APPLY_FOG( input.fogCoord, color );
		   
                // Rim lighting:
                float dotProduct = 1 - pow( dot( input.normal, input.viewDir ), _RimPower );
                float4 rimResult = smoothstep( 0.5, 1.0, dotProduct );
                rimResult *= _RimColor;

                // Foam edge:
                float4 foam = ( step( input.fillEdge, 0.5 ) - step( input.fillEdge, ( 0.5 - _FoamThickness ) ) );
                float4 foamColored = foam * ( _FoamColor * 0.9 );
                
                // Rest of the liquid:
                float4 restOfLiquid = step( input.fillEdge, 0.5 ) - foam;
                float4 restOfLiquidColored = restOfLiquid * color;
                
                float4 finalResult = restOfLiquidColored + foamColored;
                finalResult.rgb += rimResult;
                
                float4 topColor = _TopColor * ( foam + restOfLiquid );
                
                //return finalResult;
                return facing > 0 ? finalResult : topColor;
                //return facingFront ? finalResult : topColor;
            }
            ENDCG
        }
    }
}
