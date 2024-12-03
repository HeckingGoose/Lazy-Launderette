Shader "Lazy Launderette/Washing Machine"
{
    Properties
    {
        _MainTex("Machine Window", 2D) = "white" {}
        _ContentsTex("Contents Texture", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Cull back
        LOD 200

        // Include
        CGINCLUDE

        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_instancing
        #pragma multi_compile_shadowcaster

        #include "UnityCG.cginc"

        struct appdata
        {
            float4 vertex : POSITION;
            float2 texcoord : TEXCOORD0;
            float2 texcoord1 : TEXCOORD1;
            half4 color : COLOR0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };
        struct v2f
        {
            float4 position : POSITION;
            float2 uv : TEXCOORD0;
            float2 uv1 : TEXCOORD1;
            half4 colour : COLOR0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        // Declare all properties in one buffer
        CBUFFER_START(UnityPerMaterial)
        sampler2D _MainTex;
        sampler2D _ContentsTex;
        float _Alpha;
        CBUFFER_END

        ENDCG

        // Draw Pass
        Pass
        {
            CGPROGRAM


            v2f vert(appdata v)
            {
                // Declare output
                v2f output;

                // Handle instancing stuff
                UNITY_SETUP_INSTANCE_ID(vert);
                UNITY_TRANSFER_INSTANCE_ID(vert, output);

                // Pass in values
                output.position = UnityObjectToClipPos(v.vertex);
                output.uv = v.texcoord.xy;
                output.uv1 = v.texcoord1.xy;
                output.colour = v.color;

                // Return output
                return output;
            }

            half4 frag (v2f input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Declare output
                half4 output;

                // Pass in texture colour from UV0
                output = tex2D(_MainTex, input.uv);

                // Multiply by colour from UV1
                output *= tex2D(_ContentsTex, input.uv1);

                // Multiply by vertex colour
                output *= input.colour;

                // Clip to alpha
                clip(output.a - _Alpha);

                // Return output
                return output;
            }

            ENDCG
        }
    }
}