using UnityEngine;

namespace GalleryVr
{
    public static class TextureExtension
    {
        public static Texture2D RotateTexture(this Texture2D originalTexture, bool clockwise)
        {
            Color32[] original = originalTexture.GetPixels32();
            Color32[] rotated = new Color32[original.Length];
            int textureWidth = originalTexture.width;
            int textureHeight = originalTexture.height;

            for (int heightIndex = 0; heightIndex < textureHeight; ++heightIndex)
            {
                for (int widthIndex = 0; widthIndex < textureWidth; ++widthIndex)
                {
                    int rotatedIndex = (widthIndex + 1) * textureHeight - heightIndex - 1;
                    int originalIndex = clockwise ? original.Length - 1 - (heightIndex * textureWidth + widthIndex) : heightIndex * textureWidth + widthIndex;
                    rotated[rotatedIndex] = original[originalIndex];
                }
            }

            Texture2D rotatedTexture = new Texture2D(textureHeight, textureWidth);
            rotatedTexture.SetPixels32(rotated);
            rotatedTexture.Apply();
            return rotatedTexture;
        }

        public static void ResizeTexture2D(this Texture2D originalTexture, int width, int height)
        {
            // Ensure the original texture is readable
            if (!originalTexture.isReadable)
            {
                Debug.LogError("Texture is not readable");
                return;
            }

            Texture2D resizedTexture = new Texture2D(width, height);

            // Calculate step sizes for sampling pixels
            float stepX = (float) originalTexture.width / width;
            float stepY = (float) originalTexture.height / height;

            // Loop through each pixel in the resized texture
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Sample the color from the source texture using nearest-neighbor
                    Color sampledColor = originalTexture.GetPixelBilinear((float) x / width, (float) y / height);

                    // Assign the sampled color to the resized texture
                    resizedTexture.SetPixel(x, y, sampledColor);
                }
            }

            resizedTexture.Apply();
        }
    }
}