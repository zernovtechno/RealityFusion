// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using UnityEngine.UI;
using RealityFusion.API;
namespace Mediapipe.Unity
{
  public class Screen : MonoBehaviour
  {
        [SerializeField] public RawImage _screen;
        [SerializeField] public RawImage Left_image;
        [SerializeField] public RawImage Right_image;
        [SerializeField] public EasyAPI _EasyAPI;

        private ImageSource _imageSource;

        public Texture texture
        {
            get => Left_image.texture;
            set { Left_image.texture = value; Right_image.texture = value; }
        }

        public UnityEngine.Rect uvRect
    {
      set => _screen.uvRect = value;
    }

    public void Initialize(ImageSource imageSource)
    {
      _imageSource = imageSource;

      Resize(_imageSource.textureWidth, _imageSource.textureHeight);
      Rotate(_imageSource.rotation.Reverse());
      ResetUvRect(RunningMode.Async);
      texture = imageSource.GetCurrentTexture();
      _EasyAPI.PutTextures(imageSource.GetCurrentWebCamTexture(), imageSource.GetCurrentTexture());
    }

    public void Resize(int width, int height)
    {
      _screen.rectTransform.sizeDelta = new Vector2(width, height);
    }

    public void Rotate(RotationAngle rotationAngle)
    {
      _screen.rectTransform.localEulerAngles = rotationAngle.GetEulerAngles();
    }

    public void ReadSync(Experimental.TextureFrame textureFrame)
    {
      if (!(texture is Texture2D))
      {
        texture = new Texture2D(_imageSource.textureWidth, _imageSource.textureHeight, TextureFormat.RGBA32, false);
        ResetUvRect(RunningMode.Sync);
      }
      textureFrame.CopyTexture(texture);
    }

    private void ResetUvRect(RunningMode runningMode)
    {
      var rect = new UnityEngine.Rect(0, 0, 1, 1);

      if (_imageSource.isVerticallyFlipped && runningMode == RunningMode.Async)
      {
        // In Async mode, we don't need to flip the screen vertically since the image will be copied on CPU.
        rect = FlipVertically(rect);
      }

      if (_imageSource.isFrontFacing)
      {
        // Flip the image (not the screen) horizontally.
        // It should be taken into account that the image will be rotated later.
        var rotation = _imageSource.rotation;

        if (rotation == RotationAngle.Rotation0 || rotation == RotationAngle.Rotation180)
        {
          rect = FlipHorizontally(rect);
        }
        else
        {
          rect = FlipVertically(rect);
        }
      }

      uvRect = rect;
    }

    private UnityEngine.Rect FlipHorizontally(UnityEngine.Rect rect)
    {
      return new UnityEngine.Rect(1 - rect.x, rect.y, -rect.width, rect.height);
    }

    private UnityEngine.Rect FlipVertically(UnityEngine.Rect rect)
    {
      return new UnityEngine.Rect(rect.x, 1 - rect.y, rect.width, -rect.height);
    }
  }
}
