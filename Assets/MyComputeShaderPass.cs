/*
MyComputeShaderPass.cs

This script demonstrates how to create a Custom Pass in Unity's High Definition Render Pipeline (HDRP) 
that uses a Compute Shader to process and render a texture on the camera's output.

How to use:
1. Create a new GameObject in your scene.
2. Add a "Custom Pass Volume" component to the GameObject (GameObject > Volume > Custom Pass Volume).
3. Set the Custom Pass Volume to "Is Global" if you want it to affect all cameras.
4. Add this MyComputeShaderPass script as a Custom Pass in the Custom Pass Volume's list.
5. Assign your Compute Shader to the 'computeShader' field in the inspector.
6. Run the scene; the compute shader will process and render to the camera output in real-time.

Key details:
- The RenderTexture is created and resized dynamically based on the camera's current resolution.
- The script uses 'ctx.hdCamera.camera' to get the correct Camera instance during rendering.
- You can toggle the writing of the processed texture to the camera via the 'enableWriting' boolean.
- Make sure your compute shader has a kernel named "CSMain" and a RWTexture2D named "Result".
*/


using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;

class MyComputeShaderPass : CustomPass
{
    public ComputeShader computeShader;
    public bool active = true;
    private RenderTexture resultTexture;
    private int kernel;

    protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd)
    {
        kernel = computeShader.FindKernel("CSMain");
    }

    protected override void Execute(CustomPassContext ctx)
    {
        if(!active)
            return;

        Camera cam = ctx.hdCamera.camera;

        int width = cam.pixelWidth;
        int height = cam.pixelHeight;

        if (resultTexture == null || resultTexture.width != width || resultTexture.height != height)
        {
            if (resultTexture != null)
                resultTexture.Release();

            resultTexture = new RenderTexture(width, height, 0);
            resultTexture.enableRandomWrite = true;
            resultTexture.Create();
        }

        int threadGroupsX = Mathf.CeilToInt(width / 8f);
        int threadGroupsY = Mathf.CeilToInt(height / 8f);

        computeShader.SetTexture(kernel, "Result", resultTexture);
        computeShader.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);

        ctx.cmd.Blit(resultTexture, ctx.cameraColorBuffer);
    }

    protected override void Cleanup()
    {
        if (resultTexture != null)
            resultTexture.Release();
    }
}
