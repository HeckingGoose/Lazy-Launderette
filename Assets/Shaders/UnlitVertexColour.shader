Shader "LL/Unlit Vertex Colour"
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

        #include "UnityCG.cginc"

        struct v2f
        {
            float4 position : SV_POSITION;
            float3 normal : NORMAL;
            half4 colour : COLOR0;
        };

        ENDCG

        // Draw Pass
        Pass
        {
            CGPROGRAM


            v2f vert(
                float4 position : POSITION,
                float3 normal : NORMAL,
                half4 colour : COLOR0
            )
            {
                // Declare output
                v2f output;

                // Pass in values
                output.position = UnityObjectToClipPos(position);
                output.normal = normalize(normal);
                output.colour = colour;

                // Return output
                return output;
            }

            half4 frag (v2f input) : SV_Target
            {
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