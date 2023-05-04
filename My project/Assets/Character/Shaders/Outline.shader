Shader "Unlit/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(1.0,5.0)) =1.01
    }

    CGINCLUDE 
    #include "UnityCG.cginc"

    struct appdata 
    {
        float4 vertex : POSITION;
        float3 normal : NORMAL;
    }

    struct v2f
    {
        float4 pos : POSITION;

        float3 normal : NORMAL;
    }

    float _OutlineWidth;
    float4 _OutlineColor;

    v2f vert(appdata v)
    {
        v.vertex.xyz *= _Outline;
        v2f 0;
        o.pos= UnityObjectToClipPos(v.vertex);

        return 0;
    }

    ENDCG
    SubShader
    {
        Pass//outline
        {
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            half4 frag(v2f i) : COLOR
            {
                return _OutlineColor;
            }
            ENDCG
        }

        Pass//Normal
        {

        }
    }
}
