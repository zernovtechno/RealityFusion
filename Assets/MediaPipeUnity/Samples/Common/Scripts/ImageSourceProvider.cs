// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity.Sample
{
  public static class ImageSourceProvider
  {
    private static WebCamSource _WebCamSource;

    public static ImageSource ImageSource { get; private set; }

    public static ImageSourceType CurrentSourceType
    {
      get
      {
        return ImageSourceType.WebCamera;
      }
    }

    internal static void Initialize(WebCamSource webCamSource)
    {
      _WebCamSource = webCamSource;
    }

    public static void Switch(ImageSourceType imageSourceType)
    {
      ImageSource = _WebCamSource;
    }
  }
}
