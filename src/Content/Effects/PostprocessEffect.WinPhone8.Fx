//-----------------------------------------------------------------------------
// PostprocessEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

// How should the sketch effect respond to changes of brightness in the input scene?
float SketchThreshold = 0.1;
float SketchBrightness = 0.333;

// Randomly offsets the sketch overlay pattern to create a hand-drawn animation effect.
float2 SketchJitter;

// This texture contains the main scene image, which the edge detection
// and/or sketch filter are being applied over the top of.
texture SceneTexture;

sampler SceneSampler : register(s0) = sampler_state
{
    Texture = SceneTexture;
    
    MinFilter = Linear;
    MagFilter = Linear;
    
    AddressU = Clamp;
    AddressV = Clamp;
};


// This texture contains an overlay sketch pattern, used to create the hatched
// pencil drawing effect.
texture SketchTexture;

sampler SketchSampler : register(s2) = sampler_state
{
    Texture = SketchTexture;

    AddressU = Wrap;
    AddressV = Wrap;
};


// Pixel shader applies the edge detection and/or sketch filter postprocessing.
// It is compiled several times using different settings for the uniform boolean
// parameters, producing different optimized versions of the shader depending on
// which combination of processing effects is desired.
float4 PixelShaderFunction(float4 position : SV_Position, float4 color	: COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the original color from the main scene.
    float3 scene = tex2D(SceneSampler, texCoord);
    
    // Adjust the scene color to remove very dark values and increase the contrast.
    float3 saturatedScene = saturate((scene - SketchThreshold) * 2);
        
    // Look up into the sketch pattern overlay texture.
    float3 sketchPattern = tex2D(SketchSampler, texCoord + SketchJitter);
    
    // Convert into negative color space, and combine the scene color with the
    // sketch pattern. We need to do this multiply in negative space to get good
    // looking results, because pencil sketching works by drawing black ink
    // over an initially white page, rather than adding light to an initially
    // black background as would be more common in computer graphics.
    float3 negativeSketch = (1 - saturatedScene) * (1 - sketchPattern);
        
    // Convert the result into a positive color space greyscale value.
    float sketchResult = dot(1 - negativeSketch, SketchBrightness);
        
    // Apply the sketch result to the main scene color.
    scene *= sketchResult;
 
    return float4(scene, 1);
}


// Compile the pixel shader for doing a colored sketch effect without edge detection.
technique ColorSketch
{
    pass P0
    {
        PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
    }
}