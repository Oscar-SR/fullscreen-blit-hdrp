# Fullscreen blit HDRP

This project demonstrates how to create a **Custom Pass** in Unity's **High Definition Render Pipeline (HDRP)** that uses a **Compute Shader** to process and render a texture on the camera's output in real-time.

## How to use

1. **Create a GameObject** in your scene.  
2. **Add a `Custom Pass Volume`** component to the GameObject:  
   - Menu path: `GameObject > Volume > Custom Pass Volume`.
3. Enable **“Is Global”** on the Custom Pass Volume if you want it to affect all cameras.
4. **Add the `MyComputeShaderPass.cs` script** as a Custom Pass in the Custom Pass Volume’s list.
5. In the Inspector, **assign your Compute Shader** to the `computeShader` field.
6. **Run the scene** — the compute shader will process and render to the camera output in real-time.

## Notes

- The `RenderTexture` is created and resized automatically based on the camera’s current resolution.

- The script uses `ctx.hdCamera.camera` to access the correct `Camera` instance during rendering.

- You can enable or disable the writing of the processed texture using the `enableWriting` boolean.

- Make sure your compute shader has a kernel named `CSMain` and a RWTexture2D named `Result`.
