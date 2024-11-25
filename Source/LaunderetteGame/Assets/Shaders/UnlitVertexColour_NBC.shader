Shader "Lazy Launderette/Unlit Vertex Colour (Double Sided)"
{
    Properties
    {

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

        #include "UnityCG.cginc"

        struct v2f
        {
            float4 position : POSITION;
            float3 normal : NORMAL;
            half4 colour : COLOR0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        ENDCG

        // Draw Pass
        Pass
        {
            CGPROGRAM


            v2f vert(
                v2f vert
            )
            {
                // Declare output
                v2f output;

                // Handle instancing stuff
                UNITY_SETUP_INSTANCE_ID(vert);
                UNITY_TRANSFER_INSTANCE_ID(vert, output);

                // Pass in values
                output.position = UnityObjectToClipPos(vert.position);
                output.normal = normalize(vert.normal);
                output.colour = vert.colour;

                // Return output
                return output;
            }

            half4 frag (v2f input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Declare output
                half4 output;

                // Pass in vertex colour
                output = input.colour;

                // Return output
                return output;
            }

            ENDCG
        }
    }
}