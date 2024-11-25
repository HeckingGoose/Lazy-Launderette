Shader "Lazy Launderette/Textured Cutout"
{
    Properties
    {
        _MainTex("Albedo", 2D) = "white" {}
        _Alpha("Alpha", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Cull Off
        LOD 200

        // Include
        CGINCLUDE

        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_instancing
        #pragma multi_compile_shadowcaster

        #include "UnityCG.cginc"

        struct v2f
        {
            float4 position : POSITION;
            float2 uv : TEXCOORD3;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        sampler2D _MainTex;
        float _Alpha;

        ENDCG

        // Draw Pass
        Pass
        {
            CGPROGRAM


            v2f vert(appdata_base v)
            {
                // Declare output
                v2f output;

                // Handle instancing stuff
                UNITY_SETUP_INSTANCE_ID(vert);
                UNITY_TRANSFER_INSTANCE_ID(vert, output);

                // Pass in values
                output.position = UnityObjectToClipPos(v.vertex);
                output.uv = v.texcoord.xy;

                // Return output
                return output;
            }

            half4 frag (v2f input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Declare output
                half4 output;

                // Pass in vertex colour
                output = tex2D(_MainTex, input.uv);

                // Clip to alpha
                clip(output.a - _Alpha);

                // Return output
                return output;
            }

            ENDCG
        }
    }
}