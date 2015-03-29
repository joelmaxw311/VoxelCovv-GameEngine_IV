float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;
 
//float4 AmbienceColor = float4(0.1f, 0.1f, 0.1f, 1.0f);
 
// For Diffuse Lightning
//float4x4 WorldInverseTransposeMatrix;
//float3 DiffuseLightDirection = float3(-1.0f, 0.0f, 0.0f);
//float4 DiffuseColor = float4(1.0f, 1.0f, 1.0f, 1.0f);

float3 Camera;
//float4 FogColor = float4(0,0,0,1);
//float FogNear = 2.0f;
//float FogFar = 6.0f;
 
// For Texture
texture ModelTexture;
sampler2D TextureSampler = sampler_state {
    Texture = (ModelTexture);
    MagFilter = Point;
    MinFilter = Point;
    AddressU = Clamp;
    AddressV = Clamp;
};
 
struct VertexShaderInput
{
    float4 Position : POSITION0;
    // For Diffuse Lightning
    //float4 NormalVector : NORMAL0;
	float4 Color : COLOR0;
    // For Texture
    float2 TextureCoordinate : TEXCOORD0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    // For Diffuse Lightning
    float4 VertexColor : COLOR0;
    // For Texture    
    float2 TextureCoordinate : TEXCOORD0;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, WorldMatrix);
    float4 viewPosition = mul(worldPosition, ViewMatrix);
    output.Position = mul(viewPosition, ProjectionMatrix);
 
    // For Diffuse Lightning
    //float4 normal = normalize(mul(input.NormalVector, WorldInverseTransposeMatrix));
    //float lightIntensity = dot(normal, DiffuseLightDirection);

    output.VertexColor = input.Color;	//saturate(DiffuseColor * lightIntensity);

	
	float2 camPlane = {Camera[0], Camera[2]};
	float2 worldPlane = {worldPosition[0], worldPosition[2]};
	float dist = abs(distance(camPlane, worldPlane));
	float height = -cos(dist / 8);
    output.Position[1] += -(degrees(height) / 4.0f) - 14.3f;
	
	//float l = saturate((dist-FogNear)/(FogFar-FogNear));  
    //output.VertexColor = lerp(input.Color, FogColor, l);
	
	//output.VertexColor[3] = 255;
 
    // For Texture
	output.TextureCoordinate = input.TextureCoordinate;
 
    return output;
}
 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // For Texture
	float4 VertexTextureColor = tex2D(TextureSampler, input.TextureCoordinate);
	//VertexTextureColor.a = 1;
 
	return saturate(VertexTextureColor * input.VertexColor);
}
 
technique Texture
{
    pass Pass1
    {
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}