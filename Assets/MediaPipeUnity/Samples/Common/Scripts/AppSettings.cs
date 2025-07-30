// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity.Sample
{
  [Serializable]
  public class AppSettings : ScriptableObject
  {
    [Serializable]
    public enum AssetLoaderType
    {
      StreamingAssets,
      AssetBundle,
      Local,
    }

    [SerializeField] private ImageSourceType _defaultImageSource;
    [SerializeField] private InferenceMode _preferableInferenceMode;
    [SerializeField] private AssetLoaderType _assetLoaderType;
    [SerializeField] private Logger.LogLevel _logLevel = Logger.LogLevel.Debug;

    [Header("Glog settings")]
    [SerializeField] private int _glogMinloglevel = Glog.Minloglevel;
    [SerializeField] private int _glogStderrthreshold = Glog.Stderrthreshold;
    [SerializeField] private int _glogV = Glog.V;

    [Header("WebCam Source")]
    [Tooltip("For the default resolution, the one whose width is closest to this value will be chosen")]

    [SerializeField] private int _preferredDefaultWebCamWidth = 1080;
    [SerializeField] private ImageSource.ResolutionStruct[] _defaultAvailableWebCamResolutions = new ImageSource.ResolutionStruct[] {
      new ImageSource.ResolutionStruct(640, 640, 60),
      new ImageSource.ResolutionStruct(1080, 1080, 60),
    };

    public ImageSourceType defaultImageSource => _defaultImageSource;
    public InferenceMode preferableInferenceMode => _preferableInferenceMode;
    public AssetLoaderType assetLoaderType
    {
      get => _assetLoaderType;
      set => _assetLoaderType = value;
    }
    public Logger.LogLevel logLevel => _logLevel;

    public void ResetGlogFlags()
    {
      Glog.Logtostderr = true;
      Glog.Minloglevel = _glogMinloglevel;
      Glog.Stderrthreshold = _glogStderrthreshold;
      Glog.V = _glogV;
    }

    public WebCamSource BuildWebCamSource() => new WebCamSource(
      _preferredDefaultWebCamWidth,
      _defaultAvailableWebCamResolutions
    );
  }
}
