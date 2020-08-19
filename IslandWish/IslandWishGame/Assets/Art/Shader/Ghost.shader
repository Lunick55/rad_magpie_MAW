Shader "Ghost"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _MainTex ("Color and Alpha", 2D) = "white" 
    }
    Category 
    {
        Lighting On
        Zwrite Off
        Cull Back
        Blend SrcAlpha OneMinusSrcAlpha

        Tags {Queue = Transparent}

        Subshader 
        {
            Material 
            {
                Emission[_Color]
            }
            Pass 
            {
                SetTexture [_MainTex] 
                {
                    Combine Texture * Primary, Texture * Primary
                }
            }
        }
    }
}
